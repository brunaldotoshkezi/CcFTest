//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// The CustomerWorkflowManager
//
//===================================================================================

#region Usings
using System;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;

using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Samples.CcfWorkflows;
using Microsoft.Ccf.Samples.HostedControlInterfaces;
#endregion

namespace Microsoft.Ccf.Samples.CustomerWorkflowManager
{
	/// <summary>
	/// The CustomerWorkflowManager class. This class implements the human workflow via "Human Workflow Automation"
	/// action and Human Workflow functions (see Human Workflow region below). It is also an example of how 
	/// workflow runtime can be accessed by a System Integrator.
	/// </summary>
	public partial class CustomerWorkflowManager : WorkflowManager.WorkflowManager, IWorkflowManager
	{
		private static object workflowService = new CcfWorkflows.UserInteractionServiceImpl();

		/// <summary>
		/// This event will be raised when a hosted application has to be brought to focus.
		/// </summary>
		public event WorkFlowEventHandler FocusHostedApp;

		//delegates for human workflow automation
		private delegate void SetActionDelegate(string hostedAppName, string action);
		private delegate void SetFocusDelegate(int hostedAppId);

		/// <summary>
		/// Constructor which must exist for a hosted application to snap into desktop.
		/// </summary>
		public CustomerWorkflowManager( int appID, string appName, string initString ) : 
				base( appID, appName, initString )
		{
			InitializeComponent();
		}

		/// <summary>
		/// Prevents this control from appearing in the lists within Agent Desktop's GUI.
		/// </summary>
		public override bool IsListed
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// This method is an example of how to add a service to the workflow runtime.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="data"></param>
		public override void DoAction(Microsoft.Ccf.Csr.Action action, string data)
		{
			Microsoft.Ccf.Samples.CcfWorkflows.UserInteractionServiceImpl.SetFocusTarget = SetFocusReceive;
			Microsoft.Ccf.Samples.CcfWorkflows.UserInteractionServiceImpl.SetDoActionTarget = DoActionReceive;

			base.DoAction(action, data);

			if (action.Name == "Human Workflow Automation")
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlNode node;

				this.AddServices(workflowService);
				
				// Prepare parameters from data sent with action
				xmlDocument.LoadXml(data);

				// Fill in the parameters
				Dictionary <string, object> parameters = new Dictionary<string, object>();
				parameters.Add("WFControlHandle", this.Handle);
				node = xmlDocument.SelectSingleNode( "Workflow/StepName" );
				parameters.Add("StepName", node.InnerText);

				node = xmlDocument.SelectSingleNode("Workflow/HostedApplicationId");
				parameters.Add("HostedApplicationId", Int32.Parse(node.InnerText));

				node = xmlDocument.SelectSingleNode("Workflow/HostedApplicationName");
				parameters.Add("HostedApplicationName", node.InnerText);

				node = xmlDocument.SelectSingleNode("Workflow/Action"); 
				parameters.Add("Action", node.InnerText);

				this.StartRuntime();

				System.Workflow.Runtime.WorkflowInstance instance = WorkflowManager.WorkflowManager.WorkflowRuntime.CreateWorkflow(typeof(CcfWorkflowStep), parameters);
				instance.Start();
			}
		}

		#region Human Workflow related functions
		/// <summary>
		/// Fires requested action
		/// </summary>
		/// <param name="hostedAppName"></param>
		/// <param name="action"></param>
		public void DoWorkflowAction(string hostedAppName, string action)
		{
			if (this.InvokeRequired)
			{
				SetActionDelegate setAction = new SetActionDelegate(this.DoWorkflowAction);
				this.Invoke(setAction, hostedAppName, action);
			}
			else
			{
				this.FireRequestAction(new RequestActionEventArgs(hostedAppName, action, ""));
			}
		}

		/// <summary>
		/// Workflow runtime thread hands control over to the CustomerWorkflowManager thread
		/// and passes in doaction parameters
		/// </summary>
		public void DoActionReceive(IntPtr handle, string hostedAppName, string action)
		{
			CustomerWorkflowManager form = (CustomerWorkflowManager)(Form.FromHandle(handle));
			if (form != null)
			{
				form.DoWorkflowAction(hostedAppName, action);
			}
		}

		/// <summary>
		/// Passes on the SetFocus to Desktop
		/// </summary>
		/// <param name="hostedAppId"></param>
		public void SetFocus(int hostedAppId)
		{
			if (this.InvokeRequired)
			{
				SetFocusDelegate setFocus = new SetFocusDelegate(this.SetFocus);
				this.Invoke(setFocus, hostedAppId);
			}
			else
			{
				WorkflowArgs e = new WorkflowArgs();
				e.ApplicationId = hostedAppId;
				FocusHostedApp(this, e);
			}
		}

		/// <summary>
		/// Workflow runtime thread hands control over to the WorkFlowControl thread
		/// and passes in setfocus parameters
		/// </summary>
		public void SetFocusReceive(IntPtr handle, int hostedAppId)
		{
			CustomerWorkflowManager form = (CustomerWorkflowManager)(Form.FromHandle(handle));

			if (form != null)
			{
				form.SetFocus(hostedAppId);
			}
		}
		#endregion

	}
}
