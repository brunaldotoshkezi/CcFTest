//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// SampleAdapter.cs
//
// This file contains the definition for Adapter which talks with MCh of CCF as well as Sample.
//
//===============================================================================
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Threading;
using System.Xml;
using Microsoft.Ccf.Multichannel.AdapterManager;
using Microsoft.Ccf.Multichannel.AdapterManager.Common;

using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Common;

using Microsoft.Ccf.MultiChannelAdapters.Common;

namespace Microsoft.Ccf.Multichannel.Adapter.Sample
{
	class SampleAdapter : AdapterAbstractImplementation
	{
		public SampleAgent mAgent = null;
		//MultichannelMessageInformation ciRequestinfo = null;

		public SampleAdapter(object obj)
			: base(obj)
		{
		}

		/// <summary>
		/// Inherited function.
		/// </summary>
		/// <param name="ci"></param>
		/// <returns></returns>
		//public override MultichannelMessageInformation ProcessMessageFromAgent(MultichannelMessageInformation ci)
        public override MultichannelMessageInformation ProcessMessageFromAgent(MultichannelMessageInformation mmInMessage)
		{
			Logging.Trace("Sample Adapter", "Inside Process from Agent");
            switch (mmInMessage.Action.ToUpper())
			{
				case Constants.Actions.Login:
                    mmInMessage = LoginIntoProvider(mmInMessage);
					break;
				case Constants.Actions.MakeCall:
                    mmInMessage = MakeCallFromAdapter(mmInMessage);
					break;
				case Constants.Actions.AnswerCall:
                    mmInMessage = AnswerCallFromAdapter(mmInMessage);
					break;
				case Constants.Actions.ReleaseCall:
                    mmInMessage = ReleaseCallFromAdapter(mmInMessage);
					break;
				case Constants.Actions.HoldCall:
                    mmInMessage = HoldCallFromAdapter(mmInMessage);
					break;
				case Constants.Actions.UnHoldCall:
                    mmInMessage = UnHoldCallFromAdapter(mmInMessage);
					break;
				case Constants.Actions.MuteCall:
                    mmInMessage = MuteOn(mmInMessage);
					break;
				case Constants.Actions.UnmuteCall:
                    mmInMessage = MuteOff(mmInMessage);
					break;
				case Constants.Actions.TransferCall:
                    mmInMessage = TransferCallFromAdapter(mmInMessage);
					break;
				case Constants.Actions.LogOut:
                    mmInMessage = LogoutFromProvider(null, null, null);
					break;
				case Constants.Actions.MarkDone:
                    mmInMessage = MarkDoneFromAdapter(mmInMessage);
					break;
				case Constants.Actions.MakeReady:
                    mmInMessage = MakeAgentReadyFromAdapter(mmInMessage);
					break;
				case Constants.Actions.MakeNotReady:
                    mmInMessage = MakeAgentNotReadyFromAdapter(mmInMessage);
					break;
				case Constants.Actions.AgentStatus:
                    mmInMessage = GetAgentStatusFromAdapter(mmInMessage);
					break;
				case Constants.Actions.KillAllLoggedInSessionsForAgent:
                    mmInMessage = KillAllLoggedInSessionsForAgent(null, null, null);
					break;
				case Constants.Actions.SetAgentStatus:
                    mmInMessage = SetAgentStatus(mmInMessage);
					break;
				default:
                    mmInMessage.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						 + "Unknown Action : "
                         + mmInMessage.Action.ToUpper() 
						 + Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
                         mmInMessage.Action.ToUpper(),
						 true);
					break;
			}

			//this.SendMessage(ci);
			Logging.Trace("Sample Adapter","Exiting Process from Agent");
            return mmInMessage;

		}

        
		/// <summary>
		/// Inherited function.
		/// </summary>
        /// <param name="mmInMessage"></param>
		/// <returns></returns>
        public override MultichannelMessageInformation LoginIntoProvider(MultichannelMessageInformation mmInMessage)
        {
			try
			{
				Logging.Trace("Sample Adapter : Inside Sample adapter login");
				/// Creating Agent and Connection objects for this status form
				mAgent = new SampleAgent();
				AgentInfo agentInfo = this.GetAgentInfo();
				string providerURI = agentInfo.Uri;
				string providerConnectionString = agentInfo.ConnectionString;
				string providerConfigurationString = agentInfo.ConfigurationString;
                string message = mmInMessage.Message;

				XmlDocument doc = new XmlDocument();
				try
				{
					doc.LoadXml(message);
				}
				catch (Exception ex)
				{
                    return ParseXMLExceptionInSampleAdapter(mmInMessage, ex, Constants.Actions.Login);
				}

				mAgent.mAgentId = GetNodeValue(doc, Constants.NodeNames.Agent);
				mAgent.mAgentPassword = GetNodeValue(doc, Constants.NodeNames.Password);

				Logging.Trace("Sample adapter", "trying to connect to Sample with: 1) URI: " + providerURI + "  login " + mAgent.mAgentId + " password " + mAgent.mAgentPassword);
				mAgent.mConnection = new SampleProviderConnection(mAgent.mAgentId, mAgent.mAgentPassword, providerURI, providerConnectionString, providerConfigurationString);
				Logging.Trace("Sample adapter", "Successfully connected to the sample provider");
				
				StringBuilder msgToSend = new StringBuilder("");
				Object errorObject = mAgent.Login();

				if (errorObject != null)
				{
                    mmInMessage.Status = false;
					msgToSend.Append(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Login: : "
						+ errorObject.ToString()
						+ " ERROR: "
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage) + "\n");
                    mmInMessage.Message = mAgent.MakeMessageString(msgToSend.ToString(), Constants.Actions.Login, false);
					Logging.Trace("Sample Adapter", "Going out from login unSuccessfully");
				}
				else
				{
                    mmInMessage.Status = true;
					msgToSend.Append(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Login: "
						+ " SUCCESSFUl"
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage) + "\n");
                    mmInMessage.Message = mAgent.MakeMessageString(msgToSend.ToString(), Constants.Actions.Login, true);
					Logging.Trace("Sample Adapter", "Going out from login successfully");
				}

                mmInMessage.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
                return mmInMessage;
			}
			catch (Exception ex)
			{
                return ParseAllExceptionInSampleAdapter(mmInMessage, ex, Constants.Actions.Login);
			}
		}


        /// <summary>
        ///  This function is called when the Listner/adapter is being shutdown by the Mulitchannel server 
        /// </summary>
        public override void ShutdownListener()
        {
            base.ShutdownListener();
        }
		
		/// <summary>
		/// Inherited function.
		/// </summary>
		/// <param name="ci"></param>
		/// <returns></returns>
		public override void Initialize(string name, NameValueCollection attributes)
		{
		}

		/// <summary>
		/// Inherited function.
		/// </summary>
		/// <param name="ci"></param>
		/// <returns></returns>
		public override MultichannelMessageInformation LogoutFromProvider(string agent, string guid, string provider)
		{
			return DoLogout(false, agent, guid, provider);
		}
		/// <summary>
		/// Inherited function.
		/// </summary>
		/// <param name="ci"></param>
		/// <returns></returns>
		private MultichannelMessageInformation DoLogout(bool logoutFromAllDNs, string agent, string guid, string provider)
		{
			Logging.Trace("Sample Adapter", "Entering Logoutfromprovider");
			MultichannelMessageInformation logoutObj = new MultichannelMessageInformation();
			try
			{
				StringBuilder msgToSend = new StringBuilder("");
				Object logoutError = null;
				if (logoutFromAllDNs)
				{
					logoutError = mAgent.LogoutFromAllDNs();
				}
				else
				{
					logoutError = mAgent.Logout();
				}

				if (logoutError != null)
				{
					logoutObj.Status = false;
					msgToSend.Append(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Logout"
						+ logoutError.ToString()
						+ " ERROR: "
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage) + "\n");
					logoutObj.Message = mAgent.MakeMessageString(msgToSend.ToString(), Constants.Actions.LogOut, false);
					Logging.Trace("Sample Adapter", "Logout failed ");
				}
				else
				{
					logoutObj.Status = true;
					msgToSend.Append(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Logout Successfull"
						+ Constants.MakeEndTag(Constants.NodeNames .ReturnMessage) + "\n");
					logoutObj.Message = mAgent.MakeMessageString(msgToSend.ToString(), Constants.Actions.LogOut, true);
					Logging.Trace("Sample Adapter", "Logout Successful ");
				}

				logoutObj.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				logoutObj.Action = Constants.Actions.LogOut;
				//Microsoft.Ccf.Multichannel.AdapterManager.InteractionType.
				//It will return -1 if there is no application correponding to this type or else 
				//will return the corresponding application ID
				logoutObj.AgentApplicationID = AdapterUtility.GetApplicationID(this.GetAgentInfo().Provider,
												Microsoft.Ccf.Multichannel.AdapterManager.InteractionType.LOGIN).ToString();
				Logging.Trace("Sample Adapter", "Logout App ID : " + logoutObj.AgentApplicationID);
				//Cleaning at server side
				Logging.Trace("Cleaning at provider");
				CleanAdapter();
				return logoutObj;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(logoutObj, ex, Constants.Actions.LogOut);
			}
		}

		/// <summary>
        /// This is an example of handeling the KillAllLoggedInSessionsForAgent Action.
        /// This is for dealing with a crash situation where you need to clean up the Channel provider. 
		/// </summary>
        /// <param name="agent">Agent ID from the IAD</param>
        /// <param name="guid">IAD Session ID</param>
        /// <param name="provider">Name of the Provider</param>
		/// <returns></returns>

		public MultichannelMessageInformation KillAllLoggedInSessionsForAgent(string agent, string guid, string provider)
		{
			Logging.Trace("Sample Adapter", "Closing all previous LoggedIn sessions for the agent:" + mAgent.mAgentId);
			MultichannelMessageInformation responseInfo = new MultichannelMessageInformation();
			try
			{
				Logging.Trace("sample Adapter", "logout agent from all loggedIn DNs...started");
				responseInfo = DoLogout(true, agent, guid, provider);
				if (responseInfo.Status)
				{
					Logging.Trace("Sample Adapter", "logout agent from all loggedIn DNs...successful");
				}
				else
				{
					Logging.Trace("Sample Adapter", "logout agent from all loggedIn DNs...failed");
					Logging.Trace("Error: " + responseInfo.Message);
				}
				return responseInfo;

			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(responseInfo, ex, Constants.Actions.KillAllLoggedInSessionsForAgent);
			}

		}
		
		private void CleanAdapter()
		{
			mAgent.UnRegisterEvent();
			mAgent.mConnection.Disconnect();
		}

		private string GetNodeValue(XmlDocument table, string key)
		{
			XmlNode node = table.DocumentElement.SelectSingleNode(key);
			if (node != null)
			{
				return node.InnerText;
			}
			return null;
		}

		/// <summary>
		/// It makes call from the adapter
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation MakeCallFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Sample Adapter", "inside make call from adapter");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				ParseXMLExceptionInSampleAdapter(input, ex,Constants.Actions.MakeCall);
			}
			try
			{
				string location = GetNodeValue(doc, Constants.NodeNames.Location);
				string destination = GetNodeValue(doc, Constants.NodeNames.DestNumber);
				Object voiceError = mAgent.MakePhoneCall(null);

				StringBuilder returnMessage = new StringBuilder("");
				//TODO hardcoding the callID for the moment
				string callID = "1234";
				returnMessage.Append(Constants.MakeStartTag(Constants.NodeNames.CallID) 
					+ callID + Constants.MakeEndTag(Constants.NodeNames.CallID));

				if (voiceError != null)
				{
					string errorMsg = "NewPhoneCall: ERROR (" 
						+ voiceError.ToString() 
						+ ") - " ;
					returnMessage.Append(Constants.MakeStartTag(Constants.NodeNames .ReturnMessage)
						+ errorMsg 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage ));
					input.Status = false;
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Actions.MakeCall,false);
					Logging.Trace(errorMsg);
				}
				else
				{
					input.Status = true;
					returnMessage.Append(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Make call successfull." 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Actions.MakeCall,true);
					Logging.Trace("Make call successfull");
				}
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				Logging.Trace("Sample Adapter", "going out from make call");
				return input;

			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.MakeCall);
			}
		}

		/// <summary>
		/// IAD can use this for replying for a call
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation AnswerCallFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Sample Adapter","Entering answer call from adapter");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				ParseXMLExceptionInSampleAdapter(input, ex,Constants.Actions.AnswerCall);
			}
			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;

				Object vErr = mAgent.AnswerCall(null);//attributes
				if (vErr != null)
				{
					string errorMessage = "Answering failed due to following : \n" +
										vErr.ToString();
					Logging.Error("Sample Adapter", errorMessage);
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						 + errorMessage 
						 + Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						 Constants.Actions.AnswerCall, 
						 false);
					input.Status = false;
					return input;

				}
				input.Status = true;
				input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
					+ "Answering call successfull" 
					+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
					Constants.Actions.AnswerCall, 
					true);
				Logging.Trace("Sample Adapter", "Going out from answering call successfully");
				return input;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.AnswerCall);
			}
		}

		/// <summary>
		/// It can be used for releasing or hanging up a call
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation ReleaseCallFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Sample Adapter","Entering release call");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				ParseXMLExceptionInSampleAdapter(input, ex ,Constants.Actions.ReleaseCall);
			}

			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				Object vErr = mAgent.ReleaseCall( null);//attributes
				if (vErr != null)
				{
					string errorMessage = "Releasing failed due to following : \n" +
										vErr.ToString();

					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ errorMessage
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.ReleaseCall,
						false);
					input.Status = false;
					Logging.Error("Sample Adapter", errorMessage);
					return input;

				}
				else
				{
					Logging.Trace("Relasing call successfull");
					Logging.Trace("Trying to mark it as done");
					return MarkDoneFromAdapter(input, callID);
				}
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.ReleaseCall);
			}
		}

		/// <summary>
		/// It can be used for marking a call as done
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation MarkDoneFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Sample Adapter", "Entering Mark done");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				return ParseXMLExceptionInSampleAdapter(input, ex, Constants.Actions.MarkDone);
			}
			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				mAgent.MarkDoneCall(null);//attributes
				input.Status = true;
				input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
					+ "mark DOne" 
					+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage), 
					Constants.Actions.MarkDone , true);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				return input;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.MarkDone);
			}
		}

		private MultichannelMessageInformation MarkDoneFromAdapter(MultichannelMessageInformation input, string callID)
		{
			Logging.Trace("Sample Adapter", "Entering Mark done");
			try
			{
				mAgent.MarkDoneCall(null);//attributes
				input.Status = true;
				input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
					+"Release CAll Successfull and Mark done call is also successful" 
					+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage), 
					Constants.Actions.MarkDone,
					true);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				Logging.Trace("Sample Adapter", "Exiting Mark done");
				return input;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.MarkDone);
			}
		}

		/// <summary>
		/// It can be used for holding a call
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation HoldCallFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Entering Hold call");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				return ParseXMLExceptionInSampleAdapter(input, ex, Constants.Actions.HoldCall);
			}
			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				Object vErr = mAgent.HoldCall(null);//attributes
				if (vErr != null)
				{
					string errorMessage = "Holding call failed due to following : \n" +
										"telephony error : " + vErr.ToString();
					Logging.Error("Sample Adapter",errorMessage);
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Hold call failed" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage), 
						Constants.Actions.HoldCall, 
						false);
					input.Status = false;
					return input;
				}
				else
				{
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Hold call successfull" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.HoldCall,
						true);
					input.Status = true;
					Logging.Trace("Sample Adapter", "Hold call successfull");
					return input;
				}
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.HoldCall);
			}
		}
		/// <summary>
		/// It can be used for holding a call
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation UnHoldCallFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Entering UnHold call");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				return ParseXMLExceptionInSampleAdapter(input, ex, Constants.Actions.UnHoldCall);
			}
			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				object vErr = mAgent.UnHoldCall(null);//attributes
				if (vErr != null)
				{
					string errorMessage = "UnHolding call failed due to following : \n" +
										"telephony error : " + vErr.ToString();
					Logging.Error("Sample Adapter",errorMessage);
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "UnHolding call failed" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.UnHoldCall,
						false);
					input.Status = false;
					return input;

				}
				else
				{
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Unholding call successfull" 
						+Constants.MakeEndTag(Constants.NodeNames.ReturnMessage), 
						Constants.Actions.UnHoldCall,
						true);
					input.Status = true;
					Logging.Trace("Sample Adapter", "Unholding call successfull");
					return input;
				}
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.UnHoldCall);
			}
		}

		/// <summary>
		/// It can be used for holding a call
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation MuteOn(MultichannelMessageInformation input)
		{
			Logging.Trace("Entering Mute On");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				return ParseXMLExceptionInSampleAdapter(input, ex, Constants.Actions.MuteCall);
			}
			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				object vErr = mAgent.MuteOn(null);//attributes
				if (vErr != null)
				{
					string errorMessage = "Mute call failed due to following : \n" +
										"telephony error : " + vErr.ToString();
					Logging.Error("Sample Adapter ", errorMessage);
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Mute call failed" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.MuteCall, false);
					input.Status = false;
					return input;

				}
				else
				{
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Mute call successfull" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.MuteCall,
						true);
					input.Status = true;
					Logging.Trace("Sample Adapter", "Mute call successfull");
					return input;
				}
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.MuteCall);
			}
		}

		/// <summary>
		/// It can be used for holding a call
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation MuteOff(MultichannelMessageInformation input)
		{
			Logging.Trace("Entering Mute Off");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				return ParseXMLExceptionInSampleAdapter(input, ex, Constants.Actions.UnmuteCall);
			}
			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				object vErr = mAgent.MuteOff(null);//attributes
				if (vErr != null)
				{
					string errorMessage = "Mute Off failed due to following : \n" +
										"telephony error : " + vErr.ToString();
					Logging.Error("Sample  Adapter",errorMessage);
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Mute Off call failed" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage), 
						Constants.Actions.UnmuteCall,
						false);
					input.Status = false;
					return input;

				}
				else
				{
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Mute Off call successfull" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.UnmuteCall,
						true);
					input.Status = true;
					Logging.Trace("Sample Adapter", "Mute Off call successfull"); 
					return input;
				}
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.UnmuteCall);
			}
		}
		/// <summary>
		/// It can be used for transferring a call in two way mode
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation TransferCallFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Sample Adapter", "Entering transferring call");
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(input.Message);
			}
			catch (Exception ex)
			{
				return ParseXMLExceptionInSampleAdapter(input, ex, Constants.Actions.TransferCall);
			}

			try
			{
				string callID = GetNodeValue(doc, Constants.NodeNames.CallID);
				string destnumber = GetNodeValue(doc, Constants.NodeNames.DestNumber);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				object vErr = mAgent.TransferCall(null);//attributes

				if (vErr != null)
				{
					string errorMessage = "Transferring call failed due to following : \n" +
										"telephony error : " + vErr.ToString();
					Logging.Error("Sample Adapter",errorMessage);
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Transfering not successfull" 
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.TransferCall,
						false);
					input.Status = false;
					return input;

				}
				else
				{
					Logging.Trace("Sample Adapter","Transferring successfull");
					input.Status = true;
					input.Message = mAgent.MakeMessageString(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Transferring successfull" 
						+Constants.MakeEndTag(Constants.NodeNames.ReturnMessage),
						Constants.Actions.TransferCall,
						true);
					return input;
				}
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.TransferCall);
			}
		}

		/// <summary>
		/// This will make the status of an agent as ready of possible
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation MakeAgentReadyFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Making agent ready from Sample adapter");
			try
			{

				object mErr = mAgent.Ready();
				StringBuilder returnMessage = new StringBuilder();
				
				if (mErr != null)
				{
					input.Status = false;
					Logging.Error("Sample Adapter", "Make ready: ERROR: " + mErr.ToString());
					returnMessage.AppendLine(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Make ready not successful on:"
						+ mErr.ToString()
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Actions.MakeReady, false);
				}
				else
				{
					input.Status = true;
					Logging.Trace("Make ready: Successfull");
					returnMessage.AppendLine(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Make ready  successful"
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Actions.MakeReady, true);
				}

				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				return input;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.MakeReady);
			}
		}

		/// <summary>
		/// This will make the status of an agent as not ready of possible
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation MakeAgentNotReadyFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Making agent not ready from Sample adapter");
			try
			{

				object mErr = mAgent.NotReady();
				StringBuilder returnMessage = new StringBuilder();
				
				if (mErr != null)
				{
					input.Status = false;
					Logging.Error("Sample Adapter", "Make not ready: ERROR: " + mErr.ToString());
					returnMessage.AppendLine(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Make not ready not successful on:"
						+ mErr.ToString()
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Actions.MakeReady, false);
				}
				else
				{
					input.Status = true;
					Logging.Trace("Make not ready: Successfull");
					returnMessage.AppendLine(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "Make not ready successful"
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Actions.MakeReady, true);
				}

				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				return input;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Actions.MakeNotReady);
			}
		}

		//TODO add aftercallwork for action as well
		/// <summary>
		/// This will make the status of an agent as AfterCallWork 
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation MakeAgentAfterCallWorkFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Making agent AfterCallWork from Sample adapter");
			try
			{

				Object mErr = mAgent.AfterCallWork();
				StringBuilder returnMessage = new StringBuilder();
				if (mErr != null)
				{
					input.Status = false;
					Logging.Error("Sample Adapter", "AfterCallWork: ERROR: " + mErr.ToString());
					returnMessage.AppendLine(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "AfterCallWork not successful on:"
						+ mErr.ToString()
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Status.AfterCallWorkStatus, false);
				}
				else
				{
					input.Status = true;
					Logging.Trace("AfterCallWork: Successfull");
					returnMessage.AppendLine(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
						+ "AfterCallWork  successful"
						+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
					input.Message = mAgent.MakeMessageString(returnMessage.ToString(), Constants.Status.AfterCallWorkStatus, true);
				}

				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				return input;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, Constants.Status.AfterCallWorkStatus);
			}
		}
		/// <summary>
		/// This will return the status of the agent
		/// Unstable right now due to hardcoding of status
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private MultichannelMessageInformation GetAgentStatusFromAdapter(MultichannelMessageInformation input)
		{
			Logging.Trace("Getting Agent status");
			try
			{
				string status = mAgent.GetAgentStatus("action",false);
				StringBuilder returnMessage = new StringBuilder();
				returnMessage.AppendLine(Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
					+ "Successfully retrived the status of agent" 
					+ Constants.MakeEndTag(Constants.NodeNames.ReturnMessage));
				returnMessage.AppendLine("<AgentStatus>"+status+"<AgentStatus>");
				
				input.Status = true;
				input.Message = mAgent.MakeMessageString(returnMessage.ToString(),Constants.Actions.AgentStatus,true);
				input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
				Logging.Trace("Going out from Getting agent status");
				return input;
			}
			catch (Exception ex)
			{
				return ParseAllExceptionInSampleAdapter(input, ex, "GETAGENTSTATUS");
			}
		}

		private MultichannelMessageInformation SetAgentStatus(MultichannelMessageInformation input)
		{
			string message = input.Message;
			XmlDocument doc = new XmlDocument();
			try
			{
				doc.LoadXml(message);
			}
			catch (Exception ex)
			{

				return ParseXMLExceptionInSampleAdapter(input, ex, Constants.Actions.Login);
			}

			string status = GetNodeValue(doc, Constants.NodeNames.StatusNode);
			input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
			if (status != null)
			{
				switch (status)
				{
					case Constants.Status.ReadyStatus:
						return MakeAgentReadyFromAdapter(input);
					case Constants.Status.NotReadyStatus:
						return MakeAgentNotReadyFromAdapter(input);
					case Constants.Status.AfterCallWorkStatus:
						return MakeAgentAfterCallWorkFromAdapter(input);
					default:
						Logging.Trace("SampleAdapter.SetAgentStatus", "Unknown status type: " + status);
						input.Message = mAgent.MakeMessageString("Unknown status type:"+status, Constants.Actions.SetAgentStatus, false);
						break;
				}
			}
			else
			{
				Logging.Trace("SampleAdapter.SetAgentStatus", "empty status type");
				input.Message = mAgent.MakeMessageString("empty status", Constants.Actions.SetAgentStatus, false);
			}
			input.Status = false;
			return input;
		}
		private string LoggingAtServerStatement()
		{
			return Constants.MakeStartTag(Constants.NodeNames.ReturnMessage)
				+ "Error at Server please contact server" 
				+Constants.MakeEndTag(Constants.NodeNames.ReturnMessage);
		}

		private MultichannelMessageInformation ParseAllExceptionInSampleAdapter(MultichannelMessageInformation input, Exception ex, string functionName)
		{
			if(ex is XmlException)
			{
				StringBuilder returnMessage = new StringBuilder("");
				returnMessage.Append("XML Error. Possibly some required nodes are missing in the message");
				Logging.Error("Sample Adapter", returnMessage.ToString());
			}
			else
			{
				StringBuilder returnMessage = new StringBuilder("");
				returnMessage.Append("Unknown error");
				returnMessage.Append(ex.ToString());
				Logging.Error("Sample Adapter", returnMessage.ToString());
			}
			input.Message = mAgent.MakeMessageString(LoggingAtServerStatement(), functionName, false);
			input.Status = false;
			input.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
			return input;
		}

		private MultichannelMessageInformation ParseXMLExceptionInSampleAdapter(MultichannelMessageInformation mmi, Exception ex,string functionName)
		{
			StringBuilder returnMessage = new StringBuilder("Sample Adapter : ");
			returnMessage.Append(" Error in parsing the message: " + ex.ToString());
			Logging.Error("Sample adapter", returnMessage.ToString());
			mmi.Message = mAgent.MakeMessageString(LoggingAtServerStatement(), functionName, false);
			mmi.Status = false;
			mmi.MessageType = MultiChannelMessageType.FINALRESPONSEFROMADAPTER;
			return mmi;
		}

		public static MultichannelMessageInformation CreateRequest(string appID, string agentGuid, string message, string action, MultiChannelMessageType messageType)
		{
			MultichannelMessageInformation request = new MultichannelMessageInformation();
			//			string agentGuid = Context["AgentGuid"];

			request.AgentApplicationID = appID;
			request.AgentGuid = agentGuid;
			request.Message = message;
			request.MessageType = messageType;
			request.Action = action;
			request.Status = true;
			return request;
		}

		/// <summary>
		/// Handling events coming from provider
		/// </summary>
		/// <param name="events"></param>
		public void notifyEvents(Object[] events)
		{
			if (events == null)
			{
				Logging.Trace("Sample Adapter: In notify events : " + "NotifyEvents - null");
				//return;
			}
			else
			{
				foreach (Object evt in events)
				{
					Logging.Trace("Recieved new event from Service: " + evt.ToString());
					MultichannelMessageInformation eventFromProvider = mAgent.HandleEvent(evt);
					if (eventFromProvider != null)
					{
						switch (eventFromProvider.AgentApplicationID.ToUpper())
						{
							case Constants.ApplicationTypes.VoiceApplicationType:
								eventFromProvider.AgentApplicationID = AdapterUtility.GetApplicationID(this.GetAgentInfo().Provider,
																									Microsoft.Ccf.Multichannel.AdapterManager.InteractionType.VOICE).ToString();
								break;
							case Constants.ApplicationTypes.LoginApplicationType:
								eventFromProvider.AgentApplicationID = AdapterUtility.GetApplicationID(this.GetAgentInfo().Provider,
																									Microsoft.Ccf.Multichannel.AdapterManager.InteractionType.LOGIN).ToString();
								break;
						}
						Logging.Trace("Sample Adapter", "Sending event from Adapter with action : " + eventFromProvider.Action);
						this.SendMessage(eventFromProvider);
					}
				}
			}
		}
	}
}
