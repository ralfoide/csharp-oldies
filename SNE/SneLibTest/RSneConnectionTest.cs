//*******************************************************************
/* 

 		Project:	SneLibTest
 		File:		RSneConnectionTest.cs

*/ 
//*******************************************************************

using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

using NUnit.Framework;

using Alfray.SneLib;

//*************************************
namespace Alfray.SneLibTest
{
	//***************************************************
	/// <summary>
	/// Summary description for RSneConnectionTest.
	/// </summary>
	//***************************************************
	[TestFixture]
	public class RSneConnectionTest: RSneConnection
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


		//*************************************
		public RSneConnectionTest(): base(null)
		{
		}


		//***********************************************
		public RSneConnectionTest(TcpClient tc): base(tc)
		{
		}

		//****************************
		[SetUp]
		public void SetUp()
		{
			mServer = new RSneListener();
			mServer.BeginListen(kTestPort);
		}


		//****************************
		[TearDown]
		public void TearDown()
		{
			closeConnections();
			mServer.EndListen();
			mServer = null;
		}


		//************************
		[Test]
		public void TestTcpClient()
		{
			// the connection doesn't exists at first
			Assert.IsNull(mConx);

			// open it
			Assert.IsTrue(openConnection());

			// now the object should be present
			Assert.IsNotNull(mConx.TcpClient);

			// close it
			mConx.Close();

			// now it should not be present
			Assert.IsNull(mConx.TcpClient);
		}

		
		//******************************
		[Test]
		public void TestIsOpenAndClose()
		{
			// the connection is initially closed
			Assert.IsNull(mConx);

			// open it
			Assert.IsTrue(openConnection());

			// now it should be open
			Assert.IsTrue(mConx.IsOpen);

			// close it
			mConx.Close();

			// now it should be closed
			Assert.IsFalse(mConx.IsOpen);
		}


		//************************
		[Test]
		public void TestSendLine()
		{
			MemoryStream out_ns = new MemoryStream();			

			Assert.IsTrue(this.sendAsciiLine(out_ns, kSignature));

			String out_str = Encoding.ASCII.GetString(out_ns.ToArray());
			String result = String.Format("{0} {1}{2}", kSignature.Length+2, kSignature, kLineSep);
			Assert.AreEqual(result, "14 alfray.sne-1\n");
			Assert.AreEqual(out_str, result);
		}


		//*****************************************
		[Test]
		public void TestClientNegatioateProtected()
		{
			String receiver = ""; // "14 alfray.sne-1\n";
			MemoryStream in_ns = new MemoryStream(Encoding.ASCII.GetBytes(receiver));
			MemoryStream out_ns = new MemoryStream();			

			Assert.IsTrue(this.clientNegotiate(in_ns, out_ns));

			String out_str = Encoding.ASCII.GetString(out_ns.ToArray());
			Assert.AreEqual(out_str, "14 alfray.sne-1\n");
		}


		//*****************************************
		[Test]
		public void TestServerNegatioateProtected()
		{
			String input = "";

			Assert.IsTrue(this.serverNegotiate(null, null));
		}



		//*******************************
		[Test]
		public void TestClientNegotiate()
		{
			// create a partial connection (open but not negotiated yet)
			Assert.IsTrue(openPartialConnection());

			// negotiate the protocol
			Assert.IsTrue(mConx.ClientNegotiate());
		}


		//*******************************
		[Test]
		public void TestServerNegotiate()
		{
			// TBDL: I have no idea how to test for this...
			// Assert.IsTrue(mConx.ServerNegotiate());
		}



		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//*****************************
		private void closeConnections()
		{
			if (mConx != null)
			{
				mConx.Close();
				mConx = null;
			}

			if (mConxTest != null)
			{
				mConxTest.Close();
				mConxTest = null;
			}
		}

		//***************************
		private bool openConnection()
		{
			closeConnections();

			// create a "regular" RSneConnection object
			// Note that the value will be NULL if negotiation failed
			mConx = RSneClient.Connect(kTestHost, kTestPort);

			// HACK: impersonate it as a RSneConnectionTest for testing protected methods
			// IMPORTANT: Test methods must use either mConx or mConxTest but cannot mix both!
			if (mConx != null)
				mConxTest = new RSneConnectionTest(mConx.TcpClient);

			return mConx != null && mConxTest != null;
		}


		//**********************************
		private bool openPartialConnection()
		{
			closeConnections();

			// initiate a fake connection
			TcpClient client = new TcpClient();
			client.Connect(kTestHost, kTestPort);
			mConx = new RSneConnectionTest(client);

			// HACK: impersonate it as a RSneConnectionTest for testing protected methods
			// IMPORTANT: Test methods must use either mConx or mConxTest but cannot mix both!
			if (mConx != null)
				mConxTest = new RSneConnectionTest(mConx.TcpClient);

			return mConx != null && mConxTest != null;
		}

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		RSneListener		mServer;
		RSneConnectionTest	mConxTest;
		RSneConnection		mConx;


		private		string kTestHost = "localhost";
		private		int    kTestPort = 1234;


	} // class RSneConnectionTest
} // namespace Alfray.SneLibTest


//---------------------------------------------------------------
//
//	$Log: RSneConnectionTest.cs,v $
//	Revision 1.2  2004/01/20 09:02:34  ralf
//	Added clientNegotiate and sendAsciiLine
//	
//	Revision 1.1  2004/01/05 06:29:14  ralf
//	Asynchronous handling of listen.
//	Added RSneClient vs RSneListerner and RSneConnection and test classes.
//	
//---------------------------------------------------------------
