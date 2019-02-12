//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// Workflow user control for the agent desktop
//
//===================================================================================

#region Usings
using System;
using System.ServiceModel;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Samples.HostedControlInterfaces;
#endregion

namespace Microsoft.Ccf.Samples.WorkFlowControl
{
	/// <summary>
	/// Summary description for WorkFlowControl.
	/// </summary>
	public class WorkFlowControl : HostedControl, IWorkFlow
	{

		#region Variables
		// Constant values
		private const string XML_WORKFLOW = "Workflow";
		private const string XML_WORKFLOW_ID = "ID";
		// TODO - can we just use the built in one???
		protected ConfigurationValueReader configurationReader = GeneralFunctions.ConfigurationReader(null);

		#region Privates
		private int nGreyCheckImageIndex = 0;
		private int nGreenCheckImageIndex = 1;
		private int nGreenArrowImageIndex = 2;
		private int nIntervalMins = 4;
		// The index of the list view selected index.
		private int nLastIndex = -1;
		// Index of the intelligent workflow in the array of the workflows, AllWorkflows.
		private int nIntelligentWorkflowId = -1;
		// The index of the current workflow that is active, either pending or one of the available workflows chosen by the user.
		private int nActiveWorkflowId = -1;

		private bool bWorkflowObtainedNow;
		/// <summary>
		/// Used to reduce the number of workflow updates made.
		/// </summary>
		private bool workedBefore = false;
		private bool previousShow = true;
		// Array of the data of all the available workflows. an array of WorkflowData structures.
		private ArrayList AllWorkflows;
		private DateTime dtWorkflowsObtainedTime;
		// The current workflow that is active, either pending or one of the available workflows chosen by the user.
		private WorkflowData ActiveWorkflow;
		// If WorkflowStatus is 1, a workflow is made active. If the WorkflowStatus is 0, a workflow is complete. If the WorkflowStatus is 2, a workflow is canceled.
		// workflow web service
		private WorkflowWS.WorkflowWsClient WsWorkflow;
		private Sessions sessionManager;
		// Workflow data structures
		// Member variables
		// Agent and the session data.
		private AgentAndSessionData CurrentAgentSession;

		private System.Windows.Forms.Panel plHeadingPanel;
		private System.Windows.Forms.Label lbCurrentWorkflow;
		private System.Windows.Forms.ComboBox comboAvailableWorkflows;
		private System.Windows.Forms.Label lbAvlWorkflows;
		private System.Windows.Forms.PictureBox picHelpIcon;
		private System.Windows.Forms.Label helpContent;
		private System.Windows.Forms.Label Help;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ListView lvSteps;
		private System.Windows.Forms.Label Separator;
		private System.Windows.Forms.Label Cancel;
		private System.Windows.Forms.Label Next;
		private System.Windows.Forms.Label Previous;
		private System.Windows.Forms.Label WorkflowSteps;
		private System.Windows.Forms.Label StartDone;
		private System.Windows.Forms.ColumnHeader dummyCol;
		private System.Windows.Forms.ImageList workflowListViewImages;
		private System.Windows.Forms.ImageList helpArrowImages;
		private System.Windows.Forms.ToolTip workflowNameToolTip;
		private System.Drawing.Color BlueStartColor = System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF);
		private System.Drawing.Color BlueEndColor = System.Drawing.Color.FromArgb(165, 207, 250);
		#endregion

		#region Variables for three functions below that were refactored
		//Xml constants
		private const string STEP_COMPLETE_VAL = "1";
		private const string STEP_NOT_COMPLETE_VAL = "0";
		private const string XML_XML = "XML";
		private const string XML_SESSION = "Session";
		private const string XML_SESSION_ID = "ID";
		private const string XML_WORKFLOW_STATUS = "Status";
		private const string XML_STEP = "Step";
		private const string XML_STEP_ID = "ID";
		private const string XML_STEP_COMPLETE = "Complete";
		// Workflow ID
		private int nWorkflowId;
		// Array of the workflowstep structure.
		private ArrayList alSteps;
		// The number of the step that is active, default is -1.
		private int nActiveStepId;
		#endregion

		#endregion

		#region Events
		/// <summary>
		/// This event will be raised when a hosted application has to be brought to focus.
		/// </summary>
		public event WorkFlowEventHandler FocusHostedApp;
		/// <summary>
		/// This event will be raised when a workflow becomes active and when a workflow is completed/canceled.
		/// </summary>
		public event WorkFlowEventHandler WorkflowStatusChange;

		// Workflow Driven Implementation:
		/// <summary>
		/// Event which can properly notify the desktop that
		/// a workflow is started.
		/// </summary>
		public event WorkFlowEventHandler WorkflowStarted;
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public WorkFlowControl()
		{
			this.init();
		}

		#region IWorkflow and IHostedApplication3 Members

		public WorkFlowControl(int appID, string appName, string initString)
			:
		base(appID, appName, initString)
		{
			this.init();
		}

		/// <summary>
		/// Returns a boolean value indicating whether workflow has started or not
		/// </summary>
		public bool WorkflowNotStarted
		{
			get
			{
				// AcitiveSession.Workflow == string.Empty means workflow has not started
				return (sessionManager.ActiveSession.Workflow == string.Empty);
			}
		}

		/// <summary>
		/// Gets or Set value indicating whether the control can respond to user interaction.
		/// </summary>
		public bool ControlEnabled
		{
			get { return this.Enabled; }
			set { this.Enabled = value; }
		}

		/// <summary>
		/// Gets or Sets value indicating whether the control is displayed.
		/// </summary>
		public bool ControlVisible
		{
			get { return this.Visible; }
			set { this.Visible = value; }
		}

		/// <summary>
		/// get or set the parent container of the control
		/// </summary>
		public Control ControlParent
		{
			get { return this.Parent; }
			set { this.Parent = value; }
		}

		/// <summary>
		/// The manager for customer sessions.
		/// </summary>
		public override object SessionManager
		{
			set { sessionManager = (Sessions)value; }
		}

		/// <summary>
		/// Don't list application in SessionExplorer
		/// </summary>
		public override bool IsListed
		{
			get { return false; }
		}

		/// <summary>
		/// Returns a boolean value indicating whether workflow is forced or not
		/// </summary>
		public bool IsForced
		{
			get { return ActiveWorkflow.IsForced; }
		}

		/// <summary>
		/// Returns the current workflow that is active.
		/// </summary>
		public WorkflowData CurrentWorkflow
		{
			get
			{
				return this.ActiveWorkflow;
			}
		}

		#endregion

		private void init()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// This only appears if the sharing a CCFPanel with some other app
			// or control, then this is the tab display text.
			Text = localizeWF.WORKFLOW_MODULE_NAME;

			this.lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW;
			this.lbAvlWorkflows.Text = localizeWF.WORKFLOW_LBL_AVL_WORKFLOWS;
			this.Previous.Text = localizeWF.WORKFLOW_LBL_PREVIOUS;
			this.helpContent.Text = localizeWF.WORKFLOW_LBL_HELP_CONTENT;
			this.Help.Text = localizeWF.WORKFLOW_LBL_HELP;
			this.Cancel.Text = localizeWF.WORKFLOW_LBL_CANCEL;
			this.Previous.Text = localizeWF.WORKFLOW_LBL_PREVIOUS;
			this.Next.Text = localizeWF.WORKFLOW_LBL_NEXT;
			this.WorkflowSteps.Text = localizeWF.WORKFLOW_LBL_WORKFLOW_STEPS;
			this.StartDone.Text = localizeWF.WORKFLOW_BTN_START;
			this.comboAvailableWorkflows.Text = localizeWF.WORKFLOW_LBL_DEFAULT;

			Reset();

			showHelp(false);
		}

		/// <summary>
		/// To initialize values
		/// </summary>
		private void Reset()
		{
			// initialization
			CurrentAgentSession = new AgentAndSessionData();
			AllWorkflows = null;
			nIntelligentWorkflowId = -1;
			nActiveWorkflowId = -1;
			ActiveWorkflow = null;

			comboAvailableWorkflows.Enabled = false;
			Previous.Enabled = false;
			Next.Enabled = false;
			Cancel.Enabled = false;

			dtWorkflowsObtainedTime = DateTime.Now;
			bWorkflowObtainedNow = false;

			if (!this.DesignMode)
			{
				WsWorkflow = new WorkflowWS.WorkflowWsClient();
				WsWorkflow.Endpoint.Address = new EndpointAddress(this.configurationReader.ReadAppSettings("Microsoft_Samples_Ccf_DemoCode_CcfDemoApps_WorkFlowControl_WorkflowWS_WorkflowWs"));
				WsWorkflow.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();
			}
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WorkFlowControl));
			this.plHeadingPanel = new System.Windows.Forms.Panel();
			this.lbCurrentWorkflow = new System.Windows.Forms.Label();
			this.comboAvailableWorkflows = new System.Windows.Forms.ComboBox();
			this.lbAvlWorkflows = new System.Windows.Forms.Label();
			this.lvSteps = new System.Windows.Forms.ListView();
			this.dummyCol = new System.Windows.Forms.ColumnHeader();
			this.workflowListViewImages = new System.Windows.Forms.ImageList(this.components);
			this.picHelpIcon = new System.Windows.Forms.PictureBox();
			this.helpContent = new System.Windows.Forms.Label();
			this.Help = new System.Windows.Forms.Label();
			this.helpArrowImages = new System.Windows.Forms.ImageList(this.components);
			this.Separator = new System.Windows.Forms.Label();
			this.Cancel = new System.Windows.Forms.Label();
			this.Next = new System.Windows.Forms.Label();
			this.Previous = new System.Windows.Forms.Label();
			this.WorkflowSteps = new System.Windows.Forms.Label();
			this.StartDone = new System.Windows.Forms.Label();
			this.workflowNameToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.plHeadingPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// plHeadingPanel
			// 
			this.plHeadingPanel.Controls.Add(this.lbCurrentWorkflow);
			this.plHeadingPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.plHeadingPanel.Location = new System.Drawing.Point(0, 0);
			this.plHeadingPanel.Name = "plHeadingPanel";
			this.plHeadingPanel.Size = new System.Drawing.Size(272, 25);
			this.plHeadingPanel.TabIndex = 0;
			this.plHeadingPanel.Resize += new System.EventHandler(this.plHeadingPanel_Resize);
			this.plHeadingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.plHeadingPanel_Paint);
			// 
			// lbCurrentWorkflow
			// 
			this.lbCurrentWorkflow.BackColor = System.Drawing.Color.Transparent;
			this.lbCurrentWorkflow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbCurrentWorkflow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbCurrentWorkflow.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lbCurrentWorkflow.Image = ((System.Drawing.Image)(resources.GetObject("lbCurrentWorkflow.Image")));
			this.lbCurrentWorkflow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbCurrentWorkflow.Location = new System.Drawing.Point(0, 0);
			this.lbCurrentWorkflow.Name = "lbCurrentWorkflow";
			this.lbCurrentWorkflow.Size = new System.Drawing.Size(272, 25);
			this.lbCurrentWorkflow.TabIndex = 0;
			this.lbCurrentWorkflow.Text = "Current Workflow";
			this.lbCurrentWorkflow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbCurrentWorkflow.Paint += new System.Windows.Forms.PaintEventHandler(this.lbCurrentWorkflow_Paint);
			// 
			// comboAvailableWorkflows
			// 
			this.comboAvailableWorkflows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAvailableWorkflows.Location = new System.Drawing.Point(80, 32);
			this.comboAvailableWorkflows.Name = "comboAvailableWorkflows";
			this.comboAvailableWorkflows.Size = new System.Drawing.Size(136, 21);
			this.comboAvailableWorkflows.TabIndex = 2;
			this.workflowNameToolTip.SetToolTip(this.comboAvailableWorkflows, "Name of the workflow selected in workflows combo");
			this.comboAvailableWorkflows.SelectedIndexChanged += new System.EventHandler(this.comboAvailableWorkflows_SelectedIndexChanged);
			// 
			// lbAvlWorkflows
			// 
			this.lbAvlWorkflows.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbAvlWorkflows.ForeColor = System.Drawing.Color.Black;
			this.lbAvlWorkflows.Location = new System.Drawing.Point(8, 32);
			this.lbAvlWorkflows.Name = "lbAvlWorkflows";
			this.lbAvlWorkflows.Size = new System.Drawing.Size(64, 23);
			this.lbAvlWorkflows.TabIndex = 1;
			this.lbAvlWorkflows.Text = "Workflows";
			this.lbAvlWorkflows.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lvSteps
			// 
			this.lvSteps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.dummyCol});
			this.lvSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lvSteps.ForeColor = System.Drawing.Color.Black;
			this.lvSteps.FullRowSelect = true;
			this.lvSteps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvSteps.Location = new System.Drawing.Point(8, 80);
			this.lvSteps.MultiSelect = false;
			this.lvSteps.Name = "lvSteps";
			this.lvSteps.Size = new System.Drawing.Size(240, 112);
			this.lvSteps.SmallImageList = this.workflowListViewImages;
			this.lvSteps.TabIndex = 5;
			this.lvSteps.View = System.Windows.Forms.View.Details;
			this.lvSteps.Click += new System.EventHandler(this.lvSteps_Click);
			this.lvSteps.SelectedIndexChanged += new System.EventHandler(this.lvSteps_SelectedIndexChanged);
			// 
			// dummyCol
			// 
			this.dummyCol.Width = 230;
			// 
			// workflowListViewImages
			// 
			this.workflowListViewImages.ImageSize = new System.Drawing.Size(16, 16);
			this.workflowListViewImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("workflowListViewImages.ImageStream")));
			this.workflowListViewImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// picHelpIcon
			// 
			this.picHelpIcon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picHelpIcon.Image = ((System.Drawing.Image)(resources.GetObject("picHelpIcon.Image")));
			this.picHelpIcon.Location = new System.Drawing.Point(16, 240);
			this.picHelpIcon.Name = "picHelpIcon";
			this.picHelpIcon.Size = new System.Drawing.Size(16, 16);
			this.picHelpIcon.TabIndex = 11;
			this.picHelpIcon.TabStop = false;
			this.picHelpIcon.Click += new System.EventHandler(this.Help_Click);
			// 
			// helpContent
			// 
			this.helpContent.ForeColor = System.Drawing.Color.RoyalBlue;
			this.helpContent.Location = new System.Drawing.Point(16, 264);
			this.helpContent.Name = "helpContent";
			this.helpContent.Size = new System.Drawing.Size(246, 40);
			this.helpContent.TabIndex = 10;
			this.helpContent.Text = "Help Contents";
			this.helpContent.Visible = false;
			// 
			// Help
			// 
			this.Help.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Help.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Help.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Help.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Help.ImageIndex = 1;
			this.Help.ImageList = this.helpArrowImages;
			this.Help.Location = new System.Drawing.Point(40, 240);
			this.Help.Name = "Help";
			this.Help.Size = new System.Drawing.Size(48, 16);
			this.Help.TabIndex = 9;
			this.Help.Text = "Help";
			this.Help.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Help.Click += new System.EventHandler(this.Help_Click);
			// 
			// helpArrowImages
			// 
			this.helpArrowImages.ImageSize = new System.Drawing.Size(16, 16);
			this.helpArrowImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("helpArrowImages.ImageStream")));
			this.helpArrowImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Separator
			// 
			this.Separator.BackColor = System.Drawing.Color.Lavender;
			this.Separator.ForeColor = System.Drawing.Color.Lavender;
			this.Separator.Location = new System.Drawing.Point(8, 232);
			this.Separator.Name = "Separator";
			this.Separator.Size = new System.Drawing.Size(256, 3);
			this.Separator.TabIndex = 8;
			this.Separator.Resize += new System.EventHandler(this.WorkFlow_Resize);
			this.Separator.Paint += new System.Windows.Forms.PaintEventHandler(this.Separator_Paint);
			// 
			// Cancel
			// 
			this.Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Cancel.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Cancel.Location = new System.Drawing.Point(208, 200);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(45, 23);
			this.Cancel.TabIndex = 8;
			this.Cancel.Text = "Cancel";
			this.Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// Next
			// 
			this.Next.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Next.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Next.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Next.Image = ((System.Drawing.Image)(resources.GetObject("Next.Image")));
			this.Next.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Next.Location = new System.Drawing.Point(120, 200);
			this.Next.Name = "Next";
			this.Next.Size = new System.Drawing.Size(64, 23);
			this.Next.TabIndex = 7;
			this.Next.Text = "Next";
			this.Next.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Next.Click += new System.EventHandler(this.Next_Click);
			// 
			// Previous
			// 
			this.Previous.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Previous.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Previous.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Previous.Image = ((System.Drawing.Image)(resources.GetObject("Previous.Image")));
			this.Previous.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Previous.Location = new System.Drawing.Point(16, 200);
			this.Previous.Name = "Previous";
			this.Previous.Size = new System.Drawing.Size(80, 23);
			this.Previous.TabIndex = 6;
			this.Previous.Text = "Previous";
			this.Previous.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Previous.Click += new System.EventHandler(this.Previous_Click);
			// 
			// WorkflowSteps
			// 
			this.WorkflowSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.WorkflowSteps.ForeColor = System.Drawing.Color.Black;
			this.WorkflowSteps.Location = new System.Drawing.Point(8, 64);
			this.WorkflowSteps.Name = "WorkflowSteps";
			this.WorkflowSteps.Size = new System.Drawing.Size(96, 16);
			this.WorkflowSteps.TabIndex = 4;
			this.WorkflowSteps.Text = "Workflow Steps";
			this.WorkflowSteps.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StartDone
			// 
			this.StartDone.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartDone.Enabled = false;
			this.StartDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.StartDone.ForeColor = System.Drawing.Color.RoyalBlue;
			this.StartDone.Location = new System.Drawing.Point(224, 32);
			this.StartDone.Name = "StartDone";
			this.StartDone.Size = new System.Drawing.Size(32, 24);
			this.StartDone.TabIndex = 3;
			this.StartDone.Text = "Start";
			this.StartDone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.StartDone.Click += new System.EventHandler(this.StartDone_Click);
			// 
			// WorkFlow
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.StartDone);
			this.Controls.Add(this.WorkflowSteps);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Next);
			this.Controls.Add(this.Previous);
			this.Controls.Add(this.Help);
			this.Controls.Add(this.helpContent);
			this.Controls.Add(this.picHelpIcon);
			this.Controls.Add(this.Separator);
			this.Controls.Add(this.lbAvlWorkflows);
			this.Controls.Add(this.comboAvailableWorkflows);
			this.Controls.Add(this.plHeadingPanel);
			this.Controls.Add(this.lvSteps);
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.Name = "WorkFlow";
			this.Size = new System.Drawing.Size(272, 320);
			this.Resize += new System.EventHandler(this.WorkFlow_Resize);
			this.plHeadingPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Invalidate the client area of panel to repaint itself.
		/// </summary>
		/// <param name="sender">sender of this event, the panel</param>
		/// <param name="e">Null EventArgs</param>
		private void plHeadingPanel_Resize(object sender, System.EventArgs e)
		{
			plHeadingPanel.Invalidate();
		}

		/// <summary>
		/// Paint the panel with blue gradient color.
		/// </summary>
		/// <param name="sender">sender of this event, the panel</param>
		/// <param name="e">Paint EventArgs used for painting</param>
		private void plHeadingPanel_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rectToPaint = new Rectangle(0, 0, plHeadingPanel.Width, plHeadingPanel.Height - 1);
			using (LinearGradientBrush brBrushToPaint = new LinearGradientBrush(rectToPaint, BlueStartColor, BlueEndColor, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(brBrushToPaint, rectToPaint);
			}
		}

		/// <summary>
		/// paint the separator to have the look as in the HLD doc.
		/// </summary>
		/// <param name="sender">the sender, the separator.</param>
		/// <param name="e">paint event arguments.</param>
		private void Separator_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rectToPaint = new Rectangle(0, 0, Separator.Width - 1, Separator.Height - 1);
			using (LinearGradientBrush brBrushToPaint = new LinearGradientBrush(rectToPaint, BlueEndColor, BlueEndColor, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(brBrushToPaint, rectToPaint);
			}
		}

		/// <summary>
		/// Update the combo box with the available workflows.
		/// </summary>
		/// <returns>True for success else false.</returns>
		private bool UpdateCombo()
		{
			try
			{
				// Clear the contents in the combo.
				comboAvailableWorkflows.Items.Clear();

				// Workflow Driven Implementation:
				// Add the item for default selection in combo.
				comboAvailableWorkflows.Items.Add(localizeWF.WORKFLOW_COMBO_DEFAULT_ITEM);

				int nActiveWorkflowIndex = -1;
				int nIndex = -1;
				foreach (WorkflowData workflow in AllWorkflows)
				{
					// add the workflow only if there are steps for it.
					if (workflow.Steps == null || workflow.Steps.Count == 0)
					{
						continue;
					}
					nIndex++; // index of the item added in the next line.
					comboAvailableWorkflows.Items.Add(workflow.WorkflowName);
					// find a method of combo box to store the workflow Id along with the item.
					if (workflow.WorkflowId == nActiveWorkflowId)
					{
						nActiveWorkflowIndex = nIndex + 1; //take into account the default item at index 0 
					}
				}

				// Set the selected index to the intelligent workflow/pending workflow.
				if (nActiveWorkflowIndex == -1)
				{
					// Implies that the pending workflow is not part
					// of the present available workflows. throw an exception in this case.
					nActiveWorkflowIndex = 0;
				}

				if (this.WorkflowNotStarted)
				{
					comboAvailableWorkflows.SelectedIndex = 0;
				}
				else if (comboAvailableWorkflows.Items.Count > 0)
				{
					comboAvailableWorkflows.SelectedIndex = nActiveWorkflowIndex;
				}

				workflowNameToolTip.SetToolTip(this.comboAvailableWorkflows, comboAvailableWorkflows.Text);
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_UPDATING_COMBO, exp);
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Sets the button's text to the appropriate value.
		/// </summary>
		private void SetStartDoneText()
		{
			if (sessionManager.ActiveSession == null ||
				!sessionManager.ActiveSession.IsWorkflowPending)
			{
				StartDone.Text = localizeWF.WORKFLOW_BTN_START;
			}
			else
			{
				StartDone.Text = localizeWF.WORKFLOW_BTN_DONE;
			}
		}

		/// <summary>
		/// Workflow start handler
		/// </summary>
		/// <returns>true if success else false</returns>
		private bool StartHandler()
		{
			StartDone.Text = localizeWF.WORKFLOW_BTN_DONE;
			comboAvailableWorkflows.Enabled = false;
			Cancel.Enabled = true;

			// raise the event to notify that the workflow has started.
			if (WorkflowStatusChange != null)
			{
				WorkflowArgs e = new WorkflowArgs();
				e.SessionId = CurrentAgentSession.SessionId;
				e.WorkflowStatus = (int)WorkflowStatus.Active;
				WorkflowStatusChange(this, e);
			}

			return true;
		}

		/// <summary>
		/// Workflow done handler
		/// </summary>
		/// <returns>true if success else false</returns>
		private bool DoneButtonHandler()
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				// act differently for forced and non-forced.
				if (ActiveWorkflow.IsForced) // forced
				{
					int nNumOfSteps = ActiveWorkflow.Steps.Count;
					WorkflowStep LastStep = new WorkflowStep();
					if (nNumOfSteps > 0)
					{
						LastStep = (WorkflowStep)ActiveWorkflow.Steps[nNumOfSteps - 1];
					}

					// if the workflow is at the last step, the workflow can be completed.
					if (!ActiveWorkflow.AreAllStepsComplete() &&
						ActiveWorkflow.ActiveStepId != LastStep.Workflowstepid)
					{
						// at least one step is not complete.
						MessageBox.Show(localizeWF.WORKFLOW_COMPLETE_STEP, localizeWF.COMMON_MSGBOX_TITLE_PART1 + localizeWF.WORKFLOW_MSGBOX_TITLE_PART2);
						return false;
					}
				}

				// refresh the view and data structures.
				LocalRefreshAfterDoneOrCancel(WorkflowStatus.Complete);
			}
			finally
			{
				Cursor = Cursors.Default;
			}

			return true;
		}

		/// <summary>
		/// click handler of the cancel button.
		/// </summary>
		/// <returns>true if success else false</returns>
		private bool CancelHandler()
		{
			// confirm the cancellation from the user.
			if (MessageBox.Show(this, localizeWF.WORKFLOW_CANCEL_CONFIRM, localizeWF.COMMON_MSGBOX_TITLE_PART1 + localizeWF.WORKFLOW_MSGBOX_TITLE_PART2, MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return false;
			}

			// refresh the view and data structures.
			return LocalRefreshAfterDoneOrCancel(WorkflowStatus.Cancel);
		}

		/// <summary>
		/// Refresh workflow
		/// </summary>
		/// <param name="status">workflow status</param>
		/// <returns>true if success else false</returns>
		private bool LocalRefreshAfterDoneOrCancel(WorkflowStatus status)
		{
			//AUDIT TRAIL
			LogData logData = new LogData(); //Agent ID picked up from the Desktop.
			logData.ActivityID = ActivityID.WorkFlowEnd;
			logData.UniversalCurrentTime = DateTime.UtcNow;
			logData.WorkFlowActiveStepID = ActiveWorkflow.ActiveStepId;
			logData.WorkFlowName = ActiveWorkflow.WorkflowName;
			logData.WorkFlowStatus = status.ToString();
			AuditLog.Log(AuditType.WorkFlow, logData);
			//AUDIT TRAIL END

			try
			{
				Cursor = Cursors.WaitCursor;

				// Form the xml and save it to the DB.
				SaveActiveWorkflow(status, -1);

				// Raise the event to notify the others.
				if (WorkflowStatusChange != null)
				{
					WorkflowArgs e = new WorkflowArgs();
					e.SessionId = CurrentAgentSession.SessionId;
					e.WorkflowStatus = (int)status;
					WorkflowStatusChange(this, e);
				}

				// Set for the next workflow.
				ActiveWorkflow = new WorkflowData();

				// Get the intelligent workflow id.
				nActiveWorkflowId = GetIntelligentWorkflowID();
				if (nActiveWorkflowId != -1)
				{
					WorkflowData wdTemp = GetWorkflowByWorkflowId(nActiveWorkflowId);
					if (wdTemp != null)
					{
						ActiveWorkflow = wdTemp.ReturnACopy();
					}
					else
					{
						nActiveWorkflowId = -1;
					}
				}

				// If the intelligent workflowid is not found.
				if ((nActiveWorkflowId == -1 || ActiveWorkflow == null) && (AllWorkflows != null && AllWorkflows.Count != 0))
				{
					// Set the first workflow as the active workflow.
					nActiveWorkflowId = ((WorkflowData)AllWorkflows[0]).WorkflowId;
					ActiveWorkflow = ((WorkflowData)AllWorkflows[0]).ReturnACopy();
				}

				// enable the combo box, raise the complete event
				StartDone.Text = localizeWF.WORKFLOW_BTN_START;
				comboAvailableWorkflows.Enabled = true;

				// refresh the combo.
				UpdateCombo();

				// update the list view also.
				UpdateListView();
			}
			finally
			{
				Cursor = Cursors.Default;
			}
			return true;
		}

		/// <summary>
		/// Click handler of a list view item.
		/// </summary>
		/// <param name="nClickedStepIndex">Step index</param>
		/// <param name="nClickedStepId">Step Id</param>
		/// <returns>true if success else false</returns>
		private bool ListviewClickHandler(int nClickedStepIndex, int nClickedStepId)
		{
			try
			{
				if (nClickedStepIndex < 0 || nClickedStepIndex > lvSteps.Items.Count)
				{
					return false;
				}

				Cursor = Cursors.WaitCursor;

				// do not check if the same step is clicked.
				// update the view anyways.

				// make the clicked row visible in the list view.
				lvSteps.EnsureVisible(nClickedStepIndex);

				// the workflow is started by clicking on a step.
				if (ActiveWorkflow.ActiveStepId == -1)
				{
					// Workflow Driven Implementation:
					// Notify the desktop that a workflow has been started. So that the 
					// necessary applications will be loaded or made visible.
					if (WorkflowStarted != null)
					{
						WorkflowArgs e = new WorkflowArgs();
						e.WorkflowStatus = (int)WorkflowStatus.Active;
						e.CurrentWorkflow = ActiveWorkflow;
						WorkflowStarted(this, e);
					}

					// Going to start a workflow.
					// Check if the Agent has access to all the steps' hosted applications.
					string strStepsNotAccessible = CheckForAccessToAllSteps(ActiveWorkflow);
					if (strStepsNotAccessible != null && strStepsNotAccessible != string.Empty)
					{
						// user doesn't have access to one or more hosted app ids.
						// display and log the error and return.
						string strMsg = localizeWF.WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART1 + strStepsNotAccessible + localizeWF.WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART2;
						Logging.Warn(Application.ProductName, localizeWF.WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_STEPS + strStepsNotAccessible);

						// ask the user if he wants to proceed.
						if (MessageBox.Show(strMsg, localizeWF.COMMON_MSGBOX_TITLE_PART1 + localizeWF.WORKFLOW_MSGBOX_TITLE_PART2, MessageBoxButtons.YesNo) == DialogResult.No)
						{
							// refresh the view and data structures.
							LocalRefreshAfterDoneOrCancel(WorkflowStatus.Complete);

							return true;
						}
					}

					// Do the initial function when a workflow is started.
					StartHandler();
					//AUDIT TRAIL
					LogData logData = new LogData();
					logData.ActivityID = ActivityID.WorkFlowStart;
					logData.UniversalCurrentTime = DateTime.UtcNow;
					logData.WorkFlowActiveStepID = nClickedStepId;
					logData.WorkFlowName = ActiveWorkflow.WorkflowName;
					AuditLog.Log(AuditType.WorkFlow, logData);
					//AUDIT TRAIL END
				}

				// Set the current active step complete.
				if (ActiveWorkflow.ActiveStepId != -1 && nClickedStepId != ActiveWorkflow.ActiveStepId)
				{
					// Set the step complete.
					ActiveWorkflow.GetStepByStepId(ActiveWorkflow.ActiveStepId).IsStepComplete = true;
					// Setting the green check mark is done in selection change itself.
				}

				if (nClickedStepId != ActiveWorkflow.ActiveStepId)
				{
					// Form the xml and save it to the DB.
					SaveActiveWorkflow(WorkflowStatus.Active, nClickedStepId);
				}

				// set the current active step to nStepClicked
				ActiveWorkflow.ActiveStepId = nClickedStepId;
				lvSteps.Items[nClickedStepIndex].ImageIndex = nGreenArrowImageIndex;

				// Focus the corresponding hosted app.
				if (FocusHostedApp != null)
				{
					WorkflowArgs e = new WorkflowArgs();
					WorkflowStep activeStep = ActiveWorkflow.GetStepByStepId(ActiveWorkflow.ActiveStepId);

					e.ApplicationId = activeStep.HostedApplicationId;
					// Fire an action if the workflow step is activated
					IHostedApplication app = this.sessionManager.ActiveSession.GetApplication(e.ApplicationId);
					if (null != app && null != activeStep.WorkflowStepAction && 0 < activeStep.WorkflowStepAction.Length)
					{
						app.DoAction(activeStep.WorkflowStepAction, ActiveWorkflow.WorkflowName);
					}
					FocusHostedApp(this, e);
				}
				// take care of enabling/disabling of the prev and the next step.
				EnableDisablePrevNextButtons(nClickedStepIndex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}

			return true;
		}

		/// <summary>
		/// update the list view with the active workflow.
		/// </summary>
		/// <returns>true if success else false</returns>	
		private bool UpdateListView()
		{
			try
			{
				// Workflow Driven Implementation
				if (ActiveWorkflow == null)
				{
					lvSteps.Items.Clear();
				}
				if (ActiveWorkflow == null || ActiveWorkflow.Steps == null)
				{
					return false;
				}

				// Add the steps of the active workflow to the list view.
				if (ActiveWorkflow.Steps.Count == 0)
				{
					return false;
				}

				if (comboAvailableWorkflows.SelectedItem.ToString() != localizeWF.WORKFLOW_COMBO_DEFAULT_ITEM)
				{
					AddStepsToListView(ActiveWorkflow);
				}


				// If it is a pending workflow, raise the events.
				if (ActiveWorkflow.ActiveStepId != -1)
				{
					if (WorkflowStatusChange != null)
					{
						WorkflowArgs e = new WorkflowArgs();
						e.SessionId = CurrentAgentSession.SessionId;
						e.WorkflowStatus = (int)WorkflowStatus.Active;
						WorkflowStatusChange(this, e);
					}
				}

				// Take care of enabling/disabling of the prev and the next step.
				EnableDisablePrevNextButtons(nLastIndex);
			}
			catch (Exception exp)
			{
				Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_LISTVIEW_UPDATE, exp);
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Enables or disables Next and Previous buttons
		/// </summary>
		/// <param name="nCurrentIndex">Index</param></param>
		private void EnableDisablePrevNextButtons(int nCurrentIndex)
		{
			// Take care of enabling/disabling of the prev and the next step.
			Previous.Enabled = true;
			Next.Enabled = true;
			Cancel.Enabled = true;

			if (nCurrentIndex == 0)
			{
				Previous.Enabled = false;
			}
			if (nCurrentIndex == lvSteps.Items.Count - 1)
			{
				Next.Enabled = false;
			}
			if (nCurrentIndex == -1)
			{
				Previous.Enabled = false;
				Next.Enabled = false;
				Cancel.Enabled = false;
			}
		}

		/// <summary>
		/// Add the steps to the list view.
		/// </summary>
		/// <param name="AddWorkflow">WorkflowData object to att to list.</param>
		private void AddStepsToListView(WorkflowData AddWorkflow)
		{
			// Delete the items in the list view.
			lvSteps.Items.Clear();

			if (AddWorkflow.Steps.Count == 0)
			{
				return;
			}

			float fColumnWidth = 0;
			Graphics gra = Graphics.FromHwnd(this.Handle);
			SizeF size = new SizeF(0, 0);

			for (int nIndex = 0; nIndex < AddWorkflow.Steps.Count; nIndex++)
			{
				ListViewItem lvItem = new ListViewItem();
				lvItem.Text = AddWorkflow.GetStepByIndex(nIndex).WorkflowStepName;
				lvItem.Tag = AddWorkflow.GetStepByIndex(nIndex).Workflowstepid;

				if (AddWorkflow.GetStepByIndex(nIndex).IsStepComplete)
				{
					lvItem.ImageIndex = nGreenCheckImageIndex;
				}
				else if (AddWorkflow.GetStepByIndex(nIndex).Workflowstepid == AddWorkflow.ActiveStepId)
				{
					lvItem.ImageIndex = nGreenArrowImageIndex;
					nLastIndex = nIndex;
				}
				else
				{
					lvItem.ImageIndex = nGreyCheckImageIndex;
				}
				lvSteps.Items.Add(lvItem);

				size = gra.MeasureString(lvItem.Text, lvSteps.Font);
				if (size.Width > fColumnWidth)
				{
					fColumnWidth = size.Width;
				}
			}

			// If the columns are not there, add it explicitly so as to set the column width.
			if (lvSteps.Columns.Count <= 0)
			{
				this.lvSteps.Columns.AddRange(
					new System.Windows.Forms.ColumnHeader[] { this.dummyCol });
			}
			lvSteps.Columns[0].Width = (int)fColumnWidth + lvSteps.SmallImageList.ImageSize.Width + 10;

			// Make the current active step visible.
			if (nLastIndex != -1)
			{
				lvSteps.EnsureVisible(nLastIndex);
			}
		}

		/// <summary>
		/// Get the workflow data for all the available workflows
		/// </summary>
		/// <returns>True for success else false.</returns>
		private bool GetAvailableWorkflowsNames()
		{
			bool bResult = false;
			try
			{
				// So that work flow names will periodically be re-read from the server.
				// Though not when the change is made, just some time later.
				TimeSpan timediff = new TimeSpan(0, nIntervalMins, 0);
				if (AllWorkflows != null && AllWorkflows.Count != 0 && timediff > (dtWorkflowsObtainedTime - DateTime.Now))
				{
					bWorkflowObtainedNow = false;
					bResult = true;
				}
				else
				{
					// Get the workflows.
					// Make a web method call to get the available workflows.
					string strWorkflowsXML = "";
					strWorkflowsXML = WsWorkflow.GetWorkflowNames(CurrentAgentSession.AgentId);

					if (strWorkflowsXML == string.Empty)
					{
						// No workflows for this agent.
						Logging.Warn(Application.ProductName, localizeWF.WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_WORKFLOWS);
					}
					// Set the workflows into the array list.
					else if (!SetWorkflowDataFromXML(strWorkflowsXML, true))
					{
						Logging.Warn(Application.ProductName, localizeWF.WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_WORKFLOWS);
					}
					else
					{
						// Update the item and the variable saying that new workflows have been obtained.
						bWorkflowObtainedNow = true;
						dtWorkflowsObtainedTime = DateTime.Now;
						bResult = true;
					}
				}

				if (bResult)
				{
					// Set the intelligent workflow id.
					nActiveWorkflowId = GetIntelligentWorkflowID();

					if (nActiveWorkflowId != -1)
					{
						WorkflowData wdTemp = GetWorkflowByWorkflowId(nActiveWorkflowId);
						if (wdTemp != null)
						{
							ActiveWorkflow = wdTemp.ReturnACopy();
						}
						else
						{
							nActiveWorkflowId = -1;
							ActiveWorkflow = new WorkflowData();
						}
					}

					// If the intelligentworkflowid not found.
					if ((nActiveWorkflowId == -1 || ActiveWorkflow == null) &&
						AllWorkflows != null && AllWorkflows.Count != 0)
					{
						// Set the first workflow as the active workflow.
						nActiveWorkflowId = ((WorkflowData)AllWorkflows[0]).WorkflowId;
						ActiveWorkflow = ((WorkflowData)AllWorkflows[0]).ReturnACopy();
					}
				}
			}
			catch (System.Net.WebException wex)
			{
				// Catch here so the general Exception below is not catch it.
				throw wex;
			}
			catch (Exception exp)
			{
				// Check message - if appropriate log it.
				if (exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST) < 0 &&
					exp.Message.IndexOf(localizeWF.COMMON_MSG_IIS_ERROR) < 0)
				{
					Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_GET_WORKFLOWS, exp);
				}
				throw exp;
			}
			return bResult;
		}

		/// <summary>
		/// Read the XML and set the workflow data.
		/// </summary>
		/// <param name="strWorkflowsXML">Workflow Xml</param>
		/// <param name="bShowErr">Flag saying if any errors are to be displayed or not</param>
		/// <returns>Returns true is workflow read from XML.</returns>
		private bool SetWorkflowDataFromXML(string strWorkflowsXML, bool bShowErr)
		{
			bool bReturnVal = false;

			if (strWorkflowsXML != string.Empty)
			{
				try
				{
					AllWorkflows = new ArrayList();

					XmlDocument doc = new XmlDocument();
					doc.LoadXml(strWorkflowsXML);
					if (doc.ChildNodes.Count > 0)
					{
						XmlNode root = doc.DocumentElement;
						XmlNodeList Workflows = root.SelectNodes(XML_WORKFLOW);
						// Check for nodes.
						if (Workflows.Count > 0)
						{
							foreach (XmlNode WorkflowNode in Workflows)
							{
								// Form the workflowdata structure
								WorkflowData Workflow = new WorkflowData();
								Workflow.SetWorkflowData(WorkflowNode.OuterXml);
								// Add the structure to the list.
								AllWorkflows.Add(Workflow);
							}

							bReturnVal = true;
						}
					}
				}
				catch (Exception exp)
				{
					if (bShowErr)
					{
						Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_SET_WORKFLOW_DATA_FROM_XML, exp);
						throw exp;
					}
				}
			}

			return bReturnVal;
		}

		/// <summary>
		/// Gets the intelligent workflow
		/// </summary>
		/// <returns>The workflow ID or -1 for none</returns>
		private int GetIntelligentWorkflowID()
		{
			nIntelligentWorkflowId = -1;

			//try
			//{
			//    // Get the intelligent workflow id now.
			//    // Make a web method call to get the intelligent workflow Id
			//    // TODO: Does this make any sense in a real call center where we have
			//    // an IVR?
			//    //nIntelligentWorkflowId = WsWorkflow.GetIntelligentWorkflow( CurrentAgentSession.SessionId );
			//}
			//catch ( Exception exp)
			//{
			//    if (exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST) < 0 &&
			//        exp.Message.IndexOf(localizeWF.COMMON_MSG_IIS_ERROR) < 0)
			//    {
			//        Logging.Warn(Application.ProductName, localizeWF.WORKFLOW_ERR_GET_INTELLIGENT_WORKFLOW_ID);
			//        // Continue if the intelligent workflow cannot be obtained.
			//        // Continue with the rest of the workflow steps
			//        nIntelligentWorkflowId = -1;
			//    }
			//    else
			//    {
			//        throw exp;
			//    }
			//}

			return nIntelligentWorkflowId;
		}

		/// <summary>
		/// Get the workflow steps data for each of the available workflow.
		/// </summary>
		/// <returns>True for success else false</returns>
		private bool GetWorkflowSteps()
		{
			// Check if the workflows list has changed now.
			if (!bWorkflowObtainedNow)
			{
				return true;
			}

			try
			{
				// Some thing wrong happened, should give an error.
				if (AllWorkflows == null)
				{
					return false;
				}

				// Get the fresh workflow steps.
				foreach (WorkflowData EachWorkflow in AllWorkflows)
				{
					string strWorkflowXML = "";
					// Call the web method to get the steps of the workflow.
					strWorkflowXML = WsWorkflow.GetWorkflowSteps(EachWorkflow.WorkflowId);

					if (strWorkflowXML == "")
					{
						// No steps for this, check for the next workflow.
						continue;
					}

					// Set the XML into the data structure.
					EachWorkflow.SetWorkflowStepsData(strWorkflowXML);

					// Store the index of the intelligentworkflow.
					if (EachWorkflow.WorkflowId == nIntelligentWorkflowId)
					{
						// Check if the workflow has proper steps in it.
						if (EachWorkflow.Steps != null && EachWorkflow.Steps.Count != 0)
						{
							nActiveWorkflowId = nIntelligentWorkflowId;
							ActiveWorkflow = EachWorkflow.ReturnACopy();
						}
					}
				}

				// If the intelligentworkflowid not found.
				if (nActiveWorkflowId == -1)
				{
					// set the first workflow as the active workflow.
					nActiveWorkflowId = ((WorkflowData)AllWorkflows[0]).WorkflowId;
					ActiveWorkflow = ((WorkflowData)AllWorkflows[0]).ReturnACopy();
				}

				SetStartDoneText();
			}
			catch (Exception exp)
			{
				if (exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST) < 0 &&
					exp.Message.IndexOf(localizeWF.COMMON_MSG_IIS_ERROR) < 0)
					Logging.Error(Application.ProductName, localizeWF.WORKFLOW_GET_WORKFLOW_STEPS, exp);
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Set the workflow data for the pending workflow.
		/// </summary>
		/// <returns>True for success else false.</returns>
		private bool GetPendingWorkflow()
		{
			string strPendingWorkflowXML = string.Empty;
			int tempActiveWorkflowId = -1;
			WorkflowData tempActiveWorkflow = null;

			try
			{
				// Call the session manager to get the pending workflow.
				strPendingWorkflowXML = sessionManager.ActiveSession.Workflow;
				if (strPendingWorkflowXML == string.Empty)
				{
					return true;
				}

				// Since the xml content has info only about the complete and active steps only,
				// copy the workflowdata corresponding to the pending workflow id.
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(strPendingWorkflowXML);

				XmlNode root = doc.DocumentElement;
				if (root.ChildNodes.Count == 0)
				{
					// No pending workflow.
					return true;
				}
				// Workflow Driven Implementation
				bool bPendingWorkflowIdFound = false;

				XmlNode WorkflowNode = root.SelectSingleNode(XML_WORKFLOW);

				// Find the workflow structure pertaining to this workflow id.
				if (AllWorkflows != null && AllWorkflows.Count != 0)
				{
					foreach (WorkflowData Workflow in AllWorkflows)
					{
						if (Workflow.WorkflowId == Convert.ToInt64(WorkflowNode.Attributes[XML_WORKFLOW_ID].InnerText, 10))
						{
							if (WorkflowNode.ChildNodes.Count == 0)
							{
								break;
							}
							bPendingWorkflowIdFound = true;
							tempActiveWorkflowId = Workflow.WorkflowId;
							tempActiveWorkflow = Workflow.ReturnACopy();
							break;
						}
					}
				}

				if (!bPendingWorkflowIdFound)
				{
					// Pending workflow id is not part of the workflows available to the agent.
					// Do not throw the exception.
					// Display an error and allow the user to work with the available workflows.
					Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_PENDING_WORKFLOW_ID);
					return true;
				}

				// Set the step details of the pending workflow.
				if ( !tempActiveWorkflow.SetPendingWorkflowData(WorkflowNode.OuterXml))
				{
					// Problem with the pending workflow.
					return true;
				}

				// If the pending workflow is all set, set it to the actual variables.
				ActiveWorkflow = tempActiveWorkflow.ReturnACopy();
				nActiveWorkflowId = tempActiveWorkflowId;

				StartDone.Text = localizeWF.WORKFLOW_BTN_DONE;
				comboAvailableWorkflows.Enabled = false;

				// Workflow Driven Implementation:
				// Check if the workflow has been recoved from pending state as part of saved session 
				// restoration. If it is, then we shoudl notify desktop so that the required apps are
				// loaded and made visible. Otherwise, if it is just session change then the the required 
				// applications might have already loaded.
				XmlNode restoredWrkflNode = root.SelectSingleNode("restoredWorkflow");
				if (restoredWrkflNode != null && WorkflowStarted != null)
				{
					WorkflowArgs e = new WorkflowArgs();
					e.ApplicationId = ActiveWorkflow.GetStepByStepId(ActiveWorkflow.ActiveStepId).HostedApplicationId;
					e.WorkflowStatus = (int)WorkflowStatus.Restored;
					e.CurrentWorkflow = ActiveWorkflow;
					WorkflowStarted(this, e);

					// Remove the restoredWorkflow tag so next time around it just treats
					// it as a session change.
					root.RemoveChild(restoredWrkflNode);
					sessionManager.ActiveSession.Workflow = root.OuterXml;
				}
			}
			catch (Exception exp)
			{
				if (exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST) < 0 &&
					exp.Message.IndexOf(localizeWF.COMMON_MSG_IIS_ERROR) < 0)
				{
					Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_GET_PENDING_WORKFLOW, exp);
					return false;
				}
				else
				{
					throw exp;
				}
			}

			return true;
		}

		/// <summary>
		/// Get the pending worklfow for a given session id.
		/// </summary>
		/// <param name="agentId">Current's agent's Id.</param>
		/// <returns>True if a valid pending worklfow is present, 
		/// else return false even in the case of errrors</returns>
		public bool IsWorkflowPending(int agentId)
		{
			bool bResult = false;
			string strPendingWorkflowXML = string.Empty;

			try
			{
				Cursor = Cursors.WaitCursor;

				WorkflowData tempWorkflow = new WorkflowData();

				// call the SessionManager to get the pending workflow.
				strPendingWorkflowXML = sessionManager.ActiveSession.Workflow;
				if (strPendingWorkflowXML == string.Empty)
				{
					// No pending workflow
					return false;
				}

				// Since the xml content has info only about the complete and active steps only,
				// copy the workflowdata corresponding to the pending workflow id.
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(strPendingWorkflowXML);
				XmlNode root = doc.DocumentElement;
				// No valid pending workflow.
				if (root.ChildNodes.Count == 0)
				{
					return false;
				}

				// Check if you have all the available workflows for this agent.
				if (AllWorkflows == null || AllWorkflows.Count == 0)
				{
					// Get the list of available worklfows.
					// Get the workflows.
					// Make a web method call to get the available worklfows.
					string strWorkflowsXML = string.Empty;
					strWorkflowsXML = WsWorkflow.GetWorkflowNames(agentId);

					if (strWorkflowsXML == string.Empty)
					{
						// No workflows for this agent.
						return false;
					}

					// Set the workflows into the array list.
					if (!SetWorkflowDataFromXML(strWorkflowsXML, false))
					{
						return false;
					}
				}

				XmlNode WorkflowNode = root.SelectSingleNode(XML_WORKFLOW);
				if (WorkflowNode.ChildNodes.Count == 0)
				{
					// Not a valid pending workflow
					return false;
				}

				// Find the workflow structure pertaining to this workflow id.
				if (AllWorkflows != null && AllWorkflows.Count != 0)
				{
					foreach (WorkflowData Workflow in AllWorkflows)
					{
						if (Workflow.WorkflowId == Convert.ToInt64(WorkflowNode.Attributes[XML_WORKFLOW_ID].InnerText, 10))
						{
							bResult = true;
							tempWorkflow = Workflow.ReturnACopy();
							break;
						}
					}
				}

				if (!bResult)
				{
					// Pending workflow id is not part of the workflows available to the agent.
					// gave error.
					return false;
				}

				// Set the step details of the pending workflow.
				if ( !tempWorkflow.SetPendingWorkflowData(WorkflowNode.OuterXml, false))
				{
					// Problem with the pending workflow.
					return false;
				}
			}
			catch
			{
				// Do not display any errors or give the errors back to the caller.
				return false;
			}
			finally
			{
				Cursor = Cursors.Default;
			}

			return true;
		}

		/// <summary>
		/// Save the current active workflow as the pending workflow for this session.
		/// </summary>
		/// <param name="status">Workflow status</param>
		/// <param name="nActiveStepId">Active step Id</param>
		/// <returns>True if there is no exception.</returns>
		private bool SaveActiveWorkflow(WorkflowStatus status, int nActiveStepId)
		{
			try
			{
				// Get the workflow in the form of XML.
				string strWorkflowXML = ActiveWorkflow.WorkflowDataInXML(
					CurrentAgentSession.SessionId,
					(int)status,
					nActiveStepId );
				// Save workflow to session
				if (status == WorkflowStatus.Active)
				{
					sessionManager.ActiveSession.Workflow = strWorkflowXML;
				}
				else
				{
					sessionManager.ActiveSession.Workflow = String.Empty;
				}
			}
			catch (Exception exp)
			{
				if (exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST) < 0 &&
					exp.Message.IndexOf(localizeWF.COMMON_MSG_IIS_ERROR) < 0)
				{
					Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_SAVE_ACTIVE_WORKFLOW, exp);
				}
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Gets the workflow by workflow id
		/// </summary>
		/// <param name="nWorkflowId">workflow id</param>
		/// <returns>Workflow Data object.</returns>
		private WorkflowData GetWorkflowByWorkflowId(int nWorkflowId)
		{
			// Go through the AllWorkflows arraylist and give the workflow pertaining to the nWorkflowId
			foreach (WorkflowData Workflow in AllWorkflows)
			{
				if (Workflow.WorkflowId == nWorkflowId)
				{
					return Workflow;
				}
			}
			return null;
		}

		/// <summary>
		/// Call this function to refresh the workflow view.
		/// </summary>
		/// <param name="agentId">Agent ID</param>
		/// <param name="sessionID">Session ID</param>
		/// <returns>True for success else false</returns>
		public bool WorkflowUpdate(int agentId, Guid sessionID)
		{
			// check if session Id and the AgentId are valid.
			if (agentId == 0 || sessionID == Guid.Empty)
			{
				throw new ArgumentNullException("agentId or sessionID", localizeWF.WORKFLOW_ERR_AGENTID_SESSIONID_NULL);
			}

			try
			{
				Cursor = Cursors.WaitCursor;

				// Check if the session ID is the same, no refresh required.
				if (workedBefore && sessionID == CurrentAgentSession.SessionId)
				{
					return true;
				}

				// Clear all the UI and then start with actual work
				workedBefore = false;
				Clear();

				// Update the UI elements.
				comboAvailableWorkflows.Enabled = true;
				StartDone.Enabled = true;

				// Clear the items for the previous session.
				nIntelligentWorkflowId = -1;
				nActiveWorkflowId = -1;

				// Save the new agent id and the session id.
				CurrentAgentSession.AgentId = agentId;
				CurrentAgentSession.SessionId = sessionID;

				// Get the available workflow names.
				GetAvailableWorkflowsNames();

				// Get the workflow steps for each workflow.
				GetWorkflowSteps();

				// Get the pending workflow.
				if (!GetPendingWorkflow())
				{
					UpdateUIElementsState(false);
					return false;
				}

				// Add the workflow names to the combo.
				// Set the selection and also raises the event to update the list view.
				if (!UpdateCombo())
				{
					UpdateUIElementsState(false);
					return false;
				}

				// Update the list view.
				if (!UpdateListView())
				{
					UpdateUIElementsState(true);
					return true;
				}

				UpdateUIElementsState(true);

				if ((comboAvailableWorkflows.SelectedItem.ToString() == localizeWF.WORKFLOW_COMBO_DEFAULT_ITEM)) //|| (comboAvailableWorkflows.SelectedText==""))
				{
					StartDone.Enabled = false;
				}
				else
				{
					StartDone.Enabled = true;
				}

				workedBefore = true;

				return true;
			}
			finally
			{
				// Make the cursor normal.
				Cursor = Cursors.Default;
			}
		}

		private void UpdateUIElementsState(bool bEnable)
		{
			// Reset the UI elements.
			if (bEnable == false)
			{
				comboAvailableWorkflows.Enabled = bEnable;
			}

			StartDone.Enabled = (ActiveWorkflow == null || ActiveWorkflow.Steps == null) ? false : bEnable;

			SetStartDoneText();
			workflowNameToolTip.SetToolTip(this.comboAvailableWorkflows, comboAvailableWorkflows.Text);
		}


		/// <summary>
		/// Triggers when listview steps are clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lvSteps_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			int nSelIndex = -1;
			ListView lvw = (ListView)sender;

			if (lvw.SelectedIndices.Count != 0)
			{
				nSelIndex = lvw.SelectedIndices[0];
				if (nSelIndex != nLastIndex)
				{
					if (ActiveWorkflow.IsForced)
					{
						if (Math.Abs(nSelIndex - nLastIndex) > 1)
						{
							if (nLastIndex != -1)
							{
								lvw.Items[nLastIndex].Selected = true;
								lvw.Items[nLastIndex].Focused = true;
							}
							else
							{
								lvw.SelectedItems[0].Focused = false;
								lvw.SelectedItems[0].Selected = false;
							}
						}
						else
						{
							if (nLastIndex != -1)
								lvw.Items[nLastIndex].ImageIndex = nGreenCheckImageIndex;
							nLastIndex = nSelIndex;
							CommonStepHandler();
						}
					}
					else
					{
						if (nLastIndex != -1)
						{
							lvw.Items[nLastIndex].ImageIndex = nGreenCheckImageIndex;
						}
						nLastIndex = nSelIndex;
						CommonStepHandler();
					}
				}
			}
		}

		/// <summary>
		/// Triggers when workflow item is clicked.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lvSteps_Click(object sender, EventArgs e)
		{
			// Changed in v1.02 from being the MouseUp event since that event
			// can recurse if a modalless wait dialog is being shown.
			if (nLastIndex != -1)
			{
				CommonStepHandler();
			}
		}

		/// <summary>
		/// Common handler when a step is selected, either by mouse or through keyboard.
		/// </summary>
		private void CommonStepHandler()
		{
			lvSteps.Items[nLastIndex].Selected = true;
			lvSteps.Items[nLastIndex].Focused = true;
			lvSteps.Items[nLastIndex].ImageIndex = nGreenArrowImageIndex;

			ListviewClickHandler(nLastIndex, (int)lvSteps.Items[nLastIndex].Tag);
		}

		/// <summary>
		/// Triggers when workflow combo box selected index changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboAvailableWorkflows_SelectedIndexChanged(object sender, System.EventArgs e)
		{

			if (!comboAvailableWorkflows.Enabled)
			{
				// Update the label to specify if the workflow is forced or non-forced.
				if (ActiveWorkflow != null)
				{
					if (ActiveWorkflow.IsForced)
					{
						lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW + " " + localizeWF.WORKFLOW_LBL_FORCED;
					}
					else
					{
						lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW + " " + localizeWF.WORKFLOW_LBL_NON_FORCED;
					}
				}
				// Modified Workflow Implementation
				else
				{
					lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW;
				}
				return;
			}

			// Gget the workflow id of the selected item in the combo box.
			int nIndex = -1;
			for (int i = 0; i < AllWorkflows.Count; i++)
			{
				if (comboAvailableWorkflows.Items[comboAvailableWorkflows.SelectedIndex].ToString() == ((WorkflowData)AllWorkflows[i]).WorkflowName)
				{
					nIndex = i;
					break;
				}
			}

			// Workflow Driven Implementation:
			// If the selected item is default option ("Select...") instead of some particular
			// workflow, then invalidate the active workflow id and data object.
			if (nIndex == -1)
			{
				nActiveWorkflowId = -1;
				ActiveWorkflow = null;
				lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_DEFAULT;
				StartDone.Enabled = false;
			}
			else
			{
				// set the active workflow.
				nActiveWorkflowId = ((WorkflowData)AllWorkflows[nIndex]).WorkflowId;
				ActiveWorkflow = ((WorkflowData)AllWorkflows[nIndex]).ReturnACopy();

				// update the label to specify if the workflow is forced or non-forced.
				if (ActiveWorkflow.IsForced)
				{
					lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW + " " + localizeWF.WORKFLOW_LBL_FORCED;
				}
				else
				{
					lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW + " " + localizeWF.WORKFLOW_LBL_NON_FORCED;
				}

				StartDone.Enabled = true;
			}
			// reset the selection in the list view.
			nLastIndex = -1;
			// update the list view.
			UpdateListView();
			EnableDisablePrevNextButtons(nLastIndex);
			workflowNameToolTip.SetToolTip(this.comboAvailableWorkflows, comboAvailableWorkflows.Text);
		}

		/// <summary>
		/// Check if the agent has access to all the steps of the workflow
		/// </summary>
		/// <param name="SelectedWorkflow">The workflow whose steps are to checked for.</param>
		/// <returns>The string having the steps' names to which the agent doesn't have access to.</returns>
		private string CheckForAccessToAllSteps(WorkflowData SelectedWorkflow)
		{
			string missingSteps = String.Empty;

			foreach (WorkflowStep step in SelectedWorkflow.Steps)
			{
				// If the agent does not have access to any of the hosted applications
				// or if its missing some of the steps,
				// return the list of the steps it doesn't have access to.
				if (!sessionManager.ActiveSession.ApplicationExists(step.HostedApplicationId))
				{
					// Hosted app is not found.
					if (missingSteps.Length != 0)
					{
						missingSteps += "\n";
					}
					missingSteps += "    " + step.WorkflowStepName;
				}
			}

			return missingSteps;
		}

		/// <summary>
		/// Handler when the previous button is clicked. 
		/// set the images accordingly.
		/// </summary>
		/// <param name="sender">the previous button</param>
		/// <param name="e">system event arguments.</param>
		private void Previous_Click(object sender, System.EventArgs e)
		{
			if (nLastIndex >= 0 && nLastIndex < lvSteps.Items.Count
				&& nLastIndex - 1 >= 0 && nLastIndex - 1 < lvSteps.Items.Count)
			{
				lvSteps.Items[nLastIndex].ImageIndex = nGreenCheckImageIndex;
				lvSteps.Items[nLastIndex - 1].Selected = true;
				// Once the above statement is exceuted, the nLastIndex is changed and hence
				// do not change the following nLastIndex to nLastIndex+1.
				lvSteps.Items[nLastIndex].Focused = true;
			}
		}

		/// <summary>
		/// Handler when the next button is clicked. 
		/// set the images accordingly.
		/// </summary>
		/// <param name="sender">The next button</param>
		/// <param name="e">System event arguments.</param>
		private void Next_Click(object sender, System.EventArgs e)
		{
			if (nLastIndex >= 0 && nLastIndex < lvSteps.Items.Count
				&& nLastIndex + 1 >= 0 && nLastIndex + 1 < lvSteps.Items.Count)
			{
				lvSteps.Items[nLastIndex].ImageIndex = nGreenCheckImageIndex;
				lvSteps.Items[nLastIndex + 1].Selected = true;
				// Once the above statement is executed, the nLastIndex is changed and hence
				// do not change the following nLastIndex to nLastIndex+1.
				lvSteps.Items[nLastIndex].Focused = true;
			}
		}

		/// <summary>
		/// Handler when the cancel button is clicked.
		/// </summary>
		/// <param name="sender">The cancel button</param>
		/// <param name="e">System event arguments.</param>
		private void Cancel_Click(object sender, System.EventArgs e)
		{
			CancelHandler();
		}

		/// <summary>
		/// This function is called from the desktop to clear the contents of the workflow.
		/// </summary>
		public void Clear()
		{
			lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW;

			lvSteps.Clear();
			comboAvailableWorkflows.Items.Clear();
			// Workflow Driven Implementation:
			// Add the default option and select the same
			comboAvailableWorkflows.Items.Add(localizeWF.WORKFLOW_COMBO_DEFAULT_ITEM);
			comboAvailableWorkflows.SelectedIndex = 0;

			StartDone.Enabled = false;
			Previous.Enabled = false;
			Next.Enabled = false;
			Cancel.Enabled = false;
			comboAvailableWorkflows.Enabled = false;

			// Clean up the data structures for the workflow to be reset.
			ActiveWorkflow = new WorkflowData();
			nActiveWorkflowId = -1;
			nIntelligentWorkflowId = -1;

			CurrentAgentSession = new AgentAndSessionData();
			nLastIndex = -1;
		}

		/// <summary>
		/// Show/Hide the help content and also update its arrow.
		/// </summary>
		/// <param name="sender">The help icon or help label</param>
		/// <param name="e">System event arguments.</param>
		private void Help_Click(object sender, System.EventArgs e)
		{
			showHelp(!helpContent.Visible);
			Help.ImageIndex = (Help.ImageIndex == 0) ? 1 : 0;
		}

		/// <summary>
		/// When the help text is hidden, allow the session tree to expand to fill its
		/// space.
		/// </summary>
		/// <param name="show">True to show help text, false to hide it</param>
		private void showHelp(bool show)
		{
			if (show == previousShow)
			{
				return;
			}

			helpContent.Visible = previousShow = show;
			if (show)
			{
				this.Help.Top = helpContent.Top - Help.Height - 12;
			}
			else
			{
				this.Help.Top = this.helpContent.Top + 16;
			}

			this.picHelpIcon.Top = Help.Top;
			this.Separator.Top = Help.Top - Separator.Height - 8;

			this.Previous.Top = Separator.Top - Previous.Height - 8;
			this.Next.Top = this.Cancel.Top = this.Previous.Top;

			if (show)
			{
				this.lvSteps.Height -= helpContent.Height;
			}
			else
			{
				this.lvSteps.Height += helpContent.Height;
			}
		}

		/// <summary>
		/// Reflow the locations and sizes of some controls
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WorkFlow_Resize(object sender, System.EventArgs e)
		{
			showHelp(helpContent.Visible);
		}

		/// <summary>
		/// paint the label displaying the "current workflow" as per the preset color.
		/// </summary>
		/// <param name="sender">The currentworkflow label</param>
		/// <param name="e">Paint event arguments.</param>
		private void lbCurrentWorkflow_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Rectangle rect = new Rectangle(0, 0, lbCurrentWorkflow.Width - 1, lbCurrentWorkflow.Height - 1);
			using (LinearGradientBrush lgBrush = new LinearGradientBrush(rect, System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF), System.Drawing.Color.FromArgb(165, 207, 250), LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(lgBrush, rect);
			}

			// Create point for upper-left corner of drawing.
			float x = 5.0F;
			float y = 3.0F;
			e.Graphics.DrawImage(lbCurrentWorkflow.Image, x, y, 16, 16);

			// Draw title of workflow handler
			using (SolidBrush drawBrush = new SolidBrush(Color.RoyalBlue))
			{
				// Create point for upper-left corner of drawing.
				x = 25.0F;
				y = 4.0F;
				// Draw string to screen.
				e.Graphics.DrawString(lbCurrentWorkflow.Text, lbCurrentWorkflow.Font, drawBrush, x, y);
			}
		}

		/// <summary>
		/// Start Workflow from index value of the available workflow combobox passed in
		/// </summary>
		/// <param name="index">index of the available workflow combobox</param>
		public void StartWorkflowByIndex(int index)
		{
			comboAvailableWorkflows.SelectedIndex = index;
			BeginWorkFlow();
		}

		/// <summary>
		/// Start Workflow from the name of the workflow passed in  
		/// </summary>
		/// <param name="name">Name of workflow</param>
		public void StartWorkflowByName(string name)
		{
			for (int index = 0; index < comboAvailableWorkflows.Items.Count; index++)
			{
				if (comboAvailableWorkflows.Items[index].ToString() == name)
				{
					StartWorkflowByIndex(index);
					break;
				}
			}
		}

		/// <summary>
		/// Start Workflow from the ID of the workflow passed in
		/// </summary>
		/// <param name="workflowID">ID of the workflow</param>
		public void StartWorkflowByID(int workflowID)
		{
			foreach (WorkflowData workflow in AllWorkflows)
			{
				if (workflow.WorkflowId == workflowID)
				{
					StartWorkflowByName(workflow.WorkflowName);
					break;
				}
			}
		}

		/// <summary>
		/// Triggers when label start/done clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void StartDone_Click(object sender, System.EventArgs e)
		{
			BeginWorkFlow();
		}

		private void BeginWorkFlow()
		{
			// start button clicked.
			if (StartDone.Text == localizeWF.WORKFLOW_BTN_START)
			{
				nLastIndex = 0;
				if (lvSteps.Items != null && lvSteps.Items.Count > 0)
				{
					ListviewClickHandler(nLastIndex, (int)lvSteps.Items[nLastIndex].Tag);
				}
			}
			else if (StartDone.Text == localizeWF.WORKFLOW_BTN_DONE)
			{
				if (DoneButtonHandler())
				{
					//Hiding main panel and sessionExplorer Panel
					//&&(nLastIndex== lvSteps.Items.Count))
					if (this.ParentForm.Controls[0].Name == "mainPanel")
					{
						lvSteps.Clear();
						comboAvailableWorkflows.SelectedIndex = 0;
						comboAvailableWorkflows.Enabled = true;
						StartDone.Text = localizeWF.WORKFLOW_BTN_START;
						StartDone.Enabled = false;
						this.Parent.Parent.Controls[1].Refresh();
					}
				}
			}
		}
	}
}