<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagMemorySelect.ascx.cs"
    Inherits="Kalitte.Sensors.Rfid.Client.CommandEditors.TagMemorySelect" %>
<%@ Import Namespace="Kalitte.Sensors.Utilities" %>
<div>
    <table class="tagMemory" runat="server" id="imgTable" cellpadding="0" cellspacing="0">
        <tr>
            <td style="padding-top: 55px">
                <asp:RadioButton runat="server" ID="ctlReserved" GroupName="tagMemory"  />
            </td>
            <td style="padding-top: 60px;">
                <asp:RadioButton runat="server" ID="ctlTid" GroupName="tagMemory" />
            </td>
        </tr>
        <tr>
            <td style="width: 50%; padding-top: 40px">
                <asp:RadioButton runat="server" ID="ctlEpc" GroupName="tagMemory" Checked="true" />
            </td>
            <td style="padding-top: 40px">
                <asp:RadioButton runat="server" ID="ctlUser" GroupName="tagMemory" />
            </td>
        </tr>
    </table>
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
                <asp:Label ID="Label5" runat="server" Text="Memory Seek:"></asp:Label>
            </td>
            <td colspan="2">
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Start Byte:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                        <td>
                            <asp:Label ID="ctlLengthLabel" runat="server" Text="Length:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox runat="server" ID="ctlStart" Text="4"></asp:TextBox>
                        </td>
                        <td class="validCol">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                ErrorMessage="*" ControlToValidate="ctlStart"></asp:RequiredFieldValidator></td>
                        <td>
                            <asp:TextBox runat="server" ID="ctlLength" Text="12"></asp:TextBox>
                        </td>
                        <td class="validCol">
                            <asp:RequiredFieldValidator ID="ctlLengthValidator" runat="server" 
                                ErrorMessage="*" ControlToValidate="ctlLength"></asp:RequiredFieldValidator> </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
