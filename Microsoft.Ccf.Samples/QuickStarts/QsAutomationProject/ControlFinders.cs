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
	public sealed partial class ControlFinders : SequentialWorkflowActivity
	{
		public ControlFinders()
		{
			InitializeComponent();
		}

		private void IsControl1Found(object sender, ConditionalEventArgs e)
		{
			e.Result = controlFinder1.ControlFound;
		}

		private void IsControl1NotFound(object sender, ConditionalEventArgs e)
		{
			e.Result = !controlFinder1.ControlFound;
		}

		private void IsControl2Found(object sender, ConditionalEventArgs e)
		{
			e.Result = controlFinder2.ControlFound;
		}

		private void IsControl2NotFound(object sender, ConditionalEventArgs e)
		{
			e.Result = !controlFinder2.ControlFound;
		}

		private void codeActivity1_ExecuteCode(object sender, EventArgs e)
		{
			System.Diagnostics.Trace.WriteLine("** Control #1 Found **");
		}

		private void printControl1NotFound_ExecuteCode(object sender, EventArgs e)
		{
			System.Diagnostics.Trace.WriteLine("** Control #1 Not Found **");
		}

		private void printControl2Found_ExecuteCode(object sender, EventArgs e)
		{
			System.Diagnostics.Trace.WriteLine("** Control #2 Found **");
		}

		private void printControl2NotFound_ExecuteCode(object sender, EventArgs e)
		{
			System.Diagnostics.Trace.WriteLine("** Control #2 Not Found **");
		}
	}
}
