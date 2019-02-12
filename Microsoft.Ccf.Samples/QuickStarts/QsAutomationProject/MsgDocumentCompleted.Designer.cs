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
	partial class MsgDocumentCompleted
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
			this.msgDocumentCompleted1 = new System.Workflow.Activities.CodeActivity();
			this.getActionData1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.GetActionData();
			// 
			// msgDocumentCompleted1
			// 
			this.msgDocumentCompleted1.Name = "msgDocumentCompleted1";
			this.msgDocumentCompleted1.ExecuteCode += new System.EventHandler(this.msgDocumentCompleted1_ExecuteCode);
			// 
			// getActionData1
			// 
			this.getActionData1.Name = "getActionData1";
			// 
			// MsgDocumentCompleted
			// 
			this.Activities.Add(this.getActionData1);
			this.Activities.Add(this.msgDocumentCompleted1);
			this.Name = "MsgDocumentCompleted";
			this.CanModifyActivities = false;

		}

		#endregion

		private Microsoft.Ccf.HostedApplicationToolkit.Activity.GetActionData getActionData1;
		private CodeActivity msgDocumentCompleted1;

	}
}
