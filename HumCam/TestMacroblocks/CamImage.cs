//*******************************************************************
/* 

 		Project:	TestForm
 		File:		CamImage.cs

*/ 
//*******************************************************************

using System;
using System.Drawing;


//*************************************
namespace Alfray.HumCam.TestForm
{
	//***************************************************
	/// <summary>
	/// Summary description for CamImage.
	/// </summary>
	public class CamImage
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		public CamImage()
		{
		}


		//************************************
		public void LoadImage(string filename)
		{
			// this typically creates a format24bppRGB
			mBitmap = new Bitmap(filename);

		}


		//************************
		public void CreateBlocks()
		{
			if (mBitmap == null)
				return;

			const int kBlockSize = 8;

			/*
			Image.GetThumbnailImageAbort myCallback =
				new Image.GetThumbnailImageAbort(ThumbnailCallback);

			// this typically creates a format32bppPARGB
			mBlocks = (Bitmap) mBitmap.GetThumbnailImage(mBitmap.Width / kBlockSize,
				mBitmap.Height / kBlockSize, myCallback, IntPtr.Zero);
			*/

			mBlocks = new Bitmap(mBitmap,
				mBitmap.Width / kBlockSize,
				mBitmap.Height / kBlockSize);

			System.Diagnostics.Debug.Write(mBitmap.PixelFormat.ToString());
			System.Diagnostics.Debug.Write(mBlocks.PixelFormat.ToString());

			int nx = mBlocks.Width;
			int ny = mBlocks.Height;

			mBright = new float[nx,ny];

			for(int y = 0; y < ny; y++)
			{
				for(int x = 0; x < nx; x++)
				{
					Color c = mBlocks.GetPixel(x, y);
					// float f = c.GetHue() / 360 + c.GetSaturation() + c.GetBrightness();
					float f = c.GetBrightness();
					mBright[x,y] = f;

					c = Color.FromArgb((int)(255*f), (int)(255*f), (int)(255*f));
					mBlocks.SetPixel(x, y, c);
				}
			}
		}


		//***************************************************************
		public void DisplayImage(System.Windows.Forms.PictureBox pictBox)
		{
			if (mBlocks != null)
			{
				pictBox.Image = mBlocks;
			}
			else
			{
				pictBox.Image = mBitmap;
			}
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------

		private bool ThumbnailCallback()
		{
			return false;
		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		Bitmap mBitmap;
		Bitmap mBlocks;
		float [,] mBright;

	} // class CamImage
} // namespace TestMacroblocks


//---------------------------------------------------------------
//
//	$Log$
//---------------------------------------------------------------
