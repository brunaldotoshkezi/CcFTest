//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// copyright 2003-2006 Microsoft Corp.
//
// Splash screen for during application startup.
//
//===================================================================================
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// The form used for the splash screen.
	/// </summary>
	public partial class SplashForm : Form
	{
		public SplashForm()
		{
			InitializeComponent();
		}
	}

	/// <summary>
	/// Handles creating and destroying the splash screen during application startup.
	/// </summary>
	public class SplashScreen
	{
		private static Form form = null;
		private static Timer splashTimer = new Timer();

		public SplashScreen(Form splashForm)
		{
			if (splashForm != null)
			{
				form = splashForm;
				form.Show();
				form.Refresh();

				splashTimer.Tick += new EventHandler(SplashTimeUp);
				splashTimer.Interval = 500;
				splashTimer.Enabled = true;
			}
		}

		/// <summary>
		/// Causes the splash screen to be removed.
		/// </summary>
		static public void SplashDone()
		{
			splashTimer.Enabled = false;
			splashTimer.Dispose();

			if (form != null)
			{
				form.BringToFront();

				// Do a quick fade
				for (int cnt = 50; cnt > 0; cnt--)
				{
					form.Opacity -= 0.02f;
					form.Refresh();
					System.Threading.Thread.Sleep(50);
				}

				form.Close();
				form = null;
			}
		}

		/// <summary>
		/// Closes the splash screen immediately
		/// </summary>
		static public void SplashDoneNoFade()
		{
			splashTimer.Enabled = false;
			splashTimer.Dispose();

			if (form != null)
			{
				form.Close();
				form = null;
			}
		}

		/// <summary>
		/// Force the splash screen, if any, to redraw.
		/// </summary>
		static public void SplashRefresh()
		{
			if (form != null)
			{
				form.Activate();
				form.Refresh();
			}
		}

		/// <summary>
		/// Makes sure the screen stays looking pretty.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SplashTimeUp(object sender, EventArgs e)
		{
			splashTimer.Enabled = false;

			form.Activate();
			form.Refresh();
		}

	}
}