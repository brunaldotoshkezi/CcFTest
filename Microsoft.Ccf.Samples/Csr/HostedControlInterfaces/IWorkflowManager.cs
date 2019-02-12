//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// This file contains the interface definition for the IWorkflowManager interface.
// 
//===============================================================================
#pragma warning disable 1591

#region Usings
using System;
using System.Windows.Forms;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr;
#endregion

namespace Microsoft.Ccf.Samples.HostedControlInterfaces
{
	/// <summary>
	/// Interface definition for IWorkflowManager
	/// </summary>
	public interface IWorkflowManager : IHostedApplication3
	{
		/// <summary>
		/// this event will be raised when a hosted application has to be brought to focus.
		/// </summary>
		event WorkFlowEventHandler FocusHostedApp;
	}
}
#pragma warning restore 1591