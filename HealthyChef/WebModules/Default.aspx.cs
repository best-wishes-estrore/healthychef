using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;

namespace HealthyChef.WebModules
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators"))
                Response.Redirect("~/WebModules/ShoppingCart/", true);

            Response.Redirect("~/WebModules/Admin/MyWebsite/");
        }
    }
}