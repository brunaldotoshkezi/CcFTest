//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
//===================================================================================
//
// CCF Agent Desktop TOOLBAR user interface
//
// There are two reference User Interface's shipped with CCF.
//
// One is a tabbed interface (SDI) which has existed since CCF 1.0.  This is a
// standard Windows style application where the hosted applications are on
// tab pages within the larger CCF window.
//
// The other is a application bar interface where the hosted applications
// are all floating windows (MDI) and the CCF menu is a toolbar across the top of
// the agent's screen that partially hides when not in use.
//
// Both user interfaces support the same hosted applications, multiple sessions, CTI,
// Instant Messaging and other features.  They simply display it in different ways.
//
// To use this UI, change the "DesktopToolBar" appSettings value in the config 
// file to true.
//
//===================================================================================

// There is a bug in the designer.  To fix close button (if it shows up like "Cl...")
// You is to make sure the text is set BEFORE it is being added to the rightToolBar.
// So, in initializeComponent() CUT the closeSession section, where the text is set, 
// and PASTE it above BEFORE the rightToolBar section, where the closeSession button 
// is being added to the rightToolBar.  This can revert if you use the designer to modify
// the layout
// For more information see: http://groups.google.com/group/microsoft.public.dotnet.framework.windowsforms.controls/browse_thread/thread/6315fefbe26574b5/2041831cb308131b?lnk=st&q=toolbarbutton+text&rnum=20&hl=en#2041831cb308131b

#region Usings
using System;
using System.Net;
using System.ServiceModel;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Windows.Forms;
using System.DirectoryServices;
using System.Drawing.Drawing2D;
using System.Security.Principal;
using System.ComponentModel;

using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Csr.Rtc;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.DataAccessLayer;
using Microsoft.Ccf.Csr.Cti.Providers;
using Microsoft.Ccf.Adapter.CustomerWS;
using Microsoft.Ccf.Csr.UIConfiguration;
using Microsoft.Ccf.Adapter.Multichannel;
using Microsoft.Ccf.Samples.Csr.AgentDesktop.Lookup;
using Microsoft.Ccf.Samples.Csr.AgentDesktop.SkillsRouting;
using Microsoft.Ccf.Samples.HostedControlInterfaces;
using CustomerWS = Microsoft.Ccf.Adapter.CustomerWS;

using CTI = Microsoft.Ccf.Csr.Cti;
using Session = Microsoft.Ccf.Csr.Session;
using RTC = Microsoft.Ccf.Samples.Csr.RtcLib;
using Options = Microsoft.Ccf.Samples.Csr.AgentDesktop.Options;

using RTCCORELib;
using Microsoft.Ccf.Multichannel.AdapterManager.Common;
using Microsoft.Ccf.MultiChannelAdapters.Common;
#endregion

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// This is the main class which represents the Agent's desktop as a toolbar
	/// for integrating multiple applications and providing some tools
	/// to assist with their work.
	/// </summary>
	public class DesktopToolBar : UIConfiguration
	{
		#region Variables
		// Private constants
		const int NOTIFY_FOR_THIS_SESSION = 0;

		const int WTS_CONSOLE_CONNECT        = 0x1;
		const int WTS_CONSOLE_DISCONNECT     = 0x2;
//		const int WTS_REMOTE_CONNECT         = 0x3;
//		const int WTS_REMOTE_DISCONNECT      = 0x4;
//		const int WTS_SESSION_LOGON          = 0x5;
//		const int WTS_SESSION_LOGOFF         = 0x6;
		const int WTS_SESSION_LOCK           = 0x7;
		const int WTS_SESSION_UNLOCK         = 0x8;
//		const int WTS_SESSION_REMOTE_CONTROL = 0x9;

		static bool firstTime = true;
		static Lookup.LookupProviderPresenceState savedState = Lookup.LookupProviderPresenceState.Ready;

		// Mappings from IAD status to the multichannel providers status. Used to update status at provider when 
		// status at IAD is updated
		private static Hashtable clientToMCProvidersStatusMapping = new Hashtable();

		//Privates
		private const string adVersion = "3.0";
		private PhoneMenuToolBar phoneMenu;

		// Used to cache the last save so we don't hit the web service for no reason
		private string lastSavedSessions = String.Empty;
		// Channel information object
		private ChannelInformation ci = null;
		private string agentNumber = String.Empty;
		private TimeSpan  warnAgentCallIsTooLong;
		private System.Drawing.Image warningIcon;
		// Look up routine "handler"
		private doLookupDelegate doLookUpRoutine = null;
		// seconds between UI updates and polling of various services.
		private int secondsBetweenTickPolls = 0;
		private BindingList<Lookup.LookupProviderLookupEntry> PresenceStates;
		private ContactCenterStatistics.ContactCenterStatistics GetStats = null;
		private ContactCenterStatistics.ContactCenterStatisticsClient client;
		private CCFPanel hiddenPanel;	
		/// <summary>
		/// True if there is workflow manager
		/// </summary>
		private bool workflowExists = true;
		/// <summary>
		/// True if an exception is thrown while registering the client with the CCF Server
		/// </summary>
		private bool registerClientException = false;
		private CustomerClient customerLookup;
		private CustomerProviderCustomerRecord customer = null;
		private RoutingClient skillsRoutingService = new RoutingClient();
		private BindingList<Lookup.LookupProviderLookupEntry> Skills;
		private System.Windows.Forms.ImageList toolBarImageList;
		private System.ComponentModel.IContainer components;
		private bool loggingOut = false;
		private MyMessageFilter msgFilter;
		private System.Windows.Forms.Timer pollTimer;
		private System.Windows.Forms.Timer clockTimer;
		private System.Windows.Forms.ToolBar rightToolBar;
		private System.Windows.Forms.ToolBarButton rightSeparator;
		private System.Windows.Forms.ToolBarButton closeSession;
		private System.Windows.Forms.ToolBar leftToolBar;
		private System.Windows.Forms.ToolBarButton requestAssistance;
		private System.Windows.Forms.ToolBarButton separator1;
		private System.Windows.Forms.ToolBarButton phone;
		private System.Windows.Forms.ToolBarButton lookup;
		private System.Windows.Forms.ToolBarButton nonHostedApplications;
		private System.Windows.Forms.ToolBarButton separator2;
		private System.Windows.Forms.ToolBarButton separator3;
		private System.Windows.Forms.ToolBarButton help;
		private System.Windows.Forms.ToolBarButton dynamicApplications;
		private System.Windows.Forms.ToolBarButton status;
		private System.Windows.Forms.Panel toolPanel;
		private System.Windows.Forms.Button btnSessions;
		private System.Windows.Forms.Button btnWorkflow;
		private OwnerDrawLabel ticker;
		private OwnerDrawLabel info;
		private System.Windows.Forms.ToolBarButton toolBarButton1;
		private ImageButton exitCCF;
		// Cti hosted Control
		private Microsoft.Ccf.Csr.Cti.Providers.ICti ctiHostedControl = null;
		// Session Explorer hosted control
		private Microsoft.Ccf.Samples.HostedControlInterfaces.ISessionExplorer sessionExplorerControl = null;
		// Workflow hosted control
		private Microsoft.Ccf.Samples.HostedControlInterfaces.IWorkFlow workFlowControl = null;
		// CustomerWorkflowManager hosted control
		private Microsoft.Ccf.Samples.HostedControlInterfaces.IWorkflowManager customerWorkflowManager = null;
		// Current Session hosted control
		private Microsoft.Ccf.Csr.IHostedApplication3 currentSessionControl = null;
		// Email hosted control
		private IHostedApplicationMultichannel emailControl = null;
		// Web chat hosted control
		private IHostedApplicationMultichannel webchatControl = null;
		// ToolBarButton hosted control
		private IHostedToolBarButton toolBarButtonControl = null;

		// Private Statics

		// Publics
		// This allows IM conversations
		public RTC.RtcLib Rtc;
		// Get references to the Windows Terminal Services API so we can detect when the
		public AgentStats.AgentStateClient agentState;
		public Sessions SessionManager;
		public Lookup.LookupProviderPresenceState MyState;
		public CustomerAuthentication customerAuthenticated = CustomerAuthentication.NotAuthenticated;
		public bool AllowLookup = false;
		private System.Windows.Forms.Button btnCurrentSession;
		private System.Windows.Forms.ToolBar centerToolBar;
		// this variable indicates whether the webserver is down
		private bool wsdown = false;

		private const string EVENTSOURCE = "Customer Care Framework";

		// Public Statics
		public static bool LoggedIn = false;

		// system is locked.
		[System.Runtime.InteropServices.DllImport("wtsapi32.dll")]
		public static extern bool WTSRegisterSessionNotification( IntPtr hwnd, uint flags );
		[System.Runtime.InteropServices.DllImport("wtsapi32.dll")]
		public static extern bool WTSUnRegisterSessionNotification( IntPtr hwnd );

		// Public Delegates
		// Public Delegates
		public delegate void doLookupDelegate();
		#endregion

		#region Properties
		private IHostedApplicationMultichannel WebchatControl
		{
			get
			{
				try
				{
					this.webchatControl = (IHostedApplicationMultichannel)this.GetHostedApp("Web Chat Client");
				}
				catch (Exception e)
				{
					Console.Write(e.Message);
				}
				return this.webchatControl;
			}
		}

		private IHostedApplicationMultichannel EmailControl
		{
			get
			{
				this.emailControl = (IHostedApplicationMultichannel)this.GetHostedApp("Mail Client");
				return this.emailControl;
			}
		}

		public ApplicationHost appHost
		{
			get
			{
				if ( SessionManager == null || SessionManager.ActiveSession == null )
				{
					return null;
				}
				return SessionManager.ActiveSession.AppHost;
			}
		}
		#endregion

		public DesktopToolBar()
		{
			UIConfiguration.Use_ApplicationBar = true;

			// Collection of the UI elements displaying the hosted applications
			AppsUI = new CCFAppsUI();
			AppsUI.SelectedAppChanged += new SelectedAppChangedHandler(panel_SelectedIndexChanged);
			this.AppsUI.CloseApplicationClick += new CloseApplicationClickHandler(panel_CloseApplicationClick);

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// HostedApps in this panel will not be displayed
			this.hiddenPanel = new CCFPanel();

			//
			// hiddenPanel
			//
			this.hiddenPanel.Name = "hiddenPanel";
			this.hiddenPanel.Visible = false;

			// Get the debug flag setting
			Logging.Debug = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["Debug"]);

			// localized text
			this.requestAssistance.Text = localize.DESKTOP_REQUEST_ASSISTANCE_TEXT;
			this.requestAssistance.ToolTipText = localize.DESKTOP_REQUEST_ASSISTANCE_TOOLTIP;
			this.phone.Text = localize.DESKTOP_PHONE_TEXT;
			this.phone.ToolTipText = localize.DESKTOP_PHONE_TOOLTIP;
			this.lookup.Text = localize.DESKTOP_LOOKUP_TEXT;
			this.lookup.ToolTipText = localize.DESKTOP_LOOKUP_TOOLTIP;
			this.nonHostedApplications.Text = localize.DESKTOP_APPLICATIONS_TEXT;
			this.nonHostedApplications.ToolTipText = localize.DESKTOP_APPLICATIONS_TOOLTIP;
			this.help.Text = localize.DESKTOP_HELP_TEXT;
			this.help.ToolTipText = localize.DESKTOP_HELP_TOOLTIP;
			this.dynamicApplications.Text = localize.DESKTOP_DYNAMIC_APPS;
			this.dynamicApplications.ToolTipText = localize.DESKTOP_DYNAMIC_APPS_TOOLTIP;
			this.info.Text = localize.DESKTOP_INFO_NO_CURRENT_CALL;
			this.closeSession.Text = localize.SESSION_EXPLORER_CLOSE_SESSION;
			this.closeSession.ToolTipText = localize.DESKTOP_CLOSE_SESSION_TOOLTIP;
			this.closeSession.Enabled = false;

			// Handles multiple customer sessions
			int maxNumberOfSessions = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["maxNumberOfSessions"]);
			SessionManager = new AgentDesktopSessions(this.Use_SessionExplorer, maxNumberOfSessions);

			// This traps all messages so we can intercept keyboard commands before
			// the hosted apps see them
			msgFilter = new MyMessageFilter();
			msgFilter.owner = this;
			Application.AddMessageFilter( msgFilter );

			// Sets the settings object
			UIConfiguration.Settings = Properties.Settings.Default;
		}
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			try
			{
				if (  disposing )
				{
					try
					{
						loggingOut = true;
						if ( agentState != null )
						{
							try
							{
								SetAgentState( Lookup.LookupProviderPresenceState.LoggedOut, null );
							}
							catch ( Exception exp )
							{
								Logging.Warn( Application.ProductName, exp.Message );
							}
							agentState = null;
						}
					}
					catch{};

					if(ctiHostedControl != null)
					{
						if(ctiHostedControl.Cti != null)
						{
							ctiHostedControl.Cti.CallChanged -= new CallEventHandler( CallEvent );
						}
						ctiHostedControl.CleanUp();	
					}

					if ( SessionManager != null )
					{
						SessionManager.CloseAll();
						SessionManager = null;
					}

					if (components != null) 
					{
						components.Dispose();
						components = null;
					}
				}
				base.Dispose( disposing );
			}
			finally
			{
				Application.Exit();
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(DesktopToolBar));
			this.toolBarImageList = new System.Windows.Forms.ImageList(this.components);
			this.pollTimer = new System.Windows.Forms.Timer(this.components);
			this.clockTimer = new System.Windows.Forms.Timer(this.components);
			this.rightToolBar = new System.Windows.Forms.ToolBar();
			this.rightSeparator = new System.Windows.Forms.ToolBarButton();
			this.closeSession = new System.Windows.Forms.ToolBarButton();
			this.leftToolBar = new System.Windows.Forms.ToolBar();
			this.requestAssistance = new System.Windows.Forms.ToolBarButton();
			this.separator1 = new System.Windows.Forms.ToolBarButton();
			this.phone = new System.Windows.Forms.ToolBarButton();
			this.lookup = new System.Windows.Forms.ToolBarButton();
			this.nonHostedApplications = new System.Windows.Forms.ToolBarButton();
			this.separator2 = new System.Windows.Forms.ToolBarButton();
			this.separator3 = new System.Windows.Forms.ToolBarButton();
			this.help = new System.Windows.Forms.ToolBarButton();
			this.dynamicApplications = new System.Windows.Forms.ToolBarButton();
			this.toolPanel = new System.Windows.Forms.Panel();
			this.centerToolBar = new System.Windows.Forms.ToolBar();
			this.exitCCF = new Microsoft.Ccf.Samples.Csr.AgentDesktop.ImageButton();
			this.btnSessions = new System.Windows.Forms.Button();
			this.btnWorkflow = new System.Windows.Forms.Button();
			this.ticker = new Microsoft.Ccf.Samples.Csr.AgentDesktop.OwnerDrawLabel();
			this.info = new Microsoft.Ccf.Samples.Csr.AgentDesktop.OwnerDrawLabel();
			this.toolBarButton1 = new System.Windows.Forms.ToolBarButton();
			this.btnCurrentSession = new System.Windows.Forms.Button();
			this.toolPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolBarImageList
			// 
			this.toolBarImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolBarImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.toolBarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolBarImageList.ImageStream")));
			this.toolBarImageList.TransparentColor = System.Drawing.Color.Aqua;
			// 
			// pollTimer
			// 
			this.pollTimer.Interval = 10000;
			this.pollTimer.Tick += new System.EventHandler(this.pollTimer_Tick);
			// 
			// clockTimer
			// 
			this.clockTimer.Interval = 7000;
			this.clockTimer.Tick += new System.EventHandler(this.clockTimer_Tick);
			// 
			// closeSession
			// 
			this.closeSession.ImageIndex = 6;
			this.closeSession.Text = "Close Session";
			// 
			// rightToolBar
			// 
			this.rightToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.rightToolBar.AutoSize = false;
			this.rightToolBar.BackColor = System.Drawing.Color.White;
			this.rightToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							this.rightSeparator,
																							this.closeSession});
			this.rightToolBar.ButtonSize = new System.Drawing.Size(25, 36);
			this.rightToolBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightToolBar.DropDownArrows = true;
			this.rightToolBar.Font = new System.Drawing.Font("Tahoma", 8.5F);
			this.rightToolBar.ImageList = this.toolBarImageList;
			this.rightToolBar.Location = new System.Drawing.Point(754, 0);
			this.rightToolBar.Name = "rightToolBar";
			this.rightToolBar.ShowToolTips = true;
			this.rightToolBar.Size = new System.Drawing.Size(216, 32);
			this.rightToolBar.TabIndex = 1;
			this.rightToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.rightToolBar.Wrappable = false;
			this.rightToolBar.ButtonDropDown += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.leftToolBar_ButtonDropDown);
			this.rightToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// rightSeparator
			// 
			this.rightSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// leftToolBar
			// 
			this.leftToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.leftToolBar.AutoSize = false;
			this.leftToolBar.BackColor = System.Drawing.Color.White;
			this.leftToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {this.requestAssistance,
																						this.separator1,
																						this.phone,
																						this.lookup,
																						this.nonHostedApplications,
																						this.separator2,
																						this.dynamicApplications,
																						this.separator3,
																						this.help});
			this.leftToolBar.ButtonSize = new System.Drawing.Size(25, 36);
			this.leftToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.leftToolBar.DropDownArrows = true;
			this.leftToolBar.Font = new System.Drawing.Font("Tahoma", 8.5F);
			this.leftToolBar.ImageList = this.toolBarImageList;
			this.leftToolBar.Location = new System.Drawing.Point(0, 0);
			this.leftToolBar.Name = "leftToolBar";
			this.leftToolBar.ShowToolTips = true;
			this.leftToolBar.Size = new System.Drawing.Size(1010, 32);
			this.leftToolBar.TabIndex = 0;
			this.leftToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.leftToolBar.Wrappable = false;
			this.leftToolBar.ButtonDropDown += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.leftToolBar_ButtonDropDown);
			this.leftToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// requestAssistance
			// 
			this.requestAssistance.ImageIndex = 0;
			this.requestAssistance.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.requestAssistance.Text = "Assistance";
			// 
			// separator1
			// 
			this.separator1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// phone
			// 
			this.phone.Enabled = false;
			this.phone.ImageIndex = 1;
			this.phone.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.phone.Text = "Phone";
			this.phone.Visible = false;
			// 
			// lookup
			// 
			this.lookup.ImageIndex = 2;
			this.lookup.Text = "Lookup";
			// 
			// applications
			// 
			this.nonHostedApplications.ImageIndex = 3;
			this.nonHostedApplications.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.nonHostedApplications.Text = "Applications";
			// 
			// separator2
			// 
			this.separator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// separator3
			// 
			this.separator3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// help
			// 
			this.help.ImageIndex = 4;
			this.help.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.help.Text = "Help";
			//
			// dynamicApps
			//
			this.dynamicApplications.ImageIndex = 3;
			this.dynamicApplications.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.dynamicApplications.Text = "Dynamic Applications";
			// 
			// toolPanel
			// 
			this.toolPanel.BackColor = System.Drawing.Color.Transparent;
			this.toolPanel.Controls.Add(this.centerToolBar);
			this.toolPanel.Controls.Add(this.rightToolBar);
			this.toolPanel.Controls.Add(this.exitCCF);
			this.toolPanel.Controls.Add(this.leftToolBar);
			this.toolPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.toolPanel.Location = new System.Drawing.Point(0, 0);
			this.toolPanel.Name = "toolPanel";
			this.toolPanel.Size = new System.Drawing.Size(1010, 32);
			this.toolPanel.TabIndex = 2;
			// 
			// centerToolBar
			// 
			this.centerToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.centerToolBar.AutoSize = false;
			this.centerToolBar.BackColor = System.Drawing.Color.White;
			this.centerToolBar.ButtonSize = new System.Drawing.Size(25, 36);
			this.centerToolBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.centerToolBar.DropDownArrows = true;
			this.centerToolBar.Font = new System.Drawing.Font("Tahoma", 8.5F);
			this.centerToolBar.ImageList = this.toolBarImageList;
			this.centerToolBar.Location = new System.Drawing.Point(594, 0);
			this.centerToolBar.Name = "centerToolBar";
			this.centerToolBar.ShowToolTips = true;
			this.centerToolBar.Size = new System.Drawing.Size(160, 32);
			this.centerToolBar.TabIndex = 3;
			this.centerToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.centerToolBar.Wrappable = false;
			this.centerToolBar.ButtonDropDown += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.leftToolBar_ButtonDropDown);
			this.centerToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar_ButtonClick);
			// 
			// exitCCF
			// 
			this.exitCCF.Border = false;
			this.exitCCF.BorderColor = System.Drawing.Color.Empty;
			this.exitCCF.Dock = System.Windows.Forms.DockStyle.Right;
			this.exitCCF.Image = ((System.Drawing.Image)(resources.GetObject("exitCCF.Image")));
			this.exitCCF.ImageHover = ((System.Drawing.Image)(resources.GetObject("exitCCF.ImageHover")));
			this.exitCCF.ImagePressed = ((System.Drawing.Image)(resources.GetObject("exitCCF.ImagePressed")));
			this.exitCCF.Location = new System.Drawing.Point(970, 0);
			this.exitCCF.Name = "exitCCF";
			this.exitCCF.Size = new System.Drawing.Size(40, 32);
			this.exitCCF.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.exitCCF.TabIndex = 2;
			this.exitCCF.TabStop = false;
			this.exitCCF.Click += new System.EventHandler(this.exitCCF_Click);
			// 
			// btnSessions
			// 
			this.btnSessions.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnSessions.Font = new System.Drawing.Font("Tahoma", 8.5F);
			this.btnSessions.Image = ((System.Drawing.Image)(resources.GetObject("btnSessions.Image")));
			this.btnSessions.Location = new System.Drawing.Point(0, 32);
			this.btnSessions.Name = "btnSessions";
			this.btnSessions.Size = new System.Drawing.Size(104, 22);
			this.btnSessions.TabIndex = 3;
			this.btnSessions.Text = "Sessions";
			this.btnSessions.Click += new System.EventHandler(this.btnSessions_Click);
			// 
			// btnWorkflow
			// 
			this.btnWorkflow.BackColor = System.Drawing.Color.White;
			this.btnWorkflow.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnWorkflow.Font = new System.Drawing.Font("Tahoma", 8.5F);
			this.btnWorkflow.Location = new System.Drawing.Point(112, 32);
			this.btnWorkflow.Name = "btnWorkflow";
			this.btnWorkflow.Size = new System.Drawing.Size(104, 22);
			this.btnWorkflow.TabIndex = 4;
			this.btnWorkflow.Text = "Workflow";
			this.btnWorkflow.Click += new System.EventHandler(this.btnWorkflow_Click);
			// 
			// ticker
			// 
			this.ticker.BackColor = System.Drawing.Color.White;
			this.ticker.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ticker.Location = new System.Drawing.Point(368, 32);
			this.ticker.Name = "ticker";
			this.ticker.Size = new System.Drawing.Size(380, 12);
			this.ticker.TabIndex = 6;
			this.ticker.Text = "call center statistics";
			this.ticker.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.ticker.Visible = false;
			// 
			// info
			// 
			this.info.BackColor = System.Drawing.Color.White;
			this.info.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.info.Location = new System.Drawing.Point(760, 32);
			this.info.Name = "info";
			this.info.Size = new System.Drawing.Size(112, 12);
			this.info.TabIndex = 7;
			this.info.Text = "info";
			this.info.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.info.Visible = false;
			// 
			// toolBarButton1
			// 
			this.toolBarButton1.ImageIndex = 6;
			this.toolBarButton1.Text = "Exit";
			// 
			// btnCurrentSession
			// 
			this.btnCurrentSession.BackColor = System.Drawing.Color.White;
			this.btnCurrentSession.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCurrentSession.Font = new System.Drawing.Font("Tahoma", 8.5F);
			this.btnCurrentSession.Location = new System.Drawing.Point(224, 32);
			this.btnCurrentSession.Name = "btnCurrentSession";
			this.btnCurrentSession.Size = new System.Drawing.Size(104, 22);
			this.btnCurrentSession.TabIndex = 8;
			this.btnCurrentSession.Text = "Current Session";
			this.btnCurrentSession.Click += new System.EventHandler(this.btnCurrentSession_Click);
			// 
			// Desktop
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.DarkCyan;
			this.ClientSize = new System.Drawing.Size(1010, 54);
			this.ControlBox = false;
			this.Controls.Add(this.btnCurrentSession);
			this.Controls.Add(this.btnWorkflow);
			this.Controls.Add(this.btnSessions);
			this.Controls.Add(this.toolPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Desktop";
			this.SplashScreenImage = ((System.Drawing.Image)(resources.GetObject("$this.SplashScreenImage")));
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Contact Center Agent Desktop";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Desktop_Closing);
			this.Load += new System.EventHandler(this.Desktop_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Desktop_Paint);
			this.toolPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		// This is to insure that when created as a Singleton, the first instance never dies,
		// regardless of the time between users.  Only needed when remoting the
		// telephony object.
		public override object InitializeLifetimeService()
		{
			return null;
		}


		/// <summary>
		/// Logs the agent into the system.
		/// </summary>
		/// <returns>true for success, false for failure</returns>
		private bool login()
		{
			try
			{
				agentState = new AgentStats.AgentStateClient();
				agentState.Endpoint.Address = new EndpointAddress(ConfigurationReader.ReadSettings("AgentDesktop_AgentStats_AgentState"));
				agentState.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

				string agentNumber = this.agentNumber;

				// There are no guarentees that a number from TAPI is truly a number. TSAPI
				// doesn't make promises either, but all TSAPI and CSTA implementations that
				// I've seen are true phone numbers.
				// For most TAPI's the following will work and TSAPI's won't be affected.
				for ( int i = 0; i < agentNumber.Length; i++ )
				{
					if ( "0123456789".IndexOf( agentNumber[i] ) < 0 )
					{
						agentNumber = agentNumber.Remove( i, 1 );
						i--;
					}
				}

				// The computer name is being used as the SIP address
				string hostName = System.Windows.Forms.SystemInformation.ComputerName;
				this.AgentID = agentState.AgentLogin2( "sip:" + hostName, agentNumber );
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_CONNECT_SQL, exp );
				}
				else
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_LOGIN, exp );
				}
				agentState = null;   // keep closing of app from trying to log out agent
				return false;
			}

			return true;
		}


		/// <summary>
		/// Reads the options from a web service.
		/// </summary>
		private void getOptions()
		{
			// set some defaults
			secondsBetweenTickPolls = -1;
			warnAgentCallIsTooLong = new TimeSpan( 0, 4, 0 );
			agentNumber = null;

			// Get some configuration settings
			using (Options.OptionsClient wsClient = new Options.OptionsClient())
			{
				//ws.Url = ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
				//ws.Credentials = System.Net.CredentialCache.DefaultCredentials;
				//ws.PreAuthenticate = true;

				string Url = ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
				System.ServiceModel.EndpointAddress optionsAddress = new System.ServiceModel.EndpointAddress(Url);
				wsClient.Endpoint.Address = optionsAddress;
				wsClient.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();
				try 
				{
					workflowExists = Convert.ToBoolean(wsClient.GetOptionSetting("WorkflowExists"));

					secondsBetweenTickPolls = Convert.ToInt32(wsClient.GetOptionSetting("secondsBetweenTickerPolls"));
					warnAgentCallIsTooLong = new TimeSpan(0, 0, Convert.ToInt32(wsClient.GetOptionSetting("warnAgentCallIsTooLong")));

					// Agent's phone number is tied to the machine.  It can be
					// either in the database or in a local config file.
					// The local config file overrides the database.
					agentNumber = System.Configuration.ConfigurationManager.AppSettings["agentNumber"];
					if ( agentNumber == String.Empty || agentNumber == null )
					{
						agentNumber = wsClient.GetOptionSetting("PhoneAt_" + SystemInformation.ComputerName);
					}
					wsClient.Close();
				}
				catch ( System.Net.WebException wex )
				{
					// Log the error but proceed
					Logging.Error( Application.ProductName, localize.COMMON_MSG_IIS_ERROR + "\n\n" + localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, wex );
				}
				catch ( Exception exp ) 
				{
					if ( exp.Message.IndexOf( localize.COMMON_ERR_SQL_CONNECTION ) >= 0 )
					{
						Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_CONNECT_SQL + "\n\n" + localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, exp );
					}
					else
					{
						Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_TO_READ_OPTIONS, exp );
					}
				}
			}

			// don't let this setting be too low by mistake
			if ( warnAgentCallIsTooLong.TotalSeconds <= 0 )
				warnAgentCallIsTooLong = new TimeSpan( 0, 4, 0 );  // 4 minutes is the default

			if ( agentNumber == null )
				agentNumber = String.Empty;
		}

		/// <summary>
		/// Used to prevent user performing operations when CCF is doing some other
		/// operation.
		/// </summary>
		/// <param name="enable"></param>
		private void SetEnableSettingForCorePanels( bool enable )
		{
			if ( !loggingOut )
			{
				if ( !enable )
				{
					this.Refresh();
				}
			}

			toolPanel.Enabled = enable;

			if(workFlowControl != null)
			{
				workFlowControl.ControlEnabled = enable;
			}
		}

		/// <summary>
		/// Called when the main form is about to be displayed.  Some operations are here that
		/// must be done after the form is created but before it is displayed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Desktop_Load(object sender, System.EventArgs e)
		{
			bool loadSession;

			if (Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["loadSessions"], out loadSession) == false)
			{
				Logging.Error(localize.DESKTOP_MODULE_NAME, String.Format(localize.DESKTOP_ERR_READING_OR_PARSING_APPSETTINGS, "loadSessions"));
				this.Close();
			}

			this.Use_SaveSession = loadSession;

			try
			{
				SplashScreen.SplashRefresh();

				Cursor = Cursors.WaitCursor;

				doLookUpRoutine = new doLookupDelegate(this.doLookupCommand);

				// Save the icon used for indicating a warning and then blank it
				// until needed.  Must be done before the timer fires since it uses this.
				warningIcon = info.Image;
				info.Image = null;

				ticker.Text = "";

				// Get various options used by the desktop.
				getOptions();

				if ( !login() )
				{
					SplashScreen.SplashDoneNoFade();
					this.Close();  // exit if login fails
					return;
				}

				// Register desktop with CCF Server
				// May want to create a routine to set this port dynamically
				this.RegisterClient("1000");
				// Register desktop listener
				this.RegisterListener("1000");

				// The non-hosted apps are read and 
				// added to the NonHostedApps array to be called in menus
				GetNonHostedApps();

				//GetStats = new ContactCenterStatistics.ContactCenterStatistics();
				//GetStats.Url = ConfigurationReader.ReadSettings("AgentDesktop_ContactCenterStatistics_ContactCenterStatistics");
				//GetStats.Credentials = System.Net.CredentialCache.DefaultCredentials;
				//GetStats.PreAuthenticate = true;

				client = new ContactCenterStatistics.ContactCenterStatisticsClient();
				string Url = ConfigurationReader.ReadSettings("AgentDesktop_ContactCenterStatistics_ContactCenterStatistics");
				System.ServiceModel.EndpointAddress configAddress = new System.ServiceModel.EndpointAddress(Url);
				client.Endpoint.Address = configAddress;
				client.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

				// The following must be done in the Load() routine and not before.
				// This has to do with the IE browser control being created.
				//
				// Create the hosted applications
				startHostedApplications();

				IHostedApplication multiChannelLoginApplication = (IHostedApplication)this.GetHostedApp("Multi Channel Login");
				if (multiChannelLoginApplication != null)
				{
					multiChannelLoginApplication.DoAction("UpdateClientID", this.ClientID.ToString());
				}

				IHostedApplication multiChannelVoiceApplication = (IHostedApplication)this.GetHostedApp("Multi Channel Login");
				if (multiChannelVoiceApplication != null)
				{
					multiChannelVoiceApplication.DoAction("UpdateClientID", this.ClientID.ToString());
				}

				currentSessionControl = (IHostedApplication3)this.GetHostedApp("Current Session");
				if(currentSessionControl != null)
				{
					currentSessionControl.SessionManager = SessionManager;
				}

				workFlowControl = (IWorkFlow)this.GetHostedApp("Workflow");
				if(workFlowControl != null)
				{
					workFlowControl.SessionManager = SessionManager;
				}

				customerWorkflowManager = (IWorkflowManager)this.GetHostedApp("Customer Workflow Manager");

				sessionExplorerControl = (ISessionExplorer)this.GetHostedApp("Session Explorer");
				if(sessionExplorerControl != null)
				{
					// Give the session explorer control access to the UI and sessions
					sessionExplorerControl.SessionManager = SessionManager;
					sessionExplorerControl.AppsUI = this.AppsUI;
				}

				toolBarButtonControl = (IHostedToolBarButton)this.GetHostedApp("HostedToolBarButton");

				if(toolBarButtonControl != null)
				{
					status = toolBarButtonControl.Status;
					this.centerToolBar.Buttons.Add(status);
					this.status.Text = localize.DESKTOP_STATUS_TEXT;
					this.status.ToolTipText = localize.DESKTOP_STATUS_TOOLTIP;
				}
				else
				{
					// This is here to allow the code to remain unchanged but 
					// hide the toolbar...
					status = new ToolBarButton();
					status.Visible = false;
				}

				ctiHostedControl = (ICti)this.GetHostedApp("CTI");

				if( ctiHostedControl != null)
				{
					// Create the phone menu
					this.phoneMenu = new PhoneMenuToolBar();
					this.phone.DropDownMenu = this.phoneMenu;
					this.phone.Enabled = true;
					
					// Initialize CTI - comment this out if not needed
					phoneMenu.InitCTI( this, this.agentNumber, ctiHostedControl );	

					if(phoneMenu.MyLine == null) //if we don't have telephony, block the softphone menu
					{
						this.phone.Enabled = false;
					}
					else
					{
						ctiHostedControl.Cti.CallChanged += new CallEventHandler( CallEvent );
					}

					this.phone.Visible = true;	
				}

				// Move here after workFlowControl hosted control is created
				SetEnableSettingForCorePanels( false );

				init();


				// Start timer running so ticker and other updates will be handled
				// was: if ( Convert.ToInt32( ConfigurationSettings.AppSettings[ "secondsBetweenTickerPolls" ] ) > 0 )
				if ( secondsBetweenTickPolls > 0 )
				{
					pollTimer.Interval = 2000;
					pollTimer.Start();
				}
				clockTimer.Start();

				// this is a debug only message, not in the UI so no need to translate
				Logging.Trace( Application.ProductName, "Desktop_Load() successful" );

				// restore any previously unclosed sessions
				GetSessions();
			}
			catch ( System.Net.WebException wex )
			{
				// Set this flag so when desktop close it does not un-register the client
				registerClientException = true;
				Logging.Error( Application.ProductName, localize.DESKTOP_IIS_ERROR, wex );
				//similar to login failure and hence exit the application.
				SplashScreen.SplashDoneNoFade();
				this.Close();
				return;
			}
			catch ( Exception exp ) 
			{
				if ( exp.Message.IndexOf(localize.DESKTOP_MSG_SQL_EXIST) >= 0 )
				{
					throw exp;
				}
				Logging.Error( localize.DESKTOP_MODULE_NAME, localize.DESKTOP_ERR_IN_LOAD, exp );
			}
			finally
			{
				SetEnableSettingForCorePanels( true );

				SplashScreen.SplashDone();
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Close and exit the desktop application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Desktop_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				// If there was an exception while registering the client, don't unregister
				if (!registerClientException)
				{
					// Unregister client for CCF Server
					this.UnregisterClient();
					this.UnregisterListener();
				}
				SaveSessions();
			}
			catch (Exception exp)
			{
				// Web Server is down
				System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();
				// Event source should have already been setup on initial install
				if (System.Diagnostics.EventLog.SourceExists(EVENTSOURCE))
				{
					eventLog.Source = EVENTSOURCE;
					eventLog.WriteEntry(localize.COMMON_MSG_IIS_ERROR + " " + exp.Message, System.Diagnostics.EventLogEntryType.Error);
				}
			}

			loggingOut = true;
			this.Enabled = false;

			// to reduce the UI flickering as we exit
			SetEnableSettingForCorePanels( false );

			pollTimer.Stop();
			clockTimer.Stop();

			FreeSessionExplorerAndWorkflow();

			// If RTC is being used, we need to explicitly close it to release
			// its interop resources.
			if ( Rtc != null )
			{
				Rtc.Close();
				Rtc = null;
			}

			if ( SessionManager != null )
			{
				SessionManager.CloseAll();
				SessionManager = null;
			}

			// Get rid of the splash screen if its still here
			SplashScreen.SplashDoneNoFade();

			customerLookup.Close();
		}

		/// <summary>
		/// Does most of the initialization for the application
		/// </summary>
		private void init()
		{
			// Create menus
			setupMenus();

			try
			{
				// Set up so we can later look up customer records
				noCustomer();

				customerLookup = new CustomerClient();
				customerLookup.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

				LoggedIn = true;

			}
			catch ( Exception exp )
			{
				if ((exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 ) ||
					(exp.Message.IndexOf( localize.COMMON_MSG_IIS_ERROR ) >= 0 ))
				{
					throw;
				}
				else
					Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_WEBSERVICE, exp );
			}

			// Set up client for handling IM
			try
			{
				// Avoid problem where a crash can happen with TAPI and RTC running together
				// There is a problem as of 3/2003 with TAPI and RTC where if they
				// are both used in a single process, a thread which RTC spawns
				// to handle video will crash after an hour or so.
//#if NOT_XP_SP2	// Fixed in XP SP2
//				if ( myLine != null && myLine.Type == TelephonyProvider.CtiType.TAPI &&
//					remoteTelephonyProcess == null )
//				{
//					Logging.Error( Application.ProductName, localize.DESKTOP_ERR_RTC_TAPI_CONFLICT );
//
//					string message = "RTC is disabled because of a conflict with TAPI." + 
//						System.Environment.NewLine + System.Environment.NewLine + "You can " +
//						"use CTI remoting with TAPI or use a telephony standard besides TAPI " +
//						"or not use RTC.  There is a problem as of 3/2003 with TAPI and RTC " +
//						"where if they are both used in a single process, a media thread which " +
//						"RTC spawns to handle video will cause problems within 45 minutes.";
//					MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK);
//				}
//				else
//#endif
//			{
				RtcLib.RtcLib lib = CreateRtcLib();

				if (lib == null)
				{
					this.requestAssistance.Enabled = false;
				}
				else
				{
					Rtc = lib;
				}
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_RTC, exp );
			}

			InitializeMultichannelStatusMap();
			// yes, login does this, but this fixes the toolbar too
			SetAgentState( Lookup.LookupProviderPresenceState.Ready, null );

			InitializeSessionExplorerAndWorkflow();
		}

		// initialized a map from IAD status to multichannel provider status
		private void InitializeMultichannelStatusMap()
		{
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.Assisting.ToString(), Constants.Status.NotReadyStatus);
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.Away.ToString(), Constants.Status.NotReadyStatus);
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.Busy.ToString(), Constants.Status.NotReadyStatus);
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.CallWrapup.ToString(), Constants.Status.AfterCallWorkStatus);
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.LoggedOut.ToString(), Constants.Status.LoggedOutStatus);
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.OnCallActive.ToString(), Constants.Status.NotReadyStatus);
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.OnCallHold.ToString(), Constants.Status.NotReadyStatus);
			clientToMCProvidersStatusMapping.Add(Lookup.LookupProviderPresenceState.Ready.ToString(), Constants.Status.ReadyStatus);
		}


		// TODO move to UIConfigurations???
		private RtcLib.RtcLib CreateRtcLib()
		{
			Options.OptionsClient optionsService = new Options.OptionsClient();
			//optionsService.PreAuthenticate = true;
			//optionsService.Url = ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
			//optionsService.Credentials = CredentialCache.DefaultCredentials;

			string Url = ConfigurationReader.ReadSettings("AgentDesktop_Options_Options");
			System.ServiceModel.EndpointAddress optionsAddress = new System.ServiceModel.EndpointAddress(Url);
			optionsService.Endpoint.Address = optionsAddress;
			optionsService.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

			string instantMessaging = optionsService.GetOptionSetting("InstantMessaging");
			string lcsServer = optionsService.GetOptionSetting("LCSServer");
			string lcsTransportType = optionsService.GetOptionSetting("LCSTransportType");
			string sipAddress = "sip:" + SystemInformation.ComputerName;
			RTC_SESSION_TYPE sessionType = RTC_SESSION_TYPE.RTCST_IM;
			if (instantMessaging.ToLower(CultureInfo.InvariantCulture) == "lcs")
			{
				sessionType = RTC_SESSION_TYPE.RTCST_MULTIPARTY_IM;
				sipAddress = GetSipAddress();
				if (sipAddress == null)
				{
					return null;
				}
			}

			int transport = RTCHelp.ParseLcsTransport(lcsTransportType);

			return new RTC.RtcLib(sessionType, sipAddress, lcsServer, transport);
			optionsService.Close();
		}

		// TODO move to UIConfigurations???
		private string GetSipAddress()
		{
			DirectoryEntry root = new DirectoryEntry("LDAP://rootDSE");
			object namingContext = root.Properties["rootDomainNamingContext"].Value;
			Logging.Information(Application.ProductName, "Naming Context: " + namingContext.ToString());
			DirectoryEntry domain = new DirectoryEntry(("LDAP://" + namingContext));
			string domainUsername = WindowsIdentity.GetCurrent().Name;
			Logging.Information(Application.ProductName, "Domain user name: " + domainUsername);
			int indexOfBackslash = domainUsername.IndexOf('\\');
			string name = domainUsername.Substring(indexOfBackslash + 1);
			string filter = String.Format("(&(objectCategory=person)(objectClass=user)(samAccountName={0})(msRTCSIP-UserEnabled=TRUE))", name);
			DirectorySearcher searcher = new DirectorySearcher(domain, filter, 
				new string[]{"msRTCSIP-PrimaryUserAddress"});

			SearchResult results = searcher.FindOne();
			if (results != null)
			{
				DirectoryEntry user = results.GetDirectoryEntry();
				string primaryUserAddress = (string) user.Properties["msRTCSIP-PrimaryUserAddress"].Value;
				Logging.Information(Application.ProductName, "Primary User address: " + primaryUserAddress);
				return primaryUserAddress;
			}
			else
			{
				MessageBox.Show(localize.DESKTOP_ERR_REQUEST_ASSISTANCE_MESSAGE, localize.DESKTOP_ERR_REQUEST_ASSISTANCE_CAPTION, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
		}

		/// <summary>
		/// Sets the focus on to the application passed
		/// </summary>
		/// <param name="nApplicationId">id of the application to which 
		/// focus need to be given</param>
		private bool setFocusForApplicationID( int nApplicationId )
		{
			bool hostedAppFound = false;
			try
			{
				hostedAppFound = AppsUI.SelectApplication( nApplicationId );

				if ( !hostedAppFound && appHost.Count > 0 )
				{
					// No need to tell user about this, but log it.
					Logging.Warn( this.ToString(), localize.DESKTOP_ERR_COULDNOT_FIND_APPLICATION );
				}
			}
			catch ( Exception exp)
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_TABFOCUS_ERROR, exp );
			}

			return hostedAppFound;
		}


		/// <summary>
		/// This creates the tabbed window interface and calls the application hosting
		/// layer to start and place the applications the agent will use.
		/// </summary>
		private void startHostedApplications()
		{
			try
			{
				this.AppsUI.AddPanel( this.hiddenPanel );

				// if there is a splash screen, force it to refresh since some hosted
				// apps may have drawn over it.
				SplashScreen.SplashRefresh();

				AddSession( null, null );
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
					throw;
				if ( exp.Message.IndexOf( localize.COMMON_MSG_IIS_ERROR ) >= 0 )
					throw;

				Logging.Error( Application.ProductName, localize.DESKTOP_APPCREATE_ERROR, exp );
				this.Close();
			}
		}


		/// <summary>
		/// This stops a single hosted application.
		/// Callable either directly or from a AppHost event.
		/// </summary>
		/// <param name="app"></param>
		private void stopApplication(IHostedApplication app)
		{
			if (app != null)
			{
				try
				{
					// Remove only the dynamic applications from the UI.
					if (appHost != null && appHost.IsDynamicApplication(app))
					{
						this.AppsUI.SetRedraw(false);

						if (app.TopLevelWindow != null)
						{
							this.AppsUI.RemoveApplication(app);

							// app that was removed cannot be focused
							if (this.SessionManager.ActiveSession.FocusedApplication == app)
								this.SessionManager.ActiveSession.FocusedApplication = null;
						
							if (currentSessionControl != null)
							{
								currentSessionControl.FireRequestAction(new RequestActionEventArgs(currentSessionControl.ApplicationName, "FocusedAppChanged", string.Empty));
							}

							if (sessionExplorerControl != null)
							{
								sessionExplorerControl.RemoveApplicationNode(SessionManager.ActiveSession, app);
							}
						}
					}
				}
				finally
				{
					this.AppsUI.SetRedraw(true);
				}
			}
		}

		/// <summary>
		/// This starts a single hosted application.
		/// Callable either directly or from a Session/AppHost event.
		/// </summary>
		/// <param name="app"></param>
		private void startApplication( IHostedApplication app )
		{
			try
			{
				// See if this app is hosted within another window
				if ( app.CanEmbed )
				{
					// Used to allow developers to customize their applications with their
					// own XML tags in the Applications table.
					string initializationXml = appHost.GetApplicationInitializationXML( app.ApplicationID );

					bool provideCloseButton = false;

					// Only provide close button if app is dynamic and can be closed dynamically.
					if (appHost.IsDynamicApplication(app))
					{
						if (appHost.CanCloseDynamicApplication(app))
						{
							provideCloseButton = true;
						}
					}

					if ( AppsUI.AddApplication( app.DisplayGroup, app, initializationXml, provideCloseButton ) == null )
					{
						Logging.Error( Application.ProductName, localize.DESKTOP_APPSTART_ERROR + " " + app.ApplicationName );
						return;
					}

					// if there is a splash screen, force it to refresh since some hosted
					// apps may have drawn over it.
					SplashScreen.SplashRefresh();
				}

				// If we are also a IHostedApplicationEx, then do some extra initialization
				// This is an example of how to use the IHostedApplicationEx,
				// with this version of CCF onward it is suggested you used
				// IHostedApplication3 (or the HostedControl class) and the CTI
				// Hosted control.
				if ( app is IHostedApplicationEx )
				{
					IHostedApplicationEx appExtended = app as IHostedApplicationEx;

					appExtended.AgentID = this.AgentID;

					if(ctiHostedControl != null)
					{
						appExtended.Cti = ctiHostedControl.Cti;
						appExtended.AgentLine = ctiHostedControl.CtiLine;
					}
				}
			}
			catch ( Exception exp )
			{
				if ( app.ApplicationName != null )
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_APPSTART_ERROR + " " + app.ApplicationName, exp );
				}
				else
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_APPSTART_ERROR, exp );
				}
			}
		}

		/// <summary>
		/// Handler for action errors in case they come from another thread
		/// </summary>
		/// <param name="appSender"></param>
		/// <param name="args"></param>
		private void HandleRequestActionStatus(IHostedApplication appSender, RequestActionEventArgs args)
		{
			Logging.Error( Application.ProductName, args.TargetApplication );
		}

		/// <summary>
		/// Handler for when a close button is click for a dynamic application.
		/// </summary>
		/// <param name="app"></param>
		private void panel_CloseApplicationClick(IHostedApplication app)
		{
			if (workFlowControl != null)
			{
				// Workflow has started
				if (!workFlowControl.WorkflowNotStarted)
				{
					// Check if application is a part currently running workflow, 
					// if it is give warning and don't close
					foreach (WorkflowStep step in workFlowControl.CurrentWorkflow.Steps)
					{
						if (app.ApplicationID == step.HostedApplicationId)
						{
							MessageBox.Show(localize.DESKTOP_UNABLE_TO_CLOSE_DYNAMIC_APP_DUE_TO_WORKFLOW, Application.ProductName);
							return;
						}
					}
				}
			}

			appHost.UnloadDynamicApplication(app);
			this.UpdateDynamicApplicationMenu(SessionManager.ActiveSession.Global);

			this.SyncSessionExplorerApplicationSelection(CCFAppsUI.AppWithFocus);
		}

		/// <summary>
		/// Event for changing tab of hosted app tab control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void panel_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				
				if ( SessionManager != null && SessionManager.ActiveSession != null )
				{
					// save state of all the applications
					if ( !SessionManager.ActiveSession.Global && !wsdown )
					{
						SaveSessions();
					}
					// save which application has the focus so if sessions are switched and
					// switched back, the same app can have the focus again.
					if ( !dontBotherCertainOperations )
					{
						SessionManager.ActiveSession.FocusedApplication = CCFAppsUI.AppWithFocus;
					}
				}

				SyncSessionExplorerApplicationSelection( CCFAppsUI.AppWithFocus );
			}
			catch ( System.Net.WebException wex )
			{
				// Web Server is down
				System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();
				// Event source should have already been setup on initial install
				if (System.Diagnostics.EventLog.SourceExists(EVENTSOURCE))
				{
					eventLog.Source = EVENTSOURCE;
					eventLog.WriteEntry(localize.COMMON_MSG_IIS_ERROR + " " + wex.Message, System.Diagnostics.EventLogEntryType.Error);
				}
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_CONNECT_SQL, exp );
				}
				else
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_TABAPP_ERROR, exp );
				}
			}
		}

		/// <summary>
		/// This fires whenever an action is finished for a hosted app.
		/// </summary>
		/// <param name="app"></param>
		private void appHostRequest_Focus( IHostedApplication app )
		{
			// Allows foreach loops to cycle across the tab groups.  Makes it easier to
			// add or remove tab controls with few changes to the overall code.            

			AppsUI.SelectApplication( app.ApplicationID );
		}


		// v1.02 - fixed name collision that confused the reading of the code.  This was
		//   given the same name as the web service which reads it.
		private struct NonHostedApplication
		{
			public string displayName;
			public string commandLine;
			public bool   defaultApp;
		};

		/// <summary>
		/// This array contains the list of non hosted apps configured.
		/// </summary>
		private NonHostedApplication []NonHostedApps = null;

		/// <summary>
		/// Get Non Hosted Applications list from the database configured.
		/// </summary>
		private void GetNonHostedApps()
		{
			try
			{
				// Create the proxy for application state webservice and call the method to save.			
				using (NonHostedApplications.NonHostedApplicationClient nonHostedApplicationsClient = new NonHostedApplications.NonHostedApplicationClient())
				{
					//nonHostedApplications.Url = ConfigurationReader.ReadSettings("AgentDesktop_NonHostedApplications_NonHostedApplication");
					//nonHostedApplications.Credentials = System.Net.CredentialCache.DefaultCredentials;
					//nonHostedApplications.PreAuthenticate = true;

					string Url = ConfigurationReader.ReadSettings("AgentDesktop_NonHostedApplications_NonHostedApplication");
					System.ServiceModel.EndpointAddress nonHostedAppAddress = new System.ServiceModel.EndpointAddress(Url);
					nonHostedApplicationsClient.Endpoint.Address = nonHostedAppAddress;
					nonHostedApplicationsClient.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

					// Get the xml string containing Non Hosted Application
					string nonHostedAppXml = nonHostedApplicationsClient.GetNonHostedApplications();

					// if there are no non-hosted applications configured, return.
					if ( nonHostedAppXml == "" )
					{
						return;
					}

					// Load the xml to document
					System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
					doc.LoadXml( nonHostedAppXml );

					// Get the list of all the rows
					System.Xml.XmlNodeList nodeList = doc.SelectNodes("descendant::row");
				
					// Create the array of non hosted apps - 
					// to get all the apps listed in the DB
					NonHostedApps = new NonHostedApplication[ nodeList.Count ];

					int i = 0;
					string appName;
					string cmdLine;
					foreach ( System.Xml.XmlNode nodeRow in nodeList )
					{
						appName = nodeRow.Attributes["Name"].Value;
						if ( appName != null )
						{
							cmdLine = nodeRow.Attributes["CommandLine"].Value.Trim();

							NonHostedApps[ i ].displayName = appName.Trim();
							NonHostedApps[ i ].commandLine = cmdLine;

							if ( nodeRow.Attributes["DefaultApp"].Value == "1" )
							{
								NonHostedApps[ i ].defaultApp = true;
							}
							else
							{
								NonHostedApps[ i ].defaultApp = false;
							}
							i++;
						}
						nonHostedApplicationsClient.Close();
					}
				}
			}
			catch ( System.Net.WebException wex )
			{
				// Log the error but proceed
				Logging.Error( Application.ProductName, localize.DESKTOP_IIS_ERROR, wex );
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
				{
					throw exp;
				}
				if ( exp.Message.IndexOf( localize.COMMON_MSG_IIS_ERROR ) >= 0 )
				{
					throw exp;
				}
				Logging.Error( Application.ProductName, localize.DESKTOP_LOADNONHOSTEDAPP_ERROR, exp );
			}
		}


		/// <summary>
		/// Some menus are created from information in the databases.
		/// </summary>
		private void setupMenus()
		{
			try
			{
				int i;
				ContextMenu dropdownMenu;
				MenuItem	menuItem;

				LookupClient lookupClient = new LookupClient();
				string Url = ConfigurationReader.ReadSettings("AgentDesktop_Lookup_Lookup");
				System.ServiceModel.EndpointAddress lookupAddress = new EndpointAddress(Url);
				lookupClient.Endpoint.Address = lookupAddress;
				lookupClient.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

				// Setup the Status/Presence menu
				PresenceStates = lookupClient.GetPresenceStates();
				if ( PresenceStates != null )
				{
					int shortcutIndex = 0;

					dropdownMenu = new ContextMenu();
					for ( i = 0; i < PresenceStates.Count; i++ )
					{
						if ( PresenceStates[ i ].ID != (int)Lookup.LookupProviderPresenceState.LoggedOut )
						{
							menuItem = dropdownMenu.MenuItems.Add( PresenceStates[ i ].Name, new EventHandler(this.status_Click) );
							if ( i < 12 )
							{
								menuItem.ShowShortcut = true;
								menuItem.Shortcut = (System.Windows.Forms.Shortcut)((int)System.Windows.Forms.Shortcut.CtrlF1 + shortcutIndex);

								++shortcutIndex;
							}
						}
					}
					if ( dropdownMenu.MenuItems.Count > 0 )
					{
						status.DropDownMenu = dropdownMenu;
					}
				}
				else
				{
					status.Enabled = false;
				}

				// Setup Request Assistance menu
				Skills = lookupClient.GetSkills();
				if ( Skills != null && Skills.Count > 0 )
				{
					dropdownMenu = new ContextMenu();
					for ( i = 0; i < Skills.Count; i++ )
					{
						menuItem = dropdownMenu.MenuItems.Add( Skills[ i ].Name, new EventHandler(this.requestAssistance_Click) );
						if ( i < 12 )
						{
							menuItem.ShowShortcut = true;
							menuItem.Shortcut = (System.Windows.Forms.Shortcut)( (int)System.Windows.Forms.Shortcut.ShiftF1 + i );
						}
					}
					requestAssistance.DropDownMenu = dropdownMenu;
				}
				else
				{
					requestAssistance.Enabled = false;
				}

				// Setup Applications menu
				// check if there are any non-hosted apps.
				if ( NonHostedApps != null && NonHostedApps.Length > 0 )
				{
					dropdownMenu = new ContextMenu();

					for ( i = 0; i < NonHostedApps.Length; i++ )
					{
						menuItem = dropdownMenu.MenuItems.Add( NonHostedApps[ i ].displayName, new EventHandler(this.nonHostedApplications_Click) );
						menuItem.DefaultItem = NonHostedApps[ i ].defaultApp;
						if ( i < 26 )
						{
							menuItem.ShowShortcut = true;
							menuItem.Shortcut = (System.Windows.Forms.Shortcut)((int)System.Windows.Forms.Shortcut.CtrlShiftA + i );
						}
					}
					if ( dropdownMenu.MenuItems.Count > 0 )
					{
						nonHostedApplications.DropDownMenu = dropdownMenu;
					}
				}
				else
				{
					nonHostedApplications.Enabled = false;
				}

				UpdateDynamicApplicationMenu(true);

				// create Help About menu
				dropdownMenu = new ContextMenu();
				dropdownMenu.MenuItems.Add( localize.DESKTOP_MENU_ITEM_TEXT_ABOUT_CONTACT_CENTER_FRAMEWORK, new EventHandler(this.help_Click) );
				help.DropDownMenu = dropdownMenu;

				lookupClient.Close();
			}
			catch ( Exception exp ) 
			{
				Logging.Error( localize.DESKTOP_MODULE_NAME, localize.DESKTOP_ERR_SETUP_MENU, exp );
			}
		}

		/// <summary>
		/// This handles the selection of any menu item under the Request Assistance button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void requestAssistance_Click(object sender, System.EventArgs e)
		{
			try
			{
				MenuItem m = (System.Windows.Forms.MenuItem)sender;

				string Url = ConfigurationReader.ReadSettings("AgentDesktop_SkillsRouting_Routing");
				System.ServiceModel.EndpointAddress routingAddress = new System.ServiceModel.EndpointAddress(Url);
				skillsRoutingService.Endpoint.Address = routingAddress;
				skillsRoutingService.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

				if ( Rtc == null )
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_REQUEST_ASSISTANCE );
				}
				else
				{
					BindingList<SkillsRouting.RoutingProviderAgentContact> contacts = skillsRoutingService.GetSkillContacts( Skills[ m.Index ].ID, (int)Lookup.LookupProviderChannel.InstantMessaging );
					if ( contacts == null )
					{
						Logging.Error( this.Name, localize.DESKTOP_MSG_UNABLE_FIND_ASSISTANCE + " " + m.Text + "." );
					}
					else
					{
						// Start an IM conversation
						Logging.Information( Application.ProductName, localize.DESKTOP_MSG_STARTING_IM + " " + contacts[ 0 ].SIPAddress );
						Rtc.StartConversation( contacts[ 0 ].SIPAddress, contacts[ 0 ].FirstName, contacts[ 0 ].LastName, m.Text, (appHost.GetContext()).GetContext() );
					}
				}
			}
			catch ( System.Net.WebException wex )  // v1.02
			{
				// Log the error but proceed
				Logging.Error( Application.ProductName, localize.COMMON_MSG_IIS_ERROR, wex );
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
				{
					throw exp;
				}
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_REQUESTASSISTANCE, exp );
			}
		}

		/// <summary>
		/// This handles the selection of any menu item under the Applications button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void nonHostedApplications_Click(object sender, System.EventArgs e)
		{
			Cursor oldCursor = Cursor.Current;
			MenuItem item = sender as MenuItem;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				System.Diagnostics.Process.Start( NonHostedApps[ item.Index ].commandLine );
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_TOSTART_APPLICATION + " " + NonHostedApps[ item.Index ].displayName + "'.", exp );
			}
			finally
			{
				Cursor.Current = oldCursor;
			}
		}
		// Begin Dynamic

		/// <summary>
		/// This handles the selection of any menu item under the Dynamic Applications button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dynamicApps_Click(object sender, System.EventArgs e)
		{
			MenuItem item = sender as MenuItem;
			if (item.Enabled && !appExistsInUI(item.Text.ToString()))
			{
				if (DialogResult.Yes == MessageBox.Show(String.Format("{0}\n{1}", localize.DESKTOP_DYNAMIC_APP_LAUNCH_CONFIRMATION_MSG, item.Text.ToString()), Application.ProductName, MessageBoxButtons.YesNo))
				{
					IHostedApplication hostedApp = appHost.LoadDynamicApplication(item.Text.ToString());

					if (hostedApp != null)
					{
						// Initialize the new dynamic app just loaded.
						appHost.InitializeApplications(hostedApp);

						// Execute the default actions on the new dynamic app just loaded.
						appHost.ExecuteDefaultActions(hostedApp);

						// Notify the new dynamic app of the context.
						appHost.NotifyContextChange(hostedApp);

						// Apply the application state.
						appHost.ExecuteApplicationState(hostedApp);
					}

					item.Enabled = false;

					if (sessionExplorerControl != null)
					{
						if (SessionManager.ActiveSession != null)
						{
							sessionExplorerControl.AddApplicationNode(SessionManager.ActiveSession, hostedApp);
						}
					}
				}
			}
		}

		/// <summary>
		/// Helper function see if an application is already loaded
		/// </summary>
		/// <param name="appName"></param>
		/// <returns></returns>
		private bool appExistsInUI(string appName)
		{
			foreach (IHostedApplication app in this.AppsUI.Applications)
			{
				if (app.ApplicationName.ToLower() == appName.ToLower())
					return true;
			}
			return false;
		}

		/// <summary>
		/// This helper method will update dynamic applications drop down menu.
		/// </summary>
		/// <param name="global">True if want only global dynamic applications or false for
		/// all applications</param>
		private void UpdateDynamicApplicationMenu(bool global)
		{
			ArrayList appNames = appHost.GetDynamicApplicationNames(global);

			if (appNames != null)
			{
				// Setup Dynamic Applications menu
				ContextMenu dropdownMenu = new ContextMenu();

				foreach (string name in appNames)
				{
					MenuItem item = new MenuItem(name, new EventHandler(this.dynamicApps_Click));
					dropdownMenu.MenuItems.Add(item);

					// If already in the UI then disable it
					if (appExistsInUI(name))
					{
						item.Enabled = false;
					}
				}

				if (dropdownMenu.MenuItems.Count > 0)
				{
					dynamicApplications.DropDownMenu = dropdownMenu;
				}
			}
			else
			{
				dynamicApplications.Enabled = false;
			}
		}

		// End Dynamic


		/// <summary>
		/// This handles the selection of any menu item under the help button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void help_Click(object sender, System.EventArgs e)
		{
			try
			{
				using ( CCFAbout ccfAbout = new CCFAbout( this.agentNumber ) )
				{
					ccfAbout.ShowDialog( this );
				}
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_CCF_ABOUT_ERROR, exp );
			}
		}


		/// <summary>
		/// This handles the selection of any menu item under the Applications button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void status_Click(object sender, System.EventArgs e)
		{
			try
			{
				MenuItem item = (MenuItem)sender;

				// Can't assume the menu item index == the value of the state, so
				// search for it by name.
				for ( int i = 0; i < PresenceStates.Count; i++ )
				{
					if ( PresenceStates[ i ].Name == item.Text )
					{
						bool doit = true;

						if(ctiHostedControl != null)
						{
							doit = ctiHostedControl.Hold(PresenceStates[i].ID == (int)Lookup.LookupProviderPresenceState.OnCallHold);
						}

						if ( doit )
						{
							SetAgentState((LookupProviderPresenceState)PresenceStates[i].ID, PresenceStates[i].Name);
						}
						break;
					}
				}
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_SET_PRESENCE_VALUE, exp );
			}
		}

		/// <summary>
		/// Event fires when any button on the toolbar is pressed or dropped down.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			Point pt;
			ContextMenu popup;

			try
			{
				if ( e.Button != null && e.Button.Enabled )
				{
					if ( e.Button == this.lookup )
					{
						doLookupCommand();
					}
					else if ( e.Button == this.closeSession )
					{
						doCloseSession();
					}
					else if ( e.Button.DropDownMenu != null )
					{
						if ( e.Button == this.status )
						{
							modifyStatusMenu();
						}
						// Get which menu we need to display
						popup = e.Button.DropDownMenu.GetContextMenu();

						// Find where to display the menu and do so
						pt = new Point( e.Button.Rectangle.X, e.Button.Rectangle.Y + e.Button.Rectangle.Height );
						popup.Show( e.Button.Parent, pt );
					}
				}
			}
			catch ( System.Net.WebException wex )
			{
				// Log the error but proceed
				Logging.Error( Application.ProductName, localize.DESKTOP_IIS_ERROR, wex );
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
				{
					throw;
				}
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_CLICKING_TOOLBAR_BUTTON, exp );                
			}
		}

		/// <summary>
		/// Used to handle the buttons on the left toolbar (most of the toolbar)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void leftToolBar_ButtonDropDown(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			try 
			{
				modifyStatusMenu();
			}
			catch ( Exception exp )
			{
				Logging.Error( localize.DESKTOP_MODULE_NAME, localize.DESKTOP_ERR_MODIFY_STATUS_MENU, exp );
			}
		}

		/// <summary>
		/// Called to update the status information on the dropdown menu
		/// </summary>
		private void modifyStatusMenu()
		{
			int menuIndex;

			if(ctiHostedControl != null)
			{
				if(ctiHostedControl.HaveCalls())
				{
					phoneMenu.modifyPhoneMenu();	
				}
			}

			// Place a check next to the current presence state
			for ( int i = 0; i < PresenceStates.Count; i++ )
			{
				bool check = ( MyState == (Lookup.LookupProviderPresenceState)PresenceStates[ i ].ID );

				// find the corresponding menu index since they may not be the same
				for ( menuIndex = status.DropDownMenu.MenuItems.Count; --menuIndex >= 0; )    
				{
					if ( status.DropDownMenu.MenuItems[ menuIndex ].Text == PresenceStates[ i ].Name )
					{
						break;
					}
				}
				if ( menuIndex < 0 )
				{
					continue;
				}

				// If we have CTI, do some smart things to keep the two in sync
				if(ctiHostedControl != null)
				{
					if( ctiHostedControl.HaveCalls() )
					{
						switch( PresenceStates[i].ID )
						{
							case (int)Lookup.LookupProviderPresenceState.OnCallHold:
								status.DropDownMenu.MenuItems[ menuIndex ].Enabled = phoneMenu.hold.Enabled;
								break;

							case (int)Lookup.LookupProviderPresenceState.OnCallActive:
								status.DropDownMenu.MenuItems[ menuIndex ].Enabled = ( ctiHostedControl.GetNumberOfCalls() > 0 ) && !phoneMenu.unhold.Enabled;
								break;

							case (int)Lookup.LookupProviderPresenceState.Away:
								status.DropDownMenu.MenuItems[ menuIndex ].Enabled = ( ctiHostedControl.GetNumberOfCalls() == 0 );
								break;

							case (int)Lookup.LookupProviderPresenceState.Ready:
								status.DropDownMenu.MenuItems[ menuIndex ].Enabled = ( ctiHostedControl.GetNumberOfCalls() == 0 );
								break;
						}
					}
				}

				status.DropDownMenu.MenuItems[ menuIndex ].Checked = check;
			}
		}

		/// <summary>
		/// Does a manual lookup customer operation.
		/// </summary>
		private void doLookupCommand()
		{
			bool dialogResultOk = false;

			try
			{
				// so no more lookups occur until this one is done
				SetEnableSettingForCorePanels( false );

				using ( LookupDlg dlg = new LookupDlg( customerLookup, customer ) )
				{
					if ( dlg.ShowDialog( this ) == DialogResult.OK )
					{
						dialogResultOk = true;

						customer = dlg.Result;
						customerAuthenticated = CustomerAuthentication.Authenticated;

						// See if we are on an existing call, if so, associate the
						// call with this session
						CallClassProvider call = null;

						if( ctiHostedControl != null)
						{
							call = ctiHostedControl.GetActiveCall();
						}

						// Create a blank customer record to use.
						// In real life, the contact center needs to add code to
						// fill in the customer information.
						if ( customer == null )
						{
							customer = newCustomer();
							// see if we can fill in any blanks
							if ( call != null )
							{
								customer.PhoneHome = call.CallerNumber;
							}
						}

						// if we have a call, assume that this is to look up a different
						//    customer for this session.
						// if no call, assume we are starting a new session
						if ( call != null && SessionManager.ActiveSession != null &&
							!SessionManager.ActiveSession.Global )
						{
							// Change customer for existing session
							AgentDesktopSession activeSession = (AgentDesktopSession) SessionManager.ActiveSession;
							activeSession.Customer = customer;

							// Post 1.02 - check if there is a firstname
							if ( customer.FirstName != null && customer.FirstName != String.Empty )
							{
								SessionManager.ActiveSession.Name = customer.FirstName + " " + customer.LastName;
							}
							else
							{
								SessionManager.ActiveSession.Name = customer.LastName;
							}

							if(sessionExplorerControl != null)
							{
								sessionExplorerControl.RefreshView();
							}

							if(currentSessionControl != null)
							{
								currentSessionControl.FireRequestAction(
									new RequestActionEventArgs(currentSessionControl.ApplicationName, "LoadAllSessions", string.Empty));
							}

							// v1.02 moved here so the context is set AFTER the
							//   session has been created.  Otherwise all the previous
							//   sessions have the wrong context assigned.
							SetContext( call );
						}
						else
						{
							// start a new session
							AddSession( customer, call );
						}


						if(ctiHostedControl != null && !ctiHostedControl.HaveCalls())
						{
							SetAgentState( LookupProviderPresenceState.OnCallActive, null );	
						}
						else
						{
							SetAgentState(LookupProviderPresenceState.Busy, null);
						}
					}
				}
			}
			finally
			{
				SetEnableSettingForCorePanels( true );

				// For Manual and Multichannel, check for the workflow and set
				// appropriately
				if(dialogResultOk && this.workFlowControl != null)
				{
					if (ci != null && ci.ChannelType == "CtiChannel")
					{
						Context ctiContext = new Context();
						string contextXml = ci.SpeechServerContext;
						ctiContext.SetContext(contextXml);
						try
						{
							if (ctiContext["newservice"] != null)
							{
								this.workFlowControl.StartWorkflowByName("New Service");
							}
							else if (ctiContext["billing"] != null)
							{
								this.workFlowControl.StartWorkflowByName("Billing Query");
							}
						}
						catch (Exception e)
						{
							Logging.Error(Application.ProductName, e.Message);
						}
					}
					else
					{
						// Set to the first workflow, CTI will set to 0, this 
						// is to show different variations.
						//	this.workFlowControl.StartWorkflowByIndex(0);
					}
				}

				// Handle non-global special applications
				if (ci != null)
				{
					if (ci.ChannelType == "WebchatChannel")
					{
						if (this.WebchatControl != null)
						{
							this.WebchatControl.Open(ci.ChannelKey);
						}
					}
					else if (ci.ChannelType == "MailChannel")
					{
						if (this.EmailControl != null)
						{
							this.EmailControl.Open(ci.ChannelKey);
						}
					}
				}
			}
		}

		/// <summary>
		/// Toolbar button comment which closes the current session (if any)
		/// </summary>
		private void doCloseSession()
		{
			Session session = SessionManager.ActiveSession;
			if ( session == null || session.Global )
			{
				// shouldn't be able to do this if there is no session to close
				this.closeSession.Enabled = false;
			}
			else
			{
				// See if they are part way thru a workflow.  If so, they
				// must do a Done or a Cancel.
				if ( session.IsWorkflowPending )
				{
					string appName = "Contact Center Framework";
					MessageBox.Show(String.Format( "{0}\n\n{1}", localize.SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG1, localize.SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG2), appName, MessageBoxButtons.OK);
				}
				else
				{
					SessionManager.CloseSession( session, false );
					this.Refresh();
				}
			}
		}

		/// <summary>
		/// This does routine updates of the screen, no web service interactions.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void clockTimer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				if(ctiHostedControl != null && ctiHostedControl.OnThePhone())
				{
					// Find the longest call
					CallClassProvider longestCall = ctiHostedControl.FindTheLongestCall();

					// See if agent needs a warning that the call has been a bit long
					if ( longestCall != null && longestCall.Started + warnAgentCallIsTooLong <= DateTime.Now )
					{
						// Only show the call duration msg when there is one call.
						// When there are more than one call, its more important to see that.
						if ( ctiHostedControl.GetNumberOfCalls() == 1 )
						{
							string lengthOfCall = ctiHostedControl.LengthOfCall(longestCall);

							if ( lengthOfCall != "1" )
								info.Text = String.Format( localize.DESKTOP_INFO_CALL_LENGTH_PLURAL, lengthOfCall );
							else
								info.Text = String.Format( localize.DESKTOP_INFO_CALL_LENGTH, lengthOfCall );
						}

						info.Image = warningIcon;
					}
					else
						info.Image = null;
				}
				else
				{
					info.Image = null;
					info.Text = localize.DESKTOP_INFO_NO_CURRENT_CALL;
				}
			}
			catch ( Exception exp )
			{
				info.Text = localize.DESKTOP_ERR_EXCEPTION + " " + exp.Message;
			}
		}

		/// <summary>
		/// This runs periodically to update screen statistics, it uses web services
		/// and its interval is set from a configuration field.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pollTimer_Tick(object sender, System.EventArgs e)
		{
			try
			{
				// Read the interval to poll in seconds
				if ( pollTimer.Interval <= 5000 )
				{
					pollTimer.Interval = secondsBetweenTickPolls;
					if ( pollTimer.Interval < 20 ) // minimum is 20 seconds
						pollTimer.Interval = 20;
					pollTimer.Interval *= 1000;    // convert seconds to ticks
				}

				try
				{
					//ContactCenterStatistics.CallCenterStats stats;
					//stats = GetStats.GetContactCenterStatistics();
					client = new ContactCenterStatistics.ContactCenterStatisticsClient();
					ContactCenterStatistics.ContactCenterStatisticsProviderCallCenterStats stats;
					stats = GetStats.GetContactCenterStatistics();
					ticker.Text = stats.ticker;
				}
				catch ( Exception exp )
				{
					info.Text = localize.DESKTOP_TIMER_TICK_INFO_EXCEPTION + exp.Message;
					return;
				}

			}
			catch ( Exception exp )
			{
				info.Text = localize.DESKTOP_ERR_EXCEPTION + " " + exp.Message;
			}
		}

		public delegate void CallEventDelegate( CallEventArgs args );

		/// <summary>
		/// Handles events from the telephony class.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public void CallEvent( object sender, CallEventArgs args )
		{
			try
			{
				if ( args != null )
					BeginInvoke( new CallEventDelegate(CallEventHandlerOnMainThread), new Object[] { args } );
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_MARSHALL, exp );
			}
		}

		/// <summary>
		/// Prevent recursion while answering a call and looking customer information
		/// </summary>
		private bool alreadyProcessingCall = false;

		/// <summary>
		/// Handles events from the telephony class.
		/// </summary>
		/// <param name="args"></param>
		private void CallEventHandlerOnMainThread(CallEventArgs args )
		{
			string infoText;

			// v1.02 moved out of try-finally block
			if ( !ctiHostedControl.HaveCalls() || ctiHostedControl.GetCalls() == null )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_RECD_CALL_EVENT );
				return;
			}

			try
			{
				int numberOfCalls = ctiHostedControl.GetNumberOfCalls();
				if ( numberOfCalls == 0 )
				{
					SetAgentState( Lookup.LookupProviderPresenceState.Ready, null );
					info.Text = localize.DESKTOP_INFO_NO_CURRENT_CALL;
				}
				else
				{
					if ( numberOfCalls > 1 )
					{
						int cntOnHold = 0;
						int cntOutgoing = 0;
						int cntIncoming = 0;

						CallsClassProvider calls = ctiHostedControl.GetCalls();
						foreach ( CallClassProvider call in calls )
						{
							if ( call.OnHold() )
								cntOnHold++;
							else if ( call.CanAnswer() )
								cntIncoming++;
							else if ( call.State == CallClassProvider.CallState.Ringing )
								cntOutgoing++;
						}

						infoText = localize.DESKTOP_MSG_YOU_ARE_ON + " " + numberOfCalls + " " + localize.DESKTOP_MSG_CALLS;
						if ( cntOnHold > 0 )
							infoText += " - " + cntOnHold + " " + localize.DESKTOP_MSG_ONHOLD;
						if ( cntIncoming > 0 )
							infoText += " - " + cntIncoming + " " + localize.DESKTOP_MSG_RINGING;
						if ( cntOutgoing > 0 )
							infoText += " - " + cntOutgoing + " " + localize.DESKTOP_MSG_OUTGOING;

						if ( info.Text != infoText )  // reduce flicker
							info.Text = infoText;
					}
					else
					{
						CallClassProvider call;
						CallClassProvider.CallState state;

						call = ctiHostedControl.CtiLine.Calls[ 0 ];
						state = call.State;

						if ( state != CallClassProvider.CallState.None )
						{
							// Give a pretty string to the status bar saying who the call
							// is with, assuming we know.
							if ( state == CallClassProvider.CallState.Ringing )
								infoText = localize.DESKTOP_INFOTEXT_CALLING + " ";
							else if ( state == CallClassProvider.CallState.Incoming_Call )
								infoText = localize.DESKTOP_MSG_CALLEDBY + " ";
							else
								infoText = localize.DESKTOP_MSG_CALLWITH + " ";
							if ( call.UserTag != null )
								infoText += call.UserTag + " ";

							infoText = infoText + call.Parties + "  [" + 								
								//CallClassProvider.TextRepOfCallState(state) + "]";
								CallClassProvider.CallStateText(state) + "]";

							if ( info.Text != infoText )  // reduce flicker
								info.Text = infoText;
						}
					}

					// For a new incoming call when we don't already have an authenticated customer,
					// ask the agent to look one up.
					// Also make sure the call is not internal before trying a db lookup.
					// If no callerId is given, show the lookup dialog anyways.
					if ( args.call.CanAnswer() &&
						( args.call.CallerNumber.Length > 6 || args.call.CallerNumber.Length == 0 ) &&
						SessionManager.Count == 0 )  // automatic answer when not handling another customer
					{
						// If the desktop is not the active dialog
						// (a modal is for instance), then don't process yet.
						if ( this.OwnedForms != null && this.OwnedForms.Length > 0 )
							return;

						// TODO: A real ACD autoanswers phones and this isn't
						//   needed in that case.
						args.call.Answer();
						this.AllowLookup = true;
					}

					if ( args.call.State == CallClassProvider.CallState.Connected &&
						!alreadyProcessingCall && 
						( args.call.CallerNumber.Length > 6 || args.call.CallerNumber.Length == 0 ) &&
						this.AllowLookup
						)
					{
						try
						{
							alreadyProcessingCall = true;
							AllowLookup = false;  // way for phoneMenu to flag an incoming call

							// indicate we are trying to authenticate
							if ( this.customerAuthenticated == CustomerAuthentication.NotAuthenticated )
								customerAuthenticated = CustomerAuthentication.KnownButNotAuthenticated;

							CustomerWS.CustomerProviderCustomerRecord []customers = null;
							string phoneNumber;

							// Remove formatting from phone numbers
							phoneNumber = args.call.CallerNumber.Replace( "(", "" );
							phoneNumber = phoneNumber.Replace( ") ", "" );
							phoneNumber = phoneNumber.Replace( "-", "" );
							phoneNumber = phoneNumber.Trim();
					
							if ( phoneNumber != String.Empty )
							{
								try
								{
									customers = customerLookup.GetCustomersByANI( phoneNumber, 2 );
								}
								catch ( Exception exp )
								{
									Logging.Error( Application.ProductName, localize.DESKTOP_ERR_LOOKUP_CALL, exp );
								}
							}
							if ( customers != null && customers.Length == 1 )
							{
								customer = customers[0];
							}
							else if ( args.call.CallerNumber != null &&
								args.call.CallerNumber != String.Empty )
							{
								// create a customer record to do a lookup against.
								// either a new one will be returned, or this one will be
								// tossed, it only exists to pass info to the lookup dialog.
								customer = newCustomer();
								customer.PhoneHome = args.call.CallerNumber;
							}

							// so no more lookups occur until this one is done
							SetEnableSettingForCorePanels( false );
							using ( LookupDlg dlg = new LookupDlg( customerLookup, customer ) )
							{
								if ( dlg.ShowDialog( this ) == DialogResult.OK )
								{
									customer = dlg.Result;

									// Create a blank customer record to use.
									// In real life, the contact center needs to add code to
									// fill in the customer information.
									if ( customer == null )
									{
										customer = newCustomer();
										customer.PhoneHome = args.call.CallerNumber;
									}
								}
								else
								{
									customer = null;
								}
							}

							// If we have a customer, create the new session
							if ( customer != null )
							{
								customerAuthenticated = CustomerAuthentication.Authenticated;

								AddSession( customer, args.call );
							}
						}
						finally
						{
							SetEnableSettingForCorePanels( true );
							// used for phoneMenu to flag an incoming call
							AllowLookup = false;
							alreadyProcessingCall = false;
						}
					}


					// Determine which state we should be in based on the calls we have
					bool setState = false;
					if ( MyState == Lookup.LookupProviderPresenceState.Ready ||
						MyState == Lookup.LookupProviderPresenceState.OnCallHold ||
						MyState == Lookup.LookupProviderPresenceState.OnCallActive )
					{
						CallsClassProvider calls = ctiHostedControl.GetCalls();
						foreach ( CallClassProvider call in calls )
						{
							if ( call.CanUnhold() )
							{
								SetAgentState( Lookup.LookupProviderPresenceState.OnCallHold, null );
								setState = true;
								break;
							}
						}

						if ( !setState )
							SetAgentState( Lookup.LookupProviderPresenceState.OnCallActive, null );
					}


					// If we've connected to another call, make sure
					// its the active session.
					if ( args.state == CallClassProvider.CallState.Connected )
					{
						Session session = SessionManager.GetSession( args.call.CallID );
						if ( session != null )
						{
							SessionManager.SetActiveSession( session.SessionID );
							SetAgentState( Lookup.LookupProviderPresenceState.OnCallActive, null );
						}
					}

				}
			}
			catch ( System.Net.WebException wex )  // v1.02
			{
				// Log the error but proceed
				Logging.Error( Application.ProductName, localize.COMMON_MSG_IIS_ERROR, wex );
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.COMMON_MSG_IIS_ERROR ) >= 0 )
				{
					throw;
				}
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_IN_CALL_EVENT, exp );
			}
		}

		/// <summary>
		/// Set the text of the status toolbar button
		/// </summary>
		/// <param name="status"></param>
		private void SetStatusText(string statusText)
		{
			if (statusText != null)
			{
				// Show the current state in the toolbar
				status.Text = localize.DESKTOP_STATUS_TEXT + statusText;

				// CCF 2.5
				// Close Session tool bar button gets pushed off the screen if the status 
				// text is lengthier. So limit the length of status text to 25 characters,
				// so that Close Session tool bar button.
				if (status.Text.Length > 25)
				{
					status.Text = status.Text.Substring(0, 22) + "...";
				}
			}
		}

		/// <summary>
		/// This sets the agent's presence state via a webservice and then clears the
		/// customer information if the call is done.
		/// </summary>
		/// <param name="presenceState"></param>
		public void SetAgentState(  Lookup.LookupProviderPresenceState presenceState, string name )
		{
			// If on the phone and the agent tries to say Ready, block it
			if( presenceState == Lookup.LookupProviderPresenceState.Ready && ctiHostedControl == null)
			{
				UpdateContextWithStatus(presenceState);
				SetStatusText(presenceState.ToString());
				this.MyState = presenceState;

				return;
			}
			else if(presenceState == Lookup.LookupProviderPresenceState.Ready && ctiHostedControl.HaveCalls() && ctiHostedControl.GetNumberOfCalls() > 0)
			{
				return;
			}

			try
			{
				if ( agentState != null && MyState != presenceState )
				{
					agentState.SetAgentState( (int)presenceState );

					if ( SessionManager != null && SessionManager.ActiveSession != null )
					{
						SessionManager.ActiveSession.PresenceState = (int)presenceState;
					}
					// if the name is not given, look it up so it can be changed on the menu
					if ( name == null )
					{
						for ( int i = 0; i < PresenceStates.Count; i++ )
						{
							if ( PresenceStates[ i ].ID == (int)presenceState )
							{
								name = PresenceStates[ i ].Name;
								break;
							}
						}

						if ( name == null )
						{
							name = presenceState.ToString();
						}
					}

					// Show the current state in the toolbar
					SetStatusText(name);

					if ( presenceState == Lookup.LookupProviderPresenceState.Ready || presenceState == Lookup.LookupProviderPresenceState.LoggedOut )
					{
						info.Image = null;  // no special icon needed for warning
					}

					MyState = presenceState;
					UpdateContextWithStatus(presenceState);
				}
			}
			catch ( Exception exp )
			{
				if ( !loggingOut ) 
				{
					if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
					{
						throw;
					}
					if ( exp.Message.IndexOf( localize.COMMON_MSG_IIS_ERROR ) >= 0 )
					{
						throw;
					}
					Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_SET_STATUS + " " + presenceState.ToString() + ".", exp );
				}
			}
		}

		// Updates the context by putting in the current status in it. This eventually calls NotifyContextChange
		// for all listeners
		private void UpdateContextWithStatus(Lookup.LookupProviderPresenceState presenceState)
		{
			Context context = appHost.GetContext();
			string targetMultichannelStatus = (string)clientToMCProvidersStatusMapping[presenceState.ToString()];
			context["status"] = targetMultichannelStatus;
			appHost.SetContext(context);
		}

		// Used to create unique temporary customer names for new customers
		private int newCustomerNumber = 1;

		/// <summary>
		/// This is where a CCF developer can add the client specific information
		/// for creating a new customer.
		/// </summary>
		/// <returns></returns>
		private CustomerWS.CustomerProviderCustomerRecord newCustomer()
		{
			customer = new CustomerWS.CustomerProviderCustomerRecord();
			customer.CustomerID = "";

			customer.LastName = "New Customer " + newCustomerNumber.ToString();
			newCustomerNumber++;

			return customer;
		}

		/// <summary>
		/// This routine exists to clear the customer information for the next call.
		/// </summary>
		private void noCustomer()
		{
			try
			{
				customerAuthenticated = CustomerAuthentication.NotAuthenticated;
				if ( customer != null )
				{
					customer = null;
				}
			}
			catch ( Exception exp )
			{
				if ( exp.Message.IndexOf( localize.DESKTOP_MSG_SQL_EXIST ) >= 0 )
				{
					throw exp;
				}
				if ( exp.Message.IndexOf( localize.COMMON_MSG_IIS_ERROR ) >= 0 )
				{
					throw exp;
				}
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_UNABLE_RESET_CUSTINFO, exp );
			}
		}

		/// <summary>
		/// WndProc is overriden so we can receive events indicating the agent has
		/// locked and unlocked their workstation.
		/// 
		/// This does NOT work in Windows 2000 and we have made an explicit choice to
		/// only support Windows XP and later.
		/// </summary>
		/// <param name="m"></param>
		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name="FullTrust")]
		protected override void WndProc( ref Message m ) 
		{
			const int WM_WTSSESSION_CHANGE = 0x02B1;  // the message which tells us of changes
			const int WM_DESTROY = 0x2;

			if ( firstTime && m.HWnd.ToInt32() != 0 )
			{
				// This registers the program to receive messages about the computer being locked
				WTSRegisterSessionNotification( m.HWnd, NOTIFY_FOR_THIS_SESSION );
				firstTime = false;
			}
 
			// Listen for operating system messages.
			switch ( m.Msg )
			{
					// The terminal services events tell us when a station is locked/unlocked
				case WM_WTSSESSION_CHANGE:
				{
					switch( m.WParam.ToInt32() )
					{
						case WTS_CONSOLE_CONNECT:
							break;
						case WTS_CONSOLE_DISCONNECT:
							break;
						case WTS_SESSION_LOCK:
							savedState = this.MyState;
							SetAgentState( Lookup.LookupProviderPresenceState.Away, null );
							break;
						case WTS_SESSION_UNLOCK:
							SetAgentState( savedState, null );
							break;
					}
					break;
				}

				case WM_DESTROY:
					if ( !firstTime )
					{
						// Must match the Register invocation
						WTSUnRegisterSessionNotification( m.HWnd );
					}
					break;

				default:
					break;
			}

			base.WndProc( ref m );
		}

		/// <summary>
		/// This is the only way I've found to intercept
		/// all keyboard input before it goes to the hosted applications.  I tried
		/// overrides of WndProc and PreProcessMessage.  I even tried handling
		/// WM_GETDLGCODE.
		/// 
		/// This creates a filter thru which all windows messages pass thru and allow
		/// the app to handle them if it wants to. There are serious performance
		/// implications for doing this since everything passes thru here.
		/// </summary>
		public class MyMessageFilter : IMessageFilter 
		{
			internal DesktopToolBar owner;

			public MyMessageFilter () 
			{
			}

			// This returns false to indicate the message should be handled
			// and true for the message should be tossed.
			public bool PreFilterMessage( ref Message m )
			{
				// Blocks all the messages relating to the left mouse button.
				const int WM_KEYDOWN = 0x100;
				const int WM_SYSKEYDOWN = 0x104;
				//const int WM_CHAR    = 0x102;

				// Listen for operating system messages.
				switch ( m.Msg )
				{
					case WM_SYSKEYDOWN:
					case WM_KEYDOWN:
					{
						bool alt;
						bool ctrl;
						bool shift;
						Keys keys;
						ToolBarButtonClickEventArgs evt = null;

						try
						{
							keys = Control.ModifierKeys;
							alt = (keys & Keys.Alt) != 0;
							ctrl = (keys & Keys.Control) != 0;
							shift = (keys & Keys.Shift) != 0;

							int keyCode;
							keyCode = m.WParam.ToInt32();
							keys = (Keys)keyCode;

							int offset;
							offset = (int)keys - (int)Keys.F1;

							if ( ctrl || shift || alt )
							{
								// If the desktop is not the active dialog
								// (a modal is for instance), then don't process keys here.
								if ( owner.OwnedForms != null && owner.OwnedForms.Length > 0 )
								{
									return false;
								}
							}

							if ( ctrl && shift )
							{
								if(owner.ctiHostedControl != null)
								{
									if ( offset >= 0 && owner.phone.DropDownMenu != null && offset < owner.phone.DropDownMenu.MenuItems.Count )
									{
										owner.phoneMenu.modifyPhoneMenu();
										if ( owner.phone.DropDownMenu.MenuItems[ offset ].Enabled )
										{
											owner.phone.DropDownMenu.MenuItems[ offset ].PerformClick();
											return true;  // toss this event
										}
									}
								}

								// The menu shortcut keys are from Ctrl+Shift+A to Ctrl+Shift+z
								// So that 26 non hosted apps can be configured
								int appOffset = (int)keys - (int)Keys.A;
								if ( appOffset >= 0 && owner.nonHostedApplications.DropDownMenu != null && owner.nonHostedApplications.DropDownMenu.MenuItems.Count > appOffset )
								{
									if ( owner.nonHostedApplications.DropDownMenu.MenuItems[ appOffset ].Enabled )
									{
										owner.nonHostedApplications.DropDownMenu.MenuItems[ appOffset ].PerformClick();
										return true;  // toss this event
									}
								}
							}
							else if ( ctrl )
							{
								if ( offset >= 0 && offset < owner.status.DropDownMenu.MenuItems.Count )
								{
									owner.modifyStatusMenu();
									if ( owner.status.DropDownMenu.MenuItems[ offset ].Enabled )
									{
										owner.status.DropDownMenu.MenuItems[ offset ].PerformClick();
										return true;  // toss this event
									}
								}
							}
							else if ( shift )
							{
								if ( offset >= 0 && offset < owner.requestAssistance.DropDownMenu.MenuItems.Count )
								{
									if ( owner.requestAssistance.DropDownMenu.MenuItems[ offset ].Enabled )
									{
										owner.requestAssistance.DropDownMenu.MenuItems[ offset ].PerformClick();
										return true;  // toss this event
									}
								}
							}
								// Handle any Alt-shortcuts
							else if ( alt )
							{
								switch ( keys )
								{
									case Keys.A:
										evt = new ToolBarButtonClickEventArgs(owner.requestAssistance);
										break;

									case Keys.L:
										evt = new ToolBarButtonClickEventArgs(owner.lookup);
										break;

									case Keys.P:
										evt = new ToolBarButtonClickEventArgs(owner.phone);
										break;

									case Keys.I:
										evt = new ToolBarButtonClickEventArgs(owner.nonHostedApplications);
										break;

									case Keys.S:
										evt = new ToolBarButtonClickEventArgs(owner.status);
										break;

									case Keys.H:
										evt = new ToolBarButtonClickEventArgs(owner.help);
										break;
								}

								if ( evt != null )
								{
									owner.toolBar_ButtonClick( this, evt );
									return true;  // means to ignore further processing of this msg
								}

								return false;
							}


							// This is for moving the focus from one tab to the next,
							// regardless of what is being hosted in the tab.
							if ( ctrl && keys == Keys.Tab )
							{
								CCFPanel active;

								active = CCFAppsUI.ActivePanel;
								if ( active != null )
								{
									TabControl tabControl = active.TabControl;

									// Move to next tab in the group or to next group
									if ( tabControl != null && 
										tabControl.SelectedIndex < tabControl.TabCount - 1 )
									{
										++tabControl.SelectedIndex;
									}
									else
									{
										// v1.0 allowed multiple tab groups which was more flexible.
										//    v1.1 removed two of these and placed session and workflow
										//    panels there.  These additions probably should've been
										//    hosted applications of their own so customers could
										//    add/change/remove them more easily.
										active = owner.AppsUI.NextPanel();
										if ( active != null )
										{
											if ( active.TabControl != null )
												active.TabControl.SelectedIndex = 0;
											else
												active.Controls[0].Focus();
										}
									}

									return true;  // means to ignore further processing of this msg
								}
							}
						}
						catch ( Exception exp )
						{
							Logging.Error( Application.ProductName, localize.DESKTOP_ERR_PREFILTERING_MSG, exp );
						}

						break;
					}
				}
				return false;
			}
		}

		//  -------------- Session and Workflow events ------------------------------ 

		/// <summary>
		/// Configures the events which tie various components together.
		/// </summary>
		private void InitializeSessionExplorerAndWorkflow()  // v1.02 made private and added other events
		{
			if ( SessionManager == null )
			{
				return;   // program is exiting
			}
			SessionManager.SessionShowEvent += new SessionShowHandler(SessionManager_SessionShowEvent);
			SessionManager.SessionHideEvent += new SessionHideHandler(SessionManager_SessionHideEvent);
			SessionManager.SessionCloseEvent += new SessionCloseHandler(SessionManager_SessionCloseEvent);

			if(workFlowControl != null)
			{
				workFlowControl.WorkflowStatusChange += new WorkFlowEventHandler(WorkflowStatusChanged);
			
				// Workflow Driven Implementation:
				// New event to support workflow change notification.
				workFlowControl.WorkflowStarted +=new WorkFlowEventHandler(WorkflowStarted);
			}
			
			if (customerWorkflowManager != null)
			{
				customerWorkflowManager.FocusHostedApp += new WorkFlowEventHandler(WorkflowFocusHostedApp);
			}

			if(sessionExplorerControl != null)
			{
				this.sessionExplorerControl.SessionClosed += new SessionExplorerEventHandler(SessionExplorer_CloseSession);
			}

			// If not using the session explorer, then just hide it
			if ( !this.Use_SessionExplorer )
			{
				if(sessionExplorerControl != null)
				{
					this.sessionExplorerControl.ControlVisible = false;
				}
			}
		}

		/// <summary>
		/// For when closing down CCF desktop.  This speeds it up by reducing the
		/// number of events being fired since we won't need them.
		/// </summary>
		private void FreeSessionExplorerAndWorkflow()  // v1.02 added
		{
			if(workFlowControl != null)
			{
				workFlowControl.WorkflowStatusChange -= new WorkFlowEventHandler(WorkflowStatusChanged);
				
				// Workflow Driven Implementation:
				// New event to support workflow change notification.
				workFlowControl.WorkflowStarted -=new WorkFlowEventHandler(WorkflowStarted);
			}

			if (customerWorkflowManager != null)
			{
				customerWorkflowManager.FocusHostedApp -= new WorkFlowEventHandler(WorkflowFocusHostedApp);
			}

			AppsUI.SelectedAppChanged -= new SelectedAppChangedHandler(panel_SelectedIndexChanged);

			if ( SessionManager != null )
			{
				SessionManager.SessionShowEvent -= new SessionShowHandler(SessionManager_SessionShowEvent);
				SessionManager.SessionHideEvent -= new SessionHideHandler(SessionManager_SessionHideEvent);
			}
		}

		/// <summary>
		/// True if a new session is currently being created
		/// </summary>
		private bool StartingNewSession = false;

		/// <summary>
		/// Adds a new session to the CCF desktop.  Uses the customer record and any
		/// call information to prepopulate the session.
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="call"></param>
		/// <returns></returns>
		private Session AddSession( CustomerWS.CustomerProviderCustomerRecord customer,
			CallClassProvider call  // may be null if there is no CTI or no call 
			)
		{
			Cursor saveCursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;

			try
			{
				int callID = (call != null ? call.CallID : -1);

				string sessionName = null;
				if ( customer != null )
				{
					// Check if there is a firstname
					if ( customer.FirstName != null && customer.FirstName != String.Empty )
					{
						sessionName = customer.FirstName + " " + customer.LastName;
					}
					else
					{
						sessionName = customer.LastName;
					}
				}

				// Check if session already exists and if so, use it
				// Empty CustomerID means its a customer that is not in
				// the database.
				Session session = null;
				if ( customer != null && customer.CustomerID != String.Empty )
					session = SessionManager.GetSession( customer.CustomerID );
				if ( session != null )
				{
					SessionManager.SetActiveSession( session.SessionID );
					if ( session.CallID == -1 )
					{
						session.CallID = callID;
					}
				}
				else
				{
					StartingNewSession = true;
					SessionManager.AddSession( sessionName, callID, customer );
					if (SessionManager.Throttled)
					{
						// TODO Localize this message
						MessageBox.Show("Maximum number of sessions reached.", Application.ProductName, MessageBoxButtons.OK);
					}
					else
					{
						// Subscribe to various events.
						//   It is already set if not using multiple sessions and this
						//   is the second time through here.
						if ( this.Use_SessionExplorer || appHost.Count == 0 )
						{
							AppsUI.SetRedraw( false );  // reduce flicker

							// Subscribe for the Request Action Status event in Application host
							appHost.RequestActionStatus += new ApplicationHost.RequestActionStatusHandler( HandleRequestActionStatus );

							// Subscribe to the appHost to request application creation events.
							// This capability is useful if the appHost needs to create an app
							// at runtime instead of just at startup.
							appHost.RequestApplicationCreation += new ApplicationHost.RequestApplicationCreationHandler( startApplication );

							// Subscribe to the appHost to request application close event.
							appHost.RequestApplicationClose += new ApplicationHost.RequestApplicationCloseHandler(stopApplication);
							
							// Calling the modified LoadApplication method of ApplicationHost so that 
							// only the global applications are loaded in case of a global session and 
							// non-global applications are loaded depending on workflow status for normal
							// session.
							appHost.LoadApplications(null, sessionName == null, workflowExists);

							// This allows applications to push the focus to another application when
							// 'sending' an action to that application.
							appHost.RequestFocus += new ApplicationHost.RequestFocusEventHandler( appHostRequest_Focus );
						}

						// Scan for applications that implement IHostedApplication4
						// and hook in configuration reader
						foreach (IHostedApplication app in this.AppsUI.Applications)
						{
							IHostedApplication4 app4 = app as IHostedApplication4;
							if (app4 != null && app4.ConfigurationReader == null)
							{
								app4.ConfigurationReader = ConfigurationReader;
							}
						}

						// Workflow Driven Implementation:
						// Let all the applications initialized. The global app initialization checking 
						// will be done by this method internally.
						appHost.InitializeApplications();

						// moved here so the context is set AFTER the
						// session has been created.  Otherwise all the previous
						// sessions have the wrong context assigned.
						SetContext( call );

						if(currentSessionControl != null)
						{
							currentSessionControl.FireRequestAction(
								new RequestActionEventArgs(currentSessionControl.ApplicationName, "LoadAllSessions", string.Empty));
						}

						if ( !SessionManager.ActiveSession.Global )
						{
							// enable any applicable workflows
							if(workFlowControl != null)
							{
								workFlowControl.WorkflowUpdate( this.AgentID, SessionManager.ActiveSession.SessionID );
							}

							// If not a global session, there may be transferred info
							// about this one from another agent
							GetTransferredSessionInfo();
						}
					}
				} // if (!SessionManager.Throttled)

				// once the session is added, save it and other sessions (unless this is the global one)
				StartingNewSession = false;
				if ( SessionManager.ActiveSession != null && !SessionManager.ActiveSession.Global )
				{
					SaveSessions();
				}
				// Keep track of how many sessions on the UI
				btnSessions.Text = "Sessions";
				if ( SessionManager.Count > 0 )
					btnSessions.Text += " - " + SessionManager.Count;
			}
			catch ( System.Net.WebException wex )
			{
				// Log the error and proceed
				Logging.Error( Application.ProductName, localize.DESKTOP_IIS_ERROR, wex );
			}
			finally
			{
				if(sessionExplorerControl != null)
				{
					sessionExplorerControl.AddSession( SessionManager.ActiveSession, true );
				}

				StartingNewSession = false;

				Cursor.Current = saveCursor;
				AppsUI.SetRedraw( true );
			}

			return SessionManager.ActiveSession;
		}

		/// <summary>
		/// Used to prevent repeated calls to Sync the session explorer and
		/// current session UI when we are removing tabs.  Each tab that is removed
		/// often causes the focus to change thus firing off the focus events.
		/// </summary>
		private bool dontBotherCertainOperations = false;

		/// <summary>
		/// This keeps the various panels in sync with the current session and application.
		/// </summary>
		/// <param name="appWithFocus"></param>
		private void SyncSessionExplorerApplicationSelection( IHostedApplication appWithFocus )
		{
			if ( !dontBotherCertainOperations )
			{
				if(sessionExplorerControl != null)
				{
					sessionExplorerControl.FocusOnApplication( appWithFocus );
				}

				if(currentSessionControl != null)
				{
					currentSessionControl.FireRequestAction(
						new RequestActionEventArgs(currentSessionControl.ApplicationName, "FocusedAppChanged", string.Empty));
				}

				// v1.02 - added so calls follow the session
				if(ctiHostedControl != null && ctiHostedControl.HaveCalls())
				{
					if (SessionManager != null && SessionManager.ActiveSession != null)
					{
						if (ctiHostedControl.Unhold(SessionManager.ActiveSession.CallID))
						{
							// place any call on hold that can be held
							phoneMenu.hold_Click(null, null);
						}
					}
				}
			}
		}

		/// <summary>
		/// Event handler for when session manager wishes to show a session.
		/// </summary>
		/// <param name="session"></param>
		private void SessionManager_SessionShowEvent( Session session )
		{
			dontBotherCertainOperations = true;
			try
			{
				AppsUI.SetRedraw( false );

				foreach ( IHostedApplication app in session )
				{
					if ( app.TopLevelWindow != null )
					{
						// If app is a hidden global dynamic app then don't add
						// to UI and just continue
						if (appHost.IsHiddenGlobalDynamicApp(app))
						{
							continue;
						}

						// if workflow has not start then dont add the tagged applications
						if(session.Workflow == string.Empty)
						{
							if(!appHost.IsTaggedApplication(app))
							{
								this.AddApplicationToUI(app);	
							}
						}
						else
						{
							this.AddApplicationToUI(app);
						}
					}
				}

				// if showing the session, give the focus to the app we know should have it.
				CCFAppsUI.AppWithFocus = session.FocusedApplication;

				if ( session.FocusedApplication != null )
				{
					this.setFocusForApplicationID( session.FocusedApplication.ApplicationID );
				}
				// Enable the close session button if there is something it can do
				if ( SessionManager.ActiveSession != null && !SessionManager.ActiveSession.Global )
				{
					this.closeSession.Enabled = true;
				}
			}
			finally
			{
				dontBotherCertainOperations = false;
				AppsUI.SetRedraw( true );
			}

			SyncSessionExplorerApplicationSelection( CCFAppsUI.AppWithFocus );

			customer = ((AgentDesktopSession)session).Customer;

			// Update the workflow for the current session
			if ( appHost != null && !session.Global )
			{
				if(workFlowControl != null)
				{
					workFlowControl.WorkflowUpdate( this.AgentID, session.SessionID );
					session.IsWorkflowPending = workFlowControl.IsWorkflowPending (this.AgentID);
				}

				// restore the agent state for this session
				if ( session.PresenceState != -1 )
				{
					SetAgentState( (LookupProviderPresenceState)session.PresenceState, null );
				}
			}
			else
			{
				if(workFlowControl != null)
				{
					workFlowControl.Clear();
				}

				this.SetAgentState( Lookup.LookupProviderPresenceState.Ready, null );
			}

			// If showing the global session
			if (session.Name == null)
			{
				UpdateDynamicApplicationMenu(true);
			}
			else 
			{
				UpdateDynamicApplicationMenu(false);
			}

			SaveSessions();
		}

		/// <summary>
		/// Add the application to the UI
		/// </summary>
		private void AddApplicationToUI(IHostedApplication app)
		{
			// Used to allow developers to customize their applications with their
			// own XML tags in the Applications table.
			string initializationXml = appHost.GetApplicationInitializationXML( app.ApplicationID );

			if (appHost.IsIsolatedApplication(app))
			{
				IHostedAppUICommand appUICmd = app as IHostedAppUICommand;
				if (null != appUICmd)
				{
					appUICmd.ShowForm();
				}
			}
			else
			{
				bool provideCloseButton = false;

				// Only provide close button if app is dynamic and can be closed dynamically.
				if (appHost.IsDynamicApplication(app))
				{
					if (appHost.CanCloseDynamicApplication(app))
					{
						provideCloseButton = true;
					}
				}

				this.AppsUI.AddApplication( app.DisplayGroup, app, initializationXml, provideCloseButton );
			}		
		}

		/// <summary>
		/// Create the context for the hosted applications to use.
		/// </summary>
		/// <returns>returns the context GUID or an empty guid</returns>
		/// <remarks>Post 1.02 patch - returns a guid since that's useful for
		/// many of the CCF engagement desktops.</remarks>
		private Guid SetContext(CallClassProvider call)
		{
			Context context = new Context();
			Cursor saveCursor = Cursor.Current;
			Guid contextID = Guid.Empty;

			try
			{
				Cursor.Current = Cursors.WaitCursor;

				if(customer != null)
				{
					context["CustomerID"] = customer.CustomerID;
					context["CustomerAuthenticated"] = customerAuthenticated.ToString();
					context["CustomerFirstName"] = customer.FirstName;
					context["CustomerLastName"] = customer.LastName;

					// Set this so later UI elements can show the name as found
					// in the database.  Especially useful when a call center doesn't
					// get the name from the phone network (ANI doesn't include name)
					if ( call != null )
					{
						// Post 1.02 - check if there is a firstname
						if ( customer.FirstName != null && customer.FirstName != String.Empty )
						{
							call.UserTag = customer.FirstName + " " + customer.LastName;
						}
						else
						{
							call.UserTag = customer.LastName;
						}
					}

					context["Street"] = customer.Street;
					context["City"] = customer.City;
					context["State"] = customer.State;
					context["ZipCode"] = customer.ZipCode;

					// commented out since the Telecom Starter Kit doesn't allow an address field for country
#if NOT_USED
					context["Country"] = customer.Country;
#endif

					context["Landline"] = customer.PhoneHome;
					context["MobilePrepaid"] = customer.PhoneMobile;

				}

				// If we have call information, store that too.
				if(ctiHostedControl != null)
				{
					if ( call != null )
					{
						context["ANI"] = call.CallerNumber;
						context["DNIS"] = call.CalledNumber;

						// to assist any hosted apps that wish to work with the call
						context["CallID"] = call.CallID.ToString();

						// to assist any hosted apps that wish to work with the call
						context["CallGuid"] = call.ID.ToString();
					}
				}

				// In case the hosted apps couldn't start.  Not sure what use the app is
				// without them though.
				if ( appHost != null )
				{
					appHost.ExecuteDefaultActions();

					// v1.02 - SetContext calls CreateContext if it needs to create a new GUID
					contextID = appHost.SetContext(context);

					if ( customer != null )
					{
						appHost.CreateApplicationState( customer.CustomerID );
						if( customer.CustomerID != String.Empty)
						{
							appHost.ExecuteApplicationState();
						}
					}
				}

			}
			catch( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_ERR_SETTING_CONTEXT_APP, exp );
			}
			finally
			{
				Cursor.Current = saveCursor;
			}

			return contextID;
		}

		/// <summary>
		/// Event handler for when the session manager wishes to remove a session
		/// from view.
		/// </summary>
		/// <param name="session"></param>
		private void SessionManager_SessionHideEvent( Session session )
		{
			dontBotherCertainOperations = true;
			try
			{
				AppsUI.SetRedraw( false );

				// Do the selectedTab last so we get fewer screen updates.
				// Because removing apps causes actions and focus events to fire,
				// just make sure all sessions are gone rather than looking for just
				// AppWithFocus.
				foreach ( IHostedApplication app in session )
				{
					// Workflow Driven Implementation:
					// Hide/Remove only the non-global and tagged global applications.

					//if ( app.TopLevelWindow != null && !appHost.IsGlobalApplication( app ) )
					if ( app.TopLevelWindow != null && (!appHost.IsGlobalApplication( app ) || appHost.IsTaggedGlobalApplication(app)))
					{
						AppsUI.RemoveApplication( app );
					}
				}

				// Disable the close session button until a session becomes active
				this.closeSession.Enabled = false;
			}
			finally
			{
				dontBotherCertainOperations = false;
				AppsUI.SetRedraw( true );
			}
		}

		/// <summary>
		/// Event triggered by the SessionManager when it closes a session.
		/// </summary>
		/// <param name="session"></param>
		/// <returns>true to continue, false to cancel</returns>
		private bool SessionManager_SessionCloseEvent( Session session )
		{
			btnSessions.Text = "Sessions";
			if ( SessionManager.Count > 1 )
				btnSessions.Text += " - " + (SessionManager.Count - 1).ToString();

			// v1.02 Added so calls aren't just left hanging.
			// Customers may wish to change this depending on how they want
			// the actual call dealt with when an agent closes the session.	
			if(ctiHostedControl != null)
			{
				if(ctiHostedControl.CanUnhold(session.CallID))
				{
					if ( MessageBox.Show( localize.SESSION_HAS_A_HELD_CALL,
						this.Text,  // caption matches UI caption
						MessageBoxButtons.YesNo ) == DialogResult.Yes )
					{
						ctiHostedControl.Unhold(session.CallID);
					}
					else
					{
						return false;
					}
				}

				if(ctiHostedControl.CanHangUp(session.CallID))
				{
					if ( MessageBox.Show( localize.SESSION_HAS_A_CONNECTED_CALL,
						this.Text,  // caption matches UI caption
						MessageBoxButtons.YesNo ) == DialogResult.Yes )
					{
						ctiHostedControl.HangUp(session.CallID);
					}	
					else
					{
						return false;
					}
				}
			}

			// Clean up desktop UI unless exiting in which case, don't bother
			if ( !loggingOut )
			{
				if(sessionExplorerControl != null)
				{
					sessionExplorerControl.RemoveSession( session );
				}

				if(currentSessionControl != null)
				{
					currentSessionControl.FireRequestAction(
					new RequestActionEventArgs(currentSessionControl.ApplicationName, "RemoveSession", session.SessionID.ToString()));
				}

				SaveSessions();
				noCustomer();
			}

			return true;
		}

		/// <summary>
		/// Event Handler for start/end of workflow.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WorkflowFocusHostedApp(object sender, WorkflowArgs e)
		{
			setFocusForApplicationID( e.ApplicationId );
		}

		/// <summary>
		/// Called when the workflow status has been changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WorkflowStatusChanged(object sender, WorkflowArgs e)
		{
			if ( SessionManager.ActiveSession != null )
				SessionManager.ActiveSession.IsWorkflowPending = (e.WorkflowStatus == 0);

			// If workflow is done or is canceled then remove the workflow applications from the UI and from Session Explorer
			if( e.WorkflowStatus == (int)WorkflowStatus.Complete || e.WorkflowStatus == (int)WorkflowStatus.Cancel)
			{
				this.RemoveWorkflowAppsFromUI();

				if(sessionExplorerControl != null)
				{
					sessionExplorerControl.RemoveWorkflowApplicationNodes(SessionManager.ActiveSession);
				}
				this.UpdateDynamicApplicationMenu(false);
			}
		}

		/// <summary>
		/// Function to save the state of sessions for an agent
		/// </summary>
		private void SaveSessions()
		{
			// If we are saving sessions and not starting a new one right now
			if ( this.Use_SaveSession && !StartingNewSession )
			{
				try
				{
					string savedSessions;

					// for debug only, no need to translate
					Logging.Trace( Application.ProductName, "SaveSessions()" );

					savedSessions = SessionManager.Save();

					if ( agentState != null && lastSavedSessions != savedSessions )
					{
						agentState.SaveSessions( this.AgentID, savedSessions );
						lastSavedSessions = savedSessions;
					}
				}
				catch ( System.Net.WebException wex )
				{
					// Web Server is down
					System.Diagnostics.EventLog eventLog = new System.Diagnostics.EventLog();
					// Event source should have already been setup on initial install
					if (System.Diagnostics.EventLog.SourceExists(EVENTSOURCE))
					{
						eventLog.Source = EVENTSOURCE;
						eventLog.WriteEntry(localize.COMMON_MSG_IIS_ERROR + " " + wex.Message, System.Diagnostics.EventLogEntryType.Error);
					}
					//the web server is down
					wsdown = true;
				}
				catch ( Exception exp )
				{
					Logging.Error( Application.ProductName, localize.DESKTOP_APP_SAVE_ERROR, exp );
				}
			}
		}

		/// <summary>
		/// Restores the sessions that were in use when an agent logged out or
		/// had a system error.
		/// </summary>
		private void GetSessions()
		{
			try
			{
				if ( this.Use_SaveSession && appHost != null )
				{
					// for debug only, no need to translate
					Logging.Trace( Application.ProductName, "GetSessions()" );

					// Get information about all saved sessions for this agent
					string sessionsXml = agentState.GetSavedSessions( this.AgentID );

					// see if there is any saved session information
					if ( sessionsXml != null && sessionsXml != String.Empty )
					{
						XmlDocument doc = new XmlDocument();
						doc.LoadXml( sessionsXml );

						XmlNodeList nodeList = doc.SelectNodes( "descendant::Session" );
						if ( nodeList != null )
						{
							// Current session being re-loaded
							Session currentSession = null;
							// The session that was marked active
							Session activeSession = null;

							foreach ( XmlNode sessionNode in nodeList )
							{
								if ( sessionNode.InnerXml != String.Empty )
								{
									// Post 1.02 patch 3 - changed to use newCustomer() instead
									//    of new CustomerWS.CustomerRecord().
									// Need a fake customer to create a new session with
									CustomerProviderCustomerRecord tempCustomer = newCustomer();
									this.AddSession( tempCustomer, null );

									// if the session was created, reset it to the previous state
									if ( SessionManager.ActiveSession != null )
									{
										// Restore returns true if the session was marked active
										if ( SessionManager.ActiveSession.Restore( sessionNode.OuterXml ) )
										{
											// Note active session
											activeSession = SessionManager.ActiveSession;
										}
										// Note current session (which maybe the same as the active session)
										currentSession = SessionManager.ActiveSession;

										// Workflow Driven Implementation:
										// In order to restore the workflows and update the UI, we need to 
										// set this restored session as 
										SessionManager.SetActiveSession( SessionManager.ActiveSession.SessionID );
										// Refresh current session if appropriate
										if ( currentSession != null && !currentSession.Global )
										{
											refreshSession( currentSession );
										}
									}
								}
							}

							// If active session was not null make sure it is the active one...
							if ( activeSession != null)
							{
								SessionManager.SetActiveSession( activeSession.SessionID );
							}
							// Do final update of UI items based on the session
							// Re-grab the active session incase it was not set up (if somehow no 
							// session was active, the last session loaded should be it)
							activeSession = SessionManager.ActiveSession;
							if ( activeSession != null && !activeSession.Global )
							{
								refreshSession( activeSession );
							}
						}
					}
				}
			}
			catch ( System.Net.WebException wex )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_IIS_ERROR, wex );
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_APP_RESTORE_ERROR, exp );
			}
			finally
			{
				// clear out any transfer information for this agent
				agentState.SetSessionTransferInformation( this.AgentID, "", this.AgentID, "" );

				if(sessionExplorerControl != null)
				{
					sessionExplorerControl.ControlRefresh();
				}
			}
		}

		/// <summary>
		/// Gets (if any) information for a transferred session.
		/// </summary>
		private void GetTransferredSessionInfo()
		{
			Session currentSession = SessionManager.ActiveSession;
			if ( currentSession == null )
			{
				return;
			}
			try
			{
				BindingList<AgentStats.AgentStatsProviderSessionTransferRecord> transferRecords = agentState.GetSessionTransferInformation(this.AgentID);

				if ( transferRecords != null && transferRecords.Count > 0 &&
					transferRecords[0].sessionInfo != String.Empty )
				{
					currentSession.Restore( transferRecords[0].sessionInfo );
					refreshSession( currentSession );
				}
			}
			catch ( Exception exp )
			{
				// This is for debug, not needed to be translated
				// ignore all errors, they mean we don't have a session
				// transferred to us or the information is incorrect.
				Logging.Trace( "Problem handing transferred session: ", exp.Message );
			}
			finally
			{
				// clear out any transfer information for this agent
				agentState.SetSessionTransferInformation( this.AgentID, "", this.AgentID, "" );
			}
		}

		/// <summary>
		/// Refreshes all the UI elements with the current session information.
		/// Not to be used except when a lot has changed, such as reloading
		/// a number of saved sessions or a session transfer.
		/// </summary>
		private void refreshSession( Session currentSession )
		{
			if ( currentSession != null )
			{
				customer = ((AgentDesktopSession)currentSession).Customer;

				this.customerAuthenticated = CustomerAuthentication.Authenticated;

				// restore the agent state for this session
				if ( currentSession.PresenceState != -1 )
					SetAgentState( (Lookup.LookupProviderPresenceState)currentSession.PresenceState, null );

				if(workFlowControl != null)
				{
					workFlowControl.Clear();
					workFlowControl.WorkflowUpdate( this.AgentID, currentSession.SessionID );
				}

				appHost.CreateApplicationState(customer.CustomerID);

				if(sessionExplorerControl != null)
				{
					sessionExplorerControl.RefreshView();
				}

				if(currentSessionControl != null)
				{
					currentSessionControl.FireRequestAction(
						new RequestActionEventArgs(currentSessionControl.ApplicationName, "LoadAllSessions", string.Empty));
				}

				// Select the app the agent was looking at
				if ( currentSession.FocusedApplication != null )
				{
					this.AppsUI.SelectApplication( currentSession.FocusedApplication.ApplicationID );
					
					if(sessionExplorerControl != null)
					{
						sessionExplorerControl.FocusOnApplication( currentSession.FocusedApplication );
					}
				}
			}
		}

		/// <summary>
		/// Show or hide the sessions panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnSessions_Click(object sender, System.EventArgs e)
		{
			try
			{
				// if it will become visible
				if( sessionExplorerControl != null )
				{
					// Get the form the sessionExplorerControl TopLevelWindow is on
					// TopLevelWindow -> CcfDeckControl -> CcfPanel -> FloatingWindowsForm
					Form form = sessionExplorerControl.ControlParent.Parent.Parent as Form;

					if (form != null)
					{
						if (!form.Visible)
						{
							form.Activate();
							form.Location = new Point(0, this.Height);
						}
						form.Visible = !form.Visible;
					}
				}
			}
			catch {}
		}

		/// <summary>
		/// Show or hide the workflow panel.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnWorkflow_Click(object sender, System.EventArgs e)
		{
			try
			{
				// if it will become visible
				if (workFlowControl != null)
				{
					// Get the form the workFlowControl's TopLevelWindow is on
					// TopLevelWindow -> CcfDeckControl -> CcfPanel -> FloatingWindowsForm
					Form form = workFlowControl.ControlParent.Parent.Parent as Form;

					if (form != null)
					{
						if (!form.Visible)
						{
							form.Activate();
							form.Location = new Point(btnWorkflow.Left, this.Height);
						}
						form.Visible = !form.Visible;
					}
				}
			}
			catch {}
		}

		/// <summary>
		/// Show or hide the CurrentSession Panel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnCurrentSession_Click(object sender, System.EventArgs e)
		{
			Point point = new Point(btnCurrentSession.Left, this.Height);
			string location = GeneralFunctions.Serialize(point);
			if( currentSessionControl != null )
			{
				currentSessionControl.FireRequestAction(
					new RequestActionEventArgs(currentSessionControl.ApplicationName, "Show_Hide", location));
			}
		}
		
		/// <summary>
		/// Session explorer event when a session is closed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SessionExplorer_CloseSession(object sender, EventArgs e)
		{
			this.doCloseSession();
		}

		/// <summary>
		/// Workflow Driven Implementation:
		/// Checks if the application host should be updated.
		/// </summary>
		/// <param name="workflow">Workflow object scanned for Hosted Application IDs to add.</param>
		/// <returns>True if Apllications are loaded and ready.</returns>
		private bool RequiresApplicationHostUpdate(WorkflowData workflow)
		{
			ArrayList appIds = new ArrayList();

			if (workflow != null && workflow.Steps.Count > 0)
			{
				foreach (WorkflowStep step in workflow.Steps)
				{
					appIds.Add(step.HostedApplicationId);
				}
			}

			return appHost.RequiresApplicationsReload(appIds);
		}

		/// <summary>
		/// Workflow Driven Implementation:
		/// Applies the saved state to the applications in process of session restoration.
		/// </summary>
		/// <param name="stateXml"></param>
		private void ApplySavedState(string stateXml)
		{
			if (stateXml == null || stateXml == string.Empty)
			{
				return;
			}

			XmlDocument doc = new XmlDocument();
			doc.LoadXml(stateXml);

			// Get the saved application state and pass it to the current apps
			XmlNodeList nodeList = doc.SelectNodes( "descendant::Application" );
			if ( nodeList != null )
			{
				foreach ( XmlNode appNode in nodeList )
				{
					XmlNode stateNode = appNode.SelectSingleNode( "State" );
					if ( stateNode != null )
					{
						XmlAttribute id = appNode.Attributes[ "id" ];
						IHostedApplication app = appHost.GetApplication( Convert.ToInt32(id.Value) );
						if ( app != null && (!appHost.IsGlobalApplication(app) || appHost.IsTaggedGlobalApplication(app)))
						{
							app.SetStateData( stateNode.InnerXml );
						}
					}
				}
			}
		}

		/// <summary>
		/// Remove all the tagged global and tagged non-global applications from the UI
		/// </summary>
		private void RemoveWorkflowAppsFromUI()
		{
			try
			{
				dontBotherCertainOperations = true;

				this.AppsUI.SetRedraw( false );
				foreach ( IHostedApplication app in SessionManager.ActiveSession )
				{
					if ( app.TopLevelWindow != null )
					{
						//if (!(appHost.IsGlobalApplication(app)) || appHost.IsTaggedGlobalApplication(app))
						if (appHost.IsTaggedApplication(app))
						{
							// If the application is global and isolated then hide the form.
							// TODO Review for what about non-global isolated app.
							if (appHost.IsGlobalApplication(app) && appHost.IsIsolatedApplication(app))
							{
								IHostedAppUICommand appUICommand = app as IHostedAppUICommand;
								appUICommand.HideForm();
							}
							else
							{
								// If the application is hosted in the Agent Desktop then remove it from UI
								this.AppsUI.RemoveApplication( app );
							}

							// If dynamic then unload
							if (appHost.IsDynamicApplication(app))
							{
								appHost.UnloadDynamicApplication(app);
							}
						}
					}
				}
			}
			finally
			{
				dontBotherCertainOperations = false;
				this.AppsUI.SetRedraw( true );
			}
		}

		/// <summary>
		/// Add currently running tagged global and non-global workflow apps back to the UI
		/// </summary>
		private void addWorkflowAppsToUI()
		{
			foreach ( IHostedApplication app in SessionManager.ActiveSession )
			{
				if ( app.TopLevelWindow != null && !appExistsInUI(app.ApplicationName) )
				{
					if (appHost.IsTaggedApplication(app))
					{
						// If the application is hosted in the Agent Desktop then add it to the UI
						this.AddApplicationToUI(app);
					}
				}
			}
		}

		/// <summary>
		/// Workflow Driven Implementation: Handles the change in the workflow and depending on 
		/// the change it manages the status of hosted applications.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		/// <returns></returns>
		private void WorkflowStarted(object sender, WorkflowArgs e)
		{
			// Check if the application host should be updated
			if (!RequiresApplicationHostUpdate(e.CurrentWorkflow))
			{
				addWorkflowAppsToUI();

				// Add workflow applications to the list of hosted applications displayed in the tree.
				if(this.Use_SessionExplorer && sessionExplorerControl != null)
				{
					sessionExplorerControl.AddWorkflowApplicationNodes(SessionManager.ActiveSession, true);
				}
				return;
			}

			// First remove all the tagged non-global and tagged global applications from UI.
			this.RemoveWorkflowAppsFromUI();

			// If there is an workflow started then ask ApplicationHost to load/unload the 
			// required applications for this workflow.
			if (e.CurrentWorkflow != null)
			{
				ArrayList appIds = new ArrayList(e.CurrentWorkflow.Steps.Count);

				foreach (WorkflowStep step in e.CurrentWorkflow.Steps)
				{
					appIds.Add(step.HostedApplicationId); 
				}
				
				bool success = appHost.LoadApplications(appIds, false, workflowExists);

				if(!success)
				{
					// Applications failed to load, log error and close desktop
					Logging.Error(localize.DESKTOP_MODULE_NAME, localize.DESKTOP_ERR_LOADING_APPLICATIONS);
					this.Close();
				}

				// Show the isolated applications in the new active apps list.
				foreach (IHostedApplication app in SessionManager.ActiveSession)
				{
					if (appHost.IsIsolatedApplication(app) && appHost.IsTaggedGlobalApplication(app))
					{
						((IHostedAppUICommand)app).ShowForm();
					}
				}

				// Initialize the new apps loaded for the workflow
				appHost.InitializeApplications();

				// Execute the default actions on the new apps loaded for the workflow
				appHost.ExecuteDefaultActions(true);

				// Set the context of the new apps loaded for the workflow.
				appHost.NotifyContextChange(true);

				// Apply the application state.
				appHost.ExecuteApplicationState(true);

				// If the workflow has been restored from the saved session, then apply the 
				// saved app state to the workflow dependant apps.
				if (e.WorkflowStatus == 3)
				{
					ApplySavedState(SessionManager.ActiveSession.RestoredSessionInfo);
				}
			}
				// If a workflow just ended then unload the non-global apps and
				// hide the tagged global apps.
			else
			{
				appHost.LoadApplications(null, false, workflowExists);
				
				// Set the first untagged global application to focus as we dont know which app should 
				// come into focus after the worflow ends.
				setFocusForApplicationID( SessionManager.ActiveSession.AppHost[0].ApplicationID );
			}
			// Add workflow applications to the list of hosted applications displayed in the tree.
			if(this.Use_SessionExplorer && sessionExplorerControl != null)
			{
				sessionExplorerControl.AddWorkflowApplicationNodes(SessionManager.ActiveSession, true);
			}

			UpdateDynamicApplicationMenu(false);
		}

		/// <summary>
		/// Produces the grapical look to the background of the toolbar.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Desktop_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics myGraphics;

			myGraphics = e.Graphics;
			myGraphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.GammaCorrected;

			Rectangle rect = new Rectangle( 0, 0, this.Width-1, this.Height-1 );
			using ( LinearGradientBrush lgBrush =
						new LinearGradientBrush( rect,
 
						// Silver Theme
						//Color.FromArgb( 255, 255, 255 ),
						//Color.FromArgb( 198, 187, 215 ),

						System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF),
						Color.DarkCyan,
						//System.Drawing.Color.FromArgb(165,207,255),
						LinearGradientMode.ForwardDiagonal) )
			{
				float[] relativeIntensities = {0.0f, 0.008f, 1.0f};
				float[] relativePositions   = {0.0f, 0.32f, 1.0f};
				//float[] relativeIntensities = {0.0f, 0.008f, 1.0f};
				//float[] relativePositions   = {0.0f, 0.32f, 1.0f};

				Blend blend = new Blend();
				blend.Factors = relativeIntensities;
				blend.Positions = relativePositions;
				lgBrush.Blend = blend;

				e.Graphics.FillRectangle( lgBrush, rect );
			}

			float x, y;
			using ( SolidBrush drawBrush = new SolidBrush(Color.Black) )
			{
				using ( Font font = new System.Drawing.Font( "Tahoma", 8.5f ) )
				{
					x = ticker.Left + 25;
					y = ticker.Top + (ticker.Height/2);
					e.Graphics.DrawString( ticker.Text, font, drawBrush, x, y );

					x = info.Left + 15;
					e.Graphics.DrawString( info.Text, font, drawBrush, x, y );
				}
			}
			//e.Graphics.DrawImage( SessionExp.Image, 5, 3, SessionExp.Image.Size.Width, SessionExp.Image.Size.Height );
		}

		/// <summary>
		/// This exits from the CCF desktop UI.
		/// </summary>
		/// <param name="sender">Sender object.</param>
		/// <param name="e">Event arguments.</param>
		private void exitCCF_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// This method represents a Web service operation for receiving a Service message that
		/// includes channel information.  A string is used here so that we pass an unknown object.
		/// </summary>
		/// <param name="channelInformation">The string containing the channel information.</param>
		/// <returns>A string with the response information.</returns>
		public override string ReceiverHandler(string channelInformation)
		{
			MultichannelMessageInformation providerResponseCi = null;
			string response;
			string action = string.Empty;
			try
			{
				if (GeneralFunctions.CanDeserialize<ChannelInformation>(channelInformation))
				{
					ci = (ChannelInformation)GeneralFunctions.Deserialize<ChannelInformation>(channelInformation);
					action = ci.Action;
					switch (action.ToUpper())
					{
						case "CALL":
						// Map to customer object
						ci.Comment = "ACK";
						customer = new CustomerProviderCustomerRecord();
						// Map customer information from ChannelInformation (SI section)
						customer.City = ci.City;
						customer.Country = ci.Country;
						customer.CustomerID = ci.CustomerID;
						customer.FirstName = ci.FirstName;
						customer.LastName = ci.LastName;
						customer.PhoneHome = ci.PhoneHome;
						customer.PhoneMobile = ci.PhoneMobile;
						customer.PhoneWork = ci.PhoneWork;
						customer.State = ci.State;
						customer.Street = ci.Street;
						customer.ZipCode = ci.ZipCode;

						// Puts it back on form thread...
						if (this.InvokeRequired)
						{
							this.BeginInvoke(doLookUpRoutine);
						}
						else
						{
							this.doLookupCommand();
						}
						break;

						case "PING":
							// Just responding that the desktop is running and OK, no
							// need to switch threads
							ci.dateTime = DateTime.Now;
							ci.Comment = "ACK";
							break;
						default:
							Logging.Warn(Application.ProductName, "Unhandled action in response. action :" + action);
							break;
					}
				}
				// If response is coming from some multichannel provider/adapter, locate the application id in response
				// and pass the channel information to the application
				else if (GeneralFunctions.CanDeserialize<MultichannelMessageInformation>(channelInformation))
				{
					providerResponseCi = (MultichannelMessageInformation)GeneralFunctions.Deserialize<MultichannelMessageInformation>(channelInformation);

					string applicationID = providerResponseCi.AgentApplicationID;
					if (applicationID == null)
					{
						throw new Exception("Application ID not present in the response from multichannel adapter");
					}
					action = providerResponseCi.Action;
					// if its an incoming call to agent, throw a lookup dialogue with customer information
					if (action.Equals(Constants.Actions.NewIncomingCall))
					{
						// Throw lookup only when it is an external call
						string message = providerResponseCi.Message;
						string callType = GeneralFunctions.GetXmlNodeValue(message, Constants.NodeNames.CallType);
						if ((callType != null) && (callType.Equals(Constants.NodeNames.CallTypeExternal)))
						{
							ShowLookupDialogue(providerResponseCi);
						}

					}
					int appID = Convert.ToInt32(applicationID);
					IHostedApplication multichannelApplication = this.GetHostedApp(appID);

                    //MSB - Deal with forced logout event from the Multichannel server. 
                    //  The applcation ID may not have come back with the event.
                    //  The Action Type for this is : Microsoft.Ccf.MultiChannelAdapters.Common.Constants.Actions.MicrosMultiChannelServer_Force_Logout
                    if (multichannelApplication != null)
                    {
                        setFocusForApplicationID(appID);
                        multichannelApplication.DoAction("ProcessResponse", GeneralFunctions.Serialize<MultichannelMessageInformation>(providerResponseCi));
                    }
                    else
                    {
                        Logging.Error(Application.ProductName, string.Format("Multichannel event {0} raised for a hosted application that cannot be found. The ID of the hosted application requested is {1}", providerResponseCi.Action, providerResponseCi.AgentApplicationID));
                    }
				}
				else
				{
					throw new Exception("Object can not be deserialized.");
				}
			}
			catch (Exception e)
			{
				Logging.Error(Application.ProductName, e.Message);
			}
			finally
			{
				if (ci != null)
				{
					response = GeneralFunctions.Serialize<ChannelInformation>(ci);
				}
				else if (providerResponseCi != null)
				{
					response = GeneralFunctions.Serialize<MultichannelMessageInformation>(providerResponseCi);
				}
				else
				{
					// TODO: Add error information to ci object
					ChannelInformation ciError = new ChannelInformation();
					response = GeneralFunctions.Serialize<ChannelInformation>(ciError);
				}
			}
			return response;
		}
		// Extracts the customer information from response and shows a lookup dialogue 
		private void ShowLookupDialogue(MultichannelMessageInformation providerResponseCi)
		{
			string message = providerResponseCi.Message;
			string ani = GeneralFunctions.GetXmlNodeValue(message, Constants.NodeNames.ANI);
			if ((ani == null) || (ani.Equals(string.Empty)))
			{
				customer = new CustomerProviderCustomerRecord();
			}
			else
			{
				//using the customer web service to get customer data
				CustomerProviderCustomerRecord[] customers = customerLookup.GetCustomersByANI(ani, 1);
				if ((customers != null) && (customers.Length > 0))
				{
					customer = customers[0];
				}
				else
				{
					customer = new CustomerProviderCustomerRecord();
				}
			}

			// Puts it back on form thread...
			if (this.InvokeRequired)
			{
				this.BeginInvoke(doLookUpRoutine);
			}
			else
			{
				this.doLookupCommand();
			}
		}
	}

	internal class OwnerDrawLabel : Label
	{
		public OwnerDrawLabel()
		{
			// Hide since we really draw the text on the form
			Visible = false;
			//BackColor = Color.Transparent;
			//this.SetStyle( ControlStyles.Opaque|ControlStyles.UserPaint, true );
		}

		public new string Text
		{
			get { return base.Text; }
			set
			{
				base.Text = value;
				if ( Parent != null )
					Parent.Refresh();
			}
		}
	}
}