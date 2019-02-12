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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

namespace Microsoft.Ccf.Csr
{
	/// <summary>
	/// Summary description for CCFPanelToolbar.
	/// </summary>
	public class CcfButtonToolbar : UserControl
	{
		///<summary>
		///</summary>
		///<param name="application"></param>
		public delegate void CloseButtonClickHandler(object application);

		/// <summary>
		/// Event for when the close button is clicked.
		/// </summary>
		public event CloseButtonClickHandler CloseButtonClick;

		#region Variables
		private IContainer components;
		protected object app = null;
		protected ImageList imageList;
		public ToolBar subToolBar;
		public ToolBarButton CloseButton;
		protected Panel panel1;
		#endregion

		///<summary>
		///</summary>
		public object Application
		{
			get
			{
				return app;
			}
		}

		/// <summary>
		/// Default constructor to permit Visual Studio design tools to work
		/// </summary>
		public CcfButtonToolbar()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Constructor used from UIConfiguration class to pass the app this toolbar
		/// is working with.
		/// </summary>
		/// <param name="app">The hosted app this toolbar is associated with</param>
		public CcfButtonToolbar(object app ) : this()
		{
			this.app = app;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CcfButtonToolbar));
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.subToolBar = new System.Windows.Forms.ToolBar();
			this.CloseButton = new System.Windows.Forms.ToolBarButton();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList.Images.SetKeyName(0, "");
			this.imageList.Images.SetKeyName(1, "");
			this.imageList.Images.SetKeyName(2, "close.bmp");
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.Transparent;
			this.panel1.Controls.Add(this.subToolBar);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(336, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(160, 30);
			this.panel1.TabIndex = 3;
			// 
			// subToolBar
			// 
			this.subToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.subToolBar.AutoSize = false;
			this.subToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.CloseButton});
			this.subToolBar.ButtonSize = new System.Drawing.Size(68, 24);
			this.subToolBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.subToolBar.DropDownArrows = true;
			this.subToolBar.ImageList = this.imageList;
			this.subToolBar.Location = new System.Drawing.Point(133, 0);
			this.subToolBar.Name = "subToolBar";
			this.subToolBar.ShowToolTips = true;
			this.subToolBar.Size = new System.Drawing.Size(27, 30);
			this.subToolBar.TabIndex = 2;
			this.subToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.subToolBar.ButtonClick += new ToolBarButtonClickEventHandler(this.subToolBar_ButtonClick);
			// 
			// CloseButton
			// 
			this.CloseButton.ImageIndex = 2;
			this.CloseButton.Name = "CloseButton";
			// 
			// CcfButtonToolbar
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this.panel1);
			this.Name = "CcfButtonToolbar";
			this.Size = new System.Drawing.Size(496, 30);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.CCFPanelToolbar_Paint);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Called whenever a button on the toolbar is pressed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void subToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
		{
			if (this.CloseButtonClick != null)
			{
				CloseButtonClick(app);
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

			Rectangle rect = new Rectangle( 0, 0, this.Width-1, this.Height-1 );
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
}
#pragma warning restore 1591