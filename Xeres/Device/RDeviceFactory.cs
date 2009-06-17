//*******************************************************************
/*

	Solution:	Xeres
	Project:	Device
	File:		RDeviceFactory.cs

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

using Alfray.LibUtils.Misc;
using Alfray.LibUtils.Buffers;

//*************************************
namespace Alfray.Xeres.Device
{
	//***************************************************
	/// <summary>
	/// RDeviceFactory is an helper class designed to
	/// instanciate various classes from the Device module
	/// at once by the application.
	/// 
	/// Access to the instanciated classes is made thru
	/// their interface to define clear boundaries with
	/// the application code.
	/// </summary>
	//***************************************************
	public class RDeviceFactory: IDisposable
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//*****************************
		/// <summary>
		/// Returns the interface of the current 
		/// video recorder object.
		/// </summary>
		//*****************************
		public RIRecorder RecorderVideo
		{
			get
			{
				return mRecorderVideo;
			}
		}


		//*****************************
		/// <summary>
		/// Returns the interface of the video 
		/// player object for the local video.
		/// </summary>
		//*****************************
		public RIPlayer LocalVideo
		{
			get
			{
				return mLocalVideo;
			}
		}


		//*****************************
		/// <summary>
		/// Returns the interface of the video 
		/// player object for the remote video.
		/// </summary>
		//*****************************
		public RIPlayer RemoteVideo
		{
			get
			{
				return mRemoteVideo;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//*********************
		/// <summary>
		/// Instantiates the various recorders and players
		/// needed by the application.
		/// </summary>
		//*********************
		public RDeviceFactory()
		{
			mRecorderVideo = new RRecorderVideo();
			mLocalVideo = new RPlayerVideo();
			mRemoteVideo = new RPlayerVideo();

			mRecorderVideo.BufferAvailableEvent += new BufferAvailableCallback(mLocalVideo.OnBufferAvailable);
		}


		//*********************************
		/// <summary>
		/// Sets the logger for the various recorders and
		/// players.
		/// To prevent non-main threads from accessing
		/// directly the UI as a side effect of logging,
		/// it may be wise for the logger to be an
		/// RAsyncLog instance. This is not a requirement
		/// though.
		/// </summary>
		//*********************************
		public void SetLogger(RILog logger)
		{
			mRecorderVideo.Logger = logger;
			mLocalVideo.Logger = logger;
			mRemoteVideo.Logger = logger;
		}

		#region IDisposable Members

		//*******************
		/// <summary>
		/// Dispose of the various recorders and players.
		/// </summary>
		//*******************
		public void Dispose()
		{
			mRecorderVideo.Dispose();
			mRecorderVideo = null;
		}

		#endregion

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private RRecorderVideo	mRecorderVideo;
		private RPlayerVideo	mLocalVideo;
		private RPlayerVideo	mRemoteVideo;

	} // class RDeviceFactory
} // namespace Alfray.Xeres.Device


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RDeviceFactory.cs,v $
//	Revision 1.5  2005/04/30 22:41:30  ralf
//	Rebuilding own VideoCaptureNet project
//	Using separate LibUtils & LibUtilsTests
//	
//	Revision 1.4  2005/03/28 00:23:05  ralf
//	Added LocalVideo
//	
//	Revision 1.3  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.2  2005/03/07 07:17:00  ralf
//	RRecorderVideo, implement start, stop
//	RRecorderVideo, capture images in data queue
//	Display recorded images from RRecorderVideo data queue
//	Updating preferences for video recorder device
//	
//	Revision 1.1  2005/03/07 01:50:21  ralf
//	New files: RIRecorder, RIPlayer, RRecorderVideo, RRecorderFactory, RPrefForm
//	
//---------------------------------------------------------------
