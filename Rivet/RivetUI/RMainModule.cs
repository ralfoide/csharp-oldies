//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetUI
	File:		RMainModule.cs

	Copyright 2005, Raphael MOLL.

	This file is part of Rivet.

	Rivet is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Rivet is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Rivet; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

*/
//*******************************************************************


using System;
using System.Windows.Forms;

using Alfray.LibUtils.Misc;

//*************************************
namespace Alfray.Rivet.RivetUI
{
	//***************************************************
	/// <summary>
	/// Summary description for RIMainModule.
	/// </summary>
	//***************************************************
	public class RMainModule
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//**********************
		public static RPref Pref
		{
			get
			{
				return mMainMod.mPref;
			}
		}


		//********************************
		public static RMainFormEx MainForm
		{
			get
			{
				return mMainMod.mMainForm;
			}
		}



		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------

		
		
		//****************
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//****************
		public RMainModule()
		{
			mMainMod = this;

			mPref     = new RPref();
			mMainForm = new RMainFormEx();
		}

		
		
		//****************
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//****************
		public void Start()
		{
			Application.Run(mMainForm);
		}


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------
		
		private static RMainModule mMainMod;

		private RMainFormEx mMainForm = null;
		private RPref		mPref	  = null;

	} // class RMainModule
} // namespace Alfray.Rivet.RivetUI


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMainModule.cs,v $
//	Revision 1.1  2005/05/30 00:48:33  ralf
//	Reorganized source. Added RivetUI library.
//	
//---------------------------------------------------------------
