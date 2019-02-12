//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// HostedToolBarButton.cs
//
// Implements the status toolBarButton as a hosted control.
//
//===============================================================================

using System.Windows.Forms;
using Microsoft.Ccf.Csr;
using Microsoft.Ccf.Samples.HostedControlInterfaces;

namespace Microsoft.Ccf.Samples.HostedToolBarButton
{
	/// <summary>
	/// Implements the status toolBarButton as a hosted control
	/// </summary>
	public class HostedToolBarButton : HostedControl, IHostedToolBarButton
	{
		private System.Windows.Forms.ToolBar rightToolBar;
		private System.Windows.Forms.ToolBarButton status;
		private System.Windows.Forms.ImageList toolBarImageList;
		private System.ComponentModel.IContainer components;

		public HostedToolBarButton()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public HostedToolBarButton(int appID, string appName, string initString) :
			base( appID, appName, initString )
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Don't list application in SessionExplorer or CurrentSession 
		/// </summary>
		public override bool IsListed
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Get or set the status ToolBarButton
		/// </summary>
		public ToolBarButton Status
		{
			get
			{
				return status;	
			}
			set
			{
				status = value;	
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(HostedToolBarButton));
			this.rightToolBar = new System.Windows.Forms.ToolBar();
			this.status = new System.Windows.Forms.ToolBarButton();
			this.toolBarImageList = new System.Windows.Forms.ImageList(this.components);
			this.SuspendLayout();
			// 
			// rightToolBar
			// 
			this.rightToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.rightToolBar.AutoSize = false;
			this.rightToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																							this.status});
			this.rightToolBar.ButtonSize = new System.Drawing.Size(25, 36);
			this.rightToolBar.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightToolBar.DropDownArrows = true;
			this.rightToolBar.ImageList = this.toolBarImageList;
			this.rightToolBar.Location = new System.Drawing.Point(0, 0);
			this.rightToolBar.Name = "rightToolBar";
			this.rightToolBar.ShowToolTips = true;
			this.rightToolBar.Size = new System.Drawing.Size(272, 32);
			this.rightToolBar.TabIndex = 2;
			this.rightToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.rightToolBar.Wrappable = false;
			// 
			// status
			// 
			this.status.ImageIndex = 5;
			this.status.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
			this.status.Text = "Status";
			// 
			// toolBarImageList
			// 
			this.toolBarImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.toolBarImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.toolBarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("toolBarImageList.ImageStream")));
			this.toolBarImageList.TransparentColor = System.Drawing.Color.Aqua;
			// 
			// HostedToolBarButton
			// 
			this.Controls.Add(this.rightToolBar);
			this.Location = new System.Drawing.Point(-8, 0);
			this.Name = "HostedToolBarButton";
			this.Size = new System.Drawing.Size(272, 32);
			this.ResumeLayout(false);
		}
		#endregion
	}
}
