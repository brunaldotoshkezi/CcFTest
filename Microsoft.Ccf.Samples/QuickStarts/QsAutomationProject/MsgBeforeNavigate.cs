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
	public sealed partial class MsgBeforeNavigate: SequentialWorkflowActivity
	{
		public MsgBeforeNavigate()
		{
			InitializeComponent();
		}

		private void msgBeforeNavigate1_ExecuteCode(object sender, EventArgs e)
		{
			System.Diagnostics.Trace.WriteLine("** MsgBeforeNavigate: Action Data=" + getActionData1.ActionData);
		}
	}
}
