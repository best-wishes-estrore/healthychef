using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using HealthyChef.Common.Events;
using HealthyChef.Common;

namespace HealthyChef.WebModules.ShoppingCart.Controls.Cart
{
    public partial class CartModule : BayshoreSolutions.WebModules.WebModuleBase
    {
        public hccCart CurrentCart { get; set; }
      
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    MembershipUser user = Helpers.LoggedUser;

                    if (user != null)
                        CurrentCart = hccCart.GetCurrentCart(user);
                    else
                        CurrentCart = hccCart.GetCurrentCart();

                    if (CurrentCart != null)
                    {
                        CartDisplay1.CurrentCartId = CurrentCart.CartID;
                        CartDisplay1.Bind();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }


    }
}