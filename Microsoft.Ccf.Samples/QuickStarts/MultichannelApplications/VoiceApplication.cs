//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
//
//===============================================================================

using System;
using System.Windows.Forms;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Multichannel;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Multichannel.AdapterManager.Common;
using Microsoft.Ccf.Multichannel.MultichannelApplicationsLibrary;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.MultiChannelAdapters.Common;

namespace Microsoft.Ccf.Samples.MultichannelApplications
{
	/// <summary>
	/// Summary description for MultiChannelVoiceApplication.
	/// </summary>
	public class VoiceApplication : Microsoft.Ccf.Csr.HostedControl
	{

		#region Form Variables

		private Button btnTerminate;
		private Button btnHold;
		private Button btnAnswer;
		private System.ComponentModel.IContainer components;
		private Label lblStatus;
		#endregion

		#region Private Variables
		// whether current call is on mute or not
		private bool isOnMute = false;
		
		// whether a call is on hold or not
		private bool isOnHold = false;

		// ID of the current call
		private string currentCallID = string.Empty;

		//ID of the call on hold
		private string onHoldCallID = string.Empty;
		private static string defaultErrorMessage = "Problem while communicating with server or some internal error occured. Check the log file for more detailed message.";
		private Label lblLoginApplicationForVoice;
		private Button btnMakeACall;
		private TextBox txtCustomerNumber;
		private Label lblPhoneNumber;
		private GroupBox gBoxLoginStatus;
		private string clientID;
		#endregion

		#region Constructor info
		public VoiceApplication()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public VoiceApplication(int appID, string appName, string initString)
			:
			base( appID, appName, initString )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}
		#endregion

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Microsoft.Ccf.Csr.Context context1 = new Microsoft.Ccf.Csr.Context();
			this.btnAnswer = new System.Windows.Forms.Button();
			this.btnTerminate = new System.Windows.Forms.Button();
			this.btnHold = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.lblLoginApplicationForVoice = new System.Windows.Forms.Label();
			this.btnMakeACall = new System.Windows.Forms.Button();
			this.txtCustomerNumber = new System.Windows.Forms.TextBox();
			this.lblPhoneNumber = new System.Windows.Forms.Label();
			this.gBoxLoginStatus = new System.Windows.Forms.GroupBox();
			this.gBoxLoginStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnAnswer
			// 
			this.btnAnswer.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnAnswer.Enabled = false;
			this.btnAnswer.Location = new System.Drawing.Point(242, 132);
			this.btnAnswer.Name = "btnAnswer";
			this.btnAnswer.Size = new System.Drawing.Size(92, 21);
			this.btnAnswer.TabIndex = 3;
			this.btnAnswer.Text = "Answer";
			this.btnAnswer.UseVisualStyleBackColor = false;
			this.btnAnswer.Click += new System.EventHandler(this.btnAnswer_Click);
			// 
			// btnTerminate
			// 
			this.btnTerminate.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnTerminate.Enabled = false;
			this.btnTerminate.Location = new System.Drawing.Point(340, 132);
			this.btnTerminate.Name = "btnTerminate";
			this.btnTerminate.Size = new System.Drawing.Size(90, 21);
			this.btnTerminate.TabIndex = 4;
			this.btnTerminate.Text = "Terminate";
			this.btnTerminate.UseVisualStyleBackColor = false;
			this.btnTerminate.Click += new System.EventHandler(this.btnTerminate_Click);
			// 
			// btnHold
			// 
			this.btnHold.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnHold.Enabled = false;
			this.btnHold.Location = new System.Drawing.Point(340, 105);
			this.btnHold.Name = "btnHold";
			this.btnHold.Size = new System.Drawing.Size(90, 21);
			this.btnHold.TabIndex = 2;
			this.btnHold.Text = "Hold";
			this.btnHold.UseVisualStyleBackColor = false;
			this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.ForeColor = System.Drawing.Color.Red;
			this.lblStatus.Location = new System.Drawing.Point(6, 22);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(370, 45);
			this.lblStatus.TabIndex = 3;
			// 
			// lblLoginApplicationForVoice
			// 
			this.lblLoginApplicationForVoice.AutoSize = true;
			this.lblLoginApplicationForVoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLoginApplicationForVoice.ForeColor = System.Drawing.SystemColors.Desktop;
			this.lblLoginApplicationForVoice.Location = new System.Drawing.Point(96, 27);
			this.lblLoginApplicationForVoice.Name = "lblLoginApplicationForVoice";
			this.lblLoginApplicationForVoice.Size = new System.Drawing.Size(299, 24);
			this.lblLoginApplicationForVoice.TabIndex = 25;
			this.lblLoginApplicationForVoice.Text = "Multichannel Voice Application";
			// 
			// btnMakeACall
			// 
			this.btnMakeACall.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnMakeACall.Enabled = false;
			this.btnMakeACall.Location = new System.Drawing.Point(242, 105);
			this.btnMakeACall.Name = "btnMakeACall";
			this.btnMakeACall.Size = new System.Drawing.Size(92, 21);
			this.btnMakeACall.TabIndex = 1;
			this.btnMakeACall.Text = "Make a call";
			this.btnMakeACall.UseVisualStyleBackColor = false;
			this.btnMakeACall.Click += new System.EventHandler(this.btnMakeACall_Click);
			// 
			// txtCustomerNumber
			// 
			this.txtCustomerNumber.Location = new System.Drawing.Point(127, 106);
			this.txtCustomerNumber.Name = "txtCustomerNumber";
			this.txtCustomerNumber.Size = new System.Drawing.Size(109, 20);
			this.txtCustomerNumber.TabIndex = 0;
			// 
			// lblPhoneNumber
			// 
			this.lblPhoneNumber.AutoSize = true;
			this.lblPhoneNumber.Location = new System.Drawing.Point(45, 109);
			this.lblPhoneNumber.Name = "lblPhoneNumber";
			this.lblPhoneNumber.Size = new System.Drawing.Size(76, 13);
			this.lblPhoneNumber.TabIndex = 9;
			this.lblPhoneNumber.Text = "Phone number";
			// 
			// gBoxLoginStatus
			// 
			this.gBoxLoginStatus.Controls.Add(this.lblStatus);
			this.gBoxLoginStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gBoxLoginStatus.ForeColor = System.Drawing.SystemColors.Desktop;
			this.gBoxLoginStatus.Location = new System.Drawing.Point(48, 171);
			this.gBoxLoginStatus.Name = "gBoxLoginStatus";
			this.gBoxLoginStatus.Size = new System.Drawing.Size(382, 83);
			this.gBoxLoginStatus.TabIndex = 26;
			this.gBoxLoginStatus.TabStop = false;
			this.gBoxLoginStatus.Text = "Call Status";
			// 
			// VoiceApplication
			// 
			this.BackColor = System.Drawing.Color.White;
			context1.ContextInformation = "<CcfContext />";
			this.Context = context1;
			this.Controls.Add(this.btnAnswer);
			this.Controls.Add(this.lblPhoneNumber);
			this.Controls.Add(this.btnTerminate);
			this.Controls.Add(this.lblLoginApplicationForVoice);
			this.Controls.Add(this.btnHold);
			this.Controls.Add(this.txtCustomerNumber);
			this.Controls.Add(this.btnMakeACall);
			this.Controls.Add(this.gBoxLoginStatus);
			this.Name = "VoiceApplication";
			this.Size = new System.Drawing.Size(518, 319);
			this.gBoxLoginStatus.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		#region Button Actions

		private void btnHold_Click(object sender, EventArgs e)
		{
			lblStatus.Text = "";
			DisableAllActions();
			string message = string.Empty;
			string action = string.Empty;
			if (!isOnHold)
			{
				action = Constants.Actions.HoldCall;
				message = string.Format("<Message>" +
											"<CallID>{0}</CallID>" +
											"<Reasons></Reasons>" +
											"<Textensions></Textensions>" +
										"</Message>", currentCallID);
			}
			else
			{
				action = Constants.Actions.UnHoldCall;
				message = string.Format("<Message>"+
											"<CallID>{0}</CallID>" +
											"<Reasons></Reasons>" +
											"<Textensions></Textensions>" +
										"</Message>", onHoldCallID);
			}
			MultiChannelMessageType messageType = MultiChannelMessageType.REQUESTFROMAGENT;
			string applicationID = this.ApplicationID.ToString();
			MultichannelMessageInformation holdRequest = Utility.CreateRequest(applicationID, clientID, message, action, messageType);
			lblStatus.Text = "Putting call on hold...";
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(holdRequest);

			// If there is any problem in the response
			if ((result == null) || (result.MessageType == MultiChannelMessageType.ERRORONADAPTER))
			{
				lblStatus.Text = defaultErrorMessage;
				EnableActionsToDefaultStates();
				return;
			}

			if (result.Status.Equals(false))
			{
				lblStatus.Text = defaultErrorMessage;
			}
			else if (result.Action.Equals(Constants.Actions.HoldCall))
			{
				lblStatus.Text = "Call on hold.";
				isOnHold = true;
				onHoldCallID = currentCallID;
				currentCallID = string.Empty;
			}
			else
			{
				lblStatus.Text = "Call unholded.";
				isOnHold = false;
				currentCallID = onHoldCallID;
				onHoldCallID = string.Empty;
			}
			string possibleActions = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.PossibleActionsNodes);
			EnablePossibleActions(possibleActions);
			string statusAtProvider = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.StatusNode);
			ProcessStatusAtProvider(statusAtProvider);
		}

		private void btnTerminate_Click(object sender, EventArgs e)
		{
			lblStatus.Text = "";
			DisableAllActions();
			string message = string.Format("<Message>" +
												"<CallID>{0}</CallID>" +
												"<Reasons></Reasons>" + 
												"<Textensions></Textensions>" +
											"</Message>", currentCallID);
			string action = Constants.Actions.ReleaseCall;
			MultiChannelMessageType messageType = MultiChannelMessageType.REQUESTFROMAGENT;
			string applicationID = this.ApplicationID.ToString();
			MultichannelMessageInformation terminateRequest = Utility.CreateRequest(applicationID, clientID, message, action, messageType);
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(terminateRequest);
			// If there is any problem in the response
			if ((result == null) || (result.MessageType == MultiChannelMessageType.ERRORONADAPTER))
			{
				lblStatus.Text = defaultErrorMessage;
				EnableActionsToDefaultStates();
				return;
			}
			if (result.Status.Equals(false))
			{
				lblStatus.Text = defaultErrorMessage;
			}
			else
			{
				lblStatus.Text = "Call successfully terminated.";
				currentCallID = string.Empty;
			}
			string possibleActions = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.PossibleActionsNodes);
			EnablePossibleActions(possibleActions);
			string statusAtProvider = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.StatusNode);
			ProcessStatusAtProvider(statusAtProvider);
		}

		private void btnMakeACall_Click(object sender, EventArgs e)
		{
			lblStatus.Text = "";
			string customerNumber = txtCustomerNumber.Text;
			if (customerNumber.Trim().Equals(string.Empty))
			{
				lblStatus.Text = "Please enter a valid number.";
				return;
			}
			DisableAllActions();
			// setting the callID as empty string to make sure that we dont use previous call's id
			currentCallID = String.Empty;
			
			string message = string.Format("<Message>" +
												"<DestNumber>{0}</DestNumber>" +
												"<Location></Location>" +
												"<Reasons></Reasons>" +
												"<Textensions></Textensions>" +
											"</Message>", customerNumber);
			string action = Constants.Actions.MakeCall;
			MultiChannelMessageType messageType = MultiChannelMessageType.REQUESTFROMAGENT;
			string applicationID = this.ApplicationID.ToString();
			MultichannelMessageInformation makeCallRequest = Utility.CreateRequest(applicationID, clientID, message, action, messageType);
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(makeCallRequest);
			// If there is any problem in the response
			if ((result == null) || (result.MessageType == MultiChannelMessageType.ERRORONADAPTER))
			{
				lblStatus.Text = defaultErrorMessage;
				EnableActionsToDefaultStates();
				return;
			}

			string possibleActions = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.PossibleActionsNodes);
			EnablePossibleActions(possibleActions);
			string statusAtProvider = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.StatusNode);
			ProcessStatusAtProvider(statusAtProvider);

			if (result.Status.Equals(false))
			{
				lblStatus.Text = defaultErrorMessage;
			}
			else
			{
				currentCallID = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.CallID);
				lblStatus.Text = "Making call...";
			}


		}

		private void btnAnswer_Click(object sender, EventArgs e)
		{
			lblStatus.Text = "";
			DisableAllActions();
			string message = string.Format("<Message>" +
												"<CallID>{0}</CallID>" +
												"<Reasons></Reasons>" +
												"<Textensions></Textensions>" +
											"</Message>", currentCallID);
			string action = Constants.Actions.AnswerCall;
			MultiChannelMessageType messageType = MultiChannelMessageType.REQUESTFROMAGENT;
			string applicationID = this.ApplicationID.ToString();
			MultichannelMessageInformation answerCallRequest = Utility.CreateRequest(applicationID, clientID, message, action, messageType);
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(answerCallRequest);
			// If there is any problem in the response
			if ((result == null) || (result.MessageType == MultiChannelMessageType.ERRORONADAPTER))
			{
				lblStatus.Text = defaultErrorMessage;
				EnableActionsToDefaultStates();
				return;
			}
			if (result.Status.Equals(false))
			{
				lblStatus.Text = defaultErrorMessage;
			}
			else
			{
				lblStatus.Text = "Call successfully answered.";
			}
			string possibleActions = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.PossibleActionsNodes);
			EnablePossibleActions(possibleActions);
			string statusAtProvider = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.StatusNode);
			ProcessStatusAtProvider(statusAtProvider);
		}
		#endregion

		public override void DoAction(string actionName, string data)
		{
			// if there is any action, clear the status
			lblStatus.Text = string.Empty;
			if (actionName.Equals("ProcessResponse"))
			{
				ProcessResponse(data);
			}
			else if (actionName.Equals("DisableAllActions"))
			{
				DisableAllActions();
			}
			else if (actionName.Equals("EnableActionsToDefaultStates"))
			{
				EnableActionsToDefaultStates();
			}
			else if (actionName.Equals("UpdateClientID"))
			{
				this.clientID = data;
			}
		}

		#region Private Methods

		/// <summary>
		/// Process the status at provider
		/// </summary>
		/// <param name="statusAtProvider">current status at provider</param>
		private void ProcessStatusAtProvider(string statusAtProvider)
		{
			//TODO: SI can write their custom code here. This status can be used to reflect the status at IAD.
			Logging.Trace("Current status at provider is " + statusAtProvider);
		}


		/// <summary>
		/// Disable all the buttons on the form
		/// </summary>
		public void DisableAllActions()
		{
			btnAnswer.Enabled = false;
			btnHold.Enabled = false;
			btnMakeACall.Enabled = false;
			btnTerminate.Enabled = false;
		}

		/// <summary>
		/// Enables buttons to default states 
		/// </summary>
		private void EnableActionsToDefaultStates()
		{
			DisableAllActions();
			btnMakeACall.Enabled = true;
		}

		/// <summary>
		/// Enables the actions specified in the list
		/// </summary>
		/// <param name="possibleActionsList">List of possible actions</param>
		private void EnablePossibleActions(string possibleActionsList)
		{
			string[] actions = possibleActionsList.Split(new char[] {','});
			foreach (string action in actions)
			{
				switch (action.ToUpper())
				{ 
					case Constants.Actions.MakeCall:
						btnMakeACall.Enabled = true;
						break;
					case Constants.Actions.HoldCall:
						btnHold.Text = "Hold";
						btnHold.Enabled = true;
						break;
					case Constants.Actions.UnHoldCall:
						btnHold.Text = "Unhold";
						btnHold.Enabled = true;
						break;
					case Constants.Actions.AnswerCall:
						btnAnswer.Enabled = true;
						break;
					case Constants.Actions.ReleaseCall:
						btnTerminate.Enabled = true;
						break;
				}
			}
		}

		/// <summary>
		/// Processes the asynchronous response obtained from provider
		/// </summary>
		/// <param name="res">Response from provider</param>
		private void ProcessResponse(string res)
		{
			Logging.Trace("Final response obtained in application :" + res);
			MultichannelMessageInformation response = GeneralFunctions.Deserialize<MultichannelMessageInformation>(res);

			// Check whether action is completed successfully at adapter

			string message = response.Message;
			if (response.Status.Equals(false))
			{
				lblStatus.Text = "Some problem encountered at server.";
			}
			else
			{
				switch (response.Action)
				{
					case Constants.Actions.NewIncomingCall:
						currentCallID = GeneralFunctions.GetXmlNodeValue(message, Constants.NodeNames.CallID);
						lblStatus.Text = "New incoming call...";
						break;
					case Constants.Actions.ReleaseCall:
						lblStatus.Text = "Call terminated from other end.";
						currentCallID = string.Empty;
						break;
					case Constants.Actions.CallConnected:
						lblStatus.Text = "Call successfully connected.";
						break;
					default:
						Logging.Warn("Multichannel Voice application", "Response message : " + response.Message);
						break;
				}
			}
			DisableAllActions();
			string possibleActions = GeneralFunctions.GetXmlNodeValue(message, Constants.NodeNames.PossibleActionsNodes);
			EnablePossibleActions(possibleActions);
		}
		#endregion

	}
}
