//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// LookupDlg.cs
//
// This is a reference implementation of a customer lookup.  This code is intended
// to be modified as needed for use within specific call centers.
//
// Revisions:
// May 2003      v1.0  release
// March 2004    v1.01 release
// December 2004 v1.02 release
//
//===============================================================================

using System;
using System.Windows.Forms;

using CustomerWS = Microsoft.Ccf.Adapter.CustomerWS;
using Microsoft.Ccf.Common.Logging;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// Summary description for LookupDlg.
	/// </summary>
	public class LookupDlg : Form
	{
		public  CustomerWS.CustomerProviderCustomerRecord Result;

		private CustomerWS.CustomerProviderCustomerRecord[] results = null;
		private CustomerWS.Customer CustomerLookup;
		private Label label1;
		private TextBox firstName;
		private TextBox id;
		private ColumnHeader last;
		private ColumnHeader first;
		private ColumnHeader address;
		private ColumnHeader homePhone;
		private ColumnHeader workPhone;
		private ListView resultsList;
		private Button select;
		private Button cancel;
		private Label errormsg;
		private Label label6;
		private TextBox phone;
		private Button btnNew;
		private Label label7;
		private Label label8;
		private Button lookupID;
		private Button lookupName;
		private Button lookupPhone;
		private TextBox lastName;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LookupDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CustomerLookup = null;
			Result = null;
		}

		public LookupDlg( CustomerWS.Customer customerLookup, CustomerWS.CustomerProviderCustomerRecord customer )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			CustomerLookup = customerLookup;
			Result = customer;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( components != null)
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
			this.lookupID = new System.Windows.Forms.Button();
			this.id = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.firstName = new System.Windows.Forms.TextBox();
			this.resultsList = new System.Windows.Forms.ListView();
			this.last = new System.Windows.Forms.ColumnHeader();
			this.first = new System.Windows.Forms.ColumnHeader();
			this.homePhone = new System.Windows.Forms.ColumnHeader();
			this.workPhone = new System.Windows.Forms.ColumnHeader();
			this.address = new System.Windows.Forms.ColumnHeader();
			this.select = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.errormsg = new System.Windows.Forms.Label();
			this.phone = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnNew = new System.Windows.Forms.Button();
			this.lookupName = new System.Windows.Forms.Button();
			this.lookupPhone = new System.Windows.Forms.Button();
			this.lastName = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lookupID
			// 
			this.lookupID.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lookupID.Location = new System.Drawing.Point(240, 16);
			this.lookupID.Name = "lookupID";
			this.lookupID.Size = new System.Drawing.Size(96, 20);
			this.lookupID.TabIndex = 2;
			this.lookupID.Text = "Lookup ID";
			this.lookupID.Click += new System.EventHandler(this.lookupID_Click);
			// 
			// id
			// 
			this.id.Location = new System.Drawing.Point(104, 16);
			this.id.Name = "id";
			this.id.Size = new System.Drawing.Size(128, 20);
			this.id.TabIndex = 1;
			this.id.TextChanged += new System.EventHandler(this.id_TextChanged);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(16, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "ID";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// firstName
			// 
			this.firstName.Location = new System.Drawing.Point(328, 80);
			this.firstName.Name = "firstName";
			this.firstName.Size = new System.Drawing.Size(128, 20);
			this.firstName.TabIndex = 9;
			this.firstName.TextChanged += new System.EventHandler(this.id_TextChanged);
			// 
			// resultsList
			// 
			this.resultsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.last,
            this.first,
            this.homePhone,
            this.workPhone,
            this.address});
			this.resultsList.FullRowSelect = true;
			this.resultsList.HideSelection = false;
			this.resultsList.Location = new System.Drawing.Point(24, 152);
			this.resultsList.MultiSelect = false;
			this.resultsList.Name = "resultsList";
			this.resultsList.Size = new System.Drawing.Size(544, 184);
			this.resultsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.resultsList.TabIndex = 11;
			this.resultsList.UseCompatibleStateImageBehavior = false;
			this.resultsList.View = System.Windows.Forms.View.Details;
			this.resultsList.DoubleClick += new System.EventHandler(this.select_Click);
			// 
			// last
			// 
			this.last.Text = "Last";
			this.last.Width = 78;
			// 
			// first
			// 
			this.first.Text = "First";
			this.first.Width = 93;
			// 
			// homePhone
			// 
			this.homePhone.Text = "Home Phone";
			this.homePhone.Width = 85;
			// 
			// workPhone
			// 
			this.workPhone.Text = "Work Phone";
			this.workPhone.Width = 85;
			// 
			// address
			// 
			this.address.Text = "Address";
			this.address.Width = 353;
			// 
			// select
			// 
			this.select.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.select.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.select.Location = new System.Drawing.Point(496, 40);
			this.select.Name = "select";
			this.select.Size = new System.Drawing.Size(72, 24);
			this.select.TabIndex = 13;
			this.select.Text = "Select";
			this.select.Click += new System.EventHandler(this.select_Click);
			// 
			// cancel
			// 
			this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancel.Location = new System.Drawing.Point(496, 120);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(72, 24);
			this.cancel.TabIndex = 14;
			this.cancel.Text = "Cancel";
			// 
			// errormsg
			// 
			this.errormsg.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.errormsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.errormsg.ForeColor = System.Drawing.Color.Red;
			this.errormsg.Location = new System.Drawing.Point(24, 103);
			this.errormsg.Name = "errormsg";
			this.errormsg.Size = new System.Drawing.Size(464, 41);
			this.errormsg.TabIndex = 9;
			this.errormsg.Text = "errormsg";
			this.errormsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// phone
			// 
			this.phone.Location = new System.Drawing.Point(104, 48);
			this.phone.Name = "phone";
			this.phone.Size = new System.Drawing.Size(128, 20);
			this.phone.TabIndex = 4;
			this.phone.TextChanged += new System.EventHandler(this.id_TextChanged);
			// 
			// label6
			// 
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.Location = new System.Drawing.Point(16, 48);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.TabIndex = 3;
			this.label6.Text = "label6";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnNew
			// 
			this.btnNew.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnNew.Location = new System.Drawing.Point(496, 8);
			this.btnNew.Name = "btnNew";
			this.btnNew.Size = new System.Drawing.Size(72, 24);
			this.btnNew.TabIndex = 12;
			this.btnNew.Text = "New";
			this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
			// 
			// lookupName
			// 
			this.lookupName.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lookupName.Location = new System.Drawing.Point(472, 80);
			this.lookupName.Name = "lookupName";
			this.lookupName.Size = new System.Drawing.Size(96, 20);
			this.lookupName.TabIndex = 10;
			this.lookupName.Text = "Lookup Name";
			this.lookupName.Click += new System.EventHandler(this.lookupName_Click);
			// 
			// lookupPhone
			// 
			this.lookupPhone.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lookupPhone.Location = new System.Drawing.Point(240, 48);
			this.lookupPhone.Name = "lookupPhone";
			this.lookupPhone.Size = new System.Drawing.Size(96, 20);
			this.lookupPhone.TabIndex = 5;
			this.lookupPhone.Text = "Lookup Phone";
			this.lookupPhone.Click += new System.EventHandler(this.lookupPhone_Click);
			// 
			// lastName
			// 
			this.lastName.Location = new System.Drawing.Point(104, 80);
			this.lastName.Name = "lastName";
			this.lastName.Size = new System.Drawing.Size(128, 20);
			this.lastName.TabIndex = 7;
			this.lastName.TextChanged += new System.EventHandler(this.id_TextChanged);
			// 
			// label7
			// 
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.Location = new System.Drawing.Point(16, 80);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(72, 16);
			this.label7.TabIndex = 6;
			this.label7.Text = "label7";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label8
			// 
			this.label8.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label8.Location = new System.Drawing.Point(256, 80);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(72, 16);
			this.label8.TabIndex = 8;
			this.label8.Text = "label8";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LookupDlg
			// 
			this.AcceptButton = this.select;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancel;
			this.ClientSize = new System.Drawing.Size(576, 342);
			this.ControlBox = false;
			this.Controls.Add(this.lookupPhone);
			this.Controls.Add(this.lookupName);
			this.Controls.Add(this.btnNew);
			this.Controls.Add(this.phone);
			this.Controls.Add(this.firstName);
			this.Controls.Add(this.id);
			this.Controls.Add(this.lastName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.errormsg);
			this.Controls.Add(this.resultsList);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lookupID);
			this.Controls.Add(this.select);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label8);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LookupDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Load += new System.EventHandler(this.LookupDlg_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// Add a customer record found in a search to the listview control.
		/// </summary>
		/// <param name="record"></param>
		private void fillList( CustomerWS.CustomerProviderCustomerRecord record )
		{
			if ( record != null )
			{
				ListViewItem item;

				item = resultsList.Items.Add( record.LastName );

				item.SubItems.Add( record.FirstName );
				item.SubItems.Add( record.PhoneHome );
				item.SubItems.Add( record.PhoneMobile );
				item.SubItems.Add( record.Street + " " + record.City + " " + record.State + " " + record.ZipCode );

				//item.Tag = (object)record;
				item.Tag = record;
			}
		}

		/// <summary>
		/// Look up a customer based on the customer ID.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lookupID_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				errormsg.Text = string.Empty;
				results = null;
				resultsList.Items.Clear();
				id.Text = id.Text.Trim();

				results = new CustomerWS.CustomerProviderCustomerRecord[1];
				results[0] = CustomerLookup.GetCustomerByID(id.Text);

				if ( results != null )
				{
					foreach ( CustomerWS.CustomerProviderCustomerRecord record in results )
					{
						fillList( record );
					}
				}
				if (resultsList.Items.Count > 0)
				{
					resultsList.Items[0].Selected = true;
					resultsList.Focus();
				}
				else
				{
					errormsg.Text = localize.LOOKUP_DLG_UNABLE_CUST_ID;
				}
			}
			catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
			{
				errormsg.Text = ex.Message;
			}
			catch ( System.Net.WebException wex )  // v1.02
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_IIS_ERROR, wex );
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.LOOKUP_DLG_UNABLE_CUST_RECORD, exp.Message );
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Search for a customer by a phone number.  There may be more than one
		/// phone number field searched.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lookupPhone_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				errormsg.Text = string.Empty;
				results = null;
				resultsList.Items.Clear();

				// Remove formatting from phone numbers
				string phoneNumber = phone.Text.Replace("(", string.Empty);
				phoneNumber = phoneNumber.Replace(") ", string.Empty);
				phoneNumber = phoneNumber.Replace("-", string.Empty);
				phoneNumber = phoneNumber.Trim();

				results = CustomerLookup.GetCustomersByANI(phoneNumber, 10);

				if ( results != null )
				{
					foreach ( CustomerWS.CustomerProviderCustomerRecord record in results )
					{
						fillList( record );
					}
				}
				if (resultsList.Items.Count > 0)
				{
					resultsList.Items[0].Selected = true;
					resultsList.Focus();
				}
				else
				{
					errormsg.Text = localize.LOOKUP_DLG_UNABLE_CUST_PHONE_NUMBER;
				}
			}
			catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
			{
				errormsg.Text = ex.Message;
			}
			catch ( System.Net.WebException wex )  // v1.02
			{
				Logging.Error( Application.ProductName, localize.DESKTOP_IIS_ERROR, wex );
			}
			catch ( Exception exp )
			{
				Logging.Error( Application.ProductName, localize.LOOKUP_DLG_UNABLE_CUST_RECORD, exp.Message );
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Search for a customer by their first and last name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lookupName_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				errormsg.Text = string.Empty;
				results = null;
				resultsList.Items.Clear();

				firstName.Text = firstName.Text.Trim();  // post 1.02 trim whitespace
				lastName.Text = lastName.Text.Trim();

				results = CustomerLookup.GetCustomersByName(firstName.Text, lastName.Text, 10);

				if (results != null)
				{
					foreach (CustomerWS.CustomerProviderCustomerRecord record in results)
					{
						fillList(record);
					}
				}
				if (resultsList.Items.Count > 0)
				{
					resultsList.Items[0].Selected = true;
					resultsList.Focus();
				}
				else
				{
					errormsg.Text = localize.LOOKUP_DLG_UNABLE_CUST_NAME;
				}
			}
			catch (System.ServiceModel.FaultException<System.ServiceModel.ExceptionDetail> ex)
			{
				errormsg.Text = ex.Message;
			}
			catch (System.Net.WebException wex)  // v1.02
			{
				Logging.Error(Application.ProductName, localize.DESKTOP_IIS_ERROR, wex);
			}
			catch (Exception exp)
			{
				// Post 1.02 - better error message
				Logging.Error(Application.ProductName, localize.LOOKUP_DLG_UNABLE_CUST_RECORD, exp);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Return to the caller the currently selected record.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void select_Click(object sender, EventArgs e)
		{
			errormsg.Text = "";
			if ( results != null && resultsList.SelectedItems.Count > 0 )
			{
				Result = resultsList.SelectedItems[0].Tag as CustomerWS.CustomerProviderCustomerRecord;
				DialogResult = DialogResult.OK;
			}
			else
			{
				errormsg.Text = localize.LOOKUP_DLG_NO_RECORD_SELECTED;
				DialogResult = DialogResult.None;
			}
		}

		/// <summary>
		/// This is used by all the text entry controls to deselect any results when
		/// text is entered or deleted.  This way a new search happens when enter is
		/// pressed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void id_TextChanged(object sender, EventArgs e)
		{
			// deselect any results so a new search will happen if enter is pressed
			resultsList.SelectedItems.Clear();
			errormsg.Text = "";
		}

		/// <summary>
		/// Returning OK with a null result indicates there is no customer match
		/// and an empty context needs to be created.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnNew_Click(object sender, EventArgs e)
		{
			Result = null;
			DialogResult = DialogResult.OK;
		}

		private void LookupDlg_Load(object sender, EventArgs e)
		{
			lookupID.Text = localize.LOOKUP_DLG_LOOKUP_ID;
			label1.Text = localize.LOOKUP_DLG_LABEL1;
			last.Text = localize.LOOKUP_DLG_LAST;
			first.Text = localize.LOOKUP_DLG_FIRST;
			homePhone.Text = localize.LOOKUP_DLG_HOME_PHONE;
			workPhone.Text = localize.LOOKUP_DLG_WORK_PHONE;
			address.Text = localize.LOOKUP_DLG_ADDRESS;
			select.Text = localize.LOOKUP_DLG_SELECT;
			cancel.Text = localize.LOOKUP_DLG_CANCEL;
			errormsg.Text = localize.LOOKUP_DLG_ERROR_MSG;
			label6.Text = localize.LOOKUP_DLG_LBL6;
			btnNew.Text = localize.LOOKUP_DLG_BTN_NEW;
			lookupName.Text = localize.LOOKUP_DLG_LOOKUP_NAME;
			lookupPhone.Text = localize.LOOKUP_DLG_LOOKUP_PHONE;
			label7.Text = localize.LOOKUP_DLG_LABEL7;
			label8.Text = localize.LOOKUP_DLG_LBEL8;
			Text = localize.LOOKUP_DLG_TEXT;

			errormsg.Text = "";

			phone.Text = "";
			firstName.Text = "";
			lastName.Text = "";

			if (Result != null)
			{
				results = new CustomerWS.CustomerProviderCustomerRecord[1];
				results[0] = Result;

				id.Text = Result.CustomerID;
				phone.Text = Result.PhoneHome;
				firstName.Text = Result.FirstName;
				lastName.Text = Result.LastName;

				// indicates we have a known record to show
				if (id.Text != null && !id.Text.Trim().Length.Equals(0))
				{
					fillList(Result);

					if (resultsList.Items.Count > 0)
					{
						resultsList.Items[0].Selected = true;
					}
				}
			}
			else
			{
				id.Text = "";
				firstName.Text = "";
				lastName.Text = "";
			}

			Activate();
		}
	}
}