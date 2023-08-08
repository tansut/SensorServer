<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GetTagsCommandEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.QueryTagsCommandEditor" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>

<%@ Register TagPrefix="mem" Src="TagDataSelect.ascx" TagName="memvis" %>


<table class="formTable" cellspacing="0" cellpadding="0">
    <tr>
        <td class="labelCol">
            <asp:Label ID="Label1" runat="server" Text="Passcode:"></asp:Label>
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
            <asp:Label ID="Label2" runat="server" Text="Tag Data Selector:"></asp:Label>
        </td>
        <td class="controlCol">
            <mem:memvis runat="server" id="ctlDataSelect" LengthEnabled="false">
</mem:memvis>
        </td>
        <td class="validCol">
            &nbsp;
        </td>
    </tr>

</table>
<asp:Repeater ID="tagRepeater" runat="server" OnItemDataBound="tagRepeater_ItemDataBound">
    <HeaderTemplate>
        <table class="dataListTable" cellspacing="0" cellpadding="0" style="font-size:11px">
            <tr class="header">
                <td>
                    Antenna
                </td>
                <td>
                    Type
                </td>
                <td>
                    ID
                </td>
                <td>
                    Data
                </td>
                <td>
                    TagTime
                </td>
                <td>
                    Last Seen
                </td>
                <td>
                    Rssi
                </td>
                <td>
                    Count
                </td>
                <td>
                    Vendor
                </td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr class="item">
            <td style="width: 60px">
                <%# Eval("Source")%>
            </td>
            <td style="width: 60px">
                <%# Eval("Type")%>
            </td>
            <td style="width: 180px">
                <asp:Label ID="ctlTagId" runat="server"></asp:Label>
            </td>
            <td style="width: 180px">
                <asp:Label ID="ctlTagData" runat="server"></asp:Label>
            </td>
            <td style="width: 120px">
                <%# Eval("Time")%>
            </td>
            <td style="width: 120px">
                <asp:Label ID="ctlLastSeen" runat="server"></asp:Label>
            </td>
            <td style="width: 50px">
                <asp:Label ID="ctlRssi" runat="server"></asp:Label>
            </td>
            <td style="width: 50px">
                <asp:Label ID="ctlReadCount" runat="server"></asp:Label>
            </td>
            <td style="width: 20px">
                <asp:Label ID="ctlVendorData" runat="server"></asp:Label>
            </td>
        </tr>
    </ItemTemplate>
    <SeparatorTemplate>
        <tr class="seperatorRow">
            <td colspan="5">
            </td>
        </tr>
    </SeparatorTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
