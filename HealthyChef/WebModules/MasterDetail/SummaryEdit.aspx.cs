using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class SummaryEdit : BayshoreSolutions.WebModules.Cms.ModuleAdminPage
    {
        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        override protected void EnsureModule()
        {
            MasterDetailSetting module = MasterDetailSetting.FetchByID(this.ModuleId);
            if (null == module)
            {
                module = new MasterDetailSetting();
                module.ModuleId = this.ModuleId;
                module.Save();
            }
        }

        public string ModuleTemplate
        {
            get { return ((string)ViewState["ModuleTemplate"]) ?? String.Empty; }
            set { ViewState["ModuleTemplate"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/MasterDetail/public/css/MasterDetail.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            Header.Controls.Add(cssLink);

            if (!IsPostBack)
            {
                LoadSettings();
                List_MasterDetail_Item(null, null);
            }
        }

        private void LoadSettings()
        {
            MasterDetailSetting settings = MasterDetailSetting.FetchByID(this.ModuleId);
            IsPostDateVisible.Checked = settings.IsPostDateVisible;
            txtItemsPerPage.Text = settings.ItemsPerPage.ToString();
            cbRequireAuthentication.Checked = settings.RequireAuthentication;
            cbAllowComments.Checked = settings.AllowComments;
            cbTagFilter.Checked = settings.ShowTagFilter;
            cbShowImageIfBlank.Checked = settings.ShowImageIfBlank;

            // Load list of templates
            dlTemplateList.DataSource = BayshoreSolutions.WebModules.WebDirectories.GetTemplates();
            dlTemplateList.DataBind();
            if (string.IsNullOrEmpty(settings.Template))
            {
                var _instanceId = int.Parse(Request.QueryString["InstanceId"]);
                var _page = Webpage.GetWebpage(_instanceId);

                dlTemplateList.SelectedValue = _page.TemplateGroup + " - " + _page.Template;
            }
            else dlTemplateList.SelectedValue = settings.Template;

            ModuleTemplate = dlTemplateList.SelectedValue;
        }
        protected void MasterDetail_List_SaveButton_Click(object sender, EventArgs e)
        {
            MasterDetailSetting settings = MasterDetailSetting.FetchByID(this.ModuleId);
            settings.IsPostDateVisible = IsPostDateVisible.Checked;
            settings.ItemsPerPage = int.Parse(txtItemsPerPage.Text);
            settings.RequireAuthentication = cbRequireAuthentication.Checked;
            settings.AllowComments = cbAllowComments.Checked;
            settings.ShowTagFilter = cbTagFilter.Checked;
            settings.ShowImageIfBlank = cbShowImageIfBlank.Checked;
            settings.Template = ModuleTemplate = dlTemplateList.SelectedValue;
            settings.Save();
            Msg.ShowSuccess("Saved settings.");
        }

        protected void MasterDetail_Item_List_PageIndexChanging(Object sender, GridViewPageEventArgs e)
        {
            MasterDetail_Item_List.PageIndex = e.NewPageIndex;
            List_MasterDetail_Item(null, null);
        }

        private void List_MasterDetail_Item(string sortExpr, bool? sortDir)
        {
            MultiViewCtl.SetActiveView(ListView);
            MasterDetail_Item_List.DataSource = MasterDetailList.GetMasterDetailChildren(this.ModuleId, false, false);
            MasterDetail_Item_List.DataBind();
        }

        protected void MasterDetail_Item_List_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int moduleId = (int)MasterDetail_Item_List.DataKeys[e.RowIndex].Value;
            MasterDetailList.DestroyMasterDetailItem(moduleId);
            List_MasterDetail_Item(null, null);
            Msg.Show("The content item was deleted.");
        }

        protected void MasterDetail_Item_List_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            int moduleId = int.Parse(e.CommandArgument.ToString());
            if (e.CommandName == "MoveUp" || e.CommandName == "MoveDown")
            {
                WebModuleInfo module = WebModule.GetModule(moduleId);
                WebpageInfo page = module.Webpage;
                if (e.CommandName == "MoveUp")
                    Webpage.SortWebpageUp(page.InstanceId);
                else
                    Webpage.SortWebpageDown(page.InstanceId);

                Response.Redirect(Request.Url.ToString());
            }

        }

        protected void MasterDetail_Item_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                WebModuleInfo MasterDetailModule = (WebModuleInfo)e.Row.DataItem;
                WebpageInfo page = MasterDetailModule.Webpage;
                MasterDetailItem item = MasterDetailItem.GetSafeResource(MasterDetailModule.Id);
                System.Web.UI.HtmlControls.HtmlAnchor selectLink = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("SelectLink");
                selectLink.HRef = ResolveUrl(MasterDetailModule.GetEditUrl())
                    //cheesy
                    + "&returnTo=" + Server.UrlEncode(Request.Url.PathAndQuery);

                selectLink.InnerHtml = MasterDetailItem.Chop(page.Title, 45, true);

                Literal postDateCtl = (Literal)e.Row.FindControl("PostDateCtl");
                if (page.PostDate.HasValue)
                {
                    postDateCtl.Text = page.PostDate.Value.ToShortDateString();
                    if (page.PostDate.Value.TimeOfDay.TotalSeconds > .001)
                    {
                        postDateCtl.Text = string.Format("{0} {1}", postDateCtl.Text, page.PostDate.Value.ToShortTimeString());
                    }
                }

                CheckBox visibleCtl = (CheckBox)e.Row.FindControl("VisibleCtl");
                visibleCtl.Checked = page.Visible;

                CheckBox featuredCtl = (CheckBox)e.Row.FindControl("FeaturedCtl");
                featuredCtl.Checked = item.IsFeatured;
            }
        }

        protected void CreateNewButton_Click(object sender, EventArgs e)
        {
            MultiViewCtl.SetActiveView(EditView);
        }

        protected void MasterDetail_SaveButton_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                DetailEditControl1.CreateNewContentPageModule(this.PageNavigationId, ModuleTemplate);
                Response.Redirect(Request.Url.ToString());
            }
        }

        protected void MasterDetail_CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }


        #region paging/sorting (not implemented)
        //protected void MasterDetail_Item_List_Sorting(object sender, GridViewSortEventArgs e)
        //{
        //    //we must manually track the previous sort parameters, because GridView 
        //    //toggles e.SortDirection only when binding to a datasource object. 
        //    string previousSortExpr = ViewState["MasterDetail_Item_List_sortExpr"] as string;
        //    bool sortDir = true; //default to ascending.
        //    if (previousSortExpr == e.SortExpression)
        //    { //the same column is being sorted.
        //        sortDir = !(bool)(ViewState["MasterDetail_Item_List_sortDir"] ?? true); //toggle the sort direction.
        //    }
        //    ViewState["MasterDetail_Item_List_sortDir"] = sortDir;
        //    ViewState["MasterDetail_Item_List_sortExpr"] = e.SortExpression;

        //    //List_MasterDetail_ItemResource(e.SortExpression, sortDir);

        //    //prevent redundant sort (we already sorted at the database).
        //    e.Cancel = true;
        //}

        ////
        ////Database-side GridView-paging requires an ObjectDataSource;
        ////ObjectDataSource-paging requires the following method signatures.
        ////see: http://weblogs.asp.net/alessandro/archive/2007/10/09/custom-serverside-paging-in-gridview-vs-datagrid.aspx
        ////
        ////required for paging.
        //int _totalCount_MasterDetail_ItemResource = 0;
        ////required for paging
        //public int GetCount_MasterDetail_ItemResource(/*...dummy parameters...*/)
        //{
        //    return _totalCount_MasterDetail_ItemResource;
        //}
        ////required for paging.
        //public DataSet Get_MasterDetail_ItemResource(int startRowIndex, int maximumRows /*, ...custom parameters...,*/)
        //{
        //    int pageIndex = 0;

        //    if (startRowIndex > 0)
        //        pageIndex = startRowIndex / maximumRows;

        //    pageIndex += 1; //sproc is 1-indexed.

        //    //MasterDetail_Item_List.DataSource = model.MasterDetail_ItemResource.GetReader(
        //    //    null,
        //    //    null,
        //    //    sortExpr,
        //    //    sortDir);
        //    //return Moorings3.WebModules.SailingResume.Model.SailingResumeSubmission
        //    //    .GetPaged(moduleId, pageIndex, maximumRows, ref _totalSubmissionsCount);
        //    return null;
        //}
        #endregion
    }
}
