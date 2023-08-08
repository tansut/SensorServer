
<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="Management.aspx.cs" 
    Inherits="Kalitte.Sensors.Web.UI.Pages.Processors.Management" %>

<%@ Register Src="List.ascx" TagName="List" TagPrefix="uc1" %>
<%@ Register Src="Editor.ascx" TagName="Editor" TagPrefix="uc2" %>
<%@ Register Src="ModuleState.ascx" TagName="ModuleState" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <uc2:Editor ID="editor" runat="server" />
    <uc3:ModuleState ID="moduleState" runat="server" />
    <ext:Viewport ID="Viewport1" runat="server">
        <Items>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <Center>
                    <ext:Panel ID="Panel1" runat="server" Layout="fit" Border="false">
                        <Content>
                            <uc1:List ID="lister" runat="server" />
                        </Content>
                    </ext:Panel>
                </Center>
            </ext:BorderLayout>
        </Items>
    </ext:Viewport>
</asp:Content>
