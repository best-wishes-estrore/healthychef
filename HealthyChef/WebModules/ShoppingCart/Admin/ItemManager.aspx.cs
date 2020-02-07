using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using System.Data;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class ItemManager : System.Web.UI.Page
    {
        protected string CurrentAlphaSelection
        {
            get
            {
                if (ViewState["CurrentAlphaSelection"] == null)
                    ViewState["CurrentAlphaSelection"] = string.Empty;

                return ViewState["CurrentAlphaSelection"].ToString();
            }

            set { ViewState["CurrentAlphaSelection"] = value; }
        }

        protected List<string> Alphas
        {
            get
            {
                List<string> alphas = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "All" };
                return alphas;
            }
        }
        
        public string LastSortExpression
        {
            get
            {
                if (ViewState["LastSortExpression"] == null)
                    ViewState["LastSortExpression"] = "Name";

                return ViewState["LastSortExpression"].ToString();

            }
            set
            {
                ViewState["LastSortExpression"] = value;
            }
        }

        public string NewSortExpression
        {
            get
            {
                if (ViewState["NewSortExpression"] == null)
                    ViewState["NewSortExpression"] = "Name";

                return ViewState["NewSortExpression"].ToString();

            }
            set
            {
                ViewState["NewSortExpression"] = value;
            }
        }

        public SortDirection LastSortDirection
        {
            get
            {
                if (ViewState["LastSortDirection"] == null)
                    ViewState["LastSortDirection"] = SortDirection.Ascending;

                return (SortDirection)ViewState["LastSortDirection"];

            }
            set
            {
                ViewState["LastSortDirection"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            rptAlphas.ItemDataBound += new RepeaterItemEventHandler(rptAlphas_ItemDataBound);
            rptAlphas.ItemCommand += new RepeaterCommandEventHandler(rptAlphas_ItemCommand);

            btnAddNewMenuItem.Click += new EventHandler(lkbAddNewMenuItem_Click);

            MenuItem_Edit1.ControlCancelled += new ControlCancelledEventHandler(MenuItem_Edit1_ControlCancelled);
            MenuItem_Edit1.ControlSaved += new ControlSavedEventHandler(MenuItem_Edit1_ControlSaved);

            //gvwActiveMenuItems.PageIndexChanging += new GridViewPageEventHandler(gvwActiveMenuItems_PageIndexChanging);
            //gvwActiveMenuItems.SelectedIndexChanged += new EventHandler(gvwActiveMenuItems_SelectedIndexChanged);
            //gvwActiveMenuItems.Sorting += new GridViewSortEventHandler(gvwActiveMenuItems_Sorting);

            //gvwRetiredMenuItems.PageIndexChanging += new GridViewPageEventHandler(gvwRetiredMenuItems_PageIndexChanging);
            //gvwRetiredMenuItems.SelectedIndexChanged += new EventHandler(gvwRetiredMenuItems_SelectedIndexChanged);
            //gvwRetiredMenuItems.Sorting += new GridViewSortEventHandler(gvwRetiredMenuItems_Sorting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRptAplhas();

                //if (Request.QueryString["i"] != null)
                //{
                //    try
                //    {
                //        int ingrId = int.Parse(Request.QueryString["i"].ToString());
                //        MenuItem_Edit1.PrimaryKeyIndex = ingrId;
                //        MenuItem_Edit1.Bind();

                //        btnAddNewMenuItem.Visible = false;
                //        rptAlphas.Visible = false;
                //        pnlEditMenuItem.Visible = true;
                //        pnlMenuLists.Visible = false;
                //    }
                //    catch (Exception)
                //    {
                //        throw;
                //    }
                //}
                int MenuItemId = 0;
               
                if (Request.QueryString.AllKeys.Contains("MenuId"))
                {
                    MenuItemId = Convert.ToInt32(Request.QueryString["MenuId"].ToString());
                }
                if (MenuItemId !=0)
                {
                    CurrentMenuId.Value = Convert.ToString(MenuItemId);
                    //int ingrId = int.Parse(Request.QueryString["i"].ToString());
                            MenuItem_Edit1.PrimaryKeyIndex = MenuItemId;
                            MenuItem_Edit1.Bind();

                            btnAddNewMenuItem.Visible = false;
                            rptAlphas.Visible = false;
                            pnlEditMenuItem.Visible = true;
                            pnlMenuLists.Visible = false;
                            AplhabetsRow.Visible = false;
                }
                //else
                //    BindBothLists();
            }
        }

        private void BindRptAplhas()
        {
            rptAlphas.DataSource = Alphas;
            rptAlphas.DataBind();
        }

        protected void rptAlphas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string alpha = e.Item.DataItem.ToString();
                LinkButton lnkAlpha = (LinkButton)e.Item.FindControl("lkbAlpha");

                lnkAlpha.Text = alpha;
                lnkAlpha.CommandArgument = alpha;

                if (CurrentAlphaSelection == alpha)
                    lnkAlpha.Font.Bold = true;
            }
        }

        protected void rptAlphas_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            foreach (RepeaterItem item in rptAlphas.Items)
            {
                LinkButton resetAlpha = (LinkButton)item.FindControl("lkbAlpha");
                resetAlpha.Font.Bold = false;
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lnkAlpha = (LinkButton)e.Item.FindControl("lkbAlpha");
                lnkAlpha.Font.Bold = true;
            }

            string selectedChar = e.CommandArgument.ToString();
            SearchIngredients(selectedChar);
        }

        private void SearchIngredients(string alphaToSearch)
        {
            if (alphaToSearch == "All")
                CurrentAlphaSelection = string.Empty;
            else
                CurrentAlphaSelection = alphaToSearch;

            BindBothLists();
        }

        void BindBothLists()
        {
            BindgvwActiveMenuItems();
            BindgvwRetiredMenuItems();
        }

        void BindgvwActiveMenuItems()
        {
            try
            {
                List<hccMenuItem> activeItems = hccMenuItem.GetBy(false);

                if (!string.IsNullOrWhiteSpace(CurrentAlphaSelection))
                    activeItems = activeItems.Where(a => a.Name.ToLower().StartsWith(CurrentAlphaSelection.ToLower())).ToList();

                var pi = typeof(hccMenuItem).GetProperty(NewSortExpression);

                if(LastSortDirection == SortDirection.Ascending)
                    activeItems = activeItems.OrderBy(x => pi.GetValue(x, null)).ToList();
                else if(LastSortDirection == SortDirection.Descending)
                    activeItems = activeItems.OrderByDescending(x => pi.GetValue(x, null)).ToList();
                               
                //gvwActiveMenuItems.DataSource = activeItems;
               // gvwActiveMenuItems.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwActiveMenuItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           // gvwActiveMenuItems.PageIndex = e.NewPageIndex;
            BindgvwActiveMenuItems();
        }

        void gvwActiveMenuItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int menuItemId = int.Parse(gvwActiveMenuItems.SelectedDataKey.Value.ToString());

                //MenuItem_Edit1.PrimaryKeyIndex = menuItemId;
                //MenuItem_Edit1.Bind();

                //btnAddNewMenuItem.Visible = false;
                //rptAlphas.Visible = false;
                //pnlEditMenuItem.Visible = true;
                //pnlMenuLists.Visible = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwActiveMenuItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int menuItemId = int.Parse(e.Keys[0].ToString());
                hccMenuItem delItem = hccMenuItem.GetById(menuItemId);

                if (delItem != null)
                {
                    delItem.Retire(false);
                    BindBothLists();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void BindgvwRetiredMenuItems()
        {
            try
            {
                List<hccMenuItem> retiredItems = hccMenuItem.GetBy(true);

                if (!string.IsNullOrWhiteSpace(CurrentAlphaSelection))
                    retiredItems = retiredItems.Where(a => a.Name.ToLower().StartsWith(CurrentAlphaSelection.ToLower())).ToList();

                var pi = typeof(hccMenuItem).GetProperty(NewSortExpression);

                if (LastSortDirection == SortDirection.Ascending)
                    retiredItems = retiredItems.OrderBy(x => pi.GetValue(x, null)).ToList();
                else if (LastSortDirection == SortDirection.Descending)
                    retiredItems = retiredItems.OrderByDescending(x => pi.GetValue(x, null)).ToList();

                //gvwRetiredMenuItems.DataSource = retiredItems;
                //gvwRetiredMenuItems.DataBind();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwRetiredMenuItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           // gvwRetiredMenuItems.PageIndex = e.NewPageIndex;
            BindgvwRetiredMenuItems();
        }

        void gvwRetiredMenuItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int menuItemId = int.Parse(gvwRetiredMenuItems.SelectedDataKey.Value.ToString());

                //MenuItem_Edit1.PrimaryKeyIndex = menuItemId;
                //MenuItem_Edit1.Bind();

                //btnAddNewMenuItem.Visible = false;
                //rptAlphas.Visible = false;
                //pnlEditMenuItem.Visible = true;
                //pnlMenuLists.Visible = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwRetiredMenuItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int menuItemId = int.Parse(e.Keys[0].ToString());
                hccMenuItem delItem = hccMenuItem.GetById(menuItemId);

                if (delItem != null)
                {
                    delItem.Retire(true);
                    BindBothLists();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void lkbAddNewMenuItem_Click(object sender, EventArgs e)
        {
            btnAddNewMenuItem.Visible = false;
            rptAlphas.Visible = false;
            pnlEditMenuItem.Visible = true;
            pnlMenuLists.Visible = false;
            AplhabetsRow.Visible = false;

            MenuItem_Edit1.Clear();
        }

        void MenuItem_Edit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            lblFeedback.Text = "Item saved: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            btnAddNewMenuItem.Visible = true;
            //rptAlphas.Visible = true;
            pnlEditMenuItem.Visible = false;
            pnlMenuLists.Visible = true;
            AplhabetsRow.Visible = true;

            MenuItem_Edit1.Clear();
            BindBothLists();
        }

        void MenuItem_Edit1_ControlCancelled(object sender)
        {
            btnAddNewMenuItem.Visible = true;
            //rptAlphas.Visible = true;
            pnlEditMenuItem.Visible = false;
            pnlMenuLists.Visible = true;
            AplhabetsRow.Visible = true;

            MenuItem_Edit1.Clear();

            if (Request.QueryString["i"] != null)
                BindBothLists();
        }

        protected void gvwActiveMenuItems_Sorting(object sender, GridViewSortEventArgs e)
        {
            NewSortExpression = e.SortExpression;
            GetSortDirection();

            BindgvwActiveMenuItems();
        }

        protected void gvwRetiredMenuItems_Sorting(object sender, GridViewSortEventArgs e)
        {
            NewSortExpression = e.SortExpression;
            GetSortDirection();

            BindgvwRetiredMenuItems();
        }

        private void GetSortDirection()
        {
            if (LastSortExpression == NewSortExpression)
            {
                if (LastSortDirection == SortDirection.Ascending)
                    LastSortDirection = SortDirection.Descending;
                else
                    LastSortDirection = SortDirection.Ascending;
            }
            else
            {
                LastSortExpression = NewSortExpression;
                LastSortDirection = SortDirection.Descending;
            }
        }
    }
}