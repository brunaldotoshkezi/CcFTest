//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2006 Microsoft Corporation. All rights reserved.
//
// An about box for CCF.
//
//===============================================================================

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// An About box for CCF.
	/// </summary>
	public class CCFAbout : System.Windows.Forms.Form
	{
		private string agentNumber = String.Empty;
		private System.Windows.Forms.Button OK;
		private System.Windows.Forms.Label Separator;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public System.Windows.Forms.Label AboutDesc;
		private System.Windows.Forms.Label dotNetVersion;
		public System.Windows.Forms.PictureBox pcCCFAppImage;

		public CCFAbout()
		{
			InitializeComponent();
		}

		public CCFAbout( string agentNumber )
		{
			InitializeComponent();

			if ( agentNumber != null )
			{
				this.agentNumber = agentNumber;
			}
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CCFAbout));
            this.OK = new System.Windows.Forms.Button();
            this.pcCCFAppImage = new System.Windows.Forms.PictureBox();
            this.AboutDesc = new System.Windows.Forms.Label();
            this.Separator = new System.Windows.Forms.Label();
            this.dotNetVersion = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pcCCFAppImage)).BeginInit();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.OK.Location = new System.Drawing.Point(253, 128);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 0;
            this.OK.Text = "OK";
            // 
            // pcCCFAppImage
            // 
            this.pcCCFAppImage.Image = global::Microsoft.Ccf.Samples.Csr.AgentDesktop.Strings.AgentDesktop;
            this.pcCCFAppImage.Location = new System.Drawing.Point(16, 24);
            this.pcCCFAppImage.Name = "pcCCFAppImage";
            this.pcCCFAppImage.Size = new System.Drawing.Size(68, 68);
            this.pcCCFAppImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcCCFAppImage.TabIndex = 2;
            this.pcCCFAppImage.TabStop = false;
            // 
            // AboutDesc
            // 
            this.AboutDesc.Location = new System.Drawing.Point(96, 16);
            this.AboutDesc.Name = "AboutDesc";
            this.AboutDesc.Size = new System.Drawing.Size(240, 80);
            this.AboutDesc.TabIndex = 3;
            this.AboutDesc.Text = "Microsoft Customer Care Framework";
            // 
            // Separator
            // 
            this.Separator.BackColor = System.Drawing.Color.Lavender;
            this.Separator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Separator.Location = new System.Drawing.Point(8, 112);
            this.Separator.Name = "Separator";
            this.Separator.Size = new System.Drawing.Size(328, 4);
            this.Separator.TabIndex = 4;
            this.Separator.Paint += new System.Windows.Forms.PaintEventHandler(this.Separator_Paint);
            // 
            // dotNetVersion
            // 
            this.dotNetVersion.Location = new System.Drawing.Point(16, 120);
            this.dotNetVersion.Name = "dotNetVersion";
            this.dotNetVersion.Size = new System.Drawing.Size(232, 23);
            this.dotNetVersion.TabIndex = 5;
            // 
            // CCFAbout
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.OK;
            this.ClientSize = new System.Drawing.Size(344, 157);
            this.Controls.Add(this.dotNetVersion);
            this.Controls.Add(this.Separator);
            this.Controls.Add(this.AboutDesc);
            this.Controls.Add(this.pcCCFAppImage);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CCFAbout";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About Customer Care Framework";
            this.Load += new System.EventHandler(this.CCFAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pcCCFAppImage)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void CCFAbout_Load(object sender, System.EventArgs e)
		{
			System.Version version = new System.Version();
			this.dotNetVersion.Text = localize.CCF_ABOUT_DOTNET_VERSION + String.Format("{0}.{1}.{2}.{3}",
				System.Environment.Version.Major.ToString(),
				System.Environment.Version.Minor.ToString(),
				System.Environment.Version.Build,
				System.Environment.Version.Revision.ToString());

			AboutDesc.Text = String.Format( "{0}\n{1}\n\n{2}\n\n{3}",
				localize.CCF_ABOUT_CONTACT_CENTER_FRAMEWORK,
				localize.CCF_ABOUT_VERSION,
				localize.CCF_ABOUT_COPYRIGHT, 
				agentNumber );
		}

		private void Separator_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			System.Drawing.Color BlueStartColor  = System.Drawing.Color.FromArgb( 165, 185, 200 );
			System.Drawing.Color BlueEndColor  = System.Drawing.Color.FromArgb( 165,207,250 );
			Rectangle rectToPaint = new Rectangle( 0, 0, Separator.Width-1, Separator.Height-1 );

			using ( LinearGradientBrush brBrushToPaint = 
						new LinearGradientBrush( rectToPaint, BlueStartColor, BlueEndColor,
						LinearGradientMode.Horizontal ) )
			{
				e.Graphics.FillRectangle( brBrushToPaint, rectToPaint );
			}
		}
	}
}
