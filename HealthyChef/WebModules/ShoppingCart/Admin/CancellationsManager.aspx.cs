using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class CancellationsManager : System.Web.UI.Page
    {
        public int CurrentPurchaseNumber
        {
            get
            {
                if (ViewState["CurrentPurchaseNumber"] == null)
                    ViewState["CurrentPurchaseNumber"] = 0;

                return int.Parse(ViewState["CurrentPurchaseNumber"].ToString());
            }
            set { ViewState["CurrentPurchaseNumber"] = value; }
        }
        public bool CurrentPurchaseNumberIsCancelled
        {
            get
            {
                if (ViewState["CurrentPurchaseNumberIsCancelled"] == null)
                    ViewState["CurrentPurchaseNumberIsCancelled"] = false;

                return bool.Parse(ViewState["CurrentPurchaseNumberIsCancelled"].ToString());
            }
            set { ViewState["CurrentPurchaseNumberIsCancelled"] = value; }
        }

        public string CurrentOrderNumber
        {
            get
            {
                if (ViewState["CurrentOrderNumber"] == null)
                    ViewState["CurrentOrderNumber"] = string.Empty;

                return ViewState["CurrentOrderNumber"].ToString();
            }
            set { ViewState["CurrentOrderNumber"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnFindPurchase.Click += btnFindPurchase_Click;
            btnCancelPurchase.Click += btnCancelPurchase_Click;

            //gvwOrderNumbers.RowDataBound += gvwOrderNumbers_RowDataBound;
            //gvwOrderNumbers.SelectedIndexChanged += gvwOrderNumbers_SelectedIndexChanged;
            //gvwOrderNumbers.RowDeleting += gvwOrderNumbers_RowDeleting;

            //gvwCartItems.RowDataBound += gvwCartItems_RowDataBound;
            //gvwCartItems.RowDeleting += gvwCartItems_RowDeleting;
        }

        void btnFindPurchase_Click(object sender, EventArgs e)
        {
            Page.Validate("CancelGroup");
          
                //pnlCartItems.Visible = true;
            
            if (Page.IsValid)
            {
               //pnlCartItems.Visible = false;
                BindGvwOrderNumbers();
            }
        }

        void BindGvwOrderNumbers()
        {
            try
            {
                CurrentPurchaseNumber = int.Parse(txtPurchaseNumber.Text.Trim());

                hccCart cart = hccCart.GetBy(CurrentPurchaseNumber);

                if (cart != null)
                {
                    if (cart.Status == Common.Enums.CartStatus.Paid)
                    {
                        btnCancelPurchase.Visible = true;
                        //btnCancelPurchase.Text = "Cancel Entire Purchase";
                        btnCancelPurchase.Text = "Void Transaction";
                    }
                    else if (cart.Status == Common.Enums.CartStatus.Cancelled)
                    {
                        CurrentPurchaseNumberIsCancelled = true;

                        if (cart.PurchaseDate.HasValue)
                        {
                            btnCancelPurchase.Visible = true;
                            btnCancelPurchase.Text = "Un-Cancel";
                        }
                    }

                    lblPurchaseStatus.Text = string.Format("Purchase #{0} - Customer: {1} - Status: {2}",
                        cart.PurchaseNumber, cart.OwnerProfile == null ? "Anonymous" : cart.OwnerProfile.FullName, cart.Status);

                    lblOrdNumsLegend.Text = "Order Numbers for Purchase #: " + cart.PurchaseNumber;
                    pnlPurchase.Visible = true;

                    List<hccCartItem> items = hccCartItem.GetBy(cart.CartID);

                    List<string> ordNums = items.Select(a => a.OrderNumber).Distinct().ToList();
                    List<Tuple<string, int, bool>> orders = new List<Tuple<string, int, bool>>(); // orderNum, itemCnt, isCancelled

                    ordNums.ForEach(delegate(string ordNum)
                    {
                        int cnt = items.Count(a => a.OrderNumber == ordNum);
                        int cntNotCancelled = items.Count(a => a.OrderNumber == ordNum && !a.IsCancelled);

                        orders.Add(new Tuple<string, int, bool>(ordNum, cnt, (cntNotCancelled == 0)));
                    });

                    //gvwOrderNumbers.DataSource = orders;
                    //gvwOrderNumbers.DataBind();
                    //gvwOrderNumbers.Visible = true;
                }
                else
                {
                    pnlPurchase.Visible = false;
                    lblFeedback.Text = "Purchase not found.";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void gvwOrderNumbers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Tuple<string, int, bool> item = (Tuple<string, int, bool>)e.Row.DataItem;

                if (item != null)
                {
                    if (CurrentPurchaseNumberIsCancelled)
                    {
                        e.Row.Cells[4].Controls.OfType<LinkButton>().First().Visible = false;
                    }
                    else if (item.Item3 && !CurrentPurchaseNumberIsCancelled)
                    {
                        e.Row.Cells[4].Controls.OfType<LinkButton>().First().Text = "Un-Cancel All Items";
                    }
                }
            }
        }

        void gvwOrderNumbers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CurrentOrderNumber = gvwOrderNumbers.DataKeys[gvwOrderNumbers.SelectedIndex].Value.ToString().Trim();
            BindgvwCartItems();
        }

        void gvwOrderNumbers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //bool doCancel = gvwOrderNumbers.Rows[e.RowIndex].Cells[4].Controls.OfType<LinkButton>().First().Text == "Cancel All Items";
                //string ordNum = gvwOrderNumbers.DataKeys[e.RowIndex].Value.ToString();

                hccCart cart = hccCart.GetBy(CurrentPurchaseNumber);

                if (cart != null)
                {
                    List<hccCartItem> cartitems = hccCartItem.GetBy(cart.CartID);

                    //List<hccCartItem> ordItems = cartitems.Where(a => a.OrderNumber == ordNum).ToList();
                    //ordItems.ForEach(delegate(hccCartItem item) { item.IsCancelled = doCancel; item.Save(); });

                    if (cartitems.Count(a => !a.IsCancelled) == 0)
                    {
                        cart.StatusID = (int)Common.Enums.CartStatus.Cancelled;
                        cart.Save();
                    }
                    else
                    {
                        cart.StatusID = (int)Common.Enums.CartStatus.Paid;
                        cart.Save();
                    }

                    BindGvwOrderNumbers();
                    BindgvwCartItems();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void gvwCartItems_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                hccCartItem item = (hccCartItem)e.Row.DataItem;

                if (CurrentPurchaseNumberIsCancelled)
                {
                    e.Row.Cells[5].Controls.OfType<LinkButton>().First().Visible = false;
                }
                else if (item.IsCancelled)
                {
                    e.Row.Cells[5].Controls.OfType<LinkButton>().First().Text = "Un-Cancel Item";
                }
            }
        }

        void gvwCartItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //bool doCancel = gvwCartItems.Rows[e.RowIndex].Cells[5].Controls.OfType<LinkButton>().First().Text == "Cancel Item";

            //if (doCancel)
            //    //gvwCartItems.Rows[e.RowIndex].Cells[5].Controls.OfType<LinkButton>().First().Visible = false; //.Text = "Un-Cancel Item";

            //int cartItemId = int.Parse(gvwCartItems.DataKeys[e.RowIndex].Value.ToString());

            //hccCartItem item = hccCartItem.GetById(cartItemId);

            //if (item != null)
            //{
            //    item.IsCancelled = doCancel;
            //    item.Save();

            //    BindGvwOrderNumbers();
            //    BindgvwCartItems();
            //}
        }

        void BindgvwCartItems()
        {
            lblCartItemsLegend.Text = "Cart Items for Order #: " + CurrentOrderNumber;

            List<hccCartItem> cartItems = hccCartItem.GetBy(CurrentOrderNumber);

            //gvwCartItems.DataSource = cartItems;
            //gvwCartItems.DataBind();

            //pnlCartItems.Visible = true;
            //gvwCartItems.Visible = true;
        }

        void btnCancelPurchase_Click(object sender, EventArgs e)
        {
            try
            {
                hccCart cart = hccCart.GetBy(CurrentPurchaseNumber);

                if (cart != null)
                {
                    bool doCancel = (btnCancelPurchase.Text == "Void Transaction");

                    List<hccCartItem> items = hccCartItem.GetBy(cart.CartID);
                    items.ForEach(delegate(hccCartItem item)
                    {
                        item.IsCancelled = doCancel;
                        item.Save();
                    });


                    cart.StatusID = doCancel ? (int)Common.Enums.CartStatus.Cancelled : (int)Common.Enums.CartStatus.Paid;
                    cart.Save();

                    CurrentPurchaseNumberIsCancelled = doCancel;

                    if (doCancel)
                        btnCancelPurchase.Text = "Un-Cancel";
                    else
                        btnCancelPurchase.Text = "Void Transaction";

                    BindGvwOrderNumbers();
                    BindgvwCartItems();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}