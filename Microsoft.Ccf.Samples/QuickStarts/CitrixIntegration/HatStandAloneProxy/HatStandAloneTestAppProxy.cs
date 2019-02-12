//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2006 Microsoft Corp.
//
//===================================================================================

#region Usings

using Microsoft.Ccf.Common;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.CitrixIntegration;

#endregion

// ** WARNING: do not use Application Isolation as it is incompatible with this sample **

namespace Microsoft.Ccf.Samples.Citrix
{
	public class HatStandAloneTestAppProxy : CitrixApplicationHostedControl
	{
		private AutomationAdapter _AutomationAdapter;
		private string _AppInitString;
		private bool _IsInitializationComplete;

		public HatStandAloneTestAppProxy(int appID, string appName, string initString): base(appID, appName, initString)
		{
			_AppInitString = initString;
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

				Logging.Trace("HatStandAloneTestAppProxy: AgentCredentialRequest event handled");
			};

			FirstHeartbeatReceived += delegate
			{
				// this event is raised when communication is up and running--which may be well after
				// the application UI has been parented
				// it can be leveraged to do things like re-submit context changes that
				// could not be processed because the channels were not ready at the time the events occurred.

				_IsInitializationComplete = true;
				Logging.Trace("HatStandAloneTestAppProxy: Citrix communication channels are now considered ready");
			};
		}

		protected override string ICAFileName
		{
			get { return @"X:\CCFCITRIX\HatStandAloneTestAppStub.ica"; }
		}

		public override void Initialize()
		{
			_IsInitializationComplete = false;

			base.Initialize();

			_AutomationAdapter = new AutomationAdapter(this, AppHostWorkItem);
			_AutomationAdapter.Name = Name;
			_AutomationAdapter.ApplicationInitString = _AppInitString;
			_AutomationAdapter.Initialize();

			// these are the DDA "implicit actions"--let Aif know so it will let them through
			int actionId = 2;
			string emptyActionInit = "<ActionInit/>";
			AddAction(actionId++, AutomationImplicitAction.FindControl, emptyActionInit);
			AddAction(actionId++, AutomationImplicitAction.GetControlValue, emptyActionInit);
			AddAction(actionId++, AutomationImplicitAction.SetControlValue, emptyActionInit);
			AddAction(actionId++, AutomationImplicitAction.ExecuteControlAction, emptyActionInit);
			AddAction(actionId++, AutomationImplicitAction.AddDoActionEventTrigger, emptyActionInit);
			AddAction(actionId++, AutomationImplicitAction.RemoveDoActionEventTrigger, emptyActionInit);

			// this is for the action implementation below
			AddAction(actionId, "MsgHelloWorld", emptyActionInit);
		}

		protected override void DoAction(RequestActionEventArgs raArgs)
		{
			if (!_IsInitializationComplete)
			{
				Logging.Information(ApplicationName, "Incoming DoAction(" + raArgs.Action + ") while Citrix communication channels are not ready");
			}

			switch (raArgs.Action)
			{
				case AutomationImplicitAction.FindControl:
					if (_IsInitializationComplete)
					{
						base.DoAction(raArgs);
					}
					else
					{
						// the Citrix communication channels are not yet ready, so report false
						raArgs.ActionReturnValue = bool.FalseString;
					}
					break;
				case AutomationImplicitAction.GetControlValue:
				case AutomationImplicitAction.SetControlValue:
				case AutomationImplicitAction.ExecuteControlAction:
				case AutomationImplicitAction.AddDoActionEventTrigger:
				case AutomationImplicitAction.RemoveDoActionEventTrigger:
					if (_IsInitializationComplete)
					{
						base.DoAction(raArgs);
					}
					else
					{
						// the Citrix communication channels are not yet ready, so dont bother transmitting the action to the stub
						raArgs.ActionReturnValue = string.Empty;
					}
					break;
				// actions implemented via Automations on CitrixApplicationHostedControls are implemented in code here
				case "MsgHelloWorld":
					if (_IsInitializationComplete)
					{
						Action action = new Action(0, raArgs.Action, @"<ActionInit><Automation>
<Type>Microsoft.Ccf.QuickStarts.QsAutomationProject.MsgHelloWorld,file://C:\CCF\Public\Microsoft.Ccf.Samples.Csr.AgentDesktop\Microsoft.Ccf.Samples\QuickStarts\QsAutomationProject\bin\Debug\Microsoft.Ccf.QuickStarts.QsAutomationProject.dll</Type>
</Automation></ActionInit>");
						_AutomationAdapter.DoAction(action, raArgs);
					}
					break;
			}
		}

		public override void NotifyContextChange(Context context)
		{
			// here, ContextChange is only transmitted when the Citrix communication channels are ready
			// in a real implementation, the context could be cached and re-sent 
			if (_IsInitializationComplete)
			{
				base.NotifyContextChange(context);
			}
		}

		public override void Close()
		{
			if (_AutomationAdapter != null)
			{
				_AutomationAdapter.Close();
				_AutomationAdapter = null;
			}
		}
	}
}
