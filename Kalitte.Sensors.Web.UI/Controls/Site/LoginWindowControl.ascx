<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginWindowControl.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Controls.Site.LoginWindowControl" %>

<script type="text/javascript">
    function showLoginWindow() {
        var window = Ext.getCmp('<%=LoginWindow.ClientID %>');
        window.show();
    }
</script>
<ext:Window ID="LoginWindow" runat="server" AutoHeight="true" Icon="Lock
" Title="Login"
    Width="350" Modal="true" Padding="5" Hidden="true" Layout="fit" >
    <Items>
        <tt:TTFormPanel runat="server" ID="loginPanel" MonitorValid="true" Border="false"
            BodyStyle="background-color:transparent;" Height="80">
            <Defaults>
                <ext:Parameter Name="AllowBlank" Value="false" Mode="Raw" />
            </Defaults>
            <Items>
                <tt:TTTextField ID="txtUsername" runat="server" FieldLabel="Username" 
                    AnchorHorizontal="98%" />
                <tt:TTTextField ID="txtPassword" runat="server" InputType="Password" FieldLabel="Password"
                     AnchorHorizontal="98%" />
                <ext:Checkbox runat="server" ID="ctlRememberMe" FieldLabel="Remember me" Checked="false">
                </ext:Checkbox>
            </Items>
        </tt:TTFormPanel>
    </Items>
    <Buttons>
        <ext:Button ID="loginOK" runat="server" Text="OK" Icon="Accept">
            <DirectEvents>
            </DirectEvents>
            <Listeners>
                <Click Handler="return #{loginPanel}.getForm().isValid();" />
            </Listeners>
            <DirectEvents>
                <Click OnEvent="btnLogin_Click">
                <EventMask ShowMask="true" CustomTarget="#{LoginWindow}" Target="CustomTarget" Msg="Login in progress ..." />
                </Click>
            </DirectEvents>
        </ext:Button>
    </Buttons>
</ext:Window>
