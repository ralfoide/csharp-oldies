//*******************************************************************
/* 

 		Project:	Simple Network Server
 		File:		TestForm.cs

*/ 
//*******************************************************************


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
			
using Alfray.SimpleNetworkServer.Client;
using Alfray.SimpleNetworkServer.Server;

namespace Alfray.TicTac
{
	/// <summary>
	/// Summary description for TestForm.
	/// </summary>
	public class TestForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TextBox mTextClientTest;
		private System.Windows.Forms.Button mButtonTestClientDiscovery;
		private System.Windows.Forms.TextBox mTextServerTest;
		private System.Windows.Forms.Button mButtonServerDiscoLoop;
		private System.Windows.Forms.Button mButtonServerDiscoQuit;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public TestForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.mTextClientTest = new System.Windows.Forms.TextBox();
			this.mButtonTestClientDiscovery = new System.Windows.Forms.Button();
			this.mTextServerTest = new System.Windows.Forms.TextBox();
			this.mButtonServerDiscoLoop = new System.Windows.Forms.Button();
			this.mButtonServerDiscoQuit = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.mButtonServerDiscoQuit);
			this.groupBox1.Controls.Add(this.mButtonServerDiscoLoop);
			this.groupBox1.Controls.Add(this.mTextServerTest);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(456, 144);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Server";
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.mButtonTestClientDiscovery);
			this.groupBox2.Controls.Add(this.mTextClientTest);
			this.groupBox2.Location = new System.Drawing.Point(8, 160);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(456, 176);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Client";
			// 
			// mTextClientTest
			// 
			this.mTextClientTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mTextClientTest.Location = new System.Drawing.Point(8, 16);
			this.mTextClientTest.Multiline = true;
			this.mTextClientTest.Name = "mTextClientTest";
			this.mTextClientTest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.mTextClientTest.Size = new System.Drawing.Size(440, 120);
			this.mTextClientTest.TabIndex = 0;
			this.mTextClientTest.Text = "";
			// 
			// mButtonTestClientDiscovery
			// 
			this.mButtonTestClientDiscovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonTestClientDiscovery.Location = new System.Drawing.Point(304, 144);
			this.mButtonTestClientDiscovery.Name = "mButtonTestClientDiscovery";
			this.mButtonTestClientDiscovery.Size = new System.Drawing.Size(136, 23);
			this.mButtonTestClientDiscovery.TabIndex = 1;
			this.mButtonTestClientDiscovery.Text = "Test Client Discovery";
			this.mButtonTestClientDiscovery.Click += new System.EventHandler(this.mButtonTestClientDiscovery_Click);
			// 
			// mTextServerTest
			// 
			this.mTextServerTest.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mTextServerTest.Location = new System.Drawing.Point(8, 16);
			this.mTextServerTest.Multiline = true;
			this.mTextServerTest.Name = "mTextServerTest";
			this.mTextServerTest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.mTextServerTest.Size = new System.Drawing.Size(440, 88);
			this.mTextServerTest.TabIndex = 0;
			this.mTextServerTest.Text = "";
			// 
			// mButtonServerDiscoLoop
			// 
			this.mButtonServerDiscoLoop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonServerDiscoLoop.Location = new System.Drawing.Point(8, 112);
			this.mButtonServerDiscoLoop.Name = "mButtonServerDiscoLoop";
			this.mButtonServerDiscoLoop.Size = new System.Drawing.Size(136, 23);
			this.mButtonServerDiscoLoop.TabIndex = 1;
			this.mButtonServerDiscoLoop.Text = "Server Discovery Loop";
			this.mButtonServerDiscoLoop.Click += new System.EventHandler(this.mButtonServerDiscoLoop_Click);
			// 
			// mButtonServerDiscoQuit
			// 
			this.mButtonServerDiscoQuit.Location = new System.Drawing.Point(336, 112);
			this.mButtonServerDiscoQuit.Name = "mButtonServerDiscoQuit";
			this.mButtonServerDiscoQuit.Size = new System.Drawing.Size(104, 23);
			this.mButtonServerDiscoQuit.TabIndex = 2;
			this.mButtonServerDiscoQuit.Text = "Quit Disco Loop";
			this.mButtonServerDiscoQuit.Click += new System.EventHandler(this.mButtonServerDiscoQuit_Click);
			// 
			// TestForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 342);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.MinimumSize = new System.Drawing.Size(480, 376);
			this.Name = "TestForm";
			this.Text = "TestForm";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void mButtonTestClientDiscovery_Click(object sender, System.EventArgs e)
		{
			try
			{
				mTextClientTest.Text += "----- Test ----\r\n";

				SimpleClient client = new SimpleClient();
				mTextClientTest.Text += client.testSendDiscovery();
			}
			catch(Exception ex)
			{
				mTextClientTest.Text += "** mButtonTestClientDiscovery_Click ERROR **\r\n" + ex.ToString();
			}
		}

		private void mButtonServerDiscoLoop_Click(object sender, System.EventArgs e)
		{
			try
			{
				mTextServerTest.Text += "----- Server Discovery Loop Started ----\r\n";

				SimpleServer serv = new SimpleServer();
				bool b = serv.DiscoveryLoop();
				mTextServerTest.Text += b.ToString();
			}
			catch(Exception ex)
			{
				mTextServerTest.Text += "** mButtonServerDiscoLoop_Click ERROR **\r\n" + ex.ToString();
			}
		}

		private void mButtonServerDiscoQuit_Click(object sender, System.EventArgs e)
		{
			try
			 {
				mTextServerTest.Text += "----- Stopping Discovery Loop ----\r\n";

				UdpClient udp = new UdpClient();
				IPEndPoint ip = new IPEndPoint(IPAddress.Loopback, SimpleServer.kServerPort);
				byte [] command = System.Text.Encoding.ASCII.GetBytes("<quit/>");
				udp.Send(command, command.Length, ip);
			}
			catch(Exception ex)
			{
				mTextServerTest.Text += "** mButtonServerDiscoQuit_Click ERROR **\r\n" + ex.ToString();
			}
		}
	}
}


//---------------------------------------------------------------
//
//	$Log: TestForm.cs,v $
//	Revision 1.1  2003/06/10 04:43:32  ralf
//	Conforming to .Net naming rules
//	
//---------------------------------------------------------------

