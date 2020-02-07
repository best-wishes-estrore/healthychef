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
    public partial class AllergenManager : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddNewAllergen.Click += new EventHandler(btnAddNewAllergen_Click);

            //gvwActiveAllergens.SelectedIndexChanged += new EventHandler(gvw_SelectedIndexChanged);
            //gvwActiveAllergens.PageIndexChanging += new GridViewPageEventHandler(gvwActiveAllergens_PageIndexChanging);

            //gvwRetiredAllergens.SelectedIndexChanged += new EventHandler(gvw_SelectedIndexChanged);
            //gvwRetiredAllergens.PageIndexChanging += new GridViewPageEventHandler(gvwRetiredAllergens_PageIndexChanging);

            AllergenEdit1.ControlSaved += new ControlSavedEventHandler(AllergenAdd1_ControlSaved);
            AllergenEdit1.ControlCancelled += new ControlCancelledEventHandler(AllergenAdd1_ControlCancelled);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int AllergenID = 0;
            if (Request.QueryString.AllKeys.Contains("AllergenID"))
            {
                AllergenID = int.Parse(Request.QueryString["AllergenID"]);
            }
            if (AllergenID != 0)
            {
                AllergenEdit1.PrimaryKeyIndex = AllergenID;
                AllergenEdit1.Bind();

                btnAddNewAllergen.Visible = false;
                pnlGrids.Visible = false;
                divEdit.Visible = true;
            }
            else if (!IsPostBack)
            {
                //BindGrids();
            }
        }

        void BindGrids()
        {
            
            BindgvwActiveAllergens();
            BindgvwRetiredAllergens();
        }

        void BindgvwActiveAllergens()
        {
            var allergens = hccAllergen.GetActive();

            //gvwActiveAllergens.DataSource = allergens;
            //gvwActiveAllergens.DataBind();
        }

        void BindgvwRetiredAllergens()
        {
            var allergens = hccAllergen.GetInactive();

            //gvwRetiredAllergens.DataSource = allergens;
            //gvwRetiredAllergens.DataBind();
        }

        void gvw_SelectedIndexChanged(object sender, EventArgs e)
        {

            GridView gvw = (GridView)sender;

            int prefId = int.Parse(gvw.SelectedDataKey.Value.ToString());
            AllergenEdit1.PrimaryKeyIndex = prefId;
            AllergenEdit1.Bind();

            btnAddNewAllergen.Visible = false;
            pnlGrids.Visible = false;
            divEdit.Visible = true;
        }

        void gvwActiveAllergens_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwActiveAllergens.PageIndex = e.NewPageIndex;
            BindgvwActiveAllergens();
        }

        void gvwRetiredAllergens_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwRetiredAllergens.PageIndex = e.NewPageIndex;
            BindgvwRetiredAllergens();
        }

        void btnAddNewAllergen_Click(object sender, EventArgs e)
        {
            btnAddNewAllergen.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;
        }

        protected void AllergenAdd1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            AllergenEdit1.Clear();
            BindGrids();

            btnAddNewAllergen.Visible = true;
            pnlGrids.Visible = true;
            divEdit.Visible = false;

            lblFeedback.Text = string.Format("Allergen saved: {0}", DateTime.Now);
        }

        protected void AllergenAdd1_ControlCancelled(object sender)
        {
            AllergenEdit1.Clear();

            btnAddNewAllergen.Visible = true;
            pnlGrids.Visible = true;
            divEdit.Visible = false;
        }
             
    }
}