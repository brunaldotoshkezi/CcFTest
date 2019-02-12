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
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.Application;
using Microsoft.Ccf.Csr.CitrixIntegration;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	class StandAloneStub : CitrixExternalApplicationStub
	{
		StandAloneStub(int appID, string appName, string initString)
			: base(appID, appName, initString)
		{
		}

		public static Form CreateInstance()
		{
			ApplicationRecord appRec = new ApplicationRecord();
			appRec.ApplicationID = 7;
			appRec.Name = "StandaloneTestApp";
			appRec.Type = 2;
			appRec.Initialization = @"
<initstring>
    <interopAssembly>
        <URL>C:\CCFCITRIX\Microsoft.Ccf.Samples.StandAloneTestApp.exe</URL>
        <Arguments/>
        <WorkingDirectory>C:\CCFCITRIX</WorkingDirectory>
        <hostInside />
    </interopAssembly>
    <adapter>
        <URL>C:\CCFCITRIX\Microsoft.Ccf.Samples.Citrix.ApplicationAdapter.dll</URL>
        <type>Microsoft.Ccf.Samples.Citrix.AppAdapter</type>
    </adapter>
    <displayGroup>None</displayGroup>
    <optimumSize x=""800"" y=""600"" />
    <minimumSize x=""640"" y=""480"" />
</initstring>";
			appRec.EnableAutoSignOn = false;
			appRec.LoginFields = null;

			BindingList<ActionRecord> list = new BindingList<ActionRecord>();

			ActionRecord ar1 = new ActionRecord();
			ar1.ActionID = 1;
			ar1.Name = "Default";
			ar1.Initialization = @"<ActionInit/>";
			list.Add(ar1);

			ActionRecord ar2 = new ActionRecord();
			ar2.ActionID = 2;
			ar2.Name = "PushButton";
			ar2.Initialization = @"
<ActionInit><Steps>
	<GetControlByText>Tab Page 2</GetControlByText>
	<Push/>
	<GetControlByText>Test Checkbox</GetControlByText>
	<Push/>
	<GetControlByText>Radio1</GetControlByText>
	<SetCheck>1</SetCheck>
	<GetControlByPosition x=""88"" y=""48""/>
	<GetText>StandaloneText</GetText>
	<SetText>StandaloneText</SetText>
	<GetControlByText>&amp;Ok</GetControlByText>
	<GetText>ButtonName</GetText>
	<Push/>
</Steps></ActionInit>";
			list.Add(ar2);

			appRec.Actions = list;

			StandAloneStub stub = new StandAloneStub(appRec.ApplicationID, appRec.Name, appRec.Initialization);
			for (int i = 0; i < appRec.Actions.Count; i++)
			{
				stub.AddAction(appRec.Actions[i].ActionID, appRec.Actions[i].Name, appRec.Actions[i].Initialization);
			}

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
