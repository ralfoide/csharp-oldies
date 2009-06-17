using Allenwood.VideoCaptureNet;
using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace TestVideoCaptureNet
{
	/// <summary>
	/// Summary description for CameraSelector.
	/// </summary>
	public class CameraSelector : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CameraSelector()
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Location = new System.Drawing.Point(8, 32);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(336, 69);
			this.listBox1.TabIndex = 0;
			this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
			this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(24, 112);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(152, 32);
			this.button1.TabIndex = 1;
			this.button1.Text = "&View";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(184, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Available Video Capture Devices";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(208, 8);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(128, 24);
			this.button2.TabIndex = 3;
			this.button2.Text = "Refresh List";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// CameraSelector
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(352, 150);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.listBox1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CameraSelector";
			this.Text = "VideoCaptureNET Test Program";
			this.Load += new System.EventHandler(this.CameraSelector_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void CameraSelector_Load(object sender, System.EventArgs e)
		{
			RefreshVidCapDeviceList();

		}
		private void RefreshVidCapDeviceList()
		{
			try
			{
				this.listBox1.Items.Clear();
				foreach (VideoCaptureDeviceDesc vcdd in VideoCaptureDeviceDesc.GetAvailableDeviceDescs())
				{
					this.listBox1.Items.Add(vcdd);
				}
				if (listBox1.Items.Count > 0) listBox1.SelectedIndex = 0;
			}
			catch (Exception ee)
			{
				StreamWriter sw = System.IO.File.CreateText("exception2.txt");
				sw.WriteLine(ee);
				sw.WriteLine(ee.StackTrace);
				sw.WriteLine(ee.InnerException);
				sw.WriteLine(ee.Message);
				sw.WriteLine(ee.HelpLink);				
				sw.Flush();
				sw.Close();
				throw ee;
			}

		}

		private void listBox1_DoubleClick(object sender, System.EventArgs e)
		{
			DoAddForm();
		}

		private void listBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				DoAddForm();
			}
		}
		private void DoAddForm()
		{
			if (listBox1.SelectedItem == null) return;
			try
			{
				VideoCaptureDeviceForm f1 = new VideoCaptureDeviceForm();

				f1.VideoCaptureDevice = new VideoCaptureDevice(listBox1.SelectedItem as VideoCaptureDeviceDesc);
				f1.Show();
				f1.Focus();
			}
			catch (InvalidOperationException e)
			{
				MessageBox.Show("Can't show video capture device: " + e.Message);
			}
		}


		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new CameraSelector());
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			DoAddForm();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.RefreshVidCapDeviceList();
		}

	}
}
