<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Dispatchers.Editor" %>
<%@ Register Src="ProcessorEditor.ascx" TagName="ProcessorEditor" TagPrefix="me" %>
<tt:TTEnumStore runat="server" ID="dsStartup" EnumType="Kalitte.Sensors.Processing.ItemStartupType,Kalitte.Sensors">
</tt:TTEnumStore>
<%@ Register Src="~/Controls/Site/ItemWatchControl.ascx" TagName="ItemWatchControl"
    TagPrefix="uc3" %>
<%@ Register Src="~/Controls/Site/LogViewer.ascx" TagName="LogViewer" TagPrefix="lv" %>
<%@ Register Src="~/Controls/Site/MonitoringDataEditor.ascx" TagName="MonitoringDataEditor"
    TagPrefix="uc4" %>
<script type="text/javascript">
<%@ Register Src="~/Controls/Site/LastEventsViewer.ascx" TagName="LastEventViewer"
    TagPrefix="lev" %>
    function confirmIfRunning(config) {
        return config.confirm;
    }
</script>
<tt:TTStore runat="server" ID="dsProcessors">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore ID="dsDispatcherProcessors" runat="server" OnRefreshData="dsProcessorBindings_RefreshData">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Processor">
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
<me:ProcessorEditor runat="server" ID="ctlProcessorEditor"></me:ProcessorEditor>
<tt:TTWindow ID="entityWindow" runat="server" Title="Edit Dispatcher" MinHeight="500"
    Width="700" Layout="FitLayout" AutoScroll="false">
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
                                <tt:TTTextField ID="ctlType" runat="server" FieldLabel="Type" AllowBlank="false">
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
                <tt:TTPanel ID="ctlProcessorBindingsPanel" runat="server" Title="Processors" Layout="FitLayout"
                    Padding="5">
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
                                                        <tt:TTComboBox runat="server" ID="ctlProcessors" FieldLabel="Processors" StoreID="dsProcessors"
                                                            Editable="false" DisplayField="Name" ValueField="Name" AllowBlank="false" AnchorHorizontal="98%">
                                                        </tt:TTComboBox>
                                                    </Items>
                                                </tt:TTFormPanel>
                                                <tt:TTFormPanel ID="ctlCreateBindingForm" runat="server">
                                                    <Items>
                                                        <tt:TTCmdButon runat="server" CommandName="CreateProcessorInEditor" ID="ctlAddModule"
                                                            Text="Add Processor" Width="100" AssociatedForm="ctlModulesForm">
                                                        </tt:TTCmdButon>
                                                    </Items>
                                                </tt:TTFormPanel>
                                            </Items>
                                        </tt:TTContainer>
                                    </Items>
                                </tt:TTFieldSet>
                                <tt:TTGrid ID="ctlCurrentModuleBindings" Flex="1" AnchorVertical="100%" Title="Current processors of dispatcher"
                                    StoreID="dsDispatcherProcessors" runat="server" AutoGenerateStateCommands="false"
                                    IsEditable="true">
                                    <ColumnModel ID="ColumnModel1" runat="server">
                                        <Columns>
                                            <tt:TTCommandColumn Width="28">
                                                <Commands>
                                                    <tt:TTGridCommand CommandName="RemoveProcessorBinding" Confirm="true" Icon="Delete"
                                                        ToolTip-Text="Remove Binding">
                                                    </tt:TTGridCommand>
                                                    <tt:TTGridCommand CommandName="EditProcessorBinding" Icon="NoteEdit" ToolTip-Text="Edit">
                                                    </tt:TTGridCommand>
                                                </Commands>
                                            </tt:TTCommandColumn>
                                            <tt:TTColumn DataIndex="State" Header="State" Width="40">
                                                <Renderer Fn="renderStateColor" />
                                            </tt:TTColumn>
                                            <tt:TTColumn DataIndex="Processor" Header="Processor" Width="75">
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
                                        <tt:TTPagingToolbar ID="userPaging" runat="server" StoreID="dsDispatcherProcessors">
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
                        <tt:PropertyProfileEditor runat="server" ID="ctlProfileEditor" OnSetPropertyProfile="setPropertiesHandler">
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
                        <uc3:ItemWatchControl runat="server" ID="ctlAnalyseItem" AnalyseItem="Dispatcher" />
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
                        <lev:lasteventviewer runat="server" id="ctlLastEvents" itemtype="Dispatcher">
                        </lev:lasteventviewer>
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
