//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
// QuickStart Web application adapter
//
//===============================================================================
using Microsoft.Ccf.Csr;
using System.Windows.Forms;

namespace Microsoft.Ccf.QuickStarts
{
	public class QsWebAppAdapter : WebApplicationAdapter
	{
		// Action update event handler
		public override bool DoAction(Microsoft.Ccf.Csr.HostedWebApplication.WebAction action, ref string data)
		{
			// Get browser DOM and element collection
			// Create an XML Document to load the passed in data to.
			HtmlDocument htmlDoc = this.Browser.Document;
			HtmlElementCollection htmlElementCollection = htmlDoc.All;

			// Check action name for something we know how to process.
			switch (action.Name)
			{
				case "UpdateFirstName":

					HtmlElement htmlFirstName = htmlElementCollection["txtFirstName"];

					// Populate data on page.
					htmlFirstName.SetAttribute("value", data);
					break;

				case "UpdateLastName":
					HtmlElement htmlLastName = htmlElementCollection["txtLastName"];

					htmlLastName.SetAttribute("value", data);
					break;
			}
			return false;
		}

		// This is the context change handler.
		public override bool NotifyContextChange(Context context)
		{
			// Populating text fields from context information.
			HtmlDocument htmlDoc = this.Browser.Document;
			if (htmlDoc != null)
			{
				HtmlElementCollection htmlElementCollection = htmlDoc.All;

				HtmlElement htmlFirstName = htmlElementCollection["txtFirstName"];
				htmlFirstName.SetAttribute(htmlFirstName.ToString(), context["CustomerFirstName"]);


			}
			// Hands control back over to the base class to notify next app of context change.
			base.NotifyContextChange(context);

			return true;
		}
	}
}
