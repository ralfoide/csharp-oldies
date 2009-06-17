//*******************************************************************
/*

	Solution:	Xeres
	Project:	TestsConsole
	File:		RTestStatCounter.cs

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
using System.Threading;

using NUnit.Framework;

using Alfray.Xeres.XeresLib;

//*************************************
namespace Alfray.Xeres.TestsConsole.XeresLib
{
	//***************************************************
	/// <summary>
	/// Summary description for RTestStatCounter.
	/// </summary>
	//***************************************************
	[TestFixture]
	public class RTestStatCounter
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
			s = new RStatCounter();
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
			Assert.IsNotNull(s);
			Assert.AreEqual(0, s.BuffersBytes);
			Assert.AreEqual(0, s.BuffersCount);
			Assert.AreEqual(0.0, s.AverageKbps);
		}

		//****************
		[Test]
		public void TestBuffersCount()
		{
			Assert.IsNotNull(s);

			Assert.AreEqual(0, s.BuffersCount);

			s.BuffersCount += 50;
			Assert.AreEqual(50, s.BuffersCount);

			s.BuffersCount -= 10;
			Assert.AreEqual(40, s.BuffersCount);
		}

		//****************
		[Test]
		public void TestBuffersBytes()
		{
			Assert.IsNotNull(s);

			Assert.AreEqual(0, s.BuffersBytes);

			s.BuffersBytes += 4000;
			Assert.AreEqual(4000, s.BuffersBytes);

			s.BuffersBytes -= 1000;
			Assert.AreEqual(3000, s.BuffersBytes);

			s.BuffersBytes += 6000;
			Assert.AreEqual(9000, s.BuffersBytes);
		}

		//****************
		[Test]
		public void TestAverageKbps()
		{
			Assert.IsNotNull(s);

			Assert.AreEqual(0, s.BuffersBytes);
			Assert.AreEqual(0.0, s.AverageKbps);

			// We can't wait in a quick nunit test executed on the fly.
			// Since the average is in kb per seconds, we would have to
			// wait several seconds to get a reasonable test. So I just
			// skip it here. If this doesn't work it will be obvious in 
			// the UI.
			// (Another approach is to write a test, try it once and
			// comment it for later usage only in case of issue... let's 
			// do this here)

#if LONGTESTS

			s.BuffersBytes += 4*1024;

			// Wait 2 seconds and a tiny bit more
			Thread.Sleep(2200); // 2.2 s in micro seconds

			s.BuffersBytes += 6*1024;

			// Wait 2 seconds and a tiny bit more -- again
			Thread.Sleep(2200); // 2.2 s in micro seconds

			// 6*1024 bytes expressed in bits per 2 seconds
			// The count is done on an integer number of seconds
			// so it should be 2 seconds, not 2.2
			// Notes:
			// 1- The count is *after* the first time the time is set
			//    so the 4*1024 above should not be counted in.
			// 2- The average must be computed when the BufferBytes is
			//    incremented, *not* when the average is requested so
			//    in this case it should be after 2 secondes, not 4.
			// 3- Since we want kilobits, it's nb_bytes * 8 / 1024...
			Assert.AreEqual(6 * 8 / 2, s.AverageKbps);

#endif // LONGTESTS

		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private RStatCounter s;

	} // class RTestStatCounter
} // namespace Alfray.Xeres.TestsConsole.XeresLib


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log: RTestStatCounter.cs,v $
//	Revision 1.4  2005/03/28 00:24:29  ralf
//	New tests
//	
//	Revision 1.3  2005/03/21 07:17:49  ralf
//	Adding inline doc comments for all classes and public methods
//	
//	Revision 1.2  2005/03/20 20:00:45  ralf
//	Updated for NUnit 2.2.
//	
//	Revision 1.1  2005/03/10 22:09:33  ralf
//	Added stat counter with average kbps
//	
//---------------------------------------------------------------
