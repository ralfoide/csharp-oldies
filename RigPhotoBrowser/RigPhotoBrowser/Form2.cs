using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.IO;
using System.Drawing.Imaging;

namespace Alfray.Rig.RigPhotoBrowser
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.ListBox mListBox;
		private System.Windows.Forms.PictureBox mPictureBox;
		private System.Windows.Forms.Button mButtonGetList;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form2()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//

			mItems = new Hashtable();
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
			this.mListBox = new System.Windows.Forms.ListBox();
			this.mPictureBox = new System.Windows.Forms.PictureBox();
			this.mButtonGetList = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// mListBox
			// 
			this.mListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.mListBox.Location = new System.Drawing.Point(8, 8);
			this.mListBox.Name = "mListBox";
			this.mListBox.Size = new System.Drawing.Size(120, 277);
			this.mListBox.TabIndex = 0;
			this.mListBox.SelectedIndexChanged += new System.EventHandler(this.mListBox_SelectedIndexChanged);
			// 
			// mPictureBox
			// 
			this.mPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.mPictureBox.Location = new System.Drawing.Point(136, 8);
			this.mPictureBox.Name = "mPictureBox";
			this.mPictureBox.Size = new System.Drawing.Size(320, 280);
			this.mPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.mPictureBox.TabIndex = 1;
			this.mPictureBox.TabStop = false;
			// 
			// mButtonGetList
			// 
			this.mButtonGetList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.mButtonGetList.Location = new System.Drawing.Point(8, 296);
			this.mButtonGetList.Name = "mButtonGetList";
			this.mButtonGetList.TabIndex = 2;
			this.mButtonGetList.Text = "Get List";
			this.mButtonGetList.Click += new System.EventHandler(this.mButtonGetList_Click);
			// 
			// Form2
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(472, 334);
			this.Controls.Add(this.mButtonGetList);
			this.Controls.Add(this.mPictureBox);
			this.Controls.Add(this.mListBox);
			this.Name = "Form2";
			this.Text = "Form2";
			this.ResumeLayout(false);

		}
		#endregion

		private void mButtonGetList_Click(object sender, System.EventArgs e)
		{
			mListBox.Items.Clear();
			mItems.Clear();

			string [] f = Directory.GetFiles(@"c:\temp", "thumb*.jpg");

			foreach(string s in f)
			{
				SItem i = new SItem(s);

				mItems[i.mShortName] = i;
				mListBox.Items.Add(i);
			}
		}

		private void mListBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			SItem item = mListBox.SelectedItem as SItem;

			System.Diagnostics.Debug.WriteLine(item.ToString());

			item.LoadBuffer();
			item.CreateBitmap();

			mPictureBox.Image = item.mImage;
		}

		//----------

		Hashtable mItems;

		class SItem
		{
			public string mShortName;
			public string mFileName;
			public Image mImage;
			public byte [] mBuffer;

			public SItem(string s)
			{
				mFileName = s;
				mShortName = Path.GetFileNameWithoutExtension(s);
			}

			public override string ToString()
			{
				return mShortName;
			}

			public void LoadBuffer()
			{
				if (mBuffer != null)
					return;
				if (!File.Exists(mFileName))
					return;

				try
				{
					FileStream fs = new FileStream(mFileName, FileMode.Open);
					int sz = (int) fs.Length;
					mBuffer = new byte[sz];
					fs.Read(mBuffer, 0, sz);
					fs.Close();
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
			}

			public void CreateBitmap()
			{
				if (mImage != null)
					return;

				if (mBuffer == null)
					return;

				try
				{
					unsafe
					{
						fixed(void *data = mBuffer)
						{
							IntPtr ptr = new IntPtr(data);
							mImage = new Bitmap(160, 120, 160*3, PixelFormat.Format24bppRgb, ptr);

							mImage.RotateFlip(RotateFlipType.Rotate180FlipY);
						}
					}
				}
				catch(Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}
			}
		}

	
	}
}
