//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// The UI element which hosts one or more hosted applications.
// 
//===============================================================================
#region Using

using System;
using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Xml;

#endregion

namespace Microsoft.Ccf.Csr.UIConfiguration
{
	/// <summary>
	/// The UI element which hosts one or more hosted applications.
	/// This can also host standard and custom windows controls.
	/// </summary>
	public class CCFPanel : Panel
	{
		#region Variables
		// Internals
		// Used when a tab page is selected to pass the event back to the main UI code
		internal event SelectedAppChangedHandler selectedAppChanged;

		/// <summary>
		/// Used when close button is clicked on the application to pass the event
		/// back to the main UI code.
		/// </summary>
		internal event CloseApplicationClickHandler closeApplicationClick;

		// Privates
		private TabAppearance appearance = TabAppearance.Normal;
		private TabAlignment alignment = TabAlignment.Top;
		private bool floating = false;
		private Point floatingLocation;
		private Size floatingSize;

		/// <summary>
		/// To show hosted application in a tab form.
		/// </summary>
		private CcfTabControl tabControl = null;
		/// <summary>
		/// To show hosted application without a tab.
		/// Used when there is only one application on the panel.
		/// </summary>
		private CcfDeckControl deckControl = null;

		// Protected
		/// <summary>
		/// If true, the panel is made slightly transparent when it does not
		/// have the focus.  Sometimes useful when you have lots of floating panels.
		/// </summary>
		protected internal bool UseOpacity = false;
		#endregion

		#region Properties
		/// <summary>
		/// Determines how tab pages appear in the CCF desktop, as normal tabs,
		/// buttons, or flatbuttons.  Some styles are only possible when the
		/// tab alignment is set to top.
		/// </summary>
		[
		Category("CCF"),
		Description("How a tab page will appear if needed"),
		Browsable(true),
		]
		public TabAppearance Appearance
		{
			get { return appearance; }
			set { appearance = value; }
		}

		/// <summary>
		/// Determines where the tabs appear if there are tabs.
		/// </summary>
		[
		Category("CCF"),
		Description("How a tab control will be positioned if one is used"),
		Browsable(true),
		]
		public TabAlignment Alignment
		{
			get { return alignment; }
			set { alignment = value; }
		}

		/// <summary>
		/// Used to determine if this pane is floating or tied into the agent
		/// desktop UI.
		/// </summary>
		[Browsable(false)]
		public bool Floating
		{
			get { return floating; }
			set { floating = value; }
		}

		/// <summary>
		/// Used to save the last location for this panel in case its used again
		///   When reused, a floating panel gets a new form and this form should be
		///   placed in the same location as the previous one as the user expects it
		///   to be.
		/// This only is used for floating panes
		/// </summary>
		[Browsable(false)]
		public Point FloatingLocation
		{
			get { return floatingLocation; }
			set { floatingLocation = value; }
		}

		/// <summary>
		/// Used to save the last size for this panel in case its used again
		///   When reused, a floating panel gets a new form and this form should be
		///   made the same size as the previous one as the user expects it
		///   to be.
		/// This only is used for floating panes
		/// </summary>
		[Browsable(false)]
		public Size FloatingSize
		{
			get { return floatingSize; }
			set { floatingSize = value; }
		}

		/// <summary>
		/// Lets other class view and modify the tab pages in the 
		/// CcfTabControl under this panel.
		/// </summary>
		public CcfTabControl TabControl
		{
			get { return tabControl; }
		}

		/// <summary>
		/// Let other class view and modify the the hosted app
		/// in the CcfDeckControl under this panel.
		/// </summary>
		public CcfDeckControl DeckControl
		{
			get { return deckControl; }
		}

		#endregion

		#region Public Methods

		/// <summary>
		/// CCFPanel class construstor.
		/// </summary>
		public CCFPanel()
		{
			// don't let this be null
			Name = String.Empty;

			InitializeControls();
		}

		/// <summary>
		/// Checks if the passed id is an application on this CCFPanel.
		/// </summary>
		/// <param name="id">ID the application</param>
		/// <returns>true if found on the panel</returns>
		public virtual bool IsApplicationOnPanel(int id)
		{
			IHostedApplication app;

			// if this panel only has one application
			if (tabControl.Count == 0)
			{
				if (deckControl.Count > 0)
				{
					// There should be only 1 app on the deckControl
					app = deckControl.HostedApplications[0] as IHostedApplication;
					if (app != null && app.ApplicationID == id)
					{
						return true;
					}
				}
			}
			else
			{
				foreach (object hostedApp in tabControl.HostedApplications)
				{
					app = hostedApp as IHostedApplication;

					if (app != null && app.ApplicationID == id)
					{
						return true;
					}
				}

			}

			return false;
		}

		/// <summary>
		/// Adds a CCF hosted application or a user WinForms control to the
		/// CCFPanel.  If there are currenlty no app on this panel, then add
		/// to CcfDeckControl.  Else if there are more than one app on this
		/// panel, then add to CcfTabControl.
		/// </summary>
		/// <param name="child">The control or hosted app to add to the panel</param>
		/// <param name="closeButton">True if a close button is provided for closing dynamic
		/// hosted application, false otherwise</param>
		/// <returns>The tabpage from the CcfTabControl if one is used or the CcfDeckControl</returns>
		public virtual Control Add(object child, bool closeButton)
		{
			return Add(child, null, false, closeButton);
		}

		/// <summary>
		/// Adds a CCF hosted application or a user WinForms control to the
		/// CCFPanel.  If there are currenlty no app on this panel, then add
		/// to CcfDeckControl.  Else if there are more than one app on this
		/// panel, then add to CcfTabControl.
		/// </summary>
		/// <param name="child">The control or hosted app to add to the panel</param>
		/// <param name="initializationXml">An XML string for the application being added.
		/// This is used when determining how the app will appear in the panel, for
		/// instance, is there a toolbar.
		/// </param>
		/// <param name="useToolbar">True if a toolbar is used no mater what, false
		/// if the xml string should be parsed to see if one is used.
		/// </param>
		/// <param name="closeButton">True if a close button is provided for closing
		/// dynamic hosted application, false otherwise</param>
		/// <returns>The tabpage from the CcfTabControl if one is used or the CcfDeckControl</returns>
		virtual public Control Add(object child, string initializationXml, bool useToolbar, bool closeButton)
		{
			CcfPanelToolbar bar = null;

			if (initializationXml != null && initializationXml != String.Empty)
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(initializationXml);

				XmlNode node = doc.DocumentElement.SelectSingleNode("descendant::toolbar");
				if (node != null)
				{
					useToolbar = true;
				}
			}

			IHostedApplication hostapp = child as IHostedApplication;

			// If you want all web apps to have the toolbar without adding them to
			// the database, just enable this code.
			//if ( hostapp != null && hostapp.HostedApp is HostedWebApplication )
			//{
			//	useToolbar = true;
			//}

			// Create and add the toolbar to the hosted app's deckControl
			if (useToolbar && hostapp != null)
			{
				foreach (Control toolbar in deckControl.Controls)
				{
					if (toolbar is CcfPanelToolbar)
					{
						deckControl.Controls.Remove(toolbar);
						break;
					}
				}
				
				bar = new CcfPanelToolbar(hostapp);
				bar.Dock = DockStyle.Top;
			}

			// If there are no apps yet, then show app on a deckControl
			// Else we show the controls in a tabControl
			if ( deckControl.Count == 0 && tabControl.Count == 0 )
			{
				deckControl.Visible = true;
				tabControl.Visible = false;

				// Begin Dynamic
				if (closeButton)
				{
					// Add the close button toolbar to the deckControl
					CcfButtonToolbar closeButtonToolBar = new CcfButtonToolbar(child);
					closeButtonToolBar.CloseButtonClick += new CcfButtonToolbar.CloseButtonClickHandler(closeButtonClick);
					deckControl.Controls.Add(closeButtonToolBar);
					closeButtonToolBar.Dock = DockStyle.Top;
				}

				//deckControl.CloseAppClick += new CcfDeckControl.CloseAppClickHandler(closeAppClick);
				
				// End Dynamic

				if (bar != null)
				{
					// Add the CcfPanelToolbar to the deckControl
					deckControl.Controls.Add(bar);
				}

				return deckControl.ShowApplication(child, closeButton);
			}
			else
			{
				// Add the previosly added app from the deckControl to the tabControl
				if (deckControl.Count > 0)
				{
					tabControl.Visible = true;
					deckControl.Visible = false;

					tabControl.SelectedIndexChanged += new EventHandler(tabControl_SelectedIndexChanged);

					// Begin Dynamic
					tabControl.CloseAppClick += new CcfTabControl.CloseAppClickHandler(closeAppClick);
					// End Dynamic

					CcfPanelToolbar tempBar = null;

					// If there are any CcfPanelToolbar on panel for this control remove from panel and add
					// to TabPage for this app
					foreach (Control toolbar in deckControl.Controls)
					{
						if (toolbar is CcfPanelToolbar)
						{
							tempBar = toolbar as CcfPanelToolbar;
							deckControl.Controls.Remove(toolbar);
							break;
						}
						else if (toolbar is CcfButtonToolbar)
						{
							CcfButtonToolbar tempbar = toolbar as CcfButtonToolbar;
							tempbar.CloseButtonClick -= new CcfButtonToolbar.CloseButtonClickHandler(closeButtonClick);
							deckControl.Controls.Remove(tempbar);
						}
					}

					// Only 1 app on the deck control
					bool closable = deckControl.IsClosableApplication(deckControl.HostedApplications[0] as IHostedApplication);
					addApplicationToTabControl(deckControl.HostedApplications[0], tempBar, closable);
					deckControl.RemoveApplication(deckControl.HostedApplications[0]);
				}

				return addApplicationToTabControl(child, bar, closeButton);
			}
		}

		/// <summary>
		/// Event handler for when the close button is clicked on the CcfButtonToolbar
		/// </summary>
		/// <param name="application"></param>
		private void closeButtonClick(object application)
		{
			closeAppClick(application as IHostedApplication);
		}

		/// <summary>
		/// Removes an application from the CCFPanel.
		/// </summary>
		/// <param name="app"></param>
		/// <returns>True if the app was removed, false if not found</returns>
		public virtual bool Remove(object app)
		{
			try
			{
				// If only one app or control is on the panel
				if (tabControl.Count == 0 && deckControl.Count > 0)
				{
					// If the application is on this deckControl
					if (deckControl.HostedApplications[0] == app)
					{
						// Remove any toolbar on the deckControl
						foreach (Control toolbar in deckControl.Controls)
						{
							if (toolbar is CcfPanelToolbar)
							{
								deckControl.Controls.Remove(toolbar);
								break;
							}
							else if (toolbar is CcfButtonToolbar)
							{
								CcfButtonToolbar tempbar = toolbar as CcfButtonToolbar;
								tempbar.CloseButtonClick -= new CcfButtonToolbar.CloseButtonClickHandler(closeButtonClick);
								deckControl.Controls.Remove(tempbar);
							}
						}

						deckControl.RemoveApplication(app);
						deckControl.Visible = false;
						return true;
					}
				}
				else
				{
					foreach (object hostedApp in tabControl.HostedApplications)
					{
						if (hostedApp == app)
						{
							tabControl.RemoveApplication(app);

							if (tabControl.Count == 1)
							{
								object lastApp = tabControl.HostedApplications[0];
								TabPage lastPage = tabControl.CcfTabPages[0];

								if (lastPage != null)
								{
									// If there are any CcfPanelToolbar on the TabPage for this application 
									// add it to the deckControl, since the TabPage is being removed.
									foreach (Control toolbar in lastPage.Controls)
									{
										if (toolbar is CcfPanelToolbar)
										{
											deckControl.Controls.Add(toolbar as CcfPanelToolbar);
											break;
										}
									}

									bool closable = tabControl.IsClosableApplication(lastPage.Tag);

									// Begin Dynamic
									if (closable)
									{
										// Add the close button toolbar to the deckControl
										CcfButtonToolbar closeButtonToolBar = new CcfButtonToolbar(lastPage.Tag);
										closeButtonToolBar.CloseButtonClick += new CcfButtonToolbar.CloseButtonClickHandler(closeButtonClick);
										deckControl.Controls.Add(closeButtonToolBar);
										closeButtonToolBar.Dock = DockStyle.Top;
									}
									// End Dynamic

									// Close the last app on the tabControl and make the control visible
									tabControl.RemoveApplication(lastPage.Tag);
									tabControl.Visible = false;

									tabControl.SelectedIndexChanged -= new EventHandler(tabControl_SelectedIndexChanged);

									// Begin Dynamic
									tabControl.CloseAppClick -= new CcfTabControl.CloseAppClickHandler(closeAppClick);
									// End Dynamic

									// Show the app on the deckControl and make the control visible
									deckControl.ShowApplication(lastApp, closable);
									deckControl.Visible = true;
								}
							}
							else if (tabControl.Count == 0)
							{
								tabControl.Visible = false;
							}
							return true;
						}
					}
				}
			}
			catch {} // Ignore any errors

			return false; // Didn't find the app
		}

		#endregion

		#region Protected Methods

		/// <summary>
		/// Called whenever the user click to close an application.
		/// </summary>
		/// <param name="app"></param>
		protected void closeAppClick(IHostedApplication app)
		{
			if (closeApplicationClick != null)
			{
				// Notify the UI code about this change
				closeApplicationClick(app);
			}
		}

		/// <summary>
		/// Called whenever the user selects a different tab in the CCFPanel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			// remember which app is currently selected
			if (tabControl.SelectedTab != null && tabControl.SelectedTab.Tag is IHostedApplication)
				CCFAppsUI.AppWithFocus = tabControl.SelectedTab.Tag as IHostedApplication;

			CCFAppsUI.ActivePanel = tabControl.Parent as CCFPanel;

			// Notify the UI code about this change
			if (selectedAppChanged != null)
			{
				selectedAppChanged(sender, e);
			}
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Add the application to the TabControl
		/// </summary>
		/// <param name="child">The application to add</param>
		/// <param name="bar">The CcfPanelToolbar to add to the control.</param>
		/// <param name="closeButton"></param>
		/// <returns>The tab page</returns>
		private TabPage addApplicationToTabControl(object child, CcfPanelToolbar bar, bool closeButton)
		{
			TabPage tabPage = null;
			Image icon = null; // Icon for the tab
			string text; // Text for the tab

			// Get the application name and icon to be displayed on the tab
			if (child is IHostedApplication)
			{
				IHostedApplication app = child as IHostedApplication;

				// Get the app's name
				text = app.ApplicationName;

				// Get the app's icon
				ImageList imageList = app.GetIconList();
				if (imageList != null && imageList.Images.Count > 0)
				{
					icon = imageList.Images[0];
				}

				tabPage = tabControl.ShowApplication(child, text, icon, closeButton);

				if (app == CCFAppsUI.AppWithFocus)
				{
					tabControl.SelectedTab = tabPage;
				}

			}
			else if (child is Control)
			{
				text = (child as Control).Text;
				tabPage = tabControl.ShowApplication(child, text, null, closeButton);
			}

			if (bar != null)
			{
                // if a toolbar already exists don't add additional ones
                foreach (Control toolbar in tabPage.Controls)
                {
                    if (toolbar is CcfPanelToolbar)
                    {
                        tabPage.Controls.Remove(toolbar);
                        break;
                    }
                }
                tabPage.Controls.Add(bar);
			}

			return tabPage;
		}
		/// <summary>
		/// Initialization of controls
		/// </summary>
		private void InitializeControls()
		{
			// deckControl
			deckControl = new CcfDeckControl();
			Controls.Add(deckControl);
			deckControl.Dock = DockStyle.Fill;
			deckControl.Visible = false;

			// tabControl
			tabControl = new CcfTabControl();
			Controls.Add(tabControl);
			tabControl.Appearance = Appearance;
			tabControl.Alignment = Alignment;
			tabControl.Dock = DockStyle.Fill;
			tabControl.Visible = false;
		}
		#endregion
	} // CCFPanel

	/// <summary>
	/// This class overrides the WndProc to detect the User Logging Off from the system.
	/// </summary>
	public class FloatingWindowForm : Form
	{
		#region Variables
		private static bool logOffRequested = false;
		#endregion

		#region Properties
		// Properties
		/// <summary>
		/// Indicates whether Desktop logOff is initiated by the User.
		/// Set to true if LogOff message is received.
		/// </summary>
		public static bool LogOffRequested
		{
			get
			{
				return logOffRequested;
			}
			set
			{
				logOffRequested = value;
			}
		}

		#endregion

		#region Protected Methods
		/// <summary>
		/// Used for intercepting the Desktop LogOff message.
		/// </summary>
		/// <param name="message">Message</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message message)
		{
			const int WM_QUERYENDSESSION = 0x0011;  // the message which tells us of log off
			switch (message.Msg)
			{
				case WM_QUERYENDSESSION:
					logOffRequested = true;
					break;

			}
			base.WndProc(ref message);
		}
		#endregion

    } // FloatingWindowForm
}
