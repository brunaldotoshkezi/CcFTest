namespace Microsoft.Ccf.Samples.Csr.AgentDesktop
{
    partial class LoginForm
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
			this.label1 = new System.Windows.Forms.Label();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.lblUserName = new System.Windows.Forms.Label();
			this.lblPassword = new System.Windows.Forms.Label();
			this.btnSubmit = new System.Windows.Forms.Button();
			this.lblError = new System.Windows.Forms.Label();
			this.lblDomain = new System.Windows.Forms.Label();
			this.txtDomain = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(91, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(137, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Please Login Here";
			// 
			// txtUsername
			// 
			this.txtUsername.Location = new System.Drawing.Point(120, 52);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(120, 20);
			this.txtUsername.TabIndex = 1;
			this.txtUsername.Enter += new System.EventHandler(this.txtUsername_Enter);
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(120, 98);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.Size = new System.Drawing.Size(120, 20);
			this.txtPassword.TabIndex = 2;
			this.txtPassword.UseSystemPasswordChar = true;
			this.txtPassword.Enter += new System.EventHandler(this.txtPassword_Enter);
			// 
			// lblUserName
			// 
			this.lblUserName.AutoSize = true;
			this.lblUserName.Location = new System.Drawing.Point(57, 55);
			this.lblUserName.Name = "lblUserName";
			this.lblUserName.Size = new System.Drawing.Size(57, 13);
			this.lblUserName.TabIndex = 3;
			this.lblUserName.Text = "UserName";
			// 
			// lblPassword
			// 
			this.lblPassword.AutoSize = true;
			this.lblPassword.Location = new System.Drawing.Point(57, 101);
			this.lblPassword.Name = "lblPassword";
			this.lblPassword.Size = new System.Drawing.Size(53, 13);
			this.lblPassword.TabIndex = 4;
			this.lblPassword.Text = "Password";
			// 
			// btnSubmit
			// 
			this.btnSubmit.Location = new System.Drawing.Point(72, 192);
			this.btnSubmit.Name = "btnSubmit";
			this.btnSubmit.Size = new System.Drawing.Size(95, 26);
			this.btnSubmit.TabIndex = 4;
			this.btnSubmit.Text = "Login";
			this.btnSubmit.UseVisualStyleBackColor = true;
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			// 
			// lblError
			// 
			this.lblError.ForeColor = System.Drawing.Color.Red;
			this.lblError.Location = new System.Drawing.Point(34, 228);
			this.lblError.Name = "lblError";
			this.lblError.Size = new System.Drawing.Size(266, 50);
			this.lblError.TabIndex = 6;
			this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblDomain
			// 
			this.lblDomain.AutoSize = true;
			this.lblDomain.Location = new System.Drawing.Point(57, 147);
			this.lblDomain.Name = "lblDomain";
			this.lblDomain.Size = new System.Drawing.Size(43, 13);
			this.lblDomain.TabIndex = 8;
			this.lblDomain.Text = "Domain";
			// 
			// txtDomain
			// 
			this.txtDomain.Location = new System.Drawing.Point(120, 144);
			this.txtDomain.Name = "txtDomain";
			this.txtDomain.Size = new System.Drawing.Size(120, 20);
			this.txtDomain.TabIndex = 3;
			this.txtDomain.Enter += new System.EventHandler(this.txtDomain_Enter);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(187, 192);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(95, 26);
			this.btnCancel.TabIndex = 9;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// LoginForm
			// 
			this.AcceptButton = this.btnSubmit;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(348, 287);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.lblDomain);
			this.Controls.Add(this.txtDomain);
			this.Controls.Add(this.lblError);
			this.Controls.Add(this.btnSubmit);
			this.Controls.Add(this.lblPassword);
			this.Controls.Add(this.lblUserName);
			this.Controls.Add(this.txtPassword);
			this.Controls.Add(this.txtUsername);
			this.Controls.Add(this.label1);
			this.Name = "LoginForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "LoginForm";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.TextBox txtDomain;
		private System.Windows.Forms.Button btnCancel;
    }
}