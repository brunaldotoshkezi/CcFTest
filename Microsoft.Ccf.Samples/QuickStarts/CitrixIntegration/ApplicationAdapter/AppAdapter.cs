//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// Citrix quick start for application adapter
//
//===================================================================================

#region Using Declarations

using Microsoft.Ccf.Csr;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	/// <summary>
	/// This is a adapter to test overriding functions within a CCF hosted application.
	/// </summary>
	public class AppAdapter : ApplicationAdapter
	{
		public override bool DoAction(Action action, RequestActionEventArgs raArgs)
		{
			return base.DoAction(action, raArgs);
		}

		public override bool NotifyContextChange(Context context)
		{
			return base.NotifyContextChange(context);
		}
	}
}
