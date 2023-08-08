using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Helpers;
using System.Data.SqlClient;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;

namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    public partial class SqlProvider : UserControl
    {
        public SqlProvider()
        {
            InitializeComponent();
        }

        SqlConnectionStringBuilder Current;

        private void ctlWindowsAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            SetAuthenticationSettings(ctlWindowsAuthentication.Checked);
        }

        private void SetAuthenticationSettings(bool check)
        {
            ctlAuthenticationSettingsLayoutPanel.SuspendLayout();
            if (check)
            {
                ctlUserName.Visible = false;
                ctlPassword.Visible = false;
                ctlUsernameLabel.Visible = false;
                ctlPasswordLabel.Visible = false;
            }
            else
            {
                ctlUserName.Visible = true;
                ctlPassword.Visible = true;
                ctlUsernameLabel.Visible = true;
                ctlPasswordLabel.Visible = true;
            }
            ctlAuthenticationSettingsLayoutPanel.ResumeLayout();
        }

        private void ctlDatabase_DropDown(object sender, EventArgs e)
        {
            SetDatabases();
        }

        private void SetDatabases()
        {
            if (ctlWindowsAuthentication.Checked)
            {
                ctlDatabase.DataSource = SqlHelper.GetDatabases(SqlHelper.CreateConnectionString(ctlServer.Text.Trim()));
            }
            else ctlDatabase.DataSource = SqlHelper.GetDatabases(SqlHelper.CreateConnectionString(ctlServer.Text.Trim(), ctlUserName.Text.Trim(), ctlPassword.Text.Trim()));
        }

        public bool IsValid()
        {
            if (Current != null)
            {
                if (string.IsNullOrWhiteSpace(Current.DataSource)) throw new UserException("Server cannot be blank");
                if (string.IsNullOrWhiteSpace(Current.InitialCatalog)) throw new UserException("Database cannot be blank");
                if (!ctlWindowsAuthentication.Checked)
                {
                    if (string.IsNullOrWhiteSpace(Current.UserID)) throw new UserException("Username cannot be blank");
                    if (string.IsNullOrWhiteSpace(Current.Password)) throw new UserException("Password cannot be blank");
                }
            }
            else throw new UserException("Database cannot be blank");
            return true;
        }

        public void Bind(string conStr)
        {
            SetAuthenticationSettings(ctlWindowsAuthentication.Checked);
            if (!string.IsNullOrWhiteSpace(conStr))
            {
                Current = new SqlConnectionStringBuilder(conStr);
                ctlServer.Text = Current.DataSource;
                ctlUserName.Text = Current.UserID;
                ctlPassword.Text = Current.Password;
                ctlServer.Text = Current.DataSource;
                SetDatabases();
                ctlDatabase.SelectedValue = Current.InitialCatalog;
            }
        }

        public SqlConnectionStringBuilder Retrieve()
        {
            return Current;
        }


        public void SetCurrent()
        {
            string path = null;
            if (ctlWindowsAuthentication.Checked)
            {
                path = SqlHelper.CreateConnectionString(ctlServer.Text.Trim(), ctlDatabase.SelectedValue.ToString());
            }
            else path = SqlHelper.CreateConnectionString(ctlServer.Text.Trim(), ctlUserName.Text.Trim(), ctlPassword.Text.Trim(), ctlDatabase.SelectedValue.ToString());
            Current = new SqlConnectionStringBuilder(path);
        }
        private void ctlDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCurrent();
        }
    }
}
