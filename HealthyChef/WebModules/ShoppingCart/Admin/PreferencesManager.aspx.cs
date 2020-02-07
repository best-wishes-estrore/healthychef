using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using HealthyChef.Common.StaticClasses;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class PreferencesManager : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddNewPreference.Click += new EventHandler(btnAddNewPreference_Click);

            //gvwActiveCustPrefs.SelectedIndexChanged += new EventHandler(gvwPrefs_SelectedIndexChanged);
            //gvwActiveCustPrefs.PageIndexChanging += new GridViewPageEventHandler(gvwActiveCustPrefs_PageIndexChanging);
            //gvwRetiredCustPrefs.SelectedIndexChanged += new EventHandler(gvwPrefs_SelectedIndexChanged);
            //gvwRetiredCustPrefs.PageIndexChanging += new GridViewPageEventHandler(gvwRetiredCustPrefs_PageIndexChanging);
            //gvwActiveMealPrefs.SelectedIndexChanged += new EventHandler(gvwPrefs_SelectedIndexChanged);
            //gvwActiveMealPrefs.PageIndexChanging += new GridViewPageEventHandler(gvwActiveMealPrefs_PageIndexChanging);
            //gvwRetiredMealPrefs.SelectedIndexChanged += new EventHandler(gvwPrefs_SelectedIndexChanged);
            //gvwRetiredMealPrefs.PageIndexChanging += new GridViewPageEventHandler(gvwRetiredMealPrefs_PageIndexChanging);

            PreferenceEdit1.ControlSaved += new ControlSavedEventHandler(PreferenceAdd_ControlSaved);
            PreferenceEdit1.ControlCancelled += new ControlCancelledEventHandler(PreferenceAdd_ControlCancelled);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int prefId = 0;
            if (Request.QueryString.AllKeys.Contains("PreferenceID"))
            {
                prefId = int.Parse(Request.QueryString["PreferenceID"]);
            }

            if (prefId != 0)
            {
                PreferenceEdit1.PrimaryKeyIndex = prefId;
                PreferenceEdit1.Bind();

                btnAddNewPreference.Visible = false;
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
            BindgvwActiveMealPrefs();
            BindgvwRetiredMealPrefs();
            BindgvwActiveCustPrefs();
            BindgvwRetiredCustPrefs();
        }

        void BindgvwActiveMealPrefs()
        {
            //gvwActiveMealPrefs.DataSource = hccPreference.GetActive((int)Enums.PreferenceType.Meal);
            //gvwActiveMealPrefs.DataBind();
        }
        void gvwActiveMealPrefs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwActiveMealPrefs.PageIndex = e.NewPageIndex;
            BindgvwActiveMealPrefs();
        }
        
        void BindgvwRetiredMealPrefs()
        {
            //gvwRetiredMealPrefs.DataSource = hccPreference.GetRetired((int)Enums.PreferenceType.Meal);
            //gvwRetiredMealPrefs.DataBind();

        }

        void gvwRetiredMealPrefs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwRetiredMealPrefs.PageIndex = e.NewPageIndex;
            BindgvwRetiredMealPrefs();
        }

        void BindgvwActiveCustPrefs()
        {
            //gvwActiveCustPrefs.DataSource = hccPreference.GetActive((int)Enums.PreferenceType.Customer);
            //gvwActiveCustPrefs.DataBind();
        }
        void gvwActiveCustPrefs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwActiveCustPrefs.PageIndex = e.NewPageIndex;
            BindgvwActiveCustPrefs();
        }

        void BindgvwRetiredCustPrefs()
        {

            //gvwRetiredCustPrefs.DataSource = hccPreference.GetRetired((int)Enums.PreferenceType.Customer);
            //gvwRetiredCustPrefs.DataBind();
        }
        void gvwRetiredCustPrefs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwRetiredCustPrefs.PageIndex = e.NewPageIndex;
            BindgvwRetiredCustPrefs();
        }
             
        void gvwPrefs_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView gvw = (GridView)sender;

            int prefId = int.Parse(gvw.SelectedDataKey.Value.ToString());
            PreferenceEdit1.PrimaryKeyIndex = prefId;
            PreferenceEdit1.Bind();

            btnAddNewPreference.Visible = false;
            pnlGrids.Visible = false;
            divEdit.Visible = true;
        }

        void btnAddNewPreference_Click(object sender, EventArgs e)
        {
            btnAddNewPreference.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;
        }

        void PreferenceAdd_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            PreferenceEdit1.Clear();
            BindGrids();

            btnAddNewPreference.Visible = true;
            pnlGrids.Visible = true;
            divEdit.Visible = false;
            lblFeedback.Text = string.Format("Preference saved: {0}", DateTime.Now);
        }

        void PreferenceAdd_ControlCancelled(object sender)
        {
            PreferenceEdit1.Clear();

            btnAddNewPreference.Visible = true;
            pnlGrids.Visible = true;
            divEdit.Visible = false;
        }
    }
}