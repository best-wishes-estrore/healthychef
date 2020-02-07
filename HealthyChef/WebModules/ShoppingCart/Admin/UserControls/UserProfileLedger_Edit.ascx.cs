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
    public partial class UserProfileLedger_Edit : FormControlBase
    {
        public Guid CurrentAspNetUserId
        {
            get
            {
                if (ViewState["CurrentAspNetUserId"] == null)
                    ViewState["CurrentAspNetUserId"] = Guid.Empty;

                return Guid.Parse(ViewState["CurrentAspNetUserId"].ToString());
            }
            set
            {
                ViewState["CurrentAspNetUserId"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlXactTypes.SelectedIndexChanged += ddlXactTypes_SelectedIndexChanged;
            btnSaveTransaction.Click += base.SubmitButtonClick;
            ddlTransactionAges.SelectedIndexChanged += ddlTransactionAges_SelectedIndexChanged;
            cstXactGCRedeem.ServerValidate += cstXactGCRedeem_ServerValidate;
        }


        protected override void LoadForm()
        {
            if (ddlXactTypes.Items.Count == 0)
            {
                ddlXactTypes.DataSource = Enums.GetEnumAsTupleList(typeof(Enums.LedgerTransactionType));
                ddlXactTypes.DataTextField = "Item1";
                ddlXactTypes.DataValueField = "Item2";
                ddlXactTypes.DataBind();
                ddlXactTypes.Items.Insert(0, new ListItem("Select a Transaction Type...", "-1"));
            }

            lblAccountBalance.Text = hccUserProfile.GetParentProfileBy(CurrentAspNetUserId).AccountBalance.ToString("c");
            BindlvLedger();
        }

        protected override void SaveForm()
        {
            try
            {
                hccLedger ledger = new hccLedger
                {
                    AspNetUserID = CurrentAspNetUserId,
                    CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                    CreatedDate = DateTime.Now,
                    Description = txtXactDesc.Text.Trim(),
                    TransactionTypeID = int.Parse(ddlXactTypes.SelectedValue)
                };

                if (ledger.TransactionType == Enums.LedgerTransactionType.RedeemGiftCertificate)
                {
                    hccCartItem gcCartItem = hccCartItem.GetGiftBy(txtXactGCRedeem.Text);
                    bool updateLedger = false;

                    if (gcCartItem != null)
                    {
                        gcCartItem.Gift_RedeemedBy = CurrentAspNetUserId;
                        gcCartItem.Gift_RedeemedDate = DateTime.Now;
                        gcCartItem.Save();
                        updateLedger = true;
                    }
                    else
                    {
                        ImportedGiftCert cert = ImportedGiftCert.GetBy(txtXactGCRedeem.Text);
                        if (cert != null)
                        {
                            cert.used_by = gcCartItem.UserProfileID;
                            cert.date_used = DateTime.Now.ToString();
                            cert.is_used = "Y";
                            cert.Save();
                            updateLedger = true;
                        }
                    }

                    if (updateLedger)
                    {
                        ledger.TotalAmount = gcCartItem.ItemPrice;
                        ledger.GiftRedeemCode = gcCartItem.Gift_RedeemCode;
                        ledger.Description = "Gift Certificate Redemption: " + ledger.GiftRedeemCode;
                    }
                    else
                    {
                        ledger = null;
                    }
                }
                else
                    ledger.TotalAmount = decimal.Parse(txtXactAmount.Text.Trim());

                hccUserProfile profile = hccUserProfile.GetParentProfileBy(CurrentAspNetUserId);

                if (profile != null)
                {
                    // check against last entry for duplicate
                    hccLedger lastEntry = hccLedger.GetByMembershipID(profile.MembershipID, null).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    bool isDuplicateEntry = false;

                    if (ledger != null)
                    {
                        if (lastEntry != null
                            && ledger.CreatedBy == lastEntry.CreatedBy
                            && ledger.CreditFromBalance == lastEntry.CreditFromBalance
                            && ledger.Description == lastEntry.Description
                            && ledger.PaymentDue == lastEntry.PaymentDue
                            && ledger.TransactionTypeID == lastEntry.TransactionTypeID
                            && ledger.TotalAmount == lastEntry.TotalAmount)
                            isDuplicateEntry = true;

                        if (!isDuplicateEntry)
                        {
                            switch (ledger.TransactionType)
                            {
                                case Enums.LedgerTransactionType.HCCAccountCredit:
                                case Enums.LedgerTransactionType.RedeemGiftCertificate:
                                    profile.AccountBalance = profile.AccountBalance + ledger.TotalAmount;
                                    break;
                                case Enums.LedgerTransactionType.HCCAccountDebit:
                                    profile.AccountBalance = profile.AccountBalance - ledger.TotalAmount;
                                    break;
                                case Enums.LedgerTransactionType.Purchase:
                                case Enums.LedgerTransactionType.Return:
                                    ledger.PaymentDue = ledger.TotalAmount;
                                    break;

                                default: break;
                            }

                            ledger.PostBalance = profile.AccountBalance;
                            ledger.Save();
                            profile.Save();

                            this.Bind();
                            OnSaved(new ControlSavedEventArgs(CurrentAspNetUserId));

                            txtXactAmount.Text = string.Empty;
                            txtXactDesc.Text = string.Empty;
                            txtXactGCRedeem.Text = string.Empty;
                            ddlXactTypes.ClearSelection();
                        }
                        else
                        {
                            lblXactFeedback.Text = "This appears to be a duplicate transaction. If it is not a duplicate, please provide a comment to differentiate this transaction from the last.";
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void ClearForm()
        {
            throw new NotImplementedException();
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void BindlvLedger()
        {
            int days = int.Parse(ddlTransactionAges.SelectedItem.Value);

            if (days == -1)
                lvLedger.DataSource = hccLedger.GetByMembershipID(CurrentAspNetUserId, null);
            else
                lvLedger.DataSource = hccLedger.GetByMembershipID(CurrentAspNetUserId, days);

            lvLedger.DataBind();
        }

        void ddlTransactionAges_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindlvLedger();
        }

        void ddlXactTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlXactTypes.SelectedValue == "40") // 40 = Enums.TransactionType.User Credit-Gift Cert
            {
                divXactGCRedeem.Visible = true;
                divXactAmount.Visible = false;
            }
            else
            {
                divXactGCRedeem.Visible = false;
                divXactAmount.Visible = true;
            }
        }

        protected void cstVal1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
        }

        void cstXactGCRedeem_ServerValidate(object source, ServerValidateEventArgs args)
        {
            hccCartItem gcCartItem = hccCartItem.GetGiftBy(txtXactGCRedeem.Text);

            if (gcCartItem == null || (gcCartItem != null && gcCartItem.IsCompleted))
                args.IsValid = false;
        }
    }
}