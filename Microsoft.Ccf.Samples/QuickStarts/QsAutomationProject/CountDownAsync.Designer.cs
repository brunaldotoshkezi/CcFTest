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
	partial class CountDownAsync
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
			System.Workflow.Activities.CodeCondition codecondition1 = new System.Workflow.Activities.CodeCondition();
			this.faultHandler1 = new System.Workflow.Activities.CodeActivity();
			this.setControlValue1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.SetControlValue();
			this.sleep = new System.Workflow.Activities.CodeActivity();
			this.faultHandlerActivity1 = new System.Workflow.ComponentModel.FaultHandlerActivity();
			this.sequenceActivity1 = new System.Workflow.Activities.SequenceActivity();
			this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
			this.whileActivity1 = new System.Workflow.Activities.WhileActivity();
			// 
			// faultHandler1
			// 
			this.faultHandler1.Name = "faultHandler1";
			this.faultHandler1.ExecuteCode += new System.EventHandler(this.faultHandler1_ExecuteCode);
			// 
			// setControlValue1
			// 
			this.setControlValue1.ApplicationName = "StandaloneTestApp";
			this.setControlValue1.ControlName = "textbox_acc";
			activitybind1.Name = "CountDownAsync";
			activitybind1.Path = "Iterations";
			this.setControlValue1.Name = "setControlValue1";
			this.setControlValue1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.SetControlValue.ControlValueProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
			// 
			// sleep
			// 
			this.sleep.Name = "sleep";
			this.sleep.ExecuteCode += new System.EventHandler(this.sleep_ExecuteCode);
			// 
			// faultHandlerActivity1
			// 
			this.faultHandlerActivity1.Activities.Add(this.faultHandler1);
			this.faultHandlerActivity1.FaultType = typeof(Microsoft.Ccf.HostedApplicationToolkit.AutomationHosting.AutomationHostingException);
			this.faultHandlerActivity1.Name = "faultHandlerActivity1";
			// 
			// sequenceActivity1
			// 
			this.sequenceActivity1.Activities.Add(this.sleep);
			this.sequenceActivity1.Activities.Add(this.setControlValue1);
			this.sequenceActivity1.Name = "sequenceActivity1";
			// 
			// faultHandlersActivity1
			// 
			this.faultHandlersActivity1.Activities.Add(this.faultHandlerActivity1);
			this.faultHandlersActivity1.Name = "faultHandlersActivity1";
			// 
			// whileActivity1
			// 
			this.whileActivity1.Activities.Add(this.sequenceActivity1);
			codecondition1.Condition += new System.EventHandler<System.Workflow.Activities.ConditionalEventArgs>(this.WhileCondition);
			this.whileActivity1.Condition = codecondition1;
			this.whileActivity1.Name = "whileActivity1";
			// 
			// CountDownAsync
			// 
			this.Activities.Add(this.whileActivity1);
			this.Activities.Add(this.faultHandlersActivity1);
			this.Name = "CountDownAsync";
			this.CanModifyActivities = false;

		}

		#endregion

		private FaultHandlerActivity faultHandlerActivity1;
		private FaultHandlersActivity faultHandlersActivity1;
		private CodeActivity faultHandler1;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.SetControlValue setControlValue1;
		private CodeActivity sleep;
		private SequenceActivity sequenceActivity1;
		private WhileActivity whileActivity1;





	}
}
