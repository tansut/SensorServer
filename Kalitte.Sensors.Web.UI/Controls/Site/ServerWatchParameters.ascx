<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ServerWatchParameters.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Controls.Site.ServerWatchParameters" %>
<tt:TTStore ID="dsWatch" runat="server">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
                <ext:RecordField Name="Description">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore ID="dsCategory" runat="server">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore ID="dsInstance" runat="server">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<tt:TTStore ID="dsMeasure" runat="server">
    <Reader>
        <ext:JsonReader IDProperty="Name">
            <Fields>
                <ext:RecordField Name="Name">
                </ext:RecordField>
            </Fields>
        </ext:JsonReader>
    </Reader>
</tt:TTStore>
<ext:FitLayout runat="server">
    <Items>
        <tt:TTPanel ID="ctlSourcePanel" runat="server" Title="Analysis Parameters" Margins="0 0 5 0"
            Padding="5" AutoHeight="true">
            <%--<LayoutConfig>
       <ext:VBoxLayoutConfig Align="Stretch" DefaultMargins="1 0 1 0" />
    </LayoutConfig>--%>
            <Items>
                <tt:TTFormPanel ID="ctlLocForm" runat="server">
                    <Items>
                        <tt:TTContainer ID="Container1" runat="server" Height="30" Layout="Column">
                            <Items>
                                <tt:TTFormPanel ID="ctlLogSourceForm" runat="server" ColumnWidth=".5">
                                    <Items>
                                        <tt:TTComboBox runat="server" ID="ctlProviders" FieldLabel="Provider" Editable="false"
                                            AllowBlank="false" AnchorHorizontal="98%" StoreID="dsWatch" ValueField="Name"
                                            DisplayField="Name">
                                            <DirectEvents>
                                                <Select OnEvent="ctlProviders_Select">
                                                <EventMask ShowMask="true" CustomTarget="#{ctlSourcePanel}" Target="CustomTarget" />
                                                </Select>
                                            </DirectEvents>
                                        </tt:TTComboBox>
                                    </Items>
                                </tt:TTFormPanel>
                                <tt:TTFormPanel ID="ctlCategoryForm" runat="server" ColumnWidth=".5">
                                    <Items>
                                        <tt:TTComboBox runat="server" ID="ctlCategory" FieldLabel="Categories" Editable="false"
                                            AllowBlank="false" AnchorHorizontal="98%" StoreID="dsCategory" ValueField="Name"
                                            DisplayField="Name">
                                            <DirectEvents>
                                                <Select OnEvent="ctlCategories_Select">
                                                <EventMask ShowMask="true" CustomTarget="#{ctlSourcePanel}" Target="CustomTarget" />

                                                </Select>
                                            </DirectEvents>
                                        </tt:TTComboBox>
                                    </Items>
                                </tt:TTFormPanel>
                            </Items>
                        </tt:TTContainer>
                    </Items>
                </tt:TTFormPanel>
                <tt:TTFormPanel ID="TTFormPanel1" runat="server">
                    <Items>
                        <tt:TTContainer ID="TTContainer1" Height="30" runat="server" Layout="ColumnLayout">
                            <Items>
                                <tt:TTFormPanel ID="ctlSelectLogSourceForm" runat="server" ColumnWidth=".5">
                                    <Items>
                                        <tt:TTComboBox runat="server" ID="ctlInstance" FieldLabel="Instance" Editable="false"
                                            AllowBlank="false" AnchorHorizontal="98%" StoreID="dsInstance" ValueField="Name"
                                            DisplayField="Name">
                                        </tt:TTComboBox>
                                    </Items>
                                </tt:TTFormPanel>
                                <tt:TTFormPanel ID="TTFormPanel2" runat="server" ColumnWidth=".5">
                                    <Items>
                                        <tt:TTComboBox runat="server" ID="ctlMeasure" FieldLabel="Measures" Editable="false"
                                            AllowBlank="false" AnchorHorizontal="98%" StoreID="dsMeasure" ValueField="Name"
                                            DisplayField="Name">
                                        </tt:TTComboBox>
                                    </Items>
                                </tt:TTFormPanel>
                            </Items>
                        </tt:TTContainer>
                    </Items>
                </tt:TTFormPanel>
            </Items>
        </tt:TTPanel>
    </Items>
</ext:FitLayout>
