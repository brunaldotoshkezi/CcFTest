//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// This user control hosted app will display short summary of
// the currently active hosted application with regard to the customer.
// 
//===============================================================================

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Csr;

namespace Microsoft.Ccf.Samples.CurrentSessionControl
{
	/// <summary>
	/// This user control hosted app will display short summary of
	/// the currently active hosted application with regard to the customer.
	/// </summary>
	public class CurrentSessionControl : HostedControl
	{
		private System.Windows.Forms.Label lblFName;
		private System.Windows.Forms.Label lblCustomer;
		private System.Windows.Forms.Label lblLName;
		private System.Windows.Forms.Label lblPhoneNumber;
		private System.Windows.Forms.Label lblCurrentSession;
		private System.Windows.Forms.ComboBox cboSessions;
		private System.Windows.Forms.Label lblCust;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.Label lblPhoneNum;
		private System.Windows.Forms.Label lblCurrApp;
		private System.Windows.Forms.ToolTip ttOpenApps;
		private System.Windows.Forms.Label lblCurrentApp;
		private System.ComponentModel.IContainer components;

		public CurrentSessionControl()
		{
			init();
		}

		public CurrentSessionControl(int appID, string appName, string initString) :
		base( appID, appName, initString )
		{
			init();
		}

		private void init()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			lblCurrentSession.Text = localize.CURRENT_SESSION_LABEL_CURRENT_SESSION;
			lblCustomer.Text = localize.CURRENT_SESSION_LABEL_CUSTOMER;
			lblFName.Text = localize.CURRENT_SESSION_LABEL_FIRST_NAME;
			lblLName.Text = localize.CURRENT_SESSION_LABEL_LAST_NAME;
			lblPhoneNumber.Text = localize.CURRENT_SESSION_LABEL_PHONE_NUMBER;
			lblCurrentApp.Text = localize.CURRENT_SESSION_LABEL_CURRENT_APPLICATION;
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
			this.components = new System.ComponentModel.Container();
			this.lblFName = new System.Windows.Forms.Label();
			this.lblCustomer = new System.Windows.Forms.Label();
			this.lblLName = new System.Windows.Forms.Label();
			this.lblPhoneNumber = new System.Windows.Forms.Label();
			this.lblCurrentApp = new System.Windows.Forms.Label();
			this.cboSessions = new System.Windows.Forms.ComboBox();
			this.lblCurrentSession = new System.Windows.Forms.Label();
			this.lblCust = new System.Windows.Forms.Label();
			this.lblLastName = new System.Windows.Forms.Label();
			this.lblFirstName = new System.Windows.Forms.Label();
			this.lblPhoneNum = new System.Windows.Forms.Label();
			this.lblCurrApp = new System.Windows.Forms.Label();
			this.ttOpenApps = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// lblFName
			// 
			this.lblFName.BackColor = System.Drawing.Color.Transparent;
			this.lblFName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblFName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblFName.Location = new System.Drawing.Point(376, 46);
			this.lblFName.Name = "lblFName";
			this.lblFName.Size = new System.Drawing.Size(112, 20);
			this.lblFName.TabIndex = 3;
			this.lblFName.Text = "First Name:";
			// 
			// lblCustomer
			// 
			this.lblCustomer.BackColor = System.Drawing.Color.Transparent;
			this.lblCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCustomer.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCustomer.Location = new System.Drawing.Point(18, 46);
			this.lblCustomer.Name = "lblCustomer";
			this.lblCustomer.Size = new System.Drawing.Size(108, 20);
			this.lblCustomer.TabIndex = 5;
			this.lblCustomer.Text = "Customer:";
			// 
			// lblLName
			// 
			this.lblLName.BackColor = System.Drawing.Color.Transparent;
			this.lblLName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblLName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblLName.Location = new System.Drawing.Point(18, 80);
			this.lblLName.Name = "lblLName";
			this.lblLName.Size = new System.Drawing.Size(112, 20);
			this.lblLName.TabIndex = 7;
			this.lblLName.Text = "Last Name:";
			// 
			// lblPhoneNumber
			// 
			this.lblPhoneNumber.BackColor = System.Drawing.Color.Transparent;
			this.lblPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPhoneNumber.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblPhoneNumber.Location = new System.Drawing.Point(376, 80);
			this.lblPhoneNumber.Name = "lblPhoneNumber";
			this.lblPhoneNumber.Size = new System.Drawing.Size(112, 20);
			this.lblPhoneNumber.TabIndex = 9;
			this.lblPhoneNumber.Text = "Phone Number:";
			// 
			// lblCurrentApp
			// 
			this.lblCurrentApp.BackColor = System.Drawing.Color.Transparent;
			this.lblCurrentApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCurrentApp.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCurrentApp.Location = new System.Drawing.Point(18, 116);
			this.lblCurrentApp.Name = "lblCurrentApp";
			this.lblCurrentApp.Size = new System.Drawing.Size(112, 20);
			this.lblCurrentApp.TabIndex = 13;
			this.lblCurrentApp.Text = "Current Application:";
			// 
			// cboSessions
			// 
			this.cboSessions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.cboSessions.ForeColor = System.Drawing.SystemColors.ControlText;
			this.cboSessions.Location = new System.Drawing.Point(132, 10);
			this.cboSessions.Name = "cboSessions";
			this.cboSessions.Size = new System.Drawing.Size(444, 21);
			this.cboSessions.TabIndex = 15;
			this.cboSessions.SelectedIndexChanged += new System.EventHandler(this.cboSessions_SelectedIndexChanged);
			// 
			// lblCurrentSession
			// 
			this.lblCurrentSession.BackColor = System.Drawing.Color.Transparent;
			this.lblCurrentSession.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCurrentSession.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCurrentSession.Location = new System.Drawing.Point(18, 12);
			this.lblCurrentSession.Name = "lblCurrentSession";
			this.lblCurrentSession.Size = new System.Drawing.Size(108, 20);
			this.lblCurrentSession.TabIndex = 16;
			this.lblCurrentSession.Text = "Current Session:";
			// 
			// lblCust
			// 
			this.lblCust.BackColor = System.Drawing.Color.Transparent;
			this.lblCust.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCust.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCust.Location = new System.Drawing.Point(132, 46);
			this.lblCust.Name = "lblCust";
			this.lblCust.Size = new System.Drawing.Size(224, 20);
			this.lblCust.TabIndex = 17;
			// 
			// lblLastName
			// 
			this.lblLastName.BackColor = System.Drawing.Color.Transparent;
			this.lblLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblLastName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblLastName.Location = new System.Drawing.Point(132, 80);
			this.lblLastName.Name = "lblLastName";
			this.lblLastName.Size = new System.Drawing.Size(226, 20);
			this.lblLastName.TabIndex = 18;
			// 
			// lblFirstName
			// 
			this.lblFirstName.BackColor = System.Drawing.Color.Transparent;
			this.lblFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblFirstName.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblFirstName.Location = new System.Drawing.Point(490, 46);
			this.lblFirstName.Name = "lblFirstName";
			this.lblFirstName.Size = new System.Drawing.Size(224, 20);
			this.lblFirstName.TabIndex = 19;
			// 
			// lblPhoneNum
			// 
			this.lblPhoneNum.BackColor = System.Drawing.Color.Transparent;
			this.lblPhoneNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblPhoneNum.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblPhoneNum.Location = new System.Drawing.Point(490, 80);
			this.lblPhoneNum.Name = "lblPhoneNum";
			this.lblPhoneNum.Size = new System.Drawing.Size(224, 20);
			this.lblPhoneNum.TabIndex = 21;
			// 
			// lblCurrApp
			// 
			this.lblCurrApp.BackColor = System.Drawing.Color.Transparent;
			this.lblCurrApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblCurrApp.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lblCurrApp.Location = new System.Drawing.Point(132, 116);
			this.lblCurrApp.Name = "lblCurrApp";
			this.lblCurrApp.Size = new System.Drawing.Size(226, 20);
			this.lblCurrApp.TabIndex = 22;
			// 
			// CurrentSessionControl
			// 
			this.AutoScroll = true;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.lblCurrApp);
			this.Controls.Add(this.lblPhoneNum);
			this.Controls.Add(this.lblFirstName);
			this.Controls.Add(this.lblLastName);
			this.Controls.Add(this.lblCust);
			this.Controls.Add(this.lblCurrentSession);
			this.Controls.Add(this.cboSessions);
			this.Controls.Add(this.lblCurrentApp);
			this.Controls.Add(this.lblPhoneNumber);
			this.Controls.Add(this.lblLName);
			this.Controls.Add(this.lblCustomer);
			this.Controls.Add(this.lblFName);
			this.Icon = null;
			this.Name = "CurrentSessionControl";
			this.Size = new System.Drawing.Size(720, 144);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.CurrentSessionUI_Paint);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Set the SessionManager value
		/// </summary>
		public override object SessionManager
		{
			set
			{
				sessionManager = (Sessions)value;
			}
		}
		private Sessions sessionManager;

		/// <summary>
		/// This overrides the default value because we dont want this control to 
		/// be listed in the SessionExplorer and the CurrentSessionUI
		/// </summary>
		public override bool IsListed
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets called whenever the context has changed
		/// </summary>
		/// <param name="context"></param>
		public override void NotifyContextChange(Context context)
		{
			if(context.Count != 0)
			{
				lblPhoneNum.Text = context["Landline"];
				lblFirstName.Text = context["CustomerFirstName"];
				lblLastName.Text = context["CustomerLastName"];
				lblCust.Text = String.Format("{0} {1}", context["CustomerFirstName"], context["CustomerLastName"]);
			}

			if(sessionManager != null)
			{
				lblCurrApp.Text = "";

				IHostedApplication3 FocusedApp = (sessionManager.ActiveSession.FocusedApplication as IHostedApplication3);
				
				if(FocusedApp != null && FocusedApp.IsListed)
				{
					lblCurrApp.Text = FocusedApp.ApplicationName;
				}

				// Used when session is changed to set the current session
				if ( cboSessions.SelectedItem != sessionManager.ActiveSession )
				{
					cboSessions.SelectedItem = sessionManager.ActiveSession;
				}
			}
		}

		/// <summary>
		/// Called to perform the action configured in the database.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="data"></param>
		public override void DoAction( Action action, string data )
		{
			if(action.Name == "FocusedAppChanged")
			{
				// display current focus app
				if (sessionManager.ActiveSession != null)
				{
					IHostedApplication3 FocusedApp = (sessionManager.ActiveSession.FocusedApplication as IHostedApplication3);

					if (FocusedApp != null && FocusedApp.IsListed)
					{
						lblCurrApp.Text = FocusedApp.ApplicationName;
					}
					// Case of a global open dynamic application which was just closed
					if (FocusedApp == null)
						ClearLabels();
				}
			}
			else if(action.Name == "LoadAllSessions")
			{
				// load all session into combobox
				LoadAllSessions();
			}
			else if(action.Name == "RemoveSession")
			{
				// remove session data from UI
				RemoveSession(data);
			}
			else if(action.Name == "Show_Hide")
			{
				Show_Hide(data);
			}
		}

		/// <summary>
		/// Show or hide current session control panel.  Only used for DesktopToolbar version, when
		/// this application is floating
		/// </summary>
		/// <param name="location"></param>
		private void Show_Hide(string location)
		{
			Point point = (Point)GeneralFunctions.Deserialize(location, typeof(Point));

			// Get the from this TopLevelWindow is on.
			// TopLevelWindow -> CcfDeckControl -> CcfPanel -> Form
			Form form = this.Parent.Parent.Parent as Form;

			if (form != null)
			{
				// if it will become visible
				if (!form.Visible)
				{
					form.Activate();
					form.Location = point;
				}
				form.Visible = !form.Visible;
			}
		}

		/// <summary>   
		/// A session has been closed, update the current session screen 
		/// </summary>
		private void RemoveSession(string SessionID)
		{
			foreach(Session s in sessionManager)
			{
				if(s.SessionID.ToString() == SessionID)
				{
					cboSessions.Items.Remove(s);
				}
			}

			// No more session clear all labels
			if ( cboSessions.Items.Count == 0 )
			{
				cboSessions.Enabled = false;
				ClearLabels();
			}
		}
	
		/// <summary>
		/// Clear all labels
		/// </summary>
		private void ClearLabels()
		{
			lblCust.Text = "";
			lblFirstName.Text = "";
			lblLastName.Text = "";
			lblPhoneNum.Text = "";
			lblCurrApp.Text = "";
			cboSessions.Text = "";
		}

		/// <summary>
		/// This method loads/reloads the sessions combo with session nodes availale
		///	in the passed in session node list object.
		/// </summary>
		private void LoadAllSessions() 
		{
			//Clear the combobox
			cboSessions.Items.Clear();

			foreach ( Session session in sessionManager )
			{
				if ( session.Name != null )
				{
					int i = cboSessions.Items.Add( session );

					if ( session == sessionManager.ActiveSession )
						cboSessions.SelectedIndex = i;
				}
			}

			if ( cboSessions.Items.Count > 0 )
				cboSessions.Enabled = true;
			else
				cboSessions.Enabled = false;
		}

		/// <summary>
		/// This event is handled so as to capture a session change from
		///	Current Session Control UI and raise an event to AgentDesktop. 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cboSessions_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Session session = cboSessions.SelectedItem as Session;

			if ( session != null && sessionManager != null )
				sessionManager.SetActiveSession( session.SessionID );
		}

		private void CurrentSessionUI_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if ((this.Width < 2) || (this.Height < 2))
				return;

			Rectangle rect = new Rectangle( 0, 0, this.Width - 1, this.Height- 1 );
			
			using ( LinearGradientBrush lgBrush = new LinearGradientBrush(rect, System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF), System.Drawing.Color.FromArgb(165,207,250), LinearGradientMode.Horizontal) )
			{
				e.Graphics.FillRectangle( lgBrush, rect );
			}
		}
	}
}
