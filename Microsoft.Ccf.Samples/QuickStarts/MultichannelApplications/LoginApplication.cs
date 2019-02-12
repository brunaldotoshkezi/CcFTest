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
using System.Collections.Generic;
using System.Text;
using Microsoft.Ccf.Multichannel.AdapterManager.Common;
using Microsoft.Ccf.Multichannel.MultichannelApplicationsLibrary;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.MultiChannelAdapters.Common;

namespace Microsoft.Ccf.Samples.MultichannelApplications
{
	class LoginApplication : Microsoft.Ccf.Csr.HostedControl
	{
		private System.Windows.Forms.Label lblLoginApplicationForVoice;
		private System.Windows.Forms.Label lblAgentID;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtAgentID;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.TextBox txtPlace;
		private System.Windows.Forms.Label lblPlace;
		private System.Windows.Forms.Button btnLogout;
		private System.ComponentModel.IContainer components;

		private string clientID = null;
		private bool isAgentLoggedIn = false;
		private static string defaultErrorMessage = "Problem while communicating with server or some internal error occured. Check the log file for more detailed message.";
		private System.Windows.Forms.GroupBox gBoxLoginStatus;

		private void InitializeComponent()
		{
			Microsoft.Ccf.Csr.Context context1 = new Microsoft.Ccf.Csr.Context();
			this.lblLoginApplicationForVoice = new System.Windows.Forms.Label();
			this.lblAgentID = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.txtAgentID = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.btnLogout = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.txtPlace = new System.Windows.Forms.TextBox();
			this.lblPlace = new System.Windows.Forms.Label();
			this.gBoxLoginStatus = new System.Windows.Forms.GroupBox();
			this.gBoxLoginStatus.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblLoginApplicationForVoice
			// 
			this.lblLoginApplicationForVoice.AutoSize = true;
			this.lblLoginApplicationForVoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblLoginApplicationForVoice.ForeColor = System.Drawing.SystemColors.Desktop;
			this.lblLoginApplicationForVoice.Location = new System.Drawing.Point(96, 32);
			this.lblLoginApplicationForVoice.Name = "lblLoginApplicationForVoice";
			this.lblLoginApplicationForVoice.Size = new System.Drawing.Size(297, 24);
			this.lblLoginApplicationForVoice.TabIndex = 1;
			this.lblLoginApplicationForVoice.Text = "Multichannel Login Application";
			// 
			// lblAgentID
			// 
			this.lblAgentID.AutoSize = true;
			this.lblAgentID.Location = new System.Drawing.Point(143, 94);
			this.lblAgentID.Name = "lblAgentID";
			this.lblAgentID.Size = new System.Drawing.Size(49, 13);
			this.lblAgentID.TabIndex = 0;
			this.lblAgentID.Text = "Agent ID";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(143, 117);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(53, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Password";
			// 
			// txtAgentID
			// 
			this.txtAgentID.Location = new System.Drawing.Point(202, 88);
			this.txtAgentID.Name = "txtAgentID";
			this.txtAgentID.Size = new System.Drawing.Size(130, 20);
			this.txtAgentID.TabIndex = 0;
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(202, 114);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(130, 20);
			this.txtPassword.TabIndex = 1;
			this.txtPassword.UseSystemPasswordChar = true;
			// 
			// btnLogin
			// 
			this.btnLogin.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnLogin.Location = new System.Drawing.Point(202, 170);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(65, 24);
			this.btnLogin.TabIndex = 3;
			this.btnLogin.Text = "Login";
			this.btnLogin.UseVisualStyleBackColor = false;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// btnLogout
			// 
			this.btnLogout.BackColor = System.Drawing.SystemColors.ButtonFace;
			this.btnLogout.Enabled = false;
			this.btnLogout.Location = new System.Drawing.Point(278, 170);
			this.btnLogout.Name = "btnLogout";
			this.btnLogout.Size = new System.Drawing.Size(65, 24);
			this.btnLogout.TabIndex = 4;
			this.btnLogout.Text = "Logout";
			this.btnLogout.UseVisualStyleBackColor = false;
			this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
			// 
			// lblStatus
			// 
			this.lblStatus.ForeColor = System.Drawing.Color.Red;
			this.lblStatus.Location = new System.Drawing.Point(25, 19);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(271, 48);
			this.lblStatus.TabIndex = 0;
			// 
			// txtPlace
			// 
			this.txtPlace.Location = new System.Drawing.Point(203, 140);
			this.txtPlace.Name = "txtPlace";
			this.txtPlace.Size = new System.Drawing.Size(130, 20);
			this.txtPlace.TabIndex = 2;
			// 
			// lblPlace
			// 
			this.lblPlace.AutoSize = true;
			this.lblPlace.Location = new System.Drawing.Point(143, 140);
			this.lblPlace.Name = "lblPlace";
			this.lblPlace.Size = new System.Drawing.Size(34, 13);
			this.lblPlace.TabIndex = 0;
			this.lblPlace.Text = "Place";
			// 
			// gBoxLoginStatus
			// 
			this.gBoxLoginStatus.Controls.Add(this.lblStatus);
			this.gBoxLoginStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gBoxLoginStatus.ForeColor = System.Drawing.SystemColors.Desktop;
			this.gBoxLoginStatus.Location = new System.Drawing.Point(97, 232);
			this.gBoxLoginStatus.Name = "gBoxLoginStatus";
			this.gBoxLoginStatus.Size = new System.Drawing.Size(318, 78);
			this.gBoxLoginStatus.TabIndex = 5;
			this.gBoxLoginStatus.TabStop = false;
			this.gBoxLoginStatus.Text = "Login Status";
			// 
			// LoginApplication
			// 
			this.BackColor = System.Drawing.Color.White;
			context1.ContextInformation = "<CcfContext />";
			this.Context = context1;
			this.Controls.Add(this.gBoxLoginStatus);
			this.Controls.Add(this.txtPlace);
			this.Controls.Add(this.lblPlace);
			this.Controls.Add(this.btnLogout);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtAgentID);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lblAgentID);
			this.Controls.Add(this.lblLoginApplicationForVoice);
			this.Name = "LoginApplication";
			this.Size = new System.Drawing.Size(513, 352);
			this.gBoxLoginStatus.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		public LoginApplication()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public LoginApplication(int appID, string appName, string initString)
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
			Logging.Trace("Disposing application");
			// In case IAD is forcefully closed, just make sure that we logout the agent. This prevent a stale connection
			// at the provider
			if (isAgentLoggedIn)
			{
				SendLogoutRequest();
			}

			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Button Actions
		private void btnLogin_Click(object sender, EventArgs e)
		{
			lblStatus.Text = "Logging in...";
			DisableAllActions();
			string username = txtAgentID.Text;
			string password = txtPassword.Text;
			string place = txtPlace.Text;
			if ((username.Trim() == string.Empty) || (place.Trim() == string.Empty))
			{
				lblStatus.Text = "Please enter valid Agent ID and Place";
				EnableActionsToDefaultStates();
				return;
			}
			string loginDetails = string.Format("LoginID:{0},Password:{1},Place:{2}", username, password, place);
			string loginDetailsHash = Utility.CalculateMD5Hash(loginDetails);
			string message = string.Format("<Message>" +
												"<AGENT>{0}</AGENT>" +
												"<PASSWORD>{1}</PASSWORD>" +
												"<PLACE>{2}</PLACE>" +
												"<QUEUE></QUEUE>" +
											"</Message>", username, password, place);
			string action = Constants.Actions.Login;

			MultiChannelMessageType messageType = MultiChannelMessageType.REQUESTFROMAGENT;
			string applicationID = this.ApplicationID.ToString();
			MultichannelMessageInformation loginRequest = Utility.CreateRequest(applicationID, clientID, message, action, messageType);
			// this hash is used by the lazy logout mechanism of AdapterManager
			loginRequest.ChannelTypeKey = loginDetailsHash;
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(loginRequest);
			
			// If there is any problem in the intermediate response
			if ((result == null) || (result.MessageType == MultiChannelMessageType.ERRORONADAPTER))
			{
				lblStatus.Text = defaultErrorMessage;
				btnLogin.Enabled = true;
				btnLogout.Enabled = false;
				return;
			}

			string statusAtProvider = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.StatusNode);
			ProcessStatusAtProvider(statusAtProvider);
			
			if (result.Status.Equals(false))
			{
				// We try to extract if there is a reason for failure in message. If yes, we show it to agent
				// else we show the standard error message. This is just to make sure that we show errors which 
				// are relevant to agent(invalid password, username etc)on IAD
				string reasonForFailure = string.Empty;
				try
				{
					reasonForFailure = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.UserVisibleError);
				}
				catch (Exception ex)
				{
					Logging.Warn(this.ToString(), "Encountered error while reading UserVisibleError from response " + ex.Message);
					reasonForFailure = null;
				}
				if ((reasonForFailure == null) || (reasonForFailure.Equals(string.Empty)))
				{
					reasonForFailure = defaultErrorMessage;
				}
				lblStatus.Text = reasonForFailure;
				btnLogin.Enabled = true;
				btnLogout.Enabled = false;
			}
			else
			{
				EnableRelatedApplicationToDefaultState();
				lblStatus.Text = "You have successfully logged in.";
				btnLogin.Enabled = false;
				btnLogout.Enabled = true;
				isAgentLoggedIn = true;
			}
		}


		private void btnLogout_Click(object sender, EventArgs e)
		{
			lblStatus.Text = "Loggin out...";
			DisableAllActions();
			string message = "<Message></Message>";
			string action = Constants.Actions.LogOut;
			MultiChannelMessageType messageType = MultiChannelMessageType.REQUESTFROMAGENT;
			string applicationID = this.ApplicationID.ToString();
			MultichannelMessageInformation logoutRequest = Utility.CreateRequest(applicationID, clientID, message, action, messageType);
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(logoutRequest);

			// If there is any problem in the response
			if ((result == null) || (result.MessageType == MultiChannelMessageType.ERRORONADAPTER) || (result.Status.Equals(false)))
			{
				lblStatus.Text = defaultErrorMessage;
				btnLogin.Enabled = false;
				btnLogout.Enabled = true;
			}
			else
			{
				DisableAllRelatedApplications();
				lblStatus.Text = "You have successfully logged out.";
				btnLogin.Enabled = true;
				btnLogout.Enabled = false;
				isAgentLoggedIn = false;
				string statusAtProvider = GeneralFunctions.GetXmlNodeText(result.Message, Constants.NodeNames.StatusNode);
				ProcessStatusAtProvider(statusAtProvider);
			}
		}

		#endregion

		/// <summary>
		/// Sets the agent status at provider
		/// </summary>
		/// <param name="action"></param>
		public void SetStatusAtProvider(string status)
		{
			Logging.Trace("Setting status at provider to " + status);
			string message = "<Message><Status>" + status + "</Status></Message>";
			MultiChannelMessageType messageType = MultiChannelMessageType.REQUESTFROMAGENT;
			string applicationID = this.ApplicationID.ToString();
			string action = Constants.Actions.SetAgentStatus;
			MultichannelMessageInformation changeStatusRequest = Utility.CreateRequest(applicationID, clientID, message, action, messageType);
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(changeStatusRequest);

			// If there is any problem in the intermediate response
			if ((result == null) || (result.MessageType == MultiChannelMessageType.ERRORONADAPTER) || (result.Status.Equals(false)))
			{
				Logging.Warn(this.ToString(),"Status change at provider failed");
			}
			else
			{
				Logging.Trace("status changed at provider to " + status);
			}
		}

		public override void DoAction(string actionName, string data)
		{
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
			else if(actionName.Equals("UpdateClientID"))
			{
				this.clientID = data;
			}
			
		}


		# region Private Methods

		/// <summary>
		/// Finds all the applications belonging to the same listener and calls the DisableAllActions method on all 
		/// of them. Thus, whenever agent logs out, we disable all the applications belonging to the provider. 
		/// </summary>
		private void DisableAllRelatedApplications()
		{
			UpdateRelatedApplications("DisableAllActions");
		}

		/// <summary>
		/// Finds all the applications belonging to the same listener and calls the EnableActionsToDefaultStates method on all of them.
		/// Thus, whenever agent logs in, we enable actions on applications belonging to the provider to their default state.
		/// </summary>
		private void EnableRelatedApplicationToDefaultState()
		{
			UpdateRelatedApplications("EnableActionsToDefaultStates");
		}
		
		/// <summary>
		/// Finds all the applications belonging to the same listener as this application and fires 
		/// the specified action on all of them.
		/// </summary>
		/// <param name="action">action to be fired on all related applications</param>
		private void UpdateRelatedApplications(string action)
		{
			string relatedApplications = MultichannelApplicationHelper.GetRelatedApplications(this.ApplicationID);
			if ((relatedApplications != null) && (relatedApplications != string.Empty))
			{
				string[] applications = relatedApplications.Split(new char[] { ',' });
				foreach (string app in applications)
				{
					FireRequestAction(new RequestActionEventArgs(app, action, null));
				}
			}
		}

		/// <summary>
		/// Enables the application to its default state.
		/// </summary>
		private void EnableActionsToDefaultStates()
		{
			btnLogin.Enabled = true;
			btnLogout.Enabled = false;
		}

		/// <summary>
		/// Disables all the buttons
		/// </summary>
		private void DisableAllActions()
		{
			btnLogin.Enabled = false;
			btnLogout.Enabled = false;
		}

		/// <summary>
		/// Processes the asynchronous response obtained from provider
		/// </summary>
		/// <param name="res">response from provider</param>
		public void ProcessResponse(string res)
		{
			Logging.Warn("Login Application","Getting unconditional response from AdapterManager : " + res);
			MultichannelMessageInformation response = GeneralFunctions.Deserialize<MultichannelMessageInformation>(res);

			if (response.Action.Equals(Constants.Actions.LogOut))
			{
				lblStatus.Text = "You are logged out from Provider. Please login again.";
				btnLogin.Enabled = true;
				btnLogout.Enabled = false;
			}
		}

		public override void NotifyContextChange(Context context)
		{
			// Looks at the status in context and accordingly updates the status at provider
			string status = Context["status"];
			if((status != null) && (status != string.Empty) && (isAgentLoggedIn))
			{
				SetStatusAtProvider(status);
			}
		}

		/// <summary>
		/// Sends a logout request
		/// </summary>
		private void SendLogoutRequest()
		{
			MultichannelMessageInformation logoutRequest = new MultichannelMessageInformation();
			logoutRequest.Action = Constants.Actions.LogOut;
			logoutRequest.AgentGuid = clientID;
			logoutRequest.MessageType = MultiChannelMessageType.REQUESTFROMAGENT;
			logoutRequest.AgentApplicationID = this.ApplicationID.ToString();
			MultichannelMessageInformation result = MultichannelApplicationHelper.SendRequest(logoutRequest);
			if (result != null)
			{
				Logging.Trace("Logging out from the provider while disposing");
			}
		}

		/// <summary>
		/// Process the status at provider
		/// </summary>
		/// <param name="statusAtProvider">current status at provider</param>
		private void ProcessStatusAtProvider(string statusAtProvider)
		{
			//TODO: SI can write their custom code here. This status can be used to reflect the status at IAD.
			Logging.Trace("Current status at provider is " + statusAtProvider);
		}
		#endregion
	}
}
