using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Microsoft.Ccf.QuickStarts.QsAutomationProject
{
	public sealed partial class CountDownAsync: SequentialWorkflowActivity
	{
		public CountDownAsync()
		{
			InitializeComponent();
		}

		private int iterations = 100;
		public string Iterations { get { return iterations.ToString(); } }

		private void sleep_ExecuteCode(object sender, EventArgs e)
		{
			System.Threading.Thread.Sleep(1000);
		}

		private void WhileCondition(object sender, ConditionalEventArgs e)
		{
			e.Result = iterations-- > 0;
		}

		private void faultHandler1_ExecuteCode(object sender, EventArgs e)
		{
			Exception faultingException = faultHandlerActivity1.Fault;
			bool sessionClosed = faultingException.Message.Contains("session closed");
			string message = string.Format("*** While Loop handled {0} sessionClosed={1}", faultingException.GetType(), sessionClosed);
			System.Diagnostics.Trace.WriteLine(message);
		}
	}

}
