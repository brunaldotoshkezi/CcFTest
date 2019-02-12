//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// Hosted control for viewing and manipulating sessions.
//
//===================================================================================

#region Usings
using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr.UIConfiguration;
using Microsoft.Ccf.Samples.HostedControlInterfaces;
#endregion

namespace Microsoft.Ccf.Samples.SessionExplorerControl
{
	/// <summary>
	/// This hosted control is user interface used for manipulating the sessions.
	/// </summary>
	public class SessionExplorerControl : HostedControl, ISessionExplorer
    {
        #region Variables

        private System.Windows.Forms.Label ShowHelp;
		private System.Windows.Forms.Label SessionExp;
		private System.Windows.Forms.ComboBox SortBy;
		public System.Windows.Forms.TreeView sessionsTree;
		private System.Windows.Forms.ContextMenu sessionMenu;
		private System.Windows.Forms.MenuItem closeSession;
		private System.Windows.Forms.Label sortByLabel;
		private System.Windows.Forms.Label closeSessionLabel;
		private System.Windows.Forms.Label helpContent;
		private System.Windows.Forms.ImageList helpArrowImages;
		private System.Windows.Forms.PictureBox picHelpIcon;
		private System.ComponentModel.IContainer components;
		private System.Drawing.Icon defaultIcon = null;

        // A ManualResetEvent to wait till AgentDesktop completes its processing before any UI operation
		// is done on the isolated application in order to synchronize the UI processing of
		// Agent Desktop window and isolated application's hosting form window.
		private ManualResetEvent processIsolatedAppUIEvent = new ManualResetEvent(false);
		// The isolated application whose UI operation is kept pending for above event to get set.
		private IHostedAppUICommand isolatedAppWithPendingUIOperation;

        #endregion

        #region Properties

        private CCFAppsUI appsUI;

        public CCFAppsUI AppsUI
        {
            set { appsUI = value; }
        }

        #endregion

        #region Constructor

        public SessionExplorerControl()
        {
            init();
        }

        #endregion

        #region Events

        /// <summary>
		/// Event for when a hosted app is selected
		/// </summary>
		public event SessionExplorerEventHandler SEHostedAppSelected;

		/// <summary>
		/// Event for when a session is closed
		/// </summary>
		public event SessionExplorerEventHandler SessionClosed;

        #endregion

        #region HostedControl Members

        #region Constructor

        public SessionExplorerControl(int appID, string appName, string initString) :
		base( appID, appName, initString )
		{
			init();
        }

        #endregion

        #region Properties

        /// <summary>
		/// The manager for customer sessions.
		/// </summary>
		public override object SessionManager
		{
			set { sessionManager = (Sessions)value; }
		}
		private Sessions sessionManager;

		/// <summary>
		/// Don't list application in SessionExplorer
		/// </summary>
		public override bool IsListed
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Get of set the height of the control
		/// </summary>
		public int ControlHeight
		{
			get
			{
				return this.Height;
			}
			set
			{
				this.Height = value;
			}
		}

		/// <summary>
		/// get or set the parent container of the control
		/// </summary>
		public Control ControlParent
		{
			get
			{
				return this.Parent;
			}
			set
			{
				this.Parent = value;
			}
		}

		/// <summary>
		/// Get or set the value indicating wheather the control is displayed.
		/// </summary>
		public bool ControlVisible
		{
			get
			{
				return this.Visible;
			}
			set
			{
				this.Visible = value;
			}
        }

        #endregion

        #region Public Method

        /// <summary>
		/// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
		/// </summary>
		public void ControlRefresh()
		{
			this.Refresh();
        }

        #endregion

        #endregion

        #region Private Methods

        private void init()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// This only appears if the sharing a CCFPanel with some other app
			// or control, then this is the tab display text.
			Text = localize.SESSION_EXPLORER_MODULE_NAME;

			sortByLabel.Text = localize.SESSION_EXPLORER_LABEL_SORT_BY_TEXT;
			this.ShowHelp.Text = localize.SESSION_EXPLORER_LABEL_HELP_ICON_TEXT;
			this.closeSessionLabel.Text = localize.SESSION_EXPLORER_CLOSE_SESSION;
 
			// This does the faded background paint on the top panel
			SessionExp.Paint += new PaintEventHandler(SessionExp_Paint);

			// This is the images which are shown in the treeview for all the sessions
			// and their applications.
			sessionsTree.ImageList = new ImageList();
			LoadDefaultIcon();

			// Add Customer Name and Call Order or SortBy options to the combo box
			SortBy.Items.AddRange( new object [] {localize.SESSION_EXPLORER_STR_SORTBY_CUSTOMER_NAME,
													 localize.SESSION_EXPLORER_STR_SORTBY_CALL_DATETIME} );

			SortBy.SelectedIndex = 0;  // also causes this.RefreshView() to run
        }

        /// <summary>
        /// Create and return an application node
        /// </summary>
        /// <param name="app"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        private TreeNode CreateApplicationNode(IHostedApplication app, Microsoft.Ccf.Csr.Session session)
        {

            // Add the app's icon to the tabControl's list
            ImageList imageList = app.GetIconList();
            if (imageList != null && imageList.Images.Count > 0)
            {
                System.Drawing.Image image = imageList.Images[0];
                sessionsTree.ImageList.Images.Add(image);
            }

            TreeNode node = new TreeNode(app.ApplicationName);
            node.Tag = app;

            // if there was no image for this application, then the image for
            // the previous application will be used.
            node.ImageIndex = sessionsTree.ImageList.Images.Count - 1;
            node.SelectedImageIndex = node.ImageIndex;

			if (session != null)
			{
				// If this app has the current selection, select it in the tree
				if (app == session.FocusedApplication)
					sessionsTree.SelectedNode = node;
			}

            return node;
        }

        /// <summary>
        /// Calls the SetFocus() of the isolated hosted application proxy on a different
        /// thread so that the UI operations across process boundaries can be synchronized.
        /// </summary>
        /// <param name="app">The AppUICommand of the app to be brought to focus</param>
        private void FocusIsolatedApplication(IHostedAppUICommand app)
        {
            isolatedAppWithPendingUIOperation = app;
            System.Threading.Thread th = new Thread(new System.Threading.ThreadStart(CallSetFocus));
            th.Start();
        }

        /// <summary>
        /// Calls the SetFocus() of the isolated hosted application after 
        /// Agent Desktop is done with its processing and the UI control can be passed to
        /// the hosted application.
        /// </summary>
        private void CallSetFocus()
        {
            // Wait till Agent Desktop is done with its own UI processing.
            processIsolatedAppUIEvent.WaitOne();
            processIsolatedAppUIEvent.Reset();

            if (isolatedAppWithPendingUIOperation != null)
            {
                isolatedAppWithPendingUIOperation.SetFocus();
            }
        }

        /// <summary>
        /// Used for setting the default icon which is visible besides the Session Node in the Session Explorer.
        /// Called whenever ImageList() is rebuilt.
        /// </summary>
        private void LoadDefaultIcon()
        {
            if (defaultIcon != null)
            {
                //Default Icon is already created.
                this.sessionsTree.ImageList.Images.Add(defaultIcon);
                return;
            }
            // Get the current executing assembly
            try
            {
                Assembly asWinApp = Assembly.GetExecutingAssembly();
                string iconResourceName = "Microsoft.Ccf.Samples.SessionExplorerControl.Icon.ico";
                Stream strmIco = asWinApp.GetManifestResourceStream(iconResourceName);

                // get the icon stream
                defaultIcon = new System.Drawing.Icon(strmIco);
                this.sessionsTree.ImageList.Images.Add(defaultIcon);
            }
            catch (Exception eX)
            {
                Logging.Error(localize.SESSION_EXPLORER_MODULE_NAME, eX.Message);
            }
        }

        #endregion

        #region Protected Methods

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

        /// <summary>
        /// Updates a number of session explorer fields to the current state.
        /// </summary>
        protected void updateFields()
        {
            // Build the string which says how many sessions we have
            String caption = localize.SESSION_EXPLORER_MODULE_NAME + " " +
                String.Format(localize.SESSION_EXPLORER_SESSIONS_COUNT,
                sessionsTree.Nodes.Count.ToString());
            this.SessionExp.Text = caption;

            bool enable = (sessionsTree.Nodes.Count != 0);
            SortBy.Enabled = enable;
            closeSessionLabel.Enabled = enable;
        }

        /// <summary>
        /// Closes a session.  Callable from a link on the session explorer.
        /// Also was callable from a context menu, however that has been disconnected
        /// since it causes issues for agents since they can accidently close a 
        /// session/customer transaction which they are not currently speaking to.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void closeSession_Click(object sender, System.EventArgs e)
        {
            TreeNode node;
            Session session = null;

            if (sessionsTree.SelectedNode != null)
            {
                node = sessionsTree.SelectedNode;

                // find the session that was clicked upon
                if (node != null && node.Parent != null)
                {
                    node = node.Parent;
                }
                if (node != null)
                {
                    session = node.Tag as Session;
                }
                if (session != null)
                {
                    if (SessionClosed != null)
                    {
                        // Fires an event to desktop to close the session
                        SessionClosed(this, new EventArgs());
                    }
                }
            }
        }

        /// <summary>
        /// Change the sorting of the session explorer display.
        /// </summary>
        /// <param name="sender">Sending object.</param>
        /// <param name="e">EventArgs object.</param>
        protected void SortBy_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            RefreshView();
        }

        /// <summary>
        /// This prevents selecting a application in another session from causing 
        /// a temporary select of an incorrect app and the resultant screen updates.
        /// </summary>
        /// <summary>
        /// A session or application node has been selected in the sessions tree.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void sessionsTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            processIsolatedAppUIEvent.Reset();

            try
            {
                Session session = null;
                IHostedApplication app;

                // so we know if this is caused by an agent changing the app with the control
                // or if this is happening from some other event within CCF.
                bool hadFocus = sessionsTree.Focused;

                if (e.Node != null)
                {
                    session = e.Node.Tag as Session;
                }
                if (session != null)
                {
                    // go to the session selected in the explorer, let it pick which
                    // app is highlighted.
                    sessionManager.SetActiveSession(session.SessionID);
                }
                else
                {
                    // go to the application selected in the session explorer
                    app = e.Node.Tag as IHostedApplication;
                    if (app != null && appsUI != null)
                    {
                        session = e.Node.Parent.Tag as Session;
                        if (session != null)
                        {
                            session.FocusedApplication = app; // so app is selected
                            sessionManager.SetActiveSession(session.SessionID);
                        }

                        // TODO LJZ
                        bool isIsolated = session.AppHost.IsIsolatedApplication(app);
                        bool isExtended = session.AppHost.IsExtendedApplication(app);
                        if (isIsolated || isExtended)
                        {
                            IHostedAppUICommand appUICommand = app as IHostedAppUICommand;

                            if (isIsolated)
                            {
                                FocusIsolatedApplication(appUICommand);
                            }
                            if (isExtended)
                            {
                                appUICommand.ShowForm();
                            }
                            if (null != SEHostedAppSelected)
                            {
                                SEHostedAppSelected(app, new EventArgs());
                            }
                        }
                        else
                        {
                            appsUI.SelectApplication(app.ApplicationID);
                        }

                        // Return the focus to the newly selected node if we had the focus
                        // to begin with.
                        if (hadFocus)
                        {
                            sessionsTree.Focus();
                        }
                    }
                }
            }
            catch (Exception eX)
            {
                Logging.Error(localize.SESSION_EXPLORER_MODULE_NAME, eX.Message);
            }
            finally
            {
                processIsolatedAppUIEvent.Set();
            }
        }

        /// <summary>
        /// Show or hide the short not very useful help screen.
        /// TODO: This should be integrated into a real help system.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void HelpIcon_Click(object sender, System.EventArgs e)
        {
            try
            {
                showHelp(!helpContent.Visible);
                ShowHelp.ImageIndex = (ShowHelp.ImageIndex == 1) ? 0 : 1;

                helpContent.Text = String.Format("{0}\n{1}", localize.SESSION_EXPLORER_STR_HELP_STRING1, localize.SESSION_EXPLORER_STR_HELP_STRING2);
            }
            catch (Exception exp)
            {
                Logging.Error(this.ToString(), localize.SESSION_EXPLORER_ERR_DISPLAYING_HELP, exp);
                // No need to throw this error
            }
        }

        /// <summary>
        /// When the help text is hidden, allow the session tree to expand to fill its
        /// space.
        /// </summary>
        /// <param name="show">true to show help text, false to hide it</param>
        protected void showHelp(bool show)
        {
            helpContent.Visible = show;
            if (show)
            {
                this.ShowHelp.Top = helpContent.Top - ShowHelp.Height - 8;
            }
            else
            {
                this.ShowHelp.Top = this.Height - ShowHelp.Height - 8;
            }

            this.picHelpIcon.Top = ShowHelp.Top;
            this.closeSessionLabel.Top = ShowHelp.Top;
            this.sessionsTree.Height = ShowHelp.Top - sessionsTree.Top - 10;
        }

        /// <summary>
        /// Reflow the locations and sizes of some controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SessionCtl_Resize(object sender, System.EventArgs e)
        {
            showHelp(helpContent.Visible);
        }

        /// <summary>
        /// Paint the faded background at the top of the session explorer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SessionExp_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, SessionExp.Width - 1, SessionExp.Height - 1);
            using (LinearGradientBrush lgBrush = new LinearGradientBrush(rect, System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF), System.Drawing.Color.FromArgb(165, 207, 250), LinearGradientMode.Horizontal))
            {
                e.Graphics.FillRectangle(lgBrush, rect);
            }

            e.Graphics.DrawImage(SessionExp.Image, 5, 3, SessionExp.Image.Size.Width, SessionExp.Image.Size.Height);

            using (SolidBrush drawBrush = new SolidBrush(Color.RoyalBlue))
            {
                e.Graphics.DrawString(SessionExp.Text, SessionExp.Font, drawBrush, 25.0F, 4.0F);
            }
        }

        #endregion

        #region Public Methods

		/// <summary>
		/// Add a single application node.  If the application is global then will add
		/// for all session, else will add only for the active session passed in.
		/// </summary>
		/// <param name="session">The current active session</param>
		/// <param name="app">The application to add</param>
		public void AddApplicationNode(Session session, IHostedApplication app)
		{
			foreach (TreeNode sessionNode in sessionsTree.Nodes)
			{
				// If app is global then add for all sessions
				if (session.AppHost.IsGlobalApplication(app))
				{
					// If this node is the current active session
					if (session.Name.ToString() == sessionNode.Text.ToString())
					{
						sessionNode.Nodes.Add(CreateApplicationNode(app, session));
					}
					else
					{
						sessionNode.Nodes.Add(CreateApplicationNode(app, null));
					}
				}
				// Not global so only add for correct session
				else if (session.Name.ToString() == sessionNode.Text.ToString())
				{
					sessionNode.Nodes.Add(CreateApplicationNode(app, session));

					return;
				}
			}
		}

		/// <summary>
		/// Remove a single application node from the session explorer.  If the application is
		/// global then will remove for all session, else will remove only for the active session
		/// passed in.
		/// </summary>
		/// <param name="session">The current active session</param>
		/// <param name="app">The application to remove</param>
		public void RemoveApplicationNode(Session session, IHostedApplication app)
		{
			if (app != null)
			{
				// If the application is global
				if (session.AppHost.IsGlobalApplication(app))
				{
					foreach (TreeNode sessionNode in sessionsTree.Nodes)
					{
						// Remove of nodes for the all session
						ArrayList al = new ArrayList();
						foreach (TreeNode applicationNode in sessionNode.Nodes)
						{
							IHostedApplication adapter = applicationNode.Tag as IHostedApplication;

							// Remove only 
							if (adapter.ApplicationName == app.ApplicationName)
							{
								al.Add(applicationNode);
							}
						}

						foreach (object o in al)
						{
							TreeNode tn = (TreeNode)o;
							sessionNode.Nodes.Remove(tn);
						}
						sessionsTree.ExpandAll();

					}
				}
				// The application is non-global
				else
				{
					foreach (TreeNode sessionNode in sessionsTree.Nodes)
					{
						// Remove of application nodes only for the correct session
						if (session.Name.ToString() == sessionNode.Text.ToString())
						{
							ArrayList al = new ArrayList();
							foreach (TreeNode applicationNode in sessionNode.Nodes)
							{
								IHostedApplication adapter = applicationNode.Tag as IHostedApplication;

								// Remove only 
								if (adapter.ApplicationName == app.ApplicationName)
								{
									al.Add(applicationNode);
								}
							}

							foreach (object o in al)
							{
								TreeNode tn = (TreeNode)o;
								sessionNode.Nodes.Remove(tn);
							}
							sessionsTree.ExpandAll();
						}
					}
				}
			}
		}


        /// <summary>
        /// Remove the applications for a session from the SessionExplorerControl
        /// </summary>
        /// <param name="session"></param>
        public void RemoveWorkflowApplicationNodes(Session session)
        {
            IHostedApplication app;

            foreach (TreeNode sessionNode in sessionsTree.Nodes)
            {
                // Remove of workflow application nodes for the correct session
                if (session.Name.ToString() == sessionNode.Text.ToString())
                {	// Collect all workflow tagged applications 
                    ArrayList al = new ArrayList();
                    foreach (TreeNode applicationNode in sessionNode.Nodes)
                    {
                        IHostedApplication adapter = applicationNode.Tag as IHostedApplication;
                        app = adapter;

                        // Remove only the tagged globabl and tagged non-global apps
                        if (session.AppHost.IsTaggedApplication(app))
                        {
                            al.Add(applicationNode);
                        }
                    }

                    foreach (object o in al)
                    {
                        TreeNode tn = (TreeNode)o;
                        sessionNode.Nodes.Remove(tn);
                    }
                    sessionsTree.ExpandAll();
                }
            }
        }

        /// <summary>
        /// Add tagged global and tagged non-global applications nodes to the Session Explorer
        /// </summary>
        public void AddWorkflowApplicationNodes(Microsoft.Ccf.Csr.Session session, bool selected)
        {
            // find the session
            foreach (TreeNode sessionNode in sessionsTree.Nodes)
            {
                if (session.Name.ToString() == sessionNode.Text.ToString())
                {
                    // add each tagged application node to the tree under that session
                    foreach (IHostedApplication app in session)
                    {
                        if (session.AppHost.IsTaggedApplication(app))
                        {
                            sessionNode.Nodes.Add(CreateApplicationNode(app, session));
                        }
                    }
                    return;
                }
            }
        }

        /// <summary>
        /// Adds the session to the tree control if its not already there
        /// </summary>
        public void AddSession(Microsoft.Ccf.Csr.Session session, bool selected)
        {
            TreeNode parent, node;

            // if there is no session name, then hide this session
            if (session == null || session.Name == null)
            {
                return;
            }

            // see if active session is already in the session explorer
            foreach (TreeNode sessionNode in sessionsTree.Nodes)
            {
                if (sessionNode.Tag == session)
                {
                    sessionNode.Text = session.Name; // in case the name has changed
                    return;
                }
            }

            parent = new TreeNode(session.Name);
            parent.Tag = session;

            // find where to insert this session node at based on the sort
            bool sortByName = (SortBy.SelectedItem as string == localize.SESSION_EXPLORER_STR_SORTBY_CUSTOMER_NAME);
            int position = 0;
            for (; position < sessionsTree.Nodes.Count; position++)
            {
                node = sessionsTree.Nodes[position];

                if (sortByName && parent.Text.CompareTo(node.Text) < 0)
                    break;
                if (!sortByName)
                {
                    Session oldSession = node.Tag as Session;
                    if (session != null && session.StartTime < oldSession.StartTime)
                        break;
                }
            }
            sessionsTree.Nodes.Insert(position, parent);

            // if asked to select this node or if its the active session, then select it
            if (selected || session == sessionManager.ActiveSession)
            {
                sessionsTree.SelectedNode = parent;
            }

            // Add the applications in this session to the session explorer view
            foreach (IHostedApplication app in session)
            {
                if ((app as IHostedApplication3) != null && (app as IHostedApplication3).IsListed)
                {
                    // This prevents external apps that are not integrated
                    // into CCF from being shown in the session explorer.
                    if (app.TopLevelWindow != null)
                    {
                        // if workflow has not started, empty workflow means no workflow has started
                        if (session.Workflow == string.Empty)
                        {
                            // only add untagged applications
                            if (!session.AppHost.IsTaggedApplication(app))
                            {
                                node = CreateApplicationNode(app, session);
                                parent.Nodes.Add(node);
                            }

                        }
                        else  //workflow has started so add all apps
                        {
                            node = CreateApplicationNode(app, session);
                            parent.Nodes.Add(node);
                        }
                    }
                }
            }

            sessionsTree.ExpandAll();

            updateFields();  // updates some fields to current state
        }

        /// <summary>
        /// Complete redraw of the session explorer UI.
        /// </summary>
        public void RefreshView()
        {
            IHostedApplication app = null;
            try
            {
                if (sessionsTree.SelectedNode != null &&
                    sessionsTree.SelectedNode.Tag != null)
                {
                    app = sessionsTree.SelectedNode.Tag as IHostedApplication;
                }

                // so the node select code doesn't run until we're done.
                sessionsTree.AfterSelect -= new System.Windows.Forms.TreeViewEventHandler(this.sessionsTree_AfterSelect);

                sessionsTree.SuspendLayout();

                // Erase the current tree
                sessionsTree.Nodes.Clear();

                if (sessionsTree.ImageList != null)
                    sessionsTree.ImageList.Dispose();
                sessionsTree.ImageList = new ImageList();
                LoadDefaultIcon();

                if (sessionManager != null)
                {
                    foreach (Session session in sessionManager)
                    {
                        AddSession(session, false);
                    }
                }

                sessionsTree.ExpandAll();
            }
            finally
            {
                sessionsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sessionsTree_AfterSelect);
                sessionsTree.ResumeLayout(true);

                updateFields();  // updates some fields to current state

                if (app != null)
                    this.FocusOnApplication(app);
                //if the session node is selected instead of application node, app will be null
                //hence set the selection to the appropriate node.
                //validate thoroughly for null before calling FocusOnApplication
                else if (sessionManager != null &&
                    sessionManager.ActiveSession != null &&
                    sessionManager.ActiveSession.FocusedApplication != null)
                {
                    this.FocusOnApplication(sessionManager.ActiveSession.FocusedApplication);
                }
            }
        }

        /// <summary>
        /// Select the given application's node in the treeview
        /// </summary>
        /// <param name="appWithFocus">Application to focus on</param>
        public void FocusOnApplication(IHostedApplication appWithFocus)
        {
            // check if it already has the focus
            if (sessionsTree.SelectedNode != null &&
                sessionsTree.SelectedNode.Tag == appWithFocus)
            {
                if (sessionsTree.SelectedNode.Parent != null &&
                    sessionsTree.SelectedNode.Parent.Tag == sessionManager.ActiveSession)
                    return;
            }

            foreach (TreeNode parent in sessionsTree.Nodes)
            {
                if (parent.Tag == sessionManager.ActiveSession)
                {
                    foreach (TreeNode node in parent.Nodes)
                    {
                        if (node.Tag == appWithFocus)
                        {
                            sessionsTree.SelectedNode = node;
                            return;
                        }
                    }
                    // This needs to be done cos in the scenario where there are no applications configured
                    // changing session from Current Session drop down will not select/highlight the relevant 
                    // node in session treeview.
                    if (parent.Nodes.Count == 0)
                    {
                        sessionsTree.SelectedNode = parent;
                    }
                }
            }
        }

        /// <summary>
        /// Removes a session from the session explorer tree.
        /// </summary>
        /// <param name="session"></param>
        public void RemoveSession(Microsoft.Ccf.Csr.Session session)
        {
            foreach (TreeNode node in sessionsTree.Nodes)
            {
                if (session == node.Tag)
                {
                    sessionsTree.Nodes.Remove(node);
                    break;
                }
            }

            updateFields();  // updates some fields to current state
        }

        #endregion

        #region Component Designer generated code
        /// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SessionExplorerControl));
			this.helpContent = new System.Windows.Forms.Label();
			this.SortBy = new System.Windows.Forms.ComboBox();
			this.sessionsTree = new System.Windows.Forms.TreeView();
			this.sessionMenu = new System.Windows.Forms.ContextMenu();
			this.closeSession = new System.Windows.Forms.MenuItem();
			this.ShowHelp = new System.Windows.Forms.Label();
			this.helpArrowImages = new System.Windows.Forms.ImageList(this.components);
			this.SessionExp = new System.Windows.Forms.Label();
			this.sortByLabel = new System.Windows.Forms.Label();
			this.closeSessionLabel = new System.Windows.Forms.Label();
			this.picHelpIcon = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// helpContent
			// 
			this.helpContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.helpContent.ForeColor = System.Drawing.Color.FromArgb(((System.Byte)(43)), ((System.Byte)(120)), ((System.Byte)(200)));
			this.helpContent.Location = new System.Drawing.Point(8, 248);
			this.helpContent.Name = "helpContent";
			this.helpContent.Size = new System.Drawing.Size(237, 96);
			this.helpContent.TabIndex = 13;
			this.helpContent.Text = "Help Contents";
			this.helpContent.Visible = false;
			// 
			// SortBy
			// 
			this.SortBy.BackColor = System.Drawing.Color.White;
			this.SortBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SortBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.SortBy.Location = new System.Drawing.Point(72, 40);
			this.SortBy.MaxDropDownItems = 2;
			this.SortBy.Name = "SortBy";
			this.SortBy.Size = new System.Drawing.Size(176, 21);
			this.SortBy.TabIndex = 11;
			this.SortBy.SelectedIndexChanged += new System.EventHandler(this.SortBy_SelectedIndexChanged);
			// 
			// sessionsTree
			// 
			this.sessionsTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.sessionsTree.FullRowSelect = true;
			this.sessionsTree.HideSelection = false;
			this.sessionsTree.ImageIndex = -1;
			this.sessionsTree.Location = new System.Drawing.Point(8, 72);
			this.sessionsTree.Name = "sessionsTree";
			this.sessionsTree.SelectedImageIndex = -1;
			this.sessionsTree.Size = new System.Drawing.Size(240, 150);
			this.sessionsTree.TabIndex = 10;
			this.sessionsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.sessionsTree_AfterSelect);
			// 
			// sessionMenu
			// 
			this.sessionMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.closeSession});
			// 
			// closeSession
			// 
			this.closeSession.Index = 0;
			this.closeSession.Text = "Close Session (not used for now)";
			this.closeSession.Click += new System.EventHandler(this.closeSession_Click);
			// 
			// ShowHelp
			// 
			this.ShowHelp.CausesValidation = false;
			this.ShowHelp.Cursor = System.Windows.Forms.Cursors.Hand;
			this.ShowHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.ShowHelp.ForeColor = System.Drawing.Color.RoyalBlue;
			this.ShowHelp.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ShowHelp.ImageIndex = 0;
			this.ShowHelp.ImageList = this.helpArrowImages;
			this.ShowHelp.Location = new System.Drawing.Point(40, 224);
			this.ShowHelp.Name = "ShowHelp";
			this.ShowHelp.Size = new System.Drawing.Size(48, 16);
			this.ShowHelp.TabIndex = 15;
			this.ShowHelp.Text = "Help";
			this.ShowHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.ShowHelp.Click += new System.EventHandler(this.HelpIcon_Click);
			// 
			// helpArrowImages
			// 
			this.helpArrowImages.ImageSize = new System.Drawing.Size(16, 16);
			this.helpArrowImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("helpArrowImages.ImageStream")));
			this.helpArrowImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// SessionExp
			// 
			this.SessionExp.BackColor = System.Drawing.Color.Lavender;
			this.SessionExp.Dock = System.Windows.Forms.DockStyle.Top;
			this.SessionExp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.SessionExp.ForeColor = System.Drawing.Color.RoyalBlue;
			this.SessionExp.Image = ((System.Drawing.Image)(resources.GetObject("SessionExp.Image")));
			this.SessionExp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.SessionExp.Location = new System.Drawing.Point(0, 0);
			this.SessionExp.Name = "SessionExp";
			this.SessionExp.Size = new System.Drawing.Size(256, 25);
			this.SessionExp.TabIndex = 14;
			this.SessionExp.Text = "SessionExplorer";
			this.SessionExp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// sortByLabel
			// 
			this.sortByLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.sortByLabel.Location = new System.Drawing.Point(8, 40);
			this.sortByLabel.Name = "sortByLabel";
			this.sortByLabel.Size = new System.Drawing.Size(48, 21);
			this.sortByLabel.TabIndex = 17;
			this.sortByLabel.Text = "Sort By:";
			this.sortByLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// closeSessionLabel
			// 
			this.closeSessionLabel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.closeSessionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.closeSessionLabel.ForeColor = System.Drawing.Color.RoyalBlue;
			this.closeSessionLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
			this.closeSessionLabel.Location = new System.Drawing.Point(160, 224);
			this.closeSessionLabel.Name = "closeSessionLabel";
			this.closeSessionLabel.Size = new System.Drawing.Size(80, 16);
			this.closeSessionLabel.TabIndex = 18;
			this.closeSessionLabel.Text = "Close Session";
			this.closeSessionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.closeSessionLabel.Click += new System.EventHandler(this.closeSession_Click);
			// 
			// picHelpIcon
			// 
			this.picHelpIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.picHelpIcon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picHelpIcon.Image = ((System.Drawing.Image)(resources.GetObject("picHelpIcon.Image")));
			this.picHelpIcon.Location = new System.Drawing.Point(16, 224);
			this.picHelpIcon.Name = "picHelpIcon";
			this.picHelpIcon.Size = new System.Drawing.Size(16, 16);
			this.picHelpIcon.TabIndex = 19;
			this.picHelpIcon.TabStop = false;
			this.picHelpIcon.Click += new System.EventHandler(this.HelpIcon_Click);
			// 
			// SessionCtl
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.picHelpIcon);
			this.Controls.Add(this.closeSessionLabel);
			this.Controls.Add(this.sortByLabel);
			this.Controls.Add(this.ShowHelp);
			this.Controls.Add(this.helpContent);
			this.Controls.Add(this.SortBy);
			this.Controls.Add(this.sessionsTree);
			this.Controls.Add(this.SessionExp);
			this.Name = "SessionCtl";
			this.Size = new System.Drawing.Size(256, 350);
			this.Resize += new System.EventHandler(this.SessionCtl_Resize);
			this.ResumeLayout(false);

		}
		#endregion

	}
}
