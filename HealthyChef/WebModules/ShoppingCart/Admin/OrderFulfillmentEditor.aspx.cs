using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class OrderFulfillmentEditor : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["ci"] != null && !string.IsNullOrWhiteSpace(Request.QueryString["ci"]))
                    {
                        int ci = 0;
                        ci = int.Parse(Request.QueryString["ci"].ToString());

                        if (Request.QueryString["it"] != null && !string.IsNullOrWhiteSpace(Request.QueryString["it"]))
                        {
                            Enums.CartItemType cit = (Enums.CartItemType)Enum.Parse(typeof(Enums.CartItemType), Request.QueryString["it"].ToString());

                            if (cit == Enums.CartItemType.AlaCarte)
                            {
                                OrderFulfillmentAlaCarte_Edit1.PrimaryKeyIndex = ci;
                                OrderFulfillmentAlaCarte_Edit1.Bind();
                                OrderFulfillmentAlaCarte_Edit1.Visible = true;
                            }
                            else if (cit == Enums.CartItemType.DefinedPlan)
                            {
                                OrderFulfillmentProgram_Edit1.PrimaryKeyIndex = ci;
                                OrderFulfillmentProgram_Edit1.Bind();
                                OrderFulfillmentProgram_Edit1.Visible = true;
                            }
                            else if (cit == Enums.CartItemType.GiftCard)
                            {
                                OrderFulfillmentGiftCert_Edit1.PrimaryKeyIndex = ci;
                                OrderFulfillmentGiftCert_Edit1.Bind();
                                OrderFulfillmentGiftCert_Edit1.Visible = true;
                            }
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}