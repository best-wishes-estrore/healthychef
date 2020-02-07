using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Controls
{
    public partial class GiftCertificateAddControl : BayshoreSolutions.WebModules.WebModuleBase
    {
        hccCartItem CurrentGiftCert;
        
        protected override void OnInit(EventArgs e)
        {
            //AddressRecipient.SetFormFieldValidationGroup(this.ValidationGroup);

            base.OnInit(e);

            //ddlIssueTo.SelectedIndexChanged += ddlIssueTo_SelectedIndexChanged;

            //btnSave.Click += new EventHandler(base.SubmitButtonClick);
            //btnCancel.Click += new EventHandler(base.CancelButtonClick);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
            else
            {
                ProcessDoPostback();
            }
        }

        protected void ProcessDoPostback()
        {
            string target = Request.Params["__EVENTTARGET"];

            if (target == "divSave")
                btnSave_Click(this, new EventArgs());

            if (target == "divCancel")
                this.ClearForm();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate();

            if (Page.IsValid)
            {
                SaveForm();
            }
        }

        protected void SaveForm()
        {
            try
            {
                hccCart userCart = hccCart.GetCurrentCart();
                CurrentGiftCert = hccCartItem.Gift_GenerateNew(userCart.CartID);

                if (CurrentGiftCert != null)
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

                    CurrentGiftCert.Save();

                    HealthyChef.Templates.HealthyChef.Controls.TopHeader header =
                                            (HealthyChef.Templates.HealthyChef.Controls.TopHeader)this.Page.Master.FindControl("TopHeader1");

                    if (header != null)
                        header.SetCartCount();

                    ClearForm();
                    lblFeedback.Text = "Gift certificate has been added to your cart.";
                }
            }
            catch
            {
                throw;
            }
        }

        protected void ClearForm()
        {
            txtAmount.Text = string.Empty;
            txtRecipEmail.Text = string.Empty;
            txtRecipMessage.Text = string.Empty;
            lblRedeemedInfo.Text = string.Empty;
            AddressRecipient.Clear();
        }        
    }
}