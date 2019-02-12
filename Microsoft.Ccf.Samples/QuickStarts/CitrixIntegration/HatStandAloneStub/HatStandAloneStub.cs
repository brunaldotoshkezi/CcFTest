//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2006 Microsoft Corp.
//
//===================================================================================

#region Usings

using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.Application;
using Microsoft.Ccf.Csr.CitrixIntegration;
using Microsoft.Ccf.HostedApplicationToolkit.DataDrivenAdapter;

#endregion

namespace Microsoft.Ccf.Samples.Citrix
{
	class HatStandAloneStub : CitrixExternalApplicationStub
	{
		private string _AppInitString;
		private DataDrivenAdapterBase _Dda;

		HatStandAloneStub(int appID, string appName, string initString)
			: base(appID, appName, initString)
		{
			_AppInitString = initString;
		}

		public static Form CreateInstance()
		{
			ApplicationRecord appRec = new ApplicationRecord();
			appRec.ApplicationID = 7;
			appRec.Name = "HatStandaloneTestApp";
			appRec.Type = 2;
			appRec.Initialization = @"
<initstring>
  <interopAssembly>
    <URL>C:\CCFCITRIX\Microsoft.Ccf.Samples.StandAloneTestApp.exe</URL>
    <Arguments/>
    <WorkingDirectory>C:\CCFCITRIX</WorkingDirectory>
    <hostInside />
  </interopAssembly>
  <DataDrivenAdapterBindings>
    <Type>Microsoft.Ccf.HostedApplicationToolkit.DataDrivenAdapter.WinDataDrivenAdapter, Microsoft.Ccf.HostedApplicationToolkit.DataDrivenAdapter</Type>
    <Controls>
     <AccControl name=""textbox_acc"">
      <Path>
       <Next match=""2"">Test:</Next>
       <Next>Test:</Next>
      </Path>
     </AccControl>
     <AccControl name=""button_acc"">
      <Path>
       <Next>Ok</Next>
       <Next>Ok</Next>
      </Path>
     </AccControl>
     <AccControl name=""checkbox_acc"">
      <Path>
       <Next>Test Checkbox</Next>
       <Next>Test Checkbox</Next>
      </Path>
     </AccControl>
     <AccControl name=""radio1_acc"">
      <Path>
       <Next>Radio1</Next>
       <Next>Radio1</Next>
      </Path>
     </AccControl>
     <AccControl name=""radio2_acc"">
      <Path>
       <Next>Radio2</Next>
       <Next>Radio2</Next>
      </Path>
     </AccControl>
     <AccControl name=""radio3_acc"">
      <Path>
       <Next>Radio3</Next>
       <Next>Radio3</Next>
      </Path>
     </AccControl>
     <AccControl name=""tab1_acc"">
      <Path>
       <Next offset=""-1"">Simulate Crash</Next>
       <Next offset=""1"">Application</Next>
       <Next>Tab Page 1</Next>
      </Path>
     </AccControl>
     <AccControl name=""tab2_acc"">
      <Path>
       <Next offset=""-1"">Simulate Crash</Next>
       <Next offset=""1"">Application</Next>
       <Next>Tab Page 2</Next>
      </Path>
     </AccControl>
     <AccControl name=""crashbutton_acc"">
      <Path>
       <Next>Simulate Crash</Next>
       <Next>Simulate Crash</Next>
      </Path>
     </AccControl>
    </Controls>
  </DataDrivenAdapterBindings>
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

			appRec.Actions = list;

			HatStandAloneStub stub = new HatStandAloneStub(appRec.ApplicationID, appRec.Name, appRec.Initialization);
			for (int i = 0; i < appRec.Actions.Count; i++)
			{
				stub.AddAction(appRec.Actions[i].ActionID, appRec.Actions[i].Name, appRec.Actions[i].Initialization);
			}

			HostingForm hostingForm = new HostingForm();
			hostingForm.Name = "HatStandAloneTestAppStub";
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

		public override void Initialize()
		{
			base.Initialize();

			// this creates an instance of the Dda as specified in the initstring above
			_Dda = DataDrivenAdapterBase.CreateInstance(_AppInitString, externalControl.Hwnd);
			_Dda.ControlChanged += new EventHandler<ControlChangedEventArgs>(_Dda_ControlChanged);

			// these are the Dda "implicit actions"--let Aif know so it will let them through
			int actionId = 2;
			string actionInit = "<ActionInit/>";
			AddAction(actionId++, AutomationImplicitAction.FindControl, actionInit);
			AddAction(actionId++, AutomationImplicitAction.GetControlValue, actionInit);
			AddAction(actionId++, AutomationImplicitAction.SetControlValue, actionInit);
			AddAction(actionId++, AutomationImplicitAction.ExecuteControlAction, actionInit);
			AddAction(actionId++, AutomationImplicitAction.AddDoActionEventTrigger, actionInit);
			AddAction(actionId, AutomationImplicitAction.RemoveDoActionEventTrigger, actionInit);
		}

		protected override void HandleIncomingChannelData(ChannelData channelData)
		{
			// when true, this means the channelData is an incoming request from the proxy side (there)
			if (channelData.ResultMessage == null)
			{
				string controlName = string.Empty;
				string controlValue = string.Empty;
				switch (channelData.ActionName)
				{
					case AutomationImplicitAction.FindControl:
						GetDdaParameters(channelData.ActionData, ref controlName, ref controlValue);
						try
						{
							channelData.ActionReturnValue = _Dda.FindControl(controlName).ToString();
							ReturnOK(channelData);
						}
						catch (DataDrivenAdapterException ex)
						{
							ReturnException(channelData, ex);
						}
						break;
					case AutomationImplicitAction.GetControlValue:
						GetDdaParameters(channelData.ActionData, ref controlName, ref controlValue);
						try
						{
							channelData.ActionReturnValue = _Dda.GetControlValue(controlName);
							ReturnOK(channelData);
						}
						catch (DataDrivenAdapterException ex)
						{
							ReturnException(channelData, ex);
						}
						break;
					case AutomationImplicitAction.SetControlValue:
						GetDdaParameters(channelData.ActionData, ref controlName, ref controlValue);
						try
						{
							_Dda.SetControlValue(controlName, controlValue);
							channelData.ActionReturnValue = string.Empty;
							ReturnOK(channelData);
						}
						catch (DataDrivenAdapterException ex)
						{
							ReturnException(channelData, ex);
						}
						break;
					case AutomationImplicitAction.ExecuteControlAction:
						GetDdaParameters(channelData.ActionData, ref controlName, ref controlValue);
						try
						{
							_Dda.ExecuteControlAction(controlName);
							channelData.ActionReturnValue = string.Empty;
							ReturnOK(channelData);
						}
						catch (DataDrivenAdapterException ex)
						{
							ReturnException(channelData, ex);
						}
						break;
					case AutomationImplicitAction.AddDoActionEventTrigger:
					case AutomationImplicitAction.RemoveDoActionEventTrigger:
						// ignore these requests as these cannot be handled by the AutomationAdapter in this usage context
						// actions can still be triggered on the stub side in response to _Dda.ControlChanged events
						ReturnOK(channelData);
						break;
					default:
						base.HandleIncomingChannelData(channelData);
						break;
				}
			}
			// otherwise, this is a response to a request originated from the stub side (here)
			else
			{
				base.HandleIncomingChannelData(channelData);
			}
		}

		private void _Dda_ControlChanged(object sender, ControlChangedEventArgs e)
		{
			// here is how actions can be triggered in response to Dda ControlChanged events
			// in this case, a ButtonPressed WinDDA event occurring on a control named "button_acc" will fire the MsgHelloWorld action
			if (e.EventTypeName.Equals("ButtonPressed", StringComparison.Ordinal)
				&& e.ControlName.Equals("button_acc", StringComparison.Ordinal))
			{
				FireRequestAction(new RequestActionEventArgs("StandAloneTestApp", "MsgHelloWorld", string.Empty));
			}

			Logging.Trace(ApplicationName, "_Dda_ControlChanged: " + e);
		}

		private void GetDdaParameters(string data, ref string controlName, ref string controlValue)
		{
			XmlDocument xmlDoc = new XmlDocument();
			try
			{
				xmlDoc.LoadXml(data);
			}
			catch (XmlException)
			{
				Logging.Error(ApplicationName, "Internal error: Data not well XML-formed (looking for <DdaParameters/>");
			}
			controlName = GetParameterValue(xmlDoc, "//DdaParameters/ControlName");
			controlValue = GetParameterValue(xmlDoc, "//DdaParameters/ControlValue");
		}

		private static string GetParameterValue(XmlDocument xmlDoc, string xpath)
		{
			XmlNode n = (xmlDoc != null) ? xmlDoc.DocumentElement.SelectSingleNode(xpath) : null;
			return (n != null) ? n.InnerText : string.Empty;
		}
	}
}
