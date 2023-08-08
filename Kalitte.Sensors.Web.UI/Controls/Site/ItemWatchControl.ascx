<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemWatchControl.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Controls.Site.ItemWatchControl" %>
<%@ Register Src="ServerWatchVisualizer.ascx" TagName="ServerWatchVisualizer" TagPrefix="uc1" %>
<%@ Register Src="~/Controls/Site/WatchVisualizerWindow.ascx" TagName="ServerWatchVisualizerWindow"
    TagPrefix="uc2" %>
<%@ Register Src="~/Controls/Site/ServerWatchParameters.ascx" TagName="WatchParameters"
    TagPrefix="wp" %>
<uc2:ServerWatchVisualizerWindow runat="server" ID="ctlNewWindow" />
<ext:FitLayout ID="FitLayout1" runat="server">
    <Items>
        <ext:Container ID="Container1" runat="server" Layout="FitLayout">
            <Items>
                <tt:TTPanel ID="TTPanel1" runat="server" Header="false" Border="false" AutoHeight="true">
                    <Content>
                        <wp:WatchParameters runat="server" ID="watchParams"></wp:WatchParameters>
                    </Content>
                </tt:TTPanel>
                <tt:TTPanel ID="TTPanel2" runat="server" Header="false" Border="false">
                    <Content>
                        <uc1:ServerWatchVisualizer ID="ctlWatch" runat="server" />
                    </Content>
                </tt:TTPanel>
            </Items>
        </ext:Container>
    </Items>
</ext:FitLayout>
