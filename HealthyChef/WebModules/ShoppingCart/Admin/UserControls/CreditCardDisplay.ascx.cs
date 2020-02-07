using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class CreditCardDisplay : System.Web.UI.UserControl
    {
        public CardInfo CurrentCardInfo { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public void Bind()
        {
            if (CurrentCardInfo != null)
            {
                lblNameOnCard.Text = CurrentCardInfo.NameOnCard;

                if (CurrentCardInfo.CardNumber.Length >= 4)
                    lblLast4.Text = "************" + CurrentCardInfo.CardNumber.Substring(CurrentCardInfo.CardNumber.Length - 4, 4);

                lblType.Text = CurrentCardInfo.CardType.ToString();

                lblExpires.Text = CurrentCardInfo.ExpMonth + "/" + CurrentCardInfo.ExpYear;
            }
        }
    }
}