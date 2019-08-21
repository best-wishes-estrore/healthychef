using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.QuickContent
{
    public partial class PopupEditor : Page
    {
        private const int MAX_CONTENT_HISTORY_RECS = 10;
        protected string ContentName = "";
        protected string ContentText = "";

        //This is the secret sauce to get the uploader to work properly
        protected void Page_Init(object sender, EventArgs e)
        {
            //txtContent.SkinPath = "skins/office2003/";
            //Session["FCKeditor:UserFilesPath"] = Page.ResolveUrl("~/CMSFiles");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            ContentName = SubSonic.Utilities.Utility.GetParameter("id");

            if (!Page.IsPostBack)
            {
                //load the locale list
                InitCultureDropDown();

                if (ContentName != string.Empty)
                {
                    SetEditableContent(true);
                }

            }

        }
        private void InitCultureDropDown()
        {
            List<CultureCode> activeCultures = CultureCode.Find(null, null, true, null);
            //remove aliased cultures
            activeCultures = activeCultures.FindAll(
                delegate(CultureCode c) { return string.IsNullOrEmpty(c.AliasToCultureCode); });

            if (null != activeCultures && activeCultures.Count > 0)
            {
                ddlCulture.Items.Clear();
                foreach (CultureCode c in activeCultures)
                {
                    CultureInfo ci = CultureInfo.CreateSpecificCulture(c.Name);
                    ListItem li = new ListItem(ci.NativeName, c.Name);
                    li.Selected = string.Equals(CultureCode.Current, c.Name, StringComparison.OrdinalIgnoreCase);
                    ddlCulture.Items.Add(li);
                }
            }
        }
        private void BindVersionDropDown(QuickContentContentCollection coll)
        {
            // Load the version dropdown
            ddlVersion.DataValueField = QuickContentContent.Columns.Id;
            ddlVersion.DataTextField = QuickContentContent.Columns.CreatedOn;

            ddlVersion.DataSource = coll;
            ddlVersion.DataBind();
        }

        private void SetEditableContent(bool cultureChange)
        {
            lblMessage.Text = string.Empty;
            txtContent.Value = String.Empty;
            QuickContentContentCollection cmsColl;

            if (cultureChange)
            {
                cmsColl = new QuickContentContentCollection()
                    .Where(QuickContentContent.Columns.ContentName, ContentName)
                    .Where(QuickContentContent.Columns.Culture, ddlCulture.SelectedValue)
                    .OrderByDesc(QuickContentContent.Columns.CreatedOn)
                    .Load();

                // Load the version history dropdown
                BindVersionDropDown(cmsColl);
            }
            else
            {
                // Load a specific record from the version history
                cmsColl = new QuickContentContentCollection()
                    .Where(QuickContentContent.Columns.Id, Convert.ToInt32(ddlVersion.SelectedValue))
                    .Load();
            }

            if (cmsColl.Count > 0)
            {
                QuickContentContent versionContent = cmsColl[0];
                txtContent.Value = versionContent.Body;
            }

            //EditorModalPopup.Show();
        }
        protected void ddlVersion_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEditableContent(false);
        }

        protected void ddlCulture_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetEditableContent(true);
        }

        protected void btnSave_Click(object sender, System.EventArgs e)
        {
            lblMessage.Text = string.Empty;
            // If a previous version of the content exists, mark it as old.
            // Also, maintain a max of 10 recs in the version history
            QuickContentContentCollection coll = new QuickContentContentCollection()
                .Where(QuickContentContent.Columns.ContentName, ContentName)
                .Where(QuickContentContent.Columns.Culture, ddlCulture.SelectedValue)
                .OrderByDesc(QuickContentContent.Columns.CreatedOn)
                .Load();
            int i = 1;
            foreach (QuickContentContent rec in coll)
            {
                if (i >= MAX_CONTENT_HISTORY_RECS)
                {
                    QuickContentContent.Delete(rec.Id);
                }
                else if (rec.StatusId == 2)
                {
                    rec.StatusId = 3; // 3 = Past Content
                    rec.Save();
                }
                i++;
            }

            // Insert the latest content
            QuickContentContent newContent = new QuickContentContent(true);
            newContent.IsNew = true;
            newContent.StatusId = 2;	// 2 = Active Content
            newContent.Culture = ddlCulture.SelectedValue;
            newContent.ContentName = ContentName;
            newContent.Body = txtContent.Value;
            newContent.Save(Page.User.Identity.Name);

            // Reload version dropdown
            SetEditableContent(true);

            lblMessage.Text = "<br>Your changes have been saved.";
        }

    }
}
