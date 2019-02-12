//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// GetNumbers.cs
//
// Revisions:
// May 2003      v1.0  release
// December 2004 v1.02 release - test
//
//===============================================================================

using System;
using System.Windows.Forms;

using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// Summary description for GetNumber.
	/// </summary>
	public class GetNumber : System.Windows.Forms.Form
	{
		// CCF 2.0
		// Limit the length of phone number to 25 (assumed number)
		private const int PhoneNumberMaxLength = 25;

		private System.Windows.Forms.ComboBox number;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button addressBook;

		private static string []PreviousNumbers = { "","","","","" };

		public GetNumber()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			// CCF 2.0
			// Restrict the lenght of phone number to certain number, 
			// otherwise a very large phone number makes Agent Desktop hang.
			number.MaxLength = PhoneNumberMaxLength;
		}

		public GetNumber( string title )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			resultNumber = "";

			// Sets the caption appropriate for the requested operation
			this.Text = title;

			this.label1.Text = localize.AGENT_DESKTOP_GET_NUMBER_LABEL1_TEXT;
			this.ok.Text = localize.AGENT_DESKTOP_GET_NUMBER_OK_TEXT;
			this.cancel.Text = localize.AGENT_DESKTOP_GET_NUMBER_CANCEL_TEXT;
			this.addressBook.Text = localize.AGENT_DESKTOP_GET_NUMBER_ADDR_BOOK_TEXT;

			// Get the history list of previously dialed numbers
			number.Text = PreviousNumbers[0];
			for ( int i = 0; i < PreviousNumbers.Length; i++ )
				number.Items.Add( PreviousNumbers[ i ] );

			// CCF 2.0
			// Restrict the lenght of phone number to certain number, 
			// otherwise a very large phone number makes Agent Desktop hang.
			number.MaxLength = PhoneNumberMaxLength;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GetNumber));
			this.number = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.addressBook = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// number
			// 
			this.number.AllowDrop = true;
			this.number.Location = new System.Drawing.Point(48, 12);
			this.number.MaxDropDownItems = 5;
			this.number.Name = "number";
			this.number.Size = new System.Drawing.Size(128, 21);
			this.number.TabIndex = 1;
			this.number.Text = "";
			this.number.TextChanged += new System.EventHandler(this.number_TextChanged);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "Number:";
			// 
			// ok
			// 
			this.ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ok.Location = new System.Drawing.Point(184, 12);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(56, 24);
			this.ok.TabIndex = 2;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.ok_Click);
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancel.Location = new System.Drawing.Point(248, 12);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(56, 24);
			this.cancel.TabIndex = 3;
			this.cancel.Text = "Cancel";
			// 
			// addressBook
			// 
			this.addressBook.DialogResult = System.Windows.Forms.DialogResult.None;
			this.addressBook.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.addressBook.Image = ((System.Drawing.Image)(resources.GetObject("addressBook.Image")));
			this.addressBook.Location = new System.Drawing.Point(344, 12);
			this.addressBook.Name = "addressBook";
			this.addressBook.Size = new System.Drawing.Size(96, 24);
			this.addressBook.TabIndex = 3;
			this.addressBook.Text = "Address Book";
			this.addressBook.Click += new System.EventHandler(this.addressBook_Click);
			// 
			// GetNumber
			// 
			this.AcceptButton = this.ok;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(448, 48);
			this.ControlBox = false;
			this.Controls.Add(this.ok);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.number);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.addressBook);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GetNumber";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.ResumeLayout(false);

		}
		#endregion

		private void ok_Click(object sender, System.EventArgs e)
		{
			resultNumber = number.Text.Trim();

			// Make sure there is a number to call
			string testNumber = resultNumber.Replace( '-', ' ' );
			testNumber = testNumber.Replace( '(', ' ' );
			testNumber = testNumber.Replace( ')', ' ' );
			testNumber = testNumber.Replace( 'x', ' ' );

			if ( testNumber.Trim() == String.Empty )
			{
				this.DialogResult = DialogResult.None;
				Microsoft.Ccf.Csr.Win32API.MessageBeep( 0 );
				number.Text = "";
			}
			else
			{
				// Post 1.02
				// Check that the number is not the same as the last one
				if ( PreviousNumbers.Length > 0 && PreviousNumbers[0] != resultNumber )
				{
					// Push the previous numbers down.  Could use Stack class but am not so I 
					// have a fixed size array rather than an increasingly large stack.
					for ( int i = PreviousNumbers.Length; --i > 0; )
					{
						PreviousNumbers[ i ] = PreviousNumbers[ i - 1 ];
					}
					PreviousNumbers[ 0 ] = resultNumber;
				}
			}
		}

		private void addressBook_Click(object sender, System.EventArgs e)
		{
			try
			{
				using ( AddressBookDlg dlg = new AddressBookDlg() )
				{
					if ( dlg.ShowDialog(this) == DialogResult.OK )
					{
						resultNumber = dlg.SelectedAddress.phone;
						resultName   = dlg.SelectedAddress.name;

						// if there is a number selected, use it
						if ( resultNumber != null && resultNumber.Trim() != String.Empty )
							DialogResult = DialogResult.OK;
					}
				}
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.AGENT_DESKTOP_GET_NUMBER_ERR_READING_ADDR_BOOK, exp );
			}
		}

		/// <summary>
		/// Verifies if the text is valid for a phone number.  Allows numbers
		/// plus ()-x and space since people tend to use those too.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void number_TextChanged(object sender, System.EventArgs e)
		{
			bool foundOne = false;
			int  editSel = number.SelectionStart;

			for ( int i = 0; i < number.Text.Length; i++ )
			{
				if ( "0123456789-()x ".IndexOf( number.Text[i] ) < 0 ||
					number.Text[0] == '-' )
				{
					// Move the selection back one so it stays in the correct location
					if ( i <= editSel )
						editSel--;

					number.Text = number.Text.Remove( i, 1 );
					i--;
					foundOne = true;
				}
			}

			// so the user notices that their illegal characters have been eaten
			if ( foundOne )
				Microsoft.Ccf.Csr.Win32API.MessageBeep( 0 );

			// Restore where the edit cursor was.
			if ( editSel >= 0 )
				number.SelectionStart = editSel;
		}

		/// <summary>
		/// Returns the number entered either on the dialog or via the address book
		/// </summary>
		public string PhoneNumber
		{
			get
			{
				if ( resultNumber != null )
					return resultNumber.Trim();
				return null;
			}
		}
		private string resultNumber;

		/// <summary>
		/// Returns the name of the called party or null if not known.  This
		/// can be filled in when the address book is used.
		/// </summary>
		public string PhoneName
		{
			get
			{
				if ( resultName != null )
					return resultName.Trim();
				return null;
			}
		}
		private string resultName;
	}
}
