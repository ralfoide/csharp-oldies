//*******************************************************************
/* 

 		Project:	SneLib
 		File:		RSneConnection.cs

*/ 
//*******************************************************************

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;


//*********************
namespace Alfray.SneLib
{
	//*************************
	/// <summary>
	/// Handles a connection between an SNE server and an SNE client.
	/// </summary>
	//*************************
	public class RSneConnection
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//****************
		/// <summary>
		/// Indicate if the connection is open.
		/// </summary>
		//****************
		public bool IsOpen
		{
			get
			{
				return (mTcpClient != null && mTcpClient.GetStream() != null);
			}
		}


		//************************
		/// <summary>
		/// Returns the underlying .Net TCP Client.
		/// This is an 'advanced' feature. It is not recommended to directly act
		/// on the TcpClient object (for example, close it or transmit directly
		/// over the stream). On the other hand, this is handy to adjust the TCP
		/// timeouts or buffer sizes if you really know what you are doing.
		/// </summary>
		/// <remarks>
		/// This method may disapear in the future.
		/// </remarks>
		//************************
		public TcpClient TcpClient
		{
			get
			{
				return mTcpClient;
			}
		}


		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		

		//***********************************
		/// <summary>
		/// Create a connection on a specific TCP stream.
		/// </summary>
		/// <remarks>
		/// Do not create connections directly. Use RSneClient.Connect() instead.
		/// </remarks>
		//***********************************
		public RSneConnection(TcpClient conx)
		{
			mTcpClient = conx;
		}


		//*****************
		public void Close()
		{
			if (mTcpClient != null)
			{
				mTcpClient.Close();
				mTcpClient = null;
			}
		}


		//***************************
		public bool ClientNegotiate()
		{
			// get the stream
			Stream ns = mTcpClient.GetStream();

			return clientNegotiate(ns, ns);
		}


		//***************************
		public bool ServerNegotiate()
		{
			// get the stream
			Stream ns = mTcpClient.GetStream();

			return serverNegotiate(ns, ns);
		}



		//-------------------------------------------
		//----------- Protected Methods -------------
		//-------------------------------------------

		//********************************************************
		protected bool clientNegotiate(Stream in_ns, Stream out_ns)
		{
			// -1- Signature exchange:
			// a- The initiator sends its signature.

			if (!sendAsciiLine(out_ns, kSignature))
				return false;

			// b- The receiver either accepts the signature or closes the connection.
			// c- The receiver sends its signature.

			String sig = receiveAsciiLine(in_ns);
			if (sig == null)
				return false;

			// d- The initiator either accepts the signature or closes the connection.

			if (sig != kSignature)
				return false;


			return true;
		}


		//********************************************************
		protected bool serverNegotiate(Stream in_ns, Stream out_ns)
		{
			return true;
		}


		//******************************************************
		/// <summary>
		/// All protocol content is transmitted as pure ASCII with these parts:
		/// - The number of characters to follow (including space and \n).
		/// - A space separator.
		/// - The ASCII payload.
		/// - A \n terminator.
		/// </summary>
		/// <param name="out_ns">The output stream</param>
		/// <param name="data">The ASCII string to send</param>
		/// <returns>True if line was sent (even if empty)</returns>
		//******************************************************
		protected bool sendAsciiLine(Stream out_ns, String data)
		{
			byte [] buf = Encoding.ASCII.GetBytes(data);

			// since it's ascii, there one character per byte
			// plus 2 for the space and the \n

			data = String.Format("{0} ", buf.Length+2);
			byte [] header = Encoding.ASCII.GetBytes(data);

			out_ns.Write(header, 0, header.Length);
			out_ns.Write(buf, 0, buf.Length);
			out_ns.WriteByte((byte)kLineSep);
			
			return true;
		}


		//*********************************************
		/// <summary>
		/// Receives a pure ASCII protocol line from the network.
		/// This is the counterpart of sendAsciiLine.
		/// </summary>
		/// <param name="in_ns">The input stream</param>
		/// <returns>The decoded ASCII string or null if nothing was transmited or it was invalid</returns>
		//*********************************************
		protected String receiveAsciiLine(Stream in_ns)
		{
			read_ascii_from_in_ns(); // RM 20040120 to be continued

			return null;
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------



		//-------------------------------------------
		//----------- Protected Attributes ----------
		//-------------------------------------------

		protected const char   kLineSep = '\n';
		protected const String kSignature = "alfray.sne-1";


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		TcpClient	mTcpClient;

	} // class RSneConnection
} // namespace Alfray.SneLib


//---------------------------------------------------------------
//
//	$Log: RSneConnection.cs,v $
//	Revision 1.2  2004/01/20 09:02:33  ralf
//	Added clientNegotiate and sendAsciiLine
//	
//	Revision 1.1  2004/01/05 06:29:14  ralf
//	Asynchronous handling of listen.
//	Added RSneClient vs RSneListerner and RSneConnection and test classes.
//	
//---------------------------------------------------------------
