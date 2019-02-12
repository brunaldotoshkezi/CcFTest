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
	partial class DefaultActionSampleWebApp
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
			System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
			this.registerActionForEvent3 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent();
			this.registerActionForEvent2 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent();
			this.registerActionForEvent1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent();
			activitybind2.Name = "registerActionForEvent1";
			activitybind2.Path = "ApplicationName";
			// 
			// registerActionForEvent3
			// 
			activitybind1.Name = "registerActionForEvent1";
			activitybind1.Path = "ActionApplicationName";
			this.registerActionForEvent3.ActionName = "MsgBeforeNewWindow";
			this.registerActionForEvent3.ControlName = null;
			this.registerActionForEvent3.Enabled = false;
			this.registerActionForEvent3.EventName = "BeforeNewWindow";
			this.registerActionForEvent3.Name = "registerActionForEvent3";
			this.registerActionForEvent3.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent.ActionApplicationNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
			this.registerActionForEvent3.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent.ApplicationNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
			activitybind4.Name = "registerActionForEvent1";
			activitybind4.Path = "ApplicationName";
			// 
			// registerActionForEvent2
			// 
			activitybind3.Name = "registerActionForEvent1";
			activitybind3.Path = "ActionApplicationName";
			this.registerActionForEvent2.ActionName = "MsgBeforeNavigate";
			this.registerActionForEvent2.ControlName = null;
			this.registerActionForEvent2.Enabled = false;
			this.registerActionForEvent2.EventName = "BeforeNavigate";
			this.registerActionForEvent2.Name = "registerActionForEvent2";
			this.registerActionForEvent2.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent.ActionApplicationNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
			this.registerActionForEvent2.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent.ApplicationNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
			// 
			// registerActionForEvent1
			// 
			this.registerActionForEvent1.ActionApplicationName = "Sample Web App";
			this.registerActionForEvent1.ActionName = "MsgDocumentCompleted";
			this.registerActionForEvent1.ApplicationName = "Sample Web App";
			this.registerActionForEvent1.ControlName = "";
			this.registerActionForEvent1.EventName = "DocumentCompleted";
			this.registerActionForEvent1.Name = "registerActionForEvent1";
			// 
			// DefaultActionSampleWebApp
			// 
			this.Activities.Add(this.registerActionForEvent1);
			this.Activities.Add(this.registerActionForEvent2);
			this.Activities.Add(this.registerActionForEvent3);
			this.Name = "DefaultActionSampleWebApp";
			this.CanModifyActivities = false;

		}

		#endregion

		private Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent registerActionForEvent3;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent registerActionForEvent2;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent registerActionForEvent1;









	}
}
