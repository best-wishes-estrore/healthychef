using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BayshoreSolutions.WebModules.Components.PagePicker
{
    public partial class PagePicker : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            //unfortunately PagePicker now requires "instanceId" or "parentInstanceID" to be in the query string.
            //we should probably expose a setter in the future, but it's not quite that simple...
            int navigationId;
            if (int.TryParse(Request.QueryString["instanceId"], out navigationId)
                //admin pages such as AddLink.aspx won't have an instance id.
                || int.TryParse(Request.QueryString["parentInstanceID"], out navigationId))
            {
                WebpageInfo webpage = Webpage.GetWebpage(navigationId);
                if (null == webpage) throw new ArgumentException("Invalid page.");
                //get the webpage's website by recursive lookup.
                Website website = webpage.Website;
                if (null == website) throw new InvalidOperationException("The current page is not associated with a website. A pages hierarchy cannot be constructed.");
                //magically override the SiteMapProvider root nav id. (yuck)
                BayshoreSolutions.WebModules.SiteMapProvider.magic_rootNavigationId =
                    website.RootNavigationId;
            }
            else
            { //we're screwed.
                navigationId = Website.Current.RootNavigationId;
            }

            base.OnInit(e);
        }
        private TreeNode findTreeNodeByNavigationId(TreeNodeCollection nodes, int navigationId)
        {
            TreeNode match = null;
            foreach (TreeNode n in nodes)
            {
                if (n.Value == navigationId.ToString()) match = n;
                else if (null != n.ChildNodes) match = findTreeNodeByNavigationId(n.ChildNodes, navigationId);
                if (null != match) break;
            }
            return match;
        }
        bool _isTreeDataBound = false;
        private void EnsureTreeData()
        {
            if (!_isTreeDataBound //prevent infinite loop
                && (null == TreeView1.Nodes || TreeView1.Nodes.Count == 0))
            {
                //this.DataBind() //causes an infinite loop because it tries to set SelectedNavigationId, which calls EnsureTreeData().

                TreeView1.DataBind();
                _isTreeDataBound = true;
            }
        }
        public int SelectedNavigationId
        {
            get
            {
                TreeNode selectedNode = TreeView1.SelectedNode;
                return (null == selectedNode) ? -1 : int.Parse(TreeView1.SelectedNode.Value);
            }
            set
            {
                EnsureTreeData();

                TreeNode n = findTreeNodeByNavigationId(TreeView1.Nodes, value);
                if (null == n)
                {
                    //throw new ArgumentException("Invalid navigation id.");
                    this.ClearSelection();
                }
                else
                {
                    n.Select();
                    onSelectNode();
                }
            }
        }
        /// <summary>
        /// Hides the branch beginning at the specified navigation id. 
        /// </summary>
        public int HideNavigationId
        {
            get { return (int)(ViewState["HideNavigationId"] ?? -1); }
            set { ViewState["HideNavigationId"] = value; }
        }
        public bool IsRequired
        {
            get { return uxRequiredFieldValidator.Visible; }
            set { uxRequiredFieldValidator.Visible = value; }
        }
        public string ValidationGroup
        {
            get { return uxRequiredFieldValidator.ValidationGroup; }
            set { uxRequiredFieldValidator.ValidationGroup = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            string script = "if (document.getElementById('" + treeDiv.ClientID
                + "').style.display=='none') { void(document.getElementById('" + treeDiv.ClientID
                + "').style.display='block');void(document.getElementById('" + ChooseLink.ClientID
                + "').innerHTML='Cancel');}else{void(document.getElementById('" + treeDiv.ClientID
                + "').style.display='none');void(document.getElementById('" + ChooseLink.ClientID
                + "').innerHTML='Browse...');}";
            ChooseLink.NavigateUrl = "javascript: " + script;
            WebpageNameTextBox.Attributes.Add("onclick", script);
        }
        public void ClearSelection()
        {
            if (null != TreeView1.SelectedNode)
            {
                TreeView1.SelectedNode.Selected = false;
            }
            WebpageNameTextBox.Text = String.Empty;
        }
        private const string NASTY_STYLE_KLUDGE_OPEN = "<span style=\"color: #5A5A5A;\">";
        private const string NASTY_STYLE_KLUDGE_CLOSE = "</span>";
        private void onSelectNode()
        {
            string pageName = TreeView1.SelectedNode.Text;
            //HACK: remove inline styling to get the page title.
            pageName = pageName.Replace(NASTY_STYLE_KLUDGE_OPEN, String.Empty);
            pageName = pageName.Replace(NASTY_STYLE_KLUDGE_CLOSE, String.Empty);
            WebpageNameTextBox.Text = pageName;

            // crawl all parents, expanding their nodes on the way
            TreeNode parentNode = TreeView1.SelectedNode.Parent;
            while (parentNode != null)
            {
                parentNode.Expand();
                parentNode = parentNode.Parent;
            }
        }
        protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            // set navigation url to empty; this allows node selection (rather than direct links)
            e.Node.NavigateUrl = String.Empty;

            e.Node.ToolTip = "Select the \"" + e.Node.Text + "\" page";

            SiteMapNode siteMapNode = (SiteMapNode)e.Node.DataItem;
            int webpageNavigationId = Convert.ToInt32(siteMapNode["WebpageNavigationId"]);
            bool display = Convert.ToBoolean(siteMapNode["Display"]);
            int roleCount = Convert.ToInt32(siteMapNode["RoleCount"]);

            // remove this node if it is defined as the stopping point
            if (webpageNavigationId == this.HideNavigationId
                && null != e.Node.Parent //except root node
                )
            {
                e.Node.Parent.ChildNodes.Remove(e.Node);
            }

            // set "value" to the navigation id, this will be fetched on selection change
            e.Node.Value = webpageNavigationId.ToString();

            if (!display)
            {
                e.Node.Text = NASTY_STYLE_KLUDGE_OPEN + e.Node.Text + NASTY_STYLE_KLUDGE_CLOSE;

                if (roleCount > 0)
                    e.Node.ImageUrl = "~/WebModules/Components/PagePicker/Images/Icon_SecurityHidden_16x16.gif";
                else
                    e.Node.ImageUrl = "~/WebModules/Components/PagePicker/Images/Icon_WebpageHidden_16x16.gif";
            }
            else
            {
                if (roleCount > 0)
                    e.Node.ImageUrl = "~/WebModules/Components/PagePicker/Images/Icon_Security_16x16.gif";
            }
        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            onSelectNode();
        }
        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearSelection();
        }
    }
}