<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.SensorProviders.Editor" %>
<%@ Register Src="~/Controls/Site/LogViewer.ascx" TagName="LogViewer" TagPrefix="lv" %>
<tt:TTEnumStore runat="server" ID="dsStartup" EnumType="Kalitte.Sensors.Processing.ItemStartupType,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTEnumStore runat="server" ID="dsLogLevel" EnumType="Kalitte.Sensors.Security.LogLevel,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTEnumStore runat="server" ID="dsDiscovery" EnumType="Kalitte.Sensors.Processing.Metadata.DiscoverySensorMatchType,Kalitte.Sensors">
</tt:TTEnumStore>
<%@ Register Src="~/Controls/Site/ItemWatchControl.ascx" TagName="ItemWatchControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Site/MonitoringDataEditor.ascx" TagName="MonitoringDataEditor"
    TagPrefix="uc4" %>
<tt:TTStore runat="server" ID="dsSensors" OnRefreshData="dsSensors_RefreshData">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="State">
                </ext:RecordField>
                <ext:RecordField Name="Startup">
                </ext:RecordField>
                <ext:RecordField Name="StateText">
                </ext:RecordField>
                <ext:RecordField Name="Description">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTWindow ID="entityWindow" runat="server" Title="Edit Sensor" MinHeight="500"
  Width="650" Layout="FitLayout" AutoScroll="false">
    <DirectEvents>
        <Hide OnEvent="closeWindow">
        </Hide>
    </DirectEvents>
    <Items>
        <tt:TTTabPanel runat="server" ID="ctlTabs">
            <Items>
                <tt:TTPanel runat="server" Title="General" Layout="FitLayout" Padding="5">
                    <Items>
                        <tt:TTFormPanel runat="server" ID="ctlGenForm">
                            <Items>
                                <tt:TTTextField ID="ctlName" runat="server" FieldLabel="Name" AllowBlank="false">
                                </tt:TTTextField>
                                <tt:TTTextField ID="ctlDescription" runat="server" FieldLabel="Description">
                                </tt:TTTextField>
                                <tt:TTFieldSet runat="server" Title="Provider Assembly" ID="ctlProviderAssGroup">
                                    <Items>
                                        <tt:TTCompositeField runat="server" AnchorHorizontal="100%">
                                            <Items>
                                                <tt:TTTextField runat="server" ID="ctlType" AllowBlank="false" LabelAlign="Left"
                                                    FieldLabel="Full type including assembly name (ie. MyCompany.MyType,MyCompany)"
                                                    Flex="1">
                                                </tt:TTTextField>
                                            </Items>
                                        </tt:TTCompositeField>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTFieldSet ID="TTFieldSet2" runat="server" Title="Startup & State">
                                    <Items>
                                        <tt:TTCompositeField ID="TTCompositeField1" runat="server" AnchorHorizontal="100%">
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
                                <tt:TTFieldSet ID="TTFieldSet1" runat="server" Title="Discovery Behaviour">
                                    <Items>
                                        <tt:TTCompositeField ID="TTCompositeField3" runat="server" AnchorHorizontal="100%">
                                            <Items>
                                                <tt:TTEnumCombo runat="server" ID="ctlDiscoveryMatch" StoreID="dsDiscovery" AllowBlank="false"
                                                    FieldLabel="Sensor Match">
                                                </tt:TTEnumCombo>
                                                <tt:TTCheckBox runat="server" ID="ctlAutoCreateSensor" FieldLabel="Create Sensor">
                                                </tt:TTCheckBox>
                                            </Items>
                                        </tt:TTCompositeField>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTFieldSet ID="TTFieldSet3" runat="server" Title="Monitoring">
                                    <Content>
                                        <uc4:MonitoringDataEditor runat="server" ID="ctlMonitoringDataEditor" />
                                    </Content>
                                </tt:TTFieldSet>
                            </Items>
                        </tt:TTFormPanel>
                    </Items>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlGenProps" runat="server" Title="Item Properties" Layout="FitLayout"
                    Border="false" Padding="5">
                    <Items>
                        <tt:TTFormPanel runat="server" ID="ctlMiscForm">
                            <Items>
                                <tt:TTFieldSet ID="TTFieldSet4" runat="server" Title="Logging" AnchorHorizontal="100%">
                                    <Items>
                                        <tt:TTCompositeField ID="TTCompositeField2" runat="server">
                                            <Items>
                                                <tt:TTEnumCombo runat="server" ID="ctlLogLevel" StoreID="dsLogLevel" AllowBlank="false"
                                                    FieldLabel="Log Level">
                                                </tt:TTEnumCombo>
                                                <tt:TTCheckBox runat="server" ID="ctlInheritLogLevel" FieldLabel="Inherit Log Level">
                                                </tt:TTCheckBox>
                                            </Items>
                                        </tt:TTCompositeField>
                                    </Items>
                                </tt:TTFieldSet>
                            </Items>
                        </tt:TTFormPanel>
                    </Items>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlSensorBindingsPanel" runat="server" Title="Current Sensors" Layout="FitLayout"
                    Icon="DeviceStylus" Padding="5">
                    <Items>
                        <tt:TTFormPanel ID="ctlLocForm" runat="server">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                            </LayoutConfig>
                            <Items>
                                <tt:TTGrid ID="ctlCurrentBindings" Flex="1" AnchorVertical="100%" AutoGenerateStateCommands="false"
                                    Title="Current sensor instances of provider" StoreID="dsSensors" runat="server"
                                    AutoGenerateDeleteCommand="false">
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <tt:TTColumn DataIndex="State" Header="State" Width="60">
                                                <Renderer Fn="renderStateColor" />
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Name" Header="Name" Width="75">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="StateText" Header="Information" Width="120">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Startup" Header="Initial Startup" Width="60">
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
                                        <tt:TTPagingToolbar ID="userPaging" runat="server" StoreID="dsSensors">
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
                        </tt:TTFormPanel>
                    </Items>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlProfilePanel" Icon="ApplicationViewList" runat="server" Title="Property Profile"
                    Layout="FitLayout" Border="false" Padding="5">
                    <Content>
                        <tt:PropertyProfileEditor runat="server" ID="ctlProfileEditor" >
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
                        <uc3:ItemWatchControl runat="server" ID="ctlAnalyseItem" AnalyseItem="SensorProvider" />
                    </Content>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlLogViewPanel" runat="server" Title="Log" Layout="FitLayout" Border="false"
                    Padding="5">
                    <Content>
                        <lv:LogViewer runat="server" ID="ctlLogView"></lv:LogViewer>
                    </Content>
                </tt:TTPanel>
            </Items>
        </tt:TTTabPanel>
    </Items>
    <Buttons>
        <tt:TTCmdButon runat="server" ID="ctlSave" Text="Save Changes" AssociatedForm="ctlGenForm,ctlMiscForm"
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
