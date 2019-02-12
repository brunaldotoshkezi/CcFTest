//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2006 Microsoft Corp.
//
//===================================================================================

#region Usings

using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Ccf.Csr.Application;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.CitrixIntegration;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	class HelloWorldStub : CitrixHostedControlStub
	{
		HelloWorldStub(int appID, string appName, string initString)
			: base(appID, appName, initString)
		{
		}

		public static Form CreateInstance()
		{
			ApplicationRecord appRec = new ApplicationRecord();
			appRec.ApplicationID = 5;
			appRec.Name = "WinForm Hello World";
			appRec.Type = 0;
			appRec.Initialization = @"
<initstring>
  <assemblyInfo>
    <URL>Microsoft.Ccf.Samples.Citrix.WinFormHelloWorld.dll</URL>
    <type>Microsoft.Ccf.Samples.Citrix.WinFormHelloWorld</type>
  </assemblyInfo>
  <displayGroup>MainPanel</displayGroup>
  <optimumSize x=""470"" y=""380"" />
  <minimumSize x=""340"" y=""180"" />
</initstring>";
			appRec.EnableAutoSignOn = false;
			appRec.LoginFields = null;

			BindingList<ActionRecord> list = new BindingList<ActionRecord>();

			ActionRecord ar1 = new ActionRecord();
			ar1.ActionID = 1;
			ar1.Name = "Default";
			ar1.Initialization = @"<ActionInit/>";
			list.Add(ar1);

			appRec.Actions = list;

#pragma warning disable 0618
			IHostedApplication stub = HostedAppFactory.CreateApplication(appRec);
#pragma warning restore 0618

			HostingForm hostingForm = new HostingForm();
			hostingForm.Name = "StandAloneTestAppStub";
			hostingForm.Text = stub.ApplicationName;
			hostingForm.ControlBox = false;
			hostingForm.MaximizeBox = false;
			hostingForm.MinimizeBox = false;
			hostingForm.ShowInTaskbar = false;
			hostingForm.FormBorderStyle = FormBorderStyle.None;
			hostingForm.StartPosition = FormStartPosition.Manual;
			hostingForm.ClientSize = stub.OptimumSize;
			hostingForm.MinimumSize = stub.MinimumSize;

			hostingForm.Closed += delegate { stub.Close(); };

			stub.TopLevelWindow.Parent = hostingForm;
			stub.TopLevelWindow.Dock = DockStyle.Fill;

			stub.Initialize();

			return hostingForm;
		}
	}
}
