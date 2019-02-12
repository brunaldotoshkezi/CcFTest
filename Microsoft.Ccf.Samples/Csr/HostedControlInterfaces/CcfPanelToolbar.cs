//===================================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other agreements
//
// Customer Care Framework
// Copyright 2003-2006 Microsoft Corp.
//
// Creates a toolbar to manage operations for a specific hosted application.
// In its initial revision, this is limited to Back and Forward buttons for
// hosted web applications.
//
//===================================================================================
#pragma warning disable 1591

#region Using
using System;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Csr.Browser.Web;
using Microsoft.Ccf.Common.Logging;
#endregion

namespace Microsoft.Ccf.Csr
{
	/// <summary>
	/// Summary description for CCFPanelToolbar.
	/// </summary>
	public class CcfPanelToolbar : UserControl
	{
		#region Variables
		private System.ComponentModel.IContainer components;
		protected IHostedApplication app = null;
		protected ToolBar subToolBar;
		protected ImageList imageList;
		protected Panel panel1;
		public ToolBarButton BackButton;
		public ToolBarButton ForwardButton;
		#endregion

		/// <summary>
		/// Default constructor to permit Visual Studio design tools to work
		/// </summary>
		public CcfPanelToolbar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// localized text
			BackButton.Text = localize.BACK;
			ForwardButton.Text = localize.FORWARD;
		}

		/// <summary>
		/// Constructor used from UIConfiguration class to pass the app this toolbar
		/// is working with.
		/// </summary>
		/// <param name="app">The hosted app this toolbar is associated with</param>
		public CcfPanelToolbar(IHostedApplication app) : this()
		{
			this.app = app;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CcfPanelToolbar));
			this.subToolBar = new System.Windows.Forms.ToolBar();
			this.BackButton = new System.Windows.Forms.ToolBarButton();
			this.ForwardButton = new System.Windows.Forms.ToolBarButton();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// subToolBar
			// 
			this.subToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.subToolBar.AutoSize = false;
			this.subToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						  this.BackButton,
																						  this.ForwardButton});
			this.subToolBar.ButtonSize = new System.Drawing.Size(68, 24);
			this.subToolBar.Dock = System.Windows.Forms.DockStyle.None;
			this.subToolBar.DropDownArrows = true;
			this.subToolBar.ImageList = this.imageList;
			this.subToolBar.Location = new System.Drawing.Point(0, -4);
			this.subToolBar.Name = "subToolBar";
			this.subToolBar.ShowToolTips = true;
			this.subToolBar.Size = new System.Drawing.Size(176, 30);
			this.subToolBar.TabIndex = 2;
			this.subToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.subToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.subToolBar_ButtonClick);
			// 
			// BackButton
			// 
			this.BackButton.ImageIndex = 0;
			this.BackButton.Text = "&Back";
			// 
			// ForwardButton
			// 
			this.ForwardButton.ImageIndex = 1;
			this.ForwardButton.Text = "&Forward";
			// 
			// imageList
			// 
			this.imageList.ImageSize = new System.Drawing.Size(24, 24);
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.subToolBar);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(184, 24);
			this.panel1.TabIndex = 3;
			// 
			// CCFPanelToolbar
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.panel1);
			this.Name = "CCFPanelToolbar";
			this.Size = new System.Drawing.Size(496, 30);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.CCFPanelToolbar_Paint);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Called whenever a button on the toolbar is pressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void subToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (e.Button == BackButton)
			{
				Back_Click(sender, e);
			}
			else if (e.Button == ForwardButton)
			{
				Forward_Click(sender, e);
			}
		}

		/// <summary>
		/// Moves the browser's display back to the previous page
		/// </summary>
		/// <param name="sender">Object Sender</param>
		/// <param name="e">System.EventArgs e</param>
		private void Back_Click(object sender, EventArgs e)
		{
			try
			{
				// Perform the "Back" operation on the web page
				HostedWebApplication webApp = app as HostedWebApplication;
				if ( webApp != null )
				{
					WebBrowserExtended browser = webApp.TopLevelWindow as WebBrowserExtended;
					browser.GoBack();
				}
			}
			catch ( Exception exp )
			{
				Logging.Trace( exp.Message + "\n\n" + exp.StackTrace );
			}
		}

		/// <summary>
		/// Moves the browser's display forward to the page that hda been displayed before going back.
		/// </summary>
		/// <param name="sender">Object Sender</param>
		/// <param name="e">System.EventArgs e</param>
		private void Forward_Click(object sender, EventArgs e)
		{
			try
			{
				// Perform the "Forward" operation on the web page
				HostedWebApplication webApp = app as HostedWebApplication;
				if ( webApp != null )
				{
					WebBrowserExtended browser = webApp.TopLevelWindow as WebBrowserExtended;
					browser.GoForward();
				}
			}
			catch ( Exception exp )
			{
				Logging.Trace( exp.Message + "\n\n" + exp.StackTrace );
			}
		}

		/// <summary>
		/// Produces the pretty background of the toolbar.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CCFPanelToolbar_Paint(object sender, PaintEventArgs e)
		{
			Graphics myGraphics = e.Graphics;
			myGraphics.CompositingQuality = CompositingQuality.GammaCorrected;

			Rectangle rect = new Rectangle( 0, 0, Width-1, Height-1 );
			using ( LinearGradientBrush lgBrush =
													new LinearGradientBrush( rect,
													// Silver Theme
													//Color.FromArgb( 255, 255, 255 ),
													//Color.FromArgb( 198, 187, 215 ),
													Color.FromArgb(0xFF, 0xFF, 0xFF),
													//Color.DarkCyan
													Color.FromArgb(165,207,255),
													LinearGradientMode.ForwardDiagonal) )
			{
				float[] relativeIntensities = {0.0f, 0.008f, 1.0f};
				float[] relativePositions   = {0.0f, 0.32f, 1.0f};

				Blend blend = new Blend();
				blend.Factors = relativeIntensities;
				blend.Positions = relativePositions;
				lgBrush.Blend = blend;

				e.Graphics.FillRectangle( lgBrush, rect );
			}
		}
	}

	///<summary>
	///</summary>
	public class localize
	{
		// Globalized strings
		public static string BACK = string.Empty;
		public static string FORWARD = string.Empty;
		static private ResourceManager rm = null;

		/// <summary>
		/// This checks that the string was found and handles an error if not.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		static private string GetAndCheckString(string name)
		{
			string val = rm.GetString(name);
			if (val == null)
			{
				// Not localized since the error indicates there may be a localization problem.
				Logging.Error(System.Windows.Forms.Application.ProductName,
					String.Format("Unable to find string {0} in resource file.", name));
				val = name;
			}

			return val;
		}

		static localize()
		{
			try
			{

				Assembly asm = Assembly.GetExecutingAssembly();
				rm = new ResourceManager("Microsoft.Ccf.Samples.HostedControlInterfaces.Strings", asm);

				BACK = GetAndCheckString("BACK");
				FORWARD = GetAndCheckString("FORWARD");
			}
			catch (Exception exp)
			{
				// Nothing much can be done if resource library itself cannot be read so leave it in English
				Logging.Error(System.Windows.Forms.Application.ProductName, "Unable to find or load localization DLL strings for CcfPanelToolbar.", exp);
			}
		}
	}
}
#pragma warning restore 1591