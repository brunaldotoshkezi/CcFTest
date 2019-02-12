//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2007 Microsoft Corp.
//
// An example of a web chat call using the multichannel system
// 
//===============================================================================
using System;
using System.ServiceModel;
using System.Collections.Specialized;

using Microsoft.Ccf.Common;
using Microsoft.Ccf.Multichannel;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Adapter.CustomerWS;
using Microsoft.Ccf.Multichannel.AgentDesktopService;

namespace Microsoft.Ccf.Adapter.Multichannel
{
	/// <summary>
	/// Summary description for WebchatChannel.
	/// </summary>
	public class WebchatChannel : MultichannelBase, IMultichannel
	{
		/// <summary>
		/// This is the method used to initialize the listener
		/// </summary>
		/// <param name="name">The name of the listener.</param>
		/// <param name="configValue">The configuration values (parameters) for the listener.</param>
		public void Initialize(string name, NameValueCollection configValue)
		{ }

		/// <summary>
		/// This is the general method to handle the message and send along as appopriate
		/// </summary>
		/// <param name="channel">The serialized channel information object.</param>
		/// <returns>A message the service wishes to return</returns>
		public string SendMessage(string channel)
		{
			ChannelInformation channelInformation = null;
			try
			{
				ChannelInformation ci = (ChannelInformation)GeneralFunctions.Deserialize(channel, typeof(ChannelInformation));
				if (ci == null)
				{
					ci = new ChannelInformation();
				}

				// Call out to get data
				CustomerClient customer = new CustomerClient();
				CustomerProviderCustomerRecord custrec = customer.GetCustomerByID(ci.CustomerID);

				customer.Close();

				ci.City = custrec.City; // = "Redmond";
				ci.Country = custrec.Country; // = "USA";
				ci.CustomerID = custrec.CustomerID; // = "1";
				ci.FirstName = custrec.FirstName; // = "Maeve";
				ci.LastName = custrec.LastName; // = "Elizabeth";
				ci.PhoneHome = custrec.PhoneHome; // = "4252210456";
				ci.PhoneMobile = custrec.PhoneMobile; // = "4252210456";
				ci.PhoneWork = custrec.PhoneWork; // = "4252210456";
				ci.State = custrec.State; // = "WA";
				ci.Street = custrec.Street; // = "Rainy Street";
				ci.ZipCode = custrec.ZipCode; // = "98008";

				AgentClientData agent = RetrieveAgentClient(ci.Agent);
				Uri toUri = new Uri(String.Format("http://{0}:{1}/", agent.IPAddress.Trim(), agent.Port.Trim()));
				EndpointAddress address = new EndpointAddress(toUri.ToString());
				string retValue = string.Empty;
				MultichannelSubsystemClient mcssclient = new MultichannelSubsystemClient();
				try
				{
					mcssclient.Endpoint.Address = address;
					mcssclient.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();
					retValue = mcssclient.Call(GeneralFunctions.Serialize<ChannelInformation>(ci));
				}
				catch (Exception ex)
				{
					Console.Write(ex.Message);
				}
			}
			catch (Exception ex)
			{
				Logging.Error(this.ToString(), ex.Message);
				return ex.Message;
			}

			return GeneralFunctions.Serialize(channelInformation);
		}
	}
}
