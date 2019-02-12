//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// This file contains the interface definition for the IHostedToolBarButton interface.
// 
//===============================================================================

using System.Windows.Forms;
using Microsoft.Ccf.Csr;

namespace Microsoft.Ccf.Samples.HostedControlInterfaces
{
	/// <summary>
	/// Summary description for IHostedToolBarButton.
	/// </summary>
	public interface IHostedToolBarButton : IHostedApplication3
	{
		/// <summary>
		/// Get or set the status ToolBarButton
		/// </summary>
		ToolBarButton Status{ get; set; }
	}
}
