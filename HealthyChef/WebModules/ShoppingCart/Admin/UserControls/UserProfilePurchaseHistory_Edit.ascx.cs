using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class UserProfilePurchaseHistory_Edit : FormControlBase
    {   // this.PrimaryKeyIndex as active item Cart.CartID
        public Guid? CurrentAspNetId
        {
            get
            {
                if (ViewState["CurrentAspNetId"] == null)
                    return null;
                else
                    return Guid.Parse(ViewState["CurrentAspNetId"].ToString());
            }
            set
            {
                ViewState["CurrentAspNetId"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected override void LoadForm()
        {
            try
            {
                BindlvwPurchaseHistory();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SaveForm()
        {
            throw new NotImplementedException();
        }

        protected override void ClearForm()
        {
            //order history
            lvwPurchaseHistory.DataSource = null;
            lvwPurchaseHistory.DataBind();
            this.PrimaryKeyIndex = 0;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void BindlvwPurchaseHistory()
        {
            try
            {
                List<hccCart> carts;

                if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators"))
                    carts = hccCart.GetBy(CurrentAspNetId.Value);
                else
                    carts = hccCart.GetCompleted(CurrentAspNetId.Value);

                //lvwPurchaseHistory.DataSource = carts.OrderByDescending(a => a.PurchaseNumber);
                lvwPurchaseHistory.DataSource = carts.OrderByDescending(a => a.PurchaseDate);
                lvwPurchaseHistory.DataBind();
            }
            catch
            {
                throw;
            }
        }

        protected void lkbSelect_Click(object sender, EventArgs e)
        {
            LinkButton lkbSelect = (LinkButton)sender;

            ListViewDataItem lvdi = (ListViewDataItem)lkbSelect.Parent;
            hccCart cart = hccCart.GetById(int.Parse(lvwPurchaseHistory.DataKeys[lvdi.DataItemIndex].Value.ToString()));
            this.PrimaryKeyIndex = cart.CartID;

            HtmlTableRow trStatus = (HtmlTableRow)lvdi.FindControl("trStatus");
            HtmlTableRow trDetail = (HtmlTableRow)lvdi.FindControl("trDetails");
            LinkButton lkbPrint = (LinkButton)trDetail.FindControl("lkbPrint");

            if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators"))
                trStatus.Attributes.Remove("style");

            trDetail.Visible = !trDetail.Visible;
            trStatus.Visible = trDetail.Visible;

            if (lkbSelect.Text == "Details")
                trDetail.Cells[0].InnerHtml = cart.ToHtml();

            lkbSelect.Text = trDetail.Visible ? "Hide" : "Details";
            lkbPrint.Visible = trDetail.Visible;
        }

        protected void lvwPurchaseHistory_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                HiddenField hdnCartId = (HiddenField)e.Item.FindControl("hdnCartId");
                HiddenField hdnStatus = (HiddenField)e.Item.FindControl("hdnStatus");
                hccCart cart = (hccCart)e.Item.DataItem;

                if (hdnCartId != null)
                    hdnCartId.Value = cart.CartID.ToString();

                if (hdnStatus != null)
                    hdnStatus.Value = cart.StatusID.ToString();
            }
        }
    }
}