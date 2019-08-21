using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using AuthorizeNet;
using HealthyChef.AuthNet;
using System.Web.Hosting;
using System.IO;
using AuthorizeNet.APICore;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class CreditCard_Edit : FormControlBase
    {   // this.PrimaryKeyIndex as PaymentProfile.PaymentProfileID
        public event CardInfoSaveFailedEventHandler CardInfoSaveFailed;
        public void OnCardInfoSaveFailed(CardInfoSaveFailedEventArgs e)
        {
            if (CardInfoSaveFailed != null)
                CardInfoSaveFailed(this, e);
        }

        public int CurrentUserProfileID
        {
            get
            {
                if (ViewState["CurrentUserProfileID"] == null)
                    ViewState["CurrentUserProfileID"] = 0;

                return int.Parse(ViewState["CurrentUserProfileID"].ToString());
            }
            set
            {
                ViewState["CurrentUserProfileID"] = value;
            }
        }

        public CardInfo CurrentCardInfo
        {
            get
            {
                if (ViewState["CurrentCardInfo"] == null)
                    ViewState["CurrentCardInfo"] = new CardInfo();

                return (CardInfo)ViewState["CurrentCardInfo"];
            }
            set
            {
                ViewState["CurrentCardInfo"] = value;
            }
        }

        /// <summary>
        /// Determines whether the Save button should be displayed. Default = false.
        /// </summary>
        public bool ShowSave
        {
            get
            {
                if (ViewState["ShowSave"] == null)
                    ViewState["ShowSave"] = false;

                return bool.Parse(ViewState["ShowSave"].ToString());
            }
            set
            {
                ViewState["ShowSave"] = value;
            }
        }

        /// <summary>
        /// Sets the text value of the Save button. Default = "Submit".
        /// </summary>
        public string SaveText
        {
            get
            {
                if (ViewState["SaveText"] == null)
                    ViewState["SaveText"] = "Submit";

                return ViewState["SaveText"].ToString();
            }
            set
            {
                ViewState["SaveText"] = value;
            }
        }
        
         /// <summary>
        /// Determines whether the fields in this control should be enabled. Default = true.
        /// </summary>
        public bool EnableFields
        {
            get
            {
                if (ViewState["EnableFields"] == null)
                    ViewState["EnableFields"] = true;

                return bool.Parse(ViewState["EnableFields"].ToString());
            }
            set
            {
                ViewState["EnableFields"] = value;
                SetEnabledFields(value);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);

            cvCardNumCCNumber.ServerValidate += new ServerValidateEventHandler(cvCardNumCCNumber_ServerValidate);
            cvCCAuthCode.ServerValidate += new ServerValidateEventHandler(cvCCAuthCode_ServerValidate);
            cvExpMonth.ServerValidate += new ServerValidateEventHandler(cvExpMonth_ServerValidate);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetButtons();
                BindddlExpYear();
            }
        }

        protected override void LoadForm()
        {
            BindddlExpYear();

            hccUserProfilePaymentProfile paymentProfile = hccUserProfilePaymentProfile.GetBy(CurrentUserProfileID);

            if (paymentProfile != null)
                this.PrimaryKeyIndex = paymentProfile.PaymentProfileID;

            if (paymentProfile != null)
            {
                CurrentCardInfo = paymentProfile.ToCardInfo();

                txtNameOnCard.Text = CurrentCardInfo.NameOnCard;
                txtCCNumber.Text = "************" + CurrentCardInfo.CardNumber;
                ddlExpMonth.SelectedIndex = ddlExpMonth.Items.IndexOf(ddlExpMonth.Items.FindByValue(CurrentCardInfo.ExpMonth.ToString()));
                ddlExpYear.SelectedIndex = ddlExpYear.Items.IndexOf(ddlExpYear.Items.FindByValue(CurrentCardInfo.ExpYear.ToString()));
            }
        }

        protected override void SaveForm()
        {
            hccUserProfile userProfile = hccUserProfile.GetById(CurrentUserProfileID);
            Address billAddr = null;

            if (userProfile != null)
            {
                //Save CardInfo
                if (pnlCardInfo.Visible)
                {
                    CurrentCardInfo.NameOnCard = txtNameOnCard.Text.Trim();
                    CurrentCardInfo.CardNumber = txtCCNumber.Text.Trim();
                    CurrentCardInfo.CardType = ValidateCardNumber(txtCCNumber.Text.Trim());
                    CurrentCardInfo.ExpMonth = int.Parse(ddlExpMonth.SelectedValue);
                    CurrentCardInfo.ExpYear = int.Parse(ddlExpYear.SelectedValue);
                    CurrentCardInfo.SecurityCode = txtCCAuthCode.Text.Trim();
                }

                if (userProfile.BillingAddressID.HasValue)
                {
                    billAddr = hccAddress.GetById(userProfile.BillingAddressID.Value).ToAuthNetAddress();
                }

                if (CurrentCardInfo.HasValues && billAddr != null)
                {
                    try
                    {
                        //send card to Auth.net for Auth.net profile
                        CustomerInformationManager cim = new CustomerInformationManager();
                        Customer cust = null;
                        string autnetResult = string.Empty;
                        
                        if (!string.IsNullOrWhiteSpace(userProfile.AuthNetProfileID))
                            cust = cim.GetCustomer(userProfile.AuthNetProfileID);

                        //Will Martinez - Commented out on 7/30/2013.
                        //This code scans all existing Profiles generated to check for duplicated email addresses, however the site registration prevents that
                        //commented out since this process had a significant performance effect on the site.
                        //if (cust == null)
                        //    cust = cim.GetCustomerByEmail(userProfile.ASPUser.Email);

                        if (cust == null)
                            cust = cim.CreateCustomer(userProfile.ASPUser.Email, userProfile.ASPUser.UserName);

                        // had to add it back in, unable to create records with duplicate email addresses caused by IT desynching data.
                        // this should only be called infrequently since we try to create the account first.
                        if (cust.ProfileID == null)
                            cust = cim.GetCustomerByEmail(userProfile.ASPUser.Email, out autnetResult);
                        if (cust != null)
                        {
                            if (userProfile.AuthNetProfileID != cust.ProfileID)
                            {
                                userProfile.AuthNetProfileID = cust.ProfileID;
                                userProfile.Save();
                            }

                            List<PaymentProfile> payProfiles = cust.PaymentProfiles.ToList();

                            if (payProfiles.Count > 0)
                                payProfiles.ForEach(a => cim.DeletePaymentProfile(userProfile.AuthNetProfileID, a.ProfileID));

                            // create new payment profile
                            autnetResult = cim.AddCreditCard(cust, CurrentCardInfo.CardNumber, CurrentCardInfo.ExpMonth,
                                 CurrentCardInfo.ExpYear, CurrentCardInfo.SecurityCode, billAddr);

                            if (!string.IsNullOrWhiteSpace(autnetResult))
                            {
                                // Validate card profile
                                validateCustomerPaymentProfileResponse valProfile = cim.ValidateProfile(userProfile.AuthNetProfileID,
                                    autnetResult, AuthorizeNet.ValidationMode.TestMode);

                                if (valProfile.messages.resultCode == messageTypeEnum.Ok)
                                {
                                    hccUserProfilePaymentProfile activePaymentProfile = null;
                                    activePaymentProfile = userProfile.ActivePaymentProfile;

                                    if (userProfile.ActivePaymentProfile == null)
                                        activePaymentProfile = new hccUserProfilePaymentProfile();

                                    activePaymentProfile.CardTypeID = (int)CurrentCardInfo.CardType;
                                    activePaymentProfile.CCLast4 = CurrentCardInfo.CardNumber.Substring(CurrentCardInfo.CardNumber.Length - 4, 4);
                                    activePaymentProfile.ExpMon = CurrentCardInfo.ExpMonth;
                                    activePaymentProfile.ExpYear = CurrentCardInfo.ExpYear;
                                    activePaymentProfile.NameOnCard = CurrentCardInfo.NameOnCard;
                                    activePaymentProfile.UserProfileID = userProfile.UserProfileID;
                                    activePaymentProfile.IsActive = true;

                                    activePaymentProfile.AuthNetPaymentProfileID = autnetResult;
                                    activePaymentProfile.Save();
                                    this.PrimaryKeyIndex = activePaymentProfile.PaymentProfileID;

                                    OnSaved(new ControlSavedEventArgs(this.PrimaryKeyIndex));
                                    lblFeedback.Text = "Payment Profile has been created and validated.";
                                }
                                else
                                {
                                    lblFeedback.Text = "Payment Profile has been created, but validation failed.";
                                }
                            }
                            else
                            {
                                lblFeedback.Text = "Authorize.Net response is empty.";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(autnetResult))
                                lblErrorOnAuth.Text = autnetResult;
                            OnCardInfoSaveFailed(new CardInfoSaveFailedEventArgs(new Exception(autnetResult)));
                        }
                    }
                    catch { throw; }
                }
            }
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            CurrentUserProfileID = 0;
            CurrentCardInfo = null;

            txtNameOnCard.Text = string.Empty;
            txtCCNumber.Text = string.Empty;
            ddlExpMonth.ClearSelection();
            ddlExpYear.ClearSelection();
            txtCCAuthCode.Text = string.Empty;
        }

        protected override void SetButtons()
        {
            if (ShowSave)
            {
                btnSave.Visible = ShowSave;
                btnSave.Text = SaveText;
            }
        }

        //void chkEditCreditCard_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (chkEditCreditCard.Checked)
        //    {
        //        Bind();
        //        CreditCardDisplay1.Visible = false;
        //        pnlCardInfo.Visible = true;
        //    }
        //    else
        //    {
        //        CreditCardDisplay1.Visible = true;
        //        pnlCardInfo.Visible = false;
        //    }
        //}

        protected void cvCardNumCCNumber_ServerValidate(object source, ServerValidateEventArgs args)
        {
            cvCardNumCCNumber.ErrorMessage = string.Empty;

            CurrentCardInfo.CardType = ValidateCardNumber(txtCCNumber.Text.Trim());
            if (CurrentCardInfo.CardType == Enums.CreditCardType.Unknown)
            {
                args.IsValid = false;
                cvCardNumCCNumber.ErrorMessage = "Enter a valid card number.";
            }

            hccUserProfile prof = hccUserProfile.GetById(CurrentUserProfileID);
            if (prof != null && !prof.BillingAddressID.HasValue)
            {
                args.IsValid = false;

                if (string.IsNullOrWhiteSpace(cvCardNumCCNumber.ErrorMessage))
                    cvCardNumCCNumber.ErrorMessage = "Card requires a billing address.";
                else
                    cvCardNumCCNumber.ErrorMessage += "Card requires a billing address.";
            }
        }

        protected void cvExpMonth_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                int iMonth = int.Parse(ddlExpMonth.SelectedValue);
                int iYear = int.Parse(ddlExpYear.SelectedValue);

                DateTime dtSelected = new DateTime(iYear, iMonth, 1).AddMonths(1);

                args.IsValid = dtSelected.CompareTo(DateTime.Now) > 0;

                if (!args.IsValid)
                    cvExpMonth.ErrorMessage = "Select a current or future month.";


            }
            catch
            {
                args.IsValid = false;
            }
        }

        protected void cvCCAuthCode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                int intCode = 0;
                int intCodeLen = 0;

                if (int.TryParse(txtCCAuthCode.Text.Trim(), out intCode))
                {
                    intCodeLen = txtCCAuthCode.Text.Trim().Length;

                    switch (CurrentCardInfo.CardType)
                    {
                        case Enums.CreditCardType.Visa:
                        case Enums.CreditCardType.MasterCard:
                        case Enums.CreditCardType.Discover:
                            args.IsValid = (intCodeLen == 3);
                            cvCCAuthCode.ErrorMessage = "Enter a valid 3-digit CVV2 Code";
                            break;
                        case Enums.CreditCardType.AmericanExpress:
                            args.IsValid = (intCodeLen == 4);
                            cvCCAuthCode.ErrorMessage = "Enter a valid 4-digit CVV2 Code";
                            break;
                    }
                }
            }
            catch
            {
                args.IsValid = false;
                return;
            }
        }

        private Enums.CreditCardType ValidateCardNumber(string sCardNumber)
        {
            string cardNum = sCardNumber.Replace(" ", "");

            Enums.CreditCardType retVal = Enums.CreditCardType.Unknown;

            //validate the type of card is accepted
            if (cardNum.StartsWith("4") == true &&
                (cardNum.Length == 13
                    || cardNum.Length == 16))
            {
                //VISA
                retVal = Enums.CreditCardType.Visa;
            }
            else if ((cardNum.StartsWith("51") == true ||
                      cardNum.StartsWith("52") == true ||
                      cardNum.StartsWith("53") == true ||
                      cardNum.StartsWith("54") == true ||
                      cardNum.StartsWith("55") == true) &&
                     cardNum.Length == 16)
            {
                //MasterCard
                retVal = Enums.CreditCardType.MasterCard;
            }
            else if ((cardNum.StartsWith("34") == true ||
                      cardNum.StartsWith("37") == true) &&
                     cardNum.Length == 15)
            {
                //Amex
                retVal = Enums.CreditCardType.AmericanExpress;
            }
            //else if ((cardNum.StartsWith("300") == true ||
            //          cardNum.StartsWith("301") == true ||
            //          cardNum.StartsWith("302") == true ||
            //          cardNum.StartsWith("304") == true ||
            //          cardNum.StartsWith("305") == true ||
            //          cardNum.StartsWith("36") == true ||
            //          cardNum.StartsWith("38") == true) &&
            //         cardNum.Length == 14)
            //{
            //    //Diners Club/Carte Blanche
            //    retVal = Enums.CreditCardType.DinersClub;
            //}
            else if (cardNum.StartsWith("6011") == true &&
                     cardNum.Length == 16)
            {
                //Discover
                retVal = Enums.CreditCardType.Discover;
            }

            if (retVal != Enums.CreditCardType.Unknown)
            {
                int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
                int checksum = 0;
                char[] chars = cardNum.ToCharArray();
                for (int i = chars.Length - 1; i > -1; i--)
                {
                    int j = ((int)chars[i]) - 48;
                    checksum += j;
                    if (((i - chars.Length) % 2) == 0)
                        checksum += DELTAS[j];
                }

                if ((checksum % 10) != 0)
                    retVal = Enums.CreditCardType.Unknown;
            }

            return retVal;
        }

        void BindddlExpYear()
        {
            if (ddlExpYear.Items.Count == 0)
            {
                ddlExpYear.Items.Clear();

                int currYear = DateTime.Now.Year;
                for (int i = 0; i <= 10; i++)
                {
                    ddlExpYear.Items.Add((currYear + i).ToString());
                }

                ddlExpYear.Items.Insert(0, new ListItem("---", "-1"));
            }
        }

        //void BindddlCardTypes()
        //{
        //    if (ddlCardTypes.Items.Count == 0)
        //    {
        //        ddlCardTypes.DataSource = Enums.GetEnumAsTupleList(typeof(Enums.CreditCardType));
        //        ddlCardTypes.DataTextField = "Item1";
        //        ddlCardTypes.DataValueField = "Item2";
        //        ddlCardTypes.DataBind();

        //        ddlCardTypes.Items.Remove(ddlCardTypes.Items[0]); // Unknown 
        //    }
        //}

        void CreditCardEdit1_CardInfoSaveFailed(object sender, CardInfoSaveFailedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.FailException.Message))
                return;

            cstValCardInfo0.Enabled = true;
            cstValCardInfo0.ErrorMessage = e.FailException.Message;
            cstValCardInfo0.Validate();
            Page.Validate();
        }

        void SetEnabledFields(bool enabled)
        {
            txtNameOnCard.Enabled = enabled;
            rfvNameOnCard.Enabled = enabled;
            txtCCNumber.Enabled = enabled;
            rfvCCNumber.Enabled = enabled;
            cvCardNumCCNumber.Enabled = enabled;
            ddlExpMonth.Enabled = enabled;
            ddlExpYear.Enabled = enabled;
            rfvExpYear.Enabled = enabled;
            rfvExpMonth.Enabled = enabled;
            cvExpMonth.Enabled = enabled;
            txtCCAuthCode.Enabled = enabled;
            rfvCCAuthCode.Enabled = enabled;
            cvCCAuthCode.Enabled = enabled;
        }
    }
}