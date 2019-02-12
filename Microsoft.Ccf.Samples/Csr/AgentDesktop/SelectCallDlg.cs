//===============================================================================
// Microsoft Product – subject to the terms of the Microsoft EULA and other 
// agreements
//
// Customer Care Framework
// Copyright © 2003-2005 Microsoft Corporation. All rights reserved.
//===============================================================================
// SelectCallDlg.cs
//
// UI for selecting a call when there are multiple possibilities for the
// operation that is to be performed on the call.
//
// Revisions:
//     May 2003       v1.0  release
//     March 2004     v1.01 release
//     December 2004  v1.02 release
//     May 2005       V2.5
//
//===============================================================================

using System.Windows.Forms;
using Microsoft.Ccf.Csr.Cti.Providers;

namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
	/// <summary>
	/// Summary description for SelectCallDlg.
	/// </summary>
	public class SelectCallDlg : System.Windows.Forms.Form
	{
		private CallClassProvider selectedCall = null;

		private System.Windows.Forms.ListView callsList;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ColumnHeader startedCol;
		private System.Windows.Forms.ColumnHeader partiesCol;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader stateCol;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SelectCallDlg()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		public SelectCallDlg( LineClassProvider myLine, string command )
		{
			bool         showThisCall;
			ListViewItem item;

			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.startedCol.Text = localize.SELECT_CALL_DLG_STARTED_COL;
			this.stateCol.Text = localize.SELECT_CALL_DLG_STATE_COL;
			this.partiesCol.Text = localize.SELECT_CALL_DLG_PARTIES_COL;
			this.btnOk.Text = localize.SELECT_CALL_DLG_BTN_OK;
			this.btnCancel.Text = localize.SELECT_CALL_DLG_BTN_CANCEL;
			this.label1.Text = localize.SELECT_CALL_DLG_LABEL1;
			this.Text = localize.SELECT_CALL_DLG_TEXT;

			if ( command != null )
			{
				btnOk.Text = command;
				command = command.Replace( "&", "" );
				command = command.ToLower();
			}
			foreach ( CallClassProvider call in myLine.Calls )
			{
				showThisCall = false;
				if ( command == "unhold" && call.CanUnhold() )
				{
					showThisCall = true;
				}
				else if ( command == "hold" && call.CanHold() )
				{
					showThisCall = true;
				}
				else if ( command == "answer" && call.CanAnswer() )
				{                
					showThisCall = true;
				}
				else if ( command == "hangup" && call.CanHangup() )
				{
					showThisCall = true;
				}
				else if ( command == "transfer" && call.CanTransfer() )
				{
					showThisCall = true;
				}
				else if ( command == null )  // use as default for all
				{
					showThisCall = true;
				}

				if ( showThisCall )
				{
					item = callsList.Items.Add( call.Started.ToShortTimeString() );
					item.SubItems.Add( CallClassProvider.CallStateText(call.State) );

					if ( call.UserTag != null )
					{
						item.SubItems.Add( call.UserTag + "  " + call.Parties );
					}
					else
					{
						item.SubItems.Add( call.Parties );
					}
					item.Tag = call;
				}
			}
			if ( callsList.Items.Count > 0 )
			{
				callsList.Items[ 0 ].Selected = true;
			}
			if ( callsList.Items.Count == 1 )
			{
				selectedCall = (CallClassProvider)callsList.Items[ 0 ].Tag;
			}
		}

		/// <summary>
		/// The number of calls which match the request made when this dialog
		/// was created, such as which calls can be unheld or answered.
		/// </summary>
		public int MatchingCalls
		{
			get
			{
				return callsList.Items.Count;
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SelectCallDlg));
			this.callsList = new System.Windows.Forms.ListView();
			this.startedCol = new System.Windows.Forms.ColumnHeader();
			this.stateCol = new System.Windows.Forms.ColumnHeader();
			this.partiesCol = new System.Windows.Forms.ColumnHeader();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// callsList
			// 
			this.callsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						this.startedCol,
																						this.stateCol,
																						this.partiesCol});
			this.callsList.FullRowSelect = true;
			this.callsList.Location = new System.Drawing.Point(8, 32);
			this.callsList.MultiSelect = false;
			this.callsList.Name = "callsList";
			this.callsList.Size = new System.Drawing.Size(344, 96);
			this.callsList.TabIndex = 0;
			this.callsList.View = System.Windows.Forms.View.Details;
			this.callsList.DoubleClick += new System.EventHandler(this.btnOk_Click);
			// 
			// startedCol
			// 
			this.startedCol.Text = "Call Started";
			// 
			// stateCol
			// 
			this.stateCol.Text = "State";
			this.stateCol.Width = 100;
			// 
			// partiesCol
			// 
			this.partiesCol.Text = "Parties";
			this.partiesCol.Width = 180;
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOk.Location = new System.Drawing.Point(77, 136);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(96, 24);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Ok";
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(189, 136);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(96, 24);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(344, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "label1";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SelectCallDlg
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(362, 168);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.callsList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SelectCallDlg";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Call";
			this.ResumeLayout(false);

		}
		#endregion

		private void btnOk_Click(object sender, System.EventArgs e)
		{

			int sel = 0;
			// see if a call is selected
			if ( callsList.SelectedItems.Count == 0 )
			{
				MessageBox.Show(localize.SELECT_CALL_DLG_SELECT_A_CALL, this.Text, MessageBoxButtons.OK);
				DialogResult = DialogResult.None;
			}
			else
			{
				sel = callsList.SelectedIndices[0];
				selectedCall = (CallClassProvider)callsList.Items[ sel ].Tag;
				DialogResult = DialogResult.OK;
			}
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			// setting the selected call to null prevents code using this dialog
			// from taking action on a call.
			selectedCall = null;
			DialogResult = DialogResult.Cancel;
		}

		/// <summary>
		/// The call which a user has selected to be acted upon.
		/// </summary>
		public CallClassProvider SelectedCall
		{
			get
			{
				return selectedCall;
			}
		}

	}
}