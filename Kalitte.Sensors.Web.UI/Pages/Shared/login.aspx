<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Kalitte.Sensors.Web.UI.Pages.Shared.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <tt:TTWindow runat="server" Title="Management Console Login" AutoHeight="true"
        Width="400" Hidden="false" Maximizable="false" Draggable="false" Closable="false">
        <Items>
            <tt:TTFormPanel runat="server" ID="ctlLoginForm" BodyBorder="false" Border="false"
                Padding="5">
                <Items>
                    <ext:Label runat="server" ID="ctlLoginHelp" Icon="Note" HideLabel="true" Html="For first time login please use <b>admin/change</b> credentials. To remove this information change showInitialLoginHelp application settings to false in web.config.">
                    </ext:Label>
                    <tt:TTFieldSet runat="server" Title="Credentials" Layout="FormLayout">
                        <Items>
                            <tt:TTTextField runat="server" ID="ctlUsername" FieldLabel="Username" AllowBlank="false"
                                AnchorHorizontal="100%">
                            </tt:TTTextField>
                            <tt:TTTextField runat="server" ID="ctlPassword" FieldLabel="Password" AllowBlank="false"
                                InputType="Password">
                            </tt:TTTextField>
                            <tt:TTCheckBox runat="server" ID="ctlRemember" FieldLabel="Remember me">
                            </tt:TTCheckBox>
                        </Items>
                    </tt:TTFieldSet>
                    <tt:TTFieldSet ID="TTFieldSet1" runat="server" Title="Server" Layout="FormLayout">
                        <Items>
                            <tt:TTTextField runat="server" ID="ctlHost" FieldLabel="Host" AllowBlank="false">
                            </tt:TTTextField>
                            <tt:TTNumberField runat="server" ID="ctlPort" FieldLabel="Port" AllowBlank="false">
                            </tt:TTNumberField>
                        </Items>
                    </tt:TTFieldSet>
                </Items>
                <Buttons>
                    <tt:TTButton runat="server" Icon="Key" Text="Login" IconAlign="Left" ID="ctlLoginButton"
                        Type="Submit" AssociatedForm="ctlLoginForm">
                        <DirectEvents>
                            <Click OnEvent="ctlLogin_Click">
                                <EventMask ShowMask="true" CustomTarget="#{ctlLoginForm}" Target="CustomTarget" Msg="Login in progress ..." />
                            </Click>
                        </DirectEvents>
                    </tt:TTButton>
                </Buttons>
            </tt:TTFormPanel>
        </Items>
    </tt:TTWindow>
</asp:Content>
