<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ModuleEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Pages.Processors.ModuleEditor" %>
<%@ Register Src="~/Controls/Site/ItemWatchControl.ascx" TagName="ItemWatchControl"
    TagPrefix="uc3" %>
<tt:TTEnumStore runat="server" ID="dsStartup" EnumType="Kalitte.Sensors.Processing.ItemStartupType,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTEnumStore runat="server" ID="dsNonExistEventHandler" EnumType="Kalitte.Sensors.Processing.NonExistEventHandlerBehavior,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTEnumStore runat="server" ID="dsNullEventBehavior" EnumType="Kalitte.Sensors.Processing.PipeNullEventBehavior,Kalitte.Sensors">
</tt:TTEnumStore>
<tt:TTWindow ID="entityWindow" runat="server" Title="Edit Processor Module" MinHeight="500"
    Icon="Package" Width="550" Layout="FitLayout" AutoScroll="false">
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
                                <ext:Label runat="server" ID="ctlProcessorName" FieldLabel="Processor">
                                </ext:Label>
                                <ext:Label runat="server" ID="ctlModuleName" FieldLabel="Module">
                                </ext:Label>
                                <tt:TTTextField ID="ctlDescription" runat="server" FieldLabel="Description">
                                </tt:TTTextField>
                                <tt:TTFieldSet ID="TTFieldSet2" runat="server" Title="State">
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
                                <tt:TTFieldSet ID="ctlEventPipeGroup" runat="server" Title="Event Pipe Settings"
                                    AnchorHorizontal="100%" LabelWidth="120">
                                    <Items>
                                        <tt:TTCheckBox LabelAlign="Left" runat="server" ID="ctlInheritNonExistBehaviour"
                                            FieldLabel="Inherit from process">
                                        </tt:TTCheckBox>
                                        <tt:TTEnumCombo runat="server" Width="400" ID="ctlNonExistEventHandler" StoreID="dsNonExistEventHandler"
                                            AllowBlank="false" FieldLabel="Non Exist Event Handler Behaviour">
                                        </tt:TTEnumCombo>
                                        <tt:TTCheckBox LabelAlign="Left" runat="server" ID="ctlInheritNullEvent" FieldLabel="Inherit from process">
                                        </tt:TTCheckBox>
                                        <tt:TTEnumCombo runat="server" Width="400" ID="ctlNullSensorEvent" StoreID="dsNullEventBehavior"
                                            AllowBlank="false" FieldLabel="Null Sensor Event Behaviour">
                                        </tt:TTEnumCombo>
                                    </Items>
                                </tt:TTFieldSet>
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
                        <uc3:ItemWatchControl runat="server" ID="ctlAnalyseItem2" AnalyseItem="EventModule" />
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
