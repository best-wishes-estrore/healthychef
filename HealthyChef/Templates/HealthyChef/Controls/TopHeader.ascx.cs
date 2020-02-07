using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.Templates.HealthyChef.Controls
{
    public partial class TopHeader : System.Web.UI.UserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            aSignOut.ServerClick += new EventHandler(aSignOut_ServerClick);
        }
        public int CartID;
        public int MealCount;
        public class Meals
    {
        public int CartID { get; set; }
        public int MealCount { get;set;}
    }
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
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
        public void MealsCountVal(int ID, int Meal)
        {
            CartID = ID;
            MealCount = Meal;
        }
       public void SetCartCount()
       {
            try
            {
                MembershipUser user = Helpers.LoggedUser;
                aCart.Attributes.Remove("class");
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    aCart.Visible = true;
                    hccCart cart = null;

                    if (user == null)
                        cart = hccCart.GetCurrentCart();
                    else
                        cart = hccCart.GetCurrentCart(user);

                    if (cart != null)
                    {
                       List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                       hccCartItem obj = new hccCartItem();
                        
                        if (CartID != 0 && MealCount != 0)
                        {
                            Session["id"] = CartID;
                            Session["meals"] = MealCount;
                        }
                        if (cartItems.Count > 0)
                        {
                            aCart.Attributes.Add("class", "cart-has-items");
                        }
                        lblCartCount.Text = cartItems.Count.ToString();
                    }
                    else
                        lblCartCount.Text = "0";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}