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
	partial class MsgBeforeNavigate
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
			this.msgBeforeNavigate1 = new System.Workflow.Activities.CodeActivity();
			this.getActionData1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.GetActionData();
			// 
			// msgBeforeNavigate1
			// 
			this.msgBeforeNavigate1.Name = "msgBeforeNavigate1";
			this.msgBeforeNavigate1.ExecuteCode += new System.EventHandler(this.msgBeforeNavigate1_ExecuteCode);
			// 
			// getActionData1
			// 
			this.getActionData1.Name = "getActionData1";
			// 
			// MsgBeforeNavigate
			// 
			this.Activities.Add(this.getActionData1);
			this.Activities.Add(this.msgBeforeNavigate1);
			this.Name = "MsgBeforeNavigate";
			this.CanModifyActivities = false;

		}

		#endregion

		private CodeActivity msgBeforeNavigate1;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.GetActionData getActionData1;
	}
}
