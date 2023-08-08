<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master" AutoEventWireup="true" CodeBehind="WatchVisualizerPage.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Server.WatchManagement.WatchVisualizerPage" %>
<%@ Register src="../../../Controls/Site/ServerWatchVisualizer.ascx" tagname="ServerWatchVisualizer" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">

    <uc1:ServerWatchVisualizer ID="ctlWatch" NewWindowBtnVisible="false" runat="server" />

</asp:Content>
