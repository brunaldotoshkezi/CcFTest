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
	partial class MsgButtonPressed
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
			this.msgButtonPressed1 = new System.Workflow.Activities.CodeActivity();
			// 
			// msgButtonPressed1
			// 
			this.msgButtonPressed1.Name = "msgButtonPressed1";
			this.msgButtonPressed1.ExecuteCode += new System.EventHandler(this.msgButtonPressed1_ExecuteCode);
			// 
			// MsgButtonPressed
			// 
			this.Activities.Add(this.msgButtonPressed1);
			this.Name = "MsgButtonPressed";
			this.CanModifyActivities = false;

		}

		#endregion

		private CodeActivity msgButtonPressed1;

	}
}
