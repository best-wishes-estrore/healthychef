using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using System.Web.Security;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class MenuManager : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddNewMenu.Click += new EventHandler(btnAddNewMenu_Click);

            MenuEdit1.ControlCancelled += new ControlCancelledEventHandler(MenuEdit1_ControlCancelled);
            MenuEdit1.ControlSaved += new ControlSavedEventHandler(MenuEdit1_ControlSaved);

            //gvwActiveMenus.PageIndexChanging += new GridViewPageEventHandler(gvwActiveMenus_PageIndexChanging);
            //gvwActiveMenus.SelectedIndexChanged += new EventHandler(gvwActiveMenus_SelectedIndexChanged);

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string MenuId = string.Empty;
            if (Request.QueryString.AllKeys.Contains("MenuId"))
            {
                MenuId = Request.QueryString["MenuId"].ToString();
            }
            if (!string.IsNullOrEmpty(MenuId) && MenuEdit1.PrimaryKeyIndex == 0)
            {
                CurrentMenuId.Value = MenuId;
                int menuItemId = Convert.ToInt32(MenuId);
                MenuEdit1.PrimaryKeyIndex = menuItemId;
                MenuEdit1.Bind();
                btnAddNewMenu.Visible = false;
                pnlEditMenu.Visible = true;
                pnlMenus.Visible = false;
            }
            //if (!Page.IsPostBack)
            //{
            //    if (Request.QueryString["m"] != null)
            //    {
            //        try
            //        {
            //            int menuId = int.Parse(Request.QueryString["m"].ToString());
            //            MenuEdit1.PrimaryKeyIndex = menuId;
            //            MenuEdit1.Bind();

            //            btnAddNewMenu.Visible = false;
            //            pnlEditMenu.Visible = true;
            //            pnlMenus.Visible = false;
            //        }
            //        catch (Exception)
            //        {
            //            throw;
            //        }
            //    }
            else if ((!Page.IsPostBack))
            {
                //BindgvwActiveMenus();
            }

        }

        void BindgvwActiveMenus()
        {
            List<hccMenu> menus = hccMenu.GetAll();

            //gvwActiveMenus.DataSource = menus;
            //gvwActiveMenus.DataBind();
        }

        void btnAddNewMenu_Click(object sender, EventArgs e)
        {
            btnAddNewMenu.Visible = false;
            pnlEditMenu.Visible = true;
            pnlMenus.Visible = false;

            MenuEdit1.Clear();
        }

        void gvwActiveMenus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // gvwActiveMenus.PageIndex = e.NewPageIndex;
            BindgvwActiveMenus();
        }

        void gvwActiveMenus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int menuItemId = int.Parse(gvwActiveMenus.SelectedDataKey.Value.ToString());

                //MenuEdit1.PrimaryKeyIndex = menuItemId;
                //MenuEdit1.Bind();

                //btnAddNewMenu.Visible = false;
                //pnlEditMenu.Visible = true;
                //pnlMenus.Visible = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwActiveMenus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int menuItemId = int.Parse(e.Keys[0].ToString());
                hccMenuItem delItem = hccMenuItem.GetById(menuItemId);

                if (delItem != null)
                {
                    delItem.Retire(false);
                    BindgvwActiveMenus();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void MenuEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            lblFeedback.Text = "Menu saved: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            btnAddNewMenu.Visible = true;
            pnlEditMenu.Visible = false;
            pnlMenus.Visible = true;

            MenuEdit1.Clear();
            BindgvwActiveMenus();
        }

        void MenuEdit1_ControlCancelled(object sender)
        {
            btnAddNewMenu.Visible = true;
            pnlEditMenu.Visible = false;
            pnlMenus.Visible = true;

            MenuEdit1.Clear();

            if (Request.QueryString["m"] != null)
                BindgvwActiveMenus();
        }
    }
}