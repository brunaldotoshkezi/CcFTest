//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2006 Microsoft Corp.
//
//===================================================================================

#region Usings

using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr.CitrixIntegration;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	public class StandAloneTestAppProxy : CitrixApplicationHostedControl
	{
		public StandAloneTestAppProxy(int appID, string appName, string initString): base(appID, appName, initString)
		{
#if DEBUG
			Logging.Debug = true;
#endif
			AgentCredentialRequest += delegate
			{
				// this event is raised when it is time to supply credentials to the ICAClient
				// the following sample code can be used to integrate SSO

				//AgentCredentials2.AgentCredentials client = new AgentCredentialsClient();
				//
				//string username = client.GetCredentials(ApplicationName)[0];
				//string password = client.GetCredentials(ApplicationName)[1];
				//string domain = GetCitrixLoginDomain();
				//
				//ICAClient.SetProp("Username", username);
				//ICAClient.SetProp("Password", password);
				//ICAClient.SetProp("Domain", domain);

				Logging.Trace("StandAloneTestAppProxy: AgentCredentialRequest event handled");
			};

			FirstHeartbeatReceived += delegate
			{
				// this event is raised when communication is up and running--which may be well after
				// the application UI has been parented
				// it can be leveraged to do things like re-submit context changes that
				// could not be processed because the channels were not ready at the time the events occurred.
				Logging.Trace("StandAloneTestAppProxy: FirstHeartbeatReceived event handled");
			};
		}

		protected override string ICAFileName
		{
			get { return @"X:\CCFCITRIX\StandAloneTestAppStub.ica"; }
		}
	}
}
