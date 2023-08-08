﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="List.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.SensorProviders.List" %>
<tt:TTStore ID="dsMain" runat="server">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Description">
                </ext:RecordField>
                <ext:RecordField Name="State">
                </ext:RecordField>
                <ext:RecordField Name="Startup">
                </ext:RecordField>
                <ext:RecordField Name="StateText">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<ext:FitLayout ID="FitLayout1" runat="server">
    <Items>
        <tt:TTGrid ID="grid" runat="server" StoreID="dsMain" IsEditable="true">
            <TopBar>
                <ext:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <tt:TTButtonGroup ID="ButtonGroup1" runat="server" Title="Edit">
                            <Items>
                                <tt:TTCmdButon ID="ctlAddBtn" runat="server" KnownCommand="CreateInEditor" Text="Create New Provider" />
                                <tt:TTCmdButon ID="ctlEditBtn" runat="server" ShowMask="true" KnownCommand="EditInEditor" />
                                <tt:TTCmdButon ID="ctlCreateBtn" runat="server" KnownCommand="DeleteEntity" />
                                <tt:TTCmdButon ID="TTCmdButon1" runat="server" KnownCommand="EditInEditor" Icon="DeviceStylus"
                                    CommandArgument="2" Text="View Sensors" ShowMask="true" />
                            </Items>
                        </tt:TTButtonGroup>
                        <tt:TTButtonGroup ID="TTButtonGroup1" runat="server" Title="Management">
                            <Items>
                                <tt:TTCmdButon ID="TTCmdButon3" runat="server" KnownCommand="EditInEditor" ShowMask="true"
                                    Text="Analyse Performance" Icon="ChartBar" CommandArgument="5">
                                </tt:TTCmdButon>
                                <tt:TTCmdButon ID="TTCmdButon2" runat="server" KnownCommand="EditInEditor" CommandArgument="6"
                                    Icon="DatabaseRefresh" Text="View Log" ShowMask="true" />
                            </Items>
                        </tt:TTButtonGroup>
                    </Items>
                </ext:Toolbar>
            </TopBar>
            <ColumnModel ID="ColumnModel1" runat="server">
                <Columns>
                    <tt:TTColumn DataIndex="State" Header="State" Width="120" Fixed="true">
                        <Renderer Fn="renderStateColor" />
                    </tt:TTColumn>
                    <tt:TTColumn DataIndex="Name" Header="Name" Width="75">
                    </tt:TTColumn>
                    <tt:TTColumn DataIndex="StateText" Header="Information" Width="120">
                    </tt:TTColumn>
                    <tt:TTColumn DataIndex="Startup" Header="Initial Startup" Width="120" Fixed="true">
                    </tt:TTColumn>
                    <tt:TTColumn DataIndex="Description" Width="150" Header="Description">
                    </tt:TTColumn>
                </Columns>
            </ColumnModel>
            <SelectionModel>
                <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                </ext:RowSelectionModel>
            </SelectionModel>
            <BottomBar>
                <tt:TTPagingToolbar ID="userPaging" runat="server" StoreID="dsMain">
                </tt:TTPagingToolbar>
            </BottomBar>
            <Plugins>
                <ext:GridFilters runat="server" ID="GridFilters1" Local="false">
                    <Filters>
                        <ext:StringFilter DataIndex="Name" />
                    </Filters>
                </ext:GridFilters>
            </Plugins>
            <View>
                <ext:GridView ForceFit="true" />
            </View>
        </tt:TTGrid>
    </Items>
</ext:FitLayout>
