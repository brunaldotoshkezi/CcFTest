using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
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
	partial class FibonacciNumber
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
			System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
			System.Workflow.Activities.CodeCondition codecondition2 = new System.Workflow.Activities.CodeCondition();
			this.returnSum = new Microsoft.Ccf.HostedApplicationToolkit.Activity.SetActionData();
			this.doAction2 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction();
			this.doAction1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction();
			this.returnOne = new Microsoft.Ccf.HostedApplicationToolkit.Activity.SetActionData();
			this.ifGreaterThanOne = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifNotGreaterThanOne = new System.Workflow.Activities.IfElseBranchActivity();
			this.ifElseActivity1 = new System.Workflow.Activities.IfElseActivity();
			this.getActionData = new Microsoft.Ccf.HostedApplicationToolkit.Activity.GetActionData();
			// 
			// returnSum
			// 
			activitybind1.Name = "FibonacciNumber";
			activitybind1.Path = "Sum";
			this.returnSum.Name = "returnSum";
			this.returnSum.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.SetActionData.ActionDataProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
			activitybind3.Name = "doAction1";
			activitybind3.Path = "ActionName";
			activitybind4.Name = "doAction1";
			activitybind4.Path = "ApplicationName";
			// 
			// doAction2
			// 
			activitybind2.Name = "FibonacciNumber";
			activitybind2.Path = "N2";
			this.doAction2.Description = "Invokes Fibonacci(n-2)";
			this.doAction2.Name = "doAction2";
			this.doAction2.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction.ActionDataProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
			this.doAction2.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction.ActionNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
			this.doAction2.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction.ApplicationNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
			// 
			// doAction1
			// 
			activitybind5.Name = "FibonacciNumber";
			activitybind5.Path = "N1";
			this.doAction1.ActionName = "FibonacciNumber";
			this.doAction1.ApplicationName = "StandaloneTestApp";
			this.doAction1.Description = "Invokes Fibonacci(n-1)";
			this.doAction1.Name = "doAction1";
			this.doAction1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction.ActionDataProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
			// 
			// returnOne
			// 
			this.returnOne.ActionData = "1";
			this.returnOne.Name = "returnOne";
			// 
			// ifGreaterThanOne
			// 
			this.ifGreaterThanOne.Activities.Add(this.doAction1);
			this.ifGreaterThanOne.Activities.Add(this.doAction2);
			this.ifGreaterThanOne.Activities.Add(this.returnSum);
			codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsGreaterThanOne);
			this.ifGreaterThanOne.Condition = codecondition1;
			this.ifGreaterThanOne.Name = "ifGreaterThanOne";
			// 
			// ifNotGreaterThanOne
			// 
			this.ifNotGreaterThanOne.Activities.Add(this.returnOne);
			codecondition2.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.IsNotGreaterThanOne);
			this.ifNotGreaterThanOne.Condition = codecondition2;
			this.ifNotGreaterThanOne.Name = "ifNotGreaterThanOne";
			// 
			// ifElseActivity1
			// 
			this.ifElseActivity1.Activities.Add(this.ifNotGreaterThanOne);
			this.ifElseActivity1.Activities.Add(this.ifGreaterThanOne);
			this.ifElseActivity1.Name = "ifElseActivity1";
			// 
			// getActionData
			// 
			this.getActionData.Name = "getActionData";
			// 
			// FibonacciNumber
			// 
			this.Activities.Add(this.getActionData);
			this.Activities.Add(this.ifElseActivity1);
			this.Name = "FibonacciNumber";
			this.CanModifyActivities = false;

		}

		#endregion

		private Microsoft.Ccf.HostedApplicationToolkit.Activity.GetActionData getActionData;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction doAction1;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.DoAction doAction2;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.SetActionData returnSum;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.SetActionData returnOne;
		private IfElseBranchActivity ifGreaterThanOne;
		private IfElseBranchActivity ifNotGreaterThanOne;
		private IfElseActivity ifElseActivity1;


































































	}
}
