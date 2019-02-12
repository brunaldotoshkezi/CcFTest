//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
//===============================================================================

#region Usings

using System.Xml;
using Microsoft.Ccf.HostedApplicationToolkit.DataDrivenAdapter;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	// this is an empty DDA implementation to make the AutomationAdapter.Initialize() method happy
	public class HatStandAloneTestAppProxyDda : DataDrivenAdapterBase
	{
		public HatStandAloneTestAppProxyDda(XmlDocument appInitString, object appObject) : base(appInitString)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// release managed resources here
			}
			// release unmanaged resources here
		}

		protected override string OperationHandler(OperationType op, string controlName, string controlValue)
		{
			return string.Empty;
		}
	}
}
