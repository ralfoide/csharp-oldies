//*******************************************************************
/*

	Solution:	Xeres
	Project:	Device
	File:		RIPlayer.cs

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
using System.Collections;

using Alfray.LibUtils.Misc;
using Alfray.LibUtils.Buffers;

//*************************************
namespace Alfray.Xeres.Device
{
	//***************************************************
	/// <summary>
	/// The RIPlayer interface provides the common
	/// access to devices that can display or output data.
	/// </summary>
	//***************************************************
	public interface RIPlayer: RIReceiver, IDisposable
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------

		//***********************
		/// <summary>
		/// Returns the currently selected device.
		/// Returns null when no device is selected.
		/// When affected, change the device. Affect null to deselect device.
		/// When affecting, a similar device must be found in the existing enumerated
		/// device list, otherwise no change is made. 
		/// </summary>
		//***********************
		RDeviceInfo CurrentDevice
		{
			get;
			set;
		}


		// -- Utilities --


		//**********
		/// <summary>
		/// Set the logger for the player.
		/// Ideally the logger should be asynchronous and not directly
		/// update any WinForm UI.
		/// </summary>
		//**********
		RILog Logger
		{
			get;
			set;
		}

		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		//************
		/// <summary>
		/// Initializes and returns an array of device info.
		/// Initialization is done only once then the same array is always returned.
		/// The returned array can be empty but it cannot be a null pointer.
		/// </summary>
		//************
		RDeviceInfo[] EnumerateDevices();


		//***********
		/// <summary>
		/// Starts the device.
		/// It will start accumulating buffers in the buffer queue.
		/// Does nothing if there is no currently selected device.
		/// </summary>
		//************
		void Start();
		

		//***********
		/// <summary>
		/// Stops the device if it was started.
		/// Does nothing if not started.
		/// </summary>
		//************
		void Stop();
		

		//************
		/// <summary>
		/// Pauses the device, if possible.
		/// </summary>
		//************
		void Pause();


	} // class RIPlayer
} // namespace Alfray.Xeres.Device


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RIPlayer.cs,v $
//	Revision 1.7  2005/04/30 22:41:30  ralf
//	Rebuilding own VideoCaptureNet project
//	Using separate LibUtils & LibUtilsTests
//	
//	Revision 1.6  2005/03/28 00:23:13  ralf
//	Minor fixes
//	
//	Revision 1.5  2005/03/24 15:32:17  ralf
//	Added RPlayerVideo
//	
//	Revision 1.4  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
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
