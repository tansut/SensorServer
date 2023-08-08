using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kalitte.Sensors.Web.Business;
using Ext.Net;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using Kalitte.Sensors.Utilities;
using System.Globalization;

namespace Kalitte.Sensors.Web.UI.Controls.Site
{
    public partial class ServerWatchVisualizer : System.Web.UI.UserControl
    {


        private ServerAnalysisBusiness business = new ServerAnalysisBusiness();

        public int MaxBars { get; set; }
        public int BarWidth { get; set; }
        public int BarSpace { get; set; }
        public int Duration
        {
            get { return taskManager.Interval; }
            set
            {
                taskManager.Interval = value;
                taskManager.Tasks[0].Interval = value;
            }
        }


        public Unit Height { get { return tblData.Height; } set { tblData.Height = value; } }


        public ServerWatchVisualizer()
            : base()
        {
            NewWindowBtnVisible = true;
            MaxBars = 100;
            BarWidth = 4;
            BarSpace = 1;
        }

        public void Reset()
        {

                Stop();
                Clear();
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            ctlNewWindow.Visible = NewWindowBtnVisible;
            base.OnPreRender(e);
        }


        private List<float> ChartData
        {
            get
            {
                if (string.IsNullOrEmpty(ctlDataList.Text))
                    return null;
                else
                {
                    return (List<float>)SerializationHelper.BinaryDeSerialize(ctlDataList.Text);
                }
            }
            set
            {
                ctlDataList.Text = value == null ? "": SerializationHelper.BinarySerialize(value);
            }
        }



        private float? DataHigh
        {
            get
            {
                if (string.IsNullOrEmpty(ctlMaxValHoder.Text))
                    return null;
                else return float.Parse(ctlMaxValHoder.Text, CultureInfo.InvariantCulture);
            }
            set
            {
                if (value.HasValue)
                    ctlMaxValHoder.Text = value.Value.ToString(CultureInfo.InvariantCulture);
                else ctlMaxValHoder.Text = "";
            }
        }


        public bool NewWindowBtnVisible { get; set; }

        public string WatchName
        {
            get
            {
                return (string)ViewState["WatchName"];
            }

            set
            {
                ViewState["WatchName"] = value;
            }
        }

        public string CategoryName
        {
            get
            {
                return (string)ViewState["CategoryName"];
            }

            set
            {
                ViewState["CategoryName"] = value;
            }
        }

        public string InstanceName
        {
            get
            {
                return (string)ViewState["InstanceName"];
            }

            set
            {
                ViewState["InstanceName"] = value;
            }
        }

        public string[] MeasureNames
        {
            get
            {

                return (string[])ViewState["MeasureNames"];
            }

            set
            {
                ViewState["MeasureNames"] = value;
            }
        }




        private void DrawChart()
        {
            try
            {
                while (tblData.Rows[0].Cells.Count > 2)
                    tblData.Rows[0].Cells.RemoveAt(1);

                float fMax = 0.0f;

                List<float> oData = new List<float>();
                if (ChartData != null)
                    oData = ChartData;

                if (DataHigh != null)
                    fMax = DataHigh.Value;

                float fHigh = 0.0f;
                bool bHasLow = false;
                float fLow = 0.0f;
                float fAverage = 0.0f;
                foreach (float fValue in oData)
                {
                    if (fValue > fMax)
                        fMax = fValue;

                    if (fValue > fHigh)
                        fHigh = fValue;

                    if ((fValue < fLow) || (!bHasLow))
                    {
                        bHasLow = true;
                        fLow = fValue;
                    }

                    fAverage += fValue;
                }

                if (oData.Count > 0)
                    fAverage = fAverage / (float)oData.Count;

                DataHigh = fMax;

                if (oData.Count > 0)
                    ctlLast.Text = FormatNumber(oData[oData.Count - 1]);
                else
                    ctlLast.Text = "0.00";
                ctlAverage.Text = FormatNumber(fAverage);
                ctlMaximum.Text = FormatNumber(fHigh);
                ctlMinimum.Text = FormatNumber(fLow);
                ctlSampleCount.Text = oData.Count.ToString();

                cellMax1.Text = FormatNumber(fMax);
                cellMax2.Text = FormatNumber(fMax);

                int iIndex = 0;
                for (int A = 0; A < MaxBars; ++A)
                {
                    TableCell oCell = new TableCell();
                    oCell.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Center;
                    oCell.VerticalAlign = System.Web.UI.WebControls.VerticalAlign.Bottom;
                    oCell.RowSpan = 2;

                    bool bHasData = false;
                    int iHeight = 1;
                    int iColour = 0;
                    if (iIndex < oData.Count)
                    {
                        bHasData = true;
                        iHeight = (int)((oData[iIndex] / fMax) * tblData.Height.Value);
                        iColour = (int)((oData[iIndex] / fMax) * 255.0f);
                    }

                    if (iHeight < 1)
                        iHeight = 1;
                    if (iColour < 1)
                        iColour = 1;

                    string sHexR = iColour.ToString("x2");
                    string sHexB = (255 - iColour).ToString("x2");

                    string sColour = sHexR + "00" + sHexB;
                    if (!bHasData)
                        sColour = "ffffff";
                    oCell.Text = string.Format("<div style='width:{0}px;'><div style=\"background-color:#" + sColour + "; height:" + iHeight.ToString() + "px; width:{1}px;\"></div></div>", BarWidth + BarSpace, BarWidth);
                    if (bHasData)
                        oCell.ToolTip = FormatNumber(oData[iIndex]);
                    if (iIndex < oData.Count)
                        tblData.Rows[0].Cells.AddAt(tblData.Rows[0].Cells.Count - 1, oCell);
                    else
                        tblData.Rows[0].Cells.AddAt(1, oCell);

                    ++iIndex;
                }
                ctlChartContent.Visible = true;
                StringBuilder sb = new StringBuilder();
                StringWriter swriter = new StringWriter(sb);
                HtmlTextWriter writer = new HtmlTextWriter(swriter);
                ctlChartContent.RenderControl(writer);
                ctlLocForm.Html = sb.ToString();
                ctlChartContent.Visible = false;

            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void AddChartPoint(float fValue)
        {
            try
            {
                List<float> oData = new List<float>();
                if (ChartData != null)
                    oData = ChartData;

                if (oData.Count >= MaxBars)
                    oData.RemoveAt(0);

                oData.Add(fValue);

                ChartData = oData;
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        public bool IsRunning
        {
            get
            {

                if (string.IsNullOrEmpty(ctlRunningHolder.Text))
                    return false;
                else return bool.Parse(ctlRunningHolder.Text);
            }
            set
            {
                ctlRunningHolder.Text = value.ToString();
            }
        }

        private void LoadData()
        {
            try
            {
                float value = business.GetMeasureValues(WatchName, CategoryName, InstanceName, MeasureNames).First();
                AddChartPoint(value);
                DrawChart();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                Stop();
            }
        }

        public void Start()
        {
            ctlStartStop.CommandName = "StopWatch";
            ctlStartStop.Text = "Stop";
            ctlStartStop.Icon = Ext.Net.Icon.StopBlue;
            Clear();
            taskManager.Interval = Duration;
            ctlAnayseName.Text = string.Format("{0}/{1}/{2}", MeasureNames[0], InstanceName, CategoryName);
            taskManager.StartAll();
            IsRunning = true;
        }

        public void Stop()
        {
            taskManager.StopAll();
            Thread.Sleep(100);
            ctlStartStop.CommandName = "StartWatch";
            ctlStartStop.Text = "Start";
            ctlStartStop.Icon = Ext.Net.Icon.PlayBlue;
            IsRunning = false;
        }


        protected void updateChart(object sender, DirectEventArgs e)
        {
            if (IsRunning)
            LoadData();
        }


        public void Clear()
        {
            lblMessage.Text = "";
            DataHigh = null;
            ChartData = null;
            DrawChart();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Ext.Net.ResourceManager.GetInstance().RegisterIcon(Ext.Net.Icon.StopBlue);
                Ext.Net.ResourceManager.GetInstance().RegisterIcon(Ext.Net.Icon.PlayBlue);
                DrawChart();
            }
        }

        private string FormatNumber(object oValue)
        {
            return string.Format("{0:###,###,###,###,###,##0.000}", oValue);
        }
    }
}