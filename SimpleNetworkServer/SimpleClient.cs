//*******************************************************************
/* 

 		Project:	Simple Network Server
 		File:		SimpleServer.cs

*/ 
//*******************************************************************

using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using System.Diagnostics;

using Alfray.SimpleNetworkServer.Server;

//----------------------------
namespace Alfray.SimpleNetworkServer.Client
{
	/// <summary>
	/// Summary description for SimpleClient.
	/// </summary>
	public class SimpleClient
	{
		//---------------------------------------
		public SimpleClient()
		{
		}

		//---------------------------------------
		public string testSendDiscovery()
		{
			string result = "";

			// need to be disposed before leaving
			Socket sock;

			try
			{
				sock = new Socket(AddressFamily.InterNetwork,
					SocketType.Dgram,
					ProtocolType.Udp);

				// send to localhost on the server port
				IPEndPoint dest_ip = new IPEndPoint(IPAddress.Loopback, SimpleServer.kServerPort);

				// send packet
				byte [] buf_send = Encoding.ASCII.GetBytes("<list/>");
				int len = sock.SendTo(buf_send, dest_ip);
				result += String.Format("Sent {0} bytes\n", len);

				// receive buffer
				const int kBufSize = 65536;
				byte[] buf_rcv = new byte[kBufSize];

				// sender EP
				IPEndPoint sender_ip = new IPEndPoint(IPAddress.Any, 0);
				EndPoint sender_ep = (EndPoint)sender_ip;

				// wait for received message -- will block
				len = sock.ReceiveFrom(buf_rcv, kBufSize, SocketFlags.None, ref sender_ep);

				// transform into a string
				string str_rcv = Encoding.ASCII.GetString(buf_rcv, 0, len);

				result += String.Format("ReceiveFrom: len={0} -- buf='{1}' -- sender={2} ({3})",
					len, str_rcv, sender_ep.ToString(), sender_ip.ToString());
				Debug.Write(result);
			}
			catch(SocketException ex)
			{
				result += ex.ToString();
				Trace.Fail(ex.ToString());
			}
			catch(Exception ex)
			{
				result += ex.ToString();
				Trace.Fail(ex.ToString());
			}

			return result;
		}
	}
}

//---------------------------------------------------------------
//
//	$Log: SimpleClient.cs,v $
//	Revision 1.1  2003/06/10 04:42:30  ralf
//	Conforming to .Net naming rules
//	
//	Revision 1.1.1.1  2003/06/08 18:49:55  ralf
//	Initial revision
//	
//---------------------------------------------------------------
