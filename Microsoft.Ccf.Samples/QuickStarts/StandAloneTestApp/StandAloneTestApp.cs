//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
// StandAloneTestApp.cs
//
// This is a test application for CCF working with Win32 external applications.
//
//===============================================================================

using System;
using System.Windows.Forms;

namespace Microsoft.Ccf.Samples.StandAloneTestApp
{
	public class TestApp : Form
	{
		private Label label1;
		private TextBox testTextBox;
		private Button button1;
		private Label labelResults;
		private CheckBox checkBox1;
		private RadioButton radioButton1;
		private RadioButton radioButton2;
		private RadioButton radioButton3;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private TabPage tabPage2;
		private TextBox textBoxTabPage1;
		private Label label4;
		private TextBox textBoxTabPage2;
		private Label label5;
		private Label labelTabPageText;
		private Label label3;
		private Button buttonTabPage2;
		private Button buttonTabPage1;

		private System.ComponentModel.Container components = null;

		public TestApp()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestApp));
			this.label1 = new System.Windows.Forms.Label();
			this.testTextBox = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.labelResults = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.textBoxTabPage1 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.textBoxTabPage2 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.labelTabPageText = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonTabPage2 = new System.Windows.Forms.Button();
			this.buttonTabPage1 = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(32, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Text Input";
			// 
			// testTextBox
			// 
			this.testTextBox.Location = new System.Drawing.Point(88, 25);
			this.testTextBox.Name = "testTextBox";
			this.testTextBox.Size = new System.Drawing.Size(313, 20);
			this.testTextBox.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.button1.Location = new System.Drawing.Point(416, 25);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(86, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "&Copy Text";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// labelResults
			// 
			this.labelResults.AutoEllipsis = true;
			this.labelResults.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelResults.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelResults.Location = new System.Drawing.Point(32, 61);
			this.labelResults.Name = "labelResults";
			this.labelResults.Size = new System.Drawing.Size(369, 83);
			this.labelResults.TabIndex = 3;
			// 
			// checkBox1
			// 
			this.checkBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.checkBox1.Location = new System.Drawing.Point(347, 181);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(126, 16);
			this.checkBox1.TabIndex = 4;
			this.checkBox1.Text = "Checkbox";
			// 
			// radioButton1
			// 
			this.radioButton1.Location = new System.Drawing.Point(347, 209);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(96, 16);
			this.radioButton1.TabIndex = 5;
			this.radioButton1.Text = "Radio1";
			// 
			// radioButton2
			// 
			this.radioButton2.Location = new System.Drawing.Point(347, 233);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(96, 16);
			this.radioButton2.TabIndex = 6;
			this.radioButton2.Text = "Radio2";
			// 
			// radioButton3
			// 
			this.radioButton3.Location = new System.Drawing.Point(347, 257);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(96, 16);
			this.radioButton3.TabIndex = 7;
			this.radioButton3.Text = "Radio3";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(32, 159);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(264, 114);
			this.tabControl1.TabIndex = 8;
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tabPage1.Controls.Add(this.buttonTabPage1);
			this.tabPage1.Controls.Add(this.textBoxTabPage1);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(256, 88);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Tab Page 1";
			// 
			// textBoxTabPage1
			// 
			this.textBoxTabPage1.Location = new System.Drawing.Point(81, 13);
			this.textBoxTabPage1.Name = "textBoxTabPage1";
			this.textBoxTabPage1.Size = new System.Drawing.Size(154, 20);
			this.textBoxTabPage1.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Location = new System.Drawing.Point(3, 13);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 20);
			this.label4.TabIndex = 0;
			this.label4.Text = "Text Input";
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tabPage2.Controls.Add(this.textBoxTabPage2);
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.buttonTabPage2);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(256, 88);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Tab Page 2";
			// 
			// textBoxTabPage2
			// 
			this.textBoxTabPage2.Location = new System.Drawing.Point(81, 13);
			this.textBoxTabPage2.Name = "textBoxTabPage2";
			this.textBoxTabPage2.Size = new System.Drawing.Size(154, 20);
			this.textBoxTabPage2.TabIndex = 4;
			// 
			// label5
			// 
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Location = new System.Drawing.Point(3, 13);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(56, 20);
			this.label5.TabIndex = 3;
			this.label5.Text = "Text Input";
			// 
			// labelTabPageText
			// 
			this.labelTabPageText.AutoEllipsis = true;
			this.labelTabPageText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelTabPageText.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.labelTabPageText.Location = new System.Drawing.Point(117, 293);
			this.labelTabPageText.Name = "labelTabPageText";
			this.labelTabPageText.Size = new System.Drawing.Size(284, 21);
			this.labelTabPageText.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(33, 301);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(78, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Tab Page Text";
			// 
			// buttonTabPage2
			// 
			this.buttonTabPage2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonTabPage2.Location = new System.Drawing.Point(149, 48);
			this.buttonTabPage2.Name = "buttonTabPage2";
			this.buttonTabPage2.Size = new System.Drawing.Size(86, 24);
			this.buttonTabPage2.TabIndex = 5;
			this.buttonTabPage2.Text = "&Copy Text";
			this.buttonTabPage2.Click += new System.EventHandler(this.buttonTabPage2_Click);
			// 
			// buttonTabPage1
			// 
			this.buttonTabPage1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonTabPage1.Location = new System.Drawing.Point(149, 48);
			this.buttonTabPage1.Name = "buttonTabPage1";
			this.buttonTabPage1.Size = new System.Drawing.Size(86, 24);
			this.buttonTabPage1.TabIndex = 6;
			this.buttonTabPage1.Text = "&Copy Text";
			this.buttonTabPage1.Click += new System.EventHandler(this.buttonTabPage1_Click);
			// 
			// TestApp
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(524, 334);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.radioButton3);
			this.Controls.Add(this.radioButton2);
			this.Controls.Add(this.radioButton1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.labelTabPageText);
			this.Controls.Add(this.labelResults);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.testTextBox);
			this.Controls.Add(this.label1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "TestApp";
			this.Text = "Win32 Stand Alone Test Application";
			this.Load += new System.EventHandler(this.TestApp_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private static string arguments;

		private void button1_Click(object sender, EventArgs e)
		{
			this.labelResults.Text = "Text box contents: " + this.testTextBox.Text;
		}

		private void TestApp_Load(object sender, EventArgs e)
		{
			labelResults.Text = "Arguments are: " + arguments +
				"\r\nWorking Directory: " + System.Diagnostics.Process.GetCurrentProcess().StartInfo.WorkingDirectory +
				"\r\nCurrent Dir: " + System.IO.Directory.GetCurrentDirectory();
		}

		private void buttonTabPage1_Click(object sender, EventArgs e)
		{
			this.labelTabPageText.Text = "Text in TabPage 1 is: " + this.textBoxTabPage1.Text;
		}

		private void buttonTabPage2_Click(object sender, EventArgs e)
		{
			this.labelTabPageText.Text = "Text in TabPage 2 is: " + this.textBoxTabPage2.Text;
		}

		[STAThread]
		static void Main(string[] args)
		{
			// rebuild the argument string
			foreach (string arg in args)
				arguments = arguments + " " + arg;

			Application.EnableVisualStyles();
			Application.Run(new TestApp());
		}
	}
}
