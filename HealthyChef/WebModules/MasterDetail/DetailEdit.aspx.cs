using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using WM = BayshoreSolutions.WebModules;
using CMS = BayshoreSolutions.WebModules.Cms;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class DetailEdit : BayshoreSolutions.WebModules.Cms.ModuleAdminPage
    {
        public string ReturnTo
        {
            get { return (string)(ViewState["ReturnTo"] ?? null); }
            set { ViewState["ReturnTo"] = value; }
        }

        ///// <summary>
        ///// Isolated=true indicates that the module should behave normally, that is, 
        ///// an initial module instance is automatically saved by EnsureModule().
        ///// Isolated=false indicates that this module is a child of a 
        ///// MasterDetail List module; in this case, the module will not be saved
        ///// until the user clicks the save button.
        ///// </summary>
        //public bool Isolated
        //{
        //    get { return (bool)(ViewState["Isolated"] ?? true); }
        //    set { ViewState["Isolated"] = value; }
        //}

        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        override protected void EnsureModule()
        {
            MasterDetailItem module = MasterDetailItem.GetResource(this.ModuleId);
            if (null == module)
            {
                module = new MasterDetailItem();
                module.ModuleId = this.ModuleId;
                module.Culture = WM.CultureCode.Current;
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
                this.ReturnTo = Request.QueryString["returnTo"];

                LoadModule();
            }
            Page.Form.DefaultButton = MasterDetail_SaveButton.UniqueID;
        }

        private void LoadModule()
        {
            DetailEditControl1.LoadContentItem(this.ModuleId);
        }

        protected void MasterDetail_SaveButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            DetailEditControl1.Save();

            LeavePage();
        }

        protected void MasterDetail_CancelButton_Click(object sender, EventArgs e)
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
