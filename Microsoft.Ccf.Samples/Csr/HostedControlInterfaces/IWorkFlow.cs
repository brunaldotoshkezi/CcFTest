//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// This file contains the interface definition for the IWorkFlow interface.
// 
//===============================================================================
#pragma warning disable 1591

#region Usings
using System;
using System.Windows.Forms;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Csr;
using System.Resources;
using System.Reflection;
#endregion

namespace Microsoft.Ccf.Samples.HostedControlInterfaces
{
	/// <summary>
	/// Declaration of a delegate for the Workflow events
	/// </summary>
	public delegate void WorkFlowEventHandler(object sender, WorkflowArgs e);

	/// <summary>
	/// if WorkflowStatus is 1, a workflow is made active. If the WorkflowStatus is 0, a workflow is complete. If the WorkflowStatus is 2, a workflow is canceled.
	/// workflow web service
	/// </summary>
	public enum WorkflowStatus
	{
		/// <summary>
		/// Workflow is active
		/// </summary>
		Active = 0,
		/// <summary>
		/// Workflow is complete
		/// </summary>
		Complete,
		/// <summary>
		/// Workflow is cancel
		/// </summary>
		Cancel,
		/// <summary>
		/// Workflow is restored
		/// </summary>
		Restored
	};

	/// <summary>
	/// Interface definition for IWorkflow
	/// </summary>
	public interface IWorkFlow : IHostedApplication3
	{
		/// <summary>
		/// this event will be raised when a workflow becomes active and when a workflow is completed/canceled.
		/// </summary>
		event WorkFlowEventHandler WorkflowStatusChange;

		// Workflow Driven Implementation:
		/// <summary>
		/// Required a new event which can properly notify the desktop that
		/// a workflow is started.
		/// </summary>
		event WorkFlowEventHandler WorkflowStarted;

		/// <summary>
		/// Returns a boolean value indicating whether workflow has started or not
		/// </summary>
		bool WorkflowNotStarted{ get; }

		/// <summary>
		/// Gets or Set value indicating whether the control can respond to user interaction
		/// </summary>
		bool ControlEnabled{ set; get; }

		/// <summary>
		/// get or set the parent container of the control
		/// </summary>
		Control ControlParent{ get; set; }

		/// <summary>
		/// Gets or Sets value indicating whether the control is displayed.
		/// </summary>
		bool ControlVisible{ set; get; }

		/// <summary>
		/// Get the current workflow that is active.
		/// </summary>
		WorkflowData CurrentWorkflow { get;}

		/// <summary>
		/// Call this function to refresh the workflow view.
		/// </summary>
		/// <param name="agentId">Agent ID</param>
		/// <param name="sessionID">Session ID</param>
		/// <returns>true if success else false</returns>
		bool WorkflowUpdate(int agentId, Guid sessionID );

		/// <summary>
		/// Is the workflow XML in the Session's Workflow string property a valid pending workflow?
		/// </summary>
		/// <returns>true if a valid pending workflow is present,
		/// else return false even in the case of errors</returns>
		bool IsWorkflowPending(int nAgentId);

		/// <summary>
		/// This function is called from the desktop to clear the contents of the workflow.
		/// </summary>
		void Clear();

		/// <summary>
		/// Start Workflow from index value of the available workflow combobox passed in
		/// </summary>
		/// <param name="index">index of the available workflow combobox</param>
		void StartWorkflowByIndex(int index);

		/// <summary>
		/// Start Workflow from the name of the workflow passed in  
		/// </summary>
		/// <param name="name">Name of workflow</param>
		void StartWorkflowByName(string name);

		/// <summary>
		/// Start Workflow from the ID of the workflow passed in
		/// </summary>
		/// <param name="workflowID">ID of the workflow</param>
		void StartWorkflowByID(int workflowID);

		/// <summary>
		/// Returns a boolean value indicating whether workflow is forced or not
		/// </summary>
		bool IsForced { get; }
	}


	public class WorkflowArgs : EventArgs
	{
		private Guid m_SessionId = Guid.Empty;
		private int m_nWorkflowStatus = -1;
		private int m_nApplicationId = -1;

		// Workflow Driven Implementation:
		// Required by the handlers to know the current workflow and its data.
		private WorkflowData m_CurrentWorkflow;

		public Guid SessionId
		{
			get { return m_SessionId; }
			set { m_SessionId = value; }
		}

		public int WorkflowStatus
		{
			get { return m_nWorkflowStatus; }
			set { m_nWorkflowStatus = value; }
		}

		public int ApplicationId
		{
			get { return m_nApplicationId; }
			set { m_nApplicationId = value; }
		}

		// Workflow Driven Implementation:
		// Required by the handlers to know the current workflow and its data.
		public WorkflowData CurrentWorkflow
		{
			get { return m_CurrentWorkflow; }
			set { m_CurrentWorkflow = value; }
		}
	}

	/// <summary>
	/// Contains strings in the appropriate languge for the UI.
	/// </summary>
	public class localizeWF
	{
		// Globalized strings
		public static string WORKFLOW_MODULE_NAME = "Workflow";
		public static string WORKFLOW_ERR_UPDATING_COMBO = "";
		public static string WORKFLOW_COMPLETE_STEP = "";
		public static string WORKFLOW_CANCEL_CONFIRM = "";
		public static string WORKFLOW_ERR_SET_WORKFLOW_DATA_FROM_XML = "";
		public static string WORKFLOW_GET_WORKFLOW_STEPS = "";
		public static string WORKFLOW_ERR_PENDING_WORKFLOW_ID = "";
		public static string WORKFLOW_ERR_GET_PENDING_WORKFLOW = "";
		public static string WORKFLOW_ERR_SAVE_ACTIVE_WORKFLOW = "";
		public static string WORKFLOW_LBL_CURRENT_WORKFLOW = "";
		public static string WORKFLOW_LBL_AVL_WORKFLOWS = "";
		public static string WORKFLOW_LBL_HELP_CONTENT = "";
		public static string WORKFLOW_LBL_HELP = "Help";
		public static string WORKFLOW_BTN_START = "Start";
		public static string WORKFLOW_BTN_DONE = "Done";
		public static string WORKFLOW_LBL_CANCEL = "Cancel";
		public static string WORKFLOW_LBL_NEXT = "Next";
		public static string WORKFLOW_LBL_PREVIOUS = "Previous";
		public static string WORKFLOW_LBL_WORKFLOW_STEPS = "";
		public static string WORKFLOW_HEADER_LABEL = "Workflow";   
		public static string WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART1 = "";
		public static string WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART2 = "";
		public static string WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_STEPS = "";
		public static string WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_WORKFLOWS = "";
		public static string WORKFLOW_LBL_FORCED = "Forced";
		public static string WORKFLOW_LBL_NON_FORCED = "Non-forced";
		public static string WORKFLOW_ERR_LISTVIEW_UPDATE = "";
		public static string WORKFLOW_ERR_GET_WORKFLOWS = "";
		public static string DESKTOP_MSG_SQL_EXIST = "";
		public static string COMMON_MSGBOX_TITLE_PART1 = "Workflow";
		public static string WORKFLOW_ERR_AGENTID_SESSIONID_NULL = "";
		public static string COMMON_MSG_IIS_ERROR = "";
		public static string WORKFLOW_MSGBOX_TITLE_PART2 = "";
		public static string WORKFLOW_ERR_TRANSFER_WORKFLOW_FAILED = "";

		// Workflow Driven Implementation:
		public static string WORKFLOW_COMBO_DEFAULT_ITEM = "";
		// Workflow Driven Implementation:
		public static string WORKFLOW_LBL_DEFAULT = "Select a workflow";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_STEP_ERR_SET_STEP_DATA = "";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_DATA = "";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_STEPS_DATA = "";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA = "";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_STATUS_INVALID = "";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_WORKFLOW_DATA_IN_XML = "";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SESSIONID_INVALID = "";
		public static string WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA_SET_TO_FIRST_STEP = "";

		static private ResourceManager rm = null;

		/// <summary>
		/// This checks that the string was found and handles an error if not.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		static private string GetAndCheckString( string name )
		{
			string val = rm.GetString( name );
			if ( val == null )
			{
				// Not localized since the error indicates there may be a localization problem.
				Logging.Error( "Workflow",
					String.Format( "Unable to find string {0} in resource file.", name ) );
				val = name;
			}

			return val;
		}

		static localizeWF()
		{
			try
			{
				Assembly asm = Assembly.GetExecutingAssembly(); 
				rm = new ResourceManager("Microsoft.Ccf.Samples.HostedControlInterfaces.Strings", asm); 

				WORKFLOW_MODULE_NAME = GetAndCheckString("WORKFLOW_MODULE_NAME");
				WORKFLOW_ERR_UPDATING_COMBO = GetAndCheckString("WORKFLOW_ERR_UPDATING_COMBO");
				WORKFLOW_COMPLETE_STEP = GetAndCheckString("WORKFLOW_COMPLETE_STEP");
				WORKFLOW_CANCEL_CONFIRM = GetAndCheckString("WORKFLOW_CANCEL_CONFIRM"); 
				WORKFLOW_ERR_SET_WORKFLOW_DATA_FROM_XML = GetAndCheckString("WORKFLOW_ERR_SET_WORKFLOW_DATA_FROM_XML");
				WORKFLOW_GET_WORKFLOW_STEPS = GetAndCheckString("WORKFLOW_GET_WORKFLOW_STEPS");
				WORKFLOW_ERR_PENDING_WORKFLOW_ID = GetAndCheckString("WORKFLOW_ERR_PENDING_WORKFLOW_ID");
				WORKFLOW_ERR_GET_PENDING_WORKFLOW = GetAndCheckString("WORKFLOW_ERR_GET_PENDING_WORKFLOW");
				WORKFLOW_ERR_SAVE_ACTIVE_WORKFLOW = GetAndCheckString("WORKFLOW_ERR_SAVE_ACTIVE_WORKFLOW");
				WORKFLOW_LBL_CURRENT_WORKFLOW = GetAndCheckString("WORKFLOW_LBL_CURRENT_WORKFLOW");
				WORKFLOW_LBL_AVL_WORKFLOWS = GetAndCheckString("WORKFLOW_LBL_AVL_WORKFLOWS");
				WORKFLOW_LBL_HELP_CONTENT = GetAndCheckString("WORKFLOW_LBL_HELP_CONTENT");
				WORKFLOW_LBL_HELP = GetAndCheckString("WORKFLOW_LBL_HELP");
				WORKFLOW_BTN_START = GetAndCheckString("WORKFLOW_BTN_START");
				WORKFLOW_BTN_DONE = GetAndCheckString("WORKFLOW_BTN_DONE");
				WORKFLOW_LBL_CANCEL = GetAndCheckString("WORKFLOW_LBL_CANCEL");
				WORKFLOW_LBL_NEXT = GetAndCheckString("WORKFLOW_LBL_NEXT");
				WORKFLOW_LBL_PREVIOUS = GetAndCheckString("WORKFLOW_LBL_PREVIOUS");
				WORKFLOW_LBL_WORKFLOW_STEPS = GetAndCheckString("WORKFLOW_LBL_WORKFLOW_STEPS");
				WORKFLOW_HEADER_LABEL = GetAndCheckString("WORKFLOW_HEADER_LABEL");
				WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART1 = GetAndCheckString("WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART1");
				WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART2 = GetAndCheckString("WORKFLOW_MSGBOX_NO_ACCESS_TO_STEPS_ASK_PART2");
				WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_STEPS = GetAndCheckString("WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_STEPS");
				WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_WORKFLOWS = GetAndCheckString("WORKFLOW_LOGGING_INFO_NO_ACCESS_TO_WORKFLOWS");
				WORKFLOW_LBL_FORCED = GetAndCheckString("WORKFLOW_LBL_FORCED");
				WORKFLOW_LBL_NON_FORCED = GetAndCheckString("WORKFLOW_LBL_NON_FORCED");
				WORKFLOW_ERR_LISTVIEW_UPDATE = GetAndCheckString("WORKFLOW_ERR_LISTVIEW_UPDATE");
				WORKFLOW_ERR_GET_WORKFLOWS = GetAndCheckString("WORKFLOW_ERR_GET_WORKFLOWS");
				DESKTOP_MSG_SQL_EXIST = GetAndCheckString("DESKTOP_MSG_SQL_EXIST");
				WORKFLOW_ERR_AGENTID_SESSIONID_NULL = GetAndCheckString("WORKFLOW_ERR_AGENTID_SESSIONID_NULL");
				COMMON_MSG_IIS_ERROR = GetAndCheckString("COMMON_MSG_IIS_ERROR");
				COMMON_MSGBOX_TITLE_PART1 = GetAndCheckString("COMMON_MSGBOX_TITLE_PART1");
				WORKFLOW_MSGBOX_TITLE_PART2 = GetAndCheckString("WORKFLOW_MSGBOX_TITLE_PART2");
				WORKFLOW_ERR_TRANSFER_WORKFLOW_FAILED  = GetAndCheckString("WORKFLOW_ERR_TRANSFER_WORKFLOW_FAILED");

				// Workflow Driven Implementation:
				WORKFLOW_COMBO_DEFAULT_ITEM = GetAndCheckString("WORKLOW_COMBO_DEFAULT_ITEM");
				// Workflow Driven Implementation:
				WORKFLOW_LBL_DEFAULT = GetAndCheckString("WORKFLOW_LBL_DEFAULT");

				WORKFLOW_DATA_STRUCTURES_WORKFLOW_STEP_ERR_SET_STEP_DATA = GetAndCheckString("WORKFLOW_DATA_STRUCTURES_WORKFLOW_STEP_ERR_SET_STEP_DATA");                

				WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_DATA = GetAndCheckString("WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_DATA");
				WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_STEPS_DATA = GetAndCheckString("WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_SET_WORKFLOW_STEPS_DATA");
				WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA = GetAndCheckString("WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA");
				WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_STATUS_INVALID = GetAndCheckString("WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_STATUS_INVALID");
				WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_WORKFLOW_DATA_IN_XML = GetAndCheckString("WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_ERR_WORKFLOW_DATA_IN_XML");
				WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA_SET_TO_FIRST_STEP = GetAndCheckString("WORKFLOW_DATA_STRUCTURES_WORKFLOW_DATA_SET_PENDING_WORKFLOW_DATA_SET_TO_FIRST_STEP");
			}
			catch ( Exception exp )
			{
				// not localized text since we can't read it from the resource.
				Logging.Error( "CCFDemoApps.Workflow", "Unable to load localized strings", exp );
			}
		}
	}
}
#pragma warning restore 1591