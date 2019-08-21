using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.Cart
{
    public partial class OrderConfirmation : System.Web.UI.Page
    {
        protected hccCart CurrentCart { get; set; }
        protected string CurrentCity { get; set; }
        protected string CurrentState { get; set; }
        protected string CurrentCountry { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnContinue.Click += btnContinue_Click;
            aSignOut.ServerClick += new EventHandler(aSignOut_ServerClick);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pn"] != null && !string.IsNullOrWhiteSpace(Request.QueryString["pn"].ToString()))
                {
                    int purchaseNum;
                    if (int.TryParse(Request.QueryString["pn"], out purchaseNum))
                    {
                        CurrentCart = hccCart.GetBy(purchaseNum);
                    }
                }

                if (CurrentCart != null && Helpers.LoggedUser != null
                    && (CurrentCart.AspNetUserID == (Guid)Helpers.LoggedUser.ProviderUserKey
                            || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators")))
                {
                    lblCompleteDetail.Text = CurrentCart.ToHtml();

                    if (CurrentCart.OwnerProfile != null && CurrentCart.OwnerProfile.ASPUser != null)
                        lblCompleteEmail.Text = CurrentCart.OwnerProfile.ASPUser.Email;

                    //hdnPurchaseNum.Value = this.PurchaseNumber.ToString();
                    //hdnTotal.Value = this.TotalAmount.ToString("f2");
                    //hdnTax.Value = this.TaxAmount.ToString("f2");
                    //hdnShipping.Value = this.ShippingAmount.ToString("f2");

                    hccUserProfile billProf = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                    if (billProf != null)
                    {
                        hccAddress billAddr = null;
                        if (billProf.BillingAddressID.HasValue)
                        {
                            billAddr = hccAddress.GetById(billProf.BillingAddressID.Value);
                        }

                        if (billAddr != null)
                        {
                            CurrentCity = billAddr.City;
                            CurrentState = billAddr.State;
                            CurrentCountry = billAddr.Country;
                            //CurrentUserName = billAddr.FirstName;
                            //CurrentUserEmail = billProf.aspnet_Membership.Email;
                        }
                        else
                        {
                            CurrentCity = string.Empty;
                            CurrentState = string.Empty;
                            CurrentCountry = string.Empty;
                            //CurrentUserName = string.Empty;
                            //CurrentUserEmail = string.Empty;
                        }
                    }
                    else
                    {
                        CurrentCity = string.Empty;
                        CurrentState = string.Empty;
                        CurrentCountry = string.Empty;
                        //CurrentUserName = string.Empty;
                        //CurrentUserEmail = string.Empty;
                    }
                    //string TrackAmount = "'" + CurrentCart.TotalAmount.ToString("f2") + "'";
                    //Session["trackAmount"] = TrackAmount;
                    //track_id.Attributes["src"] = ResolveUrl("https://shareasale.com/sale.cfm?amount=TrackAmount&tracking=user@shareasale.com&transtype=lead&merchantID=11");

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("var _gaq = _gaq || [];");
                    sb.AppendLine("_gaq.push(['_setAccount', 'UA-32947650-1']);");
                    sb.AppendLine("_gaq.push(['_trackPageview']);");
                    sb.AppendLine("_gaq.push(['_addTrans',");
                    sb.AppendLine("'" + CurrentCart.PurchaseNumber.ToString() + "',           // transaction ID - required");
                    sb.AppendLine("'Healthy Chef Creations',  // affiliation or store name");
                    sb.AppendLine("'" + CurrentCart.TotalAmount.ToString("f2") + "',          // total - required");
                    sb.AppendLine("'" + CurrentCart.TaxAmount.ToString("f2") + "',           // tax");
                    sb.AppendLine("'" + CurrentCart.ShippingAmount.ToString("f2") + "',              // shipping");
                    sb.AppendLine("'" + CurrentCity + "',       // city");
                    sb.AppendLine("'" + CurrentState + "',     // state or province");
                    sb.AppendLine("'" + CurrentCountry + "'             // country");
                    //sb.AppendLine("'" + CurrentUserName + "'    User Name//");
                    //sb.AppendLine("'" + CurrentUserEmail + "'    User Email//");
                    sb.AppendLine("]);");
                    sb.AppendLine("_gaq.push(['_trackTrans']); //submits transaction to the Analytics servers");
                    sb.AppendLine("(function() {");
                    sb.AppendLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
                    sb.AppendLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
                    sb.AppendLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
                    sb.AppendLine("})();");

                    if (!ClientScript.IsStartupScriptRegistered("GoogleTrack"))
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "GoogleTrack", sb.ToString(), true);
                    Response.Redirect(string.Format("~/thankyou.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                         CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                                        hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City,
                                        hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State,
                                        hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country),true);
                }
                else
                {
                    pnlComplete.Visible = false;
                    lblFeedback.Text = "You are not authorized to view this information.";
                }

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    aLogin.Visible = false;
                    aProfile.Visible = true;
                    aSignOut.Visible = true;
                }
                else
                {
                    aCart.Visible = true;
                    aLogin.Visible = true;
                    aProfile.Visible = false;
                    aSignOut.Visible = false;
                }


                SetCartCount();
            }

        }

        void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/order-now.aspx", true);
        }

        void aSignOut_ServerClick(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Roles.DeleteCookie();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // clear session cookie
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            FormsAuthentication.RedirectToLoginPage();
        }

        public void SetCartCount()
        {
            try
            {
                MembershipUser user = Helpers.LoggedUser;

                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    aCart.Visible = true;
                    hccCart cart = null;

                    if (user == null)
                        cart = hccCart.GetCurrentCart();

                    // commented out DBall 10-14-2013
                    //else
                    //    cart = hccCart.GetCurrentCart(user);

                    if (cart != null)
                    {
                        List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                        lblCartCount.Text = cartItems.Count.ToString();

                    }
                    else
                        lblCartCount.Text = "0";
                }
                else
                {


                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}