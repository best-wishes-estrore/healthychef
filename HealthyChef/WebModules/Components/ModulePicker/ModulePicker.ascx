<%@ Control Language="C#" ClassName="ModulePicker" %>


<script runat="server">
    protected int _selectedNavigationId;
    protected int _hideNavigationId;

    public int SelectedNavigationId
    {
        get
        {
            if (!string.IsNullOrEmpty(NavigationIdHiddenField.Value))
                _selectedNavigationId = Convert.ToInt32(NavigationIdHiddenField.Value);
            else
                _selectedNavigationId = -1;
            
            return _selectedNavigationId;
        }
        set
        {
            _selectedNavigationId = value;
            NavigationIdHiddenField.Value = _selectedNavigationId.ToString();
        }
    }

    public int HideNavigationId
    {
        get
        {
            if (_hideNavigationId == null)
                _hideNavigationId = -1;
            return _hideNavigationId;
        }
        set
        {
            _hideNavigationId = value;
        }
    }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string javaScript = "javascript: if (document.getElementById('" + treeDiv.ClientID + "').style.display=='none') { void(document.getElementById('" + treeDiv.ClientID + "').style.display='block');void(document.getElementById('" + ChooseLink.ClientID + "').childNodes[0].src='" + ResolveUrl("~/WebModules/Components/ModulePicker/Images/Button_CancelChange.gif") + "');}else{void(document.getElementById('" + treeDiv.ClientID + "').style.display='none');void(document.getElementById('" + ChooseLink.ClientID + "').childNodes[0].src='" + ResolveUrl("~/WebModules/Components/ModulePicker/Images/Button_ChooseAPage.gif") + "');}";
        ChooseLink.NavigateUrl = javaScript;
        WebpageNameTextBox.Attributes.Add("OnClick", javaScript);
        TreeView1.CssClass = "TreeView";
        CoverFrame.Attributes.Add("class", "CoverFrame");
    }

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        string pageName = TreeView1.SelectedNode.Text;
        pageName = pageName.Replace("<span style='color:gray;'>", "");
        pageName = pageName.Replace("</span>", "");
        WebpageNameTextBox.Text = pageName;
        NavigationIdHiddenField.Value = TreeView1.SelectedNode.Value;
        ModuleList.DataSource = BayshoreSolutions.WebModules.WebModule.GetModules(Convert.ToInt32(TreeView1.SelectedNode.Value));
        //ModuleList.DataSource = WebModules.WebModuleHelper.GetModules(Convert.ToInt32(TreeView1.SelectedNode.Value));
        ModuleList.DataTextField = "WebModuleName";
        ModuleList.DataValueField = "WebModuleId";
        ModuleList.DataBind();
    }

    protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
    {
        // set navigation url to empty; this allows node selection (rather than direct links)
        e.Node.NavigateUrl = "";

        // set tooltip text telling the user to click on a page
        e.Node.ToolTip = "Click to select the '" + e.Node.Text + "' webpage.";

        // get the navigation id for this page
        int webpageNavigationId = Convert.ToInt32(((SiteMapNode)e.Node.DataItem)["WebpageNavigationId"]);
        bool display = Convert.ToBoolean(((SiteMapNode)e.Node.DataItem)["Display"]);
        int roleCount = Convert.ToInt32(((SiteMapNode)e.Node.DataItem)["RoleCount"]);
        
        // remove this node if its defined as the stopping point
        if (webpageNavigationId == HideNavigationId)
            e.Node.Parent.ChildNodes.Remove(e.Node);

        // set "value" to the navigation id, this will be fetched on selection change
        e.Node.Value = webpageNavigationId.ToString();

        // if this is the current page, pre-select
        //(populate nodes from client must be set to false for this to work, otherwise deeper
        // levels are not fetched until client side click events)
        if (webpageNavigationId == SelectedNavigationId)
        {
            e.Node.Select();

            // crawl all parents, expanding their nodes on the way
            TreeNode parentNode = e.Node.Parent;
            while (parentNode != null)
            {
                parentNode.Expand();
                parentNode = parentNode.Parent;
            }

            // update the text box
            WebpageNameTextBox.Text = e.Node.Text;
        }

        // set display properties
        if (!display)
        {
            e.Node.Text = "<span style='color:gray;'>" + e.Node.Text + "</span>";

            if (roleCount > 0)
                e.Node.ImageUrl = "~/WebModules/Components/ModulePicker/Images/Icon_SecurityHidden_16x16.gif";
            else
                e.Node.ImageUrl = "~/WebModules/Components/ModulePicker/Images/Icon_WebpageHidden_16x16.gif";
        }
        else
        {
            if (roleCount > 0)
                e.Node.ImageUrl = "~/WebModules/Components/ModulePicker/Images/Icon_Security_16x16.gif";
        }
        
    }

    
</script>

<style type="text/css">

    .TreeView
    {
        width:300px;
        height:300px;
        overflow:scroll;
        background-color:white;
        border:1px #7F9DB9 solid;
        position:absolute;
    }
    .CoverFrame
    {
        position:absolute;
        height:303px;
        width:303px;
    }
    .NodeStyle
    {
        border:1px white solid;
        padding-left:1px;
        padding-right:1px;
        
        font-family: verdana;
        font-size:8pt;
        color:black;
    }
    .HoverNodeStyle
    {
        background-color:skyblue;
        border:1px blue solid;
        color:white;
    }
</style>
<asp:TextBox ID="WebpageNameTextBox" runat="server" ReadOnly="True" Width="163px" />
<asp:HyperLink ID="ChooseLink" runat="server"><img alt="" id="Img1" src="~/WebModules/Components/ModulePicker/Images/Button_ChooseAPage.gif" border="0" runat="server" style="vertical-align:middle;" /></asp:HyperLink>
<div runat="server" id="treeDiv" style="display: none;">
    <asp:HiddenField ID="NavigationIdHiddenField" runat="server" />
    <iframe id="CoverFrame" runat="server" frameborder="0"></iframe>
    <asp:TreeView ID="TreeView1" runat="server" DataSourceID="SiteMapDataSource1" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged"
        OnTreeNodeDataBound="TreeView1_TreeNodeDataBound" ExpandDepth="1" ShowLines="True"
        PopulateNodesFromClient="False">
        <HoverNodeStyle CssClass="HoverNodeStyle" />
        <NodeStyle CssClass="NodeStyle" ImageUrl="~/WebModules/Components/ModulePicker/Images/Icon_Webpage_16x16.gif" />
        <SelectedNodeStyle CssClass="HoverNodeStyle" />
        <RootNodeStyle ImageUrl="~/WebModules/Components/ModulePicker/Images/Icon_Home_16x16.gif" />
    </asp:TreeView>
</div><br />
<asp:DropDownList ID="ModuleList" runat="server"></asp:DropDownList>
<asp:SiteMapDataSource ID="SiteMapDataSource1" runat="server" SiteMapProvider="WebModulesSiteMapFullProvider" />


