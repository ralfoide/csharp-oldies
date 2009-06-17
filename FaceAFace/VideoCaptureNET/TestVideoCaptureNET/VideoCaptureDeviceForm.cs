using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Allenwood.VideoCaptureNet;

namespace TestVideoCaptureNet
{
	/// <summary>
	/// Summary description for VideoCaptureDeviceForm.
	/// </summary>
	public class VideoCaptureDeviceForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Timer timer1;
		int TotalFramesCounted = 0;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Timer timer2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TrackBar trackBar2;

		VideoCaptureFrameEventArgs LastFrame = null;

		public void HandleFrame(object sender, VideoCaptureFrameEventArgs e)
		{
			TotalFramesCounted++;
			LastFrame = e;
			//this.label1.Text = ""+e.SampleTime + " size " + e.Bytes.Length;
			
		}


		public VideoCaptureDevice VideoCaptureDevice 
		{
			get
			{
				return VideoCaptureDeviceRep;
			}
			set
			{
				if (VideoCaptureDeviceRep != null)
					VideoCaptureDeviceRep.FrameCaptured -= new VideoCaptureFrameEventHandler(HandleFrame);
				VideoCaptureDeviceRep = value;
				if (VideoCaptureDeviceRep != null)
					VideoCaptureDeviceRep.FrameCaptured += new VideoCaptureFrameEventHandler(HandleFrame);
				this.listBox1.Items.Clear();
				foreach (Size s in VideoCaptureDeviceRep.GetResolutionCaps())
				{
					this.listBox1.Items.Add(s);
				}
				this.Text = value.Desc.Name;

				VideoCaptureDevice.Properties.Brightness.Value = VideoCaptureDevice.Properties.Brightness.Default; 
				VideoCaptureDevice.Properties.Contrast.Value = VideoCaptureDevice.Properties.Contrast.Default;
				ResetTrackbarsBasedOnProperties();


			}
		}
		VideoCaptureDevice VideoCaptureDeviceRep;

		private void ResetTrackbarsBasedOnProperties()
		{
			Console.WriteLine("Brightness is " + VideoCaptureDevice.Properties.Brightness);
			SetTrackbarBasedOnProperty(VideoCaptureDevice.Properties.Brightness, trackBar1);
			SetTrackbarBasedOnProperty(VideoCaptureDevice.Properties.Contrast, trackBar2);
		}


		public VideoCaptureDeviceForm()
		{
			//
			// Required for Windows Form Designer 
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
			this.components = new System.ComponentModel.Container();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.label2 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label3 = new System.Windows.Forms.Label();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.timer2 = new System.Windows.Forms.Timer(this.components);
			this.button2 = new System.Windows.Forms.Button();
			this.trackBar1 = new System.Windows.Forms.TrackBar();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.trackBar2 = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(232, 40);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(216, 48);
			this.button1.TabIndex = 0;
			this.button1.Text = "Start";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 216);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "Total Frames Counted:";
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 32);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(144, 160);
			this.listBox1.TabIndex = 3;
			this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(496, 23);
			this.label2.TabIndex = 4;
			this.label2.Text = "Resolutions Supported by Camera";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(224, 104);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(248, 224);
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 240);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(168, 24);
			this.label3.TabIndex = 6;
			this.label3.Text = "~";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 272);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(168, 16);
			this.label4.TabIndex = 7;
			this.label4.Text = "Frames Per Second:";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(8, 296);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(168, 16);
			this.label5.TabIndex = 8;
			// 
			// timer2
			// 
			this.timer2.Enabled = true;
			this.timer2.Interval = 1000;
			this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(16, 456);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(144, 32);
			this.button2.TabIndex = 9;
			this.button2.Text = "Return to Defaults";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// trackBar1
			// 
			this.trackBar1.Location = new System.Drawing.Point(8, 344);
			this.trackBar1.Name = "trackBar1";
			this.trackBar1.Size = new System.Drawing.Size(160, 45);
			this.trackBar1.TabIndex = 10;
			this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 320);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(168, 16);
			this.label6.TabIndex = 11;
			this.label6.Text = "Brightness:";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 384);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(168, 16);
			this.label7.TabIndex = 13;
			this.label7.Text = "Contrast:";
			// 
			// trackBar2
			// 
			this.trackBar2.Location = new System.Drawing.Point(8, 408);
			this.trackBar2.Name = "trackBar2";
			this.trackBar2.Size = new System.Drawing.Size(160, 45);
			this.trackBar2.TabIndex = 12;
			this.trackBar2.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
			// 
			// VideoCaptureDeviceForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(680, 526);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.trackBar2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.trackBar1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.listBox1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Name = "VideoCaptureDeviceForm";
			this.Text = "Video Capture Demo";
			this.Closed += new System.EventHandler(this.Form1_Closed);
			((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion


		private void button1_Click(object sender, System.EventArgs e)
		{
			if (!VideoCaptureDevice.Enabled)
			{
				button1.Text = "Stop";
				VideoCaptureDevice.Enabled = true;
			}
			else
			{
				button1.Text = "Start";
				VideoCaptureDevice.Enabled = false;
			}
		}

		private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Size s = (Size)listBox1.Items[listBox1.SelectedIndex];
			VideoCaptureDevice.Resolution = s;
			pictureBox1.Size = VideoCaptureDevice.Resolution;
			
		}

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			this.label3.Text = this.TotalFramesCounted + "";

			VideoCaptureFrameEventArgs lastFrame = LastFrame;
			if (lastFrame == null) return;
//			Byte[] bytesToUse = lastFrame.Bytes;
//			Bitmap b = new Bitmap(lastFrame.Resolution.Width, lastFrame.Resolution.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
//			BitmapData bmd = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
//			byte * scan0 = (byte*)bmd.Scan0.ToPointer();
//			int numToDo = lastFrame.Resolution.Width * lastFrame.Resolution.Height * 3;
//			for (int i = 0; i < numToDo; i++)
//			{
//				scan0[i] = bytesToUse[i];
//			}
//			b.UnlockBits(bmd);

			Bitmap b = lastFrame.GetBitmap();


			this.pictureBox1.Image = b;
			if (this.pictureBox1.Width != b.Width) this.pictureBox1.Width = b.Width;
			if (this.pictureBox1.Height != b.Height) this.pictureBox1.Height = b.Height; 
		}

		int LastSecondCount = 0;

		private void timer2_Tick(object sender, System.EventArgs e)
		{
			int totalCountedThisSecond = TotalFramesCounted - LastSecondCount;
			LastSecondCount = TotalFramesCounted;
			this.label5.Text = "" + totalCountedThisSecond;
		}

		private void Form1_Closed(object sender, System.EventArgs e)
		{
			VideoCaptureDevice.Enabled = false;
			VideoCaptureDevice.Dispose();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			VideoCaptureDevice.Properties.ResetToDefaultValues();
			ResetTrackbarsBasedOnProperties();
//			throw new Exception("I want to see what's loaded.");
		}

		void SetPropertyBasedOnTrackbar(VideoCaptureDeviceProperty vcdp, TrackBar tb)
		{
			float tbrange = (float)(tb.Maximum - tb.Minimum);
			float tbValue = (float)(tb.Value);
			float propRange = (float)(vcdp.Max - vcdp.Min);
			int newValue = vcdp.Min + (int)(propRange * tbValue/tbrange);
			vcdp.Value = newValue;
		}
		void SetTrackbarBasedOnProperty(VideoCaptureDeviceProperty vcdp, TrackBar tb)
		{
			float tbrange = (float)(tb.Maximum - tb.Minimum);
	//		float tbValue = (float)(tb.Value);
			float propRange = (float)(vcdp.Max - vcdp.Min);
			float propValue = (float)(vcdp.Value);
			int newValue = tb.Minimum + (int)(tbrange * propValue/propRange);
			tb.Value = newValue;
		}

		private void trackBar1_ValueChanged(object sender, System.EventArgs e)
		{
			SetPropertyBasedOnTrackbar(VideoCaptureDevice.Properties.Brightness, trackBar1);
		}

		private void trackBar2_ValueChanged(object sender, System.EventArgs e)
		{
			SetPropertyBasedOnTrackbar(VideoCaptureDevice.Properties.Contrast, trackBar2);		
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
		}

	}
}
