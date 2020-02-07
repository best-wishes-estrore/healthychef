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
    public partial class ProgramManager : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddNewProgram.Click += new EventHandler(btnAddNewProgram_Click);

            ProgramEdit1.ControlCancelled += new ControlCancelledEventHandler(ProgramEdit1_ControlCancelled);
            ProgramEdit1.ControlSaved += new ControlSavedEventHandler(ProgramEdit1_ControlSaved);

            //gvwProgramsActive.PageIndexChanging += new GridViewPageEventHandler(gvwProgramsActive_PageIndexChanging);
            //gvwProgramsActive.RowCreated += new GridViewRowEventHandler(gvwProgramsActive_RowCreated);
            //gvwProgramsActive.SelectedIndexChanged += new EventHandler(gvwProgramsActive_SelectedIndexChanged);
            //gvwProgramsActive.RowDeleting += new GridViewDeleteEventHandler(gvwProgramsActive_RowDeleting);

            //gvwProgramsRetired.PageIndexChanging += new GridViewPageEventHandler(gvwProgramsRetired_PageIndexChanging);
            //gvwProgramsRetired.RowCreated += new GridViewRowEventHandler(gvwProgramsRetired_RowCreated);
            //gvwProgramsRetired.SelectedIndexChanged += new EventHandler(gvwProgramsRetired_SelectedIndexChanged);
            //gvwProgramsRetired.RowDeleting += new GridViewDeleteEventHandler(gvwProgramsRetired_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int ProgramID = 0;
            if (Request.QueryString.AllKeys.Contains("ProgramID"))
            {
                ProgramID = int.Parse(Request.QueryString["ProgramID"]);
            }
            if (ProgramID != 0 && ProgramEdit1.PrimaryKeyIndex == 0)
            {
                ProgramEdit1.PrimaryKeyIndex = ProgramID;
                ProgramEdit1.Bind();

                btnAddNewProgram.Visible = false;
                pnlEditProgram.Visible = true;
                pnlProgramLists.Visible = false;
            }
            else if(!IsPostBack)
            {
                //BindBothLists();
            }
        }

        void BindBothLists()
        {
            BindgvwProgramsActive();
            BindgvwProgramsRetired();
        }

        void btnAddNewProgram_Click(object sender, EventArgs e)
        {
            btnAddNewProgram.Visible = false;
            pnlEditProgram.Visible = true;
            pnlProgramLists.Visible = false;

            ProgramEdit1.Clear();
        }

        void BindgvwProgramsActive()
        {
            var programs = hccProgram.GetBy(true);
            
            //gvwProgramsActive.DataSource = programs;
            //gvwProgramsActive.DataBind();
        }

        void gvwProgramsActive_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int menuItemId = int.Parse(e.Keys[0].ToString());
                hccProgram del = hccProgram.GetById(menuItemId);

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

        void gvwProgramsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int menuItemId = int.Parse(gvwProgramsActive.SelectedDataKey.Value.ToString());

                //ProgramEdit1.PrimaryKeyIndex = menuItemId;
                //ProgramEdit1.Bind();

                //btnAddNewProgram.Visible = false;
                //pnlEditProgram.Visible = true;
                //pnlProgramLists.Visible = false;
            }
            catch (Exception)
            {
                throw;
            }       
        }

        void gvwProgramsActive_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lkbDelete = e.Row.Cells[2].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Retire");

                if (lkbDelete != null)
                    lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to retire this program?');");
            }
        }

        void gvwProgramsActive_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           // gvwProgramsActive.PageIndex = e.NewPageIndex;
            BindgvwProgramsActive();
        }

        void BindgvwProgramsRetired()
        {
            var programs = hccProgram.GetBy(false);

            //gvwProgramsRetired.DataSource = programs;
            //gvwProgramsRetired.DataBind();
        }

        void gvwProgramsRetired_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int progId = int.Parse(e.Keys[0].ToString());
                hccProgram delItem = hccProgram.GetById(progId);

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

        void gvwProgramsRetired_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //int menuItemId = int.Parse(gvwProgramsRetired.SelectedDataKey.Value.ToString());

                //ProgramEdit1.PrimaryKeyIndex = menuItemId;
                //ProgramEdit1.Bind();

                //btnAddNewProgram.Visible = false;
                //pnlEditProgram.Visible = true;
                //pnlProgramLists.Visible = false;
            }
            catch (Exception)
            {
                throw;
            }       
        }

        void gvwProgramsRetired_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lkbDelete = e.Row.Cells[2].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Reactivate");

                if (lkbDelete != null)
                    lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to reactivate this program?');");
            }
        }

        void gvwProgramsRetired_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwProgramsRetired.PageIndex = e.NewPageIndex;
            BindgvwProgramsActive();
        }
        
        protected void ProgramEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            lblFeedback.Text = "Item saved: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            btnAddNewProgram.Visible = true;
            pnlEditProgram.Visible = false;
            pnlProgramLists.Visible = true;

            ProgramEdit1.Clear();

            BindBothLists();
        }

        protected void ProgramEdit1_ControlCancelled(object sender)
        {
            btnAddNewProgram.Visible = true;
            pnlEditProgram.Visible = false;
            pnlProgramLists.Visible = true;

            ProgramEdit1.Clear();
        }

        

        //protected void lvwProgramsActive_DataBound(object sender, EventArgs e)
        //{
        //    int start = pgrProgramsActive1.StartRowIndex + 1;
        //    int end = pgrProgramsActive1.StartRowIndex + pgrProgramsActive1.PageSize;
        //    int total = pgrProgramsActive1.TotalRowCount;

        //    if (total > 0)
        //    {
        //        if (end > total)
        //            lblRecordCountActive.Text = string.Format("{0} - {1} of {2}", start.ToString(), total.ToString(), total.ToString());
        //        else
        //            lblRecordCountActive.Text = string.Format("{0} - {1} of {2}", start.ToString(), end.ToString(), total.ToString());
        //    }
        //}

        //protected void lvwProgramsActive_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    Program_Edit pe = (Program_Edit)e.Item.FindControl("ProgramEdit1");
        //    pe.SetFormFieldValidationGroup("ProgramEditGroup" + pe.PrimaryKeyIndex);
        //    pe.Bind();
        //}

        //protected void lvwProgramsActive_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    pgrProgramsActive1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        //    BindlvwProgramsActive();
        //}

        //protected void lvwProgramsRetired_DataBound(object sender, EventArgs e)
        //{
        //    int start = pgrProgramsRetired1.StartRowIndex + 1;
        //    int end = pgrProgramsRetired1.StartRowIndex + pgrProgramsRetired1.PageSize;
        //    int total = pgrProgramsRetired1.TotalRowCount;

        //    if (total > 0)
        //    {
        //        if (end > total)
        //            lblRecordCountRetired.Text = string.Format("{0} - {1} of {2}", start.ToString(), total.ToString(), total.ToString());
        //        else
        //            lblRecordCountRetired.Text = string.Format("{0} - {1} of {2}", start.ToString(), end.ToString(), total.ToString());
        //    }
        //}

        //protected void lvwProgramsRetired_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    Program_Edit pe = (Program_Edit)e.Item.FindControl("ProgramEdit1");
        //    pe.SetFormFieldValidationGroup("ProgramEditGroup" + pe.PrimaryKeyIndex);
        //    pe.Bind();
        //}

        //protected void lvwProgramsRetired_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        //{
        //    pgrProgramsRetired1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
        //    BindlvwProgramsRetired();
        //}
    }
}