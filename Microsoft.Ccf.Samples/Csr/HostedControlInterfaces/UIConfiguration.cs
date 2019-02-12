//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2007 Microsoft Corp.
//
// Classes to handle the UI components for CCF.
//
//===================================================================================

#region Usings

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Security.Permissions;
using System.ServiceModel;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Multichannel;
using Microsoft.Ccf.Samples.HostedControlInterfaces.RegisterClient;

#endregion

namespace Microsoft.Ccf.Csr.UIConfiguration
{
	#region Class UIConfiguration
	/// <summary>
	/// This class wraps a number of properties useful for all CCF agent desktop UIs.
	/// Doing this makes it easier for customizers to change the look of the CCF UI without
	/// having to modify too much code and makeing future code merges more difficult.
	/// </summary>
	/// <remarks>This class should be marked abstract, but due to a problem
	/// with teh Visual Studio designer you cannot design an abstract
	/// class or a class that inherits from it.  Do not use this
	/// class directly, as it is designed to be abstract.</remarks>
	public class UIConfiguration : Form, IReceiverHandler
	{
		#region Variables
		// Privates
		// Collection of the hosted applications tied to the UI
		private CCFAppsUI appsUI;
		private bool isSessionsUsed = true;
		private bool isWorkflowUsed = true;
		private bool useSaveSession = false;
		private Image image;
		// IP information for the desktop
		private IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
		// Used to unique ID the desktop
		private Guid clientID = Guid.NewGuid();
		// The agent's id, used for getting and setting presence state
		private int agentID;
		// Read configuration values
		private static ConfigurationValueReader configurationReader = null;
		// The settings object to read from
		private static ApplicationSettingsBase settings = null;
		// Service host for WCF services.
		private ServiceHost serviceHost = null;

		private RegisterClientClient register = new RegisterClientClient();
		// Protected

		/// <summary>
		/// Gets or sets teh Service host for WCF services.
		/// </summary>
		[
		Category("CCF"),
		Description("Gets or sets teh Service host for WCF services."),
		Browsable(false),
		]
		protected ServiceHost ServiceHost
		{
			[DebuggerStepThrough()]
			get { return this.serviceHost; }
			[DebuggerStepThrough()]
			set { this.serviceHost = value; }
		}

		/// <summary>
		/// Gets and sets the Agent ID.
		/// </summary>
		/// <remarks>Obsolete.  Use AgentID.</remarks>
		[
		Category("CCF"),
		Description("Agent ID."),
		Browsable(false),
		Obsolete("This property has been deprecated.  Use AgentID.")
		]
		public int MyID
		{
			[DebuggerStepThrough()]
			get { return this.agentID; }
			[DebuggerStepThrough()]
			set { this.agentID = value; }
		}

		/// <summary>
		/// Gets and sets the Agent ID.
		/// </summary>
		[
		Category("CCF"),
		Description("Agent ID."),
		Browsable(false)
		]
		public int AgentID
		{
			[DebuggerStepThrough()]
			get { return this.agentID; }
			[DebuggerStepThrough()]
			set { this.agentID = value; }
		}

		/// <summary>
		/// Gets and sets the Client ID.
		/// </summary>
		[
		Category("CCF"),
		Description("Unique ID for the desktop."),
		Browsable(false)
		]
		protected Guid ClientID
		{
			[DebuggerStepThrough()]
			get { return this.clientID; }
			[DebuggerStepThrough()]
			set { this.clientID = value; }
		}

		/// <summary>
		/// Gets and sets the Host information for the desktop.
		/// </summary>
		[
		Category("CCF"),
		Description("Host Information for the desktop."),
		Browsable(false)
		]
		protected IPHostEntry HostInfo
		{
			[DebuggerStepThrough()]
			get { return this.hostInfo; }
			[DebuggerStepThrough()]
			set { this.hostInfo = value; }
		}

		/// <summary>
		/// Gets and sets the CCFAppsUI instance.
		/// </summary>
		[
		Category("CCF"),
		Description("Collection of the hosted applications tied to the UI"),
		Browsable(true),
		]
		protected CCFAppsUI AppsUI
		{
			[DebuggerStepThrough()]
			get { return this.appsUI; }
			[DebuggerStepThrough()]
			set { this.appsUI = value; }
		}

		/// <summary>
		/// True if the Session Explorer UI should be shown, false if it is hidden and
		/// other screen elements will reuse that space.
		/// </summary>
		[
		Category("CCF"),
		Description("True if SessionExplorer is used, False to disable multiple sessions"),
		Browsable(true),
		]
		protected bool Use_SessionExplorer
		{
			[DebuggerStepThrough()]
			get { return isSessionsUsed; }
			[DebuggerStepThrough()]
			set { isSessionsUsed = value; }
		}

		/// <summary>
		/// True if the Workflow UI should be shown, false if it is hidden and
		/// other screen elements will reuse that space.
		/// </summary>
		[
		Category("CCF"),
		Description("True if Workflow is used, False to disable Workflow and not show it"),
		Browsable(true),
		]
		protected bool Use_Workflow
		{
			[DebuggerStepThrough()]
			get { return isWorkflowUsed; }
			[DebuggerStepThrough()]
			set { isWorkflowUsed = value; }
		}

		/// <summary>
		/// True if the the saving of session state to the database should be used.
		/// One reason not to use it is if your legacy apps can not use it.  In this case,
		/// turning this feature off reduces the writes to the database when switching apps
		/// and sessions.
		/// </summary>
		[
		Category("CCF"),
		Description("True if using session auto-saves, False to disable it"),
		Browsable(true),
		]
		protected bool Use_SaveSession
		{
			[DebuggerStepThrough()]
			get { return useSaveSession; }
			[DebuggerStepThrough()]
			set { useSaveSession = value; }
		}

		/// <summary>
		/// Sets/Gets the background image used behind the hosted applications
		/// when they are starting.  This makes the CCF desktop look better
		/// while starting.
		/// </summary>
		[
		Category("CCF"),
		Description("Set to null for no Splash screen, or an image to show on a Splash screen"),
		Browsable(true),
		]
		public Image SplashScreenImage
		{
			[DebuggerStepThrough()]
			get { return image; }
			[DebuggerStepThrough()]
			set { image = value; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// UIConfiguration constructor.
		/// </summary>
		public UIConfiguration()
		{
			InitializeComponent();
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		#region Application Bar Support

		#region Variables

		// Private Statics
		private static bool useApplicationBar = false;

		// Protected
		/// <summary>
		/// Maintains the Appliation bar reference for Desktop toolbar.
		/// </summary>
		protected ApplicationBar appBar = null;

		/// <summary>
		/// For use with application bar autohide.  Allows a delay before showing or hiding
		/// the application bar.
		/// </summary>
		protected Timer sliderTimer = null;

		#endregion

		#region Properties
		/// <summary>
		/// Set to true at design time to cause the application to start as
		/// an application bar that can dock to the sides.
		/// </summary>
		[
		Category("CCF"),
		Description("True if the UI is an application toolbar, False if not"),
		Browsable(true),
		]
		public static bool Use_ApplicationBar
		{
			get { return useApplicationBar; }
			set { useApplicationBar = value; }
		}

		/// <summary>
		/// Reference to class that handles the managed application bar code
		/// </summary>
		[
		Category("CCF"),
		Description("Reference to class that handles the managed application bar code."),
		Browsable(true),
		]
		public ApplicationBar AppBar
		{
			get { return appBar; }
		}
		#endregion

		/// <summary>
		/// Used to initialize the application bar.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLoad(EventArgs e)
		{
			if (useApplicationBar)
			{
				this.FormBorderStyle = FormBorderStyle.None;

				appBar = new ApplicationBar(this);

				// Pass true to RegisterBar is there is a caption bar on the form
				//   because you do want resizable borders.
				// Pass false if the FormBorderStyle is none or if you want a caption
				//   to be visible.
				appBar.RegisterBar(false);

				appBar.AutoHide = true;

				// Check if autohide is on in case its moved to a configuration file.
				if (appBar.AutoHide)
				{
					// Set a timer to delay hiding/showing the application bar
					sliderTimer = new Timer();
					sliderTimer.Interval = 2 * 1000;  // # seconds cause we feel like it
					sliderTimer.Tick += new EventHandler(sliderTimer_Tick);

					WireMouseEventsForAutoHide(this);
				}
			}

			base.OnLoad(e);
		}

		/// <summary>
		/// This sets up every control in the application bar to handle the
		/// mouse enter/leave events in the same way.
		/// </summary>
		/// <param name="parent"></param>
		/// <remarks>
		/// This is only needed prior to Visual Studio 2005 since that version adds
		/// this capability to Form.MouseEnter/MouseLeave.
		/// 
		/// Unfortunately, the Framework as of 1.1 does not pass these events up to
		/// parent controls or forms.
		/// </remarks>
		protected void WireMouseEventsForAutoHide(Control parent)
		{
			parent.MouseEnter += new EventHandler(UIConfiguration_EnterLeave);
			parent.MouseLeave += new EventHandler(UIConfiguration_EnterLeave);

			// recurse so all child controls will pass these events to the same handlers.
			foreach (Control c in parent.Controls)
			{
				WireMouseEventsForAutoHide(c);
			}
		}

		/// <summary>
		/// Handle both the mouse enter and leave events for all application
		/// bar controls.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void UIConfiguration_EnterLeave(object sender, EventArgs e)
		{
			Point pos = Cursor.Position;
			if (AppBar.ShowAll != this.Bounds.Contains(pos))
			{
				sliderTimer.Enabled = true;
			}
		}

		/// <summary>
		/// This is the delay for showing and hiding the application bar.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void sliderTimer_Tick(object sender, EventArgs e)
		{
			sliderTimer.Enabled = false;

			Point pos = Cursor.Position;
			AppBar.ShowAll = this.Bounds.Contains(pos);
		}

		/// <summary>
		/// Needed to remove the application bar registration from the Windows desktop
		/// space that the bar used can be used by other applications.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnClosed(EventArgs e)
		{
			if (appBar != null)
			{
				appBar.UnregisterBar();
			}
			base.OnClosed(e);
		}

		/// <summary>
		/// Used when using an application bar design to handle app bar UI
		/// change notifications.
		/// </summary>
		/// <param name="m"></param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			if (appBar != null)
			{
				switch (m.Msg)
				{
					case (int)Win32API.StandardMessage.WM_ACTIVATE:
						appBar.Activate();
						break;

					case (int)Win32API.StandardMessage.WM_WINDOWPOSCHANGED:
						appBar.WindowPosChanged();
						break;

				}

				// Handle messages to the application bar
				if (m.Msg == appBar.ApplicationBarCallBack)
				{
					switch (m.WParam.ToInt32())
					{
						case (int)ApplicationBar.ABNotify.ABN_POSCHANGED:
							appBar.ABSetPos();
							break;

						case (int)ApplicationBar.ABNotify.ABN_STATECHANGE:
							break;

						case (int)ApplicationBar.ABNotify.ABN_WINDOWARRANGE:
							// The appbar is ignoring Cascade and Tile commands
							// from windows by hiding when its asked to cascade or tile
							// and reappearing after the command.
							this.Visible = m.LParam.ToInt32() != 0 ? false : true;
							break;
					}
				}
			}

			base.WndProc(ref m);
		}

		/// <summary>
		/// Used for application bar designs to prevent the creation of borders
		/// and caption bar.
		/// </summary>
		protected override CreateParams CreateParams
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				CreateParams cp = base.CreateParams;

				// Only needed when in application bar mode
				if (useApplicationBar)
				{
					// WS_CAPTION
					cp.Style &= (~0x00C00000);
					// WS_BORDER
					cp.Style &= (~0x00800000);
					// WS_EX_TOOLWINDOW | WS_EX_TOPMOST
					cp.ExStyle = 0x00000080 | 0x00000008;
				}
				return cp;
			}
		}

		#endregion

		#region Private Methods

		private static string CheckPort(string port)
		{
			try
			{
				int.Parse(port);
			}
			catch (ArgumentNullException)
			{
				Logging.Error(System.Windows.Forms.Application.ProductName, "Port may not be null.");
			}
			catch (FormatException)
			{
				Logging.Error(System.Windows.Forms.Application.ProductName, "Port must be a numeric string.");
			}
			catch (OverflowException)
			{
				Logging.Error(System.Windows.Forms.Application.ProductName, "Port may not be null.");
			}
			return port;
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Gets a hosted application by its name from appsUI.applications
		/// </summary>
		/// <param name="appName">The application name to to get.</param>
		/// <returns>Returns IHostedApplication interface for the appliation found, null otherwise</returns>
		protected IHostedApplication GetHostedApp(string appName)
		{
			foreach (IHostedApplication app in appsUI.Applications)
			{
				if (app.ApplicationName == appName)
				{
					return app;
				}
			}
			return null;
		}

		/// <summary>
		/// Gets a hosted application by its ID from appsUI.applications
		/// </summary>
		/// <param name="appID">The application ID to to get.</param>
		/// <returns>Returns IHostedApplication interface for the appliation found, null otherwise</returns>
		protected IHostedApplication GetHostedApp(int appID)
		{
			foreach (IHostedApplication app in appsUI.Applications)
			{
				if (app.ApplicationID == appID)
				{
					return app;
				}
			}
			return null;
		}

		/// <summary>
		/// Register the client with the CCF Server.
		/// </summary>
		/// <param name="port">The port for the IP address the client is using.</param>
		/// <remarks>
		/// This code works generically but may need to be updated for your environment (IP address structure).
		/// </remarks>
		protected void RegisterClient(string port)
		{
			port = CheckPort(port);
			// Start to gather information (IP, etc...) about the client
			IPAddresses ipAddresses = new IPAddresses();
			IPAddress ipAddress = ipAddresses.IPAddress4[0];
			// Register the client with the CCF Server

			string Url = ConfigurationReader.ReadAppSettings("Microsoft_Ccf_Samples_HostedControlInterfaces_RegisterClient_RegisterClient");
			EndpointAddress registerClientAddress = new EndpointAddress(Url);
			register.Endpoint.Address = registerClientAddress;
			register.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();
			register.Register(this.ClientID, this.AgentID, ipAddress.ToString(), port);
		}

		/// <summary>
		/// Unregister the client with the CCF Server.
		/// </summary>
		protected void UnregisterClient()
		{
			// Un-register client for CCF Server
			register.Unregister(this.ClientID);
		}

		/// <summary>
		/// Register the Listener for the multi channel calls to the desktop.
		/// </summary>
		/// <param name="port">The port for the IP address the client is using.</param>
		/// <remarks>
		/// This code works generically but may need to be updated for your environment (IP address structure).
		/// </remarks>
		protected void RegisterListener(string port)
		{
			port = CheckPort(port);
			IPAddresses ipAddresses = new IPAddresses();
			IPAddress ipAddress = ipAddresses.IPAddress4[0];
			Uri httpBaseAddress = new Uri(string.Format("http://{0}:{1}/", ipAddress, port), UriKind.Absolute);
			MultichannelSubsystem mcssclient = new MultichannelSubsystem(this);
			this.ServiceHost = new ServiceHost(mcssclient, httpBaseAddress);
			this.ServiceHost.Open();
		}

		/// <summary>
		/// Unregister the Listener for the multi channel calls to the desktop.
		/// </summary>
		protected void UnregisterListener()
		{
			this.ServiceHost.Close();
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the Settings object
		/// </summary>
		public static ApplicationSettingsBase Settings
		{
			set
			{
				settings = value;
			}
		}

		/// <summary>
		/// Gets the ConfigurationValueReader object.
		/// </summary>
		public static ConfigurationValueReader ConfigurationReader
		{
			get
			{
				if (configurationReader == null)
				{
					configurationReader = GeneralFunctions.ConfigurationReader(settings);
				}
				return configurationReader;
			}
		}

		/// <summary>
		/// This method represents a Web service operation for receiving a Service message that
		/// includes channel information.  A string is used here so that we pass an unknown object.
		/// </summary>
		/// <param name="channelInformation">The string containing the channel information.</param>
		/// <returns>A string with the response information.</returns>
		/// <remarks>Treat as an abstract method.</remarks>
		public virtual string ReceiverHandler(string channelInformation)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
	#endregion

	#region Delegate
	/// <summary>
	/// Delegate used to indicate to the UI when the user has changed the hosted
	/// application they are using.
	/// </summary>
	public delegate void SelectedAppChangedHandler(object sender, EventArgs e);

	/// <summary>
	/// Delegate used to indicate to the UI when the user has close an application.
	/// </summary>
	/// <param name="app"></param>
	public delegate void CloseApplicationClickHandler(IHostedApplication app);

	#endregion

	#region Class CCFAppsUI

	/// <summary>
	/// CCFAppsUI separates the details of hosting applications and their UI's from the
	/// layout of the agent desktop form.
	/// 
	/// It keeps a collection of CCFPanels and exposes itself as a collection
	/// of IHostedApplication objects.
	/// </summary>
	public class CCFAppsUI : IEnumerable
	{
		#region Variables
		// Privates
		private static IHostedApplication appWithFocus = null;
		private static CCFPanel activePanel = null;
		private string name;
		private ArrayList applications = new ArrayList();

		// Protected
		/// <summary>
		/// This holds references to the floating forms
		/// </summary>
		protected ArrayList floatingForms = new ArrayList();
		/// <summary>
		/// List of active panels
		/// </summary>
		protected ArrayList panels = new ArrayList();

		// Events
		/// <summary>
		/// Event to handle application selection changes.
		/// </summary>
		public event SelectedAppChangedHandler SelectedAppChanged;

		/// <summary>
		/// Event to handle application close.
		/// </summary>
		public event CloseApplicationClickHandler CloseApplicationClick;

		/// <summary>
		/// Level of opaqueness to use when a panel is not active.
		/// </summary>
		/// <remarks>Used only for CCFPanels with the UseOpacity property set to true</remarks>
		protected const float _opaqueLevel = 0.8f;
		#endregion

		#region Properties
		/// <summary>
		/// Gets or Sets the application which has the focus
		/// </summary>
		public static IHostedApplication AppWithFocus
		{
			get
			{
				return appWithFocus;
			}
			set
			{
				// This is to support backwards compatability.  Previous versions apps could have been IHostedApplication2 or IHostedApplication
				if ((value as IHostedApplication3) != null)
				{
					// Only set focus to applications that are not part of the UI 
					if ((value as IHostedApplication3).IsListed)
					{
						appWithFocus = value;
					}
				}
				else
				{
					appWithFocus = value;
				}
			}
		}

		/// <summary>
		/// This contains a reference to the panel which hosts the application currently
		/// in use.  This panel may contain more than one application.
		/// </summary>
		public static CCFPanel ActivePanel
		{
			get { return activePanel; }
			set
			{
				if (activePanel != value)
				{
					if (activePanel != null && activePanel.UseOpacity)
					{
						Form form = activePanel.Parent as Form;
						if (form != null)
						{
							form.Opacity = _opaqueLevel;
						}
					}

					activePanel = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the panel name.
		/// </summary>
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// List of active applications across all panels.  This permits
		/// the CCFAppsUI class to act as an enumerator for IHostedApplications.
		/// </summary>
		public ArrayList Applications
		{
			get { return applications; }
			set { applications = value; }
		}
		#endregion

		#region Constructors

		#endregion

		#region Public Methods

		/// <summary>
		/// Add a CCFPanel (inherits from Panel) to the list of known panels
		/// which may be hosting applications.
		/// </summary>
		/// <param name="panel"></param>
		public void AddPanel(CCFPanel panel)
		{
			panels.Add(panel);
			panel.selectedAppChanged += SelectedAppChanged;
			panel.closeApplicationClick += CloseApplicationClick;
		}

		/// <summary>
		/// Gets the panel after the one which has the current focus in the CCF interface.
		/// If there is only one panel, it is always returned.
		/// </summary>
		/// <returns></returns>
		public CCFPanel NextPanel()
		{
			if (panels.Count == 0)
			{
				return null;   // no panels
			}
			// find panel after ActivePanel
			bool next = false;
			foreach (CCFPanel panel in panels)
			{
				if (next && panel.Controls.Count > 0)
				{
					ActivePanel = panel;
					return ActivePanel;
				}

				next = (panel == ActivePanel);
			}

			ActivePanel = panels[0] as CCFPanel;
			return ActivePanel;
		}

		/// <summary>
		/// Enable/Disable redrawing of the hosted app panels to make the UI
		/// look cleaner when redrawing many items.
		/// </summary>
		/// <param name="redraw"></param>
		public void SetRedraw(bool redraw)
		{
			foreach (CCFPanel panel in panels)
			{
				try
				{
					Win32API.SetRedraw(panel.Handle, redraw);
					if (redraw)
					{
						panel.Refresh();
					}
				}
				catch { }  // ignore errors
			}
		}

		/// <summary>
		/// This is what is used for a 'foreach ( IHostedApplication app in appsUI )'
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator()
		{
			return applications.GetEnumerator();
		}

		/// <summary>
		/// Allow CCFAppsUI objects to be indexed and return IHostedApplication objects.
		/// </summary>
		public IHostedApplication this[int ind]
		{
			get
			{
				if (ind < 0 || ind >= applications.Count)
				{
					throw new IndexOutOfRangeException();
				}

				return applications[ind] as IHostedApplication;
			}
		}

		/// <summary>
		/// Returns how many IHostedApplications are being used.
		/// </summary>
		public int Length
		{
			get { return applications.Count; }
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Returns a unique name for a new floating panel.
		/// The name is based upon the application or control which is hosted in it.
		/// </summary>
		/// <param name="app"></param>
		/// <returns>a name to use for a floating panel</returns>
		protected static string GetFloatingPanelName(object app)
		{
			IHostedApplication hostedApp = app as IHostedApplication;
			if (hostedApp != null)
			{
				return hostedApp.ApplicationName + "_" + hostedApp.ApplicationID + "_panel";
			}
			Control c = app as Control;
			if (c != null)
			{
				return c.Name + "_panel";
			}
			return null;
		}

		/// <summary>
		/// Used to block the manual closing of a floating hosted application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void form_ClosingNotAllowed(object sender, CancelEventArgs e)
		{
			//Desktop logoff is initiated by the User.
			if (FloatingWindowForm.LogOffRequested)
			{
				//Allows to the form to close and there by allows for logging off the desktop.
				e.Cancel = false;
				FloatingWindowForm.LogOffRequested = false;
			}
			else
			{
				//blocks the manual closing of the form.
				e.Cancel = true;
			}
		}

		/// <summary>
		/// Used to block the manual closing of a floating hosted application.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void form_ClosingAllowed(object sender, CancelEventArgs e)
		{
			//Desktop logoff is initiated by the User.
			if (FloatingWindowForm.LogOffRequested)
			{
				//Allows to the form to close and there by allows for logging off the desktop.
				e.Cancel = false;
				FloatingWindowForm.LogOffRequested = false;
			}
			else
			{
				e.Cancel = true;

				Form form = sender as Form;

				if (form != null)
				{
					if (this.CloseApplicationClick != null)
					{
						// The application is stored in the tag value
						IHostedApplication app = form.Tag as IHostedApplication;

						if( app != null)
						{
							CloseApplicationClick(app);
						}
					}
				}
			}
		}


		/// <summary>
		/// Handles a floating hosted application getting the focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void TopLevelWindow_GotFocus(object sender, EventArgs e)
		{
			try
			{
				IHostedApplication app = null;

				// Get the hosted application that is getting the focus
				if (sender is IHostedApplication)
					app = sender as IHostedApplication;
				else if (sender is ExternalControl)
					app = (sender as ExternalControl).HostedParent;
				else if (sender is Control)  // others (especially web apps) // 1.02 Patch 4
				{
					Control c = sender as Control;
					if (c.Parent != null)
					{
						// The parent of the control is the CcfDeckControl for floating hosted apps
						CcfDeckControl deckControl = c.Parent as CcfDeckControl;

						if (deckControl != null && deckControl.Count > 0)
						{
							app = deckControl.HostedApplications[0] as IHostedApplication;
						}
					}
				}

				// 1.02 Patch 4
				// Cases with some external apps and with web apps where
				// the app will not be known.
				if (app != null)
				{
					// remember which app is currently selected
					AppWithFocus = app;

					// TopLevelWindow -> CcfDeckControl -> CcfPanel
					ActivePanel = app.TopLevelWindow.Parent.Parent as CCFPanel;

					// Notify the UI code about this change
					if (this.SelectedAppChanged != null)
						SelectedAppChanged(sender, e);
				}
			}
			catch { }
		}

		/// <summary>
		/// Occurs when a form is activated, used for floating panels.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void form_Activated(object sender, EventArgs e)
		{
			try
			{
				Form form = sender as Form;

				if (form != null && form.Controls.Count > 0)
				{
					form.Opacity = 1.0;

					CCFPanel panel = form.Controls[0] as CCFPanel;
					if (panel != null && panel.DeckControl.Count > 0)
					{
						// There is only 1 application on the deckcontrol
						IHostedApplication app = panel.DeckControl.HostedApplications[0] as IHostedApplication;

						// Cases with some external apps and with web apps where
						// the app will not be known.
						if (app != null)
						{
							// remember which app is currently selected
							AppWithFocus = app;

							// TopLevelWindow -> CcfDeckControl -> CcfPanel
							ActivePanel = app.TopLevelWindow.Parent.Parent as CCFPanel;

							// Notify the UI code about this change
							if (this.SelectedAppChanged != null)
							{
								SelectedAppChanged(app, e);
							}
						}
					}
				}
			}
			catch { }
		}

		/// <summary>
		/// Used if the designer wants to cause forms to become a bit transparent when
		/// they lose focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private static void form_LostFocus(object sender, EventArgs e)
		{
			Form form = sender as Form;
			if (form != null && form.Controls.Count > 0)
			{
				CCFPanel panel = form.Controls[0] as CCFPanel;

				if (panel != null && panel.UseOpacity)
					form.Opacity = _opaqueLevel;
			}
		}

		#endregion

		#region Virtual Public Methods

		/// <summary>
		/// Creates a new floating CCFPanel for hosting controls or applications.
		/// </summary>
		/// <param name="captionUsed"></param>
		/// <param name="toolWindow">True if this should use a tool caption</param>
		/// <param name="panelName">Panel name.</param>
		/// <param name="app">Application to connect to panel.</param>
		/// <param name="icon">Icon for the panel.  If the app implements IHostedApplication2 or above the icon is pulled from the application.</param>
		/// <param name="initializationXml">Extra info that may define how the window is created</param>
		/// <returns>A control reference to the panel.</returns>
		/// <param name="closeButton"></param>
		virtual public Control CreateFloatingCCFPanel(bool captionUsed, bool toolWindow, string panelName, object app,
			Icon icon, string initializationXml, bool closeButton)
		{
			bool reused = false;
			CCFPanel foundPanel = null;
			IHostedApplication hostedApp = app as IHostedApplication;

			// first see if there is already a floating window for this app
			foreach (CCFPanel panel in panels)
			{
				if (panel.Name == panelName)
				{
					// See if this is already set up (global apps do this)
					if (panel.Parent != null)
					{
						return panel;
					}
					foundPanel = panel;
					reused = true;
					break;
				}
			}

			// if this is a new floating app, create a form and panel for it
			if (foundPanel == null)
			{
				foundPanel = new CCFPanel();
				foundPanel.Floating = true;
				foundPanel.Name = panelName;
				AddPanel(foundPanel);
			}

			// This permits developer's to add their own behavior to application display
			// without having to change how existing applications behave.  In the past,
			// developers have modified this code to make apps have fixed borders,
			// not control box, etc.  By allowing them to put tags in the
			// initialization XML in the Application table, they can make changes
			// and keep the code more flexible.
			if (initializationXml != null && initializationXml != String.Empty)
			{
				XmlDocument initializationDoc = new XmlDocument();
				initializationDoc.LoadXml(initializationXml);
			}

			// This is a floating window
			FloatingWindowForm form = new FloatingWindowForm();

			form.SuspendLayout();
			if (hostedApp != null)
			{
				form.Text = hostedApp.ApplicationName;
				form.ClientSize = hostedApp.OptimumSize;

				// Get the hosted app when we have the form
				form.Tag = hostedApp;
			}
			else
			{
				form.Text = panelName;
				form.ClientSize = (app as Control).Size;

				// TODO: set app to form tag.
			}

			form.Controls.Add(foundPanel);
			foundPanel.Dock = DockStyle.Fill;
			foundPanel.DockPadding.All = 2;

			form.Name = foundPanel.Name;

			// Begin Dynamic

			if (closeButton)
			{
				// Permit the app's icon to be shown but don't let the window be closed
				//form.ControlBox = true;
				Win32API.EnableApplicationClose(form, true);

				form.Closing += new CancelEventHandler(form_ClosingAllowed);
			}
			else
			{
				// Permit the app's icon to be shown but don't let the window be closed
				//form.ControlBox = true;
				Win32API.EnableApplicationClose(form, false);

				// Prevent floating panel from being manually closed with Alt-F4
				// if an external application.
				form.Closing += new CancelEventHandler(form_ClosingNotAllowed);
			}

			// End Dynamic

			if (toolWindow)
			{
				form.FormBorderStyle = FormBorderStyle.FixedToolWindow;
			}
			else
			{
				form.FormBorderStyle = FormBorderStyle.Sizable;
			}

			form.ControlBox = true;
			form.MaximizeBox = captionUsed;
			form.MinimizeBox = captionUsed;

			// for handling when the form or application gets the focus
			form.Activated += new EventHandler(form_Activated);
			form.LostFocus += new EventHandler(form_LostFocus);
			form.Leave += new EventHandler(form_LostFocus);

			floatingForms.Add(form);

			// if this panel has been shown before
			if (reused)
			{
				// If used again, we place the new form where 
				// the old one was.
				if (foundPanel.FloatingLocation.X >= 0 &&
					foundPanel.FloatingLocation.Y >= 0)
				{
					form.Location = foundPanel.FloatingLocation;
					form.Size = foundPanel.FloatingSize;
				}
			}

			// Set icon to the Hosted Application icon, if it implements IHostedApplication2 or abov
			if (app is IHostedApplication2)
			{
				form.Icon = (app as IHostedApplication2).Icon;
			}
			else
			{
				form.Icon = icon;
			}

			form.ResumeLayout();
			if (captionUsed)
			{
				form.Show();
			}

			if (hostedApp != null)
			{
				hostedApp.TopLevelWindow.Dock = DockStyle.Fill;
			}
			else
			{
				(app as Control).Dock = DockStyle.Fill;
			}
			applications.Add(app);

			//In case of floating hosted applications also,
			//set the AppWithFocus and raise the SelectedAppChanged event.
			if (AppWithFocus == null)
			{
				AppWithFocus = hostedApp;

				// Notify the UI code about this change
				if (SelectedAppChanged != null)
				{
					SelectedAppChanged(null, null);
				}
			}

			// For when the application gets the focus
			if (hostedApp != null)
			{
				hostedApp.TopLevelWindow.GotFocus += new EventHandler(TopLevelWindow_GotFocus);
			}
			// Close button always false because if floating then will use the button on the 
			// floating form
			return foundPanel.Add(app, initializationXml, false, false);
		}

		/// <summary>
		/// This adds a hosted application or any Windows Forms control to a CCF
		/// panel.  The panelName is used to identify which panel if there are more
		/// than one.
		/// </summary>
		/// <param name="panelName"></param>
		/// <param name="app"></param>
		/// <param name="initializationXml">Extra XML that may define how to initialize this window</param>
		/// <param name="closeButton">For dynamic application, can have option to provide a close
		/// button or not</param>
		/// <returns></returns>
		virtual public Control AddApplication(string panelName, object app, string initializationXml, bool closeButton)
		{
			IHostedApplication hostedApp = app as IHostedApplication;

			if (panelName != null)
			{
				panelName = panelName.ToLower();
			}

			// If floating, then create a panel on the fly that is not integrated
			//   into the main UI but is still controlled by CCF.
			// If the panel name is floatingTool, then give the window a toolbar
			// style caption.
			if (panelName == "floating" || panelName == "floatingtool")
			{
				if (hostedApp == null)
				{
					return null; // can only make hosted apps floating not controls
				}

				return CreateFloatingCCFPanel(true,
					panelName == "floatingtool",
					GetFloatingPanelName(app), app, null,
					initializationXml, closeButton);
			}

			// SDI type UI rather than floating
			if (panels.Count > 0)
			{
				bool found = false;

				// find the panel being referenced by name, if
				// not found use, the first panel in the list.
				CCFPanel foundPanel = panels[0] as CCFPanel;
				foreach (CCFPanel panel in panels)
				{
					if (panel.Name.ToLower() == panelName || panelName == null)
					{
						foundPanel = panel;
						found = true;
						break;
					}
				}

				// If running as a ApplicationBar, then make apps that do not
				// have a panel be floating ones.
				if (!found && UIConfiguration.Use_ApplicationBar)
				{
					return AddApplication("floating", app, initializationXml, closeButton);
				}

				// See if the app is already on this panel - can happen with
				// global applications.
				if (hostedApp != null &&
					foundPanel.IsApplicationOnPanel(hostedApp.ApplicationID))
				{
					return foundPanel;
				}

				// We can add custom controls and hosted applications to the
				// display panels, but only hosted applications get enumerated over
				if (hostedApp != null)
				{
					applications.Add(app);

					if (AppWithFocus == null)
					{
						AppWithFocus = hostedApp;


						// Notify the UI code about this change
						if (SelectedAppChanged != null)
						{
							SelectedAppChanged(null, null);
						}

					}
				}

				if (foundPanel != null)
				{
					return foundPanel.Add(app, initializationXml, false, closeButton);
				}
			}

			return null;
		}

		/// <summary>
		/// This selects the UI panel and control which is hosting the given application.
		/// </summary>
		/// <param name="id">The id of the app to select</param>
		/// <returns></returns>
		virtual public bool SelectApplication(int id)
		{
			foreach (CCFPanel panel in panels)
			{
				// if this panel only has one application
				IHostedApplication app;
				if (panel.TabControl.Count == 0)
				{
					if (panel.DeckControl.Count > 0) //panel.Controls.Count > 0 )
					{
						// There is only 1 app on the deckcontrol
						app = panel.DeckControl.HostedApplications[0] as IHostedApplication;
						if (app != null && app.ApplicationID == id)
						{
							// If app already had focus then no need to focus again
							// Also fixes bug of applications hanging for desktop toolbar because
							// its trying to focus on the same application twice
							if (app != AppWithFocus)
							{
								// remember which app is currently selected
								AppWithFocus = app;
								ActivePanel = panel;

								if (panel.Floating)
								{
									panel.Visible = true;  // in case it was hidden

									if (app.TopLevelWindow != null)
									{
										app.TopLevelWindow.Visible = true;  // in case it was hidden
										app.TopLevelWindow.Focus();
									}

									if (panel.Parent != null)
									{
										// if panel is minimized, restore it
										Form form = panel.Parent as Form;
										if (form != null && form.WindowState == FormWindowState.Minimized)
											form.WindowState = FormWindowState.Normal;

										// For floating windows we need to force the setfocus
										// to happen after this code runs.  This routine is called
										// by some handlers for other controls which go on to
										// take the focus themselves.  The Win32API call below
										// works via PostMessage and thus happens after
										// the completion of the event which called this.
										Win32API.SetFocus(panel.Parent.Handle);
									}
								}
								if (this.SelectedAppChanged != null)
								{
									SelectedAppChanged(this, new EventArgs());
								}
							}
							return true;
						}
					}

					continue;
				}

				// search the tab pages in this panel for the application
				// foreach ( TabPage tabPage in panel.tabControl.Controls )
				foreach (TabPage tabPage in panel.TabControl.CcfTabPages)
				{
					app = tabPage.Tag as IHostedApplication;
					if (app != null && app.ApplicationID == id)
					{
						// remember which app is currently selected
						AppWithFocus = app;
						ActivePanel = panel;

						if (panel.TabControl.SelectedTab != tabPage)
						{
							panel.TabControl.SelectedTab = tabPage;
							if (tabPage.CanFocus)
							{
								tabPage.Focus();
							}
						}
						// There is a chance that with floating windows being
						// used alongside tab windows, that the
						// tab is already selected but the rest of the UI
						// needs to be told that the tab's app has the focus
						else if (this.SelectedAppChanged != null)
						{
							SelectedAppChanged(this, new EventArgs());
						}

						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Removes the passed in app from the UI, regardless of which panel it is in.
		/// </summary>
		/// <param name="app"></param>
		/// <returns>true if the app was found and removed</returns>
		virtual public bool RemoveApplication(object app)
		{
			if (app != null)
			{
				if (AppWithFocus == app)
				{
					AppWithFocus = null;
				}
				foreach (CCFPanel panel in panels)
				{
					if (panel.Remove(app))
					{
						// 1.02 Patch 4
						// Remove from list of applications
						applications.Remove(app);

						// if this is a floating, close the form it is hosted on
						if (panel.Floating)
						{
							//panels.Remove( panel );
							floatingForms.Remove(panel.Parent);

							Form form = panel.Parent as Form;
							if (form != null)
							{
								// Ensure we have the window's 'normal' size
								if (form.WindowState != FormWindowState.Normal)
									form.WindowState = FormWindowState.Normal;

								// Save where the form was in case it is re-created.
								// If used again, we place the new one where 
								// the old one was.
								panel.FloatingLocation = form.Location;
								panel.FloatingSize = form.Size;

								// Close the form, need to unhook the event handler
								// which blocks the form from being closed by Alt-F4.
								form.Controls.Remove(panel);
								form.Closing -= new CancelEventHandler(form_ClosingNotAllowed);
								form.Closing -= new CancelEventHandler(form_ClosingAllowed);
								form.Close();
							}
						}

						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the selected application regardless of how its hosted.
		/// </summary>
		/// <returns></returns>
		virtual public IHostedApplication GetSelectedApplication()
		{
			return AppWithFocus;
		}

		#endregion

	}

	#endregion
}
