//*******************************************************************
/*

	Solution:	Xeres
	Project:	Device
	File:		RPlayerVideo.cs

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
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;

using Alfray.LibUtils.Misc;
using Alfray.LibUtils.Buffers;

//*************************************
namespace Alfray.Xeres.Device
{
	//***************************************************
	/// <summary>
	/// RPlayerVideo plays video buffers in a Form.PictureBox
	/// control. The player can accept two kind of pseudo
	/// devices: either a callback delegate or a picture
	/// box control.
	/// </summary>
	//***************************************************
	public class RPlayerVideo: RIPlayer
	{
		//-------------------------------------------
		//----------- Public Events -----------------
		//-------------------------------------------

		//************
		/// <summary>
		/// Delegate callback to be implemented by the
		/// window containing the control to display
		/// the given image.
		/// </summary>
		//************
		public delegate void UpdateVideoCallback(Image image);

		
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------



		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------



		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//*******************
		/// <summary>
		/// Constructs a new RPlayerVideo.
		/// </summary>
		//*******************
		public RPlayerVideo()
		{
		}


		//***********************************************************************
		/// <summary>
		/// This static method creates a speudo-device description.
		/// Whenever a new image is to be drawn, the player will call the
		/// callback represented by this pseudo-device.
		/// </summary>
		/// <param name="callback">The callback responsible for drawing the image</param>
		/// <returns>A pseudo-device description</returns>
		//***********************************************************************
		public static RDeviceInfo NewCallbackDevice(UpdateVideoCallback callback)
		{
			return new RDeviceInfo(kCallbackDevice, kCallbackDevice, callback);
		}


		//******************************************************************
		/// <summary>
		/// This static method creates a speudo-device description.
		/// Whenever a new image is to be drawn, the player will directly
		/// set the new image into the PictureBox instance represented by 
		/// this pseudo-device.
		/// </summary>
		/// <param name="pictureBox">The target PictureBox control</param>
		/// <returns>A pseudo-device description</returns>
		//******************************************************************
		public static RDeviceInfo NewPictureBoxDevice(PictureBox pictureBox)
		{
			return new RDeviceInfo(kPictureBoxDevice, kPictureBoxDevice, pictureBox);
		}


		#region RIPlayer Members

		//******************************
		/// <summary>
		/// Returns the currently selected device.
		/// Returns null when no device is selected.
		/// When affected, change the device. Affect null to deselect device.
		/// When affecting, a similar device must be found in the existing enumerated
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
				mCurrentDevice = value;
			}
		}


		//*****************
		/// <summary>
		/// Set the logger for the player.
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

		//*************************************
		/// <summary>
		/// Returns an empty list of devices.
		/// To create a pseudo-device, use NewCallbackDevice()
		/// or NewPictureBoxDevice().
		/// </summary>
		//*************************************
		public RDeviceInfo[] EnumerateDevices()
		{
			return mDevices;
		}

		//*****************
		public void Start()
		{
			Stop();

			// can't quit now...
			mPlayerQuit = false;

			// create queue as needed
			if (mPlayerQueue == null)
				mPlayerQueue = new Queue();

			// create non-signaled (blocking) event
			if (mPlayerEvent == null)
				mPlayerEvent = new AutoResetEvent(false);
			else
				mPlayerEvent.Reset();
		
			// start the player thread
			mPlayerThread = new Thread(new ThreadStart(this.playerLoop));
			mPlayerThread.Start();	
		}

		//****************
		public void Stop()
		{
			if (mPlayerThread != null)
			{
				// first notify the thread it will have to quit
				mPlayerQuit = true;

				// fire the event to let it process this
				mPlayerEvent.Set();

				// and wait for the thread to actually terminate
				mPlayerThread.Join();

				// delete the thread ptr
				mPlayerThread = null;
			}
		}

		//*****************
		public void Pause()
		{
			// TODO:  Add RPlayerVideo.Pause implementation
			System.Diagnostics.Trace.Fail("Not Implemented Yet");
		}

		#endregion

		#region RIReceiver Members

		//********************************************
		public void OnBufferAvailable(RISender sender)
		{
			if (mPlayerEvent != null)
			{
				// get all buffers in our internal queue
				lock(sender.BufferQueue.SyncRoot)
				{
					lock(mPlayerQueue.SyncRoot)
					{
						for(int n = sender.BufferQueue.Count - 1; n >= 0; n--)
							mPlayerQueue.Enqueue(sender.BufferQueue.Dequeue());
					}
				}

				// signal the player more buffers can be processed now
				mPlayerEvent.Set();
			}
		}

		#endregion

		#region IDisposable Members

		//*******************
		public void Dispose()
		{
			Stop();

			mCurrentDevice = null;
			
			if (mPlayerEvent != null)
			{
				mPlayerEvent.Close();
				mPlayerEvent = null;
			}
		}

		#endregion


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//******************************************
		private void log(string context, string msg)
		{
			if (mLog != null)
				mLog.Log(context + ": " + msg);
			else
				System.Diagnostics.Debug.WriteLine(msg, context);
		}


		//***********************
		private void playerLoop()
		{
			// wait on the event signal...
			while (mPlayerEvent != null && mPlayerEvent.WaitOne())
			{
				if (mPlayerQuit)
					break;

				while(!mPlayerQuit)
				{
					// get one buffer
					RIBuffer buffer = null;

					try
					{
						lock(mPlayerQueue)
						{
							if (mPlayerQueue.Count > 0)
								buffer = mPlayerQueue.Dequeue() as RIBuffer;
						}
					}
					catch(Exception ex)
					{
						log("playerLoop.dequeue", ex.Message);
					}

					// extract the image from the buffer
					Image image = null;
					if (buffer is RBufferVideo)
					{
						RBufferVideo rb = buffer as RBufferVideo;

						if (rb.MimeType == ".net/bitmap")
							image = rb.Metadata[RBuffer.Key.kBitmap] as Image;
						// for debug purposes only
						else if (rb.MimeType == "image/jpeg")	
							image = RBufferVideo.ImageFromJpeg(rb);
					}

					// display the image according to the current device, if any
					if (image != null)
					{
						if (mCurrentDevice != null && mCurrentDevice.Device != null)
						{
							if (mCurrentDevice.UniqueId == kCallbackDevice)
							{
								// Do not call a UI callback from a separate thread

								UpdateVideoCallback callback = (UpdateVideoCallback) mCurrentDevice.Device;

								object[] args = { image };
								callback.DynamicInvoke(args);
							}
							else if (mCurrentDevice.UniqueId == kPictureBoxDevice)
							{
								PictureBox pb = (PictureBox) mCurrentDevice.Device;

								// Setting a property using an asynchronous delegate
								// is a bit more complex.
								// Reference:
								// http://www.dotnet247.com/247reference/msgs/41/206211.aspx

								SetImageDelegate sid = (SetImageDelegate) Delegate.CreateDelegate(typeof(SetImageDelegate), pb, "set_Image", false);
								object[] args = { image };
								sid.DynamicInvoke(args);
							}
						} // display
						else
						{
							image.Dispose();
						}
					}

					// dispose buffer... currently nothing to do

				} // while buffers & !quit
			} // while player event
		}

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private delegate void SetImageDelegate(Image image);
		private delegate void SetValueDelegate(Object obj, Object val, Object[] index);

		private const string kCallbackDevice   = "callback";
		private const string kPictureBoxDevice = "pictureBox";

		private static RDeviceInfo[] mDevices	= new RDeviceInfo[0];

		private	RDeviceInfo		mCurrentDevice	= null;
		private	RILog			mLog			= null;

		private	Thread			mPlayerThread	= null;
		private AutoResetEvent	mPlayerEvent	= null;
		private	bool			mPlayerQuit		= false;
		private Queue			mPlayerQueue	= null;

	} // class RPlayerVideo
} // namespace Alfray.Xeres.Device


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RPlayerVideo.cs,v $
//	Revision 1.3  2005/04/30 22:41:30  ralf
//	Rebuilding own VideoCaptureNet project
//	Using separate LibUtils & LibUtilsTests
//	
//	Revision 1.2  2005/03/28 00:23:51  ralf
//	Fully implemented
//	
//	Revision 1.1  2005/03/24 15:32:17  ralf
//	Added RPlayerVideo
//	
//---------------------------------------------------------------
