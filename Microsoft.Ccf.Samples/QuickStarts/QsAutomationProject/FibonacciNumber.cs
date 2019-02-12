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
	public sealed partial class FibonacciNumber: SequentialWorkflowActivity
	{
		public FibonacciNumber()
		{
			InitializeComponent();
		}

		public int N
		{
			get
			{
				try
				{
					return Int32.Parse(getActionData.ActionData.ToString());
				}
				catch (FormatException)
				{
					return 0;
				}
			}
		}

		public string N1
		{
			get { return (N - 1).ToString(); }
			set { fN1 = Int32.Parse(value); }
		}

		public string N2
		{
			get { return (N - 2).ToString(); }
			set { fN2 = Int32.Parse(value); }
		}

		private int fN1, fN2;

		public string Sum
		{
			get { return (fN1 + fN2).ToString(); }
		}

		private void IsNotGreaterThanOne(object sender, ConditionalEventArgs e)
		{
			e.Result = (N <= 1);
		}

		private void IsGreaterThanOne(object sender, ConditionalEventArgs e)
		{
			e.Result = (N > 1);
		}
	}
}
