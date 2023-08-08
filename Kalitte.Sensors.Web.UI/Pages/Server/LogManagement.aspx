<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="LogManagement.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Server.ServerManagement" %>

<%@ Register Src="../../Controls/Site/LogViewer.ascx" TagName="LogViewer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <ext:Viewport ID="Viewport1" runat="server">
        <Items>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <tt:TTPanel ID="TTPanel1" runat="server" Layout="FitLayout" Header="false">
                        <Content>
                            <uc1:LogViewer ID="ctlLogView" runat="server" />
                        </Content>
                    </tt:TTPanel>
                </Center>
                <North>
                    <tt:TTPanel ID="ctlLogSourcePanel" runat="server" Title="Log Source Selection" Height="60"
                        Margins="0 0 5 0" Padding="5">
                        <LayoutConfig>
                            <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
                        </LayoutConfig>
                        <Items>
                            <tt:TTFormPanel ID="ctlLocForm" runat="server">
                                <Items>
                                    <tt:TTContainer ID="Container1" runat="server" Layout="Column">
                                        <Items>
                                            <tt:TTFormPanel ID="ctlLogSourceForm" runat="server" ColumnWidth=".5">
                                                <Items>
                                                    <tt:TTComboBox runat="server" ID="ctlLogSource" FieldLabel="Log Source" Editable="false"
                                                        AllowBlank="false" AnchorHorizontal="98%">
                                                        <DirectEvents>
                                                            <Select OnEvent="ctlLogSource_Select">
                                                            </Select>
                                                        </DirectEvents>
                                                    </tt:TTComboBox>
                                                </Items>
                                            </tt:TTFormPanel>
                                            <tt:TTFormPanel ID="ctlLogInstanceForm" runat="server" ColumnWidth=".5">
                                                <Items>
                                                    <tt:TTComboBox runat="server" ID="ctlLogInstance" FieldLabel="Log Instance" Editable="false"
                                                        AllowBlank="false" AnchorHorizontal="98%">
                                                    </tt:TTComboBox>
                                                </Items>
                                            </tt:TTFormPanel>
                                            <%--                                            <tt:TTFormPanel ID="ctlSelectLogSourceForm" runat="server">
                                                <Items>
                                                    <tt:TTCmdButon runat="server" ID="ctlAddBinding" Text="Select Log Source" Width="100"
                                                        AssociatedForm="ctlLocForm">
                                                        <DirectEvents>
                                                            <Click OnEvent="ctlBindLogClick">
                                                            </Click>
                                                        </DirectEvents>
                                                    </tt:TTCmdButon>
                                                </Items>
                                            </tt:TTFormPanel>--%>
                                        </Items>
                                    </tt:TTContainer>
                                </Items>
                            </tt:TTFormPanel>
                        </Items>
                    </tt:TTPanel>
                </North>
            </ext:BorderLayout>
        </Items>
    </ext:Viewport>
</asp:Content>
