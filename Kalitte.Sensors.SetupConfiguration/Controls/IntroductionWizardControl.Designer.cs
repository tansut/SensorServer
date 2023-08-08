namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    partial class IntroductionWizardControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IntroductionWizardControl));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ctlRefresh = new System.Windows.Forms.Button();
            this.ctlPrerequisites = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Prerequisite = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.State = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ctlImageList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.ctlBrowse = new System.Windows.Forms.Button();
            this.ctlApplicationPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 620F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 12F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(700, 418);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ctlRefresh);
            this.panel1.Controls.Add(this.ctlPrerequisites);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.ctlBrowse);
            this.panel1.Controls.Add(this.ctlApplicationPath);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(71, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(614, 392);
            this.panel1.TabIndex = 0;
            // 
            // ctlRefresh
            // 
            this.ctlRefresh.Location = new System.Drawing.Point(513, 299);
            this.ctlRefresh.Name = "ctlRefresh";
            this.ctlRefresh.Size = new System.Drawing.Size(93, 23);
            this.ctlRefresh.TabIndex = 7;
            this.ctlRefresh.Text = "Refresh";
            this.ctlRefresh.UseVisualStyleBackColor = true;
            this.ctlRefresh.Click += new System.EventHandler(this.ctlRefresh_Click);
            // 
            // ctlPrerequisites
            // 
            this.ctlPrerequisites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.Prerequisite,
            this.State});
            this.ctlPrerequisites.FullRowSelect = true;
            this.ctlPrerequisites.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ctlPrerequisites.Location = new System.Drawing.Point(8, 211);
            this.ctlPrerequisites.Name = "ctlPrerequisites";
            this.ctlPrerequisites.ShowGroups = false;
            this.ctlPrerequisites.Size = new System.Drawing.Size(598, 82);
            this.ctlPrerequisites.SmallImageList = this.ctlImageList;
            this.ctlPrerequisites.TabIndex = 6;
            this.ctlPrerequisites.UseCompatibleStateImageBehavior = false;
            this.ctlPrerequisites.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "";
            this.columnHeader1.Width = 25;
            // 
            // Prerequisite
            // 
            this.Prerequisite.Text = "Prerequisite";
            this.Prerequisite.Width = 250;
            // 
            // State
            // 
            this.State.Text = "State";
            this.State.Width = 310;
            // 
            // ctlImageList
            // 
            this.ctlImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ctlImageList.ImageStream")));
            this.ctlImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ctlImageList.Images.SetKeyName(0, "Check_16x16.png");
            this.ctlImageList.Images.SetKeyName(1, "Warning.png");
            this.ctlImageList.Images.SetKeyName(2, "Remove_16x16.png");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(5, 180);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 16);
            this.label4.TabIndex = 5;
            this.label4.Text = "Prerequisites";
            // 
            // ctlBrowse
            // 
            this.ctlBrowse.Location = new System.Drawing.Point(513, 132);
            this.ctlBrowse.Name = "ctlBrowse";
            this.ctlBrowse.Size = new System.Drawing.Size(93, 23);
            this.ctlBrowse.TabIndex = 3;
            this.ctlBrowse.Text = "Browse";
            this.ctlBrowse.UseVisualStyleBackColor = true;
            this.ctlBrowse.Click += new System.EventHandler(this.ctlBrowse_Click);
            // 
            // ctlApplicationPath
            // 
            this.ctlApplicationPath.Location = new System.Drawing.Point(101, 134);
            this.ctlApplicationPath.Name = "ctlApplicationPath";
            this.ctlApplicationPath.Size = new System.Drawing.Size(406, 20);
            this.ctlApplicationPath.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Application Path :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(430, 91);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // IntroductionWizardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.DoubleBuffered = true;
            this.Name = "IntroductionWizardControl";
            this.Size = new System.Drawing.Size(700, 418);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ctlBrowse;
        private System.Windows.Forms.TextBox ctlApplicationPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ctlRefresh;
        private System.Windows.Forms.ListView ctlPrerequisites;
        private System.Windows.Forms.ColumnHeader Prerequisite;
        private System.Windows.Forms.ColumnHeader State;
        private System.Windows.Forms.ImageList ctlImageList;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
