/*
 * Project:		PrintDirXP
 * Language:	C#
 * Author:		RM, 20020901
 * 
 * 
 * .Net printing help:
 * ms-help://MS.VSCC/MS.MSDNVS/cpref/html/frlrfsystemdrawingprintingprintdocumentclasstopic.htm
*/


using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;				// RM 20020901 for DirectoryInfo
using System.Drawing.Printing;	// RM 20020901 for printing
using System.Diagnostics;		// RM 20020901 for Debug.Assert

namespace PrintDirXP
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class PrintDirForm : System.Windows.Forms.Form
	{
		// printing -- RM 20020901
		private ArrayList		mDirectoryText;
		private string			mHeaderDirText;
		private PrintDocument	mPrintDoc;
		private PageSettings	mPageSettings;
		private int				mPrintCurrentCount;

		// UI components
		private System.Windows.Forms.GroupBox GroupSelection;
		private System.Windows.Forms.GroupBox GroupContentPreview;
		private System.Windows.Forms.GroupBox GroupPrint;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox mEditDirPath;
		private System.Windows.Forms.Button mButtonBrowseDir;
		private System.Windows.Forms.TextBox mEditDirContent;
		private System.Windows.Forms.Button mButtonPrint;
		private System.Windows.Forms.Button mButtonPrintSetup;
		private System.Windows.Forms.Button mButtonPrintPreview;
		private System.Windows.Forms.Button mButtonQuit;
		private System.Windows.Forms.PageSetupDialog mPageSetupDialog;
		private System.Windows.Forms.PrintPreviewDialog mPrintPreviewDialog;
		private System.Windows.Forms.PrintDialog mPrintDialog;
		private System.Windows.Forms.FontDialog mPrintFontDialog;

		
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public PrintDirForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// constructor code after InitializeComponent call
			//
			GetLastUsedPath();
			ParseSelectedDirectory();

			// init settings for printing
			mPrintDoc = new PrintDocument();
			mPrintDoc.PrintPage += new PrintPageEventHandler(this.printDocPage);

			mPageSettings = new PageSettings();

			mPrintDoc.DefaultPageSettings = mPageSettings;

			mPageSetupDialog.Document = mPrintDoc;
			mPageSetupDialog.PageSettings = mPageSettings;

			mPrintPreviewDialog.Document = mPrintDoc;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PrintDirForm));
			this.mEditDirPath = new System.Windows.Forms.TextBox();
			this.mButtonBrowseDir = new System.Windows.Forms.Button();
			this.GroupSelection = new System.Windows.Forms.GroupBox();
			this.GroupContentPreview = new System.Windows.Forms.GroupBox();
			this.mEditDirContent = new System.Windows.Forms.TextBox();
			this.GroupPrint = new System.Windows.Forms.GroupBox();
			this.mButtonPrint = new System.Windows.Forms.Button();
			this.mButtonPrintSetup = new System.Windows.Forms.Button();
			this.mButtonPrintPreview = new System.Windows.Forms.Button();
			this.mButtonQuit = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.mPageSetupDialog = new System.Windows.Forms.PageSetupDialog();
			this.mPrintPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
			this.mPrintDialog = new System.Windows.Forms.PrintDialog();
			this.mPrintFontDialog = new System.Windows.Forms.FontDialog();
			this.GroupSelection.SuspendLayout();
			this.GroupContentPreview.SuspendLayout();
			this.GroupPrint.SuspendLayout();
			this.SuspendLayout();
			// 
			// mEditDirPath
			// 
			this.mEditDirPath.AccessibleDescription = ((string)(resources.GetObject("mEditDirPath.AccessibleDescription")));
			this.mEditDirPath.AccessibleName = ((string)(resources.GetObject("mEditDirPath.AccessibleName")));
			this.mEditDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mEditDirPath.Anchor")));
			this.mEditDirPath.AutoSize = ((bool)(resources.GetObject("mEditDirPath.AutoSize")));
			this.mEditDirPath.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mEditDirPath.BackgroundImage")));
			this.mEditDirPath.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mEditDirPath.Dock")));
			this.mEditDirPath.Enabled = ((bool)(resources.GetObject("mEditDirPath.Enabled")));
			this.mEditDirPath.Font = ((System.Drawing.Font)(resources.GetObject("mEditDirPath.Font")));
			this.mEditDirPath.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mEditDirPath.ImeMode")));
			this.mEditDirPath.Location = ((System.Drawing.Point)(resources.GetObject("mEditDirPath.Location")));
			this.mEditDirPath.MaxLength = ((int)(resources.GetObject("mEditDirPath.MaxLength")));
			this.mEditDirPath.Multiline = ((bool)(resources.GetObject("mEditDirPath.Multiline")));
			this.mEditDirPath.Name = "mEditDirPath";
			this.mEditDirPath.PasswordChar = ((char)(resources.GetObject("mEditDirPath.PasswordChar")));
			this.mEditDirPath.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mEditDirPath.RightToLeft")));
			this.mEditDirPath.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("mEditDirPath.ScrollBars")));
			this.mEditDirPath.Size = ((System.Drawing.Size)(resources.GetObject("mEditDirPath.Size")));
			this.mEditDirPath.TabIndex = ((int)(resources.GetObject("mEditDirPath.TabIndex")));
			this.mEditDirPath.Text = resources.GetString("mEditDirPath.Text");
			this.mEditDirPath.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("mEditDirPath.TextAlign")));
			this.mEditDirPath.Visible = ((bool)(resources.GetObject("mEditDirPath.Visible")));
			this.mEditDirPath.WordWrap = ((bool)(resources.GetObject("mEditDirPath.WordWrap")));
			this.mEditDirPath.TextChanged += new System.EventHandler(this.mEditDirPath_TextChanged);
			// 
			// mButtonBrowseDir
			// 
			this.mButtonBrowseDir.AccessibleDescription = ((string)(resources.GetObject("mButtonBrowseDir.AccessibleDescription")));
			this.mButtonBrowseDir.AccessibleName = ((string)(resources.GetObject("mButtonBrowseDir.AccessibleName")));
			this.mButtonBrowseDir.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonBrowseDir.Anchor")));
			this.mButtonBrowseDir.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonBrowseDir.BackgroundImage")));
			this.mButtonBrowseDir.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonBrowseDir.Dock")));
			this.mButtonBrowseDir.Enabled = ((bool)(resources.GetObject("mButtonBrowseDir.Enabled")));
			this.mButtonBrowseDir.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonBrowseDir.FlatStyle")));
			this.mButtonBrowseDir.Font = ((System.Drawing.Font)(resources.GetObject("mButtonBrowseDir.Font")));
			this.mButtonBrowseDir.Image = ((System.Drawing.Image)(resources.GetObject("mButtonBrowseDir.Image")));
			this.mButtonBrowseDir.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonBrowseDir.ImageAlign")));
			this.mButtonBrowseDir.ImageIndex = ((int)(resources.GetObject("mButtonBrowseDir.ImageIndex")));
			this.mButtonBrowseDir.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonBrowseDir.ImeMode")));
			this.mButtonBrowseDir.Location = ((System.Drawing.Point)(resources.GetObject("mButtonBrowseDir.Location")));
			this.mButtonBrowseDir.Name = "mButtonBrowseDir";
			this.mButtonBrowseDir.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonBrowseDir.RightToLeft")));
			this.mButtonBrowseDir.Size = ((System.Drawing.Size)(resources.GetObject("mButtonBrowseDir.Size")));
			this.mButtonBrowseDir.TabIndex = ((int)(resources.GetObject("mButtonBrowseDir.TabIndex")));
			this.mButtonBrowseDir.Text = resources.GetString("mButtonBrowseDir.Text");
			this.mButtonBrowseDir.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonBrowseDir.TextAlign")));
			this.mButtonBrowseDir.Visible = ((bool)(resources.GetObject("mButtonBrowseDir.Visible")));
			this.mButtonBrowseDir.Click += new System.EventHandler(this.ButtonBrowseDir_Click);
			// 
			// GroupSelection
			// 
			this.GroupSelection.AccessibleDescription = ((string)(resources.GetObject("GroupSelection.AccessibleDescription")));
			this.GroupSelection.AccessibleName = ((string)(resources.GetObject("GroupSelection.AccessibleName")));
			this.GroupSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("GroupSelection.Anchor")));
			this.GroupSelection.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GroupSelection.BackgroundImage")));
			this.GroupSelection.Controls.AddRange(new System.Windows.Forms.Control[] {
																						 this.mEditDirPath,
																						 this.mButtonBrowseDir});
			this.GroupSelection.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("GroupSelection.Dock")));
			this.GroupSelection.Enabled = ((bool)(resources.GetObject("GroupSelection.Enabled")));
			this.GroupSelection.Font = ((System.Drawing.Font)(resources.GetObject("GroupSelection.Font")));
			this.GroupSelection.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("GroupSelection.ImeMode")));
			this.GroupSelection.Location = ((System.Drawing.Point)(resources.GetObject("GroupSelection.Location")));
			this.GroupSelection.Name = "GroupSelection";
			this.GroupSelection.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("GroupSelection.RightToLeft")));
			this.GroupSelection.Size = ((System.Drawing.Size)(resources.GetObject("GroupSelection.Size")));
			this.GroupSelection.TabIndex = ((int)(resources.GetObject("GroupSelection.TabIndex")));
			this.GroupSelection.TabStop = false;
			this.GroupSelection.Text = resources.GetString("GroupSelection.Text");
			this.GroupSelection.Visible = ((bool)(resources.GetObject("GroupSelection.Visible")));
			// 
			// GroupContentPreview
			// 
			this.GroupContentPreview.AccessibleDescription = ((string)(resources.GetObject("GroupContentPreview.AccessibleDescription")));
			this.GroupContentPreview.AccessibleName = ((string)(resources.GetObject("GroupContentPreview.AccessibleName")));
			this.GroupContentPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("GroupContentPreview.Anchor")));
			this.GroupContentPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GroupContentPreview.BackgroundImage")));
			this.GroupContentPreview.Controls.AddRange(new System.Windows.Forms.Control[] {
																							  this.mEditDirContent});
			this.GroupContentPreview.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("GroupContentPreview.Dock")));
			this.GroupContentPreview.Enabled = ((bool)(resources.GetObject("GroupContentPreview.Enabled")));
			this.GroupContentPreview.Font = ((System.Drawing.Font)(resources.GetObject("GroupContentPreview.Font")));
			this.GroupContentPreview.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("GroupContentPreview.ImeMode")));
			this.GroupContentPreview.Location = ((System.Drawing.Point)(resources.GetObject("GroupContentPreview.Location")));
			this.GroupContentPreview.Name = "GroupContentPreview";
			this.GroupContentPreview.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("GroupContentPreview.RightToLeft")));
			this.GroupContentPreview.Size = ((System.Drawing.Size)(resources.GetObject("GroupContentPreview.Size")));
			this.GroupContentPreview.TabIndex = ((int)(resources.GetObject("GroupContentPreview.TabIndex")));
			this.GroupContentPreview.TabStop = false;
			this.GroupContentPreview.Text = resources.GetString("GroupContentPreview.Text");
			this.GroupContentPreview.Visible = ((bool)(resources.GetObject("GroupContentPreview.Visible")));
			// 
			// mEditDirContent
			// 
			this.mEditDirContent.AccessibleDescription = ((string)(resources.GetObject("mEditDirContent.AccessibleDescription")));
			this.mEditDirContent.AccessibleName = ((string)(resources.GetObject("mEditDirContent.AccessibleName")));
			this.mEditDirContent.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mEditDirContent.Anchor")));
			this.mEditDirContent.AutoSize = ((bool)(resources.GetObject("mEditDirContent.AutoSize")));
			this.mEditDirContent.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mEditDirContent.BackgroundImage")));
			this.mEditDirContent.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mEditDirContent.Dock")));
			this.mEditDirContent.Enabled = ((bool)(resources.GetObject("mEditDirContent.Enabled")));
			this.mEditDirContent.Font = ((System.Drawing.Font)(resources.GetObject("mEditDirContent.Font")));
			this.mEditDirContent.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mEditDirContent.ImeMode")));
			this.mEditDirContent.Location = ((System.Drawing.Point)(resources.GetObject("mEditDirContent.Location")));
			this.mEditDirContent.MaxLength = ((int)(resources.GetObject("mEditDirContent.MaxLength")));
			this.mEditDirContent.Multiline = ((bool)(resources.GetObject("mEditDirContent.Multiline")));
			this.mEditDirContent.Name = "mEditDirContent";
			this.mEditDirContent.PasswordChar = ((char)(resources.GetObject("mEditDirContent.PasswordChar")));
			this.mEditDirContent.ReadOnly = true;
			this.mEditDirContent.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mEditDirContent.RightToLeft")));
			this.mEditDirContent.ScrollBars = ((System.Windows.Forms.ScrollBars)(resources.GetObject("mEditDirContent.ScrollBars")));
			this.mEditDirContent.Size = ((System.Drawing.Size)(resources.GetObject("mEditDirContent.Size")));
			this.mEditDirContent.TabIndex = ((int)(resources.GetObject("mEditDirContent.TabIndex")));
			this.mEditDirContent.Text = resources.GetString("mEditDirContent.Text");
			this.mEditDirContent.TextAlign = ((System.Windows.Forms.HorizontalAlignment)(resources.GetObject("mEditDirContent.TextAlign")));
			this.mEditDirContent.Visible = ((bool)(resources.GetObject("mEditDirContent.Visible")));
			this.mEditDirContent.WordWrap = ((bool)(resources.GetObject("mEditDirContent.WordWrap")));
			// 
			// GroupPrint
			// 
			this.GroupPrint.AccessibleDescription = ((string)(resources.GetObject("GroupPrint.AccessibleDescription")));
			this.GroupPrint.AccessibleName = ((string)(resources.GetObject("GroupPrint.AccessibleName")));
			this.GroupPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("GroupPrint.Anchor")));
			this.GroupPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("GroupPrint.BackgroundImage")));
			this.GroupPrint.Controls.AddRange(new System.Windows.Forms.Control[] {
																					 this.mButtonPrint,
																					 this.mButtonPrintSetup,
																					 this.mButtonPrintPreview});
			this.GroupPrint.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("GroupPrint.Dock")));
			this.GroupPrint.Enabled = ((bool)(resources.GetObject("GroupPrint.Enabled")));
			this.GroupPrint.Font = ((System.Drawing.Font)(resources.GetObject("GroupPrint.Font")));
			this.GroupPrint.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("GroupPrint.ImeMode")));
			this.GroupPrint.Location = ((System.Drawing.Point)(resources.GetObject("GroupPrint.Location")));
			this.GroupPrint.Name = "GroupPrint";
			this.GroupPrint.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("GroupPrint.RightToLeft")));
			this.GroupPrint.Size = ((System.Drawing.Size)(resources.GetObject("GroupPrint.Size")));
			this.GroupPrint.TabIndex = ((int)(resources.GetObject("GroupPrint.TabIndex")));
			this.GroupPrint.TabStop = false;
			this.GroupPrint.Text = resources.GetString("GroupPrint.Text");
			this.GroupPrint.Visible = ((bool)(resources.GetObject("GroupPrint.Visible")));
			// 
			// mButtonPrint
			// 
			this.mButtonPrint.AccessibleDescription = ((string)(resources.GetObject("mButtonPrint.AccessibleDescription")));
			this.mButtonPrint.AccessibleName = ((string)(resources.GetObject("mButtonPrint.AccessibleName")));
			this.mButtonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonPrint.Anchor")));
			this.mButtonPrint.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonPrint.BackgroundImage")));
			this.mButtonPrint.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonPrint.Dock")));
			this.mButtonPrint.Enabled = ((bool)(resources.GetObject("mButtonPrint.Enabled")));
			this.mButtonPrint.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonPrint.FlatStyle")));
			this.mButtonPrint.Font = ((System.Drawing.Font)(resources.GetObject("mButtonPrint.Font")));
			this.mButtonPrint.Image = ((System.Drawing.Image)(resources.GetObject("mButtonPrint.Image")));
			this.mButtonPrint.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonPrint.ImageAlign")));
			this.mButtonPrint.ImageIndex = ((int)(resources.GetObject("mButtonPrint.ImageIndex")));
			this.mButtonPrint.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonPrint.ImeMode")));
			this.mButtonPrint.Location = ((System.Drawing.Point)(resources.GetObject("mButtonPrint.Location")));
			this.mButtonPrint.Name = "mButtonPrint";
			this.mButtonPrint.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonPrint.RightToLeft")));
			this.mButtonPrint.Size = ((System.Drawing.Size)(resources.GetObject("mButtonPrint.Size")));
			this.mButtonPrint.TabIndex = ((int)(resources.GetObject("mButtonPrint.TabIndex")));
			this.mButtonPrint.Text = resources.GetString("mButtonPrint.Text");
			this.mButtonPrint.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonPrint.TextAlign")));
			this.mButtonPrint.Visible = ((bool)(resources.GetObject("mButtonPrint.Visible")));
			this.mButtonPrint.Click += new System.EventHandler(this.mButtonPrint_Click);
			// 
			// mButtonPrintSetup
			// 
			this.mButtonPrintSetup.AccessibleDescription = ((string)(resources.GetObject("mButtonPrintSetup.AccessibleDescription")));
			this.mButtonPrintSetup.AccessibleName = ((string)(resources.GetObject("mButtonPrintSetup.AccessibleName")));
			this.mButtonPrintSetup.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonPrintSetup.Anchor")));
			this.mButtonPrintSetup.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonPrintSetup.BackgroundImage")));
			this.mButtonPrintSetup.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonPrintSetup.Dock")));
			this.mButtonPrintSetup.Enabled = ((bool)(resources.GetObject("mButtonPrintSetup.Enabled")));
			this.mButtonPrintSetup.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonPrintSetup.FlatStyle")));
			this.mButtonPrintSetup.Font = ((System.Drawing.Font)(resources.GetObject("mButtonPrintSetup.Font")));
			this.mButtonPrintSetup.Image = ((System.Drawing.Image)(resources.GetObject("mButtonPrintSetup.Image")));
			this.mButtonPrintSetup.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonPrintSetup.ImageAlign")));
			this.mButtonPrintSetup.ImageIndex = ((int)(resources.GetObject("mButtonPrintSetup.ImageIndex")));
			this.mButtonPrintSetup.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonPrintSetup.ImeMode")));
			this.mButtonPrintSetup.Location = ((System.Drawing.Point)(resources.GetObject("mButtonPrintSetup.Location")));
			this.mButtonPrintSetup.Name = "mButtonPrintSetup";
			this.mButtonPrintSetup.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonPrintSetup.RightToLeft")));
			this.mButtonPrintSetup.Size = ((System.Drawing.Size)(resources.GetObject("mButtonPrintSetup.Size")));
			this.mButtonPrintSetup.TabIndex = ((int)(resources.GetObject("mButtonPrintSetup.TabIndex")));
			this.mButtonPrintSetup.Text = resources.GetString("mButtonPrintSetup.Text");
			this.mButtonPrintSetup.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonPrintSetup.TextAlign")));
			this.mButtonPrintSetup.Visible = ((bool)(resources.GetObject("mButtonPrintSetup.Visible")));
			this.mButtonPrintSetup.Click += new System.EventHandler(this.ButtonPrintSetup_Click);
			// 
			// mButtonPrintPreview
			// 
			this.mButtonPrintPreview.AccessibleDescription = ((string)(resources.GetObject("mButtonPrintPreview.AccessibleDescription")));
			this.mButtonPrintPreview.AccessibleName = ((string)(resources.GetObject("mButtonPrintPreview.AccessibleName")));
			this.mButtonPrintPreview.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonPrintPreview.Anchor")));
			this.mButtonPrintPreview.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonPrintPreview.BackgroundImage")));
			this.mButtonPrintPreview.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonPrintPreview.Dock")));
			this.mButtonPrintPreview.Enabled = ((bool)(resources.GetObject("mButtonPrintPreview.Enabled")));
			this.mButtonPrintPreview.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonPrintPreview.FlatStyle")));
			this.mButtonPrintPreview.Font = ((System.Drawing.Font)(resources.GetObject("mButtonPrintPreview.Font")));
			this.mButtonPrintPreview.Image = ((System.Drawing.Image)(resources.GetObject("mButtonPrintPreview.Image")));
			this.mButtonPrintPreview.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonPrintPreview.ImageAlign")));
			this.mButtonPrintPreview.ImageIndex = ((int)(resources.GetObject("mButtonPrintPreview.ImageIndex")));
			this.mButtonPrintPreview.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonPrintPreview.ImeMode")));
			this.mButtonPrintPreview.Location = ((System.Drawing.Point)(resources.GetObject("mButtonPrintPreview.Location")));
			this.mButtonPrintPreview.Name = "mButtonPrintPreview";
			this.mButtonPrintPreview.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonPrintPreview.RightToLeft")));
			this.mButtonPrintPreview.Size = ((System.Drawing.Size)(resources.GetObject("mButtonPrintPreview.Size")));
			this.mButtonPrintPreview.TabIndex = ((int)(resources.GetObject("mButtonPrintPreview.TabIndex")));
			this.mButtonPrintPreview.Text = resources.GetString("mButtonPrintPreview.Text");
			this.mButtonPrintPreview.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonPrintPreview.TextAlign")));
			this.mButtonPrintPreview.Visible = ((bool)(resources.GetObject("mButtonPrintPreview.Visible")));
			this.mButtonPrintPreview.Click += new System.EventHandler(this.mButtonPrintPreview_Click);
			// 
			// mButtonQuit
			// 
			this.mButtonQuit.AccessibleDescription = ((string)(resources.GetObject("mButtonQuit.AccessibleDescription")));
			this.mButtonQuit.AccessibleName = ((string)(resources.GetObject("mButtonQuit.AccessibleName")));
			this.mButtonQuit.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mButtonQuit.Anchor")));
			this.mButtonQuit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mButtonQuit.BackgroundImage")));
			this.mButtonQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.mButtonQuit.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mButtonQuit.Dock")));
			this.mButtonQuit.Enabled = ((bool)(resources.GetObject("mButtonQuit.Enabled")));
			this.mButtonQuit.FlatStyle = ((System.Windows.Forms.FlatStyle)(resources.GetObject("mButtonQuit.FlatStyle")));
			this.mButtonQuit.Font = ((System.Drawing.Font)(resources.GetObject("mButtonQuit.Font")));
			this.mButtonQuit.Image = ((System.Drawing.Image)(resources.GetObject("mButtonQuit.Image")));
			this.mButtonQuit.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonQuit.ImageAlign")));
			this.mButtonQuit.ImageIndex = ((int)(resources.GetObject("mButtonQuit.ImageIndex")));
			this.mButtonQuit.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mButtonQuit.ImeMode")));
			this.mButtonQuit.Location = ((System.Drawing.Point)(resources.GetObject("mButtonQuit.Location")));
			this.mButtonQuit.Name = "mButtonQuit";
			this.mButtonQuit.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mButtonQuit.RightToLeft")));
			this.mButtonQuit.Size = ((System.Drawing.Size)(resources.GetObject("mButtonQuit.Size")));
			this.mButtonQuit.TabIndex = ((int)(resources.GetObject("mButtonQuit.TabIndex")));
			this.mButtonQuit.Text = resources.GetString("mButtonQuit.Text");
			this.mButtonQuit.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("mButtonQuit.TextAlign")));
			this.mButtonQuit.Visible = ((bool)(resources.GetObject("mButtonQuit.Visible")));
			this.mButtonQuit.Click += new System.EventHandler(this.ButtonQuit_Click);
			// 
			// label1
			// 
			this.label1.AccessibleDescription = ((string)(resources.GetObject("label1.AccessibleDescription")));
			this.label1.AccessibleName = ((string)(resources.GetObject("label1.AccessibleName")));
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("label1.Anchor")));
			this.label1.AutoSize = ((bool)(resources.GetObject("label1.AutoSize")));
			this.label1.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("label1.Dock")));
			this.label1.Enabled = ((bool)(resources.GetObject("label1.Enabled")));
			this.label1.Font = ((System.Drawing.Font)(resources.GetObject("label1.Font")));
			this.label1.Image = ((System.Drawing.Image)(resources.GetObject("label1.Image")));
			this.label1.ImageAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.ImageAlign")));
			this.label1.ImageIndex = ((int)(resources.GetObject("label1.ImageIndex")));
			this.label1.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("label1.ImeMode")));
			this.label1.Location = ((System.Drawing.Point)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("label1.RightToLeft")));
			this.label1.Size = ((System.Drawing.Size)(resources.GetObject("label1.Size")));
			this.label1.TabIndex = ((int)(resources.GetObject("label1.TabIndex")));
			this.label1.Text = resources.GetString("label1.Text");
			this.label1.TextAlign = ((System.Drawing.ContentAlignment)(resources.GetObject("label1.TextAlign")));
			this.label1.Visible = ((bool)(resources.GetObject("label1.Visible")));
			// 
			// mPrintPreviewDialog
			// 
			this.mPrintPreviewDialog.AccessibleDescription = ((string)(resources.GetObject("mPrintPreviewDialog.AccessibleDescription")));
			this.mPrintPreviewDialog.AccessibleName = ((string)(resources.GetObject("mPrintPreviewDialog.AccessibleName")));
			this.mPrintPreviewDialog.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("mPrintPreviewDialog.Anchor")));
			this.mPrintPreviewDialog.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("mPrintPreviewDialog.AutoScaleBaseSize")));
			this.mPrintPreviewDialog.AutoScroll = ((bool)(resources.GetObject("mPrintPreviewDialog.AutoScroll")));
			this.mPrintPreviewDialog.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("mPrintPreviewDialog.AutoScrollMargin")));
			this.mPrintPreviewDialog.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("mPrintPreviewDialog.AutoScrollMinSize")));
			this.mPrintPreviewDialog.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("mPrintPreviewDialog.BackgroundImage")));
			this.mPrintPreviewDialog.ClientSize = ((System.Drawing.Size)(resources.GetObject("mPrintPreviewDialog.ClientSize")));
			this.mPrintPreviewDialog.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("mPrintPreviewDialog.Dock")));
			this.mPrintPreviewDialog.Enabled = ((bool)(resources.GetObject("mPrintPreviewDialog.Enabled")));
			this.mPrintPreviewDialog.Font = ((System.Drawing.Font)(resources.GetObject("mPrintPreviewDialog.Font")));
			this.mPrintPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("mPrintPreviewDialog.Icon")));
			this.mPrintPreviewDialog.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("mPrintPreviewDialog.ImeMode")));
			this.mPrintPreviewDialog.Location = ((System.Drawing.Point)(resources.GetObject("mPrintPreviewDialog.Location1")));
			this.mPrintPreviewDialog.MaximumSize = ((System.Drawing.Size)(resources.GetObject("mPrintPreviewDialog.MaximumSize")));
			this.mPrintPreviewDialog.MinimumSize = ((System.Drawing.Size)(resources.GetObject("mPrintPreviewDialog.MinimumSize")));
			this.mPrintPreviewDialog.Name = "mPrintPreviewDialog";
			this.mPrintPreviewDialog.Opacity = 1;
			this.mPrintPreviewDialog.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("mPrintPreviewDialog.RightToLeft")));
			this.mPrintPreviewDialog.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("mPrintPreviewDialog.StartPosition")));
			this.mPrintPreviewDialog.Text = resources.GetString("mPrintPreviewDialog.Text");
			this.mPrintPreviewDialog.TransparencyKey = System.Drawing.Color.Empty;
			this.mPrintPreviewDialog.Visible = ((bool)(resources.GetObject("mPrintPreviewDialog.Visible")));
			// 
			// mPrintFontDialog
			// 
			this.mPrintFontDialog.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			// 
			// PrintDirForm
			// 
			this.AccessibleDescription = ((string)(resources.GetObject("$this.AccessibleDescription")));
			this.AccessibleName = ((string)(resources.GetObject("$this.AccessibleName")));
			this.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("$this.Anchor")));
			this.AutoScaleBaseSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScaleBaseSize")));
			this.AutoScroll = ((bool)(resources.GetObject("$this.AutoScroll")));
			this.AutoScrollMargin = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMargin")));
			this.AutoScrollMinSize = ((System.Drawing.Size)(resources.GetObject("$this.AutoScrollMinSize")));
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.mButtonQuit;
			this.ClientSize = ((System.Drawing.Size)(resources.GetObject("$this.ClientSize")));
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1,
																		  this.mButtonQuit,
																		  this.GroupPrint,
																		  this.GroupContentPreview,
																		  this.GroupSelection});
			this.Dock = ((System.Windows.Forms.DockStyle)(resources.GetObject("$this.Dock")));
			this.Enabled = ((bool)(resources.GetObject("$this.Enabled")));
			this.Font = ((System.Drawing.Font)(resources.GetObject("$this.Font")));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("$this.ImeMode")));
			this.Location = ((System.Drawing.Point)(resources.GetObject("$this.Location")));
			this.MaximumSize = ((System.Drawing.Size)(resources.GetObject("$this.MaximumSize")));
			this.MinimumSize = ((System.Drawing.Size)(resources.GetObject("$this.MinimumSize")));
			this.Name = "PrintDirForm";
			this.RightToLeft = ((System.Windows.Forms.RightToLeft)(resources.GetObject("$this.RightToLeft")));
			this.StartPosition = ((System.Windows.Forms.FormStartPosition)(resources.GetObject("$this.StartPosition")));
			this.Text = resources.GetString("$this.Text");
			this.Visible = ((bool)(resources.GetObject("$this.Visible")));
			this.GroupSelection.ResumeLayout(false);
			this.GroupContentPreview.ResumeLayout(false);
			this.GroupPrint.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new PrintDirForm());
		}


		// -------------------------------------------------------------------


		/// <summary>
		/// Parses directory and prepare text output
		/// </summary>
		private void ParseSelectedDirectory()
		{
			string dir_path = mEditDirPath.Text;
			string result = "";
			bool can_print = false;

			if (mDirectoryText == null)
				mDirectoryText = new ArrayList();
			else
				mDirectoryText.Clear();

			const int max_name_length = 42;
			string format1 = "{0,-45} {1,9}    {2,-10} {3,-8}\r\n";
			string format2 = "{0,-45} {1,9}    {2,-10} {3,8}\r\n";
			mHeaderDirText = String.Format(format1, "Nom", "Taille", "Date", "");
			result = mHeaderDirText;

			if (dir_path != "")
			{
				try
				{
					DirectoryInfo info = new DirectoryInfo(dir_path);

					DirectoryInfo[] dir_list = info.GetDirectories();
					foreach(DirectoryInfo dir in dir_list)
					{
						string line;

						string name = dir.Name;
						name = name.Substring(0, Math.Min(name.Length, max_name_length));


						line = String.Format(format2,
							name,
							"[Rép]",
							dir.LastWriteTime.ToShortDateString(),
							dir.LastWriteTime.ToShortTimeString());

						mDirectoryText.Add(line);
						result += line;
						can_print = true;
					}

					FileInfo[] file_list = info.GetFiles();
					foreach(FileInfo file in file_list)
					{
						string line;
						string kb;
						
						if (file.Length < 1024)
							kb = String.Format("{0}", file.Length);
						else
							kb = String.Format("{0} ko", file.Length / 1024);

						string name = file.Name;
						name = name.Substring(0, Math.Min(name.Length, max_name_length));

						line = String.Format(format2,
							name,
							kb,
							file.LastWriteTime.ToShortDateString(),
							file.LastWriteTime.ToShortTimeString());

						mDirectoryText.Add(line);
						result += line;
						can_print = true;
					}

				}
				catch(DirectoryNotFoundException /* e */)
				{
					result = "(Sélectionner un dossier d'abord)";
				}
				catch(Exception e)
				{
					result = "(Errur: Impossible d'accéder à ce dossier. Sélectionnez en un autre.)";
					MessageBox.Show(result,
									"Erreur acces disque: " + e.Message,
									MessageBoxButtons.OK,
									MessageBoxIcon.Exclamation);
				}
			}

			// affect new result to the content edit box
			mEditDirContent.Text = result;

			// validate the previe and print button if there is something to print
			mButtonPrintPreview.Enabled = can_print;
			mButtonPrint.Enabled = can_print;
		}

		// -------------------------------------------------------------------

		/// <summary>
		/// Let the user select the directory to be printed
		/// </summary>
		private void ButtonBrowseDir_Click(object sender, System.EventArgs e)
		{
			Microbion.SHBrowseFolders.SHBrowser browser = new Microbion.SHBrowseFolders.SHBrowser();
			string result = browser.DoBrowse("Sélection du dossier à imprimer:");
			if (result != "")
			{
				this.mEditDirPath.Text = result;
				// ParseSelectedDirectory();
			}
		}

		/// <summary>
		/// Closes the application
		/// </summary>
		private void ButtonQuit_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void ButtonPrintSetup_Click(object sender, System.EventArgs e)
		{
			if (mPageSetupDialog.ShowDialog() == DialogResult.OK)
			{
				if (!Object.ReferenceEquals(mPageSetupDialog.PageSettings, mPageSettings))
					mPageSettings = mPageSetupDialog.PageSettings;
				mPrintDoc.DefaultPageSettings = mPageSettings;
			}
		}

		private void mButtonPrintPreview_Click(object sender, System.EventArgs e)
		{
			mPrintCurrentCount = 0;
			mPrintPreviewDialog.ShowDialog();
		}

		private void mButtonPrint_Click(object sender, System.EventArgs e)
		{
			try 
			{
				mPrintCurrentCount = 0;
				mPrintDoc.Print();
			}  
			catch(Exception ex) 
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void printDocPage(object sender, PrintPageEventArgs e)
		{
			string line;
			int count = 0;
			float y_pos = 0;
			float left_margin   = e.MarginBounds.Left;
			float right_margin  = e.MarginBounds.Right;
			float top_margin    = e.MarginBounds.Top;
			float bottom_margin = e.MarginBounds.Bottom;


			// the list of lines might be empty, but it must be allocated
			Debug.Assert(mDirectoryText != null);

			// nb lines
			int nb_lines = mDirectoryText.Count;

			// the font used to print, the height of the font
			Font print_font = mPrintFontDialog.Font;
			float font_height = print_font.GetHeight(e.Graphics);

			// calculate the number of lines per page.
			int lines_per_page = (int)(e.MarginBounds.Height / print_font.GetHeight(e.Graphics));
			// the first line is always the table header
			lines_per_page -= 1;

			// continue printing where left before or start again at begining of document
			if (mPrintCurrentCount < nb_lines)
				count = mPrintCurrentCount;
			else
				count = 0;

			// --- print the header ---

			Font header_font = new Font("Arial", 10);
			y_pos = top_margin - 0.10f * font_height;

			e.Graphics.DrawLine(Pens.Black, left_margin, y_pos, right_margin, y_pos);
			y_pos -= 1.5f * font_height;

			line = mEditDirPath.Text;
			e.Graphics.DrawString(line, header_font, Brushes.Black, left_margin, y_pos, new StringFormat());
			
			// --- print the footer ---

			y_pos = bottom_margin + 0.10f * font_height;

			e.Graphics.DrawLine(Pens.Black, left_margin, y_pos, right_margin, y_pos);
			y_pos += 0.5f * font_height;

			line = String.Format("Imprimé le {0}, {1}",
								 DateTime.Now.ToShortDateString(),
								 DateTime.Now.ToShortTimeString());
			e.Graphics.DrawString(line, header_font, Brushes.Black, left_margin, y_pos, new StringFormat());

			int nb_pages  = (int) Math.Ceiling((nb_lines / lines_per_page) + 0.5);
			int curr_page = (int) Math.Ceiling((count / lines_per_page) + 0.5);

			line = String.Format("Page {0}/{1}", curr_page, nb_pages);
			RectangleF rectangle = new RectangleF(left_margin, y_pos, right_margin, y_pos + font_height);
			StringFormat str_format = new StringFormat();
			str_format.Alignment = StringAlignment.Center;
			e.Graphics.DrawString(line, header_font, Brushes.Black, rectangle, str_format);


			// --- print the actual content ---
			
			// the table starts at the top margin
			y_pos = top_margin;

			// the first line is always the table header
			line = mHeaderDirText;
			e.Graphics.DrawString(line, print_font, Brushes.Black, left_margin, y_pos, new StringFormat());
			y_pos += font_height;

			e.Graphics.DrawLine(Pens.Black, left_margin, y_pos, right_margin, y_pos);


			// print each line for this page
			for(lines_per_page += count; count < lines_per_page && count < nb_lines; count++)
			{
				line = mDirectoryText[count].ToString();
				e.Graphics.DrawString(line, print_font, Brushes.Black, left_margin, y_pos, new StringFormat());
				// positiony for next line
				y_pos += font_height;
			}

			// if more lines exist, print another page.
			mPrintCurrentCount = count;
			e.HasMorePages = (count < nb_lines);
		}

		private void mEditDirPath_TextChanged(object sender, System.EventArgs e)
		{
			ParseSelectedDirectory();
		}


		private void GetLastUsedPath()
		{
		}


		private void SetLastUsedPath()
		{
		}
	
	
	}
}
