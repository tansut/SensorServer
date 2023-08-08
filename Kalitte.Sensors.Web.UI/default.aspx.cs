using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Kalitte.Sensors.Web.UI;
using Kalitte.Dashboard.Framework.Types;
using System.Threading;
using Ext.Net;
using System.Web.Security;
using Kalitte.Sensors.Web.UI.Controls.Site;
using Kalitte.Dashboard.Framework;
using System.Configuration;
using Kalitte.Sensors.Web.Business;

namespace Kalitte.Sensors.Web.UI
{
    public partial class _default : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                SiteMapNode siteNode = SiteMap.RootNode;
                TreeNode root = this.CreateNode(siteNode);
                ManageNodes(root, root);
                AddUserDashboards(root);
                AddSharedDashboards(root);
                ctlMenuTree.Root.Add(root);
                ResourceManager.GetInstance(this).AddScript(string.Format("loadPage(Ext.getCmp('{0}'), Ext.getCmp('{1}').getRootNode()); ", ExampleTabs.ClientID, ctlMenuTree.ClientID));
                ResourceManager.GetInstance(this).AddScript(string.Format("Ext.getCmp('{0}').expandAll();", ctlMenuTree.ClientID));
                ctlVersion.Text = string.Format("Build {0}", ConfigurationManager.AppSettings["ProductVersion"]);

            }
        }
        

        private void AddSharedDashboards(TreeNode root)
        {
            List<DashboardInstance> list;
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                list = DashboardFramework.GetDashboards(DashboardShareType.Workgroup).Where(p => p.Username != Thread.CurrentPrincipal.Identity.Name).Select(p => p).ToList();
                list.AddRange(DashboardFramework.GetDashboards(DashboardShareType.AllUsers).Select(p => p).ToList());

            }
            else
            {
                list = DashboardFramework.GetDashboards(DashboardShareType.AllUsers).Select(p => p).ToList();
            }
            if (list.Count > 0)
            {
                TreeNode node = new TreeNode("Shared Dashboards");
                root.Nodes.Add(node);
                foreach (DashboardInstance instance in list)
                {
                    TreeNode dNode = new TreeNode(instance.Title, (Icon)Enum.Parse(typeof(Icon), instance.Icon.ToString()));
                    dNode.Href = ResolveClientUrl(string.Format("~/Pages/Dynamic/ViewDashboard.aspx?d={0}", instance.InstanceKey));
                    node.Nodes.Add(dNode);
                }

            }
        }

        private void AddUserDashboards(TreeNode root)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                List<DashboardInstance> list = DashboardFramework.GetDashboards(Thread.CurrentPrincipal.Identity.Name);
                TreeNode node = new TreeNode("My Dashboards");
                root.Nodes.Add(node);
                foreach (DashboardInstance instance in list)
                {
                    TreeNode dNode = new TreeNode(instance.Title, (Icon)Enum.Parse(typeof(Icon), instance.Icon.ToString()));
                    dNode.Href = ResolveClientUrl(string.Format("~/Pages/Dynamic/ViewDashboard.aspx?d={0}", instance.InstanceKey));
                    node.Nodes.Add(dNode);
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            CreateChildControlsDynamicControls();
        }

        protected void Logoff(object sender, DirectEventArgs e)
        {
            AuthenticationBusiness.Logout();
        }

        private MenuItem createThemeMenuItem(Theme theme, string title)
        {
            MenuItem i = new MenuItem(title);
            if (ResMan.Theme == theme)
                i.Icon = Icon.Tick;
            i.CustomConfig.Add(new ConfigItem("theme", "'" + ResMan.GetThemeUrl(theme) + "'", ParameterMode.Value));
            i.Listeners.Click.Handler = "changeTheme(el)";
            return i;
        }


        private void CreateChildControlsDynamicControls()
        {

            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                if (!X.IsAjaxRequest)
                {
                    authenticatedUserToolbar.Visible = true;
                    anonymousUserToolbar.Visible = false;
                    ctlUserInfo.Text = string.Format("{0}", Thread.CurrentPrincipal.Identity.Name);
                    var loginData = AuthenticationBusiness.GetLoginData();
                    ctlServerInfo.Text = string.Format("{0}:{1}", loginData.ServerHost, loginData.Port);
                }
                dynamicControls.Controls.Add(ChangeUserPasswordControl.LoadInto(this));
            }
            else
            {
                dynamicControls.Controls.Add(LoginWindowControl.LoadInto(this));
                if (!X.IsAjaxRequest)
                {
                    authenticatedUserToolbar.Visible = false;
                    anonymousUserToolbar.Visible = true;
                }
            }

        }

        [DirectMethod]
        public string RefreshMenu()
        {
            SiteMapNode siteNode = SiteMap.RootNode;
            TreeNode root = this.CreateNode(siteNode);
            ManageNodes(root, root);
            AddUserDashboards(root);
            AddSharedDashboards(root);
            Ext.Net.TreeNodeCollection nodes = new TreeNodeCollection() { root };
            return nodes.ToJson();
        }

        [DirectMethod]
        public string GetThemeUrl(string theme)
        {
            Theme temp = (Theme)Enum.Parse(typeof(Theme), theme);

            this.Session["Ext.Net.Theme"] = temp;

            return (temp == Ext.Net.Theme.Default) ? "Default" : ResMan.GetThemeUrl(temp);
        }

        [DirectMethod]
        public static int GetHashCode(string s)
        {
            return Math.Abs(("/Pages" + s).ToLower().GetHashCode());
        }

        private void ManageNodes(TreeNode root, TreeNode node)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                TreeNode n = node.Nodes[i] as TreeNode;

                for (int j = 0; j < root.Nodes.Count + 3; j++)
                {
                    ManageNodes(root, n);
                    if (n.ParentNode != null && n.Nodes.Count == 0 && n.Href == "#")
                        node.Nodes.Remove(n);
                }

            }
            if (root.Nodes.Count == 1 && (root.Nodes[0] as TreeNode).Nodes.Count == 0)
                root.Nodes.RemoveAt(0);
        }

        private TreeNode CreateNode(SiteMapNode siteMapNode)
        {
            TreeNode treeNode = new TreeNode();
            if (!string.IsNullOrEmpty(siteMapNode.Url) && !siteMapNode.Url.StartsWith("/x"))
            {
                treeNode.Href = this.Page.ResolveUrl(siteMapNode.Url);
            }
            treeNode.NodeID = siteMapNode.Key;
            treeNode.Text = siteMapNode.Title;

            if (!string.IsNullOrEmpty(siteMapNode.Description))
            {
                string[] parts = siteMapNode.Description.Split(':');
                if (parts.Length == 2)
                {
                    //treeNode.Qtip = parts[1];
                    treeNode.Icon = (Icon)Enum.Parse(typeof(Icon), parts[1]);
                }
                //else treeNode.Icon = (Icon)Enum.Parse(typeof(Icon), siteMapNode.Description);
            }

            SiteMapNodeCollection children = siteMapNode.ChildNodes;
            if (children != null && children.Count > 0)
            {
                foreach (SiteMapNode mapNode in siteMapNode.ChildNodes)
                {
                    TreeNode newNode = this.CreateNode(mapNode);

                    if (userHasRight(mapNode))
                        treeNode.Nodes.Add(newNode);
                }
            }

            return treeNode;
        }

        private bool userHasRight(SiteMapNode mapNode)
        {
            if (mapNode.Roles.Count == 0)
                return true;
            foreach (string role in mapNode.Roles)
            {
                if (Roles.IsUserInRole(role))
                    return true;
            }
            return false;
        }




    }
}