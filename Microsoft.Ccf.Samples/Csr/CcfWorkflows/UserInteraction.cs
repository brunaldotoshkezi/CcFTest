using System;
using System.Threading;
using System.Windows.Forms;
using System.Workflow.Runtime;
using System.Workflow.Activities;

namespace Microsoft.Ccf.Samples.CcfWorkflows
{
	//
	// Workflow communication interface which defines the contract
	// between a local service and a workflow
	//
	[ExternalDataExchange]
	internal interface IInteractionService
	{
		// transfer set focus execution to another thread
		void SetFocus(IntPtr handle, int hostedAppId);
		// transfer do action execution to another thread
		void SetDoAction(IntPtr handle, string hostedAppName, string action);
	}

	public delegate void FocusDelegate(IntPtr handle, int hostedAppId);
	public delegate void SetActionDelegate(IntPtr handle, string hostedAppName, string action);

	//
	// This is the local service that implements the contract on the host side
	// it implements the methods and calls the events, which are
	// implemented by the workflow.
	//
	public class UserInteractionServiceImpl : IInteractionService
	{
		public static FocusDelegate SetFocusTarget;
		public static SetActionDelegate SetDoActionTarget;

		// hook the desired function for set focus in host thread via SetFocusTarget
		public void SetFocus(IntPtr handle,int hostedAppId)
		{
			SetFocusTarget(handle,hostedAppId);
		}
		// hook the desired function for do action in host thread via SetDoActionTarget
		public void SetDoAction(IntPtr handle, string hostedAppName, string action)
		{
			SetDoActionTarget(handle, hostedAppName, action);
		}
	}
}