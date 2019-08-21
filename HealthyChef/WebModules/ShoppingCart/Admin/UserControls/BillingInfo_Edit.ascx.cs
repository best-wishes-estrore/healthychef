using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HealthyChef.Common;
using HealthyChef.DAL;
using AuthorizeNet;
using HealthyChef.AuthNet;
using System.Web.Security;
using AuthorizeNet.APICore;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class BillingInfo_Edit : FormControlBase
    {// Note: this.PrimaryKeyIndex as hccUserProfiles.UserProfileId

        public int? CurrentBillingAddressID
        {
            get
            {
                if (ViewState["CurrentBillingAddressID"] == null)
                    ViewState["CurrentBillingAddressID"] = 0;

                return int.Parse(ViewState["CurrentBillingAddressID"].ToString());
            }
            set
            {
                ViewState["CurrentBillingAddressID"] = value;
            }
        }

        /// <summary>
        /// Determines whether the Save button should be displayed. Default = false.
        /// </summary>
        public bool ShowAddressSave
        {
            get
            {
                if (ViewState["ShowAddressSave"] == null)
                    ViewState["ShowAddressSave"] = false;

                return bool.Parse(ViewState["ShowAddressSave"].ToString());
            }
            set
            {
                ViewState["ShowAddressSave"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            AddressEdit_Billing1.SetFormFieldValidationGroup(this.ValidationGroup);
            CreditCardEdit1.SetFormFieldValidationGroup(this.ValidationGroup);

            base.OnInit(e);

            chkUpdateCard.CheckedChanged += chkUpdateCard_CheckedChanged;
            btnSave.Click += base.SubmitButtonClick;
            AddressEdit_Billing1.ControlSaved += AddressEdit_Billing_ControlSaved;
        }

        void chkUpdateCard_CheckedChanged(object sender, EventArgs e)
        {
            CreditCardEdit1.EnableFields = chkUpdateCard.Checked;
        }

        void AddressEdit_Billing_ControlSaved(object sender, Common.Events.ControlSavedEventArgs e)
        {
            try
            {
                hccUserProfile userProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);
                userProfile.BillingAddressID = int.Parse(e.PrimaryKeyIndex.ToString());
                userProfile.Save();
                chkUpdateCard.Enabled = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected string PadCardMonth(int mon)
        {
            string strMon = mon.ToString();
            while (strMon.Length < 2)
            {
                strMon = "0" + strMon;
            }

            return strMon;
        }

        protected string PadCardLast4(Enums.CreditCardType cardType, string last4)
        {
            string retVal = string.Empty;
            switch (cardType)
            {
                case Enums.CreditCardType.AmericanExpress:
                case Enums.CreditCardType.Discover:
                case Enums.CreditCardType.MasterCard:
                case Enums.CreditCardType.Visa:
                    retVal = "XXXXXXXXXXXX" + last4;
                    break;
                case Enums.CreditCardType.Unknown:
                default:
                    break;
            }

            return retVal;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetButtons();
            }
        }

        protected override void LoadForm()
        {
            hccUserProfile userProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            //load address
            CreditCardEdit1.CurrentUserProfileID = this.PrimaryKeyIndex;

            if (userProfile != null && userProfile.BillingAddressID.HasValue && userProfile.BillingAddressID.Value > 0)
            {
                AddressEdit_Billing1.PrimaryKeyIndex = userProfile.BillingAddressID.Value;
                AddressEdit_Billing1.Bind();
                chkUpdateCard.Enabled = true;
            }

            // load CardInfo
            CreditCardEdit1.CurrentUserProfileID = this.PrimaryKeyIndex;
            CreditCardEdit1.Bind();
        }

        protected override void SaveForm()
        {
            hccUserProfile userProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            if (userProfile != null)
            {
                // Save Address
                AddressEdit_Billing1.Save();

                if (chkUpdateCard.Checked)
                    CreditCardEdit1.Save();
            }

        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            CurrentBillingAddressID = 0;
            AddressEdit_Billing1.Clear();
        }

        protected override void SetButtons()
        {
            AddressEdit_Billing1.ValidationMessagePrefix = this.ValidationMessagePrefix;
            CreditCardEdit1.ValidationMessagePrefix = this.ValidationMessagePrefix;
        }
    }
}