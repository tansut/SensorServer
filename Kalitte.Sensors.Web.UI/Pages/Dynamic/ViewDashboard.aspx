<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master" AutoEventWireup="true" 
CodeBehind="ViewDashboard.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Dynamic.ViewDashboard" %>
<%@ Register assembly="Kalitte.Dashboard.Framework" namespace="Kalitte.Dashboard.Framework" tagprefix="kalitte" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">

    <kalitte:DashboardSurface ID="surface" runat="server" 
         >
    <PanelSettings Icon="None" Border="False" BodyBorder="False" Collapsible="False" HideCollapseTool="False" AutoWidth="False" AutoHeight="False" Enabled="True" Frame="False" Header="False" Padding="0" Shim="False" TitleCollapse="False" Unstyled="False" HeaderDisplayMode="Always" Dragable="False" AutoScroll="False" FitLayout="False" Stretch="False" Hidden="False">
    </PanelSettings>
    <WindowSettings AutoShow="False" Modal="False" Resizable="True" Maximizable="True" Minimizable="False" Closable="True" Icon="None" Width="500" Height="500" Border="True" BodyBorder="True" Collapsible="False" HideCollapseTool="False" AutoWidth="False" AutoHeight="False" Enabled="True" Frame="False" Header="True" Padding="0" Shim="False" TitleCollapse="False" Unstyled="False" HeaderDisplayMode="Always" Dragable="True" AutoScroll="True" FitLayout="False" Stretch="False" Hidden="False">
    </WindowSettings>
</kalitte:DashboardSurface>

</asp:Content>
