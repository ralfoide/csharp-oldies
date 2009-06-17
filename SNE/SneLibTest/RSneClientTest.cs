//*******************************************************************
/* 

 		Project:	SneLibTest
 		File:		RSneClientTest.cs

*/ 
//*******************************************************************

using System;
using NUnit.Framework;

using Alfray.SneLib;

//***************************
namespace Alfray.SneLibTest
{
	//***************************************************
	/// <summary>
	/// Summary description for RSneClientTest.
	/// </summary>
	//***************************************************
	[TestFixture]
	public class RSneClientTest
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
			mServer = new RSneListener();
			mServer.BeginListen();
		}


		//****************************
		[TearDown]
		public void TearDown()
		{
			mServer.EndListen();
		}


		//************************
		[Test]
		public void TestConnect0()
		{
			RSneConnection cnx = RSneClient.Connect();
			Assert.IsNotNull(cnx);
			cnx.Close();
		}

		//************************
		[Test]
		public void TestConnect1()
		{
			RSneConnection cnx = RSneClient.Connect(kTestHost);
			Assert.IsNotNull(cnx);
			cnx.Close();
		}

		//********************************
		[Test]
		public void TestConnectWrongPort()
		{
			// The default server (from SetUp) listens on the default
			// port so the next call should fail using a custom port
			RSneConnection cnx = RSneClient.Connect(kTestHost, kTestPort);
			Assert.IsNull(cnx);
		}

#if false
		//**********************************
		[Test]
		[Ignore("Default TCP connection timeout is too long.")]
		public void TestConnectWrongClient()
		{
			// Should also fail use a random host on the correct port
			RSneConnection cnx = RSneClient.Connect("192.168.255.255", kTestPort);
			Assert.IsNull(cnx);
		}
#endif

		//************************
		[Test]
		public void TestConnect2()
		{
			// Discard the default server and run one on the custom port
			mServer.EndListen();
			mServer = new RSneListener();
			mServer.BeginListen(kTestPort);

			RSneConnection cnx = RSneClient.Connect(kTestHost, kTestPort);
			Assert.IsNotNull(cnx);
			cnx.Close();
		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		RSneListener	mServer;

		private		string kTestHost = "localhost";
		private		int    kTestPort = 1234;

	} // class RSneClientTest
} // namespace Alfray.SneLib.Tests


//---------------------------------------------------------------
//
//	$Log: RSneClientTest.cs,v $
//	Revision 1.3  2004/01/05 06:29:14  ralf
//	Asynchronous handling of listen.
//	Added RSneClient vs RSneListerner and RSneConnection and test classes.
//	
//	Revision 1.2  2003/12/31 07:16:41  ralf
//	Tests: Test class deriving from tested class (to access inner protected methods).
//	Using SetUp, TearDown and using the test class as a mock object.
//	
//	Revision 1.1.1.1  2003/12/24 08:03:31  ralf
//	Empty skeleton with NUnit module
//	
//---------------------------------------------------------------
