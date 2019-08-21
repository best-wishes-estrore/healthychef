using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BayshoreSolutions.WebModules.Templates.WebModules
{
    public partial class _Default : System.Web.UI.MasterPage
    {
        protected static readonly string _webmodulesVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/BayshoreSolutions.WebModules.dll")).FileVersion;
        protected static readonly string _projectVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(HttpContext.Current.Server.MapPath("~/bin/HealthyChef.dll")).FileVersion;

        protected bool IsAdmin { get; set; }
        protected bool IsEmployeeManager { get; set; }
        protected bool IsEmployeeProduction { get; set; }
        protected bool IsEmployeeService { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("X-UA-Compatible", "IE=9");
            if (!IsPostBack)
            {
                this.Page.Title = "WebModules: " + this.Page.Title;
                ChangePWLink.Visible = this.Page.User.Identity.IsAuthenticated;
                if(this.Page.User.Identity.IsAuthenticated)
                    LoginName1.Text = Membership.GetUser(this.Page.User.Identity.Name).Email;
            }

            MembershipUser user = HealthyChef.Common.Helpers.LoggedUser;
            IsAdmin = Roles.IsUserInRole(user.UserName, "Administrators");
            IsEmployeeManager = Roles.IsUserInRole(user.UserName, "EmployeeManager");
            IsEmployeeProduction= Roles.IsUserInRole(user.UserName, "EmployeeProduction");
            IsEmployeeService= Roles.IsUserInRole(user.UserName, "EmployeeService");

            system.Visible = IsAdmin;
            websites.Visible = IsAdmin;
            storemanagement.Visible = IsAdmin || IsEmployeeManager || IsEmployeeProduction || IsEmployeeService;
            productionmanagement.Visible = IsAdmin || IsEmployeeManager || IsEmployeeProduction;
            ordermanagement.Visible = IsAdmin || IsEmployeeManager || IsEmployeeProduction;
            listmanagement.Visible = IsAdmin || IsEmployeeManager;
            reports.Visible = IsAdmin || IsEmployeeManager || IsEmployeeProduction || IsEmployeeService;
            useraccounts.Visible = IsAdmin || IsEmployeeManager || IsEmployeeService;

            //reports
            chefproductionworksheet.Visible = IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            salesdetailreport.Visible = IsAdmin || IsEmployeeManager;
            meallabelreport.Visible = IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            customeraccountbalance.Visible = IsAdmin || IsEmployeeManager;
            customercalendar.Visible= IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            weeklymenureport.Visible= IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            weekatglancereport.Visible= IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            packingslip.Visible= IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            mealOrdertix.Visible= IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            mealcount.Visible= IsAdmin || IsEmployeeManager || IsEmployeeProduction|| IsEmployeeService;
            activecustomers.Visible = IsAdmin || IsEmployeeManager || IsEmployeeService;
        }         
    }
}
