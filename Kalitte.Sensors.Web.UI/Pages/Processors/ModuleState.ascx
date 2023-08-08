<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleState.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Pages.Processors.ModuleState" %>
<tt:TTStore ID="dsBindings" runat="server" OnRefreshData="dsBindings_RefreshData">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Module">
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
<tt:TTWindow ID="entityWindow" runat="server" Title="Module States" MinHeight="500"
    Width="500" Layout="FitLayout" AutoScroll="false">
    <Items>
        <tt:TTTabPanel runat="server" ID="ctlTabs">
            <Items>
                <tt:TTPanel ID="ctlModuleBindingsPanel" runat="server" Title="Processor Modules"
                    Layout="FitLayout" Padding="5">
                    <Items>
                        <tt:TTFormPanel ID="ctlLocForm" runat="server">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                            </LayoutConfig>
                            <Items>
                                <tt:TTGrid ID="ctlCurrentBindings" Flex="1" AnchorVertical="100%" StoreID="dsBindings"
                                    runat="server" AutoGenerateStateCommands="false" IsEditable="true" StartItemCommandName="StartModule"
                                    StopItemCommandName="StopModule">
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <tt:TTColumn DataIndex="State" Header="State" Width="40">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Module" Header="Module" Width="75">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="StateText" Header="Information" Width="50">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Startup" Header="Initial Startup" Width="50">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Description" Width="50" Header="Description">
                                            </tt:TTColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <BottomBar>
                                        <tt:TTPagingToolbar ID="userPaging" runat="server" StoreID="dsBindings">
                                        </tt:TTPagingToolbar>
                                    </BottomBar>
                                    <Plugins>
                                        <ext:GridFilters runat="server" ID="GridFilters1" Local="false">
                                            <Filters>
                                                <ext:StringFilter DataIndex="Module" />
                                            </Filters>
                                        </ext:GridFilters>
                                    </Plugins>
                                    <View>
                                        <ext:GridView ForceFit="true" />
                                    </View>
                                </tt:TTGrid>
                            </Items>
                        </tt:TTFormPanel>
                    </Items>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlProfilePanel" Icon="ApplicationViewList" runat="server" Title="Property Profile"
                    Layout="FitLayout" Border="false" Padding="5">
                    <Content>
                        <tt:PropertyProfileEditor runat="server" ID="profileEditorCtrl">
                        </tt:PropertyProfileEditor>
                    </Content>
                </tt:TTPanel>
            </Items>
        </tt:TTTabPanel>
    </Items>
    <Buttons>
        <tt:TTButton ID="TTButton1" runat="server" Text="Close">
            <Listeners>
                <Click Handler="#{entityWindow}.hide();" />
            </Listeners>
        </tt:TTButton>
    </Buttons>
</tt:TTWindow>
