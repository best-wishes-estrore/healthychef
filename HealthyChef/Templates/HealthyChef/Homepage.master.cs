using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BayshoreSolutions.WebModules;
using BayshoreSolutions.WebModules.TemplateProperties;

using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.Templates.HealthyChef
{
    public partial class Homepage : System.Web.UI.MasterPage
    {
        //TemplateSetting example... 
        /*
        [TemplateSettingAttribute("Header image"), TemplateSettingOverrideAttribute("image")]
        public string HeaderImagePath
        {
            get { return Logo.ImageUrl ?? String.Empty; }
            set { Logo.ImageUrl = value; }
        }
        */

        protected WebpageInfo _webpage = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebpageBase webpageBase = this.Page as WebpageBase;
                if (null != webpageBase) _webpage = webpageBase.WebpageInfo;
                if (_webpage != null) { Page.Title = webpageBase.Title; }
                //TemplateSetting example... 
                //Header.Visible = !String.IsNullOrEmpty(HeaderImagePath);

                lblFullName.Text = "";
                lblUserName.Text = "";
                lblUserEmail.Text = "";

                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    if (user != null && Roles.IsUserInRole(user.UserName, "Customer"))
                    {
                        hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);
                        lblFullName.Text = parentProfile.FullName;
                        lblUserName.Text = parentProfile.FirstName;
                        lblUserEmail.Text = user.Email;
                    }
                }
            }
        }
    }
}
