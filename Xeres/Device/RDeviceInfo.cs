//*******************************************************************
/*

	Solution:	Xeres
	Project:	Device
	File:		RDeviceInfo.cs

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

//*************************************
namespace Alfray.Xeres.Device
{
	//***************************************************
	/// <summary>
	/// RDeviceInfo holds information describing a device
	/// for recording or playing.
	/// </summary>
	//***************************************************
	public class RDeviceInfo
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------

		public const string kNoDeviceId = "[none]";


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//*******************
		/// <summary>
		/// Returns the device name as a string, suitable for display.
		/// The string should be reasonably unique and meaningful in
		/// identifying the device.
		/// 
		/// This string should not be used internally to uniquely identify
		/// the device. Use UniqueId for this purpose.
		/// </summary>
		//*******************
		public string Display
		{
			get
			{
				return mDisplay;
			}
		}


		//********************
		/// <summary>
		/// Returns a string that uniquely indentifies this device,
		/// typically for storage in preferences and later reselection.
		/// The string does not need to be human-readable.
		/// </summary>
		//********************
		public string UniqueId
		{
			get
			{
				return mUniqueId;
			}
		}


		//*******************
		/// <summary>
		/// Returns the native object representing the device.
		/// This may be a Windows Form component or a DirectX object, etc.
		/// </summary>
		//*******************
		public object Device
		{
			get
			{
				return mDevice;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//*********************************************************
		public RDeviceInfo() : this(kNoDeviceId, kNoDeviceId, null)
		{
		}


		//**************************************************************
		public RDeviceInfo(string display, string unique_id, object dev)
		{
			mDisplay = display;
			mUniqueId = unique_id; 
			mDevice = dev;
		}


		//*******************************
		public override int GetHashCode()
		{
			if (mDevice == null)
				return 0;
			else
				return mDevice.GetHashCode();
		}


		//*******************************
		/// <returns>The display string for the device</returns>
		//*******************************
		public override string ToString()
		{
			if (mDevice == null)
				return base.ToString();
			else
				return mDisplay;
		}




		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private string mDisplay;
		private object mDevice;
		private string mUniqueId;

	} // class RDeviceInfo
} // namespace Alfray.Xeres.Device


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RDeviceInfo.cs,v $
//	Revision 1.4  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.3  2005/03/07 01:50:04  ralf
//	New files: RIRecorder, RIPlayer, RRecorderVideo, RRecorderFactory, RPrefForm
//	
//	Revision 1.2  2005/02/22 06:11:21  ralf
//	Added Device.RBuffer
//	
//	Revision 1.1  2005/02/21 06:21:54  ralf
//	Started RDeviceInfo
//	
//---------------------------------------------------------------
