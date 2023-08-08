<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogViewer.ascx.cs" Inherits="Kalitte.Sensors.Web.UI.Controls.Site.LogViewer" %>
<tt:TTEnumStore runat="server" ID="dsLogLevel" EnumType="Kalitte.Sensors.Security.LogLevel,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTStore runat="server" ID="dsLog" OnRefreshData="dsLog_RefreshData">
    <Reader>
        <ext:JsonReader>
            <Fields>
                <ext:RecordField Name="ManagedThreadId">
                </ext:RecordField>
                <ext:RecordField Name="Level">
                </ext:RecordField>
                <ext:RecordField Name="Time" Type="Date">
                </ext:RecordField>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Message">
                </ext:RecordField>
                <ext:RecordField Name="File">
                </ext:RecordField>
                <ext:RecordField Name="LineNumber">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<ext:TaskManager runat="server" ID="taskManager" Interval="2000">
    <Tasks>
        <ext:Task AutoRun="false" TaskID="updateChart">
            <DirectEvents>
                <Update OnEvent="UpdateLog">
                <EventMask ShowMask="true" CustomTarget="#{ctlGrid}" Msg="Auto refreshing" />
                </Update>
            </DirectEvents>
        </ext:Task>
    </Tasks>
</ext:TaskManager>
<ext:FitLayout runat="server">
    <Items>
        <tt:TTFormPanel ID="ctlLocForm" Padding="5" runat="server" AutoDoLayout="true">
            <TopBar>
                <tt:TTToolbar ID="TTToolbar1" runat="server">
                    <Items>
                        <tt:TTCmdButon runat="server" CommandName="QueryLog" ID="ctlAddBinding" Text="Query Log"
                            Icon="DatabaseRefresh" Width="100" AssociatedForm="ctlLogLevelForm,ctlLogMaxForm">
                            <DirectEvents>
                                <Click>
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{ctlGrid}" Msg="Loading ..." />
                                </Click>
                            </DirectEvents>
                        </tt:TTCmdButon>
                        <tt:TTNumberField runat="server" FieldLabel="Maximum Entries" ID="ctlMaxItems" MinValue="1"
                            MaxValue="100000" ValueAsInt="100" AllowBlank="false" Width="175">
                        </tt:TTNumberField>
                        <ext:ToolbarSeparator runat="server">
                        </ext:ToolbarSeparator>
                        <tt:TTCheckBox runat="server" ID="ctlAutoRefresh" Checked="false" FieldLabel="Auto Refresh" >
                            <DirectEvents>
                                <Check OnEvent="setAutoRefresh" />
                            </DirectEvents>
                        </tt:TTCheckBox>
                        <tt:TTNumberField runat="server" ID="ctlRefreshInterval" ValueAsInt="2000" AllowBlank="false"
                            MinValue="1000" MaxValue="60000" Enabled="false" FieldLabel="Refresh Interval" >
                        </tt:TTNumberField>
                    </Items>
                </tt:TTToolbar>
            </TopBar>
            <LayoutConfig>
                <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
            </LayoutConfig>
            <Items>
                <tt:TTFieldSet ID="TTFieldSet1" runat="server" Title="Query Settings" AutoHeight="true"
                    Collapsible="true" Margins="0 0 5 0">
                    <Listeners>
                        <Collapse Handler="#{ctlLocForm}.doLayout();" />
                        <Expand Handler="#{ctlLocForm}.doLayout();" />
                    </Listeners>
                    <Items>
                        <tt:TTContainer ID="Container1" runat="server" Layout="ColumnLayout" Height="90">
                            <Items>
                                <tt:TTFormPanel ID="ctlLogLevelForm" runat="server" ColumnWidth=".5" LabelAlign="Top">
                                    <Items>
                                        <tt:TTEnumCombo runat="server" ID="ctlLogLevel" FieldLabel="Log Level" AnchorHorizontal="98%"
                                            StoreID="dsLogLevel">
                                        </tt:TTEnumCombo>
                                        <tt:TTComboBox runat="server" ID="ctlLogSet" Editable="false" FieldLabel="Log Set"
                                            AnchorHorizontal="98%" ValueField="Name" DisplayField="Name">
                                            <Store>
                                                <ext:Store ID="Store2" runat="server">
                                                    <Reader>
                                                        <ext:ArrayReader>
                                                            <Fields>
                                                                <ext:RecordField Name="Name">
                                                                </ext:RecordField>
                                                            </Fields>
                                                        </ext:ArrayReader>
                                                    </Reader>
                                                </ext:Store>
                                            </Store>
                                        </tt:TTComboBox>
                                    </Items>
                                </tt:TTFormPanel>
                                <tt:TTFormPanel ID="ctlLogMaxForm" runat="server" ColumnWidth=".5" LabelAlign="Top">
                                    <Items>
                                        <ext:DropDownField ID="ctlNameFilter" runat="server" TriggerIcon="Search" FieldLabel="Name">
                                            <Component>
                                                <ext:GridPanel ID="ctlNameGrid" runat="server" Height="350" AutoExpandColumn="Name">
                                                    <Store>
                                                        <ext:Store ID="Store1" runat="server">
                                                            <Reader>
                                                                <ext:ArrayReader>
                                                                    <Fields>
                                                                        <ext:RecordField Name="Name">
                                                                        </ext:RecordField>
                                                                    </Fields>
                                                                </ext:ArrayReader>
                                                            </Reader>
                                                        </ext:Store>
                                                    </Store>
                                                    <ColumnModel ID="ColumnModel2" runat="server">
                                                        <Columns>
                                                            <ext:Column ColumnID="Name" Header="Name" DataIndex="Name" Width="220" />
                                                            <ext:CommandColumn>
                                                                <Commands>
                                                                    <ext:CommandFill />
                                                                    <ext:GridCommand Icon="Accept" ToolTip-Text="Select" CommandName="Pick">
                                                                    </ext:GridCommand>
                                                                    <ext:CommandSpacer Width="20" />
                                                                </Commands>
                                                            </ext:CommandColumn>
                                                        </Columns>
                                                    </ColumnModel>
                                                    <Listeners>
                                                        <Command Handler="this.dropDownField.setValue(record.data.Name);" />
                                                    </Listeners>
                                                    <LoadMask ShowMask="true" />
                                                    <SelectionModel>
                                                        <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" />
                                                    </SelectionModel>
                                                </ext:GridPanel>
                                            </Component>
                                        </ext:DropDownField>
                                        <tt:TTTextField runat="server" ID="ctlLogSearch" FieldLabel="Search" AnchorHorizontal="98%">
                                        </tt:TTTextField>
                                    </Items>
                                </tt:TTFormPanel>
                            </Items>
                        </tt:TTContainer>
                    </Items>
                </tt:TTFieldSet>
                <tt:TTGrid ID="ctlGrid" Flex="1" AnchorVertical="100%" AutoGenerateStateCommands="false"
                    StoreID="dsLog" runat="server" IsEditable="true" Layout="FitLayout" AutoExpandColumn="Message">
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <ext:RowNumbererColumn Width="50" />
                            <tt:TTColumn DataIndex="ManagedThreadId" Header="ThreadID" Width="60" Fixed="true">
                            </tt:TTColumn>
                            <tt:TTDateColumn DataIndex="Time" Header="Time" Width="120" Format="yyyy-MM-dd HH:mm:ss"
                                Fixed="true">
                            </tt:TTDateColumn>
                            <tt:TTColumn DataIndex="Name" Width="150" Header="Name">
                            </tt:TTColumn>
                            <tt:TTColumn DataIndex="Message" ColumnID="Message" Header="Message" Editable="true">
                            </tt:TTColumn>
                            <tt:TTColumn DataIndex="Level" Width="60" Header="Level" Fixed="true">
                            </tt:TTColumn>
                            <tt:TTColumn DataIndex="File" Header="File" Width="50">
                            </tt:TTColumn>
                            <tt:TTColumn DataIndex="LineNumber" Header="Line" Width="60" Fixed="true">
                            </tt:TTColumn>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                            <Listeners>
                                <RowSelect Handler="#{ctlMessage}.setValue(record.data.Message);" />
                            </Listeners>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <BottomBar>
                        <tt:TTPagingToolbar ID="userPaging" runat="server" StoreID="dsLog" PageSize="50">
                        </tt:TTPagingToolbar>
                    </BottomBar>
                    <Plugins>
                        <ext:GridFilters runat="server" ID="GridFilters1" Local="false">
                            <Filters>
                                <ext:StringFilter DataIndex="Name" />
                                <ext:DateFilter DataIndex="Time" />
                                <ext:StringFilter DataIndex="Level" />
                                <ext:NumericFilter DataIndex="ManagedThreadId">
                                </ext:NumericFilter>
                            </Filters>
                        </ext:GridFilters>
                    </Plugins>
                    <View>
                        <ext:GridView ForceFit="true" >
                                <GetRowClass Fn="getLogViewerRowClass" />
                        </ext:GridView>
                    </View>
                </tt:TTGrid>
                <tt:TTPanel ID="ctlMessagePanel" runat="server" Layout="FitLayout" Collapsible="true"
                    Title="Message Details" Height="100">
                    <Items>
                        <tt:TTTextArea runat="server" ID="ctlMessage">
                        </tt:TTTextArea>
                    </Items>
                </tt:TTPanel>
            </Items>
        </tt:TTFormPanel>
    </Items>
</ext:FitLayout>
