//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// RtcLib.cs
//
// Revisions:
//     May 2003      v1.0  release
//     December 2004 v1.02 release
//
//===============================================================================

using System;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using RTCCORELib;

using Microsoft.Ccf.Csr.Rtc;
using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.Csr.RtcLib
{
	/// <summary>
	/// Summary description for RtcLib.
	/// </summary>
	public class RtcLib
	{
		/// <summary>
		/// RTC variables
		/// </summary>
		private Microsoft.Ccf.Csr.Rtc.RtcClient rtcClient;

		public RtcLib(RTC_SESSION_TYPE sessionType, string userUri, string serverUri, int transport)
		{
			// Initialize RTC Client
			rtcClient = new Microsoft.Ccf.Csr.Rtc.RtcClient(sessionType, userUri, serverUri, transport);
			rtcClient.IncomingSession += new  Microsoft.Ccf.Csr.Rtc.SessionEventHandler(this.IncomingSession);
			rtcClient.Listen();
		}

		public void Close()
		{
			if ( null != rtcClient )
			{
				rtcClient.Terminate();
				rtcClient = null;
			}
		}


		public void StartConversation( string sipAddress, string firstName, string lastName, string subject, string context )
		{
			if ( sipAddress != null && sipAddress.StartsWith( "sip:" ) )
			{
				RtcDlg dlg = new RtcDlg( rtcClient, sipAddress, firstName, lastName, subject, context );
				if ( !RtcDlg.ExistingRequest )
				{
					// v1.01 made this a ShowDialog() but that is broken because the
					// agent needs to handle IM and other things at the same time.  Using
					// a modal dialog here is incorrect.  This was fixed again in v1.02
					dlg.Show();
				}
				else
				{
					MessageBox.Show( localize.RTCLIB_TEXTMSG_UNABLE, Application.ProductName );
				}
			}
		}

		private void IncomingSession(object sender, SessionEventArgs e)
		{
			//TODO: if ( null != activeSession )
			//{
			//        statusBar.Text = "Incoming Session ignored";
			//        e.Session.Terminate( TerminationReason.Busy );
			//}
			//      else
			RtcDlg dlg = new RtcDlg( rtcClient, e.Session );
			dlg.Show();
		}
	}

	// v1.02 added this so the strings don't get inited for every external app and
	// also to place the code in one single location.
	/// <summary>    
	/// Contains strings in the appropriate languge for the UI.
	/// </summary>
	class localize
	{
		public static string RTCLIB_TEXTMSG_UNABLE = "";

		internal static string RTCDLG_MSG_ANSWERING = "";
		internal static string RTCDLG_TEXTMSG_ASSISTANCE = "";
		internal static string RTCDLG_TEXTMSG_TO = "";
		internal static string RTCDLG_TEXTMSG_CONVERSATION_CLOSED = "";
		internal static string RTCDLG_TEXTMSG_HELPREQUEST = "";
		internal static string RTCDLG_TEXTMSG_SAYS = "";
		internal static string RTCDLG_ERRMSG_EXCEPTION = "";
		internal static string RTCDLG_MSGBOX_MSG_UNABLE_REACH = "";
		internal static string RTCDLG_TEXTMSG_YOUSAY = "";

		internal static string RTCDLG_INFORMATION_ERROR = "";
		internal static string RTCDLG_INFO = "";
		internal static string RTCDLG_HIDE = "";

		static private ResourceManager rm = null;

		/// <summary>
		/// This checks that the string was found and handles an error if not.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		private static string GetAndCheckString( string name )
		{
			string val = rm.GetString( name );
			if ( val == null )
			{
				// Not localized since the error indicates there may be a localization problem.
				Logging.Error( "RtcLib",
					String.Format( "Unable to find string {0} in resource file.", name ) );
				val = name;
			}

			return val;
		}

		static localize()
		{
			try
			{
				
				//Assembly asm = Assembly.LoadWithPartialName("AgentDesktopGlobalizedStrings.resources");
				//rm = new ResourceManager("AgentDesktopGlobalizedStrings", asm);

				//V2.5 new localization approach.
				Assembly asm = Assembly.GetExecutingAssembly(); 
				rm = new ResourceManager("Microsoft.Ccf.Samples.Csr.RtcLib.Strings", asm); 

				RTCLIB_TEXTMSG_UNABLE = GetAndCheckString("RTCLIB_TEXTMSG_UNABLE");

				RTCDLG_MSG_ANSWERING = GetAndCheckString("RTCDLG_MSG_ANSWERING");
				RTCDLG_TEXTMSG_ASSISTANCE = GetAndCheckString("RTCDLG_TEXTMSG_ASSISTANCE");
				RTCDLG_TEXTMSG_TO = GetAndCheckString("RTCDLG_TEXTMSG_TO");
				RTCDLG_TEXTMSG_CONVERSATION_CLOSED = GetAndCheckString("RTCDLG_TEXTMSG_CONVERSATION_CLOSED");
				RTCDLG_TEXTMSG_HELPREQUEST = GetAndCheckString("RTCDLG_TEXTMSG_HELPREQUEST");
				RTCDLG_TEXTMSG_SAYS = GetAndCheckString("RTCDLG_TEXTMSG_SAYS");
				RTCDLG_ERRMSG_EXCEPTION = GetAndCheckString("RTCDLG_ERRMSG_EXCEPTION");
				RTCDLG_MSGBOX_MSG_UNABLE_REACH = GetAndCheckString("RTCDLG_MSGBOX_MSG_UNABLE_REACH");
				RTCDLG_TEXTMSG_YOUSAY = GetAndCheckString("RTCDLG_TEXTMSG_YOUSAY");
 
				RTCDLG_INFORMATION_ERROR = GetAndCheckString("RTCDLG_INFORMATION_ERROR");
				RTCDLG_INFO = GetAndCheckString("RTCDLG_INFO");
				RTCDLG_HIDE = GetAndCheckString("RTCDLG_HIDE");
			}
			catch
			{
				// not localized text since we can't read it from the resource.
				MessageBox.Show( "Unable to load localized strings in RtcLib", Application.ProductName );
			}
		}
	}
}