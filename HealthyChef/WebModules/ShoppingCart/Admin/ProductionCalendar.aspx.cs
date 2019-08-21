using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class ProductionCalendar : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnFind.Click += new EventHandler(btnFind_Click);
            btnAddNewProdCal.Click += new EventHandler(btnAddNewProdCal_Click);
            btnBatchNewProdCals.Click += btnBatchNewProdCals_Click;
            //ProdCalendar1.ControlCancelled += new ControlCancelledEventHandler(ProdCalendar1_ControlCancelled);
            //ProdCalendar1.ControlSaved += new ControlSavedEventHandler(ProdCalendar1_ControlSaved);

            //gvwProdCalendars.PageIndexChanging += new GridViewPageEventHandler(gvwProdCalendars_PageIndexChanging);
            //gvwProdCalendars.RowCreated += new GridViewRowEventHandler(gvwProdCalendars_RowCreated);
            //gvwProdCalendars.SelectedIndexChanged += new EventHandler(gvwProdCalendars_SelectedIndexChanged);
            //gvwProdCalendars.RowDeleting += new GridViewDeleteEventHandler(gvwProdCalendars_RowDeleting);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                string CalendarID = string.Empty;
                if (Request.QueryString.AllKeys.Contains("CalendarId"))
                {
                    CalendarID = Request.QueryString["CalendarId"].ToString();
                }
                if (!string.IsNullOrEmpty(CalendarID))
                {
                    CurrentCalendarId.Value = CalendarID;
                    int menuItemId = Convert.ToInt32(CalendarID);

                    ProdCalendar1.PrimaryKeyIndex = menuItemId;
                    ProdCalendar1.Bind();
                    btnAddNewProdCal.Visible = false;
                    pnlEditProdCal.Visible = true;
                    pnlProdCalList.Visible = false;
                   
                }

                //BindgvwProdCalendars();
            }

        }

        void btnAddNewProdCal_Click(object sender, EventArgs e)
        {
            btnAddNewProdCal.Visible = false;
            pnlEditProdCal.Visible = true;
            pnlProdCalList.Visible = false;

            ProdCalendar1.Clear();
        }

        protected void btnFind_Click(object sender, EventArgs e)
        {
            BindgvwProdCalendars();
        }

        void BindgvwProdCalendars()
        {
            DateTime startDate = !string.IsNullOrWhiteSpace(txtStartDate.Text.Trim()) ? DateTime.Parse(txtStartDate.Text.Trim()) : DateTime.MinValue;
            DateTime endDate = !string.IsNullOrWhiteSpace(txtEndDate.Text.Trim()) ? DateTime.Parse(txtEndDate.Text.Trim()) : DateTime.MaxValue;

            List<hccProductionCalendar> calendars = hccProductionCalendar.GetBy(startDate, endDate);
           
            var cals = calendars.Select(a => new
            {
                CalendarID = a.CalendarID,
                Name = a.Name,
                DeliveryDate = a.DeliveryDate,
                OrderCutOffDate = a.OrderCutOffDate,
                MenuName = (a.GetMenu() == null ? string.Empty : a.GetMenu().Name)
            });

            var futs = cals.Where(a => a.DeliveryDate >= DateTime.Now);
            var past = cals.Where(a => a.DeliveryDate < DateTime.Now);

            //gvwProdCalendars.DataSource = futs;
            //gvwProdCalendars.DataBind();

            //gvwPastCalendars.DataSource = past.OrderByDescending(a => a.DeliveryDate);
            //gvwPastCalendars.DataBind();
        }

        //protected void ProdCalendar1_ControlSaved(object sender, ControlSavedEventArgs e)
        //{
        //    lblFeedback.Text = "Calendar saved: " + DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
        //    btnAddNewProdCal.Visible = true;
        //    pnlEditProdCal.Visible = false;
        //    pnlProdCalList.Visible = true;

        //    ProdCalendar1.Clear();

        //    BindgvwProdCalendars();
        //}

        //protected void ProdCalendar1_ControlCancelled(object sender)
        //{
        //    btnAddNewProdCal.Visible = true;
        //    pnlEditProdCal.Visible = false;
        //    pnlProdCalList.Visible = true;

        //    ProdCalendar1.Clear();
        //}

        //void gvwProdCalendars_RowDeleting(object sender, GridViewDeleteEventArgs e)
        //{
        //    try
        //    {
        //        //int menuItemId = int.Parse(e.Keys[0].ToString());
        //        //hccProgram del = hccProgram.GetById(menuItemId);

        //        //if (del != null)
        //        //{
        //        //    del.Retire(true);
        //        //    BindgvwProdCalendars();
        //        //}
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //void gvwProdCalendars_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //int menuItemId = int.Parse(gvwProdCalendars.SelectedDataKey.Value.ToString());

        //        //ProdCalendar1.PrimaryKeyIndex = menuItemId;
        //        //ProdCalendar1.Bind();

        //        //btnAddNewProdCal.Visible = false;
        //        //pnlEditProdCal.Visible = true;
        //        //pnlProdCalList.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //void gvwProdCalendars_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    //    LinkButton lkbDelete = e.Row.Cells[2].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Retire");

        //    //    if (lkbDelete != null)
        //    //        lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to retire this calendar?');");
        //    //}
        //}

        //void gvwProdCalendars_PageIndexChanging(object sender, GridViewPageEventArgs e)
        //{
        //    //gvwProdCalendars.PageIndex = e.NewPageIndex;
        //    BindgvwProdCalendars();
        //}

        void btnBatchNewProdCals_Click(object sender, EventArgs e)
        {
            DateTime lastDelDate = hccProductionCalendar.GetAll().Max(a => a.DeliveryDate);
            DateTime endDate = lastDelDate.AddYears(1);
            List<DateTime> dates = new List<DateTime>();

            for (DateTime date = lastDelDate.AddDays(1); date <= endDate; date = date.AddDays(1))
            {
                if (date.DayOfWeek == DayOfWeek.Friday)
                    dates.Add(date);
            }

            dates.ForEach(delegate (DateTime date)
            {
                hccProductionCalendar CurrentProductionCalendar = new hccProductionCalendar();

                CurrentProductionCalendar.Name = "Delivery Date " + date.ToShortDateString();
                CurrentProductionCalendar.DeliveryDate = date;
                CurrentProductionCalendar.OrderCutOffDate = date.AddDays(-8);

                CurrentProductionCalendar.Save();
            });

            BindgvwProdCalendars();
        }
    }
}