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
    public partial class PlanManager : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddNewPlan.Click += new EventHandler(btnAddNewPlan_Click);

            PlanEdit1.ControlCancelled += new ControlCancelledEventHandler(PlanEdit1_ControlCancelled);
            PlanEdit1.ControlSaved += new ControlSavedEventHandler(PlanEdit1_ControlSaved);

            //gvwPlansActive.PageIndexChanging += new GridViewPageEventHandler(gvwPlansActive_PageIndexChanging);
            //gvwPlansActive.RowCreated += new GridViewRowEventHandler(gvwPlansActive_RowCreated);
            //gvwPlansActive.RowDataBound += new GridViewRowEventHandler(gvwPlansActive_RowDataBound);
            //gvwPlansActive.SelectedIndexChanged += new EventHandler(gvwPlansActive_SelectedIndexChanged);
            //gvwPlansActive.RowDeleting += new GridViewDeleteEventHandler(gvwPlansActive_RowDeleting);

            //gvwPlansRetired.PageIndexChanging += new GridViewPageEventHandler(gvwPlansRetired_PageIndexChanging);
            //gvwPlansRetired.RowDataBound += new GridViewRowEventHandler(gvwPlansRetired_RowDataBound);
            //gvwPlansRetired.RowCreated += new GridViewRowEventHandler(gvwPlansRetired_RowCreated);
            //gvwPlansRetired.SelectedIndexChanged += new EventHandler(gvwPlansRetired_SelectedIndexChanged);
            //gvwPlansRetired.RowDeleting += new GridViewDeleteEventHandler(gvwPlansRetired_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["pid"] != null)
                {
                    try
                    {
                        int planId = int.Parse(Request.QueryString["pid"].ToString());
                        PlanEdit1.PrimaryKeyIndex = planId;
                        PlanEdit1.Bind();

                        btnAddNewPlan.Visible = false;
                        pnlEditPlan.Visible = true;
                        pnlPlanLists.Visible = false;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                //else
                //    BindBothLists();
            }
        }

        void BindBothLists()
        {
            BindgvwPlansActive();
            BindgvwPlansRetired();
        }

        void btnAddNewPlan_Click(object sender, EventArgs e)
        {
            btnAddNewPlan.Visible = false;
            pnlEditPlan.Visible = true;
            pnlPlanLists.Visible = false;

            PlanEdit1.Clear();
        }

        void BindgvwPlansActive()
        {
            var plans = hccProgramPlan.GetBy(true);

            //gvwPlansActive.DataSource = plans;
            //gvwPlansActive.DataBind();
        }

        void gvwPlansActive_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int menuItemId = int.Parse(e.Keys[0].ToString());
                hccProgramPlan del = hccProgramPlan.GetById(menuItemId);

                if (del != null)
                {
                    del.Retire(true);
                    BindBothLists();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwPlansActive_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                hccProgramPlan plan = (hccProgramPlan)e.Row.DataItem;

                if (plan != null)
                {
                    hccProgram prog = hccProgram.GetById(plan.ProgramID);

                    if (prog != null)
                        e.Row.Cells[4].Text = prog.Name;
                }
            }
        }

        void gvwPlansActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int menuItemId = int.Parse(gvwPlansActive.SelectedDataKey.Value.ToString());

                //PlanEdit1.PrimaryKeyIndex = menuItemId;
                //PlanEdit1.Bind();

                //btnAddNewPlan.Visible = false;
                //pnlEditPlan.Visible = true;
                //pnlPlanLists.Visible = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwPlansActive_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lkbDelete = e.Row.Cells[2].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Retire");

                if (lkbDelete != null)
                    lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to retire this Plan?');");
            }
        }

        void gvwPlansActive_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           // gvwPlansActive.PageIndex = e.NewPageIndex;
            BindgvwPlansActive();
        }

        void BindgvwPlansRetired()
        {
            var Plans = hccProgramPlan.GetBy(false);

            //gvwPlansRetired.DataSource = Plans;
            //gvwPlansRetired.DataBind();
        }

        void gvwPlansRetired_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int progId = int.Parse(e.Keys[0].ToString());
                hccProgramPlan delItem = hccProgramPlan.GetById(progId);

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

        void gvwPlansRetired_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                hccProgramPlan plan = (hccProgramPlan)e.Row.DataItem;

                if (plan != null)
                {
                    hccProgram prog = hccProgram.GetById(plan.ProgramID);

                    if (prog != null)
                        e.Row.Cells[4].Text = prog.Name;
                }
            }
        }

        void gvwPlansRetired_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int menuItemId = int.Parse(gvwPlansRetired.SelectedDataKey.Value.ToString());

                //PlanEdit1.PrimaryKeyIndex = menuItemId;
                //PlanEdit1.Bind();

                //btnAddNewPlan.Visible = false;
                //pnlEditPlan.Visible = true;
                //pnlPlanLists.Visible = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        void gvwPlansRetired_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lkbDelete = e.Row.Cells[2].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Reactivate");

                if (lkbDelete != null)
                    lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to reactivate this Plan?');");
            }
        }

        void gvwPlansRetired_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwPlansRetired.PageIndex = e.NewPageIndex;
            BindgvwPlansActive();
        }

        protected void PlanEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            lblFeedback.Text = "Item saved: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            btnAddNewPlan.Visible = true;
            pnlEditPlan.Visible = false;
            pnlPlanLists.Visible = true;

            PlanEdit1.Clear();

            BindBothLists();
        }

        protected void PlanEdit1_ControlCancelled(object sender)
        {
            btnAddNewPlan.Visible = true;
            pnlEditPlan.Visible = false;
            pnlPlanLists.Visible = true;

            PlanEdit1.Clear();

            if (Request.QueryString["pid"] != null)
                BindBothLists();
        }
    }
}