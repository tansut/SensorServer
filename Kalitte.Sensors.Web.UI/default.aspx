<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/DefaultMaster.Master"
    AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Kalitte.Sensors.Web.UI._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="pageContentHolder" runat="server">
    <asp:PlaceHolder ID="dynamicControls" runat="server"></asp:PlaceHolder>
    <ext:Viewport ID="ViewPort1" runat="server">
        <Items>
            <ext:BorderLayout ID="BorderLayout1" runat="server">
                <North>
                    <ext:Panel ID="Panel1" runat="server" Height="26" BodyBorder="false" Border="false"
                        Layout="FitLayout">
                        <Items>
                            <ext:Toolbar ID="anonymousUserToolbar" runat="server">
                                <Items>
                                    <ext:Label runat="server" Html="<b>Kalitte Sensor Server Management Console</b>"
                                        ID="nologinNameToolbar">
                                    </ext:Label>
                                    <ext:ToolbarFill ID="ToolbarFill">
                                    </ext:ToolbarFill>
                                    <ext:Button runat="server" ID="Button2" Icon="Tab" Text="Close Tabs">
                                        <Listeners>
                                            <Click Handler="closeTabs(#{ExampleTabs});" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:Button ID="loginBtn" runat="server" Icon="LockBreak" Text="Login">
                                        <Listeners>
                                            <Click Handler="showLoginWindow()" />
                                        </Listeners>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                            <ext:Toolbar ID="authenticatedUserToolbar" runat="server">
                                <Items>
                                    <ext:Label ID="ctlUserInfo" Icon="UserKey" runat="server">
                                    </ext:Label>
                                    <ext:ToolbarSeparator runat="server">
                                    </ext:ToolbarSeparator>
                                    <ext:Label runat="server" ID="ctlServerInfo" Icon="Server">
                                    </ext:Label>
                                    <ext:ToolbarFill ID="ToolbarFill2">
                                    </ext:ToolbarFill>
                                    <ext:Button runat="server" ID="ctlCloseTabs" Icon="Tab" Text="Close Tabs">
                                        <Listeners>
                                            <Click Handler="closeTabs(#{ExampleTabs});" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator5" runat="server">
                                    </ext:ToolbarSeparator>
                                    <ext:Button ID="changePasswordBtn" runat="server" Icon="UserEdit" Text="Change Password">
                                        <Listeners>
                                            <Click Handler="showPasswordChangeWindow()" />
                                        </Listeners>
                                    </ext:Button>
                                    <ext:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                    </ext:ToolbarSeparator>
                                    <ext:Button ID="ctlLogoff" runat="server" Icon="StopRed" Text="Logout">
                                        <DirectEvents>
                                            <Click OnEvent="Logoff">
                                            </Click>
                                        </DirectEvents>
                                    </ext:Button>
                                </Items>
                            </ext:Toolbar>
                        </Items>
                    </ext:Panel>
                </North>
                <West Collapsible="true" Split="true" CollapseMode="Mini" MarginsSummary="4 0 4 4">
                    <ext:Panel ID="Panel21" runat="server" Layout="Fit" Width="180" Header="false" Border="false">
                        <Items>
                            <ext:TreePanel ID="ctlMenuTree" runat="server" Header="false" AutoScroll="true" Lines="false"
                                UseArrows="true" CollapseFirst="false" ContainerScroll="true" RootVisible="true">
                                <TopBar>
                                    <ext:Toolbar ID="Toolbar1" runat="server">
                                        <Items>
                                            <ext:TriggerField ID="TriggerField1" runat="server" EnableKeyEvents="true" Width="120"
                                                EmptyText="Filter Items">
                                                <Triggers>
                                                    <ext:FieldTrigger Icon="Clear" HideTrigger="true" />
                                                </Triggers>
                                                <Listeners>
                                                    <KeyUp Fn="filterTree" Buffer="100" />
                                                    <TriggerClick Fn="clearFilter" />
                                                </Listeners>
                                            </ext:TriggerField>
                                            <ext:ToolbarFill ID="ToolbarFill1" runat="server" />
                                            <ext:Button ID="ctlRefreshTree" runat="server" Icon="PageRefresh" ToolTip="Reload Items">
                                                <Listeners>
                                                    <Click Handler="refreshTree(#{ctlMenuTree});" />
                                                </Listeners>
                                            </ext:Button>
                                            <ext:Button ID="Button1" runat="server" Icon="Cog" ToolTip="Options">
                                                <Menu>
                                                    <ext:Menu ID="Menu1" runat="server">
                                                        <Items>
                                                            <ext:MenuItem ID="MenuItem1" runat="server" Text="Expand All" IconCls="icon-expand-all">
                                                                <Listeners>
                                                                    <Click Handler="#{ctlMenuTree}.root.expand(true);" />
                                                                </Listeners>
                                                            </ext:MenuItem>
                                                            <ext:MenuItem ID="MenuItem2" runat="server" Text="Collapse All" IconCls="icon-collapse-all">
                                                                <Listeners>
                                                                    <Click Handler="#{ctlMenuTree}.root.collapse(true);" />
                                                                </Listeners>
                                                            </ext:MenuItem>
                                                            <ext:MenuSeparator ID="MenuSeparator1" runat="server" />
                                                            <ext:MenuItem ID="MenuItem3" runat="server" Text="Theme" Icon="Paintcan">
                                                                <Menu>
                                                                    <ext:Menu ID="Menu2" runat="server">
                                                                        <Items>
                                                                            <ext:CheckMenuItem ID="CheckMenuItem1" runat="server" Text="Blue" Group="theme">
                                                                                <CustomConfig>
                                                                                    <ext:ConfigItem Name="theme" Mode="Value" Value="Default">
                                                                                    </ext:ConfigItem>
                                                                                </CustomConfig>
                                                                            </ext:CheckMenuItem>
                                                                            <ext:CheckMenuItem ID="CheckMenuItem2" runat="server" Text="Gray" Checked="true"
                                                                                Group="theme">
                                                                                <CustomConfig>
                                                                                    <ext:ConfigItem Name="theme" Mode="Value" Value="Gray">
                                                                                    </ext:ConfigItem>
                                                                                </CustomConfig>
                                                                            </ext:CheckMenuItem>
                                                                            <ext:CheckMenuItem ID="MenuItem4" runat="server" Text="Slate" Group="theme">
                                                                                <CustomConfig>
                                                                                    <ext:ConfigItem Name="theme" Mode="Value" Value="Slate">
                                                                                    </ext:ConfigItem>
                                                                                </CustomConfig>
                                                                            </ext:CheckMenuItem>
                                                                        </Items>
                                                                        <Listeners>
                                                                            <ItemClick Handler="TT.GetThemeUrl(menuItem.theme,{
                                                                                success : function (result) {
                                                                                    Ext.net.ResourceMgr.setTheme(result);
                                                                                    pageContentHolder_ExampleTabs.items.each(function (el) {
                                                                                        if (!Ext.isEmpty(el.iframe)) {
                                                                                            if (el.getBody().Ext) {
                                                                                                el.getBody().Ext.net.ResourceMgr.setTheme(result);
                                                                                            }
                                                                                        }
                                                                                    });
                                                                                }
                                                                            });" />
                                                                        </Listeners>
                                                                    </ext:Menu>
                                                                </Menu>
                                                            </ext:MenuItem>
                                                        </Items>
                                                    </ext:Menu>
                                                </Menu>
                                            </ext:Button>
                                        </Items>
                                    </ext:Toolbar>
                                </TopBar>
                                <Listeners>
                                    <Click Handler="if (node.attributes.href) { e.stopEvent(); loadPage(#{ExampleTabs}, node);}" />
                                </Listeners>
                            </ext:TreePanel>
                        </Items>
                    </ext:Panel>
                </West>
                <Center MarginsSummary="4 4 5 0">
                    <ext:TabPanel ID="ExampleTabs" runat="server" EnableTabScroll="true" Border="false"
                        BodyBorder="false">
                        <Plugins>
                            <ext:TabCloseMenu ID="TabCloseMenu1" runat="server" />
                        </Plugins>
                    </ext:TabPanel>
                </Center>
                <South Collapsible="false" Split="false">
                    <ext:Panel ID="Panel2" Height="10" runat="server" Border="false">
                        <BottomBar>
                            <ext:StatusBar ID="StatusBar1" runat="server" DefaultText="<b>Kalitte Sensor Server Management Console </b>">
                                <Items>
                                    <ext:ToolbarFill runat="server">
                                    </ext:ToolbarFill>
                                    <ext:ToolbarTextItem runat="server" Html="<b>© 2011</b> Kalitte" ID="ToolbarTextItem1">
                                    </ext:ToolbarTextItem>
                                    <ext:ToolbarTextItem runat="server" ID="ctlVersion" Text="V1.0">
                                    </ext:ToolbarTextItem>
                                </Items>
                            </ext:StatusBar>
                        </BottomBar>
                    </ext:Panel>
                </South>
            </ext:BorderLayout>
        </Items>
    </ext:Viewport>
</asp:Content>
