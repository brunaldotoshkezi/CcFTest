namespace Microsoft.Ccf.Samples.WfWorkflowControl
{
	/// <summary>
	/// Designer portion of WfWorkflowControl
	/// </summary>
	partial class WfWorkflowControl
	{
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(WfWorkflowControl));
			this.plHeadingPanel = new System.Windows.Forms.Panel();
			this.lbCurrentWorkflow = new System.Windows.Forms.Label();
			this.comboAvailableWorkflows = new System.Windows.Forms.ComboBox();
			this.lbAvlWorkflows = new System.Windows.Forms.Label();
			this.lvSteps = new System.Windows.Forms.ListView();
			this.dummyCol = new System.Windows.Forms.ColumnHeader();
			this.workflowListViewImages = new System.Windows.Forms.ImageList(this.components);
			this.picHelpIcon = new System.Windows.Forms.PictureBox();
			this.helpContent = new System.Windows.Forms.Label();
			this.Help = new System.Windows.Forms.Label();
			this.helpArrowImages = new System.Windows.Forms.ImageList(this.components);
			this.Separator = new System.Windows.Forms.Label();
			this.Cancel = new System.Windows.Forms.Label();
			this.Next = new System.Windows.Forms.Label();
			this.Previous = new System.Windows.Forms.Label();
			this.WorkflowSteps = new System.Windows.Forms.Label();
			this.StartDone = new System.Windows.Forms.Label();
			this.workflowNameToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.plHeadingPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// plHeadingPanel
			// 
			this.plHeadingPanel.Controls.Add(this.lbCurrentWorkflow);
			this.plHeadingPanel.Dock = System.Windows.Forms.DockStyle.Top;
			this.plHeadingPanel.Location = new System.Drawing.Point(0, 0);
			this.plHeadingPanel.Name = "plHeadingPanel";
			this.plHeadingPanel.Size = new System.Drawing.Size(272, 25);
			this.plHeadingPanel.TabIndex = 0;
			this.plHeadingPanel.Resize += new System.EventHandler(this.plHeadingPanel_Resize);
			this.plHeadingPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.plHeadingPanel_Paint);
			// 
			// lbCurrentWorkflow
			// 
			this.lbCurrentWorkflow.BackColor = System.Drawing.Color.Transparent;
			this.lbCurrentWorkflow.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbCurrentWorkflow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbCurrentWorkflow.ForeColor = System.Drawing.Color.RoyalBlue;
			this.lbCurrentWorkflow.Image = ((System.Drawing.Image)(resources.GetObject("lbCurrentWorkflow.Image")));
			this.lbCurrentWorkflow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbCurrentWorkflow.Location = new System.Drawing.Point(0, 0);
			this.lbCurrentWorkflow.Name = "lbCurrentWorkflow";
			this.lbCurrentWorkflow.Size = new System.Drawing.Size(272, 25);
			this.lbCurrentWorkflow.TabIndex = 0;
			this.lbCurrentWorkflow.Text = "Current Workflow";
			this.lbCurrentWorkflow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbCurrentWorkflow.Paint += new System.Windows.Forms.PaintEventHandler(this.lbCurrentWorkflow_Paint);
			// 
			// comboAvailableWorkflows
			// 
			this.comboAvailableWorkflows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboAvailableWorkflows.Location = new System.Drawing.Point(80, 32);
			this.comboAvailableWorkflows.Name = "comboAvailableWorkflows";
			this.comboAvailableWorkflows.Size = new System.Drawing.Size(136, 21);
			this.comboAvailableWorkflows.TabIndex = 2;
			this.workflowNameToolTip.SetToolTip(this.comboAvailableWorkflows, "Name of the workflow selected in workflows combo");
			this.comboAvailableWorkflows.SelectedIndexChanged += new System.EventHandler(this.comboAvailableWorkflows_SelectedIndexChanged);
			// 
			// lbAvlWorkflows
			// 
			this.lbAvlWorkflows.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lbAvlWorkflows.ForeColor = System.Drawing.Color.Black;
			this.lbAvlWorkflows.Location = new System.Drawing.Point(8, 32);
			this.lbAvlWorkflows.Name = "lbAvlWorkflows";
			this.lbAvlWorkflows.Size = new System.Drawing.Size(64, 23);
			this.lbAvlWorkflows.TabIndex = 1;
			this.lbAvlWorkflows.Text = "Workflows";
			this.lbAvlWorkflows.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lvSteps
			// 
			this.lvSteps.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																					  this.dummyCol});
			this.lvSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lvSteps.ForeColor = System.Drawing.Color.Black;
			this.lvSteps.FullRowSelect = true;
			this.lvSteps.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvSteps.Location = new System.Drawing.Point(8, 80);
			this.lvSteps.MultiSelect = false;
			this.lvSteps.Name = "lvSteps";
			this.lvSteps.Size = new System.Drawing.Size(240, 112);
			this.lvSteps.SmallImageList = this.workflowListViewImages;
			this.lvSteps.TabIndex = 5;
			this.lvSteps.View = System.Windows.Forms.View.Details;
			this.lvSteps.Click += new System.EventHandler(this.lvSteps_Click);
			this.lvSteps.SelectedIndexChanged += new System.EventHandler(this.lvSteps_SelectedIndexChanged);
			// 
			// dummyCol
			// 
			this.dummyCol.Width = 230;
			// 
			// workflowListViewImages
			// 
			this.workflowListViewImages.ImageSize = new System.Drawing.Size(16, 16);
			this.workflowListViewImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("workflowListViewImages.ImageStream")));
			this.workflowListViewImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// picHelpIcon
			// 
			this.picHelpIcon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.picHelpIcon.Image = ((System.Drawing.Image)(resources.GetObject("picHelpIcon.Image")));
			this.picHelpIcon.Location = new System.Drawing.Point(16, 240);
			this.picHelpIcon.Name = "picHelpIcon";
			this.picHelpIcon.Size = new System.Drawing.Size(16, 16);
			this.picHelpIcon.TabIndex = 11;
			this.picHelpIcon.TabStop = false;
			this.picHelpIcon.Click += new System.EventHandler(this.Help_Click);
			// 
			// helpContent
			// 
			this.helpContent.ForeColor = System.Drawing.Color.RoyalBlue;
			this.helpContent.Location = new System.Drawing.Point(16, 264);
			this.helpContent.Name = "helpContent";
			this.helpContent.Size = new System.Drawing.Size(246, 40);
			this.helpContent.TabIndex = 10;
			this.helpContent.Text = "Help Contents";
			this.helpContent.Visible = false;
			// 
			// Help
			// 
			this.Help.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Help.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Help.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Help.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Help.ImageIndex = 1;
			this.Help.ImageList = this.helpArrowImages;
			this.Help.Location = new System.Drawing.Point(40, 240);
			this.Help.Name = "Help";
			this.Help.Size = new System.Drawing.Size(48, 16);
			this.Help.TabIndex = 9;
			this.Help.Text = "Help";
			this.Help.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Help.Click += new System.EventHandler(this.Help_Click);
			// 
			// helpArrowImages
			// 
			this.helpArrowImages.ImageSize = new System.Drawing.Size(16, 16);
			this.helpArrowImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("helpArrowImages.ImageStream")));
			this.helpArrowImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// Separator
			// 
			this.Separator.BackColor = System.Drawing.Color.Lavender;
			this.Separator.ForeColor = System.Drawing.Color.Lavender;
			this.Separator.Location = new System.Drawing.Point(8, 232);
			this.Separator.Name = "Separator";
			this.Separator.Size = new System.Drawing.Size(256, 3);
			this.Separator.TabIndex = 8;
			this.Separator.Resize += new System.EventHandler(this.WorkFlow_Resize);
			this.Separator.Paint += new System.Windows.Forms.PaintEventHandler(this.Separator_Paint);
			// 
			// Cancel
			// 
			this.Cancel.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Cancel.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Cancel.Location = new System.Drawing.Point(208, 200);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(45, 23);
			this.Cancel.TabIndex = 8;
			this.Cancel.Text = "Cancel";
			this.Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
			// 
			// Next
			// 
			this.Next.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Next.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Next.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Next.Image = ((System.Drawing.Image)(resources.GetObject("Next.Image")));
			this.Next.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Next.Location = new System.Drawing.Point(120, 200);
			this.Next.Name = "Next";
			this.Next.Size = new System.Drawing.Size(64, 23);
			this.Next.TabIndex = 7;
			this.Next.Text = "Next";
			this.Next.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Next.Click += new System.EventHandler(this.Next_Click);
			// 
			// Previous
			// 
			this.Previous.Cursor = System.Windows.Forms.Cursors.Hand;
			this.Previous.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Previous.ForeColor = System.Drawing.Color.RoyalBlue;
			this.Previous.Image = ((System.Drawing.Image)(resources.GetObject("Previous.Image")));
			this.Previous.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Previous.Location = new System.Drawing.Point(16, 200);
			this.Previous.Name = "Previous";
			this.Previous.Size = new System.Drawing.Size(80, 23);
			this.Previous.TabIndex = 6;
			this.Previous.Text = "Previous";
			this.Previous.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.Previous.Click += new System.EventHandler(this.Previous_Click);
			// 
			// WorkflowSteps
			// 
			this.WorkflowSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.WorkflowSteps.ForeColor = System.Drawing.Color.Black;
			this.WorkflowSteps.Location = new System.Drawing.Point(8, 64);
			this.WorkflowSteps.Name = "WorkflowSteps";
			this.WorkflowSteps.Size = new System.Drawing.Size(96, 16);
			this.WorkflowSteps.TabIndex = 4;
			this.WorkflowSteps.Text = "Workflow Steps";
			this.WorkflowSteps.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StartDone
			// 
			this.StartDone.Cursor = System.Windows.Forms.Cursors.Hand;
			this.StartDone.Enabled = false;
			this.StartDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.StartDone.ForeColor = System.Drawing.Color.RoyalBlue;
			this.StartDone.Location = new System.Drawing.Point(224, 32);
			this.StartDone.Name = "StartDone";
			this.StartDone.Size = new System.Drawing.Size(32, 24);
			this.StartDone.TabIndex = 3;
			this.StartDone.Text = "Start";
			this.StartDone.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.StartDone.Click += new System.EventHandler(this.StartDone_Click);
			// 
			// WorkFlow
			// 
			this.BackColor = System.Drawing.SystemColors.Window;
			this.Controls.Add(this.StartDone);
			this.Controls.Add(this.WorkflowSteps);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Next);
			this.Controls.Add(this.Previous);
			this.Controls.Add(this.Help);
			this.Controls.Add(this.helpContent);
			this.Controls.Add(this.picHelpIcon);
			this.Controls.Add(this.Separator);
			this.Controls.Add(this.lbAvlWorkflows);
			this.Controls.Add(this.comboAvailableWorkflows);
			this.Controls.Add(this.plHeadingPanel);
			this.Controls.Add(this.lvSteps);
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.Name = "WorkFlow";
			this.Size = new System.Drawing.Size(272, 320);
			this.Resize += new System.EventHandler(this.WorkFlow_Resize);
			this.plHeadingPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private System.Windows.Forms.Panel plHeadingPanel;
		private System.Windows.Forms.Label lbCurrentWorkflow;
		private System.Windows.Forms.ComboBox comboAvailableWorkflows;
		private System.Windows.Forms.Label lbAvlWorkflows;
		private System.Windows.Forms.PictureBox picHelpIcon;
		private System.Windows.Forms.Label helpContent;
		private System.Windows.Forms.Label Help;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.ListView lvSteps;
		private System.Windows.Forms.Label Separator;
		private System.Windows.Forms.Label Cancel;
		private System.Windows.Forms.Label Next;
		private System.Windows.Forms.Label Previous;
		private System.Windows.Forms.Label WorkflowSteps;
		private System.Windows.Forms.Label StartDone;
		private System.Windows.Forms.ColumnHeader dummyCol;
		private System.Windows.Forms.ImageList workflowListViewImages;
		private System.Windows.Forms.ImageList helpArrowImages;
		private System.Windows.Forms.ToolTip workflowNameToolTip;
	}
}