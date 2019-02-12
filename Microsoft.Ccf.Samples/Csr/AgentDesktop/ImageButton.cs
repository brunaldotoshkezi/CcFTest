//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// ImageButton.cs
//
// This class uses a .Net PictureBox control to create a control that
// acts like a button and has images for normal, pressed, and mouse hovered states.
//
//===============================================================================

using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// The ImageButton class is used to implement a button which has
	/// different images for normal, hovered, and down positions.  It makes
	/// a simple way to have a complicated image be drawn onto a button.
	/// </summary>
	public class ImageButton : System.Windows.Forms.PictureBox
	{
		public ImageButton()
		{
			this.MouseEnter += new EventHandler(ImageButton_MouseEnter);
			this.MouseLeave += new EventHandler(ImageButton_MouseLeave);

			this.MouseDown += new MouseEventHandler(ImageButton_MouseDown);
			this.MouseUp += new MouseEventHandler(ImageButton_MouseUp);

			this.Click += new EventHandler(ImageButton_Click);
		}

		/// <summary>
		/// Image used during normal display of the button
		/// </summary>
		public new Image Image
		{
			get { return base.Image; }
			set
			{
				if ( imageDefault == null )
					imageDefault = value;
				base.Image = value;
			}
		}
		private Image imageDefault = null;

		/// <summary>
		/// Image used when the button is pressed
		/// </summary>
		[
		Category("Appearance"),
		Description("Image used when button is pressed"),
		Browsable(true),
		]
		public Image ImagePressed
		{
			get { return imagePressed; }
			set { imagePressed = value; }
		}
		private Image imagePressed = null;
	
		/// <summary>
		/// Image used when hovering over the button
		/// </summary>
		[
		Category("Appearance"),
		Description("Image used when hovering over button"),
		Browsable(true),
		]
		public Image ImageHover
		{
			get { return imageHover; }
			set { imageHover = value; }
		}
		private Image imageHover = null;

		/// <summary>
		/// Called to change the image to hover if there is a hover image.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ImageButton_MouseEnter(object sender, EventArgs e)
		{
			if ( !pressed && imageHover != null )
				Image = imageHover;
		}

		/// <summary>
		/// Called when the mouse is no longer over the button and used
		/// to change the image to the default one if there is a default image.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ImageButton_MouseLeave(object sender, EventArgs e)
		{
			if ( !pressed && imageDefault != null )
				Image = imageDefault;
		}

		/// <summary>
		/// Called when the button is pressed by a mouse to change the
		/// image to the 'down' one (if provided).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ImageButton_MouseDown(object sender, MouseEventArgs e)
		{
			if ( imagePressed != null )
				Image = imagePressed;
		}

		/// <summary>
		/// Called when the button is no longer pressed by a mouse to change the
		/// image to the 'default' one (if provided).
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void ImageButton_MouseUp(object sender, MouseEventArgs e)
		{
			if ( !pressed && imageDefault != null )
				Image = imageDefault;
		}

		/// <summary>True if a border is drawn around the button</summary>
		[
		Category("Appearance"),
		Description("True if a border is drawn around the button images."),
		Browsable(true),
		]
		public bool Border
		{
			get { return border; }
			set { border = value; }
		}
		private bool border = false;

		/// <summary>
		/// The color of the border for the button
		/// </summary>
		[
		Category("Appearance"),
		Description("The color to use for a button border."),
		Browsable(true),
		]
		public Color BorderColor
		{
			get { return borderColor; }
			set { borderColor = value; }
		}
		private Color borderColor;

		private bool pressed = false;

		/// <summary>
		/// Draws the control and border if a border is needed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint (e);

			if ( border )
			{
				using ( Pen myPen = new Pen( borderColor, 1.0f ) )
					e.Graphics.DrawRectangle( myPen, 0, 0, Width, Height );
			}
		}

		protected void ImageButton_Click(object sender, EventArgs e)
		{
			if ( this.ContextMenu != null )
			{
				try
				{
					pressed = true;  // makes the button look pressed

					// This code executes when the menu button is pressed or when
					// the menu button has its accelerator key pressed. ( Alt-x )
					// This happens when a context menu is attached to the button.
					//
					// It does NOT allow the button to act like the top menu item
					// in a normal menubar since it doesn't handle moving from one button to
					// another like a real menu.
					ContextMenu.Show( this, new Point( 0, this.Height ) );
				}
				finally
				{
					pressed = false;
				}
			}
		}
	}
}
