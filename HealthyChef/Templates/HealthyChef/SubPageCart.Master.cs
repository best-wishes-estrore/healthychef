using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using BayshoreSolutions.WebModules;
using HealthyChef.Common;
using System.Web.Security;
using HealthyChef.DAL;

namespace HealthyChef.Templates.HealthyChef
{
    public partial class SubPageCart : System.Web.UI.MasterPage
    {
        protected WebpageInfo _webpage = null;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            WebpageBase webpageBase = this.Page as WebpageBase;
            if (null != webpageBase) _webpage = webpageBase.WebpageInfo;
            if (_webpage != null) { Page.Title = webpageBase.Title; }

            Response.ClearHeaders();
            Response.AppendHeader("Cache-Control", "no-cache"); //HTTP 1.1
            Response.AppendHeader("Cache-Control", "private"); // HTTP 1.1
            Response.AppendHeader("Cache-Control", "no-store"); // HTTP 1.1
            Response.AppendHeader("Cache-Control", "must-revalidate"); // HTTP 1.1
            Response.AppendHeader("Cache-Control", "max-stale=0"); // HTTP 1.1 
            Response.AppendHeader("Cache-Control", "post-check=0"); // HTTP 1.1 
            Response.AppendHeader("Cache-Control", "pre-check=0"); // HTTP 1.1 
            Response.AppendHeader("Pragma", "no-cache"); // HTTP 1.0 
            Response.AppendHeader("Expires", "Mon, 26 Jul 1997 05:00:00 GMT"); // HTTP 1.0
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