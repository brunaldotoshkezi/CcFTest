namespace Microsoft.Ccf.QuickStarts
{
    partial class QsHostedControl
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Microsoft.Ccf.Csr.Context context1 = new Microsoft.Ccf.Csr.Context();
            this.firstName = new System.Windows.Forms.Label();
            this.lastName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.updateData = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtID = new System.Windows.Forms.TextBox();
            this.address = new System.Windows.Forms.Label();
            this.customerID = new System.Windows.Forms.Label();
            this.btnFireContextChange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // firstName
            // 
            this.firstName.AutoSize = true;
            this.firstName.Location = new System.Drawing.Point(16, 43);
            this.firstName.Name = "firstName";
            this.firstName.Size = new System.Drawing.Size(57, 13);
            this.firstName.TabIndex = 1;
            this.firstName.Text = "First Name";
            // 
            // lastName
            // 
            this.lastName.AutoSize = true;
            this.lastName.Location = new System.Drawing.Point(191, 43);
            this.lastName.Name = "lastName";
            this.lastName.Size = new System.Drawing.Size(58, 13);
            this.lastName.TabIndex = 2;
            this.lastName.Text = "Last Name";
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(19, 59);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(152, 20);
            this.txtFirstName.TabIndex = 3;
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(194, 59);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(165, 20);
            this.txtLastName.TabIndex = 4;
            // 
            // updateData
            // 
            this.updateData.Location = new System.Drawing.Point(19, 194);
            this.updateData.Name = "updateData";
            this.updateData.Size = new System.Drawing.Size(75, 23);
            this.updateData.TabIndex = 5;
            this.updateData.Text = "Update";
            this.updateData.UseVisualStyleBackColor = true;
            this.updateData.Click += new System.EventHandler(this.updateData_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(19, 103);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(340, 20);
            this.txtAddress.TabIndex = 6;
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(19, 153);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(75, 20);
            this.txtID.TabIndex = 7;
            // 
            // address
            // 
            this.address.AutoSize = true;
            this.address.Location = new System.Drawing.Point(16, 87);
            this.address.Name = "address";
            this.address.Size = new System.Drawing.Size(45, 13);
            this.address.TabIndex = 8;
            this.address.Text = "Address";
            // 
            // customerID
            // 
            this.customerID.AutoSize = true;
            this.customerID.Location = new System.Drawing.Point(16, 137);
            this.customerID.Name = "customerID";
            this.customerID.Size = new System.Drawing.Size(18, 13);
            this.customerID.TabIndex = 9;
            this.customerID.Text = "ID";
            // 
            // btnFireContextChange
            // 
            this.btnFireContextChange.Location = new System.Drawing.Point(194, 194);
            this.btnFireContextChange.Name = "btnFireContextChange";
            this.btnFireContextChange.Size = new System.Drawing.Size(115, 23);
            this.btnFireContextChange.TabIndex = 10;
            this.btnFireContextChange.Text = "FireContextChange";
            this.btnFireContextChange.UseVisualStyleBackColor = true;
            this.btnFireContextChange.Click += new System.EventHandler(this.btnFireContextChange_Click);
            // 
            // QSHostedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            context1.ContextInformation = "<CcfContext></CcfContext>";
            this.Context = context1;
            this.Controls.Add(this.btnFireContextChange);
            this.Controls.Add(this.customerID);
            this.Controls.Add(this.address);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.updateData);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lastName);
            this.Controls.Add(this.firstName);
            this.Name = "QSHostedControl";
            this.Size = new System.Drawing.Size(428, 307);
            this.Load += new System.EventHandler(this.QSHostedControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label firstName;
        private System.Windows.Forms.Label lastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.Button updateData;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label address;
        private System.Windows.Forms.Label customerID;
        private System.Windows.Forms.Button btnFireContextChange;
    }
}
