<%@ Page Title="Manage Your Asp.Net Dashboards and Widgets" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master" AutoEventWireup="true" 
CodeBehind="EditDashboards.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Dynamic.EditDashboards" %>
<%@ Register src="~/Controls/Widgets/DashboardEditor/Editor.ascx" tagname="Editor" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <uc1:Editor ID="ctlEditor" runat="server" />
</asp:Content>
