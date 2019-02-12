//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
//
// UI for reading custom login details in case of non-IWS environment
//
//===============================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Ccf.Csr.UIConfiguration;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Common.Exception;
using Microsoft.Ccf.Common.Logging;


namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// Summary description for LoginForm.
	/// </summary>
	public partial class LoginForm : Form
	{
		public LoginForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Runs when user fills the login details and presses submit
		/// </summary>
		private void btnSubmit_Click(object sender, EventArgs e)
		{
			try
			{
				btnSubmit.Enabled = false;
				lblError.Text = "";
				if ((txtUsername.Text == String.Empty) || (txtPassword.Text == String.Empty) || (txtDomain.Text == String.Empty))
				{
					lblError.Text = localize.DESKTOP_MSG_EMPTY_LOGIN_DETAILS;
					btnSubmit.Enabled = true;
					return;
				}
				// validate the login details and keep a copy on client for all further communication with server
				AgentCredentialUtilities.ValidateAndSetCCFAgentCredential(txtUsername.Text, txtPassword.Text, txtDomain.Text);
				this.Close();
			}
			// We can not log these exceptions as we dont have enough priviledges yet to write in the logs
			catch (WebserviceConfigurationException wce)
			{
				lblError.Text = localize.DESKTOP_MSG_WEBSERVICE_CLIENT_MISCONFIGURED;
				btnSubmit.Enabled = true;
			}
			catch (Exception ex)
			{
				lblError.Text = localize.DESKTOP_MSG_INVALID_LOGIN_DETAILS;
				btnSubmit.Enabled = true;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Abort;
		}

		private void txtUsername_Enter(object sender, EventArgs e)
		{
			txtUsername.SelectAll();
		}

		private void txtPassword_Enter(object sender, EventArgs e)
		{
			txtPassword.SelectAll();
		}

		private void txtDomain_Enter(object sender, EventArgs e)
		{
			txtDomain.SelectAll();
		}
	}
}
