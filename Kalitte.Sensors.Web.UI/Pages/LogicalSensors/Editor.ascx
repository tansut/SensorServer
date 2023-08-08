<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.LogicalSensors.Editor" %>
<%@ Register Src="~/Controls/Site/ItemWatchControl.ascx" TagName="ItemWatchControl"
    TagPrefix="uc3" %>
    <%@ Register Src="~/Controls/Site/LastEventsViewer.ascx" TagName="LastEventViewer"
    TagPrefix="lev" %>
<tt:TTStore runat="server" ID="dsLogicalBindings" OnRefreshData="dsLogicalBindings_RefreshData">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="LogicalSensorName">
                </ext:RecordField>
                <ext:RecordField Name="SensorName">
                </ext:RecordField>
                <ext:RecordField Name="SensorSource">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTEnumStore runat="server" ID="dsStartup" EnumType="Kalitte.Sensors.Processing.ItemStartupType,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTWindow ID="entityWindow" runat="server" Title="Edit Sensor" MinHeight="500"
    Width="650" Layout="FitLayout" AutoScroll="false">
        <DirectEvents>
    <Hide OnEvent="closeWindow"></Hide>
    </DirectEvents>
    <Items>
        <tt:TTTabPanel runat="server" ID="ctlTabs">
            <Items>
                <tt:TTPanel ID="TTPanel1" runat="server" Title="General" Layout="FitLayout" Padding="5">
                    <Items>
                        <tt:TTFormPanel runat="server" ID="ctlGenForm">
                            <Items>
                                <tt:TTTextField ID="ctlName" runat="server" FieldLabel="Logical Sensor Name" AllowBlank="false">
                                </tt:TTTextField>
                                <tt:TTTextField ID="ctlDescription" runat="server" FieldLabel="Description">
                                </tt:TTTextField>
                                <tt:TTFieldSet ID="TTFieldSet2" runat="server" Title="Startup & State">
                                    <Items>
                                        <tt:TTCompositeField ID="TTCompositeField2" runat="server" AnchorHorizontal="100%">
                                            <Items>
                                                <tt:TTEnumCombo runat="server" ID="ctlInitialStartup" StoreID="dsStartup" AllowBlank="false"
                                                    FieldLabel="Initial Startup">
                                                </tt:TTEnumCombo>
                                                <tt:TTExceptionViewer runat="server" ID="ctlLastException" FieldLabel="Last Exception"
                                                    Flex="1">
                                                </tt:TTExceptionViewer>
                                            </Items>
                                        </tt:TTCompositeField>
                                    </Items>
                                </tt:TTFieldSet>
                            </Items>
                        </tt:TTFormPanel>
                    </Items>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlSensorBindingsPanel" runat="server" Title="Sensor Bindings" Layout="FitLayout"
                    Padding="5">
                    <Items>
                        <tt:TTFormPanel ID="ctlLocForm" runat="server">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                            </LayoutConfig>
                            <Items>
                                <tt:TTGrid ID="ctlCurrentSensorBindings" Flex="1" AnchorVertical="100%" AutoGenerateStateCommands="false"
                                    Title="Current Bindings" StoreID="dsLogicalBindings" runat="server" IsEditable="true">
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <tt:TTColumn DataIndex="SensorName" Header="Logical Sensor" Width="120">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="SensorSource" Header="Sensor Source (Empty for All Sources)">
                                            </tt:TTColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <BottomBar>
                                        <tt:TTPagingToolbar ID="userPaging" runat="server" StoreID="dsLogicalBindings">
                                        </tt:TTPagingToolbar>
                                    </BottomBar>
                                    <Plugins>
                                        <ext:GridFilters runat="server" ID="GridFilters1" Local="false">
                                            <Filters>
                                                <ext:StringFilter DataIndex="SensorName" />
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
                        <tt:PropertyProfileEditor runat="server" ID="ctlProfileEditor">
                        </tt:PropertyProfileEditor>
                    </Content>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlExtendedProfilePanel" Icon="ApplicationDouble" runat="server"
                    Title="Extended Profile" Layout="FitLayout" Border="false" Padding="5">
                    <Content>
                        <tt:PropertyProfileEditor runat="server" ID="ctlExtendedProfileEditor">
                        </tt:PropertyProfileEditor>
                    </Content>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlAnalysisPanel" Icon="ChartBar" runat="server" Title="Item Analysis"
                    Layout="FitLayout" Border="false" Padding="5" BodyBorder="false">
                    <Content>
                        <uc3:ItemWatchControl runat="server" ID="ctlAnalyseItem" AnalyseItem="LogicalSensor" />
                    </Content>
                </tt:TTPanel>
                	                <tt:TTPanel ID="ctlLastEventsPanel" runat="server" Title="Last Events" Layout="FitLayout"
                    Border="false" BodyBorder="false"  Padding="0">
                    <Content>
                        <lev:LastEventViewer runat="server" ID="ctlLastEvents" ItemType="LogicalSensor">
                        </lev:LastEventViewer>
                    </Content>
                </tt:TTPanel>
            </Items>
        </tt:TTTabPanel>
    </Items>
    <Buttons>
        <tt:TTCmdButon runat="server" ID="ctlSave" Text="Save Changes" AssociatedForm="ctlGenForm"
            AutoValidateForm="true">
            <DirectEvents>
                <Click>
                    <EventMask Msg="Processing ..." Target="CustomTarget" CustomTarget="#{entityWindow}"
                        ShowMask="True"></EventMask>
                </Click>
            </DirectEvents>
        </tt:TTCmdButon>
        <tt:TTButton ID="TTButton1" runat="server" Text="Close">
            <Listeners>
                <Click Handler="#{entityWindow}.hide();" />
            </Listeners>
        </tt:TTButton>
    </Buttons>
</tt:TTWindow>
