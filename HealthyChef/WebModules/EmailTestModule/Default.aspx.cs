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
using HealthyChef.DAL;
using HealthyChef.Email;
using System.Text;

namespace HealthyChef.WebModules.EmailTestModule
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

        }

        protected void btnEmailTest_Click(object sender, EventArgs e)
        {
            try
            {
                EmailController ec = new EmailController();
                if (ec.SendMail_ToAdmin("Email Test", "This is a test of the HCC email system."))
                    lblEmailTest.Text = "Test Successful";
                else
                    lblEmailTest.Text = "Test Failed";
            }
            catch (Exception ex)
            {
                lblEmailTest.Text = ex.Message + ex.StackTrace;
            }
        }

        //protected void btnUserImprot_Click(object sender, EventArgs e)
        //{
        //    //int count = 0;
        //    //List<string> errors = DAL.ImportedCustomer.Import(out count);
        //    //StringBuilder sb = new StringBuilder();
        //    //foreach(string error in errors)
        //    //{
        //    //    sb.Append("<li>" + error + "</li>");
        //    //}
        //    //lblImportResults.Text = "Count:" + count + "<ul>" + sb.ToString() + "</ul>";
        //}

        protected void btnDelDupes_Click(object sender, EventArgs e)
        {
            try
            {
                hccProgramDefaultMenu.DeleteDuplicates();

                lblFeedbackDelDupes.Text = "Duplicates Deleted.";
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
