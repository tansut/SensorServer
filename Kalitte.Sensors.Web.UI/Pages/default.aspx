<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function popUp(URL) {
            day = new Date();
            id = day.getTime();
            eval("page" + id + " = window.open(URL, 'ttvysPopUp', 'toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=400,height=300,left = 640,top = 325');");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <kalitte:DashboardSurface ID="surface" runat="server">
    </kalitte:DashboardSurface>
</asp:Content>
