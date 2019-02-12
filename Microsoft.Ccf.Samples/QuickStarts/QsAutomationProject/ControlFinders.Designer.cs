using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Reflection;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Microsoft.Ccf.QuickStarts.QsAutomationProject
{
	partial class ControlFinders
	{
		#region Designer generated code
		
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
        [System.Diagnostics.DebuggerNonUserCode]
		private void InitializeComponent()
		{
			this.CanModifyActivities = true;
			System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
			System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
			System.Workflow.Activities.CodeCondition codecondition3 = new System.Workflow.Activities.CodeCondition();
			System.Workflow.Activities.CodeCondition codecondition4 = new System.Workflow.Activities.CodeCondition();
			this.printControl2NotFound = new System.Workflow.Activities.CodeActivity();
			this.printControl2Found = new System.Workflow.Activities.CodeActivity();
			this.printControl1NotFound = new System.Workflow.Activities.CodeActivity();
			this.printControl1Found = new System.Workflow.Activities.CodeActivity();
			this.ifControl2NotFound = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifControl2Found = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifControl1NotFound = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifControl1Found = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifElseActivity2 = new System.Workflow.Activities.IfElseActivity();
			this.controlFinder2 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.ControlFinder();
			this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
			this.controlFinder1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.ControlFinder();
			this.sequenceActivity2 = new System.Workflow.Activities.SequenceActivity();
			this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
			this.parallelActivity1 = new System.Workflow.Activities.ParallelActivity();
			// 
			// printControl2NotFound
			// 
			this.printControl2NotFound.Name = "printControl2NotFound";
			this.printControl2NotFound.ExecuteCode += new System.EventHandler(this.printControl2NotFound_ExecuteCode);
			// 
			// printControl2Found
			// 
			this.printControl2Found.Name = "printControl2Found";
			this.printControl2Found.ExecuteCode += new System.EventHandler(this.printControl2Found_ExecuteCode);
			// 
			// printControl1NotFound
			// 
			this.printControl1NotFound.Name = "printControl1NotFound";
			this.printControl1NotFound.ExecuteCode += new System.EventHandler(this.printControl1NotFound_ExecuteCode);
			// 
			// printControl1Found
			// 
			this.printControl1Found.Name = "printControl1Found";
			this.printControl1Found.ExecuteCode += new System.EventHandler(this.printControl1NotFound_ExecuteCode);
			// 
			// ifControl2NotFound
			// 
			this.ifControl2NotFound.Activities.Add(this.printControl2NotFound);
			codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsControl2NotFound);
			this.ifControl2NotFound.Condition = codecondition1;
			this.ifControl2NotFound.Name = "ifControl2NotFound";
			// 
			// ifControl2Found
			// 
			this.ifControl2Found.Activities.Add(this.printControl2Found);
			codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsControl2Found);
			this.ifControl2Found.Condition = codecondition2;
			this.ifControl2Found.Name = "ifControl2Found";
			// 
			// ifControl1NotFound
			// 
			this.ifControl1NotFound.Activities.Add(this.printControl1NotFound);
			codecondition3.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsControl1NotFound);
			this.ifControl1NotFound.Condition = codecondition3;
			this.ifControl1NotFound.Name = "ifControl1NotFound";
			// 
			// ifControl1Found
			// 
			this.ifControl1Found.Activities.Add(this.printControl1Found);
			codecondition4.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsControl1Found);
			this.ifControl1Found.Condition = codecondition4;
			this.ifControl1Found.Name = "ifControl1Found";
			// 
			// ifElseActivity2
			// 
			this.ifElseActivity2.Activities.Add(this.ifControl2Found);
			this.ifElseActivity2.Activities.Add(this.ifControl2NotFound);
			this.ifElseActivity2.Name = "ifElseActivity2";
			// 
			// controlFinder2
			// 
			this.controlFinder2.ApplicationName = "StandaloneTestApp";
			this.controlFinder2.ControlName = "textbox_acc";
			this.controlFinder2.Name = "controlFinder2";
			// 
			// ifElseActivity1
			// 
			this.ifElseActivity1.Activities.Add(this.ifControl1Found);
			this.ifElseActivity1.Activities.Add(this.ifControl1NotFound);
			this.ifElseActivity1.Name = "ifElseActivity1";
			// 
			// controlFinder1
			// 
			this.controlFinder1.ApplicationName = "Sample Web App";
			this.controlFinder1.ControlName = "name textbox";
			this.controlFinder1.Name = "controlFinder1";
			// 
			// sequenceActivity2
			// 
			this.sequenceActivity2.Activities.Add(this.controlFinder2);
			this.sequenceActivity2.Activities.Add(this.ifElseActivity2);
			this.sequenceActivity2.Name = "sequenceActivity2";
			// 
			// sequenceActivity1
			// 
			this.sequenceActivity1.Activities.Add(this.controlFinder1);
			this.sequenceActivity1.Activities.Add(this.ifElseActivity1);
			this.sequenceActivity1.Name = "sequenceActivity1";
			// 
			// parallelActivity1
			// 
			this.parallelActivity1.Activities.Add(this.sequenceActivity1);
			this.parallelActivity1.Activities.Add(this.sequenceActivity2);
			this.parallelActivity1.Name = "parallelActivity1";
			// 
			// ControlFinders
			// 
			this.Activities.Add(this.parallelActivity1);
			this.Name = "ControlFinders";
			this.CanModifyActivities = false;

		}

		#endregion

		private Microsoft.Ccf.HostedApplicationToolkit.Activity.ControlFinder controlFinder1;
		private SequenceActivity sequenceActivity2;
		private SequenceActivity sequenceActivity1;
		private IfElseBranchActivity ifControl1NotFound;
		private IfElseBranchActivity ifControl1Found;
		private IfElseActivity ifElseActivity1;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.ControlFinder controlFinder2;
		private IfElseBranchActivity ifControl2NotFound;
		private IfElseBranchActivity ifControl2Found;
		private IfElseActivity ifElseActivity2;
		private CodeActivity printControl1Found;
		private CodeActivity printControl1NotFound;
		private CodeActivity printControl2Found;
		private CodeActivity printControl2NotFound;
		private ParallelActivity parallelActivity1;




































	}
}
