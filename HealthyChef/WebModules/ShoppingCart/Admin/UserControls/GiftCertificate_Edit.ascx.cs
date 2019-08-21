using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class GiftCertificate_Edit : FormControlBase
    {   // primaryKey as hccCartId
        public bool ShowSentToRecipCheckbox
        {
            get
            {
                if (ViewState["ShowSentToRecipCheckbox"] == null)
                    ViewState["ShowSentToRecipCheckbox"] = false;

                return bool.Parse(ViewState["ShowSentToRecipCheckbox"].ToString());
            }
            set
            {
                ViewState["ShowSentToRecipCheckbox"] = value;
            }
        }

        hccCartItem CurrentGiftCert
        {
            get
            {
                if (ViewState["CurrentGiftCert"] == null)
                    ViewState["CurrentGiftCert"] = hccCartItem.Gift_GenerateNew(this.PrimaryKeyIndex);

                return (hccCartItem)ViewState["CurrentGiftCert"];
            }
            set
            {
                ViewState["CurrentGiftCert"] = value;
            }
        }

        public int CurrentCartItemID
        {
            get
            {
                if (ViewState["CurrentCartItemID"] == null)
                    return 0;
                else
                    return int.Parse(ViewState["CurrentCartItemID"].ToString());
            }
            set { ViewState["CurrentCartItemID"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            this.Page.MaintainScrollPositionOnPostBack = true;
            AddressRecipient.SetFormFieldValidationGroup(this.ValidationGroup);

            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
        }

        protected override void LoadForm()
        {
            if (CurrentCartItemID > 0)
                CurrentGiftCert = hccCartItem.GetById(CurrentCartItemID);           

            if (CurrentGiftCert != null)
            {
                this.PrimaryKeyIndex = CurrentGiftCert.CartID;
                lblRedeemCode.Text = CurrentGiftCert.Gift_RedeemCode.ToString();
                txtAmount.Text = CurrentGiftCert.ItemPrice.ToString("f2");

                if (CurrentGiftCert.Gift_IssuedTo != null)
                {
                    MembershipUser issuedUser = Membership.GetUser(CurrentGiftCert.Gift_IssuedTo);
                    lblCreatedInfo.Text = string.Format("Issued to {0} on {1}.", issuedUser.Email, CurrentGiftCert.Gift_IssuedDate);
                }

                txtRecipEmail.Text = CurrentGiftCert.Gift_RecipientEmail;

                if (CurrentGiftCert.Gift_RecipientAddressId.HasValue)
                {
                    AddressRecipient.PrimaryKeyIndex = CurrentGiftCert.Gift_RecipientAddressId.Value;
                    AddressRecipient.Bind();
                }

                txtRecipMessage.Text = CurrentGiftCert.Gift_RecipientMessage;
                CartId.Value = Convert.ToString(CurrentGiftCert.CartID);
                chkSentToRecip.Visible = ShowSentToRecipCheckbox;

                if (ShowSentToRecipCheckbox && CurrentGiftCert.IsCompleted)
                {
                    chkSentToRecip.Checked = CurrentGiftCert.IsCompleted;

                    if (chkSentToRecip.Checked)
                        chkSentToRecip.Enabled = false;
                }

                if (CurrentGiftCert.Gift_RedeemedBy.HasValue && CurrentGiftCert.Gift_RedeemedDate.HasValue)
                {
                    MembershipUser redeemedUser = Membership.GetUser(CurrentGiftCert.Gift_RedeemedBy.Value);

                    if (redeemedUser != null)
                        lblRedeemedInfo.Text = string.Format("Redeemed: Yes, by <a href='UserProfile.aspx?{0}'>{0}</a> on {1}.",
                            redeemedUser.Email, CurrentGiftCert.Gift_RedeemedDate);

                    pnlGiftCertificateEdit.Enabled = false;
                    btnSave.Enabled = false;
                }
                else
                {
                    lblRedeemedInfo.Text = "Redeemed: No";
                    pnlGiftCertificateEdit.Enabled = true;
                    btnSave.Enabled = true;
                }
            }
        }

        protected override void SaveForm()
        {
            try
            {
                //if (CurrentCartItemID > 0)
                //    CurrentGiftCert = hccCartItem.GetById(CurrentCartItemID);
                //else
                //    CurrentGiftCert = hccCartItem.Gift_GenerateNew(this.PrimaryKeyIndex);

                if (CurrentGiftCert != null)
                {
                    hccCart userCart = hccCart.GetById(this.PrimaryKeyIndex);

                    if (userCart != null)
                    {
                        CurrentGiftCert.ItemPrice = decimal.Parse(txtAmount.Text.Trim());

                        //recipient info
                        CurrentGiftCert.Gift_RecipientEmail = txtRecipEmail.Text.Trim();
                        CurrentGiftCert.Gift_RecipientMessage = txtRecipMessage.Text.Trim();
                        CurrentGiftCert.DeliveryDate = hccProductionCalendar.GetNext4Calendars().First().DeliveryDate;

                        CurrentGiftCert.GetOrderNumber(userCart);

                        AddressRecipient.AddressType = Enums.AddressType.GiftRecipient;
                        AddressRecipient.Save();
                        CurrentGiftCert.Gift_RecipientAddressId = AddressRecipient.PrimaryKeyIndex;

                        string itemFullName = string.Format("{0} - {1} - {2} - For: {3} {4}",
                            "Gift Certificate", CurrentGiftCert.Gift_RedeemCode, CurrentGiftCert.ItemPrice.ToString("c"),
                            AddressRecipient.CurrentAddress.FirstName, AddressRecipient.CurrentAddress.LastName);

                        CurrentGiftCert.ItemName = itemFullName;

                        if (ShowSentToRecipCheckbox)
                            CurrentGiftCert.IsCompleted = chkSentToRecip.Checked;

                        CurrentGiftCert.Save();

                        this.OnSaved(new ControlSavedEventArgs(CurrentGiftCert.CartItemID));
                    }                   

                }
            }
            catch
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            CurrentCartItemID = 0;
            CurrentGiftCert = null;
            lblRedeemCode.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtRecipEmail.Text = string.Empty;
            lblCreatedInfo.Text = string.Empty;
            lblRedeemedInfo.Text = string.Empty;
            AddressRecipient.Clear();

            SetButtons();
        }

        protected override void SetButtons()
        {
            // do nothing
        }
    }
}