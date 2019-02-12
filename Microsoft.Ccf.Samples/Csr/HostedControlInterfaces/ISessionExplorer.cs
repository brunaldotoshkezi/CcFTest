//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// This file contains the interface definition for the ISessionExplorer interface.
// 
//===============================================================================

using System;
using Microsoft.Ccf.Csr;
using System.Windows.Forms;
using Microsoft.Ccf.Csr.UIConfiguration;

namespace Microsoft.Ccf.Samples.HostedControlInterfaces
{
	/// <summary>
	/// ISessionExplorer interface.  Used to allow SessionExplorer hosted control
	/// interact with the Agent Desktop more directly.
	/// </summary>
	public interface ISessionExplorer : IHostedApplication3
	{
		/// <summary>
		/// Event for when a hosted app is selected
		/// </summary>
		event SessionExplorerEventHandler SEHostedAppSelected;

		/// <summary>
		/// Event for when a session is closed
		/// </summary>
		event SessionExplorerEventHandler SessionClosed;

		/// <summary>
		/// Set the AppsUI value
		/// </summary>
		CCFAppsUI AppsUI{ set; }

		/// <summary>
		/// Get or set the value indicating wheather the control is displayed
		/// </summary>
		bool ControlVisible{ set; get; }

		/// <summary>
		/// Get of set the height of the control
		/// </summary>
		int ControlHeight{set; get;}

		/// <summary>
		/// get or set the parent container of the control
		/// </summary>
		Control ControlParent{ get; set; }

		/// <summary>
		/// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
		/// </summary>
		void ControlRefresh();

		/// <summary>
		/// Select the given application's node in the treeview
		/// </summary>
		/// <param name="appWithFocus">Application to focus on</param>
		void FocusOnApplication( IHostedApplication appWithFocus );

		/// <summary>
		/// Complete redraw of the session explorer UI.
		/// </summary>
		void RefreshView();

		/// <summary>
		/// Adds the session to the tree control if its not already there
		/// </summary>
		void AddSession(Microsoft.Ccf.Csr.Session session, bool selected );

		/// <summary>
		/// Removes a session from the session explorer tree.
		/// </summary>
		/// <param name="session"></param>
		void RemoveSession(Microsoft.Ccf.Csr.Session session );

		/// <summary>
		/// Remove the applications for a session from the SessionExplorerControl
		/// </summary>
		/// <param name="session"></param>
		void RemoveWorkflowApplicationNodes(Session session);

		/// <summary>
		/// Adds the application nodes back to sessionexplorer
		/// </summary>
		void AddWorkflowApplicationNodes(Microsoft.Ccf.Csr.Session session, bool selected);

		/// <summary>
		/// Add a single application node.  If the application is global then will add
		/// for all session, else will add only for the active session passed in.
		/// </summary>
		/// <param name="session">The current active session</param>
		/// <param name="app">The application to add</param>
		void AddApplicationNode(Microsoft.Ccf.Csr.Session session, IHostedApplication app);

		/// <summary>
		/// Remove a single application node from the session explorer.  If the application is
		/// global then will remove for all session, else will remove only for the active session
		/// passed in.
		/// </summary>
		/// <param name="session">The current active session</param>
		/// <param name="app">The application to remove</param>
		void RemoveApplicationNode(Microsoft.Ccf.Csr.Session session, IHostedApplication app);
	}

	/// <summary>
	/// Session exploreer event handler.
	/// </summary>
	/// <param name="sender">Sending object.</param>
	/// <param name="e">EventArgs class.</param>
	public delegate void SessionExplorerEventHandler(object sender, EventArgs e);
}
