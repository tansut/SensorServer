namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    partial class SqlProvider
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ctlSqlServerAuthentication = new System.Windows.Forms.RadioButton();
            this.ctlWindowsAuthentication = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ctlAuthenticationSettingsLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ctlUsernameLabel = new System.Windows.Forms.Label();
            this.ctlPasswordLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.ctlServer = new System.Windows.Forms.TextBox();
            this.ctlUserName = new System.Windows.Forms.TextBox();
            this.ctlPassword = new System.Windows.Forms.TextBox();
            this.ctlDatabase = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.ctlAuthenticationSettingsLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(588, 326);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Provider Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ctlSqlServerAuthentication);
            this.groupBox2.Controls.Add(this.ctlWindowsAuthentication);
            this.groupBox2.Location = new System.Drawing.Point(19, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(552, 52);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Authentication Type";
            // 
            // ctlSqlServerAuthentication
            // 
            this.ctlSqlServerAuthentication.AutoSize = true;
            this.ctlSqlServerAuthentication.Location = new System.Drawing.Point(179, 22);
            this.ctlSqlServerAuthentication.Name = "ctlSqlServerAuthentication";
            this.ctlSqlServerAuthentication.Size = new System.Drawing.Size(151, 17);
            this.ctlSqlServerAuthentication.TabIndex = 1;
            this.ctlSqlServerAuthentication.Text = "SQL Server Authentication";
            this.ctlSqlServerAuthentication.UseVisualStyleBackColor = true;
            this.ctlSqlServerAuthentication.CheckedChanged += new System.EventHandler(this.ctlWindowsAuthentication_CheckedChanged);
            // 
            // ctlWindowsAuthentication
            // 
            this.ctlWindowsAuthentication.AutoSize = true;
            this.ctlWindowsAuthentication.Checked = true;
            this.ctlWindowsAuthentication.Location = new System.Drawing.Point(19, 22);
            this.ctlWindowsAuthentication.Name = "ctlWindowsAuthentication";
            this.ctlWindowsAuthentication.Size = new System.Drawing.Size(140, 17);
            this.ctlWindowsAuthentication.TabIndex = 0;
            this.ctlWindowsAuthentication.TabStop = true;
            this.ctlWindowsAuthentication.Text = "Windows Authentication";
            this.ctlWindowsAuthentication.UseVisualStyleBackColor = true;
            this.ctlWindowsAuthentication.CheckedChanged += new System.EventHandler(this.ctlWindowsAuthentication_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ctlAuthenticationSettingsLayoutPanel);
            this.groupBox3.Location = new System.Drawing.Point(19, 85);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(10);
            this.groupBox3.Size = new System.Drawing.Size(552, 138);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Authentication Settings";
            // 
            // ctlAuthenticationSettingsLayoutPanel
            // 
            this.ctlAuthenticationSettingsLayoutPanel.AutoSize = true;
            this.ctlAuthenticationSettingsLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ctlAuthenticationSettingsLayoutPanel.ColumnCount = 2;
            this.ctlAuthenticationSettingsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ctlAuthenticationSettingsLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.ctlPassword, 1, 2);
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.ctlUserName, 1, 1);
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.label4, 0, 3);
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.ctlPasswordLabel, 0, 2);
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.ctlUsernameLabel, 0, 1);
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.label1, 0, 0);
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.ctlServer, 1, 0);
            this.ctlAuthenticationSettingsLayoutPanel.Controls.Add(this.ctlDatabase, 1, 3);
            this.ctlAuthenticationSettingsLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctlAuthenticationSettingsLayoutPanel.Location = new System.Drawing.Point(10, 23);
            this.ctlAuthenticationSettingsLayoutPanel.Name = "ctlAuthenticationSettingsLayoutPanel";
            this.ctlAuthenticationSettingsLayoutPanel.RowCount = 4;
            this.ctlAuthenticationSettingsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ctlAuthenticationSettingsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ctlAuthenticationSettingsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ctlAuthenticationSettingsLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ctlAuthenticationSettingsLayoutPanel.Size = new System.Drawing.Size(532, 105);
            this.ctlAuthenticationSettingsLayoutPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server :";
            // 
            // ctlUsernameLabel
            // 
            this.ctlUsernameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ctlUsernameLabel.AutoSize = true;
            this.ctlUsernameLabel.Location = new System.Drawing.Point(31, 32);
            this.ctlUsernameLabel.Name = "ctlUsernameLabel";
            this.ctlUsernameLabel.Size = new System.Drawing.Size(66, 13);
            this.ctlUsernameLabel.TabIndex = 1;
            this.ctlUsernameLabel.Text = "User Name :";
            // 
            // ctlPasswordLabel
            // 
            this.ctlPasswordLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ctlPasswordLabel.AutoSize = true;
            this.ctlPasswordLabel.Location = new System.Drawing.Point(38, 58);
            this.ctlPasswordLabel.Name = "ctlPasswordLabel";
            this.ctlPasswordLabel.Size = new System.Drawing.Size(59, 13);
            this.ctlPasswordLabel.TabIndex = 2;
            this.ctlPasswordLabel.Text = "Password :";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Database :";
            // 
            // ctlServer
            // 
            this.ctlServer.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ctlServer.Location = new System.Drawing.Point(108, 3);
            this.ctlServer.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.ctlServer.Name = "ctlServer";
            this.ctlServer.Size = new System.Drawing.Size(217, 20);
            this.ctlServer.TabIndex = 4;
            this.ctlServer.Text = "localhost";
            // 
            // ctlUserName
            // 
            this.ctlUserName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ctlUserName.Location = new System.Drawing.Point(108, 29);
            this.ctlUserName.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.ctlUserName.Name = "ctlUserName";
            this.ctlUserName.Size = new System.Drawing.Size(217, 20);
            this.ctlUserName.TabIndex = 5;
            // 
            // ctlPassword
            // 
            this.ctlPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ctlPassword.Location = new System.Drawing.Point(108, 55);
            this.ctlPassword.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.ctlPassword.Name = "ctlPassword";
            this.ctlPassword.PasswordChar = '*';
            this.ctlPassword.Size = new System.Drawing.Size(217, 20);
            this.ctlPassword.TabIndex = 6;
            // 
            // ctlDatabase
            // 
            this.ctlDatabase.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ctlDatabase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ctlDatabase.FormattingEnabled = true;
            this.ctlDatabase.Location = new System.Drawing.Point(108, 81);
            this.ctlDatabase.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.ctlDatabase.Name = "ctlDatabase";
            this.ctlDatabase.Size = new System.Drawing.Size(217, 21);
            this.ctlDatabase.TabIndex = 7;
            this.ctlDatabase.DropDown += new System.EventHandler(this.ctlDatabase_DropDown);
            this.ctlDatabase.SelectedIndexChanged += new System.EventHandler(this.ctlDatabase_SelectedIndexChanged);
            // 
            // SqlProvider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.groupBox1);
            this.Name = "SqlProvider";
            this.Size = new System.Drawing.Size(614, 350);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ctlAuthenticationSettingsLayoutPanel.ResumeLayout(false);
            this.ctlAuthenticationSettingsLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton ctlSqlServerAuthentication;
        private System.Windows.Forms.RadioButton ctlWindowsAuthentication;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel ctlAuthenticationSettingsLayoutPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label ctlPasswordLabel;
        private System.Windows.Forms.Label ctlUsernameLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ctlPassword;
        private System.Windows.Forms.TextBox ctlUserName;
        private System.Windows.Forms.TextBox ctlServer;
        private System.Windows.Forms.ComboBox ctlDatabase;
    }
}
