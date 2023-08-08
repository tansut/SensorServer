<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Sensors.Editor" %>
<%@ Register Src="~/Controls/Site/ItemWatchControl.ascx" TagName="ItemWatchControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Site/LastEventsViewer.ascx" TagName="LastEventViewer"
    TagPrefix="lev" %>
<%@ Register Src="~/Controls/Site/MonitoringDataEditor.ascx" TagName="MonitoringDataEditor"
    TagPrefix="uc4" %>
<tt:TTStore runat="server" ID="dsProviders">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Description">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTEnumStore runat="server" ID="dsStartup" EnumType="Kalitte.Sensors.Processing.ItemStartupType,Kalitte.Sensors">
</tt:TTEnumStore>
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
<tt:TTStore runat="server" ID="dsSensorSources">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Value">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore runat="server" ID="dsSensorPropertySources">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Value">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore runat="server" ID="dsLogicalSensors">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTWindow ID="entityWindow" runat="server" Title="Edit Sensor" MinHeight="500"
    Icon="DeviceStylus" Width="650" Layout="FitLayout" AutoScroll="false">
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
                                <tt:TTTextField ID="ctlSensorId" runat="server" FieldLabel="Id" HideWithLabel="true">
                                </tt:TTTextField>
                                <tt:TTComboBox runat="server" ID="ctlProviders" ValueField="Name" StoreID="dsProviders"
                                    FieldLabel="Provider" DisplayField="Name" AllowBlank="false" Editable="false">
                                </tt:TTComboBox>
                                <tt:TTTextField ID="ctlDescription" runat="server" FieldLabel="Description">
                                </tt:TTTextField>
                                <tt:TTFieldSet runat="server" Title="Network & Authentication" ID="ctlNetworkFieldSet">
                                    <Items>
                                        <tt:TTCompositeField runat="server" AnchorHorizontal="100%">
                                            <Items>
                                                <tt:TTTextField runat="server" ID="ctlHost" FieldLabel="IP or Host" AllowBlank="false"
                                                    Flex="1">
                                                </tt:TTTextField>
                                                <tt:TTNumberField ID="ctlPort" runat="server" FieldLabel="Port" MinValue="1" MaxValue="65534"
                                                    AllowBlank="false" Flex="1">
                                                </tt:TTNumberField>
                                                <tt:TTTextField runat="server" ID="ctlUsername" FieldLabel="Username" Flex="1">
                                                </tt:TTTextField>
                                                <tt:TTTextField runat="server" ID="ctlPassword" FieldLabel="Password" InputType="Password"
                                                    Flex="1">
                                                </tt:TTTextField>
                                            </Items>
                                        </tt:TTCompositeField>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTFieldSet ID="TTFieldSet2" runat="server" Title="Startup & State">
                                    <Items>
                                        <tt:TTCompositeField ID="TTCompositeField1" runat="server" AnchorHorizontal="100%"
                                            LabelSource="UseNote">
                                            <Items>
                                                <tt:TTEnumCombo runat="server" ID="ctlInitialStartup" StoreID="dsStartup" AllowBlank="false"
                                                    FieldLabel="Initial Startup">
                                                </tt:TTEnumCombo>
                                                <tt:TTExceptionViewer runat="server" ID="ctlLastException" Flex="1">
                                                </tt:TTExceptionViewer>
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
                <tt:TTPanel ID="ctlSensorBindingsPanel" runat="server" Title="Logical Sensors" Layout="FitLayout"
                    Padding="5">
                    <Items>
                        <tt:TTFormPanel ID="ctlLocForm" runat="server">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                            </LayoutConfig>
                            <Items>
                                <tt:TTFieldSet ID="TTFieldSet1" runat="server" Title="Create New Binding" AutoHeight="true"
                                    Margins="0 0 5 0">
                                    <Items>
                                        <tt:TTContainer ID="Container1" runat="server" Layout="Column">
                                            <Items>
                                                <tt:TTFormPanel ID="ctlLogicalSensorsForm" runat="server" ColumnWidth=".5">
                                                    <Items>
                                                        <tt:TTComboBox runat="server" ID="ctlLogicalSensors" FieldLabel="Logical Sensor"
                                                            StoreID="dsLogicalSensors" Editable="false" DisplayField="Name" ValueField="Name"
                                                            AllowBlank="false" AnchorHorizontal="98%">
                                                        </tt:TTComboBox>
                                                    </Items>
                                                </tt:TTFormPanel>
                                                <tt:TTFormPanel ID="ctlSensorSourceForm" runat="server" ColumnWidth=".5">
                                                    <Items>
                                                        <tt:TTComboBox runat="server" ID="ctlSourceList" FieldLabel="Sensor Source" AnchorHorizontal="98%"
                                                            ValueField="Value" DisplayField="Name" Editable="false" StoreID="dsSensorSources">
                                                        </tt:TTComboBox>
                                                    </Items>
                                                </tt:TTFormPanel>
                                                <tt:TTFormPanel ID="ctlCreateBindingForm" runat="server">
                                                    <Items>
                                                        <tt:TTCmdButon runat="server" CommandName="AddLogicalSensorBinding" ID="ctlAddBinding"
                                                            Text="Create Binding" Width="100" AssociatedForm="ctlLogicalSensorsForm">
                                                        </tt:TTCmdButon>
                                                    </Items>
                                                </tt:TTFormPanel>
                                            </Items>
                                        </tt:TTContainer>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTGrid ID="ctlCurrentBindings" Flex="1" AnchorVertical="100%" AutoGenerateStateCommands="false"
                                    Title="Current Bindings" StoreID="dsLogicalBindings" runat="server" IsEditable="true">
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <tt:TTCommandColumn Width="5">
                                                <Commands>
                                                    <tt:TTGridCommand CommandName="RemoveLogicalBinding" Confirm="true" Icon="Delete">
                                                    </tt:TTGridCommand>
                                                </Commands>
                                            </tt:TTCommandColumn>
                                            <tt:TTColumn DataIndex="LogicalSensorName" Header="Logical Sensor" Width="120">
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
                    Border="false" Padding="5">
                    <Items>
                        <ext:RowLayout runat="server">
                            <Rows>
                                <ext:LayoutRow RowHeight="0.1">
                                    <tt:TTPanel ID="ctlComboPanel" Border="false" Layout="FormLayout" BodyBorder="false" runat="server" AutoHeight="true">
                                        <Items>
                                            <tt:TTComboBox Editable="false" runat="server" ID="ctlSource" AnchorHorizontal="100%" StoreID="dsSensorPropertySources"
                                                ValueField="Value" DisplayField="Name" HideLabel="true">
                                                <DirectEvents>
                                                    <Select OnEvent="ctlSource_Change">
                                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{ctlProfilePanel}" />
                                                    </Select>
                                                </DirectEvents>
                                            </tt:TTComboBox>
                                        </Items>
                                    </tt:TTPanel>
                                </ext:LayoutRow>
                                <ext:LayoutRow RowHeight="0.9">
                                    <tt:TTPanel ID="TTPanel1" Border="false" BodyBorder="false" runat="server" Layout="FitLayout">
                                        <Content>
                                            <tt:PropertyProfileEditor runat="server" ID="ctlProfileEditor" OnSetPropertyProfile="ctlProfileEditor_OnSetPropertyProfile">
                                            </tt:PropertyProfileEditor>
                                        </Content>
                                    </tt:TTPanel>
                                </ext:LayoutRow>
                            </Rows>
                        </ext:RowLayout>
                    </Items>
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
                        <uc3:ItemWatchControl runat="server" ID="ctlAnalyseItem" AnalyseItem="SensorDevice" />
                    </Content>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlLastEventsPanel" runat="server" Title="Last Events" Layout="FitLayout"
                    Border="false" BodyBorder="false" Padding="0">
                    <Content>
                        <lev:LastEventViewer runat="server" ID="ctlLastEvents" ItemType="SensorDevice"></lev:LastEventViewer>
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
