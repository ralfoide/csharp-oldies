//*******************************************************************
/*

	Solution:	Xeres
	Project:	Device
	File:		RBufferVideo.cs

	Copyright 2005, Raphael MOLL.

	This file is part of Xeres.

	Xeres is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Xeres is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Xeres; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************



using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using Alfray.LibUtils.Buffers;

//*************************************
namespace Alfray.Xeres.Device
{
	//***************************************************
	/// <summary>
	/// RBufferVideo is a specialized version of RBuffer
	/// for the purpose of storing data associated with an
	/// image.
	/// 
	/// Some specific metadata such as Bounds, MimeType
	/// and TimeStamp is made available by helper methods
	/// that directly manipulate the underlying metadata
	/// dictionnary.
	/// 
	/// The same provisions than for RBuffer apply regarding
	/// exclusive vs. concurrent usage.
	/// </summary>
	//***************************************************
	public class RBufferVideo: RBuffer
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------



		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------

		//*********************
		/// <summary>
		/// Gets or sets the bounds of the video buffer, that is
		/// its location and width.
		/// </summary>
		//*********************
		public Rectangle Bounds
		{
			get
			{
				return (Rectangle)(Metadata[Key.kBounds]);
			}
			set
			{
				Metadata[Key.kBounds] = value;
			}
		}


		//*********************
		/// <summary>
		/// Gets or sets the bounds of the video buffer, that is
		/// its location and width.
		/// </summary>
		//*********************
		public string MimeType
		{
			get
			{
				return (string)(Metadata[Key.kMimeType]);
			}
			set
			{
				Metadata[Key.kMimeType] = value;
			}
		}


		//***********************
		/// <summary>
		/// Gets or set the time stamp of the video buffer.
		/// </summary>
		//***********************
		public DateTime	TimeStamp
		{
			get
			{
				return new DateTime((long)(Metadata[Key.kTimeStamp]));
			}
			set
			{
				Metadata[Key.kTimeStamp] = value.Ticks;
			}
		}




		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//*******************************
		/// <summary>
		/// Construct an empty video buffer.
		/// Bounds is set to Rectangle.Empty, data is
		/// set to null and time stamp is set to DateTime.Now
		/// </summary>
		//*******************************
		public RBufferVideo() 
			: this(Rectangle.Empty, "", null)
		{
		}

		//*******************************************
		/// <summary>
		/// Construct a new video buffer with the specified
		/// boundaries, mimetype and data buffer.
		/// Time stamp is set to DateTime.Now.
		/// </summary>
		//*******************************************
		public RBufferVideo(Rectangle bounds, string mimetype, byte[] data)
			: this(bounds, mimetype, data, DateTime.Now)
		{
		}

		//***************************************************************
		/// <summary>
		/// Construct a new video buffer with the specified
		/// boundaries, format, data buffer ane timestamp.
		/// </summary>
		//***************************************************************
		public RBufferVideo(Rectangle bounds, string mimetype, byte[] data, DateTime timeStamp)
			: base(data)
		{
			MimeType = mimetype;
			Bounds = bounds;
			TimeStamp = timeStamp;
		}

		
		//***********************************************
		/// <summary>
		/// This helper static method creates a new RBufferVideo
		/// that contains the data from the given Drawing.Bitmap.
		/// The data is extracted as 32 bpp RGB data.
		/// The width is adjusted to a multiple of 4 pixels.
		/// 
		/// Pixel data in memory is stored as 0x00RRGGBB.
		/// This means that when reading byte per byte, you get
		/// [0]=BB , [1]=GG, [2]=RR, [3]=00 (A) on an Intel little-endian CPU!
		/// </summary>
		//***********************************************
		public static RBufferVideo FromBitmap(Bitmap bmp)
		{
			int w = bmp.Width;
			int h = bmp.Height;

			// Webcams usually return a width which is a multiple of 4
			// even QCIF (174x144) or CIF (352x288).
			// If it is not, we purposedly arrange to get the smallest
			// multiple of 4 width.

			if (w % 4 != 0)
				w -= w % 4;

			Rectangle bounds = new Rectangle(0, 0, w, h);

			// get all bits as 24 bpp
			BitmapData bd = bmp.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppRgb);

			byte[] dest = new byte[w * h * 4];

			unsafe
			{
				fixed(byte * d0 = dest)
				{
					uint *d = (uint *)d0;
					byte *s = (byte *) bd.Scan0.ToPointer();

					// We're copying by int32 increment i.e. 4 bytes
					// Each pixel is 4 bytes so each Int32 copies one pixel.
					// The source stride may differ from the data raw byte size though.

					int stride = bd.Stride;

					for(; h > 0; h--)
					{
						uint *s1 = (uint *)s;

						for(int i = w; i > 0; i--)
						{
							// discard the last byte (A in "RGBA")
							*(d++) = *(s1++) & (uint)0x00FFFFFF;
						}

						s += stride;
					} // for
				} // fixed
			} // unsafe

			bmp.UnlockBits(bd);

			// create new buffer
			RBufferVideo rb = new RBufferVideo(bounds, "rgba/32", dest);

			return rb;
		}


		public static RBufferVideo JpegFromBitmap(Bitmap b)
		{
			try
			{
				MemoryStream ms = new MemoryStream();


				// get the Jpeg encoder the first time
				if (mJpegEncoder == null)
				{
					ImageCodecInfo[] infos = ImageCodecInfo.GetImageEncoders();
					foreach(ImageCodecInfo info in infos)
					{
						if (info.FormatDescription == "JPEG")
						{
							mJpegEncoder = info;
							break;
						}
					}
				}

				// Set JPEG quality to 40%.
				EncoderParameters eps = new EncoderParameters(1);
				eps.Param[0] = new EncoderParameter(Encoder.Quality, 50L);

				b.Save(ms, mJpegEncoder, eps);

				// b.Save(ms, ImageFormat.Jpeg);

				ms.Close(); // will flush
				byte[] buf = ms.GetBuffer();

				// create new buffer
				
				Rectangle bounds = new Rectangle(0, 0, b.Size.Width, b.Size.Height);

				RBufferVideo rb = new RBufferVideo(bounds, "image/jpeg", buf);

				// dispose stuff
				eps.Param[0].Dispose();
				eps.Dispose();

				return rb;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return null;
		}


		public static Image ImageFromJpeg(RBufferVideo rb)
		{
			try
			{
				if (rb.Data != null && rb.MimeType == "image/jpeg")
				{
					byte[] buf = rb.Data;
					MemoryStream ms = new MemoryStream(buf);

					Image b = Image.FromStream(ms);

					ms.Close();

					return b;
				}
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}

			return null;
		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private static ImageCodecInfo mJpegEncoder = null;


	} // class RBufferVideo
} // namespace Alfray.Xeres.Device


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RBufferVideo.cs,v $
//	Revision 1.8  2005/04/30 22:41:30  ralf
//	Rebuilding own VideoCaptureNet project
//	Using separate LibUtils & LibUtilsTests
//	
//	Revision 1.7  2005/03/28 00:22:15  ralf
//	Minor fixes
//	
//	Revision 1.6  2005/03/24 15:32:17  ralf
//	Added RPlayerVideo
//	
//	Revision 1.5  2005/03/23 06:31:19  ralf
//	Using RBuffer, RIBuffer, RISender, RIReceiver and RSenderBase.
//	
//	Revision 1.4  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.3  2005/03/10 22:10:20  ralf
//	Quick test with JPEG encoding
//	
//	Revision 1.2  2005/03/09 20:02:39  ralf
//	Added Format to RBufferVideo
//	Added FromBitmap to RBufferVideo
//	
//	Revision 1.1  2005/02/28 01:15:49  ralf
//	Added RBufferVideo
//	
//	
//---------------------------------------------------------------
