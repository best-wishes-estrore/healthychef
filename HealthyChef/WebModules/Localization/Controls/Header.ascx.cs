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

using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.Cms.Localization.Controls
{
    public partial class Header : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string currentpath = Page.Request.FilePath;
                if (Bss.Web.Url.IsInBranch(currentpath, ResolveUrl(KeywordTokensLink.NavigateUrl)))
                    KeywordTokensLink.Font.Bold = true;
                if (Bss.Web.Url.IsInBranch(currentpath, ResolveUrl(CultureCodesTokensLink.NavigateUrl)))
                    CultureCodesTokensLink.Font.Bold = true;
            }
        }
    }
}