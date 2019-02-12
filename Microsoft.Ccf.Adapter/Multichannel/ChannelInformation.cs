//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//===============================================================================
// ChannelInformation.cs
// 
// An example class of the MultichannelChannelInformation class
// 
//===============================================================================
using System;
using Microsoft.Ccf.Multichannel;

namespace Microsoft.Ccf.Adapter.Multichannel
{
	/// <summary>
	/// The ChannelInformation class used in the multichannel example applications.
	/// </summary>
	[Serializable]
	[System.ServiceModel.DataContractFormat()]
	public class ChannelInformation : MultichannelChannelInformation
	{
		public Guid ClientID;
		public string MachineName;
		public DateTime dateTime;
		public string Comment;

		public string CustomerID;
		public string FirstName;
		public string LastName;
		public string Street;
		public string City;
		public string State;
		public string ZipCode;
		public string Country;
		public string PhoneHome;
		public string PhoneWork;
		public string PhoneMobile;

		public string SpeechServerContext;
	}
}
