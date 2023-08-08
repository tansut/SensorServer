<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Processors.Editor" %>
<%@ Register Src="ModuleEditor.ascx" TagName="ModuleEditor" TagPrefix="me" %>
<%@ Register Src="LogicalSensorEditor.ascx" TagName="LogicalSensorEditor" TagPrefix="le" %>
<%@ Register Src="~/Controls/Site/LogViewer.ascx" TagName="LogViewer" TagPrefix="lv" %>
<%@ Register Src="~/Controls/Site/ItemWatchControl.ascx" TagName="ItemWatchControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Site/LastEventsViewer.ascx" TagName="LastEventViewer"
    TagPrefix="lev" %>
    <%@ Register Src="~/Controls/Site/MonitoringDataEditor.ascx" TagName="MonitoringDataEditor"
    TagPrefix="uc4" %>
<script type="text/javascript">
    function confirmIfRunning(config) {
        return config.confirm;
    }
</script>
<tt:TTEnumStore runat="server" ID="dsStartup" EnumType="Kalitte.Sensors.Processing.ItemStartupType,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTEnumStore runat="server" ID="dsLogLevel" EnumType="Kalitte.Sensors.Security.LogLevel,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTStore runat="server" ID="dsModules">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="NameAndType">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore ID="dsProcessorModules" runat="server" OnRefreshData="dsModuleBindings_RefreshData">
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
                <ext:RecordField Name="Description">
                </ext:RecordField>
                <ext:RecordField Name="State">
                </ext:RecordField>
                <ext:RecordField Name="Startup">
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
<tt:TTEnumStore runat="server" ID="dsNonExistEventHandler" EnumType="Kalitte.Sensors.Processing.NonExistEventHandlerBehavior,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTEnumStore runat="server" ID="dsNullEventBehavior" EnumType="Kalitte.Sensors.Processing.PipeNullEventBehavior,Kalitte.Sensors">
</tt:TTEnumStore>

<me:ModuleEditor runat="server" ID="ctlModuleEditor"></me:ModuleEditor>
<le:LogicalSensorEditor runat="server" ID="ctlLogicalSensorEditor"></le:LogicalSensorEditor>
<tt:TTWindow ID="entityWindow" runat="server" Title="Edit Processor" MinHeight="500"
    Width="700" Layout="FitLayout" AutoScroll="false" >
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
                                 <tt:TTFieldSet ID="TTFieldSet5" runat="server" Title="Monitoring">
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
                                <tt:TTFieldSet ID="ctlEventPipeGroup" runat="server" Title="Event Pipe Settings"
                                    AnchorHorizontal="100%">
                                    <Items>
                                        <tt:TTEnumCombo runat="server" Width="500" LabelWidth="120" ID="ctlNonExistEventHandler"
                                            StoreID="dsNonExistEventHandler" AllowBlank="false" FieldLabel="Non Exist Event Handler Behaviour"
                                            Note="What to do when processing engine was unable to locate an eventhandler for a sensor event">
                                        </tt:TTEnumCombo>
                                    </Items>
                                    <Items>
                                        <tt:TTEnumCombo runat="server" Width="500" LabelWidth="120" ID="ctlNullEventBehavior" StoreID="dsNullEventBehavior"
                                            AllowBlank="false" FieldLabel="Null Sensor Event Behavior" Note="What to do when an event handler returns null sensor event">
                                        </tt:TTEnumCombo>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTFieldSet ID="TTFieldSet4" runat="server" Title="Logging" AnchorHorizontal="100%">
                                    <Items>
                                        <tt:TTCompositeField runat="server">
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
                <tt:TTPanel ID="ctlModuleBindingsPanel" runat="server" Title="Processor Modules"
                    Layout="FitLayout" Padding="5" Icon="Package">
                    <Items>
                        <tt:TTFormPanel ID="ctlLocForm" runat="server">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                            </LayoutConfig>
                            <Items>
                                <tt:TTFieldSet ID="TTFieldSet1" runat="server" Title="Add Module Instance" AutoHeight="true"
                                    Margins="0 0 5 0">
                                    <Items>
                                        <tt:TTContainer ID="Container1" runat="server" Layout="Column">
                                            <Items>
                                                <tt:TTFormPanel ID="ctlModulesForm" runat="server" ColumnWidth=".8">
                                                    <Items>
                                                        <tt:TTComboBox runat="server" ID="ctlModules" FieldLabel="Module" StoreID="dsModules"
                                                            Editable="false" DisplayField="NameAndType" ValueField="Name" AllowBlank="false"
                                                            AnchorHorizontal="98%">
                                                            <Template ID="Template1" runat="server">
                                                                <Html>
                                                                    <tpl for=".">
                        <div class="x-combo-list-item icon-combo-item {iconCls}">
                            {NameAndType}
                        </div>
                    </tpl>
                                                                </Html>
                                                            </Template>
                                                            <Listeners>
                                                                <Select Handler="this.setIconCls('icon-package');" />
                                                            </Listeners>
                                                        </tt:TTComboBox>
                                                    </Items>
                                                </tt:TTFormPanel>
                                                <tt:TTFormPanel ID="ctlCreateBindingForm" runat="server">
                                                    <Items>
                                                        <tt:TTCmdButon runat="server" CommandName="CreateModuleProcessorInEditor" ID="ctlAddModule"
                                                            Text="Add Module" Width="100" AssociatedForm="ctlModulesForm">
                                                        </tt:TTCmdButon>
                                                    </Items>
                                                </tt:TTFormPanel>
                                            </Items>
                                        </tt:TTContainer>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTGrid ID="ctlCurrentModuleBindings" Flex="1" AnchorVertical="100%" Title="Current module instances of processor"
                                    StoreID="dsProcessorModules" runat="server" AutoGenerateStateCommands="false"
                                    IsEditable="true">
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <tt:TTCommandColumn Width="28" Header="Edit">
                                                <Commands>
                                                    <tt:TTGridCommand CommandName="RemoveModuleBinding" Confirm="true" Icon="Delete"
                                                        ToolTip-Text="Remove Binding">
                                                    </tt:TTGridCommand>
                                                    <tt:TTGridCommand CommandName="EditModuleBinding" Icon="NoteEdit" ToolTip-Text="Edit">
                                                    </tt:TTGridCommand>
                                                </Commands>
                                            </tt:TTCommandColumn>
                                            <tt:TTColumn DataIndex="State" Header="Module" Width="75">
                                                <Renderer Fn="renderStateColor" />
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Module" Header="Module" Width="75">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="StateText" Header="Information" Width="50">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Startup" Header="Initial Startup" Width="50">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Description" Width="50" Header="Description">
                                            </tt:TTColumn>
                                            <tt:TTCommandColumn Width="24" Header="Order">
                                                <Commands>
                                                    <tt:TTGridCommand CommandName="DecrementModuleBindingPosition" Icon="ArrowUp">
                                                    </tt:TTGridCommand>
                                                    <tt:TTGridCommand CommandName="IncrementModuleBindingPosition" Icon="ArrowDown">
                                                    </tt:TTGridCommand>
                                                </Commands>
                                            </tt:TTCommandColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <BottomBar>
                                        <tt:TTPagingToolbar ID="userPaging" runat="server" StoreID="dsProcessorModules">
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
                <tt:TTPanel ID="ctlSensorBindingsPanel" runat="server" Title="Logical Sensors" Layout="FitLayout"
                    Padding="5">
                    <Items>
                        <tt:TTFormPanel ID="ctlLocBindingsForm" runat="server">
                            <LayoutConfig>
                                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                            </LayoutConfig>
                            <Items>
                                <tt:TTFieldSet ID="TTFieldSet3" runat="server" Title="Create New Binding" AutoHeight="true"
                                    Margins="0 0 5 0">
                                    <Items>
                                        <tt:TTContainer ID="TTContainer1" runat="server" Layout="Column">
                                            <Items>
                                                <tt:TTFormPanel ID="ctlLogicalSensorsForm" runat="server" ColumnWidth=".8">
                                                    <Items>
                                                        <tt:TTComboBox runat="server" ID="ctlLogicalSensors" FieldLabel="Logical Sensor"
                                                            StoreID="dsLogicalSensors" Editable="false" DisplayField="Name" ValueField="Name"
                                                            AllowBlank="false" AnchorHorizontal="98%">
                                                        </tt:TTComboBox>
                                                    </Items>
                                                </tt:TTFormPanel>
                                                <tt:TTFormPanel ID="TTFormPanel2" runat="server">
                                                    <Items>
                                                        <tt:TTCmdButon runat="server" CommandName="CreateLogicalSensorBindingInEditor" ID="ctlAddBinding"
                                                            Text="Create Binding" Width="100" AssociatedForm="ctlLocBindingsForm">
                                                        </tt:TTCmdButon>
                                                    </Items>
                                                </tt:TTFormPanel>
                                            </Items>
                                        </tt:TTContainer>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTGrid ID="ctlCurrentBindings" Flex="1" AnchorVertical="100%" AutoGenerateStateCommands="false"
                                    Title="Current Bindings" StoreID="dsLogicalBindings" runat="server" IsEditable="true">
                                    <ColumnModel ID="ColumnModel2" runat="server">
                                        <Columns>
                                            <tt:TTCommandColumn Width="24">
                                                <Commands>
                                                    <tt:TTGridCommand CommandName="RemoveLogicalSensorBinding" Confirm="true" Icon="Delete"
                                                        ToolTip-Text="Remove Binding">
                                                    </tt:TTGridCommand>
                                                    <tt:TTGridCommand CommandName="EditLogicalSensorBinding" Icon="NoteEdit" ToolTip-Text="Edit">
                                                    </tt:TTGridCommand>
                                                </Commands>
                                            </tt:TTCommandColumn>
                                            <tt:TTColumn DataIndex="LogicalSensorName" Header="LogicalSensor" Width="75">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="StateText" Header="Information" Width="50">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Startup" Header="Initial Startup" Width="50">
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Description" Width="50" Header="Description">
                                            </tt:TTColumn>
                                            <tt:TTCommandColumn Width="24" Header="Order">
                                                <Commands>
                                                    <tt:TTGridCommand CommandName="DecrementLogicalSensorBindingPosition" Icon="ArrowUp">
                                                    </tt:TTGridCommand>
                                                    <tt:TTGridCommand CommandName="IncrementLogicalSensorBindingPosition" Icon="ArrowDown">
                                                    </tt:TTGridCommand>
                                                </Commands>
                                            </tt:TTCommandColumn>
                                        </Columns>
                                    </ColumnModel>
                                    <SelectionModel>
                                        <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true">
                                        </ext:RowSelectionModel>
                                    </SelectionModel>
                                    <BottomBar>
                                        <tt:TTPagingToolbar ID="TTPagingToolbar1" runat="server" StoreID="dsLogicalBindings">
                                        </tt:TTPagingToolbar>
                                    </BottomBar>
                                    <Plugins>
                                        <ext:GridFilters runat="server" ID="GridFilters2" Local="false">
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
                        <uc3:ItemWatchControl runat="server" ID="ctlAnalyseItem" AnalyseItem="Processor" />
                    </Content>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlLogViewPanel" runat="server" Title="Log" Layout="FitLayout" Border="false"
                    Icon="DatabaseRefresh" Padding="0">
                    <Content>
                        <lv:LogViewer runat="server" ID="ctlLogView"></lv:LogViewer>
                    </Content>
                </tt:TTPanel>
                <tt:TTPanel ID="ctlLastEventsPanel" runat="server" Title="Last Events" Layout="FitLayout"
                    Border="false" BodyBorder="false" Padding="0">
                    <Content>
                        <lev:LastEventViewer runat="server" ID="ctlLastEvents" ItemType="Processor"></lev:LastEventViewer>
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
                    <Confirmation Message="Item seems running. Save ? This will restart process." ConfirmRequest="true"
                        BeforeConfirm="return confirmIfRunning(#{ctlSave});" />
                    <EventMask Msg="Processing ..." Target="CustomTarget" CustomTarget="#{entityWindow}"
                        ShowMask="True"></EventMask>
                </Click>
            </DirectEvents>
            <CustomConfig>
                <ext:ConfigItem Name="confirm" Value="false" Encode="false">
                </ext:ConfigItem>
            </CustomConfig>
        </tt:TTCmdButon>
        <tt:TTButton ID="TTButton1" runat="server" Text="Close">
            <Listeners>
                <Click Handler="#{entityWindow}.hide();" />
            </Listeners>
        </tt:TTButton>
    </Buttons>
</tt:TTWindow>
