using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Socket
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class RFormMain : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button mButtonConnect;
		private System.Windows.Forms.TextBox mTextResult;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RFormMain()
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
			this.mButtonConnect = new System.Windows.Forms.Button();
			this.mTextResult = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// mButtonConnect
			// 
			this.mButtonConnect.Anchor = (System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.mButtonConnect.Location = new System.Drawing.Point(416, 144);
			this.mButtonConnect.Name = "mButtonConnect";
			this.mButtonConnect.TabIndex = 0;
			this.mButtonConnect.Text = "Connect";
			this.mButtonConnect.Click += new System.EventHandler(this.mButtonConnect_Click);
			// 
			// mTextResult
			// 
			this.mTextResult.Anchor = (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right);
			this.mTextResult.Location = new System.Drawing.Point(8, 8);
			this.mTextResult.Multiline = true;
			this.mTextResult.Name = "mTextResult";
			this.mTextResult.ReadOnly = true;
			this.mTextResult.Size = new System.Drawing.Size(480, 128);
			this.mTextResult.TabIndex = 1;
			this.mTextResult.Text = "";
			// 
			// RFormMain
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(496, 173);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.mTextResult,
																		  this.mButtonConnect});
			this.Name = "RFormMain";
			this.Text = "Node Client";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new RFormMain());
		}

		/// <summary>
		/// Connects to the server once the Connect button as been activated
		/// </summary>
		/// <param name="sender">Sender of the click event</param>
		/// <param name="e">System Event (click)</param>
		private void mButtonConnect_Click(object sender, System.EventArgs e)
		{
			// - RM - Socket		
		}
	}
}
