using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;

namespace Microsoft.Ccf.Samples.CcfWorkflows
{
	public partial class CcfWorkflowStep : SequentialWorkflowActivity
	{
		private static DependencyProperty WFControlHandleProperty = System.Workflow.ComponentModel.DependencyProperty.Register("WFControlHandle", typeof(IntPtr), typeof(CcfWorkflowStep));

		/// <summary>
		/// Get or set the workflow control handle
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public IntPtr WFControlHandle
		{
			get
			{
				return ((IntPtr)(base.GetValue(CcfWorkflowStep.WFControlHandleProperty)));
			}
			set
			{
				base.SetValue(CcfWorkflowStep.WFControlHandleProperty, value);
			}
		}

		private static DependencyProperty StepNameProperty = System.Workflow.ComponentModel.DependencyProperty.Register("StepName", typeof(string), typeof(CcfWorkflowStep));

		/// <summary>
		/// Get or set the step name
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string StepName
		{
			get
			{
				return ((string)(base.GetValue(CcfWorkflowStep.StepNameProperty)));
			}
			set
			{
				base.SetValue(CcfWorkflowStep.StepNameProperty, value);
			}
		}

		private static DependencyProperty ActionProperty = System.Workflow.ComponentModel.DependencyProperty.Register("Action", typeof(string), typeof(CcfWorkflowStep));

		/// <summary>
		/// Get or set the name of the action
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string Action
		{
			get
			{
				return ((string)(base.GetValue(CcfWorkflowStep.ActionProperty)));
			}
			set
			{
				base.SetValue(CcfWorkflowStep.ActionProperty, value);
			}
		}

		private static DependencyProperty HostedApplicationIdProperty = System.Workflow.ComponentModel.DependencyProperty.Register("HostedApplicationId", typeof(int), typeof(CcfWorkflowStep));

		/// <summary>
		/// Get or set the hosted application id
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int HostedApplicationId
		{
			get
			{
				return ((int)(base.GetValue(CcfWorkflowStep.HostedApplicationIdProperty)));
			}
			set
			{
				base.SetValue(CcfWorkflowStep.HostedApplicationIdProperty, value);
			}
		}

		private static DependencyProperty HostedApplicationNameProperty = System.Workflow.ComponentModel.DependencyProperty.Register("HostedApplicationName", typeof(string), typeof(CcfWorkflowStep));
		/// <summary>
		/// Get or set the hosted application name
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public string HostedApplicationName
		{
			get
			{
				return ((string)(base.GetValue(CcfWorkflowStep.HostedApplicationNameProperty)));
			}
			set
			{
				base.SetValue(CcfWorkflowStep.HostedApplicationNameProperty, value);
			}
		}
	}
}
