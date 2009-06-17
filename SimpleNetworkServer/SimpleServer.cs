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
using System.Text.RegularExpressions;

//----------------------------
namespace Alfray.SimpleNetworkServer.Server
{


	//----------------------------------------------------------
	/// <summary>
	/// Summary description for SimpleServer.
	/// </summary>
	public class SimpleServer
	{
		// ---------- Public Constants -------------

		public const int kServerPort = 1342;
		
		// ---------- Public Properties -------------


		//-------------------------------
		public Entities Entities
		{
			get
			{
				return mEntities;
			}
		}



		// ---------- Public Methods -------------

		//--------------------
		public SimpleServer()
		{
			mEntities = new Entities();
		}



		// -----------------------
		// ---- UDP discovery ----
		// -----------------------


		public bool DiscoveryLoop()
		{
			return doDiscoveryThread();
		}


		// -------------------------
		// ---- TCP connections ----
		// -------------------------


		public bool ReceiveConnect(int state_id, Object state_params)
		{
			return false;
		}

		public bool ReceiveClose(int state_id)
		{
			return false;
		}

		public bool ReceiveMessage(int state_id, Object state_params)
		{
			return false;
		}

		public bool SendMessage(int state_id, Object state_params)
		{
			return false;
		}



		// ---------- Private Methods -------------


		protected internal bool doDiscoveryThread()
		{
			Socket sock;

			try
			{
				// create UDP socket and bind to any address on selected port
				IPEndPoint ip = new IPEndPoint(IPAddress.Any, kServerPort);

				sock = new Socket(AddressFamily.InterNetwork,
					SocketType.Dgram,
					ProtocolType.Udp);

				sock.Bind(ip);

				// receive buffer
				const int kBufSize = 65536;
				byte[] buf = new byte[kBufSize];

				// sender EP
				EndPoint sender_ep = (EndPoint)ip;

				// a regexp to split string by lines
//				Regex regexp_commands = new Regex("<[a-z]+>",
//							RegexOptions.IgnorePatternWhitespace +
//							RegexOptions.Multiline +
//							RegexOptions.IgnoreCase);

				// loop on reception
				while(true)
				{
					int len = sock.ReceiveFrom(buf, kBufSize, SocketFlags.None, ref sender_ep);

					// transform into a string
					string str = Encoding.ASCII.GetString(buf, 0, len);

					Debug.Write(String.Format("ReceiveFrom: len={0} -- buf='{1}' -- sender={2} ({3})",
						len, str, sender_ep.ToString(), ip.ToString()));
					// Debug.Write("ReceiveFrom: len="+len.ToString()+" -- buf='"+str+"' -- sender="+sender_ep.ToString()+" ("+ip.ToString()+")");

					// extract commands
					if (str.IndexOf("<quit/>") >= 0)
					{
						break;
					}
					else if (str.IndexOf("<list/>") >= 0)
					{
						ArrayList list = Entities.Enumerate();
						string str_reply = "<reply>\n";
						if (list != null)
							foreach(EntityDesc ent in list)
								str_reply += String.Format("<entity id=\"{0}\" type=\"{1}\">{2}</entity>\n",
									ent.mId, ent.mType, ent.mDesc);						
						str_reply += "<\reply>\n";

						byte [] buf_reply = Encoding.ASCII.GetBytes(str_reply);

						sock.SendTo(buf_reply, sender_ep);
					}
				}

				sock.Close();
			}
			catch(SocketException ex)
			{
				Trace.Fail(ex.ToString());
				return false;
			}
			catch(Exception ex)
			{
				Trace.Fail(ex.ToString());
				return false;
			}

			return true;
		}



		// ---------- Private Attributes -------------

		// Entities storage
		protected internal Entities mEntities;

	} // SimpleServer

}


//---------------------------------------------------------------
//
//	$Log: SimpleServer.cs,v $
//	Revision 1.1  2003/06/10 04:42:30  ralf
//	Conforming to .Net naming rules
//	
//	Revision 1.1.1.1  2003/06/08 18:49:55  ralf
//	Initial revision
//	
//---------------------------------------------------------------

