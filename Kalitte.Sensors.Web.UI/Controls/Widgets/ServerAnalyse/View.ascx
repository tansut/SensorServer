<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Controls.Widgets.ServerAnalyse.View" %>
<%@ Register Src="~/Controls/Site/ServerWatchVisualizer.ascx" TagName="ServerWatchVisualizer"
    TagPrefix="uc1" %>


    <uc1:ServerWatchVisualizer ID="ctlWatch" runat="server" NewWindowBtnVisible="false" MaxBars="100" BarWidth="2" Duration="3000" />