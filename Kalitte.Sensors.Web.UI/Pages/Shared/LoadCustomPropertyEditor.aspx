<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="LoadCustomPropertyEditor.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Shared.LoadCustomPropertyEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <script type="text/javascript">
        function doAjaxClick(e) {
            var el = document.getElementById(e);
            var postId = e.replace(/_/g, '$');
            if (el && browserName != "Firefox")
                el.click();
            else
                __doPostBack(postId, '');
        }

        function done(valueToReturn) {
            parent.window.PropertyEditorWindow.hide();
            parent.window.PropertyEditorWindow.editorControl.setValue(valueToReturn);
        }

        function StartEdit(e) {
            document.getElementById("ctlArgument").value = e;
            doAjaxClick("<% =ctlPostback.UniqueID %>");
        }

    </script>
    <ext:Viewport runat="server" ID="ctlViewport">
        <Items>
            <ext:BorderLayout runat="server" ID="ctlbl">
                <Center>
                    <tt:TTPanel runat="server" ID="ctlContentPanel" Header="false" Border="false" Padding="5"
                        AutoScroll="true">
                        <Content>
                            <asp:UpdatePanel runat="server" ID="ctlEditorUp">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="ctlMessage" Font-Bold="True" ForeColor="Red"></asp:Label>
                                    <asp:LinkButton runat="server" ID="ctlPostback" OnClick="startEditing"></asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="ctlArgument" ClientIDMode="Static" />
                                    <asp:PlaceHolder ID="ctlCmdEditorHolder" runat="server"></asp:PlaceHolder>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="ctlSave" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </Content>
                        <Buttons>
                            <tt:TTCmdButon runat="server" ID="ctlSave" CommandName="SaveSettings" Text="Save">
                            </tt:TTCmdButon>
                        </Buttons>
                    </tt:TTPanel>
                </Center>
            </ext:BorderLayout>
        </Items>
    </ext:Viewport>
</asp:Content>
