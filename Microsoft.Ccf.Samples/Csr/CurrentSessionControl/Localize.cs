//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// This file contains the Localize information for CurrentSessionControl.
// 
//===============================================================================

using System;
using System.Reflection;
using System.Resources;
using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.CurrentSessionControl
{
	/// <summary>
	/// Summary description for localize
	/// </summary>
	public class localize
	{
		public static string CURRENT_SESSION_LABEL_CURRENT_SESSION = "";
		public static string CURRENT_SESSION_LABEL_CUSTOMER = "";
		public static string CURRENT_SESSION_LABEL_FIRST_NAME = "";
		public static string CURRENT_SESSION_LABEL_LAST_NAME = "";
		public static string CURRENT_SESSION_LABEL_PHONE_NUMBER = "";
		public static string CURRENT_SESSION_LABEL_CURRENT_APPLICATION = "";

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
				rm = new ResourceManager("Microsoft.Ccf.Samples.CurrentSessionControl.Strings", asm); 

				CURRENT_SESSION_LABEL_CURRENT_SESSION = GetAndCheckString("CURRENT_SESSION_LABEL_CURRENT_SESSION");
				CURRENT_SESSION_LABEL_CUSTOMER = GetAndCheckString("CURRENT_SESSION_LABEL_CUSTOMER");
				CURRENT_SESSION_LABEL_FIRST_NAME = GetAndCheckString("CURRENT_SESSION_LABEL_FIRST_NAME");
				CURRENT_SESSION_LABEL_LAST_NAME = GetAndCheckString("CURRENT_SESSION_LABEL_LAST_NAME");
				CURRENT_SESSION_LABEL_PHONE_NUMBER = GetAndCheckString("CURRENT_SESSION_LABEL_PHONE_NUMBER");
				CURRENT_SESSION_LABEL_CURRENT_APPLICATION = GetAndCheckString("CURRENT_SESSION_LABEL_CURRENT_APPLICATION");

			}
			catch ( Exception exp ) 
			{
				// Nothing much can be done if resource library itself cannot be read so leave it in English
				Logging.Error( System.Windows.Forms.Application.ProductName, "Unable to find or load localization DLL strings.", exp );
			}
		}
	}
}
