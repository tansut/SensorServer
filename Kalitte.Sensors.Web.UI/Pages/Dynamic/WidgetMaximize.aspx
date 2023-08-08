<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WidgetMaximize.aspx.cs"
    Inherits="Kalitte.Dashboard.Management.WidgetMaximize" %>

<%@ Register Assembly="Kalitte.Dashboard.Framework" Namespace="Kalitte.Dashboard.Framework"
    TagPrefix="kalitte" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body style="background-color: #ffffff">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <kalitte:ScriptManager ID="ScriptManager2" runat="server">
        </kalitte:ScriptManager>
        <kalitte:WidgetContainer ID="WidgetContainer1" runat="server">
            <PanelSettings Icon="None" Border="False" BodyBorder="False" Collapsible="False"
                HideCollapseTool="False" AutoWidth="False" AutoHeight="False" Enabled="True"
                Frame="False" Header="False" Padding="5" Shim="False" TitleCollapse="False" Unstyled="False"
                HeaderDisplayMode="Always" Dragable="False" AutoScroll="False" FitLayout="False"
                Stretch="False" Hidden="False" AutoHideTools="False"></PanelSettings>
        </kalitte:WidgetContainer>
    </div>
    </form>
</body>
</html>