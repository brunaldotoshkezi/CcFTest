//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
//===============================================================================

using System;
using System.Xml;

using Microsoft.Ccf.HostedApplicationToolkit.DataDrivenAdapter;

namespace Microsoft.Ccf.QuickStarts.QsDataDrivenAdapterExt
{
	/// <summary>
	/// This is an example of extending an existing Data Driven Adapter, specifically the
	/// WinDataDrivenAdapter that is provided with CCF.
	/// </summary>
	public class WinDDAExt : WinDataDrivenAdapter
	{
		// This required constructor provides an opportunity to observe the application initstring (appInitString)
		// as well as the identified/acquired top-level window handle (appObject).
		// These parameters need to be passed on to the superclass:
		public WinDDAExt(XmlDocument appInitString, object appObject) : base(appInitString, appObject)
		{
		}

		// The DataDrivenAdapterBase superclass implements IDisposable, and offers the following
		// overridable method that enables managed and unmanaged resource cleanup:
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// release managed resources here
			}
			// release unmanaged resources here then propagate disposal to base class
			base.Dispose(disposing);
		}

		protected override string OperationHandler(OperationType op, string controlName, string controlValue)
		{
			// GetControlConfig() is an inherited method from the abstract class, DataDrivenAdapterBase,
			// that is useful for retreiving a control configuration XML node by name from within the
			// <DataDrivenAdapterBindings/> node within the application initstring.
			//
			// <tagname name="controlName">
			//		<!-- other user-defined xml configuration content here -->
			// </tagname>
			XmlNode n = GetControlConfig(controlName);

			if (n == null)
			{
				// This will be true when the requested control name configuration cannot be found
				// within <DataDrivenAdapterBindings/> in the application initstring.
				// Unless there is some desired special handling, the base class method can be
				// leveraged to issue the "control not found" error as follows:
				return base.OperationHandler(op, controlName, controlValue);
			}

			// This is a way of intercepting activity for all tags that have "NewTag" as a prefix.
			// Any tag that does not fit this critiera is routed to the superclass for default processing.
			if (!n.Name.StartsWith("NewTag", StringComparison.Ordinal))
			{
				return base.OperationHandler(op, controlName, controlValue);
			}

			switch (op)
			{
				// The return value type is "string", but the value must be either "true" or "false"
				// depending upon the outcome of the operation.
				// It is recommended to leverage the Boolean.TrueString and Boolean.FalseString enumerations.
				case OperationType.FindControl:
					return (Environment.TickCount & 1).Equals(1) ? Boolean.TrueString : Boolean.FalseString;

				// The return value is the outcome of this operation.
				case OperationType.GetControlValue:
					return Environment.TickCount.ToString();

				// Perform whatever is necessary for accomplishing this operation.
				// No return value is necessary for this operation, as it is discarded.
				case OperationType.SetControlValue:
					if (n.Name.Equals("NewTagBadTag", StringComparison.Ordinal))
					{
						throw new ApplicationException("Exceptions thrown within DDAs will be bubbled to the WF runtime, for WF Fault Handling");
					}
					break;

				// Perform whatever is necessary for accomplishing this operation.
				// No return value is necessary for this operation, as it is discarded.
				case OperationType.ExecuteControlAction:
					// Here is how to source an event.
					// The event type name is required, followed by an optional control name and control
					// value that can be used to further qualify the event
					ControlChangedEventArgs e = new ControlChangedEventArgs("EventWinDdaExt", "controlName", "controlValue");
					NotifyControlChanged(e);
					break;
			}

			return string.Empty;
		}

		// IAccessible FindAccObj(string controlName, bool throwExceptionIfNotFound, out int childId)
		//
		// Inherited from WinDataDrivenAdapter.  Uses the <Path/> criteria for locating IAccessible nodes.


		// IntPtr FindWindow(string controlName, bool throwExceptionIfNotFound)
		//
		// Inherited from WinDataDrivenAdapter.  Uses the <FindWindow/> criteria for locating a Window handle.
	}
}
