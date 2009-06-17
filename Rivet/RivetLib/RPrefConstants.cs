//*******************************************************************
/*

	Solution:	Rivet
	Project:	RivetApp
	File:		RPrefConstants.cs

	Copyright 2003, 2004, Raphael MOLL.

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

//*************************************
namespace Alfray.Rivet.RivetLib
{
	//***************************************************
	/// <summary>
	/// Summary description for RPrefConstants.
	/// </summary>
	public class RPrefConstants
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------

		// UI

		public const string kMainForm = "forms-main";
		public const string kPrefForm = "forms-pref";
		public const string kDebugForm = "forms-debug";

		// State

		public const string kStateLibList = "state-lib-list";
		public const string kStateLibCurr = "state-lib-curr";


	} // class RPrefConstants
} // namespace Alfray.Rivet.RivetApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RPrefConstants.cs,v $
//	Revision 1.2  2005/05/29 23:02:13  ralf
//	Fixes. Load last current library at startup.
//	
//	Revision 1.1  2005/05/25 03:54:22  ralf
//	Implemented LocationCombo with storage in prefs.
//	Added RLibrary, RAppState, RTestSkeleton.
//	
//	Revision 1.1.1.1  2005/05/23 02:48:59  ralf
//	no message
//	
//---------------------------------------------------------------
