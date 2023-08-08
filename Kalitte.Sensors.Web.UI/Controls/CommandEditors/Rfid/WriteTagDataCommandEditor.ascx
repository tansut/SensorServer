<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReadTagDataCommandEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.WriteTagDataCommandEditor" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>
<%@ Register TagPrefix="mem" Src="TagMemorySelect.ascx" TagName="memvis" %>
<table class="formTable" cellspacing="0" cellpadding="0">
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label3" runat="server" Text="Passcode:"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlPasscode" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            &nbsp;
        </td>
    </tr>
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label4" runat="server" Text="Tag ID (hex):"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlTagId" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ctlTagId"
                ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label1" runat="server" Text="Tag Data (hex):"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlTagData" TextMode="MultiLine" Rows="3" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ctlTagData"
                ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
    </tr>
</table>
