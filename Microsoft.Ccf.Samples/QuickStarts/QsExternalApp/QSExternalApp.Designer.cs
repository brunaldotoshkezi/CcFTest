namespace Microsoft.Ccf.QuickStarts
{
	partial class QSExternalApp
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.update = new System.Windows.Forms.Button();
			this.txtFirstName = new System.Windows.Forms.TextBox();
			this.lblFirstName = new System.Windows.Forms.Label();
			this.txtLastName = new System.Windows.Forms.TextBox();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.txtID = new System.Windows.Forms.TextBox();
			this.lblLastName = new System.Windows.Forms.Label();
			this.lblAddress = new System.Windows.Forms.Label();
			this.lblID = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// update
			// 
			this.update.Location = new System.Drawing.Point(47, 169);
			this.update.Name = "update";
			this.update.Size = new System.Drawing.Size(75, 23);
			this.update.TabIndex = 0;
			this.update.Text = "Update";
			this.update.UseVisualStyleBackColor = true;
			// 
			// txtFirstName
			// 
			this.txtFirstName.Location = new System.Drawing.Point(47, 32);
			this.txtFirstName.Name = "txtFirstName";
			this.txtFirstName.Size = new System.Drawing.Size(155, 20);
			this.txtFirstName.TabIndex = 1;
			// 
			// lblFirstName
			// 
			this.lblFirstName.AutoSize = true;
			this.lblFirstName.Location = new System.Drawing.Point(44, 16);
			this.lblFirstName.Name = "lblFirstName";
			this.lblFirstName.Size = new System.Drawing.Size(54, 13);
			this.lblFirstName.TabIndex = 2;
			this.lblFirstName.Text = "FirstName";
			// 
			// txtLastName
			// 
			this.txtLastName.Location = new System.Drawing.Point(223, 32);
			this.txtLastName.Name = "txtLastName";
			this.txtLastName.Size = new System.Drawing.Size(187, 20);
			this.txtLastName.TabIndex = 3;
			// 
			// txtAddress
			// 
			this.txtAddress.Location = new System.Drawing.Point(47, 81);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(363, 20);
			this.txtAddress.TabIndex = 4;
			// 
			// txtID
			// 
			this.txtID.Location = new System.Drawing.Point(47, 126);
			this.txtID.Name = "txtID";
			this.txtID.Size = new System.Drawing.Size(100, 20);
			this.txtID.TabIndex = 5;
			// 
			// lblLastName
			// 
			this.lblLastName.AutoSize = true;
			this.lblLastName.Location = new System.Drawing.Point(220, 16);
			this.lblLastName.Name = "lblLastName";
			this.lblLastName.Size = new System.Drawing.Size(58, 13);
			this.lblLastName.TabIndex = 6;
			this.lblLastName.Text = "Last Name";
			// 
			// lblAddress
			// 
			this.lblAddress.AutoSize = true;
			this.lblAddress.Location = new System.Drawing.Point(44, 65);
			this.lblAddress.Name = "lblAddress";
			this.lblAddress.Size = new System.Drawing.Size(45, 13);
			this.lblAddress.TabIndex = 7;
			this.lblAddress.Text = "Address";
			// 
			// lblID
			// 
			this.lblID.AutoSize = true;
			this.lblID.Location = new System.Drawing.Point(44, 110);
			this.lblID.Name = "lblID";
			this.lblID.Size = new System.Drawing.Size(65, 13);
			this.lblID.TabIndex = 8;
			this.lblID.Text = "Customer ID";
			// 
			// QSExternalApp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(470, 273);
			this.Controls.Add(this.lblID);
			this.Controls.Add(this.lblAddress);
			this.Controls.Add(this.lblLastName);
			this.Controls.Add(this.txtID);
			this.Controls.Add(this.txtAddress);
			this.Controls.Add(this.txtLastName);
			this.Controls.Add(this.lblFirstName);
			this.Controls.Add(this.txtFirstName);
			this.Controls.Add(this.update);
			this.Name = "QSExternalApp";
			this.Text = "QSExternalApp";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button update;
		private System.Windows.Forms.TextBox txtFirstName;
		private System.Windows.Forms.Label lblFirstName;
		private System.Windows.Forms.TextBox txtLastName;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.TextBox txtID;
		private System.Windows.Forms.Label lblLastName;
		private System.Windows.Forms.Label lblAddress;
		private System.Windows.Forms.Label lblID;
	}
}

