//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// AgentDesktopSession.cs
//
// This file contains the definition for an AgentDesktopSession.
//
//===============================================================================

#region Usings
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Ccf.Adapter.CustomerWS;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr;
#endregion

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// This represents a single customer session within CCF
	/// An enumeration over a session give a set of IHostedApplications.
	/// </summary>
	public class AgentDesktopSession : Session
	{
		private CustomerProviderCustomerRecord customer;

		public AgentDesktopSession( string name, int callID, CustomerProviderCustomerRecord customer ) : base(name, callID)
		{
			this.customer = customer;
		}

		public AgentDesktopSession( string name, int callID, CustomerProviderCustomerRecord customer, Guid sessionID ) : base(name, callID, sessionID)
		{
			this.customer = customer;
		}

		/// <summary>
		/// The customer record as defined by a web service.  This
		/// is a record that will be customized for each call center based
		/// on their needs.
		/// </summary>
		public virtual CustomerProviderCustomerRecord Customer
		{
			get { return customer; }
			set { customer = value; }
		}

		/// <summary>
		/// This method is used to return the customer ID.
		/// </summary>
		/// <returns>The customer ID the customer exists otherwise an empty string.</returns>
		public override string GetCustomerID()
		{
			if (this.customer != null && this.customer.CustomerID != null)
			{
				return this.customer.CustomerID;
			}
			else
			{
				return string.Empty;
			}
		}

		/// <summary>
		/// Function to save the sessions state for a customer
		/// </summary>
		/// <param name="active">true if this session is active</param>
		/// <returns>An XML string or null if there are no applications</returns>
		public override string Save( bool active )
		{
			// Check if the application host is null then return
			if ( this.AppHost == null )
			{
				return null;
			}

			// Create a string builder object to perform string concatenation
			StringBuilder sb = new StringBuilder();

			// build a header about the session
			sb.AppendFormat( "<Session id=\"{0:d}\" name=\"{1}\" active=\"{2}\" startTime=\"{3}\" presence=\"{4:d}\">", 
				this.SessionID, this.Name,
				active.ToString(),
				this.StartTime, this.PresenceState );

			// get the current customer info in xml
			string customerXml = String.Empty;
			if ( customer != null )
			{
				customerXml = WriteCustomer( customer, customer.GetType() );
			}

			// This context wraps the Ccf Conext, hence the "Context"
			// tag name and not "CcfContext"
			sb.AppendFormat( "<Context>{0}</Context><Customer>{1}</Customer>",
				(this.AppHost.GetContext()).GetContext(), customerXml );

			// Save the current workflow if there is any
			if ( this.Workflow != null && this.Workflow != String.Empty )
			{
				if (this.Workflow.ToString().Contains("<Session ID="))
				{
					//The line below is to be used with legacy WorkflowControl
					sb.AppendFormat("<WorkflowData>{0}</WorkflowData>", this.Workflow);
				}
				else
				{
					// The code below is to be used if you wish to use the refactored WfWorkflowControl
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(this.Workflow);
					XmlNode root = doc.DocumentElement;
					sb.AppendFormat("<WorkflowData>{0}</WorkflowData>", "<XML>" + root.InnerXml + "</XML>");

				}
			}

			// save information about each application
			foreach ( IHostedApplication app in this.AppHost )
			{
				try
				{
					// Add basic application information
					sb.AppendFormat( "<Application id=\"{0}\"><Name>{1}</Name>",
						app.ApplicationID.ToString(),
						app.ApplicationName );

					// In the future, we may wish to add all the app information or
					// make appHost self-serializable.

					// add the application state information
					sb.AppendFormat( "<State>{0}</State>", app.GetStateData() );

					sb.Append( "</Application>" );
				}
				catch ( Exception exp )
				{
					Logging.Error( localize.DESKTOP_MODULE_NAME, localize.DESKTOP_APP_SAVE_ERROR, exp );
				}
			}

			// Indicate the current application
			if ( this.FocusedApplication != null )
			{
				sb.AppendFormat( "<CurrentApplication>{0}</CurrentApplication>", this.FocusedApplication.ApplicationID );
			}
			// close the XML
			sb.Append( "</Session>" );
	
			string savedState = sb.ToString();
			return savedState;
		}
 

		/// <summary>
		/// Restores the state of a session from the passed XML.
		/// </summary>
		/// <param name="sessionInfoXml"></param>
		/// <returns>true if this is an active session, false if not</returns> 
		public override bool Restore( string sessionInfoXml )
		{
			XmlDocument doc = new XmlDocument();
			XmlNode     node;
			bool        active = false;

			// Right now we only permit a single transfer record
			doc.LoadXml( sessionInfoXml );

			// Get the name and presence state of the saved session
			node = doc.SelectSingleNode( "Session" );
			XmlAttribute attr = node.Attributes[ "name" ];
			if ( attr != null )
			{
				this.Name = attr.Value;
			}
			attr = node.Attributes[ "presence" ];
			if ( attr != null )
			{
				this.PresenceState = Convert.ToInt32( attr.Value );
			}

			// Return true if this was an active session
			attr = node.Attributes[ "active" ];
			if ( attr != null )
			{
				active = Convert.ToBoolean( attr.Value );
			}

			CustomerProviderCustomerRecord c = null;
			node = doc.SelectSingleNode( "descendant::Customer" );
			if ( node != null && 
				(this.customer == null || this.customer.CustomerID == String.Empty ) )
			{
				c = ReadCustomer( node.InnerXml ) as CustomerProviderCustomerRecord;
				if ( c != null && c.CustomerID != String.Empty )
				{
					customer = c;
				}
			}

			// Get workflow information, if any
			node = doc.SelectSingleNode( "descendant::WorkflowData" );
			if ( node != null )
			{
				//Workflow = node.InnerXml.Trim();

				// Workflow Driven Implementation:
				// Add the <restoredWorkflow/> tag to the workflow data so that
				// Workflow control recognise this pending workflow as restored one.
				XmlDocument xdoc = new XmlDocument();
				xdoc.LoadXml(node.InnerXml.Trim());
				XmlElement restoredWrkflNode = xdoc.CreateElement("restoredWorkflow");
				xdoc.DocumentElement.AppendChild(restoredWrkflNode);

				Workflow = xdoc.OuterXml;
			}

			// Get the saved application state and pass it to the current apps
			XmlNodeList nodeList = doc.SelectNodes( "descendant::Application" );
			if ( nodeList != null )
			{
				foreach ( XmlNode appNode in nodeList )
				{
					XmlNode stateNode = appNode.SelectSingleNode( "State" );
					if ( stateNode != null )
					{
						XmlAttribute id = appNode.Attributes[ "id" ];
						IHostedApplication app = GetApplication( Convert.ToInt32(id.Value) );
						if ( app != null )
						{
							app.SetStateData( stateNode.InnerXml );
						}
					}
				}
			}

			// Get the context information, if any
			// This context wraps the Ccf Conext, hence the "Context"
			// tag name and not "CcfContext"
			node = doc.SelectSingleNode( "descendant::Context" );
			if ( node != null )
			{
				Context context = new Context();
				context.SetContext(node.InnerXml.Trim());
				this.AppHost.SetContext(context);
			}

			//added to ensure that when actions are executed
			//after the correct context is set.
			if( customer.CustomerID != String.Empty)
			{
				this.AppHost.ExecuteApplicationState();
			}

			// Select the app the transferring agent was looking at
			node = doc.SelectSingleNode( "descendant::CurrentApplication" );
			if ( node != null )
			{
				int appID = Convert.ToInt32( node.InnerText );
				this.FocusedApplication = GetApplication( appID );
			}
			//AUDIT TRAIL
			if (null == customer)
			{
				LogData.CustomerID = String.Empty; 
			}
			else
			{
				LogData.CustomerID = customer.CustomerID; 
			}
			if (this.FocusedApplication != null)
			{
				LogData.ApplicationID = this.FocusedApplication.ApplicationID;
			}
			//AUDIT TRAIL END

			return active;
		}

		/// <summary>
		/// This reads XML from a string and returns a new instance with that information.
		/// </summary>
		/// <param name="xml">The xml string containing the customer.</param>
		/// <returns>An instance of a account class</returns>
		public static object ReadCustomer( string xml )
		{
			StringReader reader = null;

			try
			{
				CustomerProviderCustomerRecord local = new CustomerProviderCustomerRecord();

				XmlSerializer serializer = new XmlSerializer( local.GetType() );

				reader = new StringReader( xml );
				local = serializer.Deserialize( reader ) as CustomerProviderCustomerRecord;
				reader.Close();

				return local;
			}
			catch // ( Exception exp ) // when needed for debugging
			{
				if ( reader != null )
				{
					reader.Close();
				}

				return null;
			}
		}

		/// <summary>
		/// This writes an object instance to an XML string.
		/// </summary>
		/// <param name="o">Object to write out</param>
		/// <param name="type">Data type used to mark the Xml Serializer.</param>
		public virtual string WriteCustomer( object o, Type type )
		{
			StringWriter writer = null;
			string xmlStr = "";

			try
			{
				XmlSerializer serializer = new XmlSerializer( type );

				writer = new StringWriter();
				serializer.Serialize( writer, o );

				xmlStr = writer.ToString();

				// remove header from xml
				int i = xmlStr.IndexOf( '\n' );
				if ( i >= 0 )
				{
					xmlStr = xmlStr.Remove( 0, i+1 );
				}

				i = xmlStr.IndexOf( '>' );
				if ( i >= 0 )
				{
					xmlStr = xmlStr.Remove( 0, i+1 );
					xmlStr = "<CustomerProviderCustomerRecord>" + xmlStr;
				}

				writer.Close();
			}
			catch
			{
				if ( writer != null )
				{
					writer.Close();
				}
			}

			return xmlStr;
		}
	}
}