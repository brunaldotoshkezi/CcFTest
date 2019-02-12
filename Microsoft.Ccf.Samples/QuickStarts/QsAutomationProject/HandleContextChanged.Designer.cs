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
	partial class HandleContextChanged
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
			this.printContextChanged = new System.Workflow.Activities.CodeActivity();
			this.getContext1 = new Microsoft.Ccf.HostedApplicationToolkit.Activity.GetContext();
			// 
			// printContextChanged
			// 
			this.printContextChanged.Name = "printContextChanged";
			this.printContextChanged.ExecuteCode += new System.EventHandler(this.contextChanged_ExecuteCode);
			// 
			// getContext1
			// 
			this.getContext1.ContextKey = "CustomerFirstName";
			this.getContext1.Name = "getContext1";
			// 
			// HandleContextChanged
			// 
			this.Activities.Add(this.getContext1);
			this.Activities.Add(this.printContextChanged);
			this.Name = "HandleContextChanged";
			this.CanModifyActivities = false;

		}

		#endregion

		private CodeActivity printContextChanged;
		private Microsoft.Ccf.HostedApplicationToolkit.Activity.GetContext getContext1;





























	}
}
