using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class MiniSummaryEdit : BayshoreSolutions.WebModules.Cms.ModuleAdminPage
    {
        public string ReturnTo
        {
            get { return (string)(ViewState["ReturnTo"] ?? null); }
            set { ViewState["ReturnTo"] = value; }
        }

        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        override protected void EnsureModule()
        {
            MasterDetailMiniSummarySetting module = MasterDetailMiniSummarySetting.FetchByID(this.ModuleId);
            if (null == module)
            {
                module = new MasterDetailMiniSummarySetting();
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
            Header.Controls.AddAt(1, cssLink);

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            MasterDetailMiniSummarySetting rec = MasterDetailMiniSummarySetting.FetchByID(this.ModuleId);
            if (rec != null)
            {
                PagePicker1.SelectedNavigationId = Convert.ToInt32(rec.StartingPageId);
                tbNumRows.Text = rec.NumRows.ToString();
                cbShowElapsedTime.Checked = Convert.ToBoolean(rec.ShowElapsedTime);
                cbShowFeaturedOnly.Checked = Convert.ToBoolean(rec.FeaturedOnly);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            MasterDetailMiniSummarySetting rec = new MasterDetailMiniSummarySetting(this.ModuleId);

            rec.ModuleId = this.ModuleId;
            rec.StartingPageId = PagePicker1.SelectedNavigationId;
            if (string.IsNullOrEmpty(tbNumRows.Text.Trim()))
            {
                rec.NumRows = 10;
            }
            else
            {
                rec.NumRows = Convert.ToInt32(tbNumRows.Text.Trim());
            }
            rec.ShowElapsedTime = cbShowElapsedTime.Checked;
            rec.FeaturedOnly = cbShowFeaturedOnly.Checked;
            rec.Save();

            LeavePage();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LeavePage();
        }

        void LeavePage()
        {
            if (null == this.ReturnTo)
                Cms.Admin.RedirectToMainMenu(this.PageNavigationId);
            else
                Response.Redirect(this.ReturnTo);
        }
    }
}
