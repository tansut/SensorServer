<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WriteIdCommandEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.WriteIdCommandEditor" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>
<table class="formTable" cellspacing="0" cellpadding="0">
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label1" runat="server" Text="Passcode:"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlPasscode" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label2" runat="server" Text="Tag ID (hex):"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlTagId" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="ctlTagId" ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label3" runat="server" Text="New Access Code:"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlNewAccessCode" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            &nbsp;</td>
    </tr>
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label4" runat="server" Text="New Kill Code:"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlNewKillCode" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            &nbsp;</td>
    </tr>
</table>
