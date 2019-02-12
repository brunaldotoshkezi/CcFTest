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
	public sealed partial class HandleContextChanged: SequentialWorkflowActivity
	{
		public HandleContextChanged()
		{
			InitializeComponent();
		}

		private void contextChanged_ExecuteCode(object sender, EventArgs e)
		{
			string msg = string.Format("*** Context Changed: {0}: {1} ***", getContext1.ContextKey, getContext1.ContextValue);
			System.Diagnostics.Trace.WriteLine(msg);
		}
	}
}
