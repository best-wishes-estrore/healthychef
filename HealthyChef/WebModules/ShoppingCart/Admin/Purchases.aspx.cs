using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class Purchases : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //btnSearch.Click += btnSearch_Click;
            //btnClear.Click += btnClear_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //BindDDLS();

                try
                {
                    if (Request.QueryString["id"] != null && !string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                    {
                        int id = int.Parse(Request.QueryString["id"].ToString());
                        hccCart cart = hccCart.GetById(id);

                        if (cart != null)
                        {
                            txtSearchPurchaseNum.Text = cart.PurchaseNumber.ToString();
                            btnSearch_Click(this, new EventArgs());
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            divDetails.Visible = false;
            //gvwPaid.Visible = false;
            //gvwComplete.Visible = false;
            //gvwOpen.Visible = false;
            //gvwCancel.Visible = false;

            string email = null;
            if (!string.IsNullOrWhiteSpace(txtSearchEmail.Text.Trim())) { email = txtSearchEmail.Text.Trim(); }

            string purchNum = null;
            if (!string.IsNullOrWhiteSpace(txtSearchPurchaseNum.Text.Trim())) { purchNum = txtSearchPurchaseNum.Text.Trim(); }

            DateTime? purchDate = null;
            if (!string.IsNullOrWhiteSpace(txtSearchPurchaseDate.Text)) { purchDate = DateTime.Parse(txtSearchPurchaseDate.Text.Trim()); }

            string lastName = null;
            if (!string.IsNullOrWhiteSpace(txtSearchLastName.Text.Trim())) { lastName = txtSearchLastName.Text.Trim(); }

            DateTime? delDate = null;
            if (ddlSearchDeliveryDate.SelectedIndex > 0) { delDate = DateTime.Parse(ddlSearchDeliveryDate.SelectedItem.Text); }

            int purchaseType = int.Parse(ddlPurchaseType.SelectedValue);

            List<hccCart> carts = hccCart.Search(email, purchNum, purchDate, lastName, delDate);

            if(carts == null) return;

            if (purchaseType == 1)
                BindgvwPaid(carts.Where(a => a.StatusID == (int)Enums.CartStatus.Paid).ToList());

            if (purchaseType == 2)
                BindgvwComplete(carts.Where(a => a.StatusID == (int)Enums.CartStatus.Fulfilled).ToList());

            if (purchaseType == 3)
                BindgvwOpen(carts.Where(a => a.StatusID == (int)Enums.CartStatus.Unfinalized).ToList());

            if (purchaseType == 4)
                BindgvwCancel(carts.Where(a => a.StatusID == (int)Enums.CartStatus.Cancelled).ToList());
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchEmail.Text = string.Empty;
            txtSearchPurchaseDate.Text = string.Empty;
            txtSearchPurchaseNum.Text = string.Empty;
            txtSearchLastName.Text = string.Empty;
            ddlSearchDeliveryDate.ClearSelection();

            BindgvwPaid(null);
            BindgvwComplete(null);
            BindgvwOpen(null);
            BindgvwCancel(null);

            divDetails.Visible = false;
            divOrderHtml.InnerHtml = string.Empty;
        }

        void BindgvwPaid(List<hccCart> carts)
        {
            try
            {
                if (carts != null)
                {
                    carts = carts.OrderByDescending(a => a.PurchaseDate).ToList();

                    //gvwPaid.DataSource = carts;
                    //gvwPaid.DataBind();

                    //gvwPaid.Visible = true;
                }
                else
                {
                    //gvwPaid.Visible = false;
                }
            }
            catch
            {
                throw;
            }
        }

        void BindgvwComplete(List<hccCart> carts)
        {
            try
            {
                if (carts != null)
                {
                    //gvwComplete.DataSource = carts;
                    //gvwComplete.DataBind();
                    //gvwComplete.Visible = true;
                }
                else
                {
                    //gvwComplete.Visible = false;
                }
            }
            catch
            {
                throw;
            }
        }

        void BindgvwOpen(List<hccCart> carts)
        {
            try
            {
                if (carts != null)
                {
                    //gvwOpen.DataSource = carts;
                    //gvwOpen.DataBind();
                    //gvwOpen.Visible = true;
                }
                else
                {
                    //gvwOpen.Visible = false;
                }
            }
            catch
            {
                throw;
            }
        }

        void BindgvwCancel(List<hccCart> carts)
        {
            try
            {
                if (carts != null)
                {
                    //gvwCancel.DataSource = carts;
                    //gvwCancel.DataBind();
                    //gvwCancel.Visible = true;
                }
                else
                {
                    //gvwCancel.Visible = false;
                }
            }
            catch
            {
                throw;
            }
        }

        //protected void lkbSelect1_Click(object sender, EventArgs e)
        //{
        //    LinkButton lkbSelect = (LinkButton)sender;
        //    ListViewDataItem lvdi = (ListViewDataItem)lkbSelect.Parent;
        //    hccCart cart = hccCart.GetById(int.Parse(gvwPaid.DataKeys[lvdi.DataItemIndex].Value.ToString()));
        //    HtmlTableRow row = (HtmlTableRow)lvdi.FindControl("trDetails");
        //    LinkButton lkbPrint = (LinkButton)row.FindControl("lkbPrint");

        //    row.Visible = !row.Visible;
        //    row.Cells[0].InnerHtml = row.Visible ? cart.ToHtml() + "<br>" + cart.AuthNetResponse : string.Empty;
        //    lkbSelect.Text = row.Visible ? "Hide" : "Details";
        //    lkbPrint.Visible = row.Visible;
        //}

        //protected void lkbSelect2_Click(object sender, EventArgs e)
        //{
        //    LinkButton lkbSelect = (LinkButton)sender;
        //    ListViewDataItem lvdi = (ListViewDataItem)lkbSelect.Parent;
        //    hccCart cart = hccCart.GetById(int.Parse(lvwCompletedPurchases.DataKeys[lvdi.DataItemIndex].Value.ToString()));
        //    HtmlTableRow row = (HtmlTableRow)lvdi.FindControl("trDetails");
        //    LinkButton lkbPrint = (LinkButton)row.FindControl("lkbPrint");

        //    row.Visible = !row.Visible;
        //    row.Cells[0].InnerHtml = row.Visible ? cart.ToHtml() : string.Empty;
        //    lkbSelect.Text = row.Visible ? "Hide" : "Details";
        //    lkbPrint.Visible = row.Visible;
        //}

        //protected void lkbSelect3_Click(object sender, EventArgs e)
        //{
        //    LinkButton lkbSelect = (LinkButton)sender;
        //    ListViewDataItem lvdi = (ListViewDataItem)lkbSelect.Parent;
        //    hccCart cart = hccCart.GetById(int.Parse(lvwOpenPurchases.DataKeys[lvdi.DataItemIndex].Value.ToString()));
        //    HtmlTableRow row = (HtmlTableRow)lvdi.FindControl("trDetails");
        //    LinkButton lkbPrint = (LinkButton)row.FindControl("lkbPrint");

        //    row.Visible = !row.Visible;
        //    row.Cells[0].InnerHtml = row.Visible ? cart.ToHtml() : string.Empty;
        //    lkbSelect.Text = row.Visible ? "Hide" : "Details";
        //    lkbPrint.Visible = row.Visible;
        //}

        //protected void lkbSelect4_Click(object sender, EventArgs e)
        //{
        //    LinkButton lkbSelect = (LinkButton)sender;
        //    ListViewDataItem lvdi = (ListViewDataItem)lkbSelect.Parent;
        //    hccCart cart = hccCart.GetById(int.Parse(lvwCancelledOrders.DataKeys[lvdi.DataItemIndex].Value.ToString()));
        //    HtmlTableRow row = (HtmlTableRow)lvdi.FindControl("trDetails");
        //    LinkButton lkbPrint = (LinkButton)row.FindControl("lkbPrint");

        //    row.Visible = !row.Visible;
        //    row.Cells[0].InnerHtml = row.Visible ? cart.ToHtml() : string.Empty;
        //    lkbSelect.Text = row.Visible ? "Hide" : "Details";
        //    lkbPrint.Visible = row.Visible;
        //}

        void BindDDLS()
        {
            ddlSearchDeliveryDate.DataSource = hccProductionCalendar.GetAll();
            ddlSearchDeliveryDate.DataTextField = "DeliveryDate";
            ddlSearchDeliveryDate.DataTextFormatString = "{0:MM/dd/yyyy}";
            ddlSearchDeliveryDate.DataValueField = "CalendarID";
            ddlSearchDeliveryDate.DataBind();
            ddlSearchDeliveryDate.Items.Insert(0, new ListItem("All", "-1"));
        }

        protected void gvwPaid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwPaid.PageIndex = e.NewPageIndex;
            btnSearch_Click(this, e);
        }

        protected void gvwPaid_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //int cartId = int.Parse(gvwPaid.DataKeys[e.NewSelectedIndex].Value.ToString());

            //hccCart cart = hccCart.GetById(cartId);
            //if (cart != null)
            //{
            //    divDetails.Visible = true;
            //    divOrderHtml.InnerHtml = cart.ToHtml();
            //}
        }

        protected void gvwComplete_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwComplete.PageIndex = e.NewPageIndex;
            btnSearch_Click(this, e);
        }

        protected void gvwComplete_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //int cartId = int.Parse(gvwComplete.DataKeys[e.NewSelectedIndex].Value.ToString());

            //hccCart cart = hccCart.GetById(cartId);
            //if (cart != null)
            //{
            //    divDetails.Visible = true;
            //    divOrderHtml.InnerHtml = cart.ToHtml();
            //}
        }

        protected void gvwOpen_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwOpen.PageIndex = e.NewPageIndex;
            btnSearch_Click(this, e);
        }

        protected void gvwOpen_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //int cartId = int.Parse(gvwOpen.DataKeys[e.NewSelectedIndex].Value.ToString());

            //hccCart cart = hccCart.GetById(cartId);
            //if (cart != null)
            //{
            //    divDetails.Visible = true;
            //    divOrderHtml.InnerHtml = cart.ToHtml();
            //}
        }

        protected void gvwCancel_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           // gvwCancel.PageIndex = e.NewPageIndex;
            btnSearch_Click(this, e);
        }

        protected void gvwCancel_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            //int cartId = int.Parse(gvwCancel.DataKeys[e.NewSelectedIndex].Value.ToString());

            //hccCart cart = hccCart.GetById(cartId);
            //if (cart != null)
            //{
            //    divDetails.Visible = true;
            //    divOrderHtml.InnerHtml = cart.ToHtml();
            //}
        }
    }
}