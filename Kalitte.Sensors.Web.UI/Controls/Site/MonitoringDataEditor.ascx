<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MonitoringDataEditor.ascx.cs"
    Inherits="Kalitte.Sensors.Web.UI.Controls.Site.MonitoringDataEditor" %>
<ext:FitLayout ID="FitLayout1" runat="server">
    <Items>
        <tt:TTCompositeField ID="ctlMonitoringDataForm" runat="server" AnchorHorizontal="100%">
            <Items>
                <tt:TTNumberField runat="server" ID="ctlCheckInterval" FieldLabel="Check Interval" 
                    AllowBlank="false">
                </tt:TTNumberField>
                <tt:TTNumberField runat="server" ID="ctlMaxRetryCount" FieldLabel="Max Retry Count" 
                    AllowBlank="false">
                </tt:TTNumberField>
                <tt:TTCheckBox runat="server" ID="ctlEnabled" FieldLabel="Enabled">
                </tt:TTCheckBox>
            </Items>
        </tt:TTCompositeField>
    </Items>
</ext:FitLayout>
