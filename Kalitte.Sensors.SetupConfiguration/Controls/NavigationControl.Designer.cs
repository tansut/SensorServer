namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    partial class NavigationControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.ctlCancelButton = new System.Windows.Forms.Button();
            this.ctlWizardBoard = new System.Windows.Forms.Panel();
            this.ctlErrorMessage = new System.Windows.Forms.Label();
            this.ctlNextButton = new System.Windows.Forms.Button();
            this.ctlErrorImage = new System.Windows.Forms.PictureBox();
            this.ctlStepLevel = new System.Windows.Forms.Label();
            this.ctlBackButton = new System.Windows.Forms.Button();
            this.ctlErrorDetail = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlErrorImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 6;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.ctlCancelButton, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.ctlWizardBoard, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ctlErrorMessage, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.ctlNextButton, 5, 2);
            this.tableLayoutPanel1.Controls.Add(this.ctlErrorImage, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ctlStepLevel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ctlBackButton, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.ctlErrorDetail, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(814, 475);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // ctlCancelButton
            // 
            this.ctlCancelButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ctlCancelButton.Location = new System.Drawing.Point(526, 447);
            this.ctlCancelButton.Name = "ctlCancelButton";
            this.ctlCancelButton.Size = new System.Drawing.Size(75, 23);
            this.ctlCancelButton.TabIndex = 10;
            this.ctlCancelButton.Text = "Cancel";
            this.ctlCancelButton.UseVisualStyleBackColor = true;
            this.ctlCancelButton.Click += new System.EventHandler(this.ctlCancelButton_Click);
            // 
            // ctlWizardBoard
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.ctlWizardBoard, 6);
            this.ctlWizardBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlWizardBoard.Location = new System.Drawing.Point(3, 28);
            this.ctlWizardBoard.Name = "ctlWizardBoard";
            this.ctlWizardBoard.Size = new System.Drawing.Size(808, 412);
            this.ctlWizardBoard.TabIndex = 6;
            // 
            // ctlErrorMessage
            // 
            this.ctlErrorMessage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ctlErrorMessage.AutoSize = true;
            this.ctlErrorMessage.Location = new System.Drawing.Point(35, 452);
            this.ctlErrorMessage.Name = "ctlErrorMessage";
            this.ctlErrorMessage.Size = new System.Drawing.Size(0, 13);
            this.ctlErrorMessage.TabIndex = 5;
            this.ctlErrorMessage.Visible = false;
            // 
            // ctlNextButton
            // 
            this.ctlNextButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ctlNextButton.Location = new System.Drawing.Point(726, 447);
            this.ctlNextButton.Name = "ctlNextButton";
            this.ctlNextButton.Size = new System.Drawing.Size(75, 23);
            this.ctlNextButton.TabIndex = 2;
            this.ctlNextButton.Text = "Next";
            this.ctlNextButton.UseVisualStyleBackColor = true;
            this.ctlNextButton.Click += new System.EventHandler(this.ctlNextButton_Click);
            // 
            // ctlErrorImage
            // 
            this.ctlErrorImage.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ctlErrorImage.Image = global::Kalitte.Sensors.SetupConfiguration.Properties.Resources.Remove_24x24;
            this.ctlErrorImage.Location = new System.Drawing.Point(3, 447);
            this.ctlErrorImage.Name = "ctlErrorImage";
            this.ctlErrorImage.Size = new System.Drawing.Size(26, 24);
            this.ctlErrorImage.TabIndex = 7;
            this.ctlErrorImage.TabStop = false;
            this.ctlErrorImage.Visible = false;
            // 
            // ctlStepLevel
            // 
            this.ctlStepLevel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ctlStepLevel.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.ctlStepLevel, 6);
            this.ctlStepLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.ctlStepLevel.Location = new System.Drawing.Point(3, 3);
            this.ctlStepLevel.Name = "ctlStepLevel";
            this.ctlStepLevel.Size = new System.Drawing.Size(0, 18);
            this.ctlStepLevel.TabIndex = 8;
            // 
            // ctlBackButton
            // 
            this.ctlBackButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ctlBackButton.Location = new System.Drawing.Point(626, 447);
            this.ctlBackButton.Name = "ctlBackButton";
            this.ctlBackButton.Size = new System.Drawing.Size(75, 23);
            this.ctlBackButton.TabIndex = 3;
            this.ctlBackButton.Text = "Back";
            this.ctlBackButton.UseVisualStyleBackColor = true;
            this.ctlBackButton.Click += new System.EventHandler(this.ctlBackButton_Click);
            // 
            // ctlErrorDetail
            // 
            this.ctlErrorDetail.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ctlErrorDetail.Location = new System.Drawing.Point(426, 447);
            this.ctlErrorDetail.Name = "ctlErrorDetail";
            this.ctlErrorDetail.Size = new System.Drawing.Size(75, 23);
            this.ctlErrorDetail.TabIndex = 11;
            this.ctlErrorDetail.Text = "Error Detail";
            this.ctlErrorDetail.UseVisualStyleBackColor = true;
            this.ctlErrorDetail.Visible = false;
            this.ctlErrorDetail.Click += new System.EventHandler(this.ctlErrorDetail_Click);
            // 
            // NavigationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "NavigationControl";
            this.Size = new System.Drawing.Size(814, 475);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlErrorImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel ctlWizardBoard;
        private System.Windows.Forms.Label ctlErrorMessage;
        private System.Windows.Forms.Button ctlNextButton;
        private System.Windows.Forms.Button ctlBackButton;
        private System.Windows.Forms.PictureBox ctlErrorImage;
        private System.Windows.Forms.Label ctlStepLevel;
        private System.Windows.Forms.Button ctlCancelButton;
        private System.Windows.Forms.Button ctlErrorDetail;
    }
}
