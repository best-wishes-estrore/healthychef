using System;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SubSonic;

/**
 * This page employs two main spam-bot filtering techniques (because CAPTCHA is annoying to users)
 * 
 * 1) GET request detection - We create a session variable when a GET request is made, and 
 * then when the form is submitted, we check for that session variable.  This filters out bots
 * that submit requests directly to POST without getting a page form.
 * 
 * 2) Hidden form element - Place input fields in the form with common names that bots look
 * to fill in ("url", "email").  Hide these fields from the user with css by wrapping them in
 * a <p> tag.  The fields must be hidden using css because the bots will avoid input fields
 * of type=hidden.  When the user submits the form, if there is any data in the hidden fields,
 * we will ignore the request because that data will have come from a bot.
 * 
 * 2a) Rename real email input field to tbNoSpam so that bots are less likely to identify it
 * as an email field.
 * 
 */

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class DetailDisplay : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected int CurrentModuleId
        {
            get { return Convert.ToInt32(ViewState["currentModuleId"]); }
            set { ViewState["currentModuleId"] = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/MasterDetail/public/css/MasterDetail.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            this.Page.Header.Controls.AddAt(1, cssLink);

            // AntiSpam session var
            Session.Add("AntiSpamVar", "0");

            if (!IsPostBack)
            {
                WebpageInfo page = this.WebModuleInfo.Webpage;
                MasterDetailItem item = MasterDetailItem.GetSafeResource(this.ModuleId);
                MasterDetailSetting itemList = item.GetContentList();

                CurrentModuleId = item.ModuleId;

                if (itemList != null)
                {
                    lblPostDate.Visible = itemList.IsPostDateVisible;
                    CommentsPanel.Visible = itemList.AllowComments;

                    if (itemList.AllowComments)
                    {
                        Query q = MasterDetailComment.Query().WHERE(MasterDetailComment.Columns.ModuleId, CurrentModuleId);
                        litCommentsLink.Text = "&nbsp;&nbsp;|&nbsp;&nbsp;<a href='#Comments'>" + q.GetCount(MasterDetailComment.Columns.Id) + " Comment(s)</a>";
                    }
                    if (Page.User.Identity.IsAuthenticated || !itemList.RequireAuthentication)
                    {
                        submitCommentsDiv.Visible = true;
                        lblLoginMsg.Visible = false;
                    }
                    else
                    {
                        submitCommentsDiv.Visible = false;
                        lblLoginMsg.Visible = true;
                    }
                }

                if (lblPostDate.Visible)
                {
                    if (page.PostDate.HasValue)
                        lblPostDate.Text = page.PostDate.Value.ToLongDateString();
                }

                if (string.IsNullOrEmpty(item.ImagePath))
                {
                    img.Visible = false;
                }
                else
                {
                    img.ImageUrl = ResolveUrl(item.ImagePath);
                }

                litTitle.Text = page.Text;
                litBody.Text = item.LongDescription;
                hypBack.NavigateUrl = this.WebModuleInfo.Webpage.Parent.Path;

                CommentsListRepeater.DataSource = new MasterDetailCommentCollection()
                    .Where(MasterDetailComment.Columns.ModuleId, CurrentModuleId)
                    .OrderByDesc(MasterDetailComment.Columns.CreatedOn)
                    .Load();
                CommentsListRepeater.DataBind();
            }
        }

        protected void btnCommentSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // Check the spam-bot fields for data
            string s1 = Request.Form.Get("url");
            string s2 = Request.Form.Get("email");
            string s3 = Request.Form.Get("comment");
            object o4 = Session["AntiSpamVar"];
            if (!string.IsNullOrEmpty(Request.Form.Get("url"))
                || !string.IsNullOrEmpty(Request.Form.Get("email"))
                || !string.IsNullOrEmpty(Request.Form.Get("comment"))
                || (Session["AntiSpamVar"] == null))
            {
                Response.End();
                return;
            }

            // Save record
            MasterDetailComment rec = new MasterDetailComment();
            rec.ModuleId = CurrentModuleId;
            rec.Username = tbCommentName.Text.Trim();
            rec.Email = tbCommentEmail.Text.Trim();
            rec.IPAddress = Request.ServerVariables["REMOTE_ADDR"];
            rec.CommentText = tbCommentText.Text.Trim();
            rec.CreatedBy = Page.User.Identity.IsAuthenticated ? Page.User.Identity.Name : "Anonymous";
            rec.Save();

            Response.Redirect(this.Request.Url.ToString());

        }

        protected void CommentsListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lblName = e.Item.FindControl("lblName") as Label;
                if (lblName != null) lblName.Text = DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.Username).ToString();

                LinkButton btnFlag = e.Item.FindControl("btnFlag") as LinkButton;
                if (btnFlag != null) btnFlag.CommandArgument = DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.Id).ToString();

                Label lblTimestamp = e.Item.FindControl("lblTimestamp") as Label;
                if (lblTimestamp != null) lblTimestamp.Text = "@ " + Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.CreatedOn)).ToString("dddd, MMMM dd yyyy h:mm tt");

                Label lblText = e.Item.FindControl("lblText") as Label;
                if (lblText != null) lblText.Text = DataBinder.Eval(e.Item.DataItem, MasterDetailComment.Columns.CommentText).ToString();

            }

        }

        protected void CommentsListRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Flag":

                    // Flag a comment as inappropriate
                    long commentRecId = Convert.ToInt64(e.CommandArgument);

                    // Check to see if this user has already flagged this comment
                    if (Page.User.Identity.IsAuthenticated)
                    {
                        // If user is logged in, check the user account id
                        MasterDetailFlagCommentCollection collUserAcct = new MasterDetailFlagCommentCollection()
                            .Where(MasterDetailFlagComment.Columns.CommentId, commentRecId)
                            .Where(MasterDetailFlagComment.Columns.CreatedBy, Page.User.Identity.Name)
                            .Load();
                        if (collUserAcct.Count > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showAlertMessage",
                                    "alert('Comment has already been flagged, and will be reviewed shortly.');", true);
                            return;
                        }
                    }

                    // Check to see if this comment has been previously flagged from the same IP
                    MasterDetailFlagCommentCollection collIP = new MasterDetailFlagCommentCollection()
                        .Where(MasterDetailFlagComment.Columns.CommentId, commentRecId)
                        .Where(MasterDetailFlagComment.Columns.IPAddress, Request.ServerVariables["REMOTE_ADDR"])
                        .Load();
                    if (collIP.Count > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showAlertMessage2",
                                "alert('Comment has already been flagged, and will be reviewed shortly.');", true);
                        return;
                    }

                    MasterDetailFlagComment flagRec = new MasterDetailFlagComment();
                    flagRec.CommentId = commentRecId;
                    flagRec.IPAddress = Request.ServerVariables["REMOTE_ADDR"];
                    flagRec.CreatedBy = Page.User.Identity.IsAuthenticated ? Page.User.Identity.Name : "Anonymous";
                    flagRec.Save();

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "showAlertMessage3",
                            "alert('This comment is now flagged to be reviewed by a website administrator.');", true);
                    break;
                default:
                    break;
            }
        }


    }
}