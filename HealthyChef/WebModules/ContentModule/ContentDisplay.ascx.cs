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
using System.Threading;
using System.Globalization;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.ContentModule
{
    public partial class ContentDisplay : WebModuleBase//System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Content content = null;

                if (null != Session)
                    content = Session["BayshoreSolutions.WebModules.ContentModule.PreviewContent"] as Content;

                if (null != content
                    && content.ModuleId == this.WebModuleInfo.Id)
                {
                    Session.Remove("BayshoreSolutions.WebModules.ContentModule.PreviewContent");
                    ContentArea.Text = content.Text;
                }
                else
                {
                    content = Content.GetActiveContentForDisplay(this.WebModuleInfo.Id,
                        CultureCode.Current);
                    if (null != content)
                        ContentArea.Text = content.Text;
                }

            }
            catch (Exception err)
            {
                ContentArea.Text = err.ToString();
            }
        }
    }
}