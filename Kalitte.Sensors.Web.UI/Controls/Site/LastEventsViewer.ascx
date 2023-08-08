<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LastEventsViewer.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Controls.Site.LastEventsViewer" %>
<tt:TTStore ID="dsMain" runat="server" AutoLoad="false">
    <Reader>
        <ext:JsonReader>
            <Fields>
                <ext:RecordField Name="EventTime" Type="Date">
                </ext:RecordField>
                <ext:RecordField Name="TypeName">
                </ext:RecordField>
                <ext:RecordField Name="Source">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore ID="dsFilterSource" runat="server" AutoLoad="false">
    <Reader>
        <ext:JsonReader IDProperty="Source">
            <Fields>
                <ext:RecordField Name="Source">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore ID="dsFilterType" runat="server" AutoLoad="false">
    <Reader>
        <ext:JsonReader IDProperty="Type">
            <Fields>
                <ext:RecordField Name="Type">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<ext:TaskManager runat="server" ID="taskManager" Interval="2000">
    <Tasks>
        <ext:Task AutoRun="false" TaskID="updateChart">
            <DirectEvents>
                <Update OnEvent="UpdateData">
                    <EventMask ShowMask="true" CustomTarget="#{ctlLastEventsGrid}" Msg="Auto refreshing" />
                </Update>
            </DirectEvents>
        </ext:Task>
    </Tasks>
</ext:TaskManager>
<tt:TTWindow runat="server" Width="500" Height="500" CloseAction="Hide" Modal="true"
    Title="Set Filter" ID="ctlFilterWindow">
    <Items>
        <ext:RowLayout ID="RowLayout1" runat="server" Split="true">
            <Rows>
                <ext:LayoutRow RowHeight="0.5">
                    <tt:TTPanel ID="TTPanel3" runat="server" Border="false" BodyBorder="false" Padding="5"
                        RowHeight="0.5">
                        <Items>
                            <tt:TTFitLayout ID="TTFitLayout1" runat="server">
                                <Items>
                                    <tt:TTGrid ID="ctlFilterSourceGrid" AutoGenerateStateCommands="false" Header="false"
                                        StoreID="dsFilterSource" runat="server" AutoExpandColumn="filterSourceColumn"
                                        Border="false">
                                        <TopBar>
                                            <tt:TTToolbar ID="TTToolbar3" runat="server" Layout="HBoxLayout">
                                                <Items>
                                                    <tt:TTCompositeField ID="TTCompositeField2" runat="server" LabelSource="UseFieldLabel"
                                                        AnchorHorizontal="100%">
                                                        <Items>
                                                            <tt:TTTextField runat="server" ID="ctlFilterSource" EmptyText="Filter Source" Flex="1">
                                                            </tt:TTTextField>
                                                            <tt:TTButton runat="server" ID="ctlFilterSourceAdd" Icon="Add" IconAlign="Left" Text="Add">
                                                                <DirectEvents>
                                                                    <Click OnEvent="ctlFilterSourceAdd_Click">
                                                                    </Click>
                                                                </DirectEvents>
                                                            </tt:TTButton>
                                                        </Items>
                                                    </tt:TTCompositeField>
                                                </Items>
                                            </tt:TTToolbar>
                                        </TopBar>
                                        <ColumnModel>
                                            <Columns>
                                                <tt:TTColumn ColumnID="filterSourceColumn" Header="Filter Source" DataIndex="Source"
                                                    Editable="false">
                                                </tt:TTColumn>
                                                <tt:TTCommandColumn Width="30">
                                                    <Commands>
                                                        <tt:TTGridCommand Icon="Delete" CommandName="filterSourceDelete">
                                                            <ToolTip Text="Delete" />
                                                        </tt:TTGridCommand>
                                                    </Commands>
                                                </tt:TTCommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel3" runat="server" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                    </tt:TTGrid>
                                </Items>
                            </tt:TTFitLayout>
                        </Items>
                    </tt:TTPanel>
                </ext:LayoutRow>
                <ext:LayoutRow RowHeight="0.5">
                    <tt:TTPanel ID="TTPanel4" runat="server" Border="false" BodyBorder="false" Padding="5"
                        RowHeight="0.5">
                        <Items>
                            <tt:TTFitLayout ID="TTFitLayout2" runat="server">
                                <Items>
                                    <tt:TTGrid ID="ctlFilterTypeGrid" AutoGenerateStateCommands="false" Header="false"
                                        StoreID="dsFilterType" runat="server" AutoExpandColumn="filterTypeColumn" Border="false">
                                        <TopBar>
                                            <tt:TTToolbar ID="TTToolbar2" runat="server" Layout="HBoxLayout">
                                                <Items>
                                                    <tt:TTCompositeField ID="TTCompositeField1" runat="server" LabelSource="UseFieldLabel"
                                                        AnchorHorizontal="100%">
                                                        <Items>
                                                            <tt:TTComboBox runat="server" ID="ctlFilterType" EmptyText="Event Type" ForceSelection="false"
                                                                Flex="1">
                                                            </tt:TTComboBox>
                                                            <tt:TTButton runat="server" ID="ctlFilterTypeAdd" Icon="Add" IconAlign="Left" Text="Add">
                                                                <DirectEvents>
                                                                    <Click OnEvent="ctlFilterTypeAdd_Click">
                                                                    </Click>
                                                                </DirectEvents>
                                                            </tt:TTButton>
                                                        </Items>
                                                    </tt:TTCompositeField>
                                                </Items>
                                            </tt:TTToolbar>
                                        </TopBar>
                                        <ColumnModel>
                                            <Columns>
                                                <tt:TTColumn ColumnID="filterTypeColumn" Header="Filter Type" DataIndex="Type" Editable="false">
                                                </tt:TTColumn>
                                                <tt:TTCommandColumn Width="30">
                                                    <Commands>
                                                        <tt:TTGridCommand Icon="Delete" CommandName="filterTypeDelete">
                                                            <ToolTip Text="Delete" />
                                                        </tt:TTGridCommand>
                                                    </Commands>
                                                </tt:TTCommandColumn>
                                            </Columns>
                                        </ColumnModel>
                                        <SelectionModel>
                                            <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                                            </ext:RowSelectionModel>
                                        </SelectionModel>
                                    </tt:TTGrid>
                                </Items>
                            </tt:TTFitLayout>
                        </Items>
                    </tt:TTPanel>
                </ext:LayoutRow>
            </Rows>
        </ext:RowLayout>
    </Items>
    <Buttons>
        <tt:TTCmdButon runat="server" CommandName="SetLastEventFilter" ID="TTCmdButon1" Text="Set Filter"
            Width="100">
            <Listeners>
                <Click Handler="#{ctlFilterType}.fireEvent('change');" />
            </Listeners>
        </tt:TTCmdButon>
        <tt:TTCmdButon runat="server" CommandName="ClearLastEventFilter" ID="TTCmdButon2"
            Text="Clear Filter" Width="100">
        </tt:TTCmdButon>
    </Buttons>
</tt:TTWindow>
<ext:FitLayout runat="server">
    <Items>
        <tt:TTPanel runat="server" Header="false" BodyBorder="false" Padding="5" ID="topPanel">
            <TopBar>
                <tt:TTToolbar ID="TTToolbar1" runat="server">
                    <Items>
                        <tt:TTCmdButon runat="server" CommandName="QueryLastEvents" ID="ctlQuery" Text="Query Last Events"
                            Icon="DatabaseRefresh" Width="100">
                            <DirectEvents>
                                <Click>
                                    <EventMask ShowMask="true" Target="CustomTarget" CustomTarget="#{topPanel}" Msg="Loading ..." />
                                </Click>
                            </DirectEvents>
                        </tt:TTCmdButon>
                        <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </ext:ToolbarSeparator>
                        <tt:TTCheckBox runat="server" ID="ctlAutoRefresh" Checked="false" FieldLabel="Auto Refresh">
                            <DirectEvents>
                                <Check OnEvent="setAutoRefresh" />
                            </DirectEvents>
                        </tt:TTCheckBox>
                        <tt:TTNumberField runat="server" ID="ctlRefreshInterval" ValueAsInt="2000" AllowBlank="false"
                            MinValue="1000" MaxValue="60000" Enabled="false" FieldLabel="Refresh Interval">
                        </tt:TTNumberField>
                        <ext:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                        </ext:ToolbarSeparator>
                        <ext:Button ID="Button1" runat="server" Text="Create Filter">
                            <Listeners>
                                <Click Handler="#{ctlFilterWindow}.show();" />
                            </Listeners>
                        </ext:Button>
                    </Items>
                </tt:TTToolbar>
            </TopBar>
            <Items>
                <ext:BorderLayout runat="server">
                    <Center>
                        <tt:TTPanel runat="server" Layout="FitLayout" Padding="0" Header="false" Border="false">
                            <Items>
                                <tt:TTFormPanel ID="TTPanel1" runat="server" Title="Event Information" HideLabels="true">
                                    <Items>
                                        <tt:TTTextField ID="ctlEventType" runat="server" FieldLabel="Type" Text="Event Type"
                                            ReadOnly="true" />
                                        <tt:TTTextArea runat="server" ID="ctlEventData" ReadOnly="true" Flex="1">
                                        </tt:TTTextArea>
                                    </Items>
                                    <LayoutConfig>
                                        <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                                    </LayoutConfig>
                                </tt:TTFormPanel>
                            </Items>
                        </tt:TTPanel>
                    </Center>
                    <West Split="true">
                        <tt:TTGrid ID="ctlLastEventsGrid" AutoGenerateStateCommands="false" Header="false"
                            StoreID="dsMain" runat="server" IsEditable="true" AutoExpandColumn="sourceCol"
                            Width="350" Border="false">
                            <ColumnModel ID="ColumnModel2" runat="server">
                                <Columns>
                                    <tt:TTDateColumn DataIndex="EventTime" Header="EventTime" Width="120" Fixed="true"
                                        Format="yyyy-MM-dd HH:mm:ss">
                                    </tt:TTDateColumn>
                                    <tt:TTColumn DataIndex="TypeName" Header="Type" ColumnID="typeCol">
                                    </tt:TTColumn>
                                    <tt:TTColumn DataIndex="Source" Header="Source" ColumnID="sourceCol">
                                    </tt:TTColumn>
                                </Columns>
                            </ColumnModel>
                            <SelectionModel>
                                <ext:RowSelectionModel ID="RowSelectionModel2" runat="server" SingleSelect="true">
                                    <DirectEvents>
                                        <RowSelect OnEvent="selectLastEvent">
                                        </RowSelect>
                                    </DirectEvents>
                                </ext:RowSelectionModel>
                            </SelectionModel>
                            <Plugins>
                                <ext:GridFilters runat="server" ID="GridFilters2" Local="false">
                                    <Filters>
                                        <ext:DateFilter DataIndex="EventTime" />
                                        <ext:StringFilter DataIndex="TypeName">
                                        </ext:StringFilter>
                                        <ext:StringFilter DataIndex="Source">
                                        </ext:StringFilter>
                                    </Filters>
                                </ext:GridFilters>
                            </Plugins>
                            <View>
                                <ext:GridView ForceFit="true">
                                    <GetRowClass Fn="getLastEventViewerRowClass" />
                                </ext:GridView>
                            </View>
                        </tt:TTGrid>
                    </West>
                </ext:BorderLayout>
            </Items>
        </tt:TTPanel>
    </Items>
</ext:FitLayout>
