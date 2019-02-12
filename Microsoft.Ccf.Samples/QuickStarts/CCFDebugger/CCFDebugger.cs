//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
// CCF Hosted Application for debugging issues with other CCF apps.
//
//===============================================================================
using System;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Ccf.Csr;

namespace Microsoft.Ccf.Samples.CCFDebugger
{
	/// <summary>
	/// This tool is used to help with debugging hosted applications within CCF.
	/// </summary>
	public class CCFDebugger : HostedControl
	{
		private Button ClearOutput;
		private TextBox output;
		private TextBox actionName;
		private TextBox application;
		private TextBox actionData;
		private Label label1;
		private Label label2;
		private Label label3;
		private GroupBox groupBox1;
		private Button runAction;
		private Button pauseTracing;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private GroupBox groupBox2;
		private Button changeContext;
		private Label label4;
		private TextBox contextEdit;
		private Label label6;

		/// <summary>
		/// Reference to a trace listener that will receive all CCF traces.
		/// </summary>
		MyTraceListener myListener;

		public CCFDebugger()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public CCFDebugger(int appID, string appName, string initString)
			: base(appID, appName, initString)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// clear other text fields
			output.Text = String.Empty;
			application.Text = String.Empty;
			actionName.Text = String.Empty;
			actionData.Text = String.Empty;

			// Create a new text writer which will display the
			// output trace from the service when debug is on.
			myListener = new MyTraceListener();
			myListener.Output = this.output;
			//   System.Diagnostics.Trace.Listeners.Remove( "Default" );
			Trace.Listeners.Add(myListener);

			// Set the synchronization context of the listener to the current winform context.
			// This context will be used to avoid cross thread errors caused by monitoring isolated applications
			myListener.SyncContext = WindowsFormsSynchronizationContext.Current;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			Microsoft.Ccf.Csr.Context context1 = new Microsoft.Ccf.Csr.Context();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CCFDebugger));
			this.ClearOutput = new System.Windows.Forms.Button();
			this.output = new System.Windows.Forms.TextBox();
			this.actionName = new System.Windows.Forms.TextBox();
			this.application = new System.Windows.Forms.TextBox();
			this.actionData = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.runAction = new System.Windows.Forms.Button();
			this.pauseTracing = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.changeContext = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.contextEdit = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// ClearOutput
			// 
			this.ClearOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ClearOutput.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ClearOutput.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ClearOutput.Location = new System.Drawing.Point(544, 174);
			this.ClearOutput.Name = "ClearOutput";
			this.ClearOutput.Size = new System.Drawing.Size(114, 24);
			this.ClearOutput.TabIndex = 1;
			this.ClearOutput.Text = "&Clear Trace";
			this.ClearOutput.Click += new System.EventHandler(this.ClearOutput_Click);
			// 
			// output
			// 
			this.output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.output.BackColor = System.Drawing.Color.White;
			this.output.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.output.Location = new System.Drawing.Point(8, 8);
			this.output.Multiline = true;
			this.output.Name = "output";
			this.output.ReadOnly = true;
			this.output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.output.Size = new System.Drawing.Size(656, 158);
			this.output.TabIndex = 0;
			this.output.Text = "output";
			// 
			// actionName
			// 
			this.actionName.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.actionName.Location = new System.Drawing.Point(112, 48);
			this.actionName.Name = "actionName";
			this.actionName.Size = new System.Drawing.Size(280, 21);
			this.actionName.TabIndex = 3;
			this.actionName.Text = "actionName";
			// 
			// application
			// 
			this.application.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.application.Location = new System.Drawing.Point(112, 24);
			this.application.Name = "application";
			this.application.Size = new System.Drawing.Size(280, 21);
			this.application.TabIndex = 1;
			this.application.Text = "application";
			// 
			// actionData
			// 
			this.actionData.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.actionData.Location = new System.Drawing.Point(112, 72);
			this.actionData.Name = "actionData";
			this.actionData.Size = new System.Drawing.Size(280, 21);
			this.actionData.TabIndex = 5;
			this.actionData.Text = "actionData";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(24, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Application:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(24, 72);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "Data:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(24, 48);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Action Name:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox1.Controls.Add(this.runAction);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.actionData);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.application);
			this.groupBox1.Controls.Add(this.actionName);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(8, 230);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(528, 104);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Action Test";
			// 
			// runAction
			// 
			this.runAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.runAction.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.runAction.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.runAction.Location = new System.Drawing.Point(400, 48);
			this.runAction.Name = "runAction";
			this.runAction.Size = new System.Drawing.Size(112, 24);
			this.runAction.TabIndex = 6;
			this.runAction.Text = "&Run Action";
			this.runAction.Click += new System.EventHandler(this.runAction_Click);
			// 
			// pauseTracing
			// 
			this.pauseTracing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.pauseTracing.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.pauseTracing.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pauseTracing.Location = new System.Drawing.Point(544, 214);
			this.pauseTracing.Name = "pauseTracing";
			this.pauseTracing.Size = new System.Drawing.Size(114, 24);
			this.pauseTracing.TabIndex = 2;
			this.pauseTracing.Text = "Press to Pause";
			this.pauseTracing.Click += new System.EventHandler(this.pauseTracing_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox2.Controls.Add(this.changeContext);
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.contextEdit);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(8, 174);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(528, 56);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Context Test";
			// 
			// changeContext
			// 
			this.changeContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.changeContext.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.changeContext.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.changeContext.Location = new System.Drawing.Point(400, 22);
			this.changeContext.Name = "changeContext";
			this.changeContext.Size = new System.Drawing.Size(112, 24);
			this.changeContext.TabIndex = 2;
			this.changeContext.Text = "Change Context";
			this.changeContext.Click += new System.EventHandler(this.changeContext_Click);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(24, 72);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 20);
			this.label4.TabIndex = 4;
			this.label4.Text = "Data:";
			// 
			// contextEdit
			// 
			this.contextEdit.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.contextEdit.Location = new System.Drawing.Point(112, 22);
			this.contextEdit.Name = "contextEdit";
			this.contextEdit.Size = new System.Drawing.Size(280, 21);
			this.contextEdit.TabIndex = 1;
			this.contextEdit.Text = "context";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Tahoma", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(24, 22);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 20);
			this.label6.TabIndex = 0;
			this.label6.Text = "Context:";
			// 
			// CCFDebugger
			// 
			context1.ContextInformation = "<CcfContext></CcfContext>";
			this.Context = context1;
			this.Controls.Add(this.pauseTracing);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.output);
			this.Controls.Add(this.ClearOutput);
			this.Controls.Add(this.groupBox2);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "CCFDebugger";
			this.Size = new System.Drawing.Size(672, 344);
			this.Load += new System.EventHandler(this.CCFDebugger_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Code that runs when the UI form is created.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CCFDebugger_Load(object sender, EventArgs e)
		{
			// Force the title to be the text below
			Text = "CCF Debugger";
		}

		/// <summary>
		/// Clears the trace text in the output window.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearOutput_Click(object sender, EventArgs e)
		{
			myListener.Clear();
		}

		/// <summary>
		/// Catches the context when it changes for display in the trace window.
		/// </summary>
		/// <param name="context"></param>
		public override void NotifyContextChange(Context context)
		{
			Trace.WriteLine(Name + ": Context = " + context.GetContext());
			base.NotifyContextChange(context);

			// Set the context text for editing to change it again
			if (currentlyTracing)
			{
				contextEdit.Text = context.GetContext();
			}
		}

		/// <summary>
		/// Performs an action based on what the user entered in the text boxes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void runAction_Click(object sender, EventArgs e)
		{
			RequestActionEventArgs raArgs = new RequestActionEventArgs(application.Text, actionName.Text, actionData.Text);
			FireRequestAction(raArgs);
			if (raArgs.ActionReturnValue != null && raArgs.ActionReturnValue.Length > 0)
			{
				Trace.WriteLine(string.Format("{0}: ActionReturnValue: {1}", Name, raArgs.ActionReturnValue));
			}
		}

		bool currentlyTracing = true;
		/// <summary>
		/// Stops or Continues tracing.  Useful when you need to stop and
		/// look at some trace information while more is coming in.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pauseTracing_Click(object sender, EventArgs e)
		{
			if (currentlyTracing)
			{
				Trace.Listeners.Remove(myListener);
			}
			else
			{
				Trace.Listeners.Add(myListener);
			}
			currentlyTracing = !currentlyTracing;
			pauseTracing.Text = currentlyTracing ? "Press to Pause" : "Press to Trace";
		}

		/// <summary>
		/// Tests changing the context so you can see how the applications work.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void changeContext_Click(object sender, EventArgs e)
		{
			Context newContext = new Context();
			newContext.SetContext(contextEdit.Text);
			FireChangeContext(new ContextEventArgs(newContext));
			Trace.WriteLine(Name + ": Context = " + Context.GetContext());
		}

	}

	/// <summary>
	/// This class is for attaching to the trace/debug messages to display them.
	/// </summary>
	public class MyTraceListener : TraceListener
	{
		private SynchronizationContext syncCtx;
		public SynchronizationContext SyncContext
		{
			set { syncCtx = value; }
		}

		private TextBox _output;
		public TextBox Output
		{
			set { _output = value; }
		}

		/// <summary>
		/// Keeps the string displayed in a string builder to make it faster
		/// recreating it.
		/// </summary>
		protected StringBuilder shadow = new StringBuilder();

		private static string TimeStamp()
		{
			return DateTime.Now.ToString("T") + ": ";
		}

		/// <summary>
		/// Clear all previous results.
		/// </summary>
		public void Clear()
		{
			shadow.Remove(0, shadow.Length);
			syncCtx.Post(new SendOrPostCallback(delegate { _output.Text = String.Empty; }), null);
		}

		/// <summary>
		/// Called whenever a Debug.Write() or Trace.Write() is called.
		/// </summary>
		/// <param name="Message"></param>
		public override void Write(String Message)
		{
			if (_output != null)
			{
				shadow.Append(Message);

				// Don't save too much and take all the memory
				if (shadow.Length > 10000)
				{
					shadow.Remove(0, 2000);
				}

				syncCtx.Post(new SendOrPostCallback(delegate { _output.Text = shadow.ToString(); }), null);
			}
		}

		/// <summary>
		/// Called whenever a Debug.WriteLine() or Trace.WriteLine() is called.
		/// </summary>
		/// <param name="Message"></param>
		public override void WriteLine(String Message)
		{
			if (_output != null)
			{
				// insert \r\n where needed
				Message = Message.Replace('\r', ' ');
				Message = Message.Replace(@"\n", @"\r\n");

				Write(TimeStamp() + Message + "\r\n");

				syncCtx.Post(new SendOrPostCallback(delegate { _output.Select(shadow.Length - 1, 0); _output.ScrollToCaret(); }), null);
			}
		}
	}
}