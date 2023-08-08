<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HtmlWidget.ascx.cs" Inherits="Kalitte.WidgetLibrary.HtmlWidget.HtmlWidget" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Literal ID="Literal1" runat="server" Mode="PassThrough"></asp:Literal>
        
    </ContentTemplate>
</asp:UpdatePanel>

