//*******************************************************************
/* 

 		Project:	SneLibTest
 		File:		RSneListenerTest.cs

*/ 
//*******************************************************************

using System;
using System.Threading;

using NUnit.Framework;

using Alfray.SneLib;

//*************************
namespace Alfray.SneLibTest
{
	//***************************************************
	/// <summary>
	/// Summary description for RSneListenerTest.
	/// </summary>
	[TestFixture]
	public class RSneListenerTest: RSneListener
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

		
		//****************************
		[SetUp]
		public void SetUp()
		{
			mServer = new RSneListenerTest();
		}


		//****************************
		[TearDown]
		public void TearDown()
		{
			mServer.EndListen();
		}

		//******************************
		[Test]
		public void TestIsAsynchronous()
		{
#if DEBUG
			Assert.IsFalse(mServer.IsAsynchronous);
#else
			Assert.IsTrue(mServer.IsAsynchronous);
#endif
		}

		//*************************************
		[Test]
		public void TestStartStopAcceptThread()
		{
			Assert.IsTrue(mServer.startAcceptThread());
			// Can I search for a thread by name?
			mServer.stopAcceptThread();
		}

		//************************
		[Test]
		public void TestBeginListen0()
		{
			Assert.IsTrue(mServer.BeginListen());
		}

		//************************
		[Test]
		public void TestBeginListen1()
		{
			Assert.IsTrue(mServer.BeginListen(kTestPort));
		}

		//************************
		[Test]
		public void TestEndListen()
		{
			Assert.IsTrue(mServer.BeginListen());
			mServer.EndListen();
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		RSneListenerTest	mServer;

		private		int    kTestPort = 1234;

	} // class RSneListenerTest
} // namespace Alfray.SneLibTest


//---------------------------------------------------------------
//
//	$Log: RSneListenerTest.cs,v $
//	Revision 1.1  2004/01/05 06:29:14  ralf
//	Asynchronous handling of listen.
//	Added RSneClient vs RSneListerner and RSneConnection and test classes.
//	
//---------------------------------------------------------------
