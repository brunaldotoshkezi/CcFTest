//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// Routes the appropriate applications to their correct signer implementation.
//
//===============================================================================

using System;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.Signer.Providers;

namespace Microsoft.Ccf.Samples.Csr.Signer
{
	/// <summary>
	/// This class acts like a router to route the appropriate applications to the 
	/// correct signer (e.g. WinSigner or WebSigner)
	/// </summary>
	public class ApplicationSigner : SignerProvider
	{
		/// <summary>
		/// The empty constructor exists for serialization purposes.
		/// </summary>
		public ApplicationSigner()
		{}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="initializer"></param>
		public ApplicationSigner(string initializer) : base(initializer)
		{}

		/// <summary>
		/// This method will route the login operations to the appropriate signer (e.g. WinSigner, WebSigner, etc.)
		/// depending on the application.
		/// </summary>
		/// <param name="application"></param>
		public override void DoLogin(object application)
		{
			string applicationName = ((IHostedApplication)application).ApplicationName;

			if(applicationName == "SSOLoginWinApp")
			{
				// Create instance of ExternalAppSigner class and invoke its DoLogin Method to
				// implement auto sign-on for external application.
				ExternalAppSigner externalAppLogin = new ExternalAppSigner();
				externalAppLogin.DoLogin(application);
			}
			else if ((applicationName == "SSOLoginWinControl") || (applicationName == "Multi Channel Login"))
			{
				HostedAppSigner HostedAppLogin = new HostedAppSigner();
				HostedAppLogin.DoLogin(application);
			}
			else if (applicationName == "SSODemoLoginWebApp" || applicationName == "Knowledgebase")
			{
				// Create an instance of WebAppSigner class and invoke its DoLogin method to
				// implement auto sign-on for a web application
				WebAppSigner webAppLogin = new WebAppSigner();
				webAppLogin.DoLogin(application);
			}
		}
	}
}
