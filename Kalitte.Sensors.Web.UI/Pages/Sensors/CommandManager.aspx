<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="CommandManager.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Sensors.CommandManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
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
    <ext:Viewport runat="server">
        <Items>
            <ext:BorderLayout runat="server">
                <Center>
                    <tt:TTTabPanel runat="server" Title="Command Result" Border="true">
                        <TopBar>
                            <ext:Toolbar ID="Toolbar1" runat="server">
                                <Items>
                                    <tt:TTCmdButon ID="ctlExecCommand" runat="server" CommandName="ExecuteCommand" UseDirectClick="false"
                                        Icon="LightningGo" AutoPostBack="true" Text="Execute Command" />
                                        <ext:ToolbarSeparator runat="server"></ext:ToolbarSeparator>
                                    <tt:TTComboBox runat="server" ID="ctlSourceList" FieldLabel="Execute on Source" Width="250"
                                        ValueField="Value" DisplayField="Name" Editable="false" StoreID="dsSensorSources">
                                    </tt:TTComboBox>
                                    <ext:ToolbarFill ID="ToolbarFill1" runat="server">
                                    </ext:ToolbarFill>
                                    <ext:Container ID="Container1" runat="server">
                                        <Content>
                                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="false">
                                                <ProgressTemplate>
                                                    <asp:Label ID="Label1" runat="server" Text="Executing ..." ForeColor="Blue"></asp:Label>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </Content>
                                    </ext:Container>
                                </Items>
                            </ext:Toolbar>
                        </TopBar>
                        <Items>
                            <tt:TTPanel ID="TTPanel2" runat="server" Title="Command" Padding="5" AutoScroll="true">
                                <Content>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <asp:Label runat="server" ID="ctlCmdError" ForeColor="Red"></asp:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:UpdatePanel runat="server" ID="ctlEditorUp">
                                        <ContentTemplate>
                                            <asp:PlaceHolder ID="ctlCmdEditorHolder" runat="server"></asp:PlaceHolder>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:AsyncPostBackTrigger ControlID="ctlExecCommand" />
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </Content>
                            </tt:TTPanel>
                            <tt:TTPanel ID="TTPanel1" runat="server" Title="Response" Padding="5" Layout="FitLayout"
                                AutoScroll="true">
                                <Content>
                                    <asp:UpdatePanel runat="server" ID="ctlRespUp">
                                        <ContentTemplate>
                                            <ext:Label runat="server" ID="ctlResultAsstringLabel">
                                            </ext:Label>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </Content>
                            </tt:TTPanel>
                        </Items>
                    </tt:TTTabPanel>
                </Center>
            </ext:BorderLayout>
        </Items>
    </ext:Viewport>
</asp:Content>
