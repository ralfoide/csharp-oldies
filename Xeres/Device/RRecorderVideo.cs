//*******************************************************************
/*

	Solution:	Xeres
	Project:	Device
	File:		RRecorderVideo.cs

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
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;

using Alfray.LibUtils.Misc;
using Alfray.LibUtils.Buffers;

using Allenwood.VideoCaptureNet;

//*************************************
namespace Alfray.Xeres.Device
{
	//***************************************************
	/// <summary>
	/// RRecorderVideo allows recording of DirectX video
	/// by relying on the GPLed VideoCaptureNET library.
	/// </summary>
	//***************************************************
	public class RRecorderVideo: RSenderBase, RIRecorder
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------

		public const int kQueueSize = 2;

		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------

		static public System.Diagnostics.BooleanSwitch NUnitTesting = new System.Diagnostics.BooleanSwitch("NUnitTesting", "Operating under NUnit testing");

		//******************************
		/// <summary>
		/// Returns the currently selected device.
		/// Returns null when no device is selected.
		/// When set, change the device. Affect null to deselect device.
		/// When set, a similar device must be found in the existing enumerated
		/// device list, otherwise no change is made. 
		/// </summary>
		//******************************
		public RDeviceInfo CurrentDevice
		{
			get
			{
				return mCurrentDevice;
			}
			set
			{
				// Assert new device is null (no device selected)
				// or is in list.
				if (!NUnitTesting.Enabled)
					System.Diagnostics.Debug.Assert(value == null || (mDevices != null && hasDevice(value)));
				if (mCurrentDevice != value
					&& (value == null || (mDevices != null && hasDevice(value))))
				{
					mCurrentDevice = value;
					log(String.Format("Current Video Device = '{0}' (changed)", mCurrentDevice != null ? mCurrentDevice.Display : "(null)"));
				}
			}
		}


		//****************************
		/// <summary>
		/// Indicates how many buffers the recorder received so far
		/// from the selected device when running. This is only for
		/// stat purposes and does not represent the number of buffers
		/// available in BufferQueue since obsoletes buffers in the queue
		/// are automatically deleted.
		/// </summary>
		//****************************
		public long StatBuffersReceived
		{
			get
			{
				return mStatFps.Count;
			}
		}


		//************
		/// <summary>
		/// Indicates the average fps of the recorder so far.
		/// The information is reset when the recorder starts
		/// or resumes after a pause. An fps of zero means that
		/// the average is N/A yet and is mostly irrelevant.
		/// </summary>
		//************
		public double StatFps
		{
			get
			{
				return mStatFps.AvgPerSec;
			}
		}


		//*****************
		/// <summary>
		/// Set the logger for the recorder.
		/// Ideally the logger should be asynchronous and not directly
		/// update any WinForm UI.
		/// </summary>
		//*****************
		public RILog Logger
		{
			get
			{
				return mLog;
			}
			set
			{
				mLog = value;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//*********************
		/// <summary>
		/// Initializes the recorder.
		/// Sets the underlying sender queue to only contain
		/// a maximum of kQueueSize buffers (2 actually)
		/// </summary>
		//*********************
		public RRecorderVideo(): base(kQueueSize)
		{
		}

		#region RIRecorder Members

		//*************************************
		/// <summary>
		/// Enumerate all video-capture device presents.
		/// The returned array can be empty but it cannot be a null pointer.
		/// </summary>
		/// <returns>An array (maybe of length zero but not a null ptr)</returns>
		//*************************************
		public RDeviceInfo[] EnumerateDevices()
		{
			if (mDevices == null)
			{
				// memorize existing current device unique id if possible

				string curr_dev_id = (mCurrentDevice != null ? mCurrentDevice.UniqueId : "");

				// remove current device

				mCurrentDevice = null;

				// get list of devices

				VideoCaptureDeviceDesc[] d = null;
				
				try
				{
					d = VideoCaptureDeviceDesc.GetAvailableDeviceDescs();
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex, "RRecorderVideo.EnumerateDevices");
					log("RRecorderVideo.EnumerateDevices" + ex.Message);
				}

				mDevices = new RDeviceInfo[d != null ? d.Length : 0];

				for(int i = 0; i < mDevices.Length; i++)
				{
					mDevices[i] = new RDeviceInfo(d[i].ToString(), d[i].DevicePath, d[i]);
					log(String.Format("Enum Video Device[{0}] = '{1}'", i, mDevices[i].Display));
				}

				// reset the current device, either the previous current one
				// or the default from VideoCaptureDeviceDesc if any

				if (mDevices.Length > 0)
				{
					if (curr_dev_id == "")
					{
						VideoCaptureDeviceDesc def = null;
						
						try
						{
							def = VideoCaptureDeviceDesc.GetDefaultDeviceDesc();
						}
						catch(Exception ex)
						{
							System.Diagnostics.Debug.WriteLine(ex, "RRecorderVideo.EnumerateDevices");
							log("RRecorderVideo.EnumerateDevices" + ex.Message);
						}

						if (def != null)
							curr_dev_id = def.DevicePath;
					}

					// find it in list
					if (curr_dev_id != "")
					{
						foreach(RDeviceInfo i in mDevices)
						{
							if (i.UniqueId == curr_dev_id)
							{
								mCurrentDevice = i;
								log(String.Format("Current Video Device = '{0}' (init)", mCurrentDevice.Display));
								break;
							}
						}
					}
				}
			}

			return mDevices;
		}


		//*****************
		/// <summary>
		/// Starts the device.
		/// It will start accumulating buffers in the buffer queue.
		/// Does nothing if there is no currently selected device.
		/// </summary>
		//*****************
		public void Start()
		{
			Stop();

			System.Diagnostics.Debug.Assert(mVCDevice == null);
			System.Diagnostics.Debug.Assert(mCurrentDevice != null);

			// reset fps counter
			mStatFps = new RFreqCounter();

			if (mCurrentDevice != null)
			{
				try
				{
					try
					{
						mVCDevice = new VideoCaptureDevice(mCurrentDevice.Device as VideoCaptureDeviceDesc);
						mVCDevice.FrameCaptured += new VideoCaptureFrameEventHandler(frameReceived);

						mVCDevice.Properties.Brightness.Value = mVCDevice.Properties.Brightness.Default; 
						mVCDevice.Properties.Contrast.Value = mVCDevice.Properties.Contrast.Default;
				
						// DEBUG -- print resolutions supported by this device
						// TODO -- store in the list modifiable by user via UI
						foreach (Size s in mVCDevice.GetResolutionCaps())
							log("Resolution: " + s.ToString());

						// RM 20050323 set a default resolution
						// RM 20050520 if the device don't support this, it will throw
						// an exception. 
						try
						{
							mVCDevice.Resolution = new Size(176, 144);
						}
						catch(InvalidOperationException ex)
						{
							// This is not fatal. We'll use the device's default resolution.
							System.Diagnostics.Debug.WriteLine(ex, "Using default device's resolution");
							log("RRecorderVideo.Start" + ex.Message + " -- Using default device's resolution");
						}

						mVCDevice.Enabled = true;
					}
					catch
					{
						if (mVCDevice != null)
							mVCDevice.Dispose();
						throw;
					}
				}
				catch(Exception ex)
				{
					mVCDevice = null;

					System.Diagnostics.Debug.WriteLine(ex, "RRecorderVideo.Start");
					log("RRecorderVideo.Start" + ex.Message);
				}
			}
		}

		//****************
		/// <summary>
		/// Stops the device if it was started.
		/// Does nothing if not started.
		/// </summary>
		//****************
		public void Stop()
		{
			if (mVCDevice != null)
			{
				try
				{
					try
					{
						mVCDevice.Enabled = false;
						mVCDevice.FrameCaptured -= new VideoCaptureFrameEventHandler(frameReceived);
					}
					catch
					{
						// even if the previous calls failed, try to dispose the device
						throw;
					}
					finally
					{
						mVCDevice.Dispose();
					}
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex, "RRecorderVideo.Start");
					log("RRecorderVideo.Start" + ex.Message);
				}

				mVCDevice = null;
			}
		}

		//*****************
		/// <summary>
		/// Pauses the device, if possible.
		/// </summary>
		//*****************
		public void Pause()
		{
			if (mVCDevice != null)
			{
				try
				{
					mVCDevice.Enabled = !mVCDevice.Enabled;

					if (mVCDevice.Enabled)
					{
						// reset fps counter
						mStatFps = new RFreqCounter();
					}
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex, "RRecorderVideo.Pause");
					log("RRecorderVideo.Pause" + ex.Message);

					Stop();
				}
			}
		}


		#endregion

		#region IDisposable Members

		//*******************
		/// <summary>
		/// Display the underlying acquisition objects
		/// </summary>
		//*******************
		public void Dispose()
		{
			Stop();
			mCurrentDevice = null;
			mDevices = null;
		}

		#endregion

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//**************************************
		private bool hasDevice(RDeviceInfo info)
		{
			if (mDevices != null)
				foreach(RDeviceInfo i in mDevices)
					if (i == info)
						return true;
			
			return false;
		}


		//**************************
		private void log(string str)
		{
			if (mLog != null)
				mLog.Log(str);
		}


		//*********************************************************************
		private void frameReceived(object sender, VideoCaptureFrameEventArgs e)
		{
			// get the bitmap from the frame
			Bitmap b = e.GetBitmap();

			// create new buffer

			//Rectangle bounds = new Rectangle(0, 0, b.Size.Width, b.Size.Height);
			//RBufferVideo rb = new RBufferVideo(bounds, ".net/bitmap", null);
			//rb.Metadata[RBuffer.Key.kBitmap] = b;

			RBufferVideo rb = RBufferVideo.JpegFromBitmap(b);

			// and enqueue it
			AddBuffer(rb);
			
			// update stats
			mStatFps.Count++;
		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private RDeviceInfo[]		mDevices			= null;
		private	RDeviceInfo			mCurrentDevice		= null;
		private RILog				mLog				= null;
		private	VideoCaptureDevice	mVCDevice			= null;
		private	RFreqCounter		mStatFps			= new RFreqCounter();

	} // class RRecorderVideo
} // namespace Alfray.Xeres.Device


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RRecorderVideo.cs,v $
//	Revision 1.11  2005/10/30 03:06:41  ralf
//	Fix to support DV input
//	
//	Revision 1.10  2005/04/30 22:41:30  ralf
//	Rebuilding own VideoCaptureNet project
//	Using separate LibUtils & LibUtilsTests
//	
//	Revision 1.9  2005/03/28 00:24:08  ralf
//	Added StatFps
//	
//	Revision 1.8  2005/03/24 15:32:01  ralf
//	Test with smaller image size
//	
//	Revision 1.7  2005/03/23 06:31:19  ralf
//	Using RBuffer, RIBuffer, RISender, RIReceiver and RSenderBase.
//	
//	Revision 1.6  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.5  2005/03/10 22:10:20  ralf
//	Quick test with JPEG encoding
//	
//	Revision 1.4  2005/03/09 20:02:39  ralf
//	Added Format to RBufferVideo
//	Added FromBitmap to RBufferVideo
//	
//	Revision 1.3  2005/03/07 17:00:44  ralf
//	Update. Fixes. Doc.
//	
//	Revision 1.2  2005/03/07 07:17:00  ralf
//	RRecorderVideo, implement start, stop
//	RRecorderVideo, capture images in data queue
//	Display recorded images from RRecorderVideo data queue
//	Updating preferences for video recorder device
//	
//	Revision 1.1  2005/03/07 01:50:04  ralf
//	New files: RIRecorder, RIPlayer, RRecorderVideo, RRecorderFactory, RPrefForm
//	
//---------------------------------------------------------------
