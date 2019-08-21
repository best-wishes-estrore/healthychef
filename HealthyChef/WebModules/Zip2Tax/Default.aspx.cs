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

using BayshoreSolutions.WebModules;
using cms = BayshoreSolutions.WebModules.Cms;
using System.Collections.Generic;
using HealthyChef.Common;
using ZipToTaxService;

namespace HealthyChef.WebModules.Zip2Tax
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MainMenuLink.HRef = cms.Admin.MainMenuUrl;
            }

            LoadModule();
        }

        private void LoadModule()
        {
            ZipToTaxConfig authNetConfig = new ZipToTaxConfig();

            ltlServer.Text = authNetConfig.Settings.Server;
            ltlDBName.Text = authNetConfig.Settings.DBName;
            ltlDBUsername.Text = authNetConfig.Settings.DBUsername;
            ltlDBPassword.Text = authNetConfig.Settings.DBPassword;        
        }

        protected void btnZipTest_Click(object nseder, EventArgs e)
        {
            try
            {
                ZipToTaxService.TaxLookup z2t = ZipToTaxService.TaxLookup.RequestTax(txtZipTest.Text.Trim());
                ltlZipTest.Text = z2t.ToString();
            }
            catch (Exception ex)
            {
                ltlZipTest.Text = ex.Message;
            }
        }
    }
}
