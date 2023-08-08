namespace Kalitte.Sensors.SetupConfiguration.Forms
{
    partial class ExceptionForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ctlStackTrace = new System.Windows.Forms.TextBox();
            this.ctlMessage = new System.Windows.Forms.TextBox();
            this.ctlPicture = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.ctlPicture, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.ctlMessage, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(680, 220);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.groupBox1, 2);
            this.groupBox1.Controls.Add(this.ctlStackTrace);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 73);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(674, 144);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detail";
            // 
            // ctlStackTrace
            // 
            this.ctlStackTrace.AcceptsReturn = true;
            this.ctlStackTrace.AcceptsTab = true;
            this.ctlStackTrace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlStackTrace.Location = new System.Drawing.Point(3, 16);
            this.ctlStackTrace.Multiline = true;
            this.ctlStackTrace.Name = "ctlStackTrace";
            this.ctlStackTrace.ReadOnly = true;
            this.ctlStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ctlStackTrace.Size = new System.Drawing.Size(668, 125);
            this.ctlStackTrace.TabIndex = 0;
            this.ctlStackTrace.WordWrap = false;
            // 
            // ctlMessage
            // 
            this.ctlMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlMessage.Location = new System.Drawing.Point(73, 3);
            this.ctlMessage.Multiline = true;
            this.ctlMessage.Name = "ctlMessage";
            this.ctlMessage.ReadOnly = true;
            this.ctlMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ctlMessage.Size = new System.Drawing.Size(604, 64);
            this.ctlMessage.TabIndex = 3;
            // 
            // ctlPicture
            // 
            this.ctlPicture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlPicture.Image = global::Kalitte.Sensors.SetupConfiguration.Properties.Resources.Remove_48x48;
            this.ctlPicture.Location = new System.Drawing.Point(3, 3);
            this.ctlPicture.Name = "ctlPicture";
            this.ctlPicture.Size = new System.Drawing.Size(64, 64);
            this.ctlPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ctlPicture.TabIndex = 2;
            this.ctlPicture.TabStop = false;
            // 
            // ExceptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 220);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ExceptionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Error";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ctlPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ctlStackTrace;
        private System.Windows.Forms.PictureBox ctlPicture;
        private System.Windows.Forms.TextBox ctlMessage;
    }
}