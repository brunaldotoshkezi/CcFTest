//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2007 Microsoft Corporation. All rights reserved.
//
// QuickStart Web application
//
//===============================================================================
using System;

namespace Microsoft.Ccf.QuickStarts
{
	public partial class _Default : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				try
				{
					txtFirstName.Text = Request["txtFirstName"];
					txtLastName.Text = Request["txtLastName"];
					txtAddress.Text = Request["txtAddress"];
					txtID.Text = Request["txtID"];
				}
				catch //(Exception ex)
				{
					//handle it later
				}
			}
		}

		protected void update_Click(object sender, EventArgs e)
		{
		}
	}
}
