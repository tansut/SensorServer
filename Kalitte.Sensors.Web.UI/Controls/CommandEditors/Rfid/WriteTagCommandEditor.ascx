<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WriteTagCommandEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.WriteTagCommandEditor" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>
<%@ Register TagPrefix="mem" Src="TagMemorySelect.ascx" TagName="memvis" %>
<mem:memvis runat="server" id="ctlMemVis" LengthEnabled="false">
</mem:memvis>
<table class="formTable" cellspacing="0" cellpadding="0">
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label2" runat="server" Text="Tag Data (hex):"></asp:Label>
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