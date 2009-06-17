//*******************************************************************
/*

	Solution:	Xeres
	Project:	XeresLib
	File:		RCommState.cs

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
namespace Alfray.Xeres.XeresLib
{
	//***************************************************
	/// <summary>
	/// RCommState is an utility class to keep track of the
	/// state of the communication: globally communication
	/// can be off or on, and individual components (audio
	/// and video) can be on or off separately.
	/// This is merely a struct-like object without much logic
	/// right now.
	/// </summary>
	//***************************************************
	public class RCommState
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------

		//******************
		/// <summary>
		/// Indicates if communication is on or off
		/// </summary>
		//******************
		public bool CommIsOn
		{
			get
			{
				return mCommIsOn;
			}
			set
			{
				mCommIsOn = value;
			}
		}

		//*******************
		/// <summary>
		/// Indicates if video is on or off
		/// </summary>
		//*******************
		public bool VideoIsOn
		{
			get
			{
				return mVideoIsOn;
			}
			set
			{
				mVideoIsOn = value;
			}
		}

		//*******************
		/// <summary>
		/// Indicates if audio is on or off
		/// </summary>
		//*******************
		public bool AudioIsOn
		{
			get
			{
				return mAudioIsOn;
			}
			set
			{
				mAudioIsOn = value;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//*****************
		/// <summary>
		/// Initializes the instance with everything off.
		/// </summary>
		//*****************
		public RCommState()
		{
			
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private bool mCommIsOn = false;
		private bool mVideoIsOn = false;
		private bool mAudioIsOn = false;



	} // class RCommState
} // namespace Alfray.Xeres.XeresLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RCommState.cs,v $
//	Revision 1.2  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.1  2005/02/21 06:22:11  ralf
//	Started RCommState
//	
//---------------------------------------------------------------
