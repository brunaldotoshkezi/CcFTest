//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
// QuickStart External application adapter
//
//===============================================================================
using System;
using System.Xml;
using System.Drawing;

using Microsoft.Ccf.Csr;

namespace Microsoft.Ccf.QuickStarts
{
	public class QsExternalAppAdapter : Microsoft.Ccf.Csr.ApplicationAdapter
	{
		// Set up your locations for each component on the page.
		// If you wish, you could use Spy++ to get the actual names as well.

		// First Name text box
		int intFirstNameCoordX = 47;
		int intFirstNameCoordY = 32;

		// Last Name text box
		int intLastNameCoordX = 223;
		int intLastNameCoordY = 32;

		// Address Text box
		int intAddressCoordX = 47;
		int intAddressCoordY = 81;

		// Customer ID text box
		int intIDCoordX = 47;
		int intIDCoordY = 126;

		public override bool NotifyContextChange(Context context)
		{
			System.IntPtr ptr = this.Process.MainWindowHandle;

			// Find the control (first name) by position
			IntPtr childHwnd = Win32API.FindWindowByPosition(ptr, new Point(intFirstNameCoordX, intFirstNameCoordY));

			// Fill data out
			Win32API.SetWindowTextAny(childHwnd, context["CustomerFirstName"]);

			// Find the control (last name) by position
			childHwnd = Win32API.FindWindowByPosition(ptr, new Point(intLastNameCoordX, intLastNameCoordY));

			// Fill out the data
			Win32API.SetWindowTextAny(childHwnd, context["CustomerLastName"]);

			childHwnd = Win32API.FindWindowByPosition(ptr, new Point(intAddressCoordX, intAddressCoordY));
			Win32API.SetWindowTextAny(childHwnd, context["Street"]);

			childHwnd = Win32API.FindWindowByPosition(ptr, new Point(intIDCoordX, intIDCoordY));
			Win32API.SetWindowTextAny(childHwnd, context["CustomerID"]);

			// Hands control back over to the base class to notify next app of context change.
			base.NotifyContextChange(context);
			return true;
		}

		public override bool DoAction(Action action, ref string data)
		{
			switch (action.Name)
			{
				case "UpdateFirstName":
					// Get locations of what you want to update and handles
					System.IntPtr ptr = this.Process.MainWindowHandle;
					IntPtr childHwnd = Win32API.FindWindowByPosition(ptr, new Point(intFirstNameCoordX, intFirstNameCoordY));

					// Populate data into fields
					Win32API.SetWindowTextAny(childHwnd, data);
					break;
			}

			return base.DoAction(action, ref data);

		}
	}
}
