using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Nihongo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox mGroupSearch;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader mDefColumnIndex;
		private System.Windows.Forms.ColumnHeader mDefColumnDef;
		private System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.ColumnHeader mResultColumnNihongo;
		private System.Windows.Forms.ColumnHeader mResultColumnKana;
		private System.Windows.Forms.ColumnHeader mResultColumnEnglish;
		private System.Windows.Forms.ColumnHeader mResultColumnFrench;
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
			this.mGroupSearch = new System.Windows.Forms.GroupBox();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.button1 = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.listView1 = new System.Windows.Forms.ListView();
			this.mDefColumnIndex = new System.Windows.Forms.ColumnHeader();
			this.mDefColumnDef = new System.Windows.Forms.ColumnHeader();
			this.listView2 = new System.Windows.Forms.ListView();
			this.mResultColumnNihongo = new System.Windows.Forms.ColumnHeader();
			this.mResultColumnKana = new System.Windows.Forms.ColumnHeader();
			this.mResultColumnEnglish = new System.Windows.Forms.ColumnHeader();
			this.mResultColumnFrench = new System.Windows.Forms.ColumnHeader();
			this.mGroupSearch.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// mGroupSearch
			// 
			this.mGroupSearch.Controls.Add(this.button1);
			this.mGroupSearch.Controls.Add(this.radioButton3);
			this.mGroupSearch.Controls.Add(this.radioButton2);
			this.mGroupSearch.Controls.Add(this.radioButton1);
			this.mGroupSearch.Controls.Add(this.checkBox2);
			this.mGroupSearch.Controls.Add(this.checkBox1);
			this.mGroupSearch.Controls.Add(this.comboBox1);
			this.mGroupSearch.Location = new System.Drawing.Point(8, 8);
			this.mGroupSearch.Name = "mGroupSearch";
			this.mGroupSearch.Size = new System.Drawing.Size(200, 208);
			this.mGroupSearch.TabIndex = 0;
			this.mGroupSearch.TabStop = false;
			this.mGroupSearch.Text = "Search";
			// 
			// comboBox1
			// 
			this.comboBox1.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
			this.comboBox1.Location = new System.Drawing.Point(8, 24);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(184, 21);
			this.comboBox1.TabIndex = 0;
			this.comboBox1.Text = "comboBox1";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(8, 56);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(184, 24);
			this.checkBox1.TabIndex = 1;
			this.checkBox1.Text = "Match case";
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(8, 80);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(184, 24);
			this.checkBox2.TabIndex = 2;
			this.checkBox2.Text = "Match whole word";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(8, 104);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(184, 24);
			this.radioButton1.TabIndex = 3;
			this.radioButton1.Text = "Romaji search";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(8, 128);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(184, 24);
			this.radioButton2.TabIndex = 4;
			this.radioButton2.Text = "English search";
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(8, 152);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(184, 24);
			this.radioButton3.TabIndex = 5;
			this.radioButton3.Text = "French search";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(112, 176);
			this.button1.Name = "button1";
			this.button1.TabIndex = 6;
			this.button1.Text = "Search";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.listView1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(216, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(392, 208);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Definition";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.listView2);
			this.groupBox2.Location = new System.Drawing.Point(8, 224);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(600, 240);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Results";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Word:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label2.Location = new System.Drawing.Point(48, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(112, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "<kanji>";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label3.Location = new System.Drawing.Point(168, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "<hiragana>";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label4
			// 
			this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label4.Location = new System.Drawing.Point(280, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(104, 23);
			this.label4.TabIndex = 3;
			this.label4.Text = "<romaji>";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// listView1
			// 
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.mDefColumnIndex,
																						this.mDefColumnDef});
			this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(8, 48);
			this.listView1.MultiSelect = false;
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(376, 152);
			this.listView1.TabIndex = 4;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// mDefColumnIndex
			// 
			this.mDefColumnIndex.Text = "#";
			this.mDefColumnIndex.Width = 20;
			// 
			// mDefColumnDef
			// 
			this.mDefColumnDef.Text = "Definition";
			this.mDefColumnDef.Width = 338;
			// 
			// listView2
			// 
			this.listView2.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.mResultColumnNihongo,
																						this.mResultColumnKana,
																						this.mResultColumnEnglish,
																						this.mResultColumnFrench});
			this.listView2.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.listView2.HideSelection = false;
			this.listView2.HoverSelection = true;
			this.listView2.Location = new System.Drawing.Point(8, 16);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(584, 216);
			this.listView2.TabIndex = 0;
			this.listView2.View = System.Windows.Forms.View.Details;
			// 
			// mResultColumnNihongo
			// 
			this.mResultColumnNihongo.Text = "???";
			this.mResultColumnNihongo.Width = 100;
			// 
			// mResultColumnKana
			// 
			this.mResultColumnKana.Text = "Kana";
			this.mResultColumnKana.Width = 95;
			// 
			// mResultColumnEnglish
			// 
			this.mResultColumnEnglish.Text = "English";
			this.mResultColumnEnglish.Width = 178;
			// 
			// mResultColumnFrench
			// 
			this.mResultColumnFrench.Text = "Français";
			this.mResultColumnFrench.Width = 192;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 470);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.mGroupSearch);
			this.Name = "Form1";
			this.Text = "??? Dictionnary";
			this.mGroupSearch.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
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
	}
}
