//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2006 Microsoft Corp.
//
//===================================================================================

#region Usings

using System;
using System.Windows.Forms;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.CitrixIntegration;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class WinFormHelloWorld : CitrixHostedControlStub
	{
		private Label label1;
		private Label label2;
		private GroupBox groupBox1;
		private GroupBox groupBox2;
		private TextBox contextDisplay;
		private Button ClearContext;
		private Button testActionButton;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		public WinFormHelloWorld(int appID, string appName, string initString) : base(appID, appName, initString)
		{
			// Required by the Windows Forms designer
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose managed components here
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.contextDisplay = new System.Windows.Forms.TextBox();
			this.ClearContext = new System.Windows.Forms.Button();
			this.testActionButton = new System.Windows.Forms.Button();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(432, 48);
			this.label1.TabIndex = 0;
			this.label1.Text = "Hello World!";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Location = new System.Drawing.Point(8, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(416, 48);
			this.label2.TabIndex = 1;
			this.label2.Text = "Actions:";
			// 
			// groupBox1
			// 
			this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox1.Location = new System.Drawing.Point(0, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(432, 72);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.contextDisplay);
			this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox2.Location = new System.Drawing.Point(0, 136);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(432, 184);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			// 
			// contextDisplay
			// 
			this.contextDisplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.contextDisplay.Location = new System.Drawing.Point(8, 16);
			this.contextDisplay.Multiline = true;
			this.contextDisplay.Name = "contextDisplay";
			this.contextDisplay.Size = new System.Drawing.Size(416, 160);
			this.contextDisplay.TabIndex = 0;
			this.contextDisplay.Text = "textBox1";
			// 
			// ClearContext
			// 
			this.ClearContext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ClearContext.Location = new System.Drawing.Point(336, 328);
			this.ClearContext.Name = "ClearContext";
			this.ClearContext.Size = new System.Drawing.Size(96, 24);
			this.ClearContext.TabIndex = 5;
			this.ClearContext.Text = "&Clear Context";
			this.ClearContext.Click += new System.EventHandler(this.ClearContext_Click);
			// 
			// testActionButton
			// 
			this.testActionButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.testActionButton.Location = new System.Drawing.Point(0, 328);
			this.testActionButton.Name = "testActionButton";
			this.testActionButton.Size = new System.Drawing.Size(96, 24);
			this.testActionButton.TabIndex = 6;
			this.testActionButton.Text = "&Test Action";
			this.testActionButton.Click += new System.EventHandler(this.testActionButton_Click);
			// 
			// WinFormHelloWorld
			// 
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.testActionButton);
			this.Controls.Add(this.ClearContext);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Name = "WinFormHelloWorld";
			this.Size = new System.Drawing.Size(448, 368);
			this.Load += new System.EventHandler(this.WinFormHelloWorld_Load);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void WinFormHelloWorld_Load(object sender, EventArgs e)
		{
			contextDisplay.Text = "Context is :" + Context;
		}

		/// <summary>
		/// This occurs when the context is changed.
		/// </summary>
		public override void NotifyContextChange(Context context)
		{
			base.NotifyContextChange(context);
			contextDisplay.Text = "Context Change: " + context.GetContext();
		}

		/// <summary>
		/// This happens whenever an action is fired against the application
		/// </summary>
		protected override void DoAction(RequestActionEventArgs args)
		{
			label2.Text = string.Format("Action fired: {0} Data = {1}", args.Action, args.Data);
			base.DoAction(args);
		}

		private void ClearContext_Click(object sender, EventArgs e)
		{
			Context = new Context();
			Context.Clear();
			FireChangeContext(new ContextEventArgs(Context));
			NotifyContextChange(this.Context);
		}

		private void testActionButton_Click(object sender, EventArgs e)
		{
			FireRequestAction(new RequestActionEventArgs(
				"Citrix StandAloneTestApp", "PushButton", "<GetFocus>true</GetFocus>"));

			// This shows what happens if you fire an action against all the apps.
			// If an app doesn't have the action defined in the database, then
			// an error message will appear, otherwise the action will happen.
			//this.FireRequestAction(new RequestActionEventArgs("*", "PushButton", ""))
		}
	}
}
