//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// SampleAgent.cs
//
// This file contains the definition for an the agent who works on the Sample Provider.
//
//===============================================================================
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Microsoft.Ccf.Multichannel.AdapterManager.Common;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.MultiChannelAdapters.Common;

namespace Microsoft.Ccf.Multichannel.Adapter.Sample
{
	class SampleAgent
	{
		# region Variables
		public SampleProviderConnection mConnection;

		///  Agent properties.
		public string mAgentId;
		public string mAgentPassword;

		#endregion

		public Object Login()
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login
			
			return retValue;
		}

		public Object Logout()
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}

		public Object LogoutFromAllDNs()
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;		
		}

		public Object MakePhoneCall(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;		
		}

		public Object AnswerCall(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;		
		}

		public Object ReleaseCall(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;		
		}

		public Object MarkDoneCall(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;		
		}

		public Object HoldCall(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;		
		}

		public Object UnHoldCall(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}

		public Object MuteOn(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}

		public Object MuteOff(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}

		public Object TransferCall(Object attributes)
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}

		public Object Ready()
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}

		public Object NotReady()
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}

		public Object AfterCallWork()
		{
			Object retValue = null;
			//TODO Try to modify retValue according to the provider to get back the details
			//for trying to login

			return retValue;
		}
		public void RegisterEvent()
		{
		}

		public void UnRegisterEvent()
		{
		}

		public string GetTelephonyServerStatus()
		{
			return "On";
		}
		/// <summary>
		/// Handling events coming from provider
		/// </summary>
		/// <param name="evt"></param>
		/// <returns></returns>
		public MultichannelMessageInformation HandleEvent(Object evt)
		{
			MultichannelMessageInformation returnObject = new MultichannelMessageInformation();
			//TODO make proper switch case so that the event can go into properr categories
			//Here we define categories as Mail, Chat pr Phone
			//I am assuming a phone call and making the MMI objecta ccording to that
			returnObject.Action = Constants.Actions.NewIncomingCall;
			returnObject.Status = true;
			returnObject.Message = MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
				+ "Test"
				+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage)
				, Constants.Actions.NewIncomingCall, true);
			returnObject.AgentApplicationID = "VOICE";

			return returnObject;
		}

		#region utility functions
		public string MakeMessageString(string inputXMLString, string action, bool success)
		{
			StringBuilder outputXMLString = new StringBuilder(Constants.MakeStartTag(Constants.NodeNames.MessageNode));
			outputXMLString.Append(inputXMLString);
			outputXMLString.Append(Constants.MakeStartTag(Constants.NodeNames.StatusNode)
				+ GetAgentStatus(action, success)
				+ Constants.MakeEndTag(Constants.NodeNames.StatusNode));
			outputXMLString.Append(Constants.MakeStartTag(Constants.NodeNames.PossibleActionsNodes)
				+ GetVoiceActions(action, success)
				+ Constants.MakeEndTag(Constants.NodeNames.PossibleActionsNodes));
			outputXMLString.Append(Constants.MakeEndTag(Constants.NodeNames.MessageNode));
			return outputXMLString.ToString();
		}

		public string GetAgentStatus(string action, bool success)
		{
			string retValue;
			if (success)
			{
				switch (action.ToUpper())
				{
					case Constants.Actions.Login:
						retValue = Constants.Status.ReadyStatus;
						break;
					case Constants.Actions.ReleaseCall:
						retValue = Constants.Status.ReadyStatus;
						break;
					case Constants.Actions.TransferCall:
						retValue = Constants.Status.ReadyStatus;
						break;
					case Constants.Actions.MarkDone:
						retValue = Constants.Status.ReadyStatus;
						break;
					case Constants.Actions.MakeReady:
						retValue = Constants.Status.ReadyStatus;
						break;
					case Constants.Actions.LogOut:
						retValue = Constants.Status.LoggedOutStatus;
						break;
					case Constants.Actions.NewIncomingCall:
						retValue = Constants.Status.ReadyStatus;
						break;
					default:
						retValue = Constants.Status.BusyStatus;
						break;
				}
			}
			else
			{
				switch (action.ToUpper())
				{
					case Constants.Actions.Login:
						retValue = Constants.Status.LoggedOutStatus;
						break;
					case Constants.Actions.ReleaseCall:
						retValue = Constants.Status.BusyStatus;
						break;
					case Constants.Actions.TransferCall:
						retValue = Constants.Status.BusyStatus;
						break;
					case Constants.Actions.MarkDone:
						retValue = Constants.Status.BusyStatus;
						break;
					//DOESN'T make sense but have to do
					case Constants.Actions.MakeReady:
						retValue = Constants.Status.BusyStatus;
						break;
					default:
						retValue = Constants.Status.ReadyStatus;
						break;
				}
			}
			Logging.Trace("Sample Agent", "Agent Status : " + retValue);
			return retValue;
		}

		public string GetVoiceActions(string action, bool success)
		{
			string retVal;
			if (success)
			{
				switch (action.ToUpper())
				{
					case "LOGIN":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "MAKECALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "ANSWERCALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "RELEASECALL":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "NEWINCOMINGCALL":
						retVal = "ANSWERCALL,LOGOUT";
						break;
					case "HOLDCALL":
						retVal = "UNHOLDCALL,LOGOUT";
						break;
					case "UNHOLDCALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "MUTECALL":
						retVal = "UNMUTECALL,LOGOUT";
						break;
					case "UNMUTECALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "TRANSFERCALL":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "LOGOUT":
						retVal = "LOGIN";
						break;
					case "MARKDONE":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "MAKEREADY":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "MAKENOTREADY":
						retVal = "MAKEREADY,LOGOUT";
						break;
					case "CALLCONNECTED":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					default:
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
				}
			}
			else
			{
				switch (action.ToUpper())
				{
					case "LOGIN":
						retVal = "LOGIN";
						break;
					case "MAKECALL":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "ANSWERCALL":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "RELEASECALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "HOLDCALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "UNHOLDCALL":
						retVal = "UNHOLDCALL,LOGOUT";
						break;
					case "MUTECALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "UNMUTECALL":
						retVal = "UNMUTECALL,LOGOUT";
						break;
					case "TRANSFERCALL":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "LOGOUT":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					case "MARKDONE":
						retVal = "HOLDCALL,MUTECALL,RELEASECALL,LOGOUT";
						break;
					case "MAKEREADY":
						retVal = "MAKEREADY,LOGOUT";
						break;
					case "MAKENOTREADY":
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
					default:
						retVal = "MAKECALL,MAKENOTREADY,LOGOUT";
						break;
				}
			}
			Logging.Trace("sample Agent", "Agent Actions : " + retVal);
			return retVal;
		}
		#endregion
	}
}
