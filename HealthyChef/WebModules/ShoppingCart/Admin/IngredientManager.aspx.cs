using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class IngredientManager : System.Web.UI.Page
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            rptAlphas.ItemDataBound += new RepeaterItemEventHandler(rptAlphas_ItemDataBound);
            rptAlphas.ItemCommand += new RepeaterCommandEventHandler(rptAlphas_ItemCommand);

            btnAddNewIngredient.Click += new EventHandler(btnAddNewPreference_Click);

            //gvwActiveIngreds.SelectedIndexChanged += new EventHandler(gvwIngred_SelectedIndexChanged);
            //gvwActiveIngreds.PageIndexChanging += new GridViewPageEventHandler(gvwActiveIngreds_PageIndexChanging);
            //gvwActiveIngreds.RowDataBound += new GridViewRowEventHandler(gvwActiveIngreds_RowDataBound);

            //gvwRetiredIngreds.SelectedIndexChanged += new EventHandler(gvwIngred_SelectedIndexChanged);
            //gvwRetiredIngreds.PageIndexChanging += new GridViewPageEventHandler(gvwRetiredIngreds_PageIndexChanging);
            //gvwRetiredIngreds.RowDataBound += new GridViewRowEventHandler(gvwRetiredIngreds_RowDataBound);

            IngredientEdit1.ControlSaved += new ControlSavedEventHandler(IngredientAdd1_ControlSaved);
            IngredientEdit1.ControlCancelled += new ControlCancelledEventHandler(IngredientAdd1_ControlCancelled);
        }
               
        protected void Page_Load(object sender, EventArgs e)
        {
              if (!IsPostBack)
              {
                BindRptAplhas();

                if (Request.QueryString["i"] != null)
                {
                    try
                    {
                        int ingrId = int.Parse(Request.QueryString["i"].ToString());
                        IngredientEdit1.PrimaryKeyIndex = ingrId;
                        IngredientEdit1.Bind();

                        btnAddNewIngredient.Visible = false;
                        divEdit.Visible = true;
                        pnlGrids.Visible = false;
                        AplhabetsRow.Visible = false;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                //else
                    //BindGrids();
            }
        }

        private void BindRptAplhas()
        {
            rptAlphas.DataSource = Alphas;
            rptAlphas.DataBind();
        }

        private void SearchIngredients(string alphaToSearch)
        {
            if (alphaToSearch == "All")
                CurrentAlphaSelection = string.Empty;
            else
                CurrentAlphaSelection = alphaToSearch;

            BindGrids();
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

        void BindGrids()
        {
            BindgvwActiveIngreds();
            BindgvwRetiredIngreds();
        }

        void BindgvwActiveIngreds()
        {
            List<hccIngredient> actIngr = hccIngredient.GetActive();

            if (!string.IsNullOrWhiteSpace(CurrentAlphaSelection))
                actIngr = actIngr.Where(a => a.Name.ToLower().StartsWith(CurrentAlphaSelection.ToLower())).ToList();

            //gvwActiveIngreds.DataSource = actIngr;
            //gvwActiveIngreds.DataBind();
        }

        void BindgvwRetiredIngreds()
        {
            List<hccIngredient> retIngr = hccIngredient.GetRetired();

            if (!string.IsNullOrWhiteSpace(CurrentAlphaSelection))
                retIngr = retIngr.Where(a => a.Name.ToLower().StartsWith(CurrentAlphaSelection.ToLower())).ToList();

            //gvwRetiredIngreds.DataSource = retIngr;
            //gvwRetiredIngreds.DataBind();
        }

        void gvwIngred_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView gvw = (GridView)sender;

            int prefId = int.Parse(gvw.SelectedDataKey.Value.ToString());
            IngredientEdit1.PrimaryKeyIndex = prefId;
            IngredientEdit1.Bind();

            btnAddNewIngredient.Visible = false;
            rptAlphas.Visible = false;
            pnlGrids.Visible = false;
            divEdit.Visible = true;
            AplhabetsRow.Visible = false;
        }

        void gvwActiveIngreds_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwActiveIngreds.PageIndex = e.NewPageIndex;
            BindgvwActiveIngreds();
        }

        void gvwActiveIngreds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblAllergens = (Label)e.Row.FindControl("lblAllergens");

            if (lblAllergens != null)
            {
                hccIngredient ing = (hccIngredient)e.Row.DataItem;

                string allergens = string.Empty;
                ing.GetAllergens().ForEach(a => allergens += a.Name + ", ");

                lblAllergens.Text = allergens.Trim().TrimEnd(',');
            }
        }

        void gvwRetiredIngreds_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwRetiredIngreds.PageIndex = e.NewPageIndex;
            BindgvwRetiredIngreds();
        }

        void gvwRetiredIngreds_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lblAllergens = (Label)e.Row.FindControl("lblAllergens");

            if (lblAllergens != null)
            {
                hccIngredient ing = (hccIngredient)e.Row.DataItem;

                string allergens = string.Empty;
                ing.GetAllergens().ForEach(a => allergens += a.Name + ", ");

                lblAllergens.Text = allergens.Trim().TrimEnd(',');
            }
        }
              
        void btnAddNewPreference_Click(object sender, EventArgs e)
        {
            btnAddNewIngredient.Visible = false;
            rptAlphas.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;
            AplhabetsRow.Visible = false;
        }

        protected void IngredientAdd1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            IngredientEdit1.Clear();
            BindGrids();

            btnAddNewIngredient.Visible = true;
            //rptAlphas.Visible = true;
            pnlGrids.Visible = true;
            divEdit.Visible = false;
            lblFeedback.Text = string.Format("Ingredient saved: {0}", DateTime.Now);
            AplhabetsRow.Visible = true;
        }

        protected void IngredientAdd1_ControlCancelled(object sender)
        {
            IngredientEdit1.Clear();

            btnAddNewIngredient.Visible = true;
            //rptAlphas.Visible = true;
            pnlGrids.Visible = true;
            divEdit.Visible = false;
            AplhabetsRow.Visible = true;

            if (Request.QueryString["i"] != null)
                BindGrids();
        }
    }
}