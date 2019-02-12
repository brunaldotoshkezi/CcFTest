//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// An example of a web-based "signer" provider.
//
//===============================================================================

using System;
using System.Collections;
using System.Windows.Forms;

using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.Browser.Web;
using Microsoft.Ccf.Csr.Signer.Providers;

namespace Microsoft.Ccf.Samples.Csr.Signer
{
	/// <summary>
	/// This class provides the methods to perform automatic sign-in for the web applications.
	/// </summary>
	public class WebAppSigner: ISigner
	{
		// The attributes to be used for identifying the HTML elements corresponding to the
		// credential fields.
		private const string ID_ATTRIBUTE = "ID";
		private const string NAME_ATTRIBUTE = "Name";
		private const string VALUE_ATTRIBUTE = "Value";

		// The operations need to done on the credential control elements
		private const string SET_OPERATION = "Set";
		private const string CLICK_OPERATION = "Click";
		protected WebBrowser webBrowser;

		/// <summary>
		/// This method performs the auto sign-on by populating the given credentials 
		/// in their corresponding credential fields and submitting the page.
		/// </summary>
		/// <param name="application">
		/// The credentials of the user to be populating in the credential fields identified.
		/// </param>
		public void DoLogin(object application)
		{
			HostedWebApplication app = (HostedWebApplication)application;
			webBrowser = (System.Windows.Forms.WebBrowser) app.TopLevelWindow;
			System.Windows.Forms.HtmlDocument axDoc = webBrowser.Document;

			if (null == app.LoginFields || app.LoginFields.Count == 0 || axDoc == null)
			{
				return;
			}

			int setFieldCount = 0;

			// For each credential field, perform the required operation.
			foreach (LoginFieldRecord field in app.LoginFields)
			{
				HtmlElement element;

				int sequence = (null != field.ControlSequence && string.Empty != field.ControlSequence ? int.Parse(field.ControlSequence) : -1);

				// Get the reference to the element corresonding to the credential field.
				//element = GetElement(axDoc, field.AttributeIdentity, field.AttributeValue, sequence);
				// TODO Update GetElement function to work with new WebBrowser
				// for first pass go with ID
				element = webBrowser.Document.GetElementById(field.AttributeValue);

				// If element is not found, then auto sign-on is failed, stop the auto sign-on
				if (null == element)
				{
					throw new LoginFieldNotFoundException(field.LabelName);
				}

				switch (field.Operation)
				{
					// Populate the credentials in the value attribute of the element.
					case SET_OPERATION:
					{
						Set(element, app.AgentCreds[setFieldCount]);
						setFieldCount ++;
						break;
					}
					// Perform click operation on the element.
					case CLICK_OPERATION:
					{
						Click(element);
						break;
					}
				}
			}
		}

		///// <summary>
		///// This method returns the HTML element corresponding to the 
		///// given credential field details.
		///// </summary>
		///// <param name="document">
		///// Reference to the HTML document of login page.
		///// </param>
		///// <param name="attribute">
		///// Identification attribute - ID/Name/Value.
		///// </param>
		///// <param name="attributeValue">
		///// Value of the identification attribute to be matched.
		///// </param>
		///// <param name="sequence">
		///// The sequence of the element with the same given Name/Value attribute value.
		///// </param>
		///// <returns>
		///// Reference to the identified HTML element.
		///// </returns>
		//private IHTMLElement GetElement(HTMLDocumentClass document, string attribute, string attributeValue, int sequence)
		//{
		//    switch (attribute)
		//    {
		//        // If Id of the element is to be used to identify it, 
		//        // then return the element with the ID as given value.
		//        case ID_ATTRIBUTE:
		//            {
		//                IHTMLElement element = document.getElementById(attributeValue);
		//                return element;
		//            }
		//        // If Name attribute of the element to be used to identify it,
		//        // then the element at the given sequence in the list of elements having 
		//        // their value of Name attribute as given value.
		//        case NAME_ATTRIBUTE:
		//            {
		//                // Get all the elements having the Name attribute value as attributeValue.
		//                IHTMLElementCollection elements = document.getElementsByName(attributeValue);

		//                if (null == elements)
		//                    return null;

		//                // Get the element at the given sequence in the above popultedlist of elements.
		//                int seq = 1;
		//                foreach (IHTMLElement element in elements)
		//                {
		//                    if (seq == sequence)
		//                        return element;
		//                    else
		//                        seq++;
		//                }
		//                break;
		//            }
		//        // If Value attribute of the element to be used to identify it,
		//        // then the element at the given sequence in the list of elements having 
		//        // their value of Value attribute as given value.
		//        case VALUE_ATTRIBUTE:
		//            {
		//                ArrayList elements = new ArrayList();

		//                // Get all the elements having the Value attribute value as attributeValue.
		//                foreach (IHTMLElement element in document.all)
		//                {
		//                    if (System.DBNull.Value != element.getAttribute("value", 0) && (string)element.getAttribute("value", 0) == attributeValue)
		//                        elements.Add(element);
		//                }

		//                // Get the element at the given sequence in the above popultedlist of elements.
		//                int seq = 1;
		//                foreach (object elementObj in elements)
		//                {
		//                    if (seq == sequence)
		//                        return (IHTMLElement)elementObj;
		//                    else
		//                        seq++;
		//                }
		//                break;
		//            }
		//        default:
		//            {
		//                return null;
		//            }
		//    }

		//    return null;
		//}

		/// <summary>
		/// This method sets the value of the given element to given text.
		/// </summary>
		/// <param name="element">The element, whose value to be set</param>
		/// <param name="text">The text ot be set.</param>
		private void Set(HtmlElement element, string text)
		{
			element.SetAttribute("value", text);
		}

		/// <summary>
		/// This method performs the click operation on the given element.
		/// </summary>
		/// <param name="element">The HTML element.</param>
		private void Click(HtmlElement element)
		{
			//element.Click();
			element.InvokeMember("Click");
		}

		#region ISigner Members

		void Microsoft.Ccf.Csr.Signer.Providers.ISigner.DoLogin(object HostedApp)
		{
			// TODO:  Add WebAppSigner.Microsoft.Ccf.Csr.Signer.Providers.ISigner.DoLogin implementation
		}

		#endregion
	}
}
