//*******************************************************************
/*

	Solution:	Rivet
	Project:	TestsConsole
	File:		RTestAppState.cs

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

using NUnit.Framework;

using Alfray.Rivet.RivetLib;

//*************************************
namespace Alfray.Rivet.TestsConsole.RivetLib
{
	//***************************************************
	/// <summary>
	/// RTestAppState tests RAppState.
	/// </summary>
	[TestFixture]
	//***************************************************
	public class RTestAppState
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
		[SetUp]
		public void SetUp()
		{
			t = new RAppState();
		}

		//********************
		[TearDown]
		public void TearDown()
		{
		}

		//****************
		[Test]
		public void TestInit()
		{
			Assert.IsNotNull(t);
			Assert.IsNotNull(t.Libraries);
			Assert.IsNull(t.CurrentLibrary);
			Assert.IsNull(t.CurrentDirectory);
		}

		//****************
		[Test]
		public void TestCurrentLibrary()
		{
			// Create a library
			RLibrary lib = new RLibrary("root");

			// Add it to the list
			t.Libraries.Add(lib);

			// Check there's no current lib
			Assert.IsNull(t.CurrentLibrary);

			// Select it
			t.CurrentLibrary = lib;

			// Check it's selected
			Assert.AreSame(lib, t.CurrentLibrary);

			// Deselect it
			t.CurrentLibrary = null;

			// Check there's no current lib
			Assert.IsNull(t.CurrentLibrary);
		}

		//****************
		[Test]
		public void TestCurrentDirectory()
		{
			// Create a library
			RLibrary lib = new RLibrary("root");

			// Add it to the list
			t.Libraries.Add(lib);
		
			// tbdl
		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private RAppState t;

	} // class RTestAppState
} // namespace Alfray.Rivet.TestsConsole.RivetLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RTestAppState.cs,v $
//	Revision 1.3  2005/05/31 00:12:16  ralf
//	Current selected directory in RAppState.
//	Display image for select directory in main form.
//	
//	Revision 1.2  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//	Revision 1.1  2005/05/25 03:54:22  ralf
//	Implemented LocationCombo with storage in prefs.
//	Added RLibrary, RAppState, RTestSkeleton.
//	
//---------------------------------------------------------------
