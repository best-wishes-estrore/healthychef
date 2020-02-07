using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace HealthyChef
{
    public partial class ErrorPage : BayshoreSolutions.WebModules.WebpageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString != null && Request.QueryString["errorCode"] != null)
                {
                    string errorCode = string.Empty;

                    if (!string.IsNullOrWhiteSpace(Request.QueryString["errorCode"]))
                        errorCode = Request.QueryString["errorCode"].ToString().Trim();

                    switch (errorCode)
                    {
                        case "404":
                            lblOutput.Text = "<h1>404 - Not Found</h1><br />The requested page was not found.";
                            Page.Response.StatusCode = 404;
                            break;
                        case "500":
                            lblOutput.Text = "<h1>500 - Error</h1><br />A system error has occurred.";
                            Page.Response.StatusCode = 500;
                            break;
                        default:
                            lblOutput.Text = "<h1>" + errorCode + " - Error</h1><br />An unknown system error has occurred.";
                            break;
                    }
                }
            }
        }
    }
}
