<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Editor.ascx.cs" Inherits="Kalitte.WidgetLibrary.HtmlWidget.Editor" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

   
<cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" BorderStyle="None">
    <cc1:TabPanel runat="server" HeaderText="Html" ID="TabPanel2">
        <ContentTemplate>
<asp:TextBox ID="TextBox1" runat="server" Height="475" TextMode="MultiLine" 
    Width="100%"></asp:TextBox>
        </ContentTemplate>
    </cc1:TabPanel>
       <cc1:TabPanel runat="server" HeaderText="Java Script" ID="TabPanel3">
        <ContentTemplate>
<asp:TextBox ID="TextBox2" runat="server" Height="475" TextMode="MultiLine" 
    Width="100%"></asp:TextBox>
        </ContentTemplate>
    </cc1:TabPanel>
</cc1:TabContainer>

