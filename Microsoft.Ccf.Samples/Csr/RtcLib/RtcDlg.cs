//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// RtcDlg.cs
//
// Revisions:
//     May 2003      v1.0  release
//     December 2004 v1.02 release
//
//===============================================================================

using System;
using System.Xml;
using System.Windows.Forms;

using Microsoft.Ccf.Csr.Rtc;
using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.Csr.RtcLib
{
	/// <summary>
	/// Summary description for RtcDlg.
	/// </summary>
	public class RtcDlg : System.Windows.Forms.Form
	{
		/// <summary>
		/// Used to block making multiple outgoing RTC requests
		/// </summary>
		public static bool ExistingRequest = false;

		private string sipAddress;
		private string firstName, lastName;
		private string subject = String.Empty;
		private string context = String.Empty;

		private Microsoft.Ccf.Csr.Rtc.RtcClient   rtcClient;
		private Microsoft.Ccf.Csr.Rtc.Session     activeSession;
		private Microsoft.Ccf.Csr.Rtc.Participant participant;

		private System.Windows.Forms.Label header;
		private System.Windows.Forms.Panel bottomPanel;
		private System.Windows.Forms.RichTextBox sendText;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.RichTextBox conversationText;
		private System.Windows.Forms.Button info;
		private System.Windows.Forms.RichTextBox information;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public RtcDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// For creating a dialog to handle an incoming IM conversation
		/// </summary>
		/// <param name="rtcClient"></param>
		/// <param name="activeSession"></param>
		public RtcDlg( Microsoft.Ccf.Csr.Rtc.RtcClient rtcClient, Microsoft.Ccf.Csr.Rtc.Session activeSession )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.rtcClient = rtcClient;

			sipAddress = null;
			firstName = lastName = "";

			header.Text = "";

			// make a sound indicating a new IM conversation
			try
			{
				rtcClient.PlayRing( RingType.Message, true );
			}
			catch 
			{
				// OK to ignore this exception as it is caused by absence of working audio device.
			}

			// save the active session
			this.activeSession = activeSession;

			statusBar.Text = localize.RTCDLG_MSG_ANSWERING;
			information.Text = localize.RTCDLG_INFO;
		}

		/// <summary>
		/// For creating a new RTC conversation to someone else
		/// </summary>
		/// <param name="rtcClient"></param>
		/// <param name="sipAddress"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="subject"></param>
		/// <param name="context"></param>
		public RtcDlg( Microsoft.Ccf.Csr.Rtc.RtcClient rtcClient, string sipAddress, string firstName, string lastName, string subject, string context )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.rtcClient = rtcClient;

			this.sipAddress = sipAddress;
			this.lastName = lastName;
			this.firstName = firstName;
			this.subject = subject;
			this.context = context;

			Text = localize.RTCDLG_TEXTMSG_ASSISTANCE + " " + subject;
			header.Text = localize.RTCDLG_TEXTMSG_TO + " " + firstName + " " + lastName;

			statusBar.Text = "";

			// set margins
			RtcDlg_SizeChanged( this, null );
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(RtcDlg));
			this.header = new System.Windows.Forms.Label();
			this.bottomPanel = new System.Windows.Forms.Panel();
			this.statusBar = new System.Windows.Forms.StatusBar();
			this.btnSend = new System.Windows.Forms.Button();
			this.sendText = new System.Windows.Forms.RichTextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.conversationText = new System.Windows.Forms.RichTextBox();
			this.info = new System.Windows.Forms.Button();
			this.information = new System.Windows.Forms.RichTextBox();
			this.bottomPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// header
			// 
			this.header.BackColor = System.Drawing.Color.AliceBlue;
			this.header.Dock = System.Windows.Forms.DockStyle.Top;
			this.header.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.header.Location = new System.Drawing.Point(0, 94);
			this.header.Name = "header";
			this.header.Size = new System.Drawing.Size(292, 18);
			this.header.TabIndex = 1;
			this.header.Text = "header";
			this.header.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// bottomPanel
			// 
			this.bottomPanel.Controls.Add(this.statusBar);
			this.bottomPanel.Controls.Add(this.btnSend);
			this.bottomPanel.Controls.Add(this.sendText);
			this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomPanel.Location = new System.Drawing.Point(0, 246);
			this.bottomPanel.Name = "bottomPanel";
			this.bottomPanel.Size = new System.Drawing.Size(292, 80);
			this.bottomPanel.TabIndex = 0;
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 64);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(292, 16);
			this.statusBar.TabIndex = 2;
			this.statusBar.Text = "statusBar";
			// 
			// btnSend
			// 
			this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSend.Location = new System.Drawing.Point(224, 8);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(64, 48);
			this.btnSend.TabIndex = 1;
			this.btnSend.Text = "&Send";
			this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// sendText
			// 
			this.sendText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.sendText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.sendText.Location = new System.Drawing.Point(0, 0);
			this.sendText.MaxLength = 2048;
			this.sendText.Name = "sendText";
			this.sendText.RightMargin = 190;
			this.sendText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
			this.sendText.Size = new System.Drawing.Size(292, 80);
			this.sendText.TabIndex = 0;
			this.sendText.Text = "sendText";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(232, 0);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(1, 1);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// conversationText
			// 
			this.conversationText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Top)));
			this.conversationText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.conversationText.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.conversationText.Location = new System.Drawing.Point(0, 98);
			this.conversationText.MaxLength = 32000;
			this.conversationText.Name = "conversationText";
			this.conversationText.ReadOnly = true;
			this.conversationText.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.conversationText.Size = new System.Drawing.Size(292, 138);
			this.conversationText.TabIndex = 4;
			this.conversationText.Text = "conversationText";
			// 
			// info
			// 
			this.info.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.info.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.info.Location = new System.Drawing.Point(248, 0);
			this.info.Name = "info";
			this.info.Size = new System.Drawing.Size(40, 18);
			this.info.TabIndex = 5;
			this.info.Text = "Info";
			this.info.Click += new System.EventHandler(this.info_Click);
			// 
			// information
			// 
			this.information.Dock = System.Windows.Forms.DockStyle.Top;
			this.information.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.information.Location = new System.Drawing.Point(0, 0);
			this.information.MaxLength = 32000;
			this.information.Name = "information";
			this.information.ReadOnly = true;
			this.information.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.information.Size = new System.Drawing.Size(292, 94);
			this.information.TabIndex = 6;
			this.information.Text = "information";
			this.information.Visible = false;
			// 
			// RtcDlg
			// 
			this.AcceptButton = this.btnSend;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(292, 326);
			this.Controls.Add(this.info);
			this.Controls.Add(this.conversationText);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.bottomPanel);
			this.Controls.Add(this.information);
			this.Controls.Add(this.header);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimumSize = new System.Drawing.Size(285, 340);
			this.Name = "RtcDlg";
			this.Text = "RtcDlg";
			this.TopMost = true;
			this.Closing += new System.ComponentModel.CancelEventHandler(this.RtcDlg_Closing);
			this.SizeChanged += new System.EventHandler(this.RtcDlg_SizeChanged);
			this.Load += new System.EventHandler(this.RtcDlg_Load);
			this.bottomPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Fired when a session enters the connected state.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void activeSession_Connected(object sender,SessionEventArgs e)
		{
			if ( activeSession != null )
			{
				lock ( activeSession )
				{
					activeSession.MessageReceived += new MessagingEventHandler( activeSession_MessageReceived );
				}

				// assuming only 1 participant
				statusBar.Text = " ";
				foreach ( Microsoft.Ccf.Csr.Rtc.Participant party in e.Session.Participants )
				{
					participant = party;
					if ( sipAddress == null )
						sipAddress = party.UserURI;

					// Put something in the header if we don't already have the info
					if ( header.Text == "" )
						header.Text = localize.RTCDLG_TEXTMSG_TO + " " + party.Name;

					statusBar.Text += party.Name + "  ";  // " (" + party.UserURI + ")  ";
				}
			}
		}

		/// <summary>
		/// Fired when a session enters the disconnected state.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void activeSession_Disconnected(object sender,SessionEventArgs e)
		{
			if ( activeSession != null && e.Session.GetHashCode() == activeSession.GetHashCode() )
			{
				activeSession.Terminate();
				activeSession = null;
			}

			statusBar.Text = localize.RTCDLG_TEXTMSG_CONVERSATION_CLOSED;
		}


		private void RtcDlg_Load(object sender, System.EventArgs e)
		{
			//conversationText.Rtf = "\\rtf\\unicode";
			conversationText.Text = "";
			sendText.Text = "";

			if ( null != activeSession )
			{
				//
				// Answer the incoming session
				//
				activeSession.Connected    += new Microsoft.Ccf.Csr.Rtc.SessionEventHandler(this.activeSession_Connected);
				activeSession.Disconnected += new Microsoft.Ccf.Csr.Rtc.SessionEventHandler(this.activeSession_Disconnected);
				try
				{
					activeSession.Answer();
				}
				catch {} // catch unneeded errors from Answer()
			}

			// set margins
			RtcDlg_SizeChanged( this, null );

			// ExistingRequest is static.
			// This lets the code determine if there is an existing RTC request and
			// to remove that flag when the existing request is complete.
			ExistingRequest = true;

			sendText.Focus();
			Show();
			Activate();
		}

		/// <summary>
		/// Called when the IM conversation is ended.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RtcDlg_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if ( null != activeSession )
				{
					lock ( activeSession )
					{
						activeSession.Terminate();
						activeSession = null;
					}
				}
			}
			finally
			{
				// indicate another outbound request may be made
				ExistingRequest = false;
			}
		}


		/// <summary>
		/// Force the conversation window to scroll to the bottom where any new text
		/// is.
		/// </summary>
		private void scrollToBottom()
		{
			// dumb way to force the richedit control to scroll to the bottom
			int len = conversationText.Text.Length;
			conversationText.Focus();
			conversationText.Select( len, len );
			conversationText.ScrollToCaret();
		}

		/// <summary>
		/// Event which fires when a message is received from elsewhere.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void activeSession_MessageReceived(object sender, MessagingEventArgs e)
		{
			string msg;

			try
			{
				msg = e.Message;
				if ( msg != null )
				{
					msg = msg.Trim();

					if ( msg.StartsWith( "<request>" ) )
					{
						try
						{
							System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
							System.Xml.XmlNode node;

							doc.LoadXml( msg );

							node = doc.SelectSingleNode( "request/subject" );
							if ( node != null )
							{
								this.subject = node.InnerText;
								this.Text = localize.RTCDLG_TEXTMSG_HELPREQUEST + " " + subject;
							}

							node = doc.SelectSingleNode( "request/context" );
							if ( node != null )
							{
								this.context = node.InnerXml;
							}
						}						
						catch
						{
							// just ignore an error
						}
					}
					else
					{
						// conversationText.Rtf += "\n\\b" + participant.Name + " says:\\b0\n  " + e.Message.Trim().Replace( @"\", @"\\" );
						conversationText.Text += System.Environment.NewLine + String.Format(localize.RTCDLG_TEXTMSG_SAYS, participant.Name, msg) + System.Environment.NewLine;
					}
				}

				scrollToBottom();
				sendText.Focus();
			}
			catch ( Exception exp )
			{
				conversationText.Text += localize.RTCDLG_ERRMSG_EXCEPTION + " " + exp.Message;
			}
		}


		private void RtcDlg_SizeChanged(object sender, System.EventArgs e)
		{
			int margin;

			margin = btnSend.Location.X - 4;
			if ( margin > 10 )
				sendText.RightMargin = margin;
		}


		private void btnSend_Click(object sender, System.EventArgs e)
		{
			try
			{
				string text = sendText.Text.Trim();
				if ( text != String.Empty )
				{
					lock ( this )
					{
						// If not connected, establish connection first.
						if ( activeSession == null )
						{
							if ( sipAddress == null || sipAddress == String.Empty )
							{
								Logging.Error( Application.ProductName, localize.RTCDLG_MSGBOX_MSG_UNABLE_REACH );
								return;
							}

							activeSession = rtcClient.CreateIMSession();
							if ( null != activeSession )
							{
								lock ( activeSession )
								{
									activeSession.Connected    += new Microsoft.Ccf.Csr.Rtc.SessionEventHandler(this.activeSession_Connected);
									activeSession.Disconnected += new Microsoft.Ccf.Csr.Rtc.SessionEventHandler(this.activeSession_Disconnected);

									activeSession.AddParticipant( sipAddress, firstName + " " + lastName );

									// first send some introductory info
									string xmlHeader;
									xmlHeader = "<request><subject>" + subject + "</subject><context>" + context + "</context></request>";

									// now send message
									activeSession.SendMessage( xmlHeader );
									activeSession.SendMessage( text );
								}
							}
						}

						else
						{
							lock ( activeSession )
							{
								// Connection will not actually take place until first message is sent
								activeSession.SendMessage( text );
							}
						}

						// conversationText.Rtf += "\r\n\\bYou say:\\b0\n  " + sendText.Text.Trim();
						conversationText.Text += localize.RTCDLG_TEXTMSG_YOUSAY + "  " + text;
						scrollToBottom();

						sendText.Text = String.Empty;
					}

					sendText.Focus();
				}
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.RTCDLG_ERRMSG_EXCEPTION, exp );
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}


		/// <summary>
		/// Shows additional information for the assisting agent to know about the
		/// customer.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void info_Click(object sender, System.EventArgs e)
		{
			this.information.Text = String.Empty;

			this.information.Visible = !this.information.Visible;
			if ( !this.information.Visible )
			{
				this.conversationText.Dock = DockStyle.Fill;
				this.info.Text = localize.RTCDLG_INFO;
				return;
			}

			this.info.Text = localize.RTCDLG_HIDE;

			try
			{
				XmlNode root = null;

				if ( context != null && context != String.Empty )
				{
					XmlDocument doc = new XmlDocument();
					doc.LoadXml( context );
					root = doc.SelectSingleNode( "descendant::CcfContext" );
				}

				string info = String.Empty;

				// See if there is any customer information
				if ( root == null || root.ChildNodes == null || root.ChildNodes.Count == 0 )
					info = "\r\n  No Customer Information";
				else
				{
					// Display the customer info
					foreach ( XmlNode node in root.ChildNodes )
					{
						// skip the customer authenticated field
						if ( node.Name.StartsWith( "CustomerAuth" ) )
							continue;

						string name = String.Format( "{0}:", node.Name );
						info += String.Format( "  {0,-10}\t {1,-10}\r\n",
							name, node.InnerText );
					}
				}

				this.information.Text = info;
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.RTCDLG_INFORMATION_ERROR, exp );
			}
		}
	}
}