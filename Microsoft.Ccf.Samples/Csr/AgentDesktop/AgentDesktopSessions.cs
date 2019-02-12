//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// This file contains the definition for an AgentDesktopSessions.
//
//===============================================================================

using System;
using Microsoft.Ccf.Adapter.CustomerWS;
using Microsoft.Ccf.Csr;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// The Sessions class is the class used to create and handle the individual AgentDesktopSession classes.
	/// </summary>
	public class AgentDesktopSessions : Sessions
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="useMultipleSessions">Boolean indicating if multiple sessions are allowed.</param>
		/// <param name="maxNumberOfSessions">Set the maximum number of sessions allowed.</param>
		public AgentDesktopSessions(bool useMultipleSessions, int maxNumberOfSessions) : base(useMultipleSessions, maxNumberOfSessions) { }

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="useMultipleSessions">Boolean indicating if multiple sessions are allowed.</param>
		public AgentDesktopSessions(bool useMultipleSessions) : base(useMultipleSessions) {}

		/// <summary>
		/// This method is used to handle session creation.
		/// </summary>
		/// <param name="name">Session name, frequently the customer's name.</param>
		/// <param name="callID">The callID, if the session is started via a call.</param>
		/// <param name="customer">The customer object reference.</param>
		/// <returns>The session object created.</returns>
		protected override Session CreateSession(string name, int callID, object customer)
		{
			return new AgentDesktopSession(name, callID, (CustomerProviderCustomerRecord) customer);
		}

		/// <summary>
		/// This method is used to handle session creation.
		/// </summary>
		/// <param name="name">Session name, frequently the customer's name.</param>
		/// <param name="callID">The callID, if the session is started via a call.</param>
		/// <param name="customer">The customer object reference.</param>
		/// <param name="sessionID">Session ID to allow clients to specify the session ID explicitly.</param>
		/// <returns>The session object created.</returns>
		protected override Session CreateSession(string name, int callID, object customer, Guid sessionID)
		{
			return new AgentDesktopSession(name, callID, (CustomerProviderCustomerRecord) customer, sessionID);
		}
	}
}
