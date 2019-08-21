using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class OrderFulfillmentGiftCert_Edit : FormControlBase
    {
        // Note: this.PrimaryKeyIndex as CartItemId
        public int CurrentCalendarId
        {
            get
            {
                if (ViewState["CurrentCalendarId"] == null)
                    ViewState["CurrentCalendarId"] = 0;

                return int.Parse(ViewState["CurrentCalendarId"].ToString());
            }
            set { ViewState["CurrentCalendarId"] = value; }
        }
        public int CurrentDay
        {
            get
            {
                if (ViewState["CurrentDay"] == null)
                    ViewState["CurrentDay"] = 0;

                return int.Parse(ViewState["CurrentDay"].ToString());
            }
            set { ViewState["CurrentDay"] = value; }
        }

        public hccCartItem _currentCartItem;
        public hccCartItem CurrentCartItem
        {
            get
            {
                if (_currentCartItem == null)
                    _currentCartItem = hccCartItem.GetById(this.PrimaryKeyIndex);

                return (hccCartItem)_currentCartItem;
            }
            set { ViewState["CurrentCartItem"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += base.SubmitButtonClick;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        protected override void LoadForm()
        {
            try
            {               
                if (Request.QueryString["dd"] != null && !string.IsNullOrEmpty(Request.QueryString["dd"]))
                {
                    DateTime _delDate = DateTime.ParseExact(Request.QueryString["dd"].ToString(), "M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    hccProductionCalendar cal = hccProductionCalendar.GetBy(_delDate);
                    //hccProductionCalendar cal = hccProductionCalendar.GetBy(DateTime.Parse(Request.QueryString["dd"].ToString()));

                    if (cal != null)
                    {
                        CurrentCalendarId = cal.CalendarID;

                        if (cal.DeliveryDate < DateTime.Now)
                        {
                            //chkIsCancelled.Enabled = false;
                            btnSave.Enabled = false;
                        }
                    }

                    lkbBack.PostBackUrl += "?cal=" + cal.CalendarID.ToString();
                }

                if (CurrentCartItem != null)
                {
                    // load user profile data
                    hccUserProfile profile = CurrentCartItem.UserProfile;

                    if (profile != null)
                    {
                        hccUserProfile parent = hccUserProfile.GetParentProfileBy(profile.MembershipID);
                        if (parent != null)
                            lblCustomerName.Text = parent.FirstName + " " + parent.LastName;

                        lblProfileName.Text = profile.ProfileName;
                        lblOrderNumber.Text = CurrentCartItem.OrderNumber;
                        lblDeliveryDate.Text = CurrentCartItem.DeliveryDate.ToShortDateString();

                        lvwAllrgs.DataSource = profile.GetAllergens();
                        lvwAllrgs.DataBind();
                        ProfileNotesEdit_AllergenNote.CurrentUserProfileId = profile.UserProfileID;
                        ProfileNotesEdit_AllergenNote.Bind();

                        lvwPrefs.DataSource = profile.GetPreferences();
                        lvwPrefs.DataBind();
                        ProfileNotesEdit_PreferenceNote.CurrentUserProfileId = profile.UserProfileID;
                        ProfileNotesEdit_PreferenceNote.Bind();

                        lblItemName.Text = CurrentCartItem.ItemName;
                        lblPrice.Text = CurrentCartItem.ItemPrice.ToString("c");
                        chkIsComplete.Checked = CurrentCartItem.IsCompleted;
                        chkIsCancelledDisplay.Checked = CurrentCartItem.IsCancelled;
                        //chkIsCancelled.Attributes.Add("onclick", "javascript: return confirm('Are you sure that you want to change this order item's Cancellation status?')");
                    }

                    lblQuantity.Text = CurrentCartItem.Quantity.ToString();

                    if (CurrentCartItem.Gift_IssuedTo.HasValue)
                        lblIssuedTo.Text = hccUserProfile.GetParentProfileBy(CurrentCartItem.Gift_IssuedTo.Value).FullName;

                    if (CurrentCartItem.Gift_IssuedDate.HasValue)
                        lblIssuedDate.Text = CurrentCartItem.Gift_IssuedDate.Value.ToShortDateString();

                    lblRecipientEmail.Text = CurrentCartItem.Gift_RecipientEmail;

                    if (CurrentCartItem.Gift_RecipientAddressId.HasValue)
                        lblRecipientAddress.Text = hccAddress.GetById(CurrentCartItem.Gift_RecipientAddressId.Value).ToHtml();

                    lblRecipientMessage.Text = CurrentCartItem.Gift_RecipientMessage;

                    if (CurrentCartItem.Gift_RedeemedBy.HasValue)
                        lblRedeemedBy.Text = hccUserProfile.GetParentProfileBy(CurrentCartItem.Gift_RedeemedBy.Value).FullName;

                    if (CurrentCartItem.Gift_RedeemedDate.HasValue)
                        lblRedeemedDate.Text = CurrentCartItem.Gift_RedeemedDate.Value.ToShortDateString();

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SaveForm()
        {
            try
            {
                if (CurrentCartItem != null)
                {
                    if (chkIsComplete.Enabled)
                        CurrentCartItem.IsCompleted = chkIsComplete.Checked;

                    //if (chkIsCancelled.Enabled)
                    //    CurrentCartItem.IsCancelled = chkIsCancelled.Checked;
                    
                    CurrentCartItem.Save();

                    lblFeedback.Text = "Item Saved: " + DateTime.Now.ToString();
                    lblFeedback.ForeColor = Color.Green;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void ClearForm()
        {
            chkIsCancelledDisplay.Checked = false;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }     
    }
}