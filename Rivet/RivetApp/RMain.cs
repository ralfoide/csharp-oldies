//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetApp
	File:		RMain.cs

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
using Alfray.Rivet.RivetUI;

//*************************************
namespace Alfray.Rivet.RivetApp
{
	//***************************************************
	/// <summary>
	/// Summary description for RMain.
	/// </summary>
	//***************************************************
	public class RMain
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
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		//****************
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();

			RMainModule mod = new RMainModule();
			mod.Start();
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		

	} // class RMain
} // namespace Alfray.Rivet.RivetApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RMain.cs,v $
//	Revision 1.1  2005/05/30 00:48:33  ralf
//	Reorganized source. Added RivetUI library.
//	
//	Revision 1.1.1.1  2005/05/23 02:48:59  ralf
//	no message
//	
//	Revision 1.2  2005/03/20 19:48:39  ralf
//	Added GPL headers.
//	
//	Revision 1.1  2005/02/18 23:21:52  ralf
//	Creating both an App and a Class Lib
//	
//	Revision 1.1.1.1  2005/02/18 22:54:53  ralf
//	A skeleton application template, with NUnit testing
//	
//---------------------------------------------------------------
