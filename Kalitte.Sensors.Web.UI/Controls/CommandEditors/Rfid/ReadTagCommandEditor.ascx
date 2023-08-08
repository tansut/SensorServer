<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReadTagCommandEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.ReadTagCommandEditor" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>
<%@ Register TagPrefix="mem" Src="TagMemorySelect.ascx" TagName="memvis" %>
<mem:memvis runat="server" id="ctlMemVis">
</mem:memvis>
<table class="formTable" cellspacing="0" cellpadding="0" id="ctlResult" runat="server"
    visible="false">
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label1" runat="server" Text="Read Data (hex):"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlData" TextMode="MultiLine" Rows="3" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            &nbsp;
        </td>
    </tr>
    <tr id="ctlVendorRow" runat="server" visible="false">
        <td class="labelCol">
            <asp:Label ID="Label3" runat="server" Text="Vendor Data:"></asp:Label>
        </td>
        <td class="controlCol">
            <asp:TextBox ID="ctlVendor" TextMode="MultiLine" Rows="3" runat="server" Width="100%"></asp:TextBox>
        </td>
        <td class="validCol">
            &nbsp;
        </td>
    </tr>
</table>
