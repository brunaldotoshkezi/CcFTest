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
	partial class MsgHelloWorld
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
			this.msgHelloWorld1 = new System.Workflow.Activities.CodeActivity();
			// 
			// msgHelloWorld1
			// 
			this.msgHelloWorld1.Name = "msgHelloWorld1";
			this.msgHelloWorld1.ExecuteCode += new System.EventHandler(this.msgHelloWorld1_ExecuteCode);
			// 
			// MsgHelloWorld
			// 
			this.Activities.Add(this.msgHelloWorld1);
			this.Name = "MsgHelloWorld";
			this.CanModifyActivities = false;

		}

		#endregion

		private CodeActivity msgHelloWorld1;
	}
}
