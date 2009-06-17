//*******************************************************************
/*

	Solution:	Xeres
	Project:	XeresApp
	File:		RMainModule.cs

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
using System.Windows.Forms;

using Alfray.LibUtils.Misc;

//*************************************
namespace Alfray.Xeres.XeresApp
{
	//***************************************************
	/// <summary>
	/// Summary description for RMainModule.
	/// </summary>
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


		//******************************
		public static RMainForm MainForm
		{
			get
			{
				return RMainModule.mMainForm;
			}
		}

		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//****************
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();

			mMainMod = new RMainModule();
			mMainForm = new RMainForm();

			Application.Run(mMainForm);
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private static RMainForm mMainForm;
		private static RMainModule mMainMod;
		private RPref mPref = new RPref();


	} // class RMainModule
} // namespace Alfray.Xeres.XeresApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMainModule.cs,v $
//	Revision 1.4  2005/04/30 22:41:30  ralf
//	Rebuilding own VideoCaptureNet project
//	Using separate LibUtils & LibUtilsTests
//	
//	Revision 1.3  2005/03/07 01:50:05  ralf
//	New files: RIRecorder, RIPlayer, RRecorderVideo, RRecorderFactory, RPrefForm
//	
//	Revision 1.2  2005/02/21 04:43:27  ralf
//	GPL headers and CVS footers
//	
//	Revision 1.1.1.1  2005/02/21 03:39:10  ralf
//	no message
//	
//	Revision 1.1  2005/02/18 23:21:52  ralf
//	Creating both an App and a Class Lib
//	
//	Revision 1.1.1.1  2005/02/18 22:54:53  ralf
//	A skeleton application template, with NUnit testing
//	
//---------------------------------------------------------------
