//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// Module containing the phone menu handling code for the agent desktop.
// If this needs to be removed for a customer engagement, an easier and less invasive
// method to do so is to disable CTI hosted control
//
//===============================================================================

#region Usings
using System;
using System.Diagnostics;
using System.Windows.Forms;

using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr.Cti.Providers;
using Microsoft.Ccf.Csr.UIConfiguration;
using Options = Microsoft.Ccf.Samples.Csr.AgentDesktop.Options;
using Microsoft.Ccf.Common;
#endregion

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// Module containing the phone menu handling code for the agent desktop.
	/// </summary>
	/// <remarks>
	/// If this needs to be removed for a customer engagement, an easier and less invasive
	/// method to do so is to disable CTI hosted control
	/// </remarks>
	public class PhoneMenu : ContextMenu
	{
		#region Variables
		// Privates
		private System.Windows.Forms.MenuItem dial;
		private System.Windows.Forms.MenuItem hangup;
		private System.Windows.Forms.MenuItem transfer;
		private System.Windows.Forms.MenuItem conference;
		private System.Windows.Forms.MenuItem answer;
		public System.Windows.Forms.MenuItem hold;
		public System.Windows.Forms.MenuItem unhold;
		private Process remoteTelephonyProcess = null;
		private TelephonyProvider cti = null;
		private LineClassProvider myLine = null;
		private Desktop ownerForm = null;
		#endregion

		#region Properties
		public TelephonyProvider Cti
		{
			get { return cti; }
		}

		public LineClassProvider MyLine
		{
			get { return myLine; }
		}

		public Process RemoteTelephonyProcess
		{
			get { return remoteTelephonyProcess; }
		}
		#endregion

		/// <summary>
		/// PhoneMenu constructor
		/// </summary>
		public PhoneMenu()
		{
			this.dial = new System.Windows.Forms.MenuItem();
			this.answer = new System.Windows.Forms.MenuItem();
			this.hangup = new System.Windows.Forms.MenuItem();
			this.hold = new System.Windows.Forms.MenuItem();
			this.unhold = new System.Windows.Forms.MenuItem();
			this.transfer = new System.Windows.Forms.MenuItem();
			this.conference = new System.Windows.Forms.MenuItem();

			this.dial.Text = localize.DESKTOP_DIAL_TEXT;
			this.answer.Text = localize.DESKTOP_ANSWER_TEXT;
			this.hangup.Text = localize.DESKTOP_HANGUP_TEXT;
			this.hold.Text = localize.DESKTOP_HOLD_TEXT;
			this.unhold.Text = localize.DESKTOP_UNHOLD_TEXT;
			this.transfer.Text = localize.DESKTOP_TRANSFER_TEXT;
			this.conference.Text = localize.DESKTOP_CONFERENCE_TEXT;

			// 
			// phoneMenu
			// 
			this.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
	                                                                        this.dial,
	                                                                        this.answer,
	                                                                        this.hangup,
	                                                                        this.hold,
	                                                                        this.unhold,
	                                                                        this.transfer,
	                                                                        this.conference});
			// 
			// dial
			// 
			this.dial.DefaultItem = true;
			this.dial.Index = 0;
			this.dial.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF1;
			this.dial.Click += new System.EventHandler(this.dial_Click);
			// 
			// answer
			// 
			this.answer.Index = 1;
			this.answer.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF2;
			this.answer.Click += new System.EventHandler(this.answer_Click);
			// 
			// hangup
			// 
			this.hangup.Index = 2;
			this.hangup.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF3;
			this.hangup.Click += new System.EventHandler(this.hangup_Click);
			// 
			// hold
			// 
			this.hold.Index = 3;
			this.hold.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF4;
			this.hold.Click += new System.EventHandler(this.hold_Click);
			// 
			// unhold
			// 
			this.unhold.Index = 4;
			this.unhold.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF5;
			this.unhold.Click += new System.EventHandler(this.unhold_Click);
			// 
			// transfer
			// 
			this.transfer.Index = 5;
			this.transfer.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF6;
			this.transfer.Click += new System.EventHandler(this.transfer_Click);
			// 
			// conference
			// 
			this.conference.Index = 6;
			this.conference.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF7;
			this.conference.Click += new System.EventHandler(this.conference_Click);
		}

		/// <summary>
		/// Initializes the CTI connections
		/// </summary>
		/// <returns></returns>
		public bool InitCTI(Desktop ownerForm, string agentNumber, ICti CtiHostedControl)
		{
			TelephonyProvider.CtiType ctiType = TelephonyProvider.CtiType.None;
			cti = null;

			try
			{
				this.ownerForm = ownerForm;

				// Get some configuration settings
				using (Options.OptionsClient wsClient = new Options.OptionsClient())
				{
					//ws.Url = UIConfiguration.ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
					//ws.Credentials = Utils.GetCredentials();
					//ws.PreAuthenticate = true;
					string Url = UIConfiguration.ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
					System.ServiceModel.EndpointAddress optionsAddress = new System.ServiceModel.EndpointAddress(Url);
					wsClient.Endpoint.Address = optionsAddress;
					wsClient.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();
					
					try
					{
						ctiType = (TelephonyProvider.CtiType)Convert.ToInt32(wsClient.GetOptionSetting("CtiType"));
					}
					catch (System.Net.WebException wex)
					{
						// Log the error but proceed
						Logging.Error(Application.ProductName, localize.COMMON_MSG_IIS_ERROR + "\n\n" + localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, wex);
					}
					catch (Exception exp)
					{
						if (exp.Message.IndexOf(localize.COMMON_ERR_SQL_CONNECTION) >= 0)
						{
							Logging.Error(Application.ProductName, localize.DESKTOP_ERR_UNABLE_CONNECT_SQL + "\n\n" + localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, exp);
						}
						else
						{
							Logging.Error(Application.ProductName, localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, exp);
						}
					}
					wsClient.Close();
				}

				// Do not load CTI if no agent
				// See if we are using CTI
				if (ctiType == TelephonyProvider.CtiType.None ||
					agentNumber == null || agentNumber == String.Empty)
				{
					return false;
				}

				// Initialize the CTI class
				cti = CtiHostedControl.InitializeCti();
				//cti.Debug = Logging.Debug;

				// If we can connect to a telephony provider
				if (CtiHostedControl.Cti != null)
				{
					bool ctiLogin = CtiHostedControl.Init(ctiType);

					// If agentNumber is null, then we aren't supporting CTI.
					if (ctiLogin && agentNumber != null && agentNumber != String.Empty)
					{
						// Warn user in case we forget to turn this flag off
						if (cti.Debug)
						{
							Logging.Warn(Application.ProductName, localize.DESKTOP_CTI_DEBUG_ENABLED);
						}

						myLine = CtiHostedControl.InitializeLine(agentNumber);
						if (myLine == null)
						{
							Logging.Error(Application.ProductName, localize.DESKTOP_ERR_UNABLE_CONFIGURE_PHONE);
						}
					}
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_INIT_TELEPHONY, exp);
			}
			// True for success
			return cti != null;
		}

		/// <summary>
		/// Runs when the menu appears.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPopup(EventArgs e)
		{
			modifyPhoneMenu();
			base.OnPopup(e);
		}

		/// <summary>
		/// This modifies the phone menu based on the operations that can currently be done.
		/// </summary>
		public void modifyPhoneMenu()
		{
			try
			{
				// Set the initial state of the softphone controls based on if we have
				// a phone to use.
				dial.Enabled = false;
				answer.Enabled = false;
				hold.Enabled = false;
				unhold.Enabled = false;
				transfer.Enabled = false;
				conference.Enabled = false;
				hangup.Enabled = false;

				if (myLine != null)
				{
					dial.Enabled = true;
					dial.DefaultItem = true;

					// See which commands are possible for the given set of calls
					foreach (CallClassProvider call in myLine.Calls)
					{
						if (call.CanAnswer())
						{
							answer.Enabled = true;
							dial.DefaultItem = false;
							answer.DefaultItem = true;
						}

						if (call.CanHangup())
						{
							hangup.Enabled = true;
						}

						if (call.CanHold())
						{
							hold.Enabled = true;
						}

						if (call.CanUnhold())
						{
							unhold.Enabled = true;
						}

						if (call.CanTransfer())
						{
							transfer.Enabled = true;
						}

						if (call.CanConference())
						{
							conference.Enabled = true;
						}
					}
				}
			}
			catch (Exception exp)
			{
				Logging.Error(localize.DESKTOP_MODULE_NAME, localize.DESKTOP_MODIFY_PHONE_MENU, exp);
			}
		}

		public void dial_Click(object sender, System.EventArgs e)
		{
			bool wasReady = false;

			try
			{
				if (myLine != null)
				{
					if (ownerForm.MyState == Lookup.LookupProviderPresenceState.Ready)
					{
						wasReady = true;
						ownerForm.SetAgentState(Lookup.LookupProviderPresenceState.Busy, Lookup.LookupProviderPresenceState.Busy.ToString());
					}

					using (GetNumber dlg = new GetNumber(localize.DESKTOP_GETNUMBER_DIAL))
					{
						if (dlg.ShowDialog(ownerForm) == System.Windows.Forms.DialogResult.OK)
						{
							// place any call on hold that can be held
							hold_Click(sender, e);
							CallClassProvider call = myLine.MakeCall(dlg.PhoneNumber, 0);

							// If we know the called party's name, save it.
							if (call != null)
							{
								call.UserTag = dlg.PhoneName;
							}
						}
						else if (wasReady)
						{
							ownerForm.SetAgentState(Lookup.LookupProviderPresenceState.Ready, null);
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_MAKING_CALL, exp);
			}
		}

		/// <summary>
		/// Handle a request via the UI to answer a call.  If there is more than one
		/// call ringing, a selection dialog is displayed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void answer_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to answer
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Answer"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}
						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							// place any call on hold that can be held
							hold_Click(sender, e);
							call.Answer();
							ownerForm.AllowLookup = true;
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_MAKING_CALL, exp);
			}
		}

		public void hangup_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to hangup, not likely to have a choice,
					// but coded just in case some switch permits it.
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Hangup"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}
						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							call.Hangup();
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_HANGUP_PHONE, exp);
			}
		}

		/// <summary>
		/// Places a call on hold.  A dialog is shown to pick a call
		/// if there are more than one call that can be placed on hold.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void hold_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to place on hold
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Hold"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}
						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							call.Hold();
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_PLACING_HOLD, exp);
			}
		}

		public void unhold_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to retrieve
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Unhold"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}

						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							// place any call on hold that can be held
							hold_Click(sender, e);
							call.Unhold();
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_TAKING_CALLOFF_HOLD, exp);
			}
		}

		private void transfer_Click(object sender, System.EventArgs e)
		{
			int transferToAgentID = -1;

			try
			{
				if (myLine != null)
				{
					// Find call that can be transferred
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Transfer"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}

						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							using (GetNumber numberDlg = new GetNumber("Transfer"))
							{
								if (numberDlg.ShowDialog(ownerForm) == DialogResult.OK)
								{
									try
									{
										// See if this is being transferred to an agent
										transferToAgentID = ownerForm.agentState.GetAgentIDFromPhoneNumber(numberDlg.PhoneNumber);

										if (transferToAgentID != -1 && ownerForm.SessionManager.ActiveSession != null)
										{
											string state = ownerForm.SessionManager.ActiveSession.Save(true);
											if (state != null && state != String.Empty)
											{
												ownerForm.agentState.SetSessionTransferInformation(transferToAgentID, call.CallerNumber, ownerForm.AgentID, state);
											}
										}

										// Make sure the transfer works
										if (call.Transfer(numberDlg.PhoneNumber) < 0)
										{
											// if anything failed, clear the session transfer info
											if (transferToAgentID != -1)
											{
												ownerForm.agentState.SetSessionTransferInformation(transferToAgentID, "", ownerForm.AgentID, "");
											}
										}
										// Transfer is probably working
										else
										{
											// Ask in case the agent is expected to do wrap up or
											//   in case the call did not really transfer.
											// Don't do this if there is only a global session.
											if (ownerForm.SessionManager.ActiveSession != null &&
												!ownerForm.SessionManager.ActiveSession.Global &&
												MessageBox.Show(ownerForm, localize.DESKTOP_CLOSE_AFTER_TRANSFER, Application.ProductName,
												MessageBoxButtons.YesNo) == DialogResult.Yes)
											{
												ownerForm.SessionManager.CloseSession(ownerForm.SessionManager.ActiveSession, false);
											}
										}
									}
									catch (Exception exp)
									{
										Logging.Error(Application.ProductName, localize.DESKTOP_ERR_TRANSFERRING_CALL, exp);

										// if anything failed, clear the session transfer info
										if (transferToAgentID != -1)
										{
											ownerForm.agentState.SetSessionTransferInformation(transferToAgentID, "", ownerForm.AgentID, "");
										}
									} // try...catch
								} // if ( numberDlg.ShowDialog( ownerForm ) == DialogResult.OK )
							} // using ( GetNumber numberDlg = new GetNumber( "Transfer" ) )
						} // if ( call != null )
					} // using ( SelectCallDlg dlg = new SelectCallDlg( myLine, "&Transfer" ) )
				}
				else
				{
					Logging.Error(this.ToString(), localize.DESKTOP_MSG_NO_CONFIGURED_PHONE);
				}
			}
			catch (Exception exp)
			{
				if (exp.Message.IndexOf(localize.DESKTOP_MSG_SQL_EXIST) >= 0)
				{
					throw;
				}
				if (exp.Message.IndexOf(localize.COMMON_MSG_IIS_ERROR) >= 0)
				{
					throw;
				}
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_TRANSFERRING_CALL, exp);
			}
		}

		public void conference_Click(object sender, System.EventArgs e)
		{
			CallClassProvider secondCall;

			try
			{
				if (myLine != null)
				{
					// Find first call that can be conferenced
					// TODO: add to UI a way to select which call
					foreach (CallClassProvider call in myLine.Calls)
					{
						if (call.CanConference())
						{
							using (GetNumber dlg = new GetNumber(localize.DESKTOP_GETNUMBER_CONFERENCE))
							{
								if (dlg.ShowDialog(ownerForm) == DialogResult.OK)
								{
									call.Hold();
									secondCall = myLine.MakeCall(dlg.PhoneNumber, 0);
									if (secondCall != null)
									{
										secondCall.Conference(call);
									}
									else
									{
										Logging.Error(Application.ProductName, localize.DESKTOP_MSG_UNABLE_CREATE_CONF);
									}
								} //  if ( dlg.ShowDialog(ownerForm) == DialogResult.OK )
							} // using ( GetNumber dlg = new GetNumber( localize.DESKTOP_GETNUMBER_CONFERENCE ) )
							break;
						} // if ( call.CanConference() )
					} // foreach ( CallClassProvider call in myLine.Calls )
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_CONFERENCING_CALL, exp);
			}
		}
	}


	/// <summary>
	/// Module containing the phone menu handling code for the agent desktop.
	/// </summary>
	/// <remarks>
	/// If this needs to be removed for a customer engagement, an easier and less invasive
	/// method to do so is to disable CTI hosted control
	/// </remarks>
	public class PhoneMenuToolBar : ContextMenu
	{
		#region Variables
		// Privates
		private System.Windows.Forms.MenuItem dial;
		private System.Windows.Forms.MenuItem hangup;
		private System.Windows.Forms.MenuItem transfer;
		private System.Windows.Forms.MenuItem conference;
		private System.Windows.Forms.MenuItem answer;
		public System.Windows.Forms.MenuItem hold;
		public System.Windows.Forms.MenuItem unhold;
		private Process remoteTelephonyProcess = null;
		private TelephonyProvider cti = null;
		private LineClassProvider myLine = null;
		private DesktopToolBar ownerForm = null;
		#endregion

		#region Properties
		public TelephonyProvider Cti
		{
			get { return cti; }
		}

		public LineClassProvider MyLine
		{
			get { return myLine; }
		}

		public Process RemoteTelephonyProcess
		{
			get { return remoteTelephonyProcess; }
		}
		#endregion

		/// <summary>
		/// PhoneMenu constructor
		/// </summary>
		public PhoneMenuToolBar()
		{
			this.dial = new System.Windows.Forms.MenuItem();
			this.answer = new System.Windows.Forms.MenuItem();
			this.hangup = new System.Windows.Forms.MenuItem();
			this.hold = new System.Windows.Forms.MenuItem();
			this.unhold = new System.Windows.Forms.MenuItem();
			this.transfer = new System.Windows.Forms.MenuItem();
			this.conference = new System.Windows.Forms.MenuItem();

			this.dial.Text = localize.DESKTOP_DIAL_TEXT;
			this.answer.Text = localize.DESKTOP_ANSWER_TEXT;
			this.hangup.Text = localize.DESKTOP_HANGUP_TEXT;
			this.hold.Text = localize.DESKTOP_HOLD_TEXT;
			this.unhold.Text = localize.DESKTOP_UNHOLD_TEXT;
			this.transfer.Text = localize.DESKTOP_TRANSFER_TEXT;
			this.conference.Text = localize.DESKTOP_CONFERENCE_TEXT;

			// 
			// phoneMenu
			// 
			this.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																			this.dial,
																			this.answer,
																			this.hangup,
																			this.hold,
																			this.unhold,
																			this.transfer,
																			this.conference});
			// 
			// dial
			// 
			this.dial.DefaultItem = true;
			this.dial.Index = 0;
			this.dial.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF1;
			this.dial.Click += new System.EventHandler(this.dial_Click);
			// 
			// answer
			// 
			this.answer.Index = 1;
			this.answer.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF2;
			this.answer.Click += new System.EventHandler(this.answer_Click);
			// 
			// hangup
			// 
			this.hangup.Index = 2;
			this.hangup.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF3;
			this.hangup.Click += new System.EventHandler(this.hangup_Click);
			// 
			// hold
			// 
			this.hold.Index = 3;
			this.hold.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF4;
			this.hold.Click += new System.EventHandler(this.hold_Click);
			// 
			// unhold
			// 
			this.unhold.Index = 4;
			this.unhold.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF5;
			this.unhold.Click += new System.EventHandler(this.unhold_Click);
			// 
			// transfer
			// 
			this.transfer.Index = 5;
			this.transfer.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF6;
			this.transfer.Click += new System.EventHandler(this.transfer_Click);
			// 
			// conference
			// 
			this.conference.Index = 6;
			this.conference.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF7;
			this.conference.Click += new System.EventHandler(this.conference_Click);
		}

		/// <summary>
		/// Initializes the CTI connections
		/// </summary>
		/// <returns></returns>
		public bool InitCTI(DesktopToolBar ownerForm, string agentNumber, ICti CtiHostedControl)
		{
			TelephonyProvider.CtiType ctiType = TelephonyProvider.CtiType.None;
			cti = null;

			try
			{
				this.ownerForm = ownerForm;

				// Get some configuration settings
				using (Options.OptionsClient wsClient = new Options.OptionsClient())
				{
					//ws.Url = UIConfiguration.ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
					//ws.Credentials = Utils.GetCredentials();
					//ws.PreAuthenticate = true;

					string Url = UIConfiguration.ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
					System.ServiceModel.EndpointAddress optionsAddress = new System.ServiceModel.EndpointAddress(Url);
					wsClient.Endpoint.Address = optionsAddress;
					wsClient.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

					try
					{
						ctiType = (TelephonyProvider.CtiType)Convert.ToInt32(wsClient.GetOptionSetting("CtiType"));
					}
					catch (System.Net.WebException wex)
					{
						// Log the error but proceed
						Logging.Error(Application.ProductName, localize.COMMON_MSG_IIS_ERROR + "\n\n" + localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, wex);
					}
					catch (Exception exp)
					{
						if (exp.Message.IndexOf(localize.COMMON_ERR_SQL_CONNECTION) >= 0)
						{
							Logging.Error(Application.ProductName, localize.DESKTOP_ERR_UNABLE_CONNECT_SQL + "\n\n" + localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, exp);
						}
						else
						{
							Logging.Error(Application.ProductName, localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, exp);
						}
					}
					wsClient.Close();
				}

				// Do not load CTI if no agent
				// See if we are using CTI
				if (ctiType == TelephonyProvider.CtiType.None ||
					agentNumber == null || agentNumber == String.Empty)
				{
					return false;
				}

				// Initialize the CTI class
				cti = CtiHostedControl.InitializeCti();
				//cti.Debug = Logging.Debug;

				// If we can connect to a telephony provider
				if (CtiHostedControl.Cti != null)
				{
					bool ctiLogin = CtiHostedControl.Init(ctiType);

					// If agentNumber is null, then we aren't supporting CTI.
					if (ctiLogin && agentNumber != null && agentNumber != String.Empty)
					{
						// Warn user in case we forget to turn this flag off
						if (cti.Debug)
						{
							Logging.Warn(Application.ProductName, localize.DESKTOP_CTI_DEBUG_ENABLED);
						}

						myLine = CtiHostedControl.InitializeLine(agentNumber);
						if (myLine == null)
						{
							Logging.Error(Application.ProductName, localize.DESKTOP_ERR_UNABLE_CONFIGURE_PHONE);
						}
					}
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_INIT_TELEPHONY, exp);
			}
			// True for success
			return cti != null;
		}

		/// <summary>
		/// Runs when the menu appears.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPopup(EventArgs e)
		{
			modifyPhoneMenu();
			base.OnPopup(e);
		}

		/// <summary>
		/// This modifies the phone menu based on the operations that can currently be done.
		/// </summary>
		public void modifyPhoneMenu()
		{
			try
			{
				// Set the initial state of the softphone controls based on if we have
				// a phone to use.
				dial.Enabled = false;
				answer.Enabled = false;
				hold.Enabled = false;
				unhold.Enabled = false;
				transfer.Enabled = false;
				conference.Enabled = false;
				hangup.Enabled = false;

				if (myLine != null)
				{
					dial.Enabled = true;
					dial.DefaultItem = true;

					// See which commands are possible for the given set of calls
					foreach (CallClassProvider call in myLine.Calls)
					{
						if (call.CanAnswer())
						{
							answer.Enabled = true;
							dial.DefaultItem = false;
							answer.DefaultItem = true;
						}

						if (call.CanHangup())
						{
							hangup.Enabled = true;
						}

						if (call.CanHold())
						{
							hold.Enabled = true;
						}

						if (call.CanUnhold())
						{
							unhold.Enabled = true;
						}

						if (call.CanTransfer())
						{
							transfer.Enabled = true;
						}

						if (call.CanConference())
						{
							conference.Enabled = true;
						}
					}
				}
			}
			catch (Exception exp)
			{
				Logging.Error(localize.DESKTOP_MODULE_NAME, localize.DESKTOP_MODIFY_PHONE_MENU, exp);
			}
		}

		public void dial_Click(object sender, System.EventArgs e)
		{
			bool wasReady = false;

			try
			{
				if (myLine != null)
				{
					if (ownerForm.MyState == Lookup.LookupProviderPresenceState.Ready)
					{
						wasReady = true;
						ownerForm.SetAgentState(Lookup.LookupProviderPresenceState.Busy, Lookup.LookupProviderPresenceState.Busy.ToString());
					}

					using (GetNumber dlg = new GetNumber(localize.DESKTOP_GETNUMBER_DIAL))
					{
						if (dlg.ShowDialog(ownerForm) == System.Windows.Forms.DialogResult.OK)
						{
							// place any call on hold that can be held
							hold_Click(sender, e);
							CallClassProvider call = myLine.MakeCall(dlg.PhoneNumber, 0);

							// If we know the called party's name, save it.
							if (call != null)
							{
								call.UserTag = dlg.PhoneName;
							}
						}
						else if (wasReady)
						{
							ownerForm.SetAgentState(Lookup.LookupProviderPresenceState.Ready, null);
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_MAKING_CALL, exp);
			}
		}

		/// <summary>
		/// Handle a request via the UI to answer a call.  If there is more than one
		/// call ringing, a selection dialog is displayed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void answer_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to answer
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Answer"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}
						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							// place any call on hold that can be held
							hold_Click(sender, e);
							call.Answer();
							ownerForm.AllowLookup = true;
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_MAKING_CALL, exp);
			}
		}

		public void hangup_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to hangup, not likely to have a choice,
					// but coded just in case some switch permits it.
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Hangup"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}
						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							call.Hangup();
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_HANGUP_PHONE, exp);
			}
		}

		/// <summary>
		/// Places a call on hold.  A dialog is shown to pick a call
		/// if there are more than one call that can be placed on hold.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void hold_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to place on hold
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Hold"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}
						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							call.Hold();
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_PLACING_HOLD, exp);
			}
		}

		public void unhold_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (myLine != null)
				{
					// Pick a call to retrieve
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Unhold"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}

						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							// place any call on hold that can be held
							hold_Click(sender, e);
							call.Unhold();
						}
					}
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE, string.Empty);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_TAKING_CALLOFF_HOLD, exp);
			}
		}

		private void transfer_Click(object sender, System.EventArgs e)
		{
			int transferToAgentID = -1;

			try
			{
				if (myLine != null)
				{
					// Find call that can be transferred
					using (SelectCallDlg dlg = new SelectCallDlg(myLine, "&Transfer"))
					{
						if (dlg.MatchingCalls > 1)
						{
							dlg.ShowDialog(ownerForm);
						}

						CallClassProvider call = dlg.SelectedCall;
						if (call != null)
						{
							using (GetNumber numberDlg = new GetNumber("Transfer"))
							{
								if (numberDlg.ShowDialog(ownerForm) == DialogResult.OK)
								{
									try
									{
										// See if this is being transferred to an agent
										transferToAgentID = ownerForm.agentState.GetAgentIDFromPhoneNumber(numberDlg.PhoneNumber);

										if (transferToAgentID != -1 && ownerForm.SessionManager.ActiveSession != null)
										{
											string state = ownerForm.SessionManager.ActiveSession.Save(true);
											if (state != null && state != String.Empty)
											{
												ownerForm.agentState.SetSessionTransferInformation(transferToAgentID, call.CallerNumber, ownerForm.AgentID, state);
											}
										}

										// Make sure the transfer works
										if (call.Transfer(numberDlg.PhoneNumber) < 0)
										{
											// if anything failed, clear the session transfer info
											if (transferToAgentID != -1)
											{
												ownerForm.agentState.SetSessionTransferInformation(transferToAgentID, "", ownerForm.AgentID, "");
											}
										}
										// Transfer is probably working
										else
										{
											// Ask in case the agent is expected to do wrap up or
											//   in case the call did not really transfer.
											// Don't do this if there is only a global session.
											if (ownerForm.SessionManager.ActiveSession != null &&
												!ownerForm.SessionManager.ActiveSession.Global &&
												MessageBox.Show(ownerForm, localize.DESKTOP_CLOSE_AFTER_TRANSFER, Application.ProductName,
												MessageBoxButtons.YesNo) == DialogResult.Yes)
											{
												ownerForm.SessionManager.CloseSession(ownerForm.SessionManager.ActiveSession, false);
											}
										}
									}
									catch (Exception exp)
									{
										Logging.Error(Application.ProductName, localize.DESKTOP_ERR_TRANSFERRING_CALL, exp);

										// if anything failed, clear the session transfer info
										if (transferToAgentID != -1)
										{
											ownerForm.agentState.SetSessionTransferInformation(transferToAgentID, "", ownerForm.AgentID, "");
										}
									} // try...catch
								} // if ( numberDlg.ShowDialog( ownerForm ) == DialogResult.OK )
							} // using ( GetNumber numberDlg = new GetNumber( "Transfer" ) )
						} // if ( call != null )
					} // using ( SelectCallDlg dlg = new SelectCallDlg( myLine, "&Transfer" ) )
				}
				else
				{
					Logging.Error(this.ToString(), localize.DESKTOP_MSG_NO_CONFIGURED_PHONE);
				}
			}
			catch (Exception exp)
			{
				if (exp.Message.IndexOf(localize.DESKTOP_MSG_SQL_EXIST) >= 0)
				{
					throw;
				}
				if (exp.Message.IndexOf(localize.COMMON_MSG_IIS_ERROR) >= 0)
				{
					throw;
				}
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_TRANSFERRING_CALL, exp);
			}
		}

		public void conference_Click(object sender, System.EventArgs e)
		{
			CallClassProvider secondCall;

			try
			{
				if (myLine != null)
				{
					// Find first call that can be conferenced
					// TODO: add to UI a way to select which call
					foreach (CallClassProvider call in myLine.Calls)
					{
						if (call.CanConference())
						{
							using (GetNumber dlg = new GetNumber(localize.DESKTOP_GETNUMBER_CONFERENCE))
							{
								if (dlg.ShowDialog(ownerForm) == DialogResult.OK)
								{
									call.Hold();
									secondCall = myLine.MakeCall(dlg.PhoneNumber, 0);
									if (secondCall != null)
									{
										secondCall.Conference(call);
									}
									else
									{
										Logging.Error(Application.ProductName, localize.DESKTOP_MSG_UNABLE_CREATE_CONF);
									}
								} //  if ( dlg.ShowDialog(ownerForm) == DialogResult.OK )
							} // using ( GetNumber dlg = new GetNumber( localize.DESKTOP_GETNUMBER_CONFERENCE ) )
							break;
						} // if ( call.CanConference() )
					} // foreach ( CallClassProvider call in myLine.Calls )
				}
				else
				{
					Logging.Error(Application.ProductName, localize.DESKTOP_MSG_NO_CONFIGURED_PHONE);
				}
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_ERR_CONFERENCING_CALL, exp);
			}
		}
	}

}