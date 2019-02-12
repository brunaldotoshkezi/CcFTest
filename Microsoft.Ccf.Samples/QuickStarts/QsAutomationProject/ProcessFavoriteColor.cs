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
	public sealed partial class ProcessFavoriteColor: SequentialWorkflowActivity
	{
		public ProcessFavoriteColor()
		{
			InitializeComponent();
		}

		public String generatedPhrase = default(System.String);

		private void ExecuteCode4Black(object sender, EventArgs e)
		{
			generatedPhrase = "The night is black.";
		}

		private void ExecuteCode4Blue(object sender, EventArgs e)
		{
			generatedPhrase = "The sky is blue";
		}

		private void ExecuteCode4Green(object sender, EventArgs e)
		{
			generatedPhrase = "The tree is green";
		}

		private void ExecuteCode4Purple(object sender, EventArgs e)
		{
			generatedPhrase = "The color purple is a movie";
		}

		private void ExecuteCode4Red(object sender, EventArgs e)
		{
			generatedPhrase = "Ketchup is red";
		}

		private void ExecuteCode4White(object sender, EventArgs e)
		{
			generatedPhrase = "Blank paper is white";
		}

		private void ExecuteCode4Yellow(object sender, EventArgs e)
		{
			generatedPhrase = "Some phone book pages are yellow";
		}

	}
}
