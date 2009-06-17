//*******************************************************************
/*

	Solution:	Rivet
	Project:	TestsConsole
	File:		RTestDir.cs

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
using System.IO;

using NUnit.Framework;

using Alfray.Rivet.RivetLib;

//*************************************
namespace Alfray.Rivet.TestsConsole.RivetLib
{
	//***************************************************
	/// <summary>
	/// RTestDir tests RDir.
	/// </summary>
	[TestFixture]
	//***************************************************
	public class RTestDir
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
			string assembly_dir = Path.GetDirectoryName(typeof(RTestDir).Assembly.Location);

			t = new RDir(null, assembly_dir);
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
		}

		//****************
		[Test]
		public void TestImageFilter()
		{
			Assert.IsNotNull(t);

			t.ImageFilter = ".*";
			Assert.AreEqual(".*", t.ImageFilter);

			t.ImageFilter = "";
			Assert.AreEqual("", t.ImageFilter);
		}

		//****************
		[Test]
		public void TestDirs()
		{
			Assert.IsNotNull(t);

			RDir[] dirs = t.Directories;

			// There should not be any directories
			Assert.IsNotNull(dirs);
			Assert.AreEqual(0, dirs.Length);
		}

		//****************
		[Test]
		public void TestImages()
		{
			Assert.IsNotNull(t);

			RImage[] images = t.Images;

			// There should not be any jpeg images
			Assert.IsNotNull(images);
			Assert.AreEqual(0, images.Length);

			// change filter to accept everything
			t.ImageFilter = ".*";

			// and reload
			RImage[] images2 = t.Images;

			// It should not be the same arrays
			Assert.IsFalse(Object.ReferenceEquals(images, images2));

			// There should be some files, at least one
			Assert.IsNotNull(images2);
			Assert.IsTrue(images2.Length >= 1);
		}



		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private RDir t;

	} // class RTestDir
} // namespace Alfray.Rivet.TestsConsole.RivetLib

//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RTestDir.cs,v $
//	Revision 1.1  2005/05/30 22:20:05  ralf
//	Added RFSItem and RDir in RivetLib.
//	Load directories and RLibrary.
//	Display directory tree for library in RivetUI
//	
//---------------------------------------------------------------
