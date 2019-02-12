//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// Reads the text used in the agent desktop from a localization file.
//
//===============================================================================

using System;
using System.Resources;
using System.Reflection;
using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	public class localize
	{
		// Globalized strings
		public static string APPLICATION_NAME = string.Empty;
		public static string DESKTOP_MSG_ALREADY_RUNNING = string.Empty;
		public static string DESKTOP_MSG_STARTING = string.Empty;
		public static string DESKTOP_ERR_FATAL_ERROR = string.Empty;
		public static string DESKTOP_ERR_FATAL_UNABLE_LOG = string.Empty;
		public static string DESKTOP_MSG_SQL_EXIST = string.Empty;
		public static string DESKTOP_ERR_UNABLE_CONNECT_SQL =  string.Empty;
		public static string COMMON_MSG_IIS_ERROR = string.Empty;
		public static string DESKTOP_IIS_ERROR =string.Empty;
		public static string COMMON_ERR_SQL_CONNECTION = string.Empty;

		public static string DESKTOP_MODULE_NAME = string.Empty;
		public static string DESKTOP_APPSTART_ERROR = string.Empty;
		public static string DESKTOP_APPCREATE_ERROR = string.Empty;
		public static string DESKTOP_EXPLAIN_ERROR = string.Empty;
		public static string DESKTOP_APP_SAVE_ERROR = string.Empty;
		public static string DESKTOP_APP_RESTORE_ERROR = string.Empty;
		public static string DESKTOP_MSG_INVALID_LOGIN_DETAILS = string.Empty;
		public static string DESKTOP_MSG_EMPTY_LOGIN_DETAILS = string.Empty;
		public static string DESKTOP_MSG_WEBSERVICE_CLIENT_MISCONFIGURED = string.Empty;
		public static string DESKTOP_MSG_LOGIN_FAILURE = string.Empty;

		public static string DESKTOP_LOADNONHOSTEDAPP_ERROR = string.Empty;
		public static string DESKTOP_UPDATEHOSTEDAPP_ERROR = string.Empty;
		public static string DESKTOP_TABFOCUS_ERROR = string.Empty;
		public static string DESKTOP_TABAPP_ERROR = string.Empty;
		public static string DESKTOP_MENU_ITEM_TEXT_ABOUT_CONTACT_CENTER_FRAMEWORK = string.Empty;
		public static string DESKTOP_CCF_ABOUT_ERROR = string.Empty;
		public static string DESKTOP_ERR_INIT_SESSION = string.Empty;
		public static string DESKTOP_ERR_UPDATING_CURRENTSESSION = string.Empty;
		public static string DESKTOP_ERR_UNABLE_LOGIN = string.Empty;
		public static string DESKTOP_ERR_UNABLE_ESTABLISH_CONNECTION = string.Empty;
		public static string DESKTOP_ERR_UNABLE_CONFIGURE_PHONE = string.Empty;
		public static string DESKTOP_ERR_INIT_TELEPHONY = string.Empty;
		public static string DESKTOP_ERR_UNABLE_WEBSERVICE = string.Empty;
		public static string DESKTOP_ERR_LOADING_APPLICATIONS = string.Empty;
		public static string DESKTOP_ERR_RTC = string.Empty;
		public static string DESKTOP_ERR_COULDNOT_FIND_APPLICATION = string.Empty;
		public static string DESKTOP_ERR_UNABLE_HOSTEDAPP = string.Empty;
		public static string DESKTOP_ERR_EXCEPTION  = string.Empty; 
		public static string DESKTOP_ERR_UNABLE_MARSHALL  = string.Empty; 
		public static string DESKTOP_ERR_RECD_CALL_EVENT  = string.Empty;
		public static string DESKTOP_MSG_YOU_ARE_ON  = string.Empty;
		public static string DESKTOP_MSG_CALLS  = string.Empty;
		public static string DESKTOP_MSG_ONHOLD  = string.Empty; 
		public static string DESKTOP_MSG_RINGING  = string.Empty;
		public static string DESKTOP_MSG_OUTGOING  = string.Empty;
		public static string DESKTOP_MSG_CALLEDBY  = string.Empty;
		public static string DESKTOP_MSG_CALLWITH  = string.Empty;
		public static string DESKTOP_ERR_IN_CALL_EVENT  = string.Empty; 
		public static string DESKTOP_ERR_UNABLE_REQUEST_ASSISTANCE  = string.Empty; 
		public static string DESKTOP_MSG_UNABLE_FIND_ASSISTANCE  = string.Empty; 
		public static string DESKTOP_MSG_STARTING_IM  = string.Empty; 
		public static string DESKTOP_ERR_UNABLE_TOSTART_APPLICATION  = string.Empty; 
		public static string DESKTOP_ERR_UNABLE_SET_PRESENCE_VALUE  = string.Empty; 
		public static string DESKTOP_ERR_CLICKING_TOOLBAR_BUTTON  = string.Empty; 
		public static string DESKTOP_MSG_NO_CONFIGURED_PHONE  = string.Empty; 
		public static string DESKTOP_ERR_MAKING_CALL = string.Empty;
		public static string DESKTOP_ERR_HANGUP_PHONE = string.Empty;
		public static string DESKTOP_ERR_PLACING_HOLD = string.Empty;
		public static string DESKTOP_ERR_TAKING_CALLOFF_HOLD = string.Empty;
		public static string DESKTOP_ERR_TRANSFERRING_CALL = string.Empty;
		public static string DESKTOP_MSG_UNABLE_CREATE_CONF = string.Empty;
		public static string DESKTOP_ERR_CONFERENCING_CALL = string.Empty;
		public static string DESKTOP_ERR_UNABLE_SET_STATUS = string.Empty;
		public static string DESKTOP_ERR_UNABLE_RESET_CUSTINFO = string.Empty;
		public static string DESKTOP_ERR_SETTING_CONTEXT_APP = string.Empty;
		public static string DESKTOP_ERR_CHANGING_SESSION = string.Empty;
		public static string DESKTOP_ERR_PREFILTERING_MSG = string.Empty;
		public static string DESKTOP_ERR_IN_LOAD = string.Empty;
		public static string DESKTOP_ERR_UPDATING_HOSTED_APP_OR_CS = string.Empty;
		public static string DESKTOP_ERR_UPDATING_CS_WITH_CURR_APP = string.Empty;
		public static string DESKTOP_REQUEST_ASSISTANCE_TEXT = string.Empty;
		public static string DESKTOP_REQUEST_ASSISTANCE_TOOLTIP = string.Empty;
		public static string DESKTOP_PHONE_TEXT = string.Empty;
		public static string DESKTOP_PHONE_TOOLTIP = string.Empty;
		public static string DESKTOP_DIAL_TEXT = string.Empty;
		public static string DESKTOP_ANSWER_TEXT = string.Empty;
		public static string DESKTOP_HANGUP_TEXT = string.Empty;
		public static string DESKTOP_HOLD_TEXT = string.Empty;
		public static string DESKTOP_UNHOLD_TEXT = string.Empty;
		public static string DESKTOP_TRANSFER_TEXT = string.Empty;
		public static string DESKTOP_CONFERENCE_TEXT = string.Empty;
		public static string DESKTOP_LOOKUP_TEXT = string.Empty;
		public static string DESKTOP_LOOKUP_TOOLTIP = string.Empty;
		public static string DESKTOP_APPLICATIONS_TEXT = string.Empty;
		public static string DESKTOP_APPLICATIONS_TOOLTIP = string.Empty;
		public static string DESKTOP_HELP_TEXT = "Help";
		public static string DESKTOP_HELP_TOOLTIP = string.Empty;
		public static string DESKTOP_CLOSE_SESSION_TOOLTIP = string.Empty;
		public static string DESKTOP_DYNAMIC_APPS = string.Empty;
		public static string DESKTOP_DYNAMIC_APPS_TOOLTIP = string.Empty;
		public static string DESKTOP_DYNAMIC_APP_LAUNCH_CONFIRMATION_MSG = string.Empty;
		public static string DESKTOP_UNABLE_TO_CLOSE_DYNAMIC_APP_DUE_TO_WORKFLOW = string.Empty;

		public static string DESKTOP_STATUS_TEXT = string.Empty;
		public static string DESKTOP_STATUS_TOOLTIP = string.Empty;
		public static string DESKTOP_CTI_DEBUG_ENABLED = string.Empty;
		public static string DESKTOP_INFOTEXT_CALLING = string.Empty;
		public static string DESKTOP_GETNUMBER_DIAL = string.Empty;
		public static string DESKTOP_GETNUMBER_CONFERENCE = string.Empty;
		public static string DESKTOP_ERR_LOADING_SESSION_INTO_CS = string.Empty;
		public static string DESKTOP_ERR_BUILDAPPIMAGELIST = string.Empty;
		public static string DESKTOP_ERR_UNABLE_TO_RETRIEVE_WORKFLOW_DATA = string.Empty;
		public static string DESKTOP_ERR_SETUP_MENU = string.Empty;
		public static string DESKTOP_ERR_MODIFY_STATUS_MENU = string.Empty;
		public static string DESKTOP_MODIFY_PHONE_MENU = string.Empty;
		public static string DESKTOP_ERR_REQUESTASSISTANCE = string.Empty;
		public static string DESKTOP_ERR_REQUEST_ASSISTANCE_MESSAGE = string.Empty;
		public static string DESKTOP_ERR_REQUEST_ASSISTANCE_CAPTION = string.Empty;
		public static string DESKTOP_ERR_UPDATING_HOSTEDAPPS = string.Empty;
		public static string DESKTOP_TIMER_TICK_INFO_EXCEPTION = string.Empty;
		public static string DESKTOP_INFO_CALL_LENGTH = string.Empty; 
		public static string DESKTOP_INFO_CALL_LENGTH_PLURAL = string.Empty;
		public static string DESKTOP_INFO_NO_CURRENT_CALL = string.Empty;
		public static string DESKTOP_MESSAGE_BOX_CAPTION = string.Empty;
		public static string DESKTOP_ERR_LOOKUP_CALL = string.Empty;
		public static string DESKTOP_ERR_RETRIEVING_AGENT_PHONE_NUMBER = string.Empty;
		public static string DESKTOP_ERR_TRANSFERRING_SESSION = string.Empty;
		public static string DESKTOP_ERR_RTC_TAPI_CONFLICT = string.Empty;
		public static string DESKTOP_ERR_APP_DETAILS = string.Empty;
		public static string DESKTOP_ERR_UNABLE_TO_READ_OPTIONS = string.Empty;
		public static string DESKTOP_CLOSE_AFTER_TRANSFER = string.Empty;
		public static string DESKTOP_INFO_AGENT_DESKTOP_EXITED=string.Empty;
		public static string DESKTOP_NEW_CUSTOMER=string.Empty;
		public static string DESKTOP_ERR_READING_OR_PARSING_APPSETTINGS = string.Empty;

		public static string SESSION_HAS_A_CONNECTED_CALL = string.Empty;
		public static string SESSION_HAS_A_HELD_CALL= string.Empty;

		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_USERNAME_TEXT = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_PHONE_TEXT = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_OK_TEXT = "Ok";
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_CANCEL_TEXT = "Cancel";
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_FIND_TEXT = "Find";
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_LABEL1_TEXT = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_TEXT = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_NAME_TO_FIND = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_INITIALIZING_ADDR_BOOK = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_READING_ADDR_BOOK = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_SELECT_ADDR = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_NO_ADDR = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_DEPT = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_TITLE = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_LOCATION = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_PHONE = string.Empty;
		public static string AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_EMAIL = string.Empty;

		public static string AGENT_DESKTOP_GET_NUMBER_LABEL1_TEXT = string.Empty;
		public static string AGENT_DESKTOP_GET_NUMBER_OK_TEXT = "Ok";
		public static string AGENT_DESKTOP_GET_NUMBER_CANCEL_TEXT = "Cancel";
		public static string AGENT_DESKTOP_GET_NUMBER_ADDR_BOOK_TEXT = string.Empty;
		public static string AGENT_DESKTOP_GET_NUMBER_ERR_READING_ADDR_BOOK = string.Empty;

		public static string LOOKUP_DLG_UNABLE_CUST_RECORD = string.Empty;
		public static string LOOKUP_DLG_UNABLE_CUST_ID = string.Empty;
		public static string LOOKUP_DLG_UNABLE_CUST_PHONE_NUMBER = string.Empty;
		public static string LOOKUP_DLG_UNABLE_CUST_NAME = string.Empty;
		public static string LOOKUP_DLG_NEED_BOTH_NAMES = string.Empty;
		public static string LOOKUP_DLG_NO_RECORD_SELECTED = string.Empty;
		public static string LOOKUP_DLG_LOOKUP_ID = string.Empty;
		public static string LOOKUP_DLG_LABEL1 = string.Empty;
		public static string LOOKUP_DLG_LABEL4 = string.Empty;
		public static string LOOKUP_DLG_LAST = string.Empty;
		public static string LOOKUP_DLG_FIRST = string.Empty;
		public static string LOOKUP_DLG_HOME_PHONE = string.Empty;
		public static string LOOKUP_DLG_WORK_PHONE = string.Empty;
		public static string LOOKUP_DLG_ADDRESS = string.Empty;
		public static string LOOKUP_DLG_SELECT = "Select";
		public static string LOOKUP_DLG_CANCEL = "Cancel";
		public static string LOOKUP_DLG_ERROR_MSG = string.Empty;
		public static string LOOKUP_DLG_LBL5 = string.Empty;
		public static string LOOKUP_DLG_LBL6 = string.Empty;
		public static string LOOKUP_DLG_BTN_NEW = "New";
		public static string LOOKUP_DLG_LOOKUP_NAME = string.Empty;
		public static string LOOKUP_DLG_LOOKUP_PHONE = string.Empty;
		public static string LOOKUP_DLG_LABEL7 = string.Empty;
		public static string LOOKUP_DLG_LBEL8 = string.Empty;
		public static string LOOKUP_DLG_TEXT = string.Empty;

		public static string SELECT_CALL_DLG_STARTED_COL = string.Empty;
		public static string SELECT_CALL_DLG_STATE_COL = string.Empty;
		public static string SELECT_CALL_DLG_PARTIES_COL = string.Empty;
		public static string SELECT_CALL_DLG_BTN_OK = "Ok";
		public static string SELECT_CALL_DLG_BTN_CANCEL = "Cancel";
		public static string SELECT_CALL_DLG_LABEL1 = string.Empty;
		public static string SELECT_CALL_DLG_TEXT = string.Empty;
		public static string SELECT_CALL_DLG_SELECT_A_CALL=string.Empty;

		public static string SESSION_EXPLORER_MODULE_NAME = "Session Explorer";
		public static string SESSION_EXPLORER_SESSIONS_COUNT = string.Empty;
		public static string SESSION_EXPLORER_STR_SESSION_EXP_MSGBOX_CAPTION = "Sessions";
		public static string SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG1 = string.Empty;
		public static string SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG2 = string.Empty;
		public static string SESSION_EXPLORER_STR_SORTBY_CUSTOMER_NAME = string.Empty;
		public static string SESSION_EXPLORER_STR_SORTBY_CALL_DATETIME = string.Empty;
		public static string SESSION_EXPLORER_LABEL_SORT_BY_TEXT = "Sort by";
		public static string SESSION_EXPLORER_STR_HELP_STRING1 = string.Empty;
		public static string SESSION_EXPLORER_STR_HELP_STRING2 = string.Empty;
		public static string SESSION_EXPLORER_ERR_DISPLAYING_HELP = string.Empty;
		public static string SESSION_EXPLORER_LABEL_HELP_ICON_TEXT = "Help";
		public static string SESSION_EXPLORER_CLOSE_SESSION = "Close Session";

		public static string CCF_ABOUT_VERSION = string.Empty;
		public static string CCF_ABOUT_DOTNET_VERSION = string.Empty;
		public static string CCF_ABOUT_CONTACT_CENTER_FRAMEWORK = string.Empty;
		public static string CCF_ABOUT_COPYRIGHT = string.Empty;

		public static string DESKTOP_VERSIONS_RETRIEVAL_ERROR = string.Empty;
		public static string DESKTOP_WS_VERSION_MISMATCH_ERROR = string.Empty;
		public static string DESKTOP_DB_VERSION_MISMATCH_ERROR = string.Empty;
		public static string DESKTOP_UNKNOWN_VERSION = string.Empty;

		public static string DESKTOP_AGENT_DESKTOP_VERSION = string.Empty;
		public static string DESKTOP_WEB_SERVICES_VERSION = string.Empty;
		public static string DESKTOP_DATABASES_VERSION = string.Empty;
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
				Logging.Error( System.Windows.Forms.Application.ProductName,
					String.Format( "Unable to find string {0} in resource file.", name ) );
				val = name;
			}

			return val;
		}

		static localize()
		{
			try
			{
				Assembly asm = Assembly.GetExecutingAssembly(); 
				rm = new ResourceManager("Microsoft.Ccf.Samples.Csr.AgentDesktop.Strings", asm); 

				APPLICATION_NAME = GetAndCheckString("APPLICATION_NAME");
				DESKTOP_MESSAGE_BOX_CAPTION = GetAndCheckString("DESKTOP_MESSAGE_BOX_CAPTION");
				DESKTOP_MODULE_NAME = GetAndCheckString("DESKTOP_MODULE_NAME");
				DESKTOP_APPSTART_ERROR = GetAndCheckString("DESKTOP_APPSTART_ERROR");
				DESKTOP_APPCREATE_ERROR = GetAndCheckString("DESKTOP_APPCREATE_ERROR");
				DESKTOP_EXPLAIN_ERROR = GetAndCheckString("DESKTOP_EXPLAIN_ERROR");
				DESKTOP_APP_SAVE_ERROR = GetAndCheckString("DESKTOP_APP_SAVE_ERROR");
				DESKTOP_APP_RESTORE_ERROR = GetAndCheckString("DESKTOP_APP_RESTORE_ERROR");
				DESKTOP_MSG_LOGIN_FAILURE = GetAndCheckString("DESKTOP_MSG_LOGIN_FAILURE ");
				DESKTOP_MSG_INVALID_LOGIN_DETAILS = GetAndCheckString("DESKTOP_MSG_INVALID_LOGIN_DETAILS");
				DESKTOP_MSG_EMPTY_LOGIN_DETAILS = GetAndCheckString("DESKTOP_MSG_EMPTY_LOGIN_DETAILS");
				DESKTOP_MSG_WEBSERVICE_CLIENT_MISCONFIGURED = GetAndCheckString("DESKTOP_MSG_WEBSERVICE_CLIENT_MISCONFIGURED");
				DESKTOP_LOADNONHOSTEDAPP_ERROR = GetAndCheckString("DESKTOP_LOADNONHOSTEDAPP_ERROR");
				DESKTOP_UPDATEHOSTEDAPP_ERROR = GetAndCheckString("DESKTOP_UPDATEHOSTEDAPP_ERROR");
				DESKTOP_TABFOCUS_ERROR = GetAndCheckString("DESKTOP_TABFOCUS_ERROR");
				DESKTOP_TABAPP_ERROR = GetAndCheckString("DESKTOP_TABAPP_ERROR");

				DESKTOP_MENU_ITEM_TEXT_ABOUT_CONTACT_CENTER_FRAMEWORK = GetAndCheckString("DESKTOP_MENU_ITEM_TEXT_ABOUT_CONTACT_CENTER_FRAMEWORK");
				DESKTOP_CCF_ABOUT_ERROR = GetAndCheckString("DESKTOP_CCF_ABOUT_ERROR");
				DESKTOP_ERR_INIT_SESSION = GetAndCheckString("DESKTOP_ERR_INIT_SESSION");
				DESKTOP_ERR_UPDATING_CURRENTSESSION = GetAndCheckString("DESKTOP_ERR_UPDATING_CURRENTSESSION");
				DESKTOP_MSG_ALREADY_RUNNING = GetAndCheckString("DESKTOP_MSG_ALREADY_RUNNING");
				DESKTOP_MSG_STARTING = GetAndCheckString("DESKTOP_MSG_STARTING");
				DESKTOP_ERR_FATAL_ERROR = GetAndCheckString("DESKTOP_ERR_FATAL_ERROR");
				DESKTOP_ERR_FATAL_UNABLE_LOG = GetAndCheckString("DESKTOP_ERR_FATAL_UNABLE_LOG");
				DESKTOP_MSG_SQL_EXIST = GetAndCheckString("DESKTOP_MSG_SQL_EXIST");
				DESKTOP_ERR_UNABLE_CONNECT_SQL = GetAndCheckString("DESKTOP_ERR_UNABLE_CONNECT_SQL");
				DESKTOP_ERR_UNABLE_LOGIN = GetAndCheckString("DESKTOP_ERR_UNABLE_LOGIN");
				DESKTOP_ERR_UNABLE_ESTABLISH_CONNECTION = GetAndCheckString("DESKTOP_ERR_UNABLE_ESTABLISH_CONNECTION");
				DESKTOP_ERR_UNABLE_CONFIGURE_PHONE = GetAndCheckString("DESKTOP_ERR_UNABLE_CONFIGURE_PHONE");
				DESKTOP_ERR_INIT_TELEPHONY = GetAndCheckString("DESKTOP_ERR_INIT_TELEPHONY");
				DESKTOP_ERR_UNABLE_WEBSERVICE = GetAndCheckString("DESKTOP_ERR_UNABLE_WEBSERVICE");
				DESKTOP_ERR_RTC = GetAndCheckString("DESKTOP_ERR_RTC");
				DESKTOP_ERR_COULDNOT_FIND_APPLICATION = GetAndCheckString("DESKTOP_ERR_COULDNOT_FIND_APPLICATION");
				DESKTOP_ERR_UNABLE_HOSTEDAPP = GetAndCheckString("DESKTOP_ERR_UNABLE_HOSTEDAPP");
				DESKTOP_ERR_EXCEPTION  = GetAndCheckString("DESKTOP_ERR_EXCEPTION"); 
				DESKTOP_ERR_UNABLE_MARSHALL  = GetAndCheckString("DESKTOP_ERR_UNABLE_MARSHALL"); 
				DESKTOP_ERR_RECD_CALL_EVENT  = GetAndCheckString("DESKTOP_ERR_RECD_CALL_EVENT");
				DESKTOP_MSG_YOU_ARE_ON  = GetAndCheckString("DESKTOP_MSG_YOU_ARE_ON");
				DESKTOP_MSG_CALLS  = GetAndCheckString("DESKTOP_MSG_CALLS");
				DESKTOP_MSG_ONHOLD  = GetAndCheckString("DESKTOP_MSG_ONHOLD"); 
				DESKTOP_MSG_RINGING  = GetAndCheckString("DESKTOP_MSG_RINGING");
				DESKTOP_MSG_OUTGOING  = GetAndCheckString("DESKTOP_MSG_OUTGOING");
				DESKTOP_MSG_CALLEDBY  = GetAndCheckString("DESKTOP_MSG_CALLEDBY");
				DESKTOP_MSG_CALLWITH  = GetAndCheckString("DESKTOP_MSG_CALLWITH");
				DESKTOP_ERR_IN_CALL_EVENT  = GetAndCheckString("DESKTOP_ERR_IN_CALL_EVENT"); 
				DESKTOP_ERR_UNABLE_REQUEST_ASSISTANCE  = GetAndCheckString("DESKTOP_ERR_UNABLE_REQUEST_ASSISTANCE"); 
				DESKTOP_MSG_UNABLE_FIND_ASSISTANCE  = GetAndCheckString("DESKTOP_MSG_UNABLE_FIND_ASSISTANCE"); 
				DESKTOP_MSG_STARTING_IM  = GetAndCheckString("DESKTOP_MSG_STARTING_IM"); 
				DESKTOP_ERR_UNABLE_TOSTART_APPLICATION  = GetAndCheckString("DESKTOP_ERR_UNABLE_TOSTART_APPLICATION"); 
				DESKTOP_ERR_UNABLE_SET_PRESENCE_VALUE  = GetAndCheckString("DESKTOP_ERR_UNABLE_SET_PRESENCE_VALUE"); 
				DESKTOP_ERR_CLICKING_TOOLBAR_BUTTON  = GetAndCheckString("DESKTOP_ERR_CLICKING_TOOLBAR_BUTTON"); 
				DESKTOP_MSG_NO_CONFIGURED_PHONE  = GetAndCheckString("DESKTOP_MSG_NO_CONFIGURED_PHONE"); 
				DESKTOP_ERR_MAKING_CALL = GetAndCheckString("DESKTOP_ERR_MAKING_CALL");
				DESKTOP_ERR_HANGUP_PHONE = GetAndCheckString("DESKTOP_ERR_HANGUP_PHONE");
				DESKTOP_ERR_PLACING_HOLD = GetAndCheckString("DESKTOP_ERR_PLACING_HOLD");
				DESKTOP_ERR_TAKING_CALLOFF_HOLD = GetAndCheckString("DESKTOP_ERR_TAKING_CALLOFF_HOLD");
				DESKTOP_ERR_TRANSFERRING_CALL = GetAndCheckString("DESKTOP_ERR_TRANSFERRING_CALL");
				DESKTOP_MSG_UNABLE_CREATE_CONF = GetAndCheckString("DESKTOP_MSG_UNABLE_CREATE_CONF");
				DESKTOP_ERR_CONFERENCING_CALL = GetAndCheckString("DESKTOP_ERR_CONFERENCING_CALL");
				DESKTOP_ERR_UNABLE_SET_STATUS = GetAndCheckString("DESKTOP_ERR_UNABLE_SET_STATUS");
				DESKTOP_ERR_UNABLE_RESET_CUSTINFO = GetAndCheckString("DESKTOP_ERR_UNABLE_RESET_CUSTINFO");
				DESKTOP_ERR_SETTING_CONTEXT_APP = GetAndCheckString("DESKTOP_ERR_SETTING_CONTEXT_APP");
				DESKTOP_ERR_CHANGING_SESSION = GetAndCheckString("DESKTOP_ERR_CHANGING_SESSION");
				DESKTOP_ERR_LOADING_APPLICATIONS = GetAndCheckString("DESKTOP_ERR_LOADING_APPLICATIONS");
				DESKTOP_ERR_PREFILTERING_MSG = GetAndCheckString("DESKTOP_ERR_PREFILTERING_MSG");
				DESKTOP_ERR_IN_LOAD = GetAndCheckString("DESKTOP_ERR_IN_LOAD");
				DESKTOP_ERR_UPDATING_HOSTED_APP_OR_CS = GetAndCheckString("DESKTOP_ERR_UPDATING_HOSTED_APP_OR_CS");
				DESKTOP_ERR_UPDATING_CS_WITH_CURR_APP = GetAndCheckString("DESKTOP_ERR_UPDATING_CS_WITH_CURR_APP");
				DESKTOP_REQUEST_ASSISTANCE_TEXT = GetAndCheckString("DESKTOP_REQUEST_ASSISTANCE_TEXT");
				DESKTOP_REQUEST_ASSISTANCE_TOOLTIP = GetAndCheckString("DESKTOP_REQUEST_ASSISTANCE_TOOLTIP");
				DESKTOP_PHONE_TEXT = GetAndCheckString("DESKTOP_PHONE_TEXT");
				DESKTOP_PHONE_TOOLTIP = GetAndCheckString("DESKTOP_PHONE_TOOLTIP");
				DESKTOP_DIAL_TEXT = GetAndCheckString("DESKTOP_DIAL_TEXT");
				DESKTOP_ANSWER_TEXT = GetAndCheckString("DESKTOP_ANSWER_TEXT");
				DESKTOP_HANGUP_TEXT = GetAndCheckString("DESKTOP_HANGUP_TEXT");
				DESKTOP_HOLD_TEXT = GetAndCheckString("DESKTOP_HOLD_TEXT");
				DESKTOP_UNHOLD_TEXT = GetAndCheckString("DESKTOP_UNHOLD_TEXT");
				DESKTOP_TRANSFER_TEXT = GetAndCheckString("DESKTOP_TRANSFER_TEXT");
				DESKTOP_CONFERENCE_TEXT = GetAndCheckString("DESKTOP_CONFERENCE_TEXT");
				DESKTOP_LOOKUP_TEXT = GetAndCheckString("DESKTOP_LOOKUP_TEXT");
				DESKTOP_LOOKUP_TOOLTIP = GetAndCheckString("DESKTOP_LOOKUP_TOOLTIP");
				DESKTOP_APPLICATIONS_TEXT = GetAndCheckString("DESKTOP_APPLICATIONS_TEXT");
				DESKTOP_APPLICATIONS_TOOLTIP = GetAndCheckString("DESKTOP_APPLICATIONS_TOOLTIP");
				DESKTOP_HELP_TEXT = GetAndCheckString("DESKTOP_HELP_TEXT");
				DESKTOP_HELP_TOOLTIP = GetAndCheckString("DESKTOP_HELP_TOOLTIP");
				DESKTOP_DYNAMIC_APPS = GetAndCheckString("DESKTOP_DYNAMIC_APPS");
				DESKTOP_DYNAMIC_APPS_TOOLTIP = GetAndCheckString("DESKTOP_DYNAMIC_APPS_TOOLTIP");
				DESKTOP_CLOSE_SESSION_TOOLTIP = GetAndCheckString("DESKTOP_CLOSE_SESSION_TOOLTIP");
				DESKTOP_INFO_AGENT_DESKTOP_EXITED = GetAndCheckString("DESKTOP_INFO_AGENT_DESKTOP_EXITED");
				DESKTOP_NEW_CUSTOMER = GetAndCheckString("DESKTOP_NEW_CUSTOMER");
				DESKTOP_DYNAMIC_APP_LAUNCH_CONFIRMATION_MSG = GetAndCheckString("DESKTOP_DYNAMIC_APP_LAUNCH_CONFIRMATION_MSG");
				DESKTOP_UNABLE_TO_CLOSE_DYNAMIC_APP_DUE_TO_WORKFLOW = GetAndCheckString("DESKTOP_UNABLE_TO_CLOSE_DYNAMIC_APP_DUE_TO_WORKFLOW");

				DESKTOP_STATUS_TEXT = GetAndCheckString("DESKTOP_STATUS_TEXT");
				DESKTOP_STATUS_TOOLTIP = GetAndCheckString("DESKTOP_STATUS_TOOLTIP");
				DESKTOP_CTI_DEBUG_ENABLED = GetAndCheckString("DESKTOP_CTI_DEBUG_ENABLED");
				DESKTOP_INFOTEXT_CALLING = GetAndCheckString("DESKTOP_INFOTEXT_CALLING");
				DESKTOP_GETNUMBER_DIAL = GetAndCheckString("DESKTOP_GETNUMBER_DIAL");
				DESKTOP_GETNUMBER_CONFERENCE = GetAndCheckString("DESKTOP_GETNUMBER_CONFERENCE");
				DESKTOP_ERR_LOADING_SESSION_INTO_CS = GetAndCheckString("DESKTOP_ERR_LOADING_SESSION_INTO_CS");
				DESKTOP_ERR_BUILDAPPIMAGELIST = GetAndCheckString("DESKTOP_ERR_BUILDAPPIMAGELIST");
				DESKTOP_ERR_UNABLE_TO_RETRIEVE_WORKFLOW_DATA = GetAndCheckString("DESKTOP_ERR_UNABLE_TO_RETRIEVE_WORKFLOW_DATA");
				DESKTOP_ERR_SETUP_MENU = GetAndCheckString("DESKTOP_ERR_SETUP_MENU");
				DESKTOP_ERR_MODIFY_STATUS_MENU = GetAndCheckString("DESKTOP_ERR_MODIFY_STATUS_MENU");
				DESKTOP_MODIFY_PHONE_MENU = GetAndCheckString("DESKTOP_MODIFY_PHONE_MENU");
				COMMON_MSG_IIS_ERROR = GetAndCheckString("COMMON_MSG_IIS_ERROR");
				DESKTOP_IIS_ERROR = GetAndCheckString("DESKTOP_IIS_ERROR");
				DESKTOP_ERR_REQUESTASSISTANCE = GetAndCheckString("DESKTOP_ERR_REQUESTASSISTANCE");
				DESKTOP_ERR_REQUEST_ASSISTANCE_MESSAGE = GetAndCheckString("DESKTOP_ERR_REQUEST_ASSISTANCE_MESSAGE");
				DESKTOP_ERR_REQUEST_ASSISTANCE_CAPTION = GetAndCheckString("DESKTOP_ERR_REQUEST_ASSISTANCE_CAPTION");
				DESKTOP_ERR_UPDATING_HOSTEDAPPS = GetAndCheckString("DESKTOP_ERR_UPDATING_HOSTEDAPPS");
				DESKTOP_TIMER_TICK_INFO_EXCEPTION = GetAndCheckString("DESKTOP_TIMER_TICK_INFO_EXCEPTION");
				DESKTOP_INFO_CALL_LENGTH = GetAndCheckString("DESKTOP_INFO_CALL_LENGTH"); 
				DESKTOP_INFO_CALL_LENGTH_PLURAL = GetAndCheckString("DESKTOP_INFO_CALL_LENGTH_PLURAL");
				DESKTOP_INFO_NO_CURRENT_CALL = GetAndCheckString("DESKTOP_INFO_NO_CURRENT_CALL");
				COMMON_ERR_SQL_CONNECTION = GetAndCheckString("COMMON_ERR_SQL_CONNECTION");
				DESKTOP_ERR_LOOKUP_CALL = GetAndCheckString("DESKTOP_ERR_LOOKUP_CALL");
				DESKTOP_ERR_RETRIEVING_AGENT_PHONE_NUMBER = GetAndCheckString("DESKTOP_ERR_RETRIEVING_AGENT_PHONE_NUMBER");
				DESKTOP_ERR_TRANSFERRING_SESSION = GetAndCheckString("DESKTOP_ERR_TRANSFERRING_SESSION");
				DESKTOP_ERR_RTC_TAPI_CONFLICT = GetAndCheckString("DESKTOP_ERR_RTC_TAPI_CONFLICT");
				DESKTOP_ERR_APP_DETAILS = GetAndCheckString("DESKTOP_ERR_APP_DETAILS");
				DESKTOP_ERR_UNABLE_TO_READ_OPTIONS = GetAndCheckString("DESKTOP_ERR_UNABLE_TO_READ_OPTIONS");
				DESKTOP_ERR_READING_OR_PARSING_APPSETTINGS = GetAndCheckString("DESKTOP_ERR_READING_OR_PARSING_APPSETTINGS");

				SESSION_HAS_A_CONNECTED_CALL = GetAndCheckString("SESSION_HAS_A_CONNECTED_CALL" );
				SESSION_HAS_A_HELD_CALL = GetAndCheckString("SESSION_HAS_A_HELD_CALL" );

				AGENT_DESKTOP_ADDR_BOOK_DLG_USERNAME_TEXT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_USERNAME_TEXT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_PHONE_TEXT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_PHONE_TEXT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_OK_TEXT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_OK_TEXT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_CANCEL_TEXT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_CANCEL_TEXT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_FIND_TEXT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_FIND_TEXT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_LABEL1_TEXT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_LABEL1_TEXT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_TEXT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_TEXT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_NAME_TO_FIND = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_NAME_TO_FIND");
				AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_INITIALIZING_ADDR_BOOK = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_INITIALIZING_ADDR_BOOK");
				AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_READING_ADDR_BOOK = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_READING_ADDR_BOOK");
				AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_SELECT_ADDR = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_SELECT_ADDR");
				AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_NO_ADDR = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_NO_ADDR");
				AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_DEPT = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_DEPT");
				AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_TITLE = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_TITLE");
				AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_LOCATION = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_LOCATION");
				AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_PHONE = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_PHONE");
				AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_EMAIL = GetAndCheckString("AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_EMAIL");

				AGENT_DESKTOP_GET_NUMBER_LABEL1_TEXT = GetAndCheckString("AGENT_DESKTOP_GET_NUMBER_LABEL1_TEXT");
				AGENT_DESKTOP_GET_NUMBER_OK_TEXT = GetAndCheckString("AGENT_DESKTOP_GET_NUMBER_OK_TEXT");
				AGENT_DESKTOP_GET_NUMBER_CANCEL_TEXT = GetAndCheckString("AGENT_DESKTOP_GET_NUMBER_CANCEL_TEXT");
				AGENT_DESKTOP_GET_NUMBER_ADDR_BOOK_TEXT = GetAndCheckString("AGENT_DESKTOP_GET_NUMBER_ADDR_BOOK_TEXT");
				AGENT_DESKTOP_GET_NUMBER_ERR_READING_ADDR_BOOK = GetAndCheckString("AGENT_DESKTOP_GET_NUMBER_ERR_READING_ADDR_BOOK");

				LOOKUP_DLG_UNABLE_CUST_RECORD = GetAndCheckString("LOOKUP_DLG_UNABLE_CUST_RECORD");
				LOOKUP_DLG_UNABLE_CUST_ID = GetAndCheckString("LOOKUP_DLG_UNABLE_CUST_ID");
				LOOKUP_DLG_UNABLE_CUST_PHONE_NUMBER = GetAndCheckString("LOOKUP_DLG_UNABLE_CUST_PHONE_NUMBER");
				LOOKUP_DLG_UNABLE_CUST_NAME = GetAndCheckString("LOOKUP_DLG_UNABLE_CUST_NAME");
				// Post 1.02 patch 3
				LOOKUP_DLG_NEED_BOTH_NAMES = GetAndCheckString("LOOKUP_DLG_NEED_BOTH_NAMES");
				LOOKUP_DLG_NO_RECORD_SELECTED  = GetAndCheckString("LOOKUP_DLG_NO_RECORD_SELECTED");
				LOOKUP_DLG_LOOKUP_ID = GetAndCheckString("LOOKUP_DLG_LOOKUP_ID");
				LOOKUP_DLG_LABEL1  = GetAndCheckString("LOOKUP_DLG_LABEL1");
				LOOKUP_DLG_LABEL4  = GetAndCheckString("LOOKUP_DLG_LABEL4");
				LOOKUP_DLG_LAST  = GetAndCheckString("LOOKUP_DLG_LAST");
				LOOKUP_DLG_FIRST  = GetAndCheckString("LOOKUP_DLG_FIRST");
				LOOKUP_DLG_HOME_PHONE  = GetAndCheckString("LOOKUP_DLG_HOME_PHONE");
				LOOKUP_DLG_WORK_PHONE  = GetAndCheckString("LOOKUP_DLG_WORK_PHONE");
				LOOKUP_DLG_ADDRESS  = GetAndCheckString("LOOKUP_DLG_ADDRESS");
				LOOKUP_DLG_SELECT  = GetAndCheckString("LOOKUP_DLG_SELECT");
				LOOKUP_DLG_CANCEL  = GetAndCheckString("LOOKUP_DLG_CANCEL");
				LOOKUP_DLG_ERROR_MSG  = GetAndCheckString("LOOKUP_DLG_ERROR_MSG");
				LOOKUP_DLG_LBL5  = GetAndCheckString("LOOKUP_DLG_LBL5");
				LOOKUP_DLG_LBL6  = GetAndCheckString("LOOKUP_DLG_LBL6");
				LOOKUP_DLG_BTN_NEW  = GetAndCheckString("LOOKUP_DLG_BTN_NEW");
				LOOKUP_DLG_LOOKUP_NAME  = GetAndCheckString("LOOKUP_DLG_LOOKUP_NAME");
				LOOKUP_DLG_LOOKUP_PHONE  = GetAndCheckString("LOOKUP_DLG_LOOKUP_PHONE");
				LOOKUP_DLG_LABEL7  = GetAndCheckString("LOOKUP_DLG_LABEL7");
				LOOKUP_DLG_LBEL8  = GetAndCheckString("LOOKUP_DLG_LBEL8");
				LOOKUP_DLG_TEXT  = GetAndCheckString("LOOKUP_DLG_TEXT");

				SELECT_CALL_DLG_STARTED_COL = GetAndCheckString("SELECT_CALL_DLG_STARTED_COL");
				SELECT_CALL_DLG_STATE_COL = GetAndCheckString("SELECT_CALL_DLG_STATE_COL");
				SELECT_CALL_DLG_PARTIES_COL = GetAndCheckString("SELECT_CALL_DLG_PARTIES_COL");
				SELECT_CALL_DLG_BTN_OK = GetAndCheckString("SELECT_CALL_DLG_BTN_OK");
				SELECT_CALL_DLG_BTN_CANCEL = GetAndCheckString("SELECT_CALL_DLG_BTN_CANCEL");
				SELECT_CALL_DLG_LABEL1 = GetAndCheckString("SELECT_CALL_DLG_LABEL1");
				SELECT_CALL_DLG_TEXT = GetAndCheckString("SELECT_CALL_DLG_TEXT");
				SELECT_CALL_DLG_SELECT_A_CALL = GetAndCheckString("SELECT_CALL_DLG_SELECT_A_CALL");

				SESSION_EXPLORER_MODULE_NAME = GetAndCheckString("SESSION_EXPLORER_MODULE_NAME");
				SESSION_EXPLORER_SESSIONS_COUNT = GetAndCheckString("SESSION_EXPLORER_SESSIONS_COUNT");
				SESSION_EXPLORER_STR_SESSION_EXP_MSGBOX_CAPTION = GetAndCheckString("SESSION_EXPLORER_STR_SESSION_EXP_MSGBOX_CAPTION");
				SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG1 = GetAndCheckString("SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG1");
				SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG2 = GetAndCheckString("SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG2");
				SESSION_EXPLORER_STR_SORTBY_CALL_DATETIME = GetAndCheckString("SESSION_EXPLORER_STR_SORTBY_CALL_DATETIME");
				SESSION_EXPLORER_STR_SORTBY_CUSTOMER_NAME = GetAndCheckString("SESSION_EXPLORER_STR_SORTBY_CUSTOMER_NAME");
				SESSION_EXPLORER_STR_HELP_STRING1 = GetAndCheckString("SESSION_EXPLORER_STR_HELP_STRING1");
				SESSION_EXPLORER_STR_HELP_STRING2 = GetAndCheckString("SESSION_EXPLORER_STR_HELP_STRING2");
				SESSION_EXPLORER_LABEL_SORT_BY_TEXT = GetAndCheckString("SESSION_EXPLORER_LABEL_SORT_BY_TEXT");
				SESSION_EXPLORER_ERR_DISPLAYING_HELP = GetAndCheckString("SESSION_EXPLORER_ERR_DISPLAYING_HELP");
				SESSION_EXPLORER_LABEL_HELP_ICON_TEXT = GetAndCheckString("SESSION_EXPLORER_LABEL_HELP_ICON_TEXT");
				SESSION_EXPLORER_CLOSE_SESSION = GetAndCheckString("SESSION_EXPLORER_CLOSE_SESSION");

				DESKTOP_CLOSE_AFTER_TRANSFER = GetAndCheckString("DESKTOP_CLOSE_AFTER_TRANSFER");

				CCF_ABOUT_CONTACT_CENTER_FRAMEWORK = GetAndCheckString("CCF_ABOUT_CONTACT_CENTER_FRAMEWORK");
				CCF_ABOUT_DOTNET_VERSION = GetAndCheckString("CCF_ABOUT_DOTNET_VERSION");
				CCF_ABOUT_VERSION = GetAndCheckString("CCF_ABOUT_VERSION");
				CCF_ABOUT_COPYRIGHT = GetAndCheckString("CCF_ABOUT_COPYRIGHT");

				DESKTOP_VERSIONS_RETRIEVAL_ERROR = GetAndCheckString("DESKTOP_VERSIONS_RETRIEVAL_ERROR");
				DESKTOP_WS_VERSION_MISMATCH_ERROR = GetAndCheckString("DESKTOP_WS_VERSION_MISMATCH_ERROR");
				DESKTOP_DB_VERSION_MISMATCH_ERROR = GetAndCheckString("DESKTOP_DB_VERSION_MISMATCH_ERROR");

				DESKTOP_AGENT_DESKTOP_VERSION = GetAndCheckString("DESKTOP_AGENT_DESKTOP_VERSION");
				DESKTOP_WEB_SERVICES_VERSION = GetAndCheckString("DESKTOP_WEB_SERVICES_VERSION");
				DESKTOP_DATABASES_VERSION = GetAndCheckString("DESKTOP_DATABASES_VERSION");
				DESKTOP_UNKNOWN_VERSION = GetAndCheckString("DESKTOP_UNKNOWN_VERSION");
			}
			catch ( Exception exp ) 
			{
				// Nothing much can be done if resource library itself cannot be read so leave it in English
				Logging.Error( System.Windows.Forms.Application.ProductName, "Unable to find or load localization DLL strings.", exp );
			}
		}
	}
}