<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpgradeFirmwareCommandEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.UpgradeFirmwareCommandEditor" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>
<table class="formTable" cellspacing="0" cellpadding="0">
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label2" runat="server" Text="Firmware Location:"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlLocation" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                ControlToValidate="ctlLocation" ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
    
</table>
