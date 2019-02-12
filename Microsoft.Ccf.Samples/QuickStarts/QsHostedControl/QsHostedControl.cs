//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
// QuickStart Hosted Control
//
//===============================================================================
using System;
using Microsoft.Ccf.Csr;

namespace Microsoft.Ccf.QuickStarts
{
	public partial class QsHostedControl : Microsoft.Ccf.Csr.HostedControl
	{
		public QsHostedControl()
		{
			InitializeComponent();
		}

		// Necessary constructor
		public QsHostedControl(int appID, string appName, string initString)
			: base(appID, appName, initString)
		{
			InitializeComponent();
		}

		private void QSHostedControl_Load(object sender, EventArgs e)
		{}

		// This is the context change event handler.
		public override void NotifyContextChange(Context context)
		{
			// This is the context change handler.

			// Populating text fields from context information.
			this.txtFirstName.Text = context["CustomerFirstName"];
			this.txtLastName.Text = context["CustomerLastName"];
			this.txtAddress.Text = context["Street"];
			this.txtID.Text = context["CustomerID"];

			// Hands control back over to the base class to notify next app of context change.
			base.NotifyContextChange(context);
		}

		// This is the action event handler
		public override void DoAction(Action action, string data)
		{

			//Check the action name to see if it's something we know how to handle and perform appropriate work
			switch (action.Name)
			{
				case "UpdateFirstName":
					this.txtFirstName.Text = data;
					break;

				case "UpdateLastName":
					this.txtLastName.Text = data;
					break;

				case "UpdateAddress":
					this.txtAddress.Text = data;
					break;
			}
		}

		private void updateData_Click(object sender, EventArgs e)
		{
			// This is how you fire an action to other hosted applications. Your DoAction() code
			// in your other application or application adapter will get called via this.
			this.FireRequestAction(new RequestActionEventArgs("QSExternalApplication", "UpdateFirstName", this.txtFirstName.Text));
		}

		private void btnFireContextChange_Click(object sender, EventArgs e)
		{
			// Get the current context and create a new context object from it.
			string temp = this.Context.GetContext();
			Context updatedContext = new Context(temp);

			// Update the new context with the changed information.
			updatedContext["CustomerFirstName"] = this.txtFirstName.Text;

			// Notify everyone of this new context information
			this.FireChangeContext(new ContextEventArgs(updatedContext));
			// Tell self about this change
			NotifyContextChange(updatedContext);

		}
	}
}