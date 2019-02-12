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
	partial class DefaultActionStandAloneTestApp
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
			System.Workflow.ComponentModel.ActivityBind activitybind3 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind4 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind5 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind1 = new System.Workflow.ComponentModel.ActivityBind();
			System.Workflow.ComponentModel.ActivityBind activitybind2 = new System.Workflow.ComponentModel.ActivityBind();
			this.registerActionForEvent2 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent();
			this.unregisterActionForEvent1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.UnregisterActionForEvent();
			this.registerActionForEvent1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent();
			// 
			// registerActionForEvent2
			// 
			this.registerActionForEvent2.ActionApplicationName = "StandaloneTestApp";
			this.registerActionForEvent2.ActionName = "HandleContextChanged";
			this.registerActionForEvent2.ApplicationName = "StandaloneTestApp";
			this.registerActionForEvent2.ControlName = "";
			this.registerActionForEvent2.EventName = "ContextChanged";
			this.registerActionForEvent2.Name = "registerActionForEvent2";
			activitybind3.Name = "registerActionForEvent1";
			activitybind3.Path = "ActionName";
			activitybind4.Name = "registerActionForEvent1";
			activitybind4.Path = "ApplicationName";
			activitybind5.Name = "registerActionForEvent1";
			activitybind5.Path = "EventName";
			// 
			// unregisterActionForEvent1
			// 
			activitybind1.Name = "registerActionForEvent1";
			activitybind1.Path = "ActionApplicationName";
			activitybind2.Name = "registerActionForEvent1";
			activitybind2.Path = "ControlName";
			this.unregisterActionForEvent1.Enabled = false;
			this.unregisterActionForEvent1.Name = "unregisterActionForEvent1";
			this.unregisterActionForEvent1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.UnregisterActionForEvent.ActionApplicationNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind1)));
			this.unregisterActionForEvent1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.UnregisterActionForEvent.ActionNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind3)));
			this.unregisterActionForEvent1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.UnregisterActionForEvent.ApplicationNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind4)));
			this.unregisterActionForEvent1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.UnregisterActionForEvent.ControlNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind2)));
			this.unregisterActionForEvent1.SetBinding(Microsoft.Ccf.HostedApplicationToolkit.Activity.UnregisterActionForEvent.EventNameProperty, ((System.Workflow.ComponentModel.ActivityBind)(activitybind5)));
			// 
			// registerActionForEvent1
			// 
			this.registerActionForEvent1.ActionApplicationName = "StandaloneTestApp";
			this.registerActionForEvent1.ActionName = "MsgButtonPressed";
			this.registerActionForEvent1.ApplicationName = "StandaloneTestApp";
			this.registerActionForEvent1.ControlName = "button_acc";
			this.registerActionForEvent1.EventName = "ButtonPressed";
			this.registerActionForEvent1.Name = "registerActionForEvent1";
			// 
			// DefaultActionStandAloneTestApp
			// 
			this.Activities.Add(this.registerActionForEvent1);
			this.Activities.Add(this.unregisterActionForEvent1);
			this.Activities.Add(this.registerActionForEvent2);
			this.Name = "DefaultActionStandAloneTestApp";
			this.CanModifyActivities = false;

		}

		#endregion

		private Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent registerActionForEvent2;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.UnregisterActionForEvent unregisterActionForEvent1;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.RegisterActionForEvent registerActionForEvent1;

















	}
}
