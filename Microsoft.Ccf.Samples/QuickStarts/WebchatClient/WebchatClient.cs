//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
//================================================================================================================================

using System;
using System.Timers;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Common;

namespace Microsoft.Ccf.Samples.WebchatClient
{
	/// <summary>
	/// Summary description for InstantMessaging.
	/// </summary>
	public class WebchatClient : HostedControl, IHostedApplicationMultichannel
	{
		#region Variables
		// Privates
		private int agentID = 0;
		private int customerID = 1;
		private int webchatSessionID = 0;
		private bool firstSend = true;
		private string userName = "John";
		private string webchatReceiver = "Customer";
		private string webchatSender = "Agent";
		private string previousMessage = string.Empty;
		private System.Windows.Forms.TextBox displayTextbox;
		private System.Windows.Forms.TextBox messageTextbox;
		private System.Windows.Forms.Button sendButton;
		private Timer timer = new Timer();
		//private WebchatServer.WebchatServer wc = new WebchatServer.WebchatServer();
		private WebchatServer.WebchatServerClient wc = new WebchatServer.WebchatServerClient();
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		// Public events
		public event System.EventHandler Send;
		#endregion

		public WebchatClient(int appID, string appName, string initString) : base( appID, appName, initString )
		{
			this.Init();
		}

		public WebchatClient()
		{
			this.Init();
		}

		private void Init()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.sendButton.Enabled = false;
		}

		public override void Initialize()
		{
			//wc.Url = this.configurationReader.ReadSettings("Microsoft_Ccf_Samples_InstantMessaging_WebchatServer_WebchatServer");
			//wc.Credentials = System.Net.CredentialCache.DefaultCredentials;
			//wc.PreAuthenticate = true;
			string Url = this.configurationReader.ReadSettings("Microsoft_Ccf_Samples_InstantMessaging_WebchatServer_WebchatServer");
			System.ServiceModel.EndpointAddress wcAddress = new System.ServiceModel.EndpointAddress(Url);
			wc.Endpoint.Address = wcAddress;
			wc.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

			this.timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
			//this.timer.Tick+=new EventHandler(timer_Tick);
			this.timer.Enabled = false;
			this.timer.Interval = 2000; 
			base.Initialize();
		}

		public string WebchatReceiver
		{
			get { return this.webchatReceiver; }
			set { this.webchatReceiver = value; }
		}

		public string DisplayTextbox
		{
			get { return this.displayTextbox.Text; }
			set { this.displayTextbox.Text = value; }
		}

		public string MessageTextbox
		{
			get { return this.messageTextbox.Text; }
			set { this.messageTextbox.Text = value; }
		}

		public new int AgentID
		{
			get { return agentID; }
			set { agentID = value; }
		}

		public int CustomerID
		{
			get { return customerID; }
			set { customerID = value; }
		}

		public string WebchatSender
		{
			get { return webchatSender; }
			set { webchatSender = value; }
		}

		public int WebchatSessionID
		{
			get { return webchatSessionID; }
			set { webchatSessionID = value; }
		}

		public bool FirstSend
		{
			get { return firstSend; }
			set { firstSend = value; }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.displayTextbox = new System.Windows.Forms.TextBox();
			this.messageTextbox = new System.Windows.Forms.TextBox();
			this.sendButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// displayTextbox
			// 
			this.displayTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.displayTextbox.BackColor = System.Drawing.Color.White;
			this.displayTextbox.Enabled = false;
			this.displayTextbox.Location = new System.Drawing.Point(16, 16);
			this.displayTextbox.Multiline = true;
			this.displayTextbox.Name = "displayTextbox";
			this.displayTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.displayTextbox.Size = new System.Drawing.Size(392, 344);
			this.displayTextbox.TabIndex = 2;
			this.displayTextbox.Text = "";
			// 
			// messageTextbox
			// 
			this.messageTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.messageTextbox.Location = new System.Drawing.Point(16, 368);
			this.messageTextbox.Multiline = true;
			this.messageTextbox.Name = "messageTextbox";
			this.messageTextbox.Size = new System.Drawing.Size(272, 80);
			this.messageTextbox.TabIndex = 0;
			this.messageTextbox.Text = "";
			this.messageTextbox.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
			// 
			// sendButton
			// 
			this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.sendButton.Location = new System.Drawing.Point(304, 368);
			this.sendButton.Name = "sendButton";
			this.sendButton.Size = new System.Drawing.Size(104, 80);
			this.sendButton.TabIndex = 1;
			this.sendButton.Text = "&Send";
			this.sendButton.Click += new System.EventHandler(this.btnSend_Click);
			// 
			// InstantMessaging
			// 
			this.Controls.Add(this.sendButton);
			this.Controls.Add(this.messageTextbox);
			this.Controls.Add(this.displayTextbox);
			this.Icon = null;
			this.Name = "InstantMessaging";
			this.Size = new System.Drawing.Size(416, 464);
			this.ResumeLayout(false);

		}
		#endregion

		private void btnSend_Click(object sender, System.EventArgs e)
		{
			
			// TODO update this to take real name from channel or context
			string display = string.Empty;
			WebchatSessionID = wc.SendMessage(WebchatSessionID, AgentID, CustomerID, this.WebchatSender, this.MessageTextbox.Trim());
			if (this.Send != null)
			{
				this.Send(this, e);
			}
			if (this.DisplayTextbox != string.Empty)
			{
				display = System.Environment.NewLine;
			}
			display += "Agent: " + this.messageTextbox.Text;
			this.DisplayTextbox += display;
			this.sendButton.Enabled = false;
			// Only clear it after everything else is complete...
			this.messageTextbox.Text = string.Empty;
			this.firstSend = false;
		}

		private void txtMessage_TextChanged(object sender, System.EventArgs e)
		{
			this.sendButton.Enabled = true;
		}

		/// <summary>
		/// Opens Chat Session between Operator and the user who is waiting in the Queue
		/// </summary>
		/// <remarks>The sessionID may need to be converted for each channel as the data type is unknown.</remarks>
		/// <param name="sessionID">The key used to open a connection the channel.</param>
		/// <returns>A boolean is returned indicating success or failure.</returns>
		public bool Open(string sessionID)
		{
			try
			{
				this.WebchatSessionID = Convert.ToInt32(sessionID);
				this.CheckForMessage();
				this.timer.Enabled = true;
				return true;
			}
			catch (Exception ex)
			{
				Console.Write(ex);
				return false;
			}
		}

		/// <summary>
		/// Closes the Chat Session between the Agent and the User on that particular Chat Session
		/// </summary>
		/// <remarks>The sessionID may need to be converted for each channel as the data type is unknown.</remarks>
		/// <param name="sessionID">The key used to open a connection the channel.</param>
		/// <returns>A boolean is returned indicating success or failure.</returns>
		public bool Close(string sessionID)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Adds another agent/expert/supervisor to ongoing Chat Session
		/// </summary>
		/// <remarks>The sessionID may need to be converted for each channel as the data type is unknown.</remarks>
		/// <param name="sessionID">The key used to open a connection the channel.</param>
		/// <param name="newPartyUri">The URI indicating the party to add to the conference.</param>
		/// <returns>A boolean is returned indicating success or failure.</returns>
		public bool Conference(string sessionID, string newPartyUri)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Transfers current chat session to another Agent/Expert
		/// </summary>
		/// <remarks>The sessionID may need to be converted for each channel as the data type is unknown.</remarks>
		/// <param name="sessionID">The key used to open a connection the channel.</param>
		/// <param name="newPartyUri">The URI indicating the party to add to the conference.</param>
		/// <returns>A boolean is returned indicating success or failure.</returns>
		public bool Transfer(string sessionID, string newPartyUri)
		{
			throw new NotImplementedException();
		}

		private void CheckForMessage()
		{
			try
			{
				string display = string.Empty;
				string message = wc.ReadMessage(WebchatSessionID, this.webchatReceiver);
				// Only show new messages...this is just a simple way to keep the mock web chat
				// system from repeating
				if (message.Trim() != this.previousMessage.Trim())
				{
					if (this.DisplayTextbox != string.Empty)
					{
						display = System.Environment.NewLine;
					}
					display += this.userName + ": " + message;

					this.DisplayTextbox += display;
					this.previousMessage = message;
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex);
				this.timer.Enabled = true;
			}
		}

		private void timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			this.timer.Enabled = false;
			this.CheckForMessage();
			this.timer.Enabled = true;
		}
	}
}
