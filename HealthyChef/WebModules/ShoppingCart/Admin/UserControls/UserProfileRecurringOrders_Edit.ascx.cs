using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class UserProfileRecurringOrders_Edit : FormControlBase
    {
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

        protected void Page_PreInit(object sender, EventArgs e)
        {
            lvRecurringOrders.Items.Clear();
            LoadForm();
        }
        void lvRecurringOrders_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        void lvRecurringOrders_DataBinding(object sender, EventArgs e)
        {

        }

        public delegate void RecurringOrderListItemUpdatedEventHandler();
        public event RecurringOrderListItemUpdatedEventHandler RecurringOrderListItemUpdated;

        protected void OnCartItemListItemUpdated()
        {
            if (RecurringOrderListItemUpdated != null)
                RecurringOrderListItemUpdated();
        }

        protected override void LoadForm()
        {
            lvRecurringOrders.DataBinding += lvRecurringOrders_DataBinding;
            lvRecurringOrders.ItemDataBound += lvRecurringOrders_ItemDataBound;

            BindRecurringOrderData();
        }

        protected override void SaveForm() { }
        protected override void ClearForm() { }
        protected override void SetButtons() { }

        private void BindRecurringOrderData()
        {
            using (var hcE = new healthychefEntities())
            {
                if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators") || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "EmployeeManager") || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "EmployeeService"))
                {
                    lvRecurringOrders.DataSource = hcE.hccRecurringOrders.Where(u => u.AspNetUserID == CurrentAspNetId.Value).ToList();
                    lvRecurringOrders.DataBind();                      
                }
                else // TODO: this may not be required, if the viewstate always holds a valid AspNetId, then use code above.
                {
                    MembershipUser user = Helpers.LoggedUser;
                    lvRecurringOrders.DataSource = hcE.hccRecurringOrders.Where(u => u.AspNetUserID == (Guid)user.ProviderUserKey).ToList();
                    lvRecurringOrders.DataBind();
                }
            }            
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetCartItemMeal(int cartItemID)
        {
            try
            {
                var cartItem = hccCartItem.GetById(cartItemID);
                return cartItem.ItemName + "<br />(Quantity: " + cartItem.Quantity + ")";
            }
            catch (Exception)
            {
                return "Item not available";
            }
            
        }

        public string GetNextRecurringDate(int cartId, int cartItemId)
        {
            using (var hcE = new healthychefEntities())
            {
                try
                {
                    var recurringItem = hcE.hcc_RecurringOrderStartDate(cartId, cartItemId).SingleOrDefault();

                    return ((DateTime)recurringItem.MaxDeliveryDate).ToShortDateString();
                }
                catch (Exception)
                {
                    return "Date not available";
                }

            }
        }


        protected void btnDeleteOnCommand(object sender, CommandEventArgs e)
        {

            using (var hcE = new healthychefEntities())
            {
                var cartId = int.Parse(e.CommandArgument.ToString().Split('_')[0]);
                var cartItemId = int.Parse(e.CommandArgument.ToString().Split('_')[1]);

                var rOrder = hcE.hccRecurringOrders.FirstOrDefault(i => i.CartID == cartId && i.CartItemID == cartItemId);
                hcE.hccRecurringOrders.DeleteObject(rOrder);

                hcE.SaveChanges();


                Page_PreInit(sender, e);

            }
            
        }
    }
}