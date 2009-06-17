using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Alfray.HumCam.TestForm
{
	/// <summary>
	/// Summary description for FormTest.
	/// </summary>
	public class FormTest : System.Windows.Forms.Form
	{
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.PictureBox mPictureImage;
		private System.Windows.Forms.Button mButtonLoad;
		private System.Windows.Forms.Button mButtonQuit;
		private System.Windows.Forms.Button mButtonBlocks;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public FormTest()
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
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.mPictureImage = new System.Windows.Forms.PictureBox();
			this.mButtonLoad = new System.Windows.Forms.Button();
			this.mButtonQuit = new System.Windows.Forms.Button();
			this.mButtonBlocks = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 512);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(680, 22);
			this.statusBar1.TabIndex = 0;
			this.statusBar1.Text = "statusBar1";
			// 
			// mPictureImage
			// 
			this.mPictureImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mPictureImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureImage.Location = new System.Drawing.Point(8, 8);
			this.mPictureImage.Name = "mPictureImage";
			this.mPictureImage.Size = new System.Drawing.Size(664, 464);
			this.mPictureImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.mPictureImage.TabIndex = 1;
			this.mPictureImage.TabStop = false;
			// 
			// mButtonLoad
			// 
			this.mButtonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonLoad.Location = new System.Drawing.Point(8, 480);
			this.mButtonLoad.Name = "mButtonLoad";
			this.mButtonLoad.TabIndex = 2;
			this.mButtonLoad.Text = "Load";
			this.mButtonLoad.Click += new System.EventHandler(this.mButtonLoad_Click);
			// 
			// mButtonQuit
			// 
			this.mButtonQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.mButtonQuit.Location = new System.Drawing.Point(600, 480);
			this.mButtonQuit.Name = "mButtonQuit";
			this.mButtonQuit.TabIndex = 3;
			this.mButtonQuit.Text = "Quit";
			this.mButtonQuit.Click += new System.EventHandler(this.mButtonQuit_Click);
			// 
			// mButtonBlocks
			// 
			this.mButtonBlocks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonBlocks.Location = new System.Drawing.Point(96, 480);
			this.mButtonBlocks.Name = "mButtonBlocks";
			this.mButtonBlocks.TabIndex = 4;
			this.mButtonBlocks.Text = "Blocks";
			this.mButtonBlocks.Click += new System.EventHandler(this.mButtonBlocks_Click);
			// 
			// FormTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 534);
			this.Controls.Add(this.mButtonBlocks);
			this.Controls.Add(this.mButtonQuit);
			this.Controls.Add(this.mButtonLoad);
			this.Controls.Add(this.mPictureImage);
			this.Controls.Add(this.statusBar1);
			this.Name = "FormTest";
			this.Text = "FormTest";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new FormTest());
		}

		//-------------------------------------------
		//----------- Private Methods ---------------
		//-------------------------------------------


		private void mButtonQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void mButtonLoad_Click(object sender, System.EventArgs e)
		{
			mCamImage = new CamImage();
			mCamImage.LoadImage(@"D:\RalfDev\csharp\HumCam\Picts\" + "00-00.jpg");
			mCamImage.DisplayImage(mPictureImage);
		}


		private void mButtonBlocks_Click(object sender, System.EventArgs e)
		{
			if (mCamImage != null)
			{
				mCamImage.CreateBlocks();
				mCamImage.DisplayImage(mPictureImage);
			}
		}
		
		//-------------------------------------------
		//----------- Private Attributes ------------
		//-------------------------------------------

		CamImage mCamImage;

	}
}
