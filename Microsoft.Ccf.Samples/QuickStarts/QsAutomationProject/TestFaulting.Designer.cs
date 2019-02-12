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
	partial class TestFaulting
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
			this.codeActivity2 = new System.Workflow.Activities.CodeActivity();
			this.codeActivity1 = new System.Workflow.Activities.CodeActivity();
			this.faultHandlerException = new System.Workflow.ComponentModel.FaultHandlerActivity();
			this.faultHandlerApplicationException = new System.Workflow.ComponentModel.FaultHandlerActivity();
			this.faultHandlersActivity1 = new System.Workflow.ComponentModel.FaultHandlersActivity();
			this.getControlValue2 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.GetControlValue();
			this.getControlValue1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.GetControlValue();
			// 
			// codeActivity2
			// 
			this.codeActivity2.Name = "codeActivity2";
			this.codeActivity2.ExecuteCode += new System.EventHandler(this.codeActivity2_ExecuteCode);
			// 
			// codeActivity1
			// 
			this.codeActivity1.Name = "codeActivity1";
			this.codeActivity1.ExecuteCode += new System.EventHandler(this.codeActivity1_ExecuteCode);
			// 
			// faultHandlerException
			// 
			this.faultHandlerException.Activities.Add(this.codeActivity2);
			this.faultHandlerException.FaultType = typeof(System.Exception);
			this.faultHandlerException.Name = "faultHandlerException";
			// 
			// faultHandlerApplicationException
			// 
			this.faultHandlerApplicationException.Activities.Add(this.codeActivity1);
			this.faultHandlerApplicationException.FaultType = typeof(System.ApplicationException);
			this.faultHandlerApplicationException.Name = "faultHandlerApplicationException";
			// 
			// faultHandlersActivity1
			// 
			this.faultHandlersActivity1.Activities.Add(this.faultHandlerApplicationException);
			this.faultHandlersActivity1.Activities.Add(this.faultHandlerException);
			this.faultHandlersActivity1.Name = "faultHandlersActivity1";
			// 
			// getControlValue2
			// 
			this.getControlValue2.ApplicationName = "StandaloneTestApp";
			this.getControlValue2.ControlName = "undefined control name";
			this.getControlValue2.ExceptionsMasked = true;
			this.getControlValue2.Name = "getControlValue2";
			// 
			// getControlValue1
			// 
			this.getControlValue1.ApplicationName = "StandaloneTestApp";
			this.getControlValue1.ControlName = "textbox_acc";
			this.getControlValue1.Name = "getControlValue1";
			// 
			// TestFaulting
			// 
			this.Activities.Add(this.getControlValue1);
			this.Activities.Add(this.getControlValue2);
			this.Activities.Add(this.faultHandlersActivity1);
			this.Name = "TestFaulting";
			this.CanModifyActivities = false;

		}

		#endregion

		private Microsoft.Ccf.HostedApplicationToolkit.Activity.GetControlValue getControlValue2;
		private CodeActivity codeActivity1;
		private FaultHandlerActivity faultHandlerApplicationException;
		private FaultHandlersActivity faultHandlersActivity1;
		private CodeActivity codeActivity2;
		private FaultHandlerActivity faultHandlerException;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.GetControlValue getControlValue1;




















	}
}
