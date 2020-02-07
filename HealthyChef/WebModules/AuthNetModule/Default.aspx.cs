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
using System.Linq;

using BayshoreSolutions.WebModules;
using cms = BayshoreSolutions.WebModules.Cms;
using System.Collections.Generic;
using HealthyChef.Common;
using HealthyChef.AuthNet;
using HealthyChef.DAL;
using AuthorizeNet.APICore;

namespace HealthyChef.WebModules.AuthNetModule
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
            AuthNetConfig authNetConfig = new AuthNetConfig();

            ltlAuthNetName.Text = authNetConfig.Settings.Name;
            ltlAuthNetApiKey.Text = authNetConfig.Settings.ApiKey;
            ltlAuthNetTransactionKey.Text = authNetConfig.Settings.TransactionKey;
            ltlAuthNetMode.Text = authNetConfig.Settings.TestMode ? "Test Mode" : "Live Mode";

            try
            {
                CustomerInformationManager cim = new CustomerInformationManager();
                hccUserProfile profile = hccUserProfile.GetRootProfiles().First(a => a.AuthNetProfileID != null);

                validateCustomerPaymentProfileResponse valProfile =
                    cim.ValidateProfile(profile.AuthNetProfileID, profile.ActivePaymentProfile.AuthNetPaymentProfileID, AuthorizeNet.ValidationMode.TestMode);

                if (valProfile.messages.resultCode == messageTypeEnum.Ok)
                {
                    lblTest.Text = "Test Validation Successful.";
                }
                else
                {
                    lblTest.Text = "Test Validation Failed.";
                }
            }
            catch (Exception ex)
            {
                lblTest.Text = "Test Connection Failed." + ex.Message + ex.StackTrace;
            }
            //List<TestCard> cards = new List<TestCard>();

            //foreach(TestCard card in credentials.TestCards)
            //{
            //    cards.Add(card);
            //}

            //TestCards.DataSource = cards;
            //TestCards.DataBind();
        }
    }
}
