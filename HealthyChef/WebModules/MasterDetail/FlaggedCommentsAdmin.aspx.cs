using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SubSonic;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class FlaggedCommentsAdmin : BayshoreSolutions.WebModules.Cms.ModuleAdminPage
    {
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
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/MasterDetail/public/css/MasterDetail.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            Header.Controls.Add(cssLink);

            if (!IsPostBack)
            {
                BindData();
            }

        }
        protected void BindData()
        {
            // Filter by x number of flags
            int count;
            Int32.TryParse(tbMinFlags.Text, out count);
            StringBuilder sb = new StringBuilder();
            List<WebpageInfo> subpages = BayshoreSolutions.WebModules.Webpage.GetWebpages(this.PageNavigationId);
            foreach (WebpageInfo list in subpages)
            {
                foreach (WebModuleInfo module in list.Modules)
                {
                    if (sb.Length == 0)
                    {
                        sb.Append(module.Id);
                    }
                    else
                    {
                        sb.Append("," + module.Id);
                    }

                    // Maximum length of idList for stored proc is 8000
                    if (sb.Length > 7990) break;
                }
            }

            CommentsListRepeater.DataSource = SPs.MasterDetailGetCommentFlags(sb.ToString(), count, Convert.ToInt32(rblSortOrder.SelectedValue)).GetDataSet();
            CommentsListRepeater.DataBind();

        }
        protected void CommentsListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hidCommentRecId = e.Item.FindControl("hidCommentRecId") as HiddenField;
                if (hidCommentRecId != null) hidCommentRecId.Value = DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.Id).ToString();

                Label lblName = e.Item.FindControl("lblName") as Label;
                if (lblName != null) lblName.Text = DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.Username).ToString();

                Label lblTimestamp = e.Item.FindControl("lblTimestamp") as Label;
                if (lblTimestamp != null) lblTimestamp.Text = "@ " + Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.CreatedOn)).ToString("dddd, MMMM dd yyyy h:mm tt");

                Label lblText = e.Item.FindControl("lblText") as Label;
                if (lblText != null) lblText.Text = DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.CommentText).ToString();

                Label lblFlags = e.Item.FindControl("lblFlags") as Label;
                if (lblFlags != null) lblFlags.Text = "Flagged " + DataBinder.Eval(e.Item.DataItem, "FlagCount") + " times";

            }

        }

        protected void CommentsListRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Delete":
                    int recId = Convert.ToInt32(e.CommandArgument);
                    // Delete the comment record.  Cascase delete will remove flag records as well.
                    MasterDetailComment.Delete(recId);
                    BindData();
                    break;
                case "Accept":
                    int recId2 = Convert.ToInt32(e.CommandArgument);
                    // Accept the comment, and clear all flag records.
                    MasterDetailFlagComment.Delete(MasterDetailFlagComment.Columns.CommentId, recId2);
                    BindData();
                    break;
                default:
                    break;
            }
        }

        protected void btnUpdateMinFlags_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnDeleteAll_Click(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in CommentsListRepeater.Items)
            {
                HiddenField hidCommentRecId = item.FindControl("hidCommentRecId") as HiddenField;
                if (hidCommentRecId != null) MasterDetailComment.Delete(Convert.ToInt64(hidCommentRecId.Value));
                BindData();

            }
        }

    }
}
