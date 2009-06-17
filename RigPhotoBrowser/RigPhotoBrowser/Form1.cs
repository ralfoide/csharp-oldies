using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;



namespace Alfray.Rig.RigPhotoBrowser
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form, Alfray.Rig.AcqLib.RILog
	{
		private System.Windows.Forms.ListBox mListErrors;
		private System.Windows.Forms.Button mButtonListItems;
		private System.Windows.Forms.Button mButtonStart;
		private System.Windows.Forms.Button mButtonStop;
		private System.Windows.Forms.Button mButtonForm2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
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
			this.mButtonListItems = new System.Windows.Forms.Button();
			this.mListErrors = new System.Windows.Forms.ListBox();
			this.mButtonStart = new System.Windows.Forms.Button();
			this.mButtonStop = new System.Windows.Forms.Button();
			this.mButtonForm2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// mButtonListItems
			// 
			this.mButtonListItems.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonListItems.Location = new System.Drawing.Point(200, 152);
			this.mButtonListItems.Name = "mButtonListItems";
			this.mButtonListItems.TabIndex = 1;
			this.mButtonListItems.Text = "List Items";
			this.mButtonListItems.Click += new System.EventHandler(this.button2_Click);
			// 
			// mListErrors
			// 
			this.mListErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mListErrors.HorizontalScrollbar = true;
			this.mListErrors.Location = new System.Drawing.Point(8, 8);
			this.mListErrors.Name = "mListErrors";
			this.mListErrors.ScrollAlwaysVisible = true;
			this.mListErrors.Size = new System.Drawing.Size(272, 134);
			this.mListErrors.TabIndex = 2;
			// 
			// mButtonStart
			// 
			this.mButtonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonStart.Location = new System.Drawing.Point(8, 152);
			this.mButtonStart.Name = "mButtonStart";
			this.mButtonStart.TabIndex = 3;
			this.mButtonStart.Text = "Start";
			this.mButtonStart.Click += new System.EventHandler(this.mButtonStart_Click);
			// 
			// mButtonStop
			// 
			this.mButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonStop.Location = new System.Drawing.Point(96, 152);
			this.mButtonStop.Name = "mButtonStop";
			this.mButtonStop.TabIndex = 4;
			this.mButtonStop.Text = "Stop";
			this.mButtonStop.Click += new System.EventHandler(this.mButtonStop_Click);
			// 
			// mButtonForm2
			// 
			this.mButtonForm2.Location = new System.Drawing.Point(8, 184);
			this.mButtonForm2.Name = "mButtonForm2";
			this.mButtonForm2.TabIndex = 5;
			this.mButtonForm2.Text = "Form 2";
			this.mButtonForm2.Click += new System.EventHandler(this.mButtonForm2_Click);
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 214);
			this.Controls.Add(this.mButtonForm2);
			this.Controls.Add(this.mButtonStop);
			this.Controls.Add(this.mButtonStart);
			this.Controls.Add(this.mListErrors);
			this.Controls.Add(this.mButtonListItems);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		// ---------------

		#region RILog Members

		public void Log(string s)
		{
			// TODO:  Add Form1.Log implementation

			mListErrors.Items.Add(s);
			mListErrors.ClearSelected();
			mListErrors.SelectedIndex = mListErrors.Items.Count-1;
		}

		public void Log(object o)
		{
			this.Log(o.ToString());
		}

		#endregion

		// ---------------

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			mButtonStop_Click(sender, e);
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			if (mAcqInstance == null)
				mButtonStart_Click(sender, e);
			
			mAcqInstance.Test2();
		}

		private void mButtonStart_Click(object sender, System.EventArgs e)
		{
			if (mAcqInstance == null)
			{
				mAcqInstance = new Alfray.Rig.AcqLib.Class1();
				mAcqInstance.Start(this);
			}		
		}

		private void mButtonStop_Click(object sender, System.EventArgs e)
		{
			if (mAcqInstance != null)
			{
				mAcqInstance.Stop();
				mAcqInstance = null;
			}
		}

		private void mButtonForm2_Click(object sender, System.EventArgs e)
		{
			Form2 f2 = new Form2();
			f2.ShowDialog();
		}


		// ---------------

		AcqLib.Class1 mAcqInstance;
	}
}
