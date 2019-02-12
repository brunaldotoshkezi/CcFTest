//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// Workflow user control for the agent desktop that uses the human workflow
//
//===================================================================================

#region Usings
using System;
using System.ServiceModel;
using System.Xml;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Text;
using System.Reflection;

using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Common;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Samples.HostedControlInterfaces;
using Microsoft.Ccf.Samples.WfWorkflowControl.WorkflowWS;
using Microsoft.Ccf.WorkflowManager;
#endregion

namespace Microsoft.Ccf.Samples.WfWorkflowControl
{
	/// <summary>
	/// Summary description for WfWorkflowControl.
	/// </summary>
	// There was a request from Herb to divorce WfWorkflowControl from IWorkFlow. 
	// There are two reasons this is currently a problem:
	// 1.) AgentDesktop does not support DoAction so eventing cannot be done 
	// 2.) AgentDesktop hands its sessions over to WfWorkflowControl via the IWorkFlow property SessionManager
	public partial class WfWorkflowControl : HostedControl, Microsoft.Ccf.Samples.HostedControlInterfaces.IWorkFlow
	{

		#region Variables

		protected ConfigurationValueReader configurationReader = GeneralFunctions.ConfigurationReader(null);

		#region Private

		// Constant values
		private const string XML_WORKFLOW = "Workflow";

		// DateTime variables
		private int nIntervalMins = 4;
		private DateTime dtWorkflowsObtainedTime;

		// The index of the current workflow that is active, either pending or one of the available workflows chosen by the user.
		private int activeWorkflowId=-1;

		/// <summary>
		/// Used to reduce the number of workflow updates made.
		/// </summary>
		private bool workedBefore = false;
		private bool previousShow = true;

		// workflow web service
		private WorkflowWS.WorkflowWsClient WsWorkflow;
		private Sessions sessionManager;

		// Workflow data structures
		// Array of WorkflowData structures.
		private ArrayList workflowDataArray;
		// The current workflow that is active, either pending or one of the available workflows chosen by the user.
		private WorkflowData ActiveWorkflow;
		private AgentAndSessionData CurrentAgentSession;

		// Index of the previous list view selected index.
		private int lastSelectedIndex=-1;

		//UI variables
		// Indices for check and arrow images that appear as user moves through steps in listview
		private int greyCheckImageIndex = 0;
		private int greenCheckImageIndex = 1;
		private int greenArrowImageIndex = 2;

		private System.Drawing.Color BlueStartColor = System.Drawing.Color.FromArgb(0xFF, 0xFF, 0xFF);
		private System.Drawing.Color BlueEndColor = System.Drawing.Color.FromArgb(165, 207, 250);
		#endregion

		#endregion

		#region Events
		/// <summary>
		/// Raised when a workflow becomes active and when a workflow is completed/canceled.
		/// </summary>
		public event WorkFlowEventHandler WorkflowStatusChange;

		/// <summary>
		/// Notifies desktop that a workflow is started.
		/// </summary>
		public event WorkFlowEventHandler WorkflowStarted;
		#endregion

		/// <summary>
		/// Constructor
		/// </summary>
		public WfWorkflowControl()
		{
			this.init();
		}

		#region IWorkflow and IHostedApplication3 Members

		public WfWorkflowControl(int appID, string appName, string initString) :
		base( appID, appName, initString )
		{
			this.init();
		}

		#region Properties
		/// <summary>
		/// Return a boolean value indicating whether workflow has started or not
		/// </summary>
		public bool WorkflowNotStarted
		{
			get
			{
				return(sessionManager.ActiveSession.Workflow == string.Empty);
			}
		}

		/// <summary>
		/// Get or Set a value indicating whether the control can respond to user interaction.
		/// </summary>
		public bool ControlEnabled
		{
			get
			{
				return this.Enabled;
			}
			set
			{
				this.Enabled = value;
			}
		}

		/// <summary>
		/// Get or Set a value indicating whether the control is displayed.
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

		/// <summary>
		/// Get or set the parent container of the control
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
		/// Set the session manager object which holds available Session objects
		/// </summary>
		public override object SessionManager
		{
			set
			{
				sessionManager = (Sessions)value;
			}
		}

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
		/// Returns a boolean value indicating whether workflow is forced or not
		/// </summary>
		public bool IsForced
		{
			get
			{
				return ActiveWorkflow.IsForced; 
			}
		}

		/// <summary>
		/// Get the current workflow that is active
		/// </summary>
		public WorkflowData CurrentWorkflow
		{
			get
			{
				return this.ActiveWorkflow;
			}
		}

		#endregion

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
			this.comboAvailableWorkflows.Text=localizeWF.WORKFLOW_LBL_DEFAULT;

			Set();

			showHelp( false );
		}

		/// <summary>
		/// To initialize values
		/// </summary>
		private void Set()
		{
			// initialization
			CurrentAgentSession = new AgentAndSessionData();
			workflowDataArray = null;
			activeWorkflowId = -1;
			ActiveWorkflow = null;

			comboAvailableWorkflows.Enabled = false;
			Previous.Enabled = false;
			Next.Enabled = false;
			Cancel.Enabled = false;

			dtWorkflowsObtainedTime = DateTime.Now;
			
			if ( !this.DesignMode )
			{
				WsWorkflow = new WorkflowWsClient();
				WsWorkflow.Endpoint.Address = new EndpointAddress(this.configurationReader.ReadAppSettings("Microsoft_Samples_Ccf_DemoCode_CcfDemoApps_WorkFlowControl_WorkflowWS_WorkflowWs"));
				WsWorkflow.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();
			}
		}


		#region Painting methods
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
			Rectangle rectToPaint = new Rectangle(0, 0, plHeadingPanel.Width, plHeadingPanel.Height-1);
			using(LinearGradientBrush brBrushToPaint = new LinearGradientBrush(rectToPaint, BlueStartColor, BlueEndColor, LinearGradientMode.Horizontal))
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
			Rectangle rectToPaint = new Rectangle(0, 0, Separator.Width-1, Separator.Height-1);
			using(LinearGradientBrush brBrushToPaint = new LinearGradientBrush(rectToPaint, BlueEndColor, BlueEndColor, LinearGradientMode.Horizontal))
			{
				e.Graphics.FillRectangle(brBrushToPaint, rectToPaint);
			}
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
		#endregion Painting methods

		/// <summary>
		/// Update the combo box with available workflows.
		/// </summary>
		/// <returns>True for success else false.</returns>
		private bool UpdateCombo()
		{
			try
			{
				// Clear the contents in the combo.
				comboAvailableWorkflows.Items.Clear();

				// Add the item for default selection in ComboBox
				comboAvailableWorkflows.Items.Add(localizeWF.WORKFLOW_COMBO_DEFAULT_ITEM);

				int activeWorkflowIndex = -1;

				foreach ( WorkflowData workflow in workflowDataArray)
				{
					// add the workflow only if there are steps for it.
					if (workflow.Steps == null || workflow.Steps.Count == 0)
					{
						continue;
					}

					comboAvailableWorkflows.Items.Add(workflow.WorkflowName);

					// find a method of combo box to store the workflow Id along with the item.
					if (workflow.WorkflowId == activeWorkflowId)
					{
						activeWorkflowIndex = comboAvailableWorkflows.Items.Count - 1;
					}
				}

				// Set the selected index to the pending workflow.
				if (activeWorkflowIndex == -1)
				{
					// Implies that the pending workflow is not part
					// of the present available workflows. throw an exception in this case.
					activeWorkflowIndex = 0;
				}

				if( this.WorkflowNotStarted )
				{
					comboAvailableWorkflows.SelectedIndex = 0;
				}
				else if (comboAvailableWorkflows.Items.Count > 0)
				{
					comboAvailableWorkflows.SelectedIndex = activeWorkflowIndex;
				}

				workflowNameToolTip.SetToolTip(this.comboAvailableWorkflows, comboAvailableWorkflows.Text);
			}
			catch ( Exception exp)
			{
				Logging.Error(Application.ProductName,exp.ToString());
				throw exp;
			}
			return true;
		}

		/// <summary>
		/// Sets the button's text to the appropriate value.
		/// </summary>
		private void SetStartDoneText()
		{
			if ( sessionManager.ActiveSession == null ||
				!sessionManager.ActiveSession.IsWorkflowPending )
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
			if ( WorkflowStatusChange != null)
			{
				WorkflowArgs e = new WorkflowArgs();
				e.SessionId = CurrentAgentSession.SessionId;
				e.WorkflowStatus = (int)WorkflowStatus.Active;
				WorkflowStatusChange(this,e);
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
				if ( ActiveWorkflow.IsForced ) // forced
				{
					int nNumOfSteps = ActiveWorkflow.Steps.Count;
					WorkflowStep LastStep = new WorkflowStep();
					if ( nNumOfSteps > 0)
					{
						LastStep = (WorkflowStep)ActiveWorkflow.Steps[nNumOfSteps-1];
					}

					// if the workflow is at the last step, the workflow can be completed.
					if ( !ActiveWorkflow.AreAllStepsComplete() &&
						ActiveWorkflow.ActiveStepId != LastStep.Workflowstepid)
					{
						// at least one step is not complete.
						MessageBox.Show( localizeWF.WORKFLOW_COMPLETE_STEP, localizeWF.COMMON_MSGBOX_TITLE_PART1 + localizeWF.WORKFLOW_MSGBOX_TITLE_PART2);
						return false;
					}
				}

				// refresh the view and data structures.
				LocalRefreshAfterDoneOrCancel( WorkflowStatus.Complete );
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
			if ( MessageBox.Show(this, localizeWF.WORKFLOW_CANCEL_CONFIRM, localizeWF.COMMON_MSGBOX_TITLE_PART1 + localizeWF.WORKFLOW_MSGBOX_TITLE_PART2,MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return false;
			}

			// refresh the view and data structures.
			return LocalRefreshAfterDoneOrCancel( WorkflowStatus.Cancel );
		}

		/// <summary>
		/// Refresh workflow
		/// </summary>
		/// <param name="status">workflow status</param>
		/// <returns>true if success else false</returns>
		private bool LocalRefreshAfterDoneOrCancel(WorkflowStatus status )
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
				SaveActiveWorkflow( status, -1 );

				// Raise the event to notify the others.
				if ( WorkflowStatusChange != null)
				{
					WorkflowArgs e = new WorkflowArgs();
					e.SessionId = CurrentAgentSession.SessionId;
					e.WorkflowStatus = (int)status;
					WorkflowStatusChange(this,e);
				}

				// Set for the next workflow.
				ActiveWorkflow = new WorkflowData();

				activeWorkflowId = -1;

				if ( workflowDataArray != null && workflowDataArray.Count != 0)
				{
					// Set the first workflow as the active workflow.
					activeWorkflowId = ((WorkflowData)workflowDataArray[0]).WorkflowId;
					ActiveWorkflow = ((WorkflowData)workflowDataArray[0]).ReturnACopy();
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
				if ( nClickedStepIndex < 0 || nClickedStepIndex > lvSteps.Items.Count)
				{
					return false;
				}

				Cursor = Cursors.WaitCursor;

				// do not check if the same step is clicked.
				// update the view anyways.

				// make the clicked row visible in the list view.
				lvSteps.EnsureVisible( nClickedStepIndex );

				// the workflow is started by clicking on a step.
				if ( ActiveWorkflow.ActiveStepId == -1)
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
					string strStepsNotAccessible = CheckForAccessToAllSteps( ActiveWorkflow );
					if ( strStepsNotAccessible != null && strStepsNotAccessible != string.Empty)
					{
						// user doesn't have access to one or more hosted app ids.
						// display and log the error and return.
						string strMsg = localizeWF.WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART1 + strStepsNotAccessible + localizeWF.WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART2;
						Logging.Warn (Application.ProductName, localizeWF.WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_STEPS + strStepsNotAccessible);

						// ask the user if s/he wants to proceed.
						if ( MessageBox.Show(strMsg, localizeWF.COMMON_MSGBOX_TITLE_PART1 + localizeWF.WORKFLOW_MSGBOX_TITLE_PART2,MessageBoxButtons.YesNo) == DialogResult.No)
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
				if ( ActiveWorkflow.ActiveStepId != -1 && nClickedStepId != ActiveWorkflow.ActiveStepId)
				{
					// Set the step complete.
					ActiveWorkflow.GetStepByStepId( ActiveWorkflow.ActiveStepId ).IsStepComplete = true;
					// Setting the green check mark is done in selection change itself.
				}

				if ( nClickedStepId != ActiveWorkflow.ActiveStepId )
				{
					// Form the xml and save it to the DB.
					SaveActiveWorkflow( WorkflowStatus.Active, nClickedStepId );
				}

				// set the current active step to nStepClicked
				ActiveWorkflow.ActiveStepId = nClickedStepId;
				lvSteps.Items[nClickedStepIndex].ImageIndex = greenArrowImageIndex;

				// take care of enabling/disabling of the prev and the next step.
				EnableDisablePrevNextButtons( nClickedStepIndex );
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
				if(ActiveWorkflow==null)
				{
					lvSteps.Items.Clear();
				}
				if ( ActiveWorkflow == null || ActiveWorkflow.Steps == null)
				{
					return false;
				}

				// Add the steps of the active workflow to the list view.
				if ( ActiveWorkflow.Steps.Count == 0)
				{
					return false;
				}

				if(comboAvailableWorkflows.SelectedItem.ToString() != localizeWF.WORKFLOW_COMBO_DEFAULT_ITEM)
				{
					AddStepsToListView( ActiveWorkflow );
				}
				

				// If it is a pending workflow, raise the events.
				if ( ActiveWorkflow.ActiveStepId != -1 )
				{
					if ( WorkflowStatusChange != null )
					{
						WorkflowArgs e = new WorkflowArgs();
						e.SessionId = CurrentAgentSession.SessionId;
						e.WorkflowStatus = (int) WorkflowStatus.Active;
						WorkflowStatusChange(this,e);
					}
				}

				// Take care of enabling/disabling of the prev and the next step.
				EnableDisablePrevNextButtons( lastSelectedIndex );
			}
			catch ( Exception exp)
			{
				Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_LISTVIEW_UPDATE,exp);
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
			if ( nCurrentIndex == -1)
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
			SizeF size = new SizeF(0,0);

			for ( int nIndex = 0; nIndex < AddWorkflow.Steps.Count; nIndex++ )
			{
				ListViewItem lvItem = new ListViewItem();
				lvItem.Text = AddWorkflow.GetStepByIndex(nIndex).WorkflowStepName;
				lvItem.Tag = AddWorkflow.GetStepByIndex(nIndex).Workflowstepid;

				if ( AddWorkflow.GetStepByIndex(nIndex).IsStepComplete)
				{
					lvItem.ImageIndex = greenCheckImageIndex;
				}
				else if ( AddWorkflow.GetStepByIndex(nIndex).Workflowstepid == AddWorkflow.ActiveStepId)
				{
					lvItem.ImageIndex = greenArrowImageIndex;
					lastSelectedIndex = nIndex;
				}
				else
				{
					lvItem.ImageIndex = greyCheckImageIndex;
				}
				lvSteps.Items.Add(lvItem);

				size = gra.MeasureString(lvItem.Text,lvSteps.Font);
				if ( size.Width > fColumnWidth)
				{
					fColumnWidth = size.Width;
				}
			}

			// If the columns are not there, add it explicitly so as to set the column width.
			if ( lvSteps.Columns.Count <= 0)
			{
				this.lvSteps.Columns.AddRange(
					new System.Windows.Forms.ColumnHeader[] {this.dummyCol});
			}
			lvSteps.Columns[0].Width = (int)fColumnWidth + lvSteps.SmallImageList.ImageSize.Width + 10;

			// Make the current active step visible.
			if ( lastSelectedIndex != -1)
			{
				lvSteps.EnsureVisible(lastSelectedIndex);
			}
		}

		/// <summary>
		/// Puts workflow names, workflow id and forced/unforced workflow status into the workflowDataArray from webservices
		/// </summary>
		/// <returns>True if new workflow names obtained</returns>
		private bool GetWorkflowNames()
		{
			bool bResult = false;
			bool newWorkflowsObtained = false;

			try
			{
				// So that work flow names will periodically be re-read from the server.
				// Though not when the change is made, just some time later.
				TimeSpan timediff = new TimeSpan(0,nIntervalMins,0);
				if ( workflowDataArray != null && workflowDataArray.Count != 0 && timediff > (dtWorkflowsObtainedTime - DateTime.Now) )
				{
					newWorkflowsObtained = false;
					bResult = true;
				}
				else
				{
					// Get the workflows.
					// Make a web method call to get the available workflows.
					string strWorkflowsXML = string.Empty;
					strWorkflowsXML = WsWorkflow.GetWorkflowNames( CurrentAgentSession.AgentId );
			
					if ( strWorkflowsXML == string.Empty)
					{
						// No workflows for this agent.
						Logging.Warn(Application.ProductName,localizeWF.WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_WORKFLOWS);
						return false;
					}

					// puts workflow names, workflow id and forced/unforced workflow status into workflowDataArray
					if ( SetWorkflowDataFromXML( strWorkflowsXML, true ) )
					{
						// Update the item and the variable saying that new workflows have been obtained.
						newWorkflowsObtained = true;
						dtWorkflowsObtainedTime = DateTime.Now;
						bResult = true;
					}
					else
					{
						Logging.Warn(Application.ProductName, localizeWF.WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_WORKFLOWS);
					}
				}

				if ( bResult)
				{
					activeWorkflowId = -1;

					if ( workflowDataArray != null && workflowDataArray.Count != 0 )
					{
						// Set the first workflow as the active workflow.
						activeWorkflowId = ((WorkflowData)workflowDataArray[0]).WorkflowId;
						ActiveWorkflow = ((WorkflowData)workflowDataArray[0]).ReturnACopy();
					}
				}
			}
			catch ( System.Net.WebException wex )
			{
				// Catch here so the general Exception below is not catch it.
				throw wex;
			}
			catch ( Exception exp )
			{
				// Check message - if appropriate log it.
				if ( exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST ) < 0  &&
					exp.Message.IndexOf( localizeWF.COMMON_MSG_IIS_ERROR) < 0)
				{
					Logging.Error( Application.ProductName, localizeWF.WORKFLOW_ERR_GET_WORKFLOWS, exp );
				}
				throw exp;
			}
			return newWorkflowsObtained;
		}

		/// <summary>
		/// Deserialize the workflows into workflowDataArray
		/// </summary>
		/// <param name="strWorkflowsXML">Workflow Xml</param>
		/// <param name="bShowErr">Flag saying if any errors are to be displayed or not</param>
		/// <returns>Returns true is workflow read from XML.</returns>
		private bool SetWorkflowDataFromXML(string strWorkflowsXML, bool bShowErr)
		{
			bool bReturnVal = false;

			if ( strWorkflowsXML != string.Empty )
			{
				try
				{
					workflowDataArray = new ArrayList();

					XmlDocument doc = new XmlDocument();
					doc.LoadXml( strWorkflowsXML );
					if ( doc.ChildNodes.Count > 0)
					{
						XmlNode root = doc.DocumentElement;
						XmlNodeList Workflows = root.SelectNodes(XML_WORKFLOW);
						// Check for nodes.
						if ( Workflows.Count > 0 )
						{
							foreach ( XmlNode WorkflowNode in Workflows)
							{
								// Form the workflowdata structure
								WorkflowData Workflow = new WorkflowData();
								Workflow.SetWorkflowData(WorkflowNode.OuterXml);
								// Add the structure to the list.
								workflowDataArray.Add(Workflow);
							}

							bReturnVal = true;
						}
					}
				}
				catch ( Exception exp)
				{
					if ( bShowErr)
					{
						Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_SET_WORKFLOW_DATA_FROM_XML,exp);
						throw exp;
					}
				}
			}

			return bReturnVal;
		}
		
		/// <summary>
		/// Deserialize WorkflowStepArray for all available workflows.
		/// </summary>
		/// <returns>True for success else false</returns>
		private bool GetWorkflowSteps()
		{
			try
			{
				// Some thing wrong happened, should give an error.
				if ( workflowDataArray == null )
				{
					return false;
				}

				// Get the fresh workflow steps.
				foreach ( WorkflowData EachWorkflow in workflowDataArray)
				{
					string strWorkflowXML = string.Empty;

					// Call the web method to get the steps of the workflow.
					strWorkflowXML = WsWorkflow.GetWorkflowSteps(EachWorkflow.WorkflowId);

					if ( strWorkflowXML == string.Empty)
					{
						// No steps for this, check for the next workflow.
						continue;
					}

					// Deserialize steps
					EachWorkflow.SetWorkflowStepsData(strWorkflowXML);
				}

				SetStartDoneText();
			}
			catch ( Exception exp)
			{
				if ( exp.Message.IndexOf( localizeWF.DESKTOP_MSG_SQL_EXIST ) < 0  &&
					exp.Message.IndexOf( localizeWF.COMMON_MSG_IIS_ERROR) < 0)
					Logging.Error(Application.ProductName, localizeWF.WORKFLOW_GET_WORKFLOW_STEPS,exp);
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Deserialize WorkflowData from sessionManager.ActiveSession.Workflow to ActiveWorkflow and
		/// set the ActiveWorkflow to the one from XML in current session
		/// This is used when switching sessions or when restoring previously saved sessions
		/// </summary>
		/// <returns>True for success else false.</returns>
		private bool SetInternalWfFromSession()
		{
			try
			{
				// Call the session manager to get the pending workflow.
				if (sessionManager.ActiveSession.Workflow == string.Empty)
				{
					return true;
				}

				// Deserialize the WorkflowData
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(sessionManager.ActiveSession.Workflow);
				XmlNode root = doc.DocumentElement;
				XmlNode restoredWrkflNode = root.SelectSingleNode("restoredWorkflow");
				
				if (root.LocalName == "XML")
				{
					string prepareToDeserialize = "<?xml version=\"1.0\" encoding=\"utf-16\"?><WorkflowData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + root.InnerXml + "</WorkflowData>";
					ActiveWorkflow = (WorkflowData)GeneralFunctions.Deserialize(prepareToDeserialize, typeof(WorkflowData));
				}
				else
				{
					ActiveWorkflow = (WorkflowData)GeneralFunctions.Deserialize(sessionManager.ActiveSession.Workflow, typeof(WorkflowData));
				}

				activeWorkflowId = ActiveWorkflow.WorkflowId;

				StartDone.Text = localizeWF.WORKFLOW_BTN_DONE;
				comboAvailableWorkflows.Enabled = false;

				// Workflow Driven Implementation:
				// Check if the workflow has been recoved from pending state as part of saved session 
				// restoration. If it is, then we shoudl notify desktop so that the required apps are
				// loaded and made visible. Otherwise, if it is just session change then the the required 
				// applications might have already loaded.
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
			catch ( Exception exp)
			{
				if ( exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST ) < 0  &&
					exp.Message.IndexOf(localizeWF.COMMON_MSG_IIS_ERROR) < 0)
				{
					Logging.Error(Application.ProductName, localizeWF.WORKFLOW_ERR_GET_PENDING_WORKFLOW,exp);
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
		/// Is the workflow pending for the current session?
		/// </summary>
		/// <returns>True if a valid pending workflow is present, else return false even in the case of errors
		/// </returns>
		/// Gets or sets if the workflow being used (pending).  Returns false if no
		/// workflow or if its done or cancelled.
		// 1.)Gets workflows via webservices into the workflowdata array if they're not there already
		// 2.)Deserializes workflow from ActiveSession
		// 3.)Determines if a workflow is pending by checking the activeWorkflowStep.When a workflow is inactive the
		// workflow step is -1
		public bool IsWorkflowPending(int nAgentId)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				// Check if you have all the available workflows for this agent.
				if (workflowDataArray == null || workflowDataArray.Count == 0)
				{
					// Get the list of available workflows.
					// Get the workflows.
					// Make a web method call to get the available worklfows.
					string strWorkflowsXML = string.Empty;
					strWorkflowsXML = WsWorkflow.GetWorkflowNames(nAgentId);

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

				// Call the session manager to get the pending workflow.
				if (sessionManager.ActiveSession.Workflow == string.Empty)
				{
					return false;
				}

				// Deserialize the WorkflowData
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(sessionManager.ActiveSession.Workflow);
				XmlNode root = doc.DocumentElement;
				WorkflowData workflowFromSession = null;

				if (root.LocalName == "XML")
				{
					string prepareToDeserialize = "<?xml version=\"1.0\" encoding=\"utf-16\"?><WorkflowData xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">" + root.InnerXml + "</WorkflowData>";
					workflowFromSession = (WorkflowData)GeneralFunctions.Deserialize(prepareToDeserialize, typeof(WorkflowData));
				}
				else
				{
					workflowFromSession = (WorkflowData)GeneralFunctions.Deserialize(sessionManager.ActiveSession.Workflow, typeof(WorkflowData));
				}

				return (workflowFromSession.ActiveStepId != -1);

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
		/// Serialize the current active workflow as the pending workflow for this session.
		/// </summary>
		/// <param name="status">Workflow status</param>
		/// <param name="nActiveStepId">Active step Id</param>
		/// <returns>True if there is no exception.</returns>
		private bool SaveActiveWorkflow(WorkflowStatus status, int nActiveStepId )
		{
			try
			{
				// Serialize the workflow 
				ActiveWorkflow.ActiveStepId = nActiveStepId;
				string serializedWorkflow = GeneralFunctions.Serialize(ActiveWorkflow);

				// Save workflow to session
				if (status == WorkflowStatus.Active)
				{
					sessionManager.ActiveSession.Workflow = serializedWorkflow;
				}
				else
				{
					sessionManager.ActiveSession.Workflow = String.Empty;
				}
			}
			catch ( Exception exp)
			{
				if ( exp.Message.IndexOf(localizeWF.DESKTOP_MSG_SQL_EXIST ) < 0  &&
					exp.Message.IndexOf(localizeWF.COMMON_MSG_IIS_ERROR) < 0 )
				{
					Logging.Error(Application.ProductName,localizeWF.WORKFLOW_ERR_SAVE_ACTIVE_WORKFLOW, exp);
				}
				throw exp;
			}

			return true;
		}

		/// <summary>
		/// Call this function to refresh the workflow view.
		/// </summary>
		/// <param name="agentId">Agent ID</param>
		/// <param name="sessionID">Session ID</param>
		/// <returns>True for success else false</returns>
		public bool WorkflowUpdate(int agentId, Guid sessionID )
		{
			// check if session Id and the AgentId are valid.
			if (agentId == 0 || sessionID == Guid.Empty)
			{
				throw new ArgumentNullException("agentId or sessionID", localizeWF.WORKFLOW_ERR_AGENTID_SESSIONID_NULL);
			}

			try
			{
				//TODO what is this for?
				Cursor = Cursors.WaitCursor;

				// Check if the session ID is the same, no refresh required.
				if ( workedBefore && sessionID == CurrentAgentSession.SessionId )
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
				activeWorkflowId = -1;

				// Save the new agent id and the session id.
				CurrentAgentSession.AgentId = agentId;
				CurrentAgentSession.SessionId = sessionID;

				// Get the available workflow names.
				GetWorkflowNames();
				
				// Get the workflow steps for each workflow.
				GetWorkflowSteps();


				// Get the pending workflow.
				if ( !SetInternalWfFromSession() )
				{
					UpdateUIElementsState( false );
					return false;
				}

				// Add the workflow names to the combo.
				// Set the selection and also raises the event to update the list view.
				if ( !UpdateCombo() )
				{
					UpdateUIElementsState( false );
					return false;
				}

				// Update the list view.
				if ( !UpdateListView() )
				{
					UpdateUIElementsState( true  );
					return true;
				}

				UpdateUIElementsState( true );

				if((comboAvailableWorkflows.SelectedItem.ToString()==localizeWF.WORKFLOW_COMBO_DEFAULT_ITEM)) //|| (comboAvailableWorkflows.SelectedText==""))
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
			if ( bEnable == false )
			{
				comboAvailableWorkflows.Enabled = bEnable;
			}

			StartDone.Enabled = ( ActiveWorkflow == null || ActiveWorkflow.Steps == null)? false : bEnable;

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
			ListView lvw  = (ListView)sender;

			if ( lvw.SelectedIndices.Count != 0 )
			{	
				nSelIndex = lvw.SelectedIndices[0];
				if ( nSelIndex!=lastSelectedIndex )
				{
					if ( ActiveWorkflow.IsForced )
					{
						if ( Math.Abs(nSelIndex-lastSelectedIndex) > 1 )
						{
							if ( lastSelectedIndex != -1 ) 
							{
								lvw.Items[lastSelectedIndex].Selected = true;
								lvw.Items[lastSelectedIndex].Focused = true;
							}
							else
							{
								lvw.SelectedItems[0].Focused = false;
								lvw.SelectedItems[0].Selected = false;
							}
						}
						else
						{
							if ( lastSelectedIndex != -1)
								lvw.Items[lastSelectedIndex].ImageIndex = greenCheckImageIndex;
							lastSelectedIndex=nSelIndex;
							CommonStepHandler();
						}
					}
					else
					{
						if (lastSelectedIndex != -1)
						{
							lvw.Items[lastSelectedIndex].ImageIndex = greenCheckImageIndex;
						}
						lastSelectedIndex=nSelIndex;
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
			if (lastSelectedIndex != -1)
			{
				CommonStepHandler();
			}

			StartWorkflowRuntimeInstance();
		}

		/// <summary>
		/// Start an instance of workflow runtime and pass in the needed parameters
		/// </summary>
		public void StartWorkflowRuntimeInstance()
		{
			// Create a StringBuilder object to perform string concatenation
			StringBuilder sb = new StringBuilder();

            sb.Append("<Workflow>");
            sb.Append("<StepName>" + lvSteps.FocusedItem.Text + "</StepName>");
            WorkflowStep activeStep = ActiveWorkflow.GetStepByStepId(ActiveWorkflow.ActiveStepId);

            // Check if the Agent has access to all the steps' hosted applications.
            string strStepsNotAccessible = CheckForAccessToAllSteps(ActiveWorkflow);
            if (strStepsNotAccessible.Contains(activeStep.WorkflowStepName))
                return;
            else
            {
                sb.Append("<HostedApplicationId>" + activeStep.HostedApplicationId.ToString() + "</HostedApplicationId>");
                sb.Append("<HostedApplicationName>" + this.sessionManager.ActiveSession.GetApplication(activeStep.HostedApplicationId).ApplicationName.ToString() + "</HostedApplicationName>");
                sb.Append("<Action>" + activeStep.WorkflowStepAction + "</Action>");
                sb.Append("</Workflow>");
            }
            FireRequestAction(new RequestActionEventArgs("Customer Workflow Manager", "Human Workflow Automation", sb.ToString()));
 		}

		/// <summary>
		/// Common handler when a step is selected, either by mouse or through keyboard.
		/// </summary>
		private void CommonStepHandler()
		{
			lvSteps.Items[lastSelectedIndex].Selected = true;
			lvSteps.Items[lastSelectedIndex].Focused = true;
			lvSteps.Items[lastSelectedIndex].ImageIndex = greenArrowImageIndex;

			ListviewClickHandler( lastSelectedIndex, (int)lvSteps.Items[lastSelectedIndex].Tag );
		}
 
		/// <summary>
		/// Triggers when workflow combo box selected index changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void comboAvailableWorkflows_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			
			if ( !comboAvailableWorkflows.Enabled)
			{
				// Update the label to specify if the workflow is forced or non-forced.
				if ( ActiveWorkflow != null )
				{
					if (ActiveWorkflow.IsForced)
					{
						lbCurrentWorkflow.Text =localizeWF.WORKFLOW_LBL_CURRENT_WORKFLOW + " " + localizeWF.WORKFLOW_LBL_FORCED;
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

			// Get the workflow id of the selected item in the combo box.
			int nIndex = -1;
			for (int i = 0 ; i < workflowDataArray.Count; i++ )
			{
				if ( comboAvailableWorkflows.Items[comboAvailableWorkflows.SelectedIndex].ToString() == ((WorkflowData)workflowDataArray[i]).WorkflowName)
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
				activeWorkflowId = -1;
				ActiveWorkflow = null;
				lbCurrentWorkflow.Text = localizeWF.WORKFLOW_LBL_DEFAULT;
				StartDone.Enabled = false;
			}
			else
			{
				// set the active workflow.
				activeWorkflowId = ((WorkflowData)workflowDataArray[nIndex]).WorkflowId;
				ActiveWorkflow =  ((WorkflowData)workflowDataArray[nIndex]).ReturnACopy();

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
			lastSelectedIndex = -1;
			// update the list view.
			UpdateListView();
			EnableDisablePrevNextButtons(lastSelectedIndex);
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

			foreach ( WorkflowStep step in SelectedWorkflow.Steps )
			{
				// If the agent does not have access to any of the hosted applications
				// or if its missing some of the steps,
				// return the list of the steps it doesn't have access to.
				if ( !sessionManager.ActiveSession.ApplicationExists( step.HostedApplicationId ) )
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
			if ( lastSelectedIndex >=0 && lastSelectedIndex < lvSteps.Items.Count 
				&& lastSelectedIndex-1 >=0 && lastSelectedIndex-1 < lvSteps.Items.Count)
			{
				lvSteps.Items[lastSelectedIndex].ImageIndex = greenCheckImageIndex;
				lvSteps.Items[lastSelectedIndex-1].Selected = true;
				// Once the above statement is executed, the nLastIndex is changed and hence
				// do not change the following nLastIndex to nLastIndex+1.
				lvSteps.Items[lastSelectedIndex].Focused = true;
				
				StartWorkflowRuntimeInstance();
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
			if ( lastSelectedIndex >=0 && lastSelectedIndex < lvSteps.Items.Count 
				&& lastSelectedIndex+1 >=0 && lastSelectedIndex+1 < lvSteps.Items.Count)
			{
				lvSteps.Items[lastSelectedIndex].ImageIndex = greenCheckImageIndex;
				lvSteps.Items[lastSelectedIndex+1].Selected = true;
				// Once the above statement is executed, the nLastIndex is changed and hence
				// do not change the following nLastIndex to nLastIndex+1.
				lvSteps.Items[lastSelectedIndex].Focused= true;

				StartWorkflowRuntimeInstance();
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
			activeWorkflowId = -1;

			CurrentAgentSession = new AgentAndSessionData();
			lastSelectedIndex = -1;
		}

		/// <summary>
		/// Show/Hide the help content and also update its arrow.
		/// </summary>
		/// <param name="sender">The help icon or help label</param>
		/// <param name="e">System event arguments.</param>
		private void Help_Click(object sender, System.EventArgs e)
		{
			showHelp( !helpContent.Visible );
			Help.ImageIndex = (Help.ImageIndex == 0) ? 1 : 0;
		}

		/// <summary>
		/// When the help text is hidden, allow the session tree to expand to fill its
		/// space.
		/// </summary>
		/// <param name="show">True to show help text, false to hide it</param>
		private void showHelp( bool show )
		{
			if (show == previousShow)
			{
				return;
			}

			helpContent.Visible = previousShow = show;
			if ( show )
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
			showHelp( helpContent.Visible );
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
			for(int index = 0; index < comboAvailableWorkflows.Items.Count; index++)
			{
				if(comboAvailableWorkflows.Items[index].ToString() == name)
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
			foreach ( WorkflowData workflow in workflowDataArray)
			{
				if(workflow.WorkflowId == workflowID)
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
			if ( StartDone.Text == localizeWF.WORKFLOW_BTN_START)
			{
				lastSelectedIndex = 0;
				if ( lvSteps.Items != null && lvSteps.Items.Count > 0 )
				{
					lvSteps_Click(null, null);
				}
			}
			else if ( StartDone.Text == localizeWF.WORKFLOW_BTN_DONE)
			{
				if(DoneButtonHandler())
				{
					if(this.ParentForm.Controls[0].Name=="mainPanel")
					{
						lvSteps.Clear();
						comboAvailableWorkflows.SelectedIndex=0;
						comboAvailableWorkflows.Enabled=true;
						StartDone.Text=localizeWF.WORKFLOW_BTN_START;
						StartDone.Enabled=false;
						this.ParentForm.Controls[4].Controls[1].Refresh();
					}
				}
			}
		}
	}
}