	//===================================================================================
	// Microsoft Product – subject to the terms of the Microsoft EULA and other 
	// agreements
	//
	// Customer Care Framework
	// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
	//
	// This is where the CCF desktop application is started.  It has been separated from the
	// desktop.cs/desktoptoolbar.cs code since this is code that shouldn't be modified at
	// customer engagements.
	//
	//===================================================================================

	#region Usings
	using System;
	using System.Threading;
	using System.Windows.Forms;
	using Microsoft.Ccf.Common;
	using Microsoft.Ccf.Common.Logging;
	using Microsoft.Ccf.Csr;
	#endregion

	namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
	{
	/// <summary>
	/// This is the starting class for CCF Desktop.
	/// </summary>
	/// <remarks>
	/// This class is self-contained and any changes should be minimal at best. It contains the primary
	/// setup routine to call the Agent Desktop.
	/// </remarks>
	public class StartApplication
	{
		#region Global variables
		private static string WINDOWS_AUTHENTICATION_MODE = "Windows";
		#endregion  

		#region Main Method
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		/// <remarks>
		/// This also handles if a splash screen is created.
		/// </remarks>
		[STAThread]
		static void Main()
		{
			bool desktopToolBar = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["DesktopToolBar"]);

			if (desktopToolBar)
			{
				startDestkopToolBar();
			}
			else
			{
				startDesktop();
			}
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Start the desktop tab version.
		/// </summary>
		private static void startDesktop()
		{
			CcfShellBase<Desktop> desktop = null;

			try
			{
				// Even though there is a try-catch across this Main() function
				// exceptions thrown by the message loop of the desktop will not be handled in
				// the catch.  Hence we have a separate exception handler for the Application object.
				Application.ThreadException += new ThreadExceptionEventHandler(ApplicationExceptionHandler);

				bool showSplashScreen;

				if (Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["showSplashScreen"], out showSplashScreen) == false)
				{
					Logging.Error(localize.DESKTOP_MODULE_NAME, String.Format(localize.DESKTOP_ERR_READING_OR_PARSING_APPSETTINGS, "showSplashScreen"));
					return;
				}

				// If configured to show a splash screen, then do so.
				// Otherwise start the application without a splash screen.
				if (showSplashScreen)
				{
					// Start splash screen
					new SplashScreen(new SplashForm());
				}

				bool doExit = ShowLoginDialogue();
				if (doExit)
				{
					return;
				}

				// NOTE : DO NOT USE Logging, ListenerConfiguration or anything that access a non anonymous web service before this point 

				Logging.ShowErrors = false;
				// Ensure there is only one instance of the desktop if one is already running then shut down.
				if (!GeneralFunctions.IsInitialInstance())
				{
					Logging.Error(Application.ProductName, localize.APPLICATION_NAME + localize.DESKTOP_MSG_ALREADY_RUNNING);
					return;
				}

				// Log that the application is starting
				Logging.Information(Application.ProductName, localize.DESKTOP_MSG_STARTING);

				// Start the Agent Desktop

				// Start the desktop shell application
				desktop = new CcfShellBase<Desktop>();
				desktop.StartApplication();

				// Log that the application is ending
				Logging.Information(Application.ProductName, localize.DESKTOP_INFO_AGENT_DESKTOP_EXITED);
			}
			catch (Exception exp)
			{
				string message;
				// If something horribly bad happens, it'll get caught here.
				if (exp.Message.IndexOf(localize.DESKTOP_MSG_SQL_EXIST) >= 0)
				{
					message = localize.DESKTOP_ERR_UNABLE_CONNECT_SQL;
				}
				else if (exp.Message.IndexOf(localize.COMMON_MSG_IIS_ERROR) >= 0)
				{
					message = localize.DESKTOP_IIS_ERROR;
				}
				else if (Desktop.LoggedIn)
				{
					message = localize.DESKTOP_ERR_FATAL_ERROR;
				}
				else
				{
					message = localize.DESKTOP_ERR_FATAL_UNABLE_LOG;
				}
				Logging.Error(Application.ProductName, message, exp);
			}
			finally
			{
				// RTC needs to be explicitly closed to allow its interop to release resources
				// and thus prevent evil crashes later.
				if (desktop != null && desktop.DesktopShell != null && desktop.DesktopShell.Rtc != null)
				{
					desktop.DesktopShell.Rtc.Close();
					desktop.DesktopShell.Rtc = null;
				}
			}
		}

		/// <summary>
		/// Start the desktop toolbar version.
		/// </summary>
		private static void startDestkopToolBar()
		{
			//DesktopToolBar desktop = null;
			CcfShellBase<DesktopToolBar> desktopToolBar = null;

			try
			{
				// Even though there is a try-catch across this Main() function
				// exceptions thrown by the message loop of the desktop will not be handled in
				// the catch.  Hence we have a separate exception handler for the Application object.
				Application.ThreadException += new ThreadExceptionEventHandler(ApplicationExceptionHandler);

				// Start the Agent Desktop

				bool showSplashScreen;

				if (Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["showSplashScreen"], out showSplashScreen) == false)
				{
					Logging.Error(localize.DESKTOP_MODULE_NAME, String.Format(localize.DESKTOP_ERR_READING_OR_PARSING_APPSETTINGS, "showSplashScreen"));
					return;
				}

				// If configured to show a splash screen, then do so.
				// Otherwise start the application without a splash screen.
				if (showSplashScreen)
				{
					// Start splash screen
					new SplashScreen(new SplashForm());
				}

				bool doExit = ShowLoginDialogue();
				if (doExit)
				{
					return;
				}

				// NOTE : DO NOT USE Logging, ListenerConfiguration or anything that access a non anonymous web service before this point --


				Logging.ShowErrors = false;
				// Ensure there is only one instance of the desktop if one is already running then shut down.
				if (!GeneralFunctions.IsInitialInstance())
				{
					Logging.Error(Application.ProductName, localize.APPLICATION_NAME + localize.DESKTOP_MSG_ALREADY_RUNNING);
					return;
				}

				// Log that the application is starting
				Logging.Information(Application.ProductName, localize.DESKTOP_MSG_STARTING);

				// Start the desktopToolBar shell application
				desktopToolBar = new CcfShellBase<DesktopToolBar>();
				desktopToolBar.StartApplication();

				// Log that the application is ending
				Logging.Information(Application.ProductName, localize.DESKTOP_INFO_AGENT_DESKTOP_EXITED);
			}
			catch (Exception exp)
			{
				string message;
				// If something horribly bad happens, it'll get caught here.
				if (exp.Message.IndexOf(localize.DESKTOP_MSG_SQL_EXIST) >= 0)
				{
					message = localize.DESKTOP_ERR_UNABLE_CONNECT_SQL;
				}
				else if (exp.Message.IndexOf(localize.COMMON_MSG_IIS_ERROR) >= 0)
				{
					message = localize.DESKTOP_IIS_ERROR;
				}
				else if (DesktopToolBar.LoggedIn)
				{
					message = localize.DESKTOP_ERR_FATAL_ERROR;
				}
				else
				{
					message = localize.DESKTOP_ERR_FATAL_UNABLE_LOG;
				}
				Logging.Error(Application.ProductName, message, exp);
			}
			finally
			{
				// RTC needs to be explicitly closed to allow its interop to release resources
				// and thus prevent evil crashes later.
				if (desktopToolBar != null && desktopToolBar.DesktopShell != null && desktopToolBar.DesktopShell.Rtc != null)
				{
					desktopToolBar.DesktopShell.Rtc.Close();
					desktopToolBar.DesktopShell.Rtc = null;
				}
			}
		}

		/// <summary>
		/// The exception handler of the Application object.
		/// </summary>
		/// <param name="sender">The sending object reference.</param>
		/// <param name="args">The thread exception event arguments <see cref="ThreadExceptionEventArgs"/></param>
		/// <remarks>
		/// This is a last-ditch catch of exceptions.
		/// </remarks>
		static private void ApplicationExceptionHandler(object sender, ThreadExceptionEventArgs args)
		{
			string message;

			if (args.Exception.Message.IndexOf(localize.DESKTOP_MSG_SQL_EXIST) >= 0)
			{
				message = localize.DESKTOP_ERR_UNABLE_CONNECT_SQL;
			}
			else if (args.Exception.Message.IndexOf(localize.COMMON_MSG_IIS_ERROR) >= 0)
			{
				message = localize.DESKTOP_IIS_ERROR;
			}
			else if (Desktop.LoggedIn || DesktopToolBar.LoggedIn)
			{
				message = localize.DESKTOP_ERR_FATAL_ERROR;
			}
			else
			{
				message = localize.DESKTOP_ERR_FATAL_UNABLE_LOG;
			}
			Logging.Error(Application.ProductName, message, args.Exception);
		}

		/// <summary>
		/// Shows a login dialogue if it is configured to show.
		/// </summary>
		/// <remarks>
		/// No Logging can be done till login is done as Logging needs to access Configuration web service which is non anonymous
		/// </remarks>
		private static bool ShowLoginDialogue()
		{
			if (!(System.Configuration.ConfigurationManager.AppSettings["authenticationMode"].Equals(WINDOWS_AUTHENTICATION_MODE)))
			{
				LoginForm loginForm = new LoginForm();

				loginForm.Visible = false;
				DialogResult result = loginForm.ShowDialog();
				if (DialogResult.Abort.Equals(result))
				{
					return true;
				}
			}
			return false;
		}
		#endregion

	}
	}
