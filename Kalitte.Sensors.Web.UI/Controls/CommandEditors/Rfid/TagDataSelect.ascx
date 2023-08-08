<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagDataSelect.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.TagDataSelect" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>
<table>
    <tr>
        <td style="padding-right:6px">
            <asp:CheckBox runat="server" ID="ctlAll" Text="All" Checked="true" />
        </td>
        <td style="padding-right:6px">
            <asp:CheckBox runat="server" ID="ctlTagId" Text="TagId" />
        </td>
        <td style="padding-right:6px">
            <asp:CheckBox runat="server" ID="ctlTagType" Text="Tag Type" />
        </td>
        <td style="padding-right:6px">
            <asp:CheckBox runat="server" ID="ctlTime" Text="Time" />
        </td>
        <td style="padding-right:6px">
            <asp:CheckBox runat="server" ID="ctlNumber" Text="Numbering System Identifier" />
        </td>
        <td>
            <asp:CheckBox runat="server" ID="ctlTagData" Text="Data" />
        </td>
    </tr>
</table>

