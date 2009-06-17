//*******************************************************************
/* 

 		Project:	SneLib
 		File:		RSneClient.cs

*/ 
//*******************************************************************

using System;
using System.Net;
using System.Net.Sockets;

//*********************
namespace Alfray.SneLib
{
	//*********************
	/// <summary>
	/// Opens a client-side connection to connect to a SNE server.
	/// </summary>
	//*********************
	public class RSneClient
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------

		/// <summary>
		/// The default host name.
		/// </summary>
		public const string kDefaultHost = "localhost";

		/// <summary>
		/// The default port number.
		/// </summary>
		public const int    kDefaultPort = 31415;


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//************************************
		/// <summary>
		/// Opens a connection on the default host and the default port.
		/// </summary>
		/// <returns>
		/// A connection object to handle the connection or null if failed
		/// </returns>
		//************************************
		public static RSneConnection Connect()
		{
			return Connect(kDefaultHost, kDefaultPort);
		}


		//***********************************************
		/// <summary>
		/// Opens a connection on the specified host and the default port.
		/// </summary>
		/// <param name="host">The host to connect to (IP or DNS name)</param>
		/// <returns>
		/// A connection object to handle the connection or null if failed
		/// </returns>
		//***********************************************
		public static RSneConnection Connect(string host)
		{
			return Connect(host, kDefaultPort);
		}


		//*********************************************************
		/// <summary>
		/// Create a TcpClient to connect to the give host & port
		/// and return a connection object to handle it.
		/// 
		/// Returns null if failed to contact the server or failed to
		/// negotiate the protocol. For simplicity purposes, this method
		/// hides why the connection failed. That may need to be changed later.
		/// </summary>
		/// <param name="host">The host to connect to (IP or DNS name)</param>
		/// <param name="port">The TCP port to connect to</param>
		/// <returns>
		/// A connection object to handle the connection or null if failed
		/// </returns>
		//*********************************************************
		public static RSneConnection Connect(string host, int port)
		{
			try
			{
				// Create a TcpClient to connect to the give host & port
				TcpClient client = new TcpClient();

				// and connect...
				client.Connect(host, port);

				// create a connection object to handle it
				RSneConnection cnx = new RSneConnection(client);

				// negotiate the protocol
				if (!cnx.ClientNegotiate())
				{
					// close the connection if negotiation failed
					cnx.Close();
					cnx = null;
				}

				return cnx;
			}
			catch(Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message, "RSneClient.Connect: ");
				return null;
			}
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------



		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


	} // class RSneClient
} // namespace Alfray.SneLib


//---------------------------------------------------------------
//
//	$Log: RSneClient.cs,v $
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
