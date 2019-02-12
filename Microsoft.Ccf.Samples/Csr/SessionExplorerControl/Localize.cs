//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// This file contains the Localize information for SessionExplorerControl.
// 
//===============================================================================

using System;
using System.Reflection;
using System.Resources;
using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.SessionExplorerControl
{
	/// <summary>
	/// Summary description for localize
	/// </summary>
	public class localize
	{
		public static string SESSION_EXPLORER_MODULE_NAME = "Session Explorer";
		public static string SESSION_EXPLORER_SESSIONS_COUNT = "";
		public static string SESSION_EXPLORER_STR_SESSION_EXP_MSGBOX_CAPTION = "Sessions";
		public static string SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG1 = "";
		public static string SESSION_EXPLORER_STR_WORKFLOW_PENDING_MSG2 = "";
		public static string SESSION_EXPLORER_STR_SORTBY_CUSTOMER_NAME = "";
		public static string SESSION_EXPLORER_STR_SORTBY_CALL_DATETIME = "";
		public static string SESSION_EXPLORER_LABEL_SORT_BY_TEXT = "Sort by";
		public static string SESSION_EXPLORER_STR_HELP_STRING1 = "";
		public static string SESSION_EXPLORER_STR_HELP_STRING2 = "";
		public static string SESSION_EXPLORER_ERR_DISPLAYING_HELP = "";
		public static string SESSION_EXPLORER_LABEL_HELP_ICON_TEXT = "Help";
		public static string SESSION_EXPLORER_CLOSE_SESSION = "Close Session";

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
				//V2.5 new localization approach.
				Assembly asm = Assembly.GetExecutingAssembly(); 
				rm = new ResourceManager("Microsoft.Ccf.Samples.SessionExplorerControl.Strings", asm); 

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

			}
			catch ( Exception exp ) 
			{
				// Nothing much can be done if resource library itself cannot be read so leave it in English
				Logging.Error( System.Windows.Forms.Application.ProductName, "Unable to find or load localization DLL strings.", exp );
			}
		}
	}
}
