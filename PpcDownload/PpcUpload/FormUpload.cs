//-----------------------------------------------------------
/*
	Project:	PpcDownload
	Subproject:	PpcUpload

	Ralf (c) 2003
*/
//-----------------------------------------------------------
//-----------------------------------------------------------
/*

PpcUpload network protocol
--------------------------

PpcUpload waits for a TCP connection. It reads in a header followed
by a request. The TCP connection is closed if the header is unknown
or the request is not a valid action.
If valid, the reply is send back via the TCP connection.

All strings are UTF8 encoded *except* the protocol header string.
Command strings are UTF8 (since get contains a filename), as well as
the list replies.

[header]
	A string terminated by \n, with a fixed size
	"Alfray.PpcUpload-1.0" + LF (\n) => 21 bytes
[command]
	A string terminated by \n, with arguments
	"list"
	"get dir/filename" ("dir" is optional, / is not)
	LF (\n)
[end]


Reply for list:
	- A list of \n-terminated strings: "dir/filename <byte size>"
	- "dir" is optional. Root files are listed as "/filename <size>"
	- A last string "<end>" + \n
	- The < and > signs are not optional.

Reply for get:

[len]
	12 bytes of binary data:
	< 00 00 00 00 S1 S2 S3 S4 00 00 00 CS >
	4 zero bytes
	4 bytes for size (msb: S1 S2 S3 S4 => 0xS1S2S3S4)
	3 zero bytes
	1 byte indicating the type of checksum used:
		00 = none
		01 = uint32 sum
		02 = CRC32 (not implemented yet)
		03 = MD5   (not implemented yet)
		04 = SHA1  (not implemented yet)
	Size is a signed int (int32), so max size is 2 GB.
[data]
	<binary data stream>
[crc]
	8+ bytes of binary data:
	< 00 00 00 00 S1 S2 S3 S4 <SZ nb-bytes> >
	4 zero bytes
	4 bytes indicating the size of the checksum (00 00 00 00 if none).
	The bytes of the checksum.

*/
//-----------------------------------------------------------
//-----------------------------------------------------------


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;


//------------------------
namespace Alfray.PpcUpload
{

	//--------------------------------------------------
	/// <summary>
	/// Summary description for mFormUpload.
	/// </summary>
	public class mFormUpload : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ProgressBar mProgressUpload;
		private System.Windows.Forms.Label mLabelStatus;
		private System.Windows.Forms.GroupBox mGroupStatus;
		private System.Windows.Forms.Button mButtonStart;
		private System.Windows.Forms.Button mButtonStop;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		//------------------
		public mFormUpload()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		//-----------------------------------------------
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mProgressUpload = new System.Windows.Forms.ProgressBar();
			this.mLabelStatus = new System.Windows.Forms.Label();
			this.mGroupStatus = new System.Windows.Forms.GroupBox();
			this.mButtonStart = new System.Windows.Forms.Button();
			this.mButtonStop = new System.Windows.Forms.Button();
			this.mGroupStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// mProgressUpload
			// 
			this.mProgressUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mProgressUpload.Location = new System.Drawing.Point(0, 310);
			this.mProgressUpload.Name = "mProgressUpload";
			this.mProgressUpload.Size = new System.Drawing.Size(97, 23);
			this.mProgressUpload.TabIndex = 1;
			// 
			// mLabelStatus
			// 
			this.mLabelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mLabelStatus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mLabelStatus.Location = new System.Drawing.Point(98, 310);
			this.mLabelStatus.Name = "mLabelStatus";
			this.mLabelStatus.Size = new System.Drawing.Size(193, 23);
			this.mLabelStatus.TabIndex = 2;
			this.mLabelStatus.Text = "label1";
			this.mLabelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// mGroupStatus
			// 
			this.mGroupStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mGroupStatus.Controls.Add(this.mButtonStop);
			this.mGroupStatus.Controls.Add(this.mButtonStart);
			this.mGroupStatus.Location = new System.Drawing.Point(8, 8);
			this.mGroupStatus.Name = "mGroupStatus";
			this.mGroupStatus.Size = new System.Drawing.Size(272, 64);
			this.mGroupStatus.TabIndex = 0;
			this.mGroupStatus.TabStop = false;
			this.mGroupStatus.Text = "Connection...";
			// 
			// mButtonStart
			// 
			this.mButtonStart.Location = new System.Drawing.Point(8, 24);
			this.mButtonStart.Name = "mButtonStart";
			this.mButtonStart.TabIndex = 0;
			this.mButtonStart.Text = "Start";
			this.mButtonStart.Click += new System.EventHandler(this.mButtonStart_Click);
			// 
			// mButtonStop
			// 
			this.mButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonStop.Location = new System.Drawing.Point(184, 24);
			this.mButtonStop.Name = "mButtonStop";
			this.mButtonStop.TabIndex = 1;
			this.mButtonStop.Text = "Stop";
			this.mButtonStop.Click += new System.EventHandler(this.mButtonStop_Click);
			// 
			// mFormUpload
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 334);
			this.Controls.Add(this.mLabelStatus);
			this.Controls.Add(this.mGroupStatus);
			this.Controls.Add(this.mProgressUpload);
			this.Name = "mFormUpload";
			this.Text = "PocketPC Upload";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.mFormUpload_Closing);
			this.mGroupStatus.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		//-----------------------------------------------------------
		//-----------------------------------------------------------


		//-------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Alfray.PpcUpload.mFormUpload());
		}




		//-----------------------------------------------------------
		//-----------------------------------------------------------
		// Thread and TCP Connection Methods

		protected Thread mSyncThread;
		protected TcpListener mTcpServer;

		protected volatile bool mRunThread;

		public const int kServerPort = 13456;
		public const string kProtocolHeader = "Alfray.PpcUpload-1.0\n";
		public const string kProtocolAskList = "list\n";
		public const string kProtocolEndList = "<end>\n";
		public const char kProtocolSep = '\n';


		//--------------------------
		protected bool startThread()
		{
			if (mTcpServer == null)
			{
				this.mLabelStatus.Text = "Starting...";

				// create a TCP conx and bind to any address on selected port
				IPEndPoint ip = new IPEndPoint(IPAddress.Any, kServerPort);
	    
				// TcpListener server = new TcpListener(port);
				mTcpServer = new TcpListener(ip);

				// Start listening for client requests.
				mTcpServer.Start();
			}

			if (mSyncThread == null)
			{
				mRunThread = true;
				mSyncThread = new Thread(new ThreadStart(this.threadEntryPoint));
				mSyncThread.Name = "PpcUpload TCP Listener";
				mSyncThread.Start();
			}

			return true;
		}


		//-------------------------
		protected bool stopThread()
		{
			// stop accepting connections
			if (mTcpServer != null)
			{
				mTcpServer.Stop();
				mTcpServer = null;
			}

			// tell the upload thread it's time to quit
			mRunThread = false;

			// wait that the thread ends
			if (mSyncThread != null)
			{
				mSyncThread.Join();				// infine wait
				// mSyncThread.Join(10000);		// 10 s (10.000 ms)
				mSyncThread = null;
			}

			return true;
		}


		//-------------------------------
		protected void threadEntryPoint()
		{
			this.mLabelStatus.Text = "Started";

			while(this.mRunThread)
			{
				if (mTcpServer.Pending())
				{
					TcpClient tc = mTcpServer.AcceptTcpClient();
					NetworkStream ns = tc.GetStream();

					this.mLabelStatus.Text = "Connected";

					Debug.Assert(ns.CanRead && ns.CanWrite);
					if (ns.CanRead && ns.CanWrite)
					{
						string action;
						if (readHeader(ns) && readAction(ns, out action))
						{
							this.mLabelStatus.Text = action.Replace("\n", "");

							if (action == kProtocolAskList)
								sendFileList(ns);
							else if (action.StartsWith("get "))
								sendFile(ns, action.Substring(4));
						}
					}

					tc.Close();
					this.mLabelStatus.Text = "Disconnected";
				}
				else
				{
					Thread.Sleep(100);	// 100 ms, 1/10 s
				}
			}

			this.mLabelStatus.Text = "Stopped";
		}

		//-----------------------------------------------------------
		//-----------------------------------------------------------
		// Protocol Methods


		//-----------------------------------------
		protected bool readHeader(NetworkStream ns)
		{
			byte [] buffer = new byte[kProtocolHeader.Length];

			int nb_read = ns.Read(buffer, 0, buffer.Length);

			if (nb_read != buffer.Length)
				return false;

			string hdr = System.Text.Encoding.ASCII.GetString(buffer);
			return hdr == kProtocolHeader;
		}


		//--------------------------------------------------------------------
		protected bool readAction(NetworkStream ns, out string action)
		{
			action = readLine(ns);

			return (action != null);
		}


		//--------------------------------------------------------------------
		protected string readLine(NetworkStream ns)
			// reads a line of arbitrary length, terminated by a \n character
			// returns null if not valid string can be read
		{
			byte [] buffer = new byte[128];
			string result = "";
			int index = 0;

			while(true)
			{
				int b = ns.ReadByte();

				// end of stream... not a valid string, give up
				if (b == -1)
					return null;

				buffer[index++] = (byte)b;

				// end of line found, stop here
				if (b == kProtocolSep)
					break;

				if (index >= buffer.Length)
				{
					result += System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
					index = 0;
				}
			}

			if (index > 0)
				result += System.Text.Encoding.UTF8.GetString(buffer, 0, index);

			return result;
		}


		//-------------------------------------------
		protected bool sendFileList(NetworkStream ns)
		{
			string s = "/test.bin <56>\ntoto/titi.txt <123456789>\n<end>\n";
			byte [] buffer = System.Text.Encoding.UTF8.GetBytes(s);

			ns.Write(buffer, 0, buffer.Length);

			return true;
		}


		//----------------------------------------------------
		protected bool sendFile(NetworkStream ns, string path)
		{
			try
			{
				// get the size of the file
				FileInfo fi = new FileInfo(path);
				int size = (int) fi.Length;

				// get the file stream
				FileStream fs = new FileStream(path,
					FileMode.Open,
					FileAccess.Read,
					FileShare.Read);

				// send the header

				byte [] header_size = new byte[12] { 0,0,0,0, 0,0,0,0, 0,0,0,0 };

				header_size[7] = (byte)((size >>  0) & 0xFF);
				header_size[6] = (byte)((size >>  8) & 0xFF);
				header_size[5] = (byte)((size >> 16) & 0xFF);
				header_size[4] = (byte)((size >> 24) & 0x7F);

				// currently using the uint32 sum type
				header_size[11] = 0; // 0 = none, 1 = uint32 sum
				
				ns.Write(header_size, 0, header_size.Length);

				// send the file, computing the checksum on the fly

				uint sum = 0;

				const int buflen = 4096;
				byte [] buffer = new byte[buflen];

				while(size > 0)
				{
					// read
					int n = fs.Read(buffer, 0, size > buflen ? buflen : size);
					size -= n;

					// checksum stuff
					// TBDL...

					// write
					ns.Write(buffer, 0, n);
				}

				// send the footer

				byte [] footer_crc = new byte[8] { 0,0,0,0, 0,0,0,0, };

				int checksum_size = 0;

				footer_crc[7] = (byte)((checksum_size >>  0) & 0xFF);
				footer_crc[6] = (byte)((checksum_size >>  8) & 0xFF);
				footer_crc[5] = (byte)((checksum_size >> 16) & 0xFF);
				footer_crc[4] = (byte)((checksum_size >> 24) & 0xFF);

				ns.Write(footer_crc, 0, footer_crc.Length);
			}
			catch(Exception ex)
			{
				Debug.Fail(ex.ToString());
			}

			return true;
		}


		//-----------------------------------------------------------
		//-----------------------------------------------------------
		// UI Callback

		private void mButtonStart_Click(object sender, System.EventArgs e)
		{
			this.startThread();
		}

		private void mButtonStop_Click(object sender, System.EventArgs e)
		{
			this.stopThread();
		}

		private void mFormUpload_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!this.stopThread())
				e.Cancel = true;
		}

	}
}

//-----------------------------------------------------------
//	$Log: FormUpload.cs,v $
//	Revision 1.2  2003/07/11 17:51:45  ralf
//	File upload
//	
//	Revision 1.1.1.1  2003/07/07 16:00:58  ralf
//	Initial Version
//	
//-----------------------------------------------------------

