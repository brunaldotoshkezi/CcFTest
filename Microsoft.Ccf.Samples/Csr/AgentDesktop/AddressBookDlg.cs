//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// AddressBook.Dlg.cs
//
// UI for finding a person within the company address book.  The addresses are
// found using a web service.  The web service reference implementation uses LDAP, but
// this can be modified without changing this code as long as the web service
// provides the same interface.
//
//===============================================================================

using System;
using System.Windows.Forms;
using Microsoft.Ccf.Csr.UIConfiguration;
using Microsoft.Ccf.Samples.HostedControlInterfaces;
using Microsoft.Ccf.Common.Logging;
using Microsoft.Ccf.Common;
using System.ComponentModel;
namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// Summary description for AddressBookDlg.
	/// </summary>
	public class AddressBookDlg : System.Windows.Forms.Form
	{
		private AddressBook.AddressBookProviderAddress selectedAddress = null;
		
		private AddressBook.AddressBookClient client;

		
		

		// How many names are searched for and displayed, some names may not
		// be displayed if they don't have a phone number so a few extras are searched
		// for beyond the 9 that will be displayed without a scroll bar.
		private const int howMany = 12;

		//private AddressBook.AddressBookProviderAddress []addresses = null; 
		private BindingList<AddressBook.AddressBookProviderAddress> addresses = null; 
	
		
	
		
		private AddressBook.AddressBook book;
		private static string lastNameSearchedFor = null;

		private System.Windows.Forms.Button Ok;
		private System.Windows.Forms.Button cancel;
		private System.Windows.Forms.ListView addressList;
		private System.Windows.Forms.ColumnHeader userName;
		private System.Windows.Forms.ColumnHeader phone;
		private System.Windows.Forms.Button find;
		private System.Windows.Forms.TextBox nameToFind;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ToolTip tip;
		private System.ComponentModel.IContainer components;

		public AddressBookDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			

			this.userName.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_USERNAME_TEXT;
			this.phone.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_PHONE_TEXT;
			this.Ok.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_OK_TEXT;
			this.cancel.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_CANCEL_TEXT;
			this.find.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_FIND_TEXT;
			this.label1.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_LABEL1_TEXT;
			this.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_TEXT;
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
			this.components = new System.ComponentModel.Container();
			this.addressList = new System.Windows.Forms.ListView();
			this.userName = new System.Windows.Forms.ColumnHeader();
			this.phone = new System.Windows.Forms.ColumnHeader();
			this.Ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.nameToFind = new System.Windows.Forms.TextBox();
			this.find = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tip = new System.Windows.Forms.ToolTip(this.components);
			this.SuspendLayout();
			// 
			// addressList
			// 
			this.addressList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						  this.userName,
																						  this.phone});
			this.addressList.FullRowSelect = true;
			this.addressList.HideSelection = false;
			this.addressList.Location = new System.Drawing.Point(8, 80);
			this.addressList.MultiSelect = false;
			this.addressList.Name = "addressList";
			this.addressList.Size = new System.Drawing.Size(400, 152);
			this.addressList.TabIndex = 2;
			this.addressList.View = System.Windows.Forms.View.Details;
			this.addressList.DoubleClick += new System.EventHandler(this.addressList_DoubleClick);
			this.addressList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.addressList_MouseMove);
			// 
			// userName
			// 
			this.userName.Width = 200;
			// 
			// phone
			// 
			this.phone.Width = 190;
			// 
			// Ok
			// 
			this.Ok.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Ok.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.Ok.Location = new System.Drawing.Point(336, 8);
			this.Ok.Name = "Ok";
			this.Ok.Size = new System.Drawing.Size(72, 24);
			this.Ok.TabIndex = 3;
			this.Ok.Click += new System.EventHandler(this.Ok_Click);
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancel.Location = new System.Drawing.Point(336, 44);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(72, 24);
			this.cancel.TabIndex = 4;
			// 
			// nameToFind
			// 
			this.nameToFind.Location = new System.Drawing.Point(8, 48);
			this.nameToFind.Name = "nameToFind";
			this.nameToFind.Size = new System.Drawing.Size(232, 20);
			this.nameToFind.TabIndex = 1;
			this.nameToFind.Text = "";
			// 
			// find
			// 
			this.find.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.find.Location = new System.Drawing.Point(248, 48);
			this.find.Name = "find";
			this.find.Size = new System.Drawing.Size(64, 20);
			this.find.TabIndex = 5;
			this.find.Click += new System.EventHandler(this.find_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(304, 32);
			this.label1.TabIndex = 6;
			// 
			// tip
			// 
			this.tip.AutoPopDelay = 5000;
			this.tip.InitialDelay = 500;
			this.tip.ReshowDelay = 100;
			this.tip.ShowAlways = true;
			// 
			// AddressBookDlg
			// 
			this.AcceptButton = this.Ok;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(416, 246);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.find);
			this.Controls.Add(this.nameToFind);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.Ok);
			this.Controls.Add(this.addressList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AddressBookDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Load += new System.EventHandler(this.AddressBookDlg_Load);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// For the caller of the dialog to get the selected phone number, name, etc
		/// </summary>
		public AddressBook.AddressBookProviderAddress SelectedAddress
		{
			get
			{
				return selectedAddress;
			}
		}


		private void AddressBookDlg_Load(object sender, System.EventArgs e)
		{
			bool defaultSearch = false;

			try
			{
				//book = new AddressBook.AddressBook();
				client = new AddressBook.AddressBookClient();
				string Url = UIConfiguration.ConfigurationReader.ReadSettings("AgentDesktop_AddressBook_AddressBook");
				System.ServiceModel.EndpointAddress addressBookServiceAddress = new System.ServiceModel.EndpointAddress(Url);
				client.Endpoint.Address = addressBookServiceAddress;
                client.ClientCredentials.Windows.ClientCredential = AgentCredentialUtilities.GetCurrentCredential();

				//book.Url = UIConfiguration.ConfigurationReader.ReadSettings("AgentDesktop_AddressBook_AddressBook");
				//book.Credentials  = Utils.GetCredentials();
				//book.PreAuthenticate = true;

				// Either init to the last search or to the first entries alphabetically
				if ( lastNameSearchedFor != null )
					nameToFind.Text = lastNameSearchedFor;
				else
				{
					defaultSearch = true;
					nameToFind.Text = localize.AGENT_DESKTOP_ADDR_BOOK_DLG_NAME_TO_FIND;
				}

				find_Click( this, null );

				// remove text if using default search
				if ( defaultSearch )
					nameToFind.Text = "";

				tip.Active = false;
				tip.SetToolTip( addressList, "" );
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_INITIALIZING_ADDR_BOOK, exp );
			}
		}


		/// <summary>
		/// Run when the user presses Find, also run when the dialog is first shown.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void find_Click(object sender, System.EventArgs e)
		{
			Cursor oldCursor;

			oldCursor = this.Cursor;
			System.Windows.Forms.Cursor.Current = Cursors.WaitCursor;

			tip.Active = false;
			addressList.Items.Clear();

			string searchText = nameToFind.Text.Trim();
			if ( searchText == String.Empty )
				searchText = "1";

			try
			{
				
				addresses = client.GetAddresses(searchText, howMany );
				
				if ( addresses != null )
				{
					foreach (AddressBook.AddressBookProviderAddress addr in addresses)
					{
						// only show addresses which have a phone we can use
						
						if ( addr.phone != null && addr.phone != "" )
						{
							ListViewItem item = addressList.Items.Add( addr.name );
							item.SubItems.Add( addr.phone );

							// so we can find this address later without using the
							// index since many addresses may be filtered out
							item.Tag = addr;
						}
					}

					if ( addressList.Items.Count > 0 )
					{
						addressList.Items[ 0 ].Selected = true;
						addressList.Focus();
					}

					// for the next time we bring up the address book
					lastNameSearchedFor = nameToFind.Text.Trim();
				}
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.AGENT_DESKTOP_ADDR_BOOK_DLG_ERR_READING_ADDR_BOOK, exp );
			}
			finally
			{
				System.Windows.Forms.Cursor.Current = oldCursor;
			}
		}

		/// <summary>
		/// Act as selecting an address and the clicking Dial/OK.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void addressList_DoubleClick(object sender, System.EventArgs e)
		{
			Ok_Click( sender, e );
		}

		private void Ok_Click(object sender, System.EventArgs e)
		{
			if ( nameToFind.Focused && nameToFind.Text.Trim() != "" )
			{
				find_Click( this, null );
				this.DialogResult = DialogResult.None;
			}
			else if ( addressList.Items.Count > 0)
			{
				if(addressList.SelectedItems.Count > 0 )
				{
					ListViewItem sel = addressList.SelectedItems[0];
					if ( sel != null )
					{
						selectedAddress = (AddressBook.AddressBookProviderAddress)sel.Tag;
						this.DialogResult = DialogResult.OK;  // needed when Ok_click() is called from dblclick
					}
				}
				else
				{
					//ErrorMsg.Show( localize.AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_SELECT_ADDR, null );  // v1.02
					string appName = "Customer Care Framework";
					Logging.Error(appName, localize.AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_SELECT_ADDR, string.Empty);
					this.DialogResult = DialogResult.None;
				}
			}
			else
			{
				//ErrorMsg.Show( localize.AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_NO_ADDR, null );  // v1.02
				string appName = "Customer Care Framework";
				Logging.Error(appName, localize.AGENT_DESKTOP_ADDR_BOOK_DLG_MSG_NO_ADDR, string.Empty);
				this.DialogResult = DialogResult.None;
			}
		}


		static int lastTipSel = -1;
		/// <summary>
		/// Used to show the tool tip over the address book list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void addressList_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			AddressBook.AddressBookProviderAddress address;
			ListViewItem item;

			item = addressList.GetItemAt( e.X, e.Y );
			if ( item != null && addressList.Items.Count > 0 )
			{
				address = item.Tag as AddressBook.AddressBookProviderAddress;

				if ( item.Index != lastTipSel && address != null )
				{
					lastTipSel = item.Index;

					tip.Active = false;
					tip.SetToolTip( addressList,
						address.name +
						localize.AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_DEPT + address.department +
						localize.AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_TITLE + address.title +
						localize.AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_LOCATION + address.location +
						localize.AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_PHONE + address.phone +
						localize.AGENT_DESKTOP_ADDR_BOOK_DLG_TOOLTIP_EMAIL + address.email );
					tip.Active = true;
				}
			}
		}
	}
}
