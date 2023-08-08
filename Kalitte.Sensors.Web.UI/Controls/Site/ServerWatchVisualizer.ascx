<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerWatchVisualizer.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Controls.Site.ServerWatchVisualizer" %>
<tt:TTPanel ID="ctlLocForm" Padding="0" runat="server"  >
    <TopBar>
        <tt:TTToolbar ID="TTToolbar1" runat="server">
            <Items>
                <tt:TTCmdButon runat="server" CommandName="StartWatch" ID="ctlStartStop" Text="Start"
                    Icon="PlayBlue" Width="65">
                    <DirectEvents>
                        <Click>
                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{ctlLocForm}" />
                        </Click>
                    </DirectEvents>
                </tt:TTCmdButon>
                <tt:TTCmdButon runat="server" MaskMessage="Processing..." CommandName="ClearWatch"
                    ID="ctlClear" Text="Clear" Icon="Delete" Width="65">
                    <DirectEvents>
                        <Click>
                            <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{ctlLocForm}" />
                        </Click>
                    </DirectEvents>
                </tt:TTCmdButon>
                <tt:TTCmdButon runat="server" CommandName="OpenWatchInNewWindow" ID="ctlNewWindow"
                    Text="New Window" Icon="Application" Width="75">
                </tt:TTCmdButon>
                <ext:ToolbarFill runat="server">
                </ext:ToolbarFill>
                <ext:ToolbarTextItem runat="server" ID="ctlAnayseName">
                </ext:ToolbarTextItem>
            </Items>
        </tt:TTToolbar>
    </TopBar>
    <BottomBar>
        <ext:StatusBar runat="server" ID="ctlStatusbar">
            <Items>
                <ext:ToolbarTextItem ID="ToolbarTextItem1" runat="server" Text="<b>Last:</b>" />
                <ext:ToolbarTextItem ID="ctlLast" runat="server" Width="65" Text="" />
                <ext:ToolbarTextItem ID="ToolbarTextItem2" runat="server" Text="<b>Average:</b>" />
                <ext:ToolbarTextItem ID="ctlAverage" runat="server" Width="65" Text="" />
                <ext:ToolbarTextItem ID="ToolbarTextItem3" runat="server" Text="<b>Minimum:</b>" />
                <ext:ToolbarTextItem ID="ctlMinimum" runat="server" Width="65" Text="" />
                <ext:ToolbarTextItem ID="ToolbarTextItem4" runat="server" Text="<b>Maximum:</b>" />
                <ext:ToolbarTextItem ID="ctlMaximum" runat="server" Width="65" Text="" />
                <ext:ToolbarTextItem ID="ToolbarTextItem5" runat="server" Text="<b>Points:</b>" />
                <ext:ToolbarTextItem ID="ctlSampleCount" runat="server" Width="65" Text="" />
            </Items>
        </ext:StatusBar>
    </BottomBar>
    <Content>
        <div align="center" runat="server" id="ctlChartContent" visible="false">
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red" />
            <asp:Table ID="tblData" runat="server" CellPadding="0" CellSpacing="0" CssClass="watchChartTable"
                Height="250"  EnableViewState="false">
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" VerticalAlign="Top" CssClass="cellMinMax"
                        ID="cellMax1" runat="server">0</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" CssClass="cellMinMax right" VerticalAlign="Top"
                        ID="cellMax2" runat="server">0</asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell HorizontalAlign="Center" CssClass="cellMinMax" VerticalAlign="Bottom">0.00</asp:TableCell>
                    <asp:TableCell HorizontalAlign="Center" CssClass="cellMinMax right" VerticalAlign="Bottom">0.00</asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </Content>
</tt:TTPanel>
<ext:TaskManager runat="server" ID="taskManager" Interval="2000">
    <Tasks>
        <ext:Task AutoRun="false" TaskID="updateChart">
            <DirectEvents>
                <Update OnEvent="updateChart">
                </Update>
            </DirectEvents>
        </ext:Task>
    </Tasks>
</ext:TaskManager>
<ext:Hidden runat="server" ID="ctlRunningHolder">
</ext:Hidden>
<ext:Hidden runat="server" ID="ctlMaxValHoder">
</ext:Hidden>
<ext:Hidden runat="server" ID="ctlDataList">
</ext:Hidden>
