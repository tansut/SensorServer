<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommandWindow.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Pages.Sensors.CommandWindow" %>
<tt:TTStore runat="server" ID="dsCommands">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="DisplayName">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTWindow ID="entityWindow" runat="server" Title="Execute Command" MinHeight="500"
    Icon="Lightning" Width="725" Layout="FitLayout" AutoScroll="false" Hidden="true" BodyBorder="false">
    <Items>
        <ext:BorderLayout runat="server" ID="ctlTop">
            <West Split="true">
                <tt:TTGrid ID="ctlCommandsGrid" Flex="1" AnchorVertical="100%" AutoGenerateStateCommands="false"
                    StoreID="dsCommands" runat="server" IsEditable="true" Width="170" Margins="0 2 0 0">
                    <ColumnModel ID="ColumnModel1" runat="server">
                        <Columns>
                            <tt:TTColumn DataIndex="DisplayName" Header="Command">
                            </tt:TTColumn>
                        </Columns>
                    </ColumnModel>
                    <SelectionModel>
                        <ext:RowSelectionModel ID="RowSelectionModel1" runat="server" SingleSelect="true">
                            <DirectEvents>
                                <RowSelect OnEvent="SelectionChange_Event">
                                </RowSelect>
                            </DirectEvents>
                        </ext:RowSelectionModel>
                    </SelectionModel>
                    <Plugins>
                        <ext:GridFilters runat="server" ID="GridFilters1" Local="false">
                            <Filters>
                                <ext:StringFilter DataIndex="DisplayName" />
                            </Filters>
                        </ext:GridFilters>
                    </Plugins>
                    <View>
                        <ext:GridView ForceFit="true" />
                    </View>
                </tt:TTGrid>
            </West>
            <Center>
                <tt:TTPanel runat="server" ID="ctlCommandPanel" BodyBorder="false">
                    <AutoLoad Mode="IFrame" ShowMask="true" MaskMsg="Loading command editor ...">
                    </AutoLoad>
                </tt:TTPanel>
            </Center>
        </ext:BorderLayout>
    </Items>
</tt:TTWindow>
