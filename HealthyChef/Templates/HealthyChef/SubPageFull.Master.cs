using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BayshoreSolutions.WebModules;
using System.Web.Security;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.Templates.HealthyChef
{
    public partial class SubPageFull : System.Web.UI.MasterPage
    {
        protected WebpageInfo _webpage = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            WebpageBase webpageBase = this.Page as WebpageBase;
            if (null != webpageBase) _webpage = webpageBase.WebpageInfo;
            if (_webpage != null) { Page.Title = webpageBase.Title; }
            //intercom 
            lblUserName.Text = "";
            lblUserEmail.Text = "";
            lblFullName.Text = "";
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