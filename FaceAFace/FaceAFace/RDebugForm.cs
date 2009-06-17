//*******************************************************************
/* 

 		Project:	FaceAFace
 		File:		RDebugForm.cs

*/ 
//*******************************************************************

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using Alfray.Faf.Utils;

//*********************************
namespace Alfray.Faf.ClientApp
{
	//**************************************
	/// <summary>
	/// Summary description for RDebugForm.
	/// </summary>
	public class RDebugForm: System.Windows.Forms.Form, RILog
	{
		//-------------------------------------------
		//----------- Public Constants --------------
		//-------------------------------------------


		//-------------------------------------------
		//----------- Public Properties -------------
		//-------------------------------------------


		//******************
		public bool CanClose
		{
			get
			{
				return mCanClose;
			}
			set
			{
				mCanClose = value;
			}
		}



		//-------------------------------------------
		//----------- Public Methods ----------------
		//-------------------------------------------

		
		//****************
		public RDebugForm()
		{
			// Required for Windows Form Designer support

			InitializeComponent();

			// Inits

			mTextLog.Text = "";
			mCanClose = false;
		}


		#region RILog Members

		//***********************
		public void Log(object o)
		{
			Log(o.ToString());
		}

		//***********************
		public void Log(string s)
		{
			System.Diagnostics.Debug.WriteLine(s);

			if (!s.EndsWith("\r\n"))
				s += "\r\n";

			mTextLog.Text += s;

			mTextLog.Select(mTextLog.TextLength, 0);
			mTextLog.ScrollToCaret();
			// mTextLog.Refresh();
		}

		#endregion


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		//****************************************************************
		private void mButtonClear_Click(object sender, System.EventArgs e)
		{
			mTextLog.Clear();
		}


		//****************************************************************
		private void RDebugForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!mCanClose)
			{
				// Simply hide it
				e.Cancel = true;
				this.Visible = false;
			}
		}


		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------

		//***********************************
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

		//********************************
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mTextLog = new System.Windows.Forms.TextBox();
			this.mButtonClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// mTextLog
			// 
			this.mTextLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mTextLog.Location = new System.Drawing.Point(8, 8);
			this.mTextLog.Multiline = true;
			this.mTextLog.Name = "mTextLog";
			this.mTextLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.mTextLog.Size = new System.Drawing.Size(536, 424);
			this.mTextLog.TabIndex = 0;
			this.mTextLog.Text = "mTextLog";
			// 
			// mButtonClear
			// 
			this.mButtonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonClear.Location = new System.Drawing.Point(472, 440);
			this.mButtonClear.Name = "mButtonClear";
			this.mButtonClear.TabIndex = 1;
			this.mButtonClear.Text = "Effacer";
			this.mButtonClear.Click += new System.EventHandler(this.mButtonClear_Click);
			// 
			// RDebugForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(552, 470);
			this.Controls.Add(this.mButtonClear);
			this.Controls.Add(this.mTextLog);
			this.MinimumSize = new System.Drawing.Size(296, 296);
			this.Name = "RDebugForm";
			this.Text = "RDebugForm";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RDebugForm_Closing);
			this.ResumeLayout(false);

		}

		#endregion

		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox mTextLog;
		private System.Windows.Forms.Button mButtonClear;


		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------


		private bool	mCanClose;


	} // class RDebugForm
} // namespace Alfray.Faf.ClientApp


//---------------------------------------------------------------
//	[C# Template RM 20040516]
//	$Log$
//---------------------------------------------------------------

