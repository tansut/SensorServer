<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Server.WatchManagement._default" %>

<%@ Register Src="~/Controls/Site/ServerWatchVisualizer.ascx" TagName="ServerWatchVisualizer"
    TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Site/WatchVisualizerWindow.ascx" TagName="ServerWatchVisualizerWindow"
    TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Site/ItemWatchControl.ascx" TagName="ItemWatchControl"
    TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <tt:TTPanel ID="TTPanel1" runat="server" Layout="FitLayout" Header="false" Border="false">
        <Content>
            <uc3:ItemWatchControl runat="server" ID="ctlAnalyseItem" />
        </Content>
    </tt:TTPanel>
</asp:Content>
