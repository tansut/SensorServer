using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Core;
using Kalitte.Sensors.SetupConfiguration.Core.Data;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;
using Kalitte.Sensors.SetupConfiguration.Forms;
using Kalitte.Sensors.SetupConfiguration.Core.SetupConfigEditor;

namespace Kalitte.Sensors.SetupConfiguration.Controls
{
    public partial class NavigationControl : UserControl
    {
        private int currentIndex;
        public int CurrentIndex
        {
            get { return currentIndex; }
            set
            {
                currentIndex = value;
                UpdateNavigation(value);
            }
        }

        EditorList ConfigEditorList = null;

        SetupConfig Config;

        private void UpdateNavigation(int value)
        {
            if (value == 0 || value == ConfigEditorList.Count - 1) ctlBackButton.Enabled = false;
            else ctlBackButton.Enabled = true;
            if (value == ConfigEditorList.Count - 2) ctlNextButton.Text = "Configure";
            else if (value == ConfigEditorList.Count - 1) ctlNextButton.Text = "Close";
            else ctlNextButton.Text = "Next";
        }


        public NavigationControl()
        {
            InitializeComponent();
            ExceptionManager.OnUserException += new ExceptionManager.UserExceptionHandler(ExceptionManager_OnUserException);
            Config = new SetupConfig();
            ConfigEditorList = new EditorList();
            var initialControl = new IntroductionWizardControl();
            initialControl.Bind(Config);
            ConfigEditorList.Add(initialControl);
            ConfigEditorList.Add(new ServerConfigureWizardControl());
            ConfigEditorList.Add(new ProviderWizardControl());
            ConfigEditorList.Add(new IISConfigureWizardControl());
            ConfigEditorList.Add(new ConfigureWizardControl());
            ConfigEditorList.Add(new FinishWizardControl());
            foreach (IEditor<SetupConfig> item in ConfigEditorList)
            {
                if (item.IsProcessControl)
                {
                    item.ProcessingCompleted += new ProcessingCompletedHandler(item_IsProcessingCompleted);
                    item.ProcessingStarted += new ProcessingStartedHandler(item_ProcessingStarted);
                }
            }
            DoProgress(initialControl);
            CurrentIndex = 0;
        }

        void item_ProcessingStarted(object sender, EventArgs e)
        {
            this.ctlNextButton.Enabled = false;
            Application.DoEvents();
        }

        void item_IsProcessingCompleted(object sender, EventArgs e)
        {
            this.ctlNextButton.Enabled = true;
            Application.DoEvents();
        }

        void ExceptionManager_OnUserException(Exception e)
        {
            ctlErrorMessage.Text = e.Message;
            if (!string.IsNullOrWhiteSpace(((UserException)e).Detail))
                SetErrorLine(true, e);
            else SetErrorLine(true);
        }


        private void ctlNextButton_Click(object sender, EventArgs e)
        {
            var valid = ConfigEditorList[CurrentIndex].IsValid();
            SetErrorLine(!valid);
            if (valid)
            {
                var currentEditor = ConfigEditorList[CurrentIndex];
                currentEditor.Retrieve(Config);
                if (CurrentIndex != ConfigEditorList.Count - 1)
                {
                    var nextEditor = ConfigEditorList[CurrentIndex + 1];
                    nextEditor.Bind(Config);
                    DoProgress(nextEditor);
                }
                else
                {
                    Application.Exit();
                }
            }
        }

        private void DoProgress(SetupConfigEditor nextEditor)
        {
            ctlWizardBoard.SuspendLayout();
            int stepIndex = ConfigEditorList.IndexOf(nextEditor);
            SetWizardInfo(nextEditor, stepIndex);
            ctlWizardBoard.Controls.Clear();
            ctlWizardBoard.Controls.Add(nextEditor);
            ctlWizardBoard.ResumeLayout();
            CurrentIndex = stepIndex;
        }

        private void SetWizardInfo(SetupConfigEditor editor, int stepIndex)
        {
            var message = string.Format("Step {0} / {1}  - {2}", ConfigEditorList.IndexOf(editor) + 1, ConfigEditorList.Count, editor.WizardMessage);
            ctlStepLevel.Text = message;
        }

        private void SetErrorLine(bool show, Exception exp = null)
        {
            ctlErrorMessage.Visible = show;
            ctlErrorImage.Visible = show;

            if (!show) ctlErrorMessage.Text = string.Empty;
            else
            {
                if (exp != null)
                {
                    ctlErrorDetail.Visible = true;
                    ctlErrorDetail.Tag = exp;
                }
                else ctlErrorDetail.Visible = false;
            }
        }
        private void ctlBackButton_Click(object sender, EventArgs e)
        {
            var nextEditor = ConfigEditorList[CurrentIndex - 1];
            nextEditor.Bind(Config);
            DoProgress(nextEditor);
        }

        public void SetApplicationInstallPath(string path)
        {
            Config.ApplicationInstallPath = path;
        }

        private void ctlErrorDetail_Click(object sender, EventArgs e)
        {
            UserException exp = (UserException)ctlErrorDetail.Tag;
            ExceptionManager.ShowException(exp);
        }

        private void ctlCancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel configuration ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                Application.Exit();
        }
    }
}
