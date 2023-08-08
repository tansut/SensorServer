<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Controls.Widgets.ServerAnalyse.Editor" %>
<%@ Register Src="~/Controls/Site/ServerWatchParameters.ascx" TagName="WatchParameters"
    TagPrefix="wp" %>
<ext:Container ID="Container1" runat="server">
    <Items>
        <tt:TTPanel ID="TTPanel2" runat="server" Header="false" Layout="FitLayout" Border="false" Height="120">
            <Content>
                <wp:WatchParameters runat="server" ID="watchParams"></wp:WatchParameters>
            </Content>
        </tt:TTPanel>
        <tt:TTPanel ID="TTPanel1" runat="server" Title="General Settings" Layout="FormLayout">
            <Items>
                <tt:TTCheckBox runat="server" ID="ctlAutoStart" Checked="false" FieldLabel="Auto Start">
                </tt:TTCheckBox>
            </Items>
        </tt:TTPanel>
    </Items>
</ext:Container>
