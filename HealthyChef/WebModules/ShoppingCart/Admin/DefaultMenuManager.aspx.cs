using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class DefaultMenuManager : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ddlDelDates.SelectedIndexChanged += ddlDelDates_SelectedIndexChanged;
            ddlPrograms.SelectedIndexChanged += ddlPrograms_SelectedIndexChanged;
            btnGetPrograms.Click += btnGetPrograms_Click;
        }

        void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultMenuManager1.Visible)
            {
                DefaultMenuManager1.Clear();
                DefaultMenuManager1.Visible = false;
            }
        }

        void ddlDelDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DefaultMenuManager1.Visible)
            {
                DefaultMenuManager1.Clear();
                DefaultMenuManager1.Visible = false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindddlDelDates();
                BindddlPrograms();
            }
        }

        void BindddlDelDates()
        {
            if (ddlDelDates.Items.Count == 0)
            {
                ddlDelDates.DataSource = hccProductionCalendar.GetAll();
                ddlDelDates.DataTextField = "DeliveryDate";
                ddlDelDates.DataTextFormatString = "{0:MM/dd/yyyy}";
                ddlDelDates.DataValueField = "CalendarId";
                ddlDelDates.DataBind();

                try
                {
                    hccProductionCalendar next = hccProductionCalendar.GetNextCalendar();
                    ListItem li = ddlDelDates.Items.FindByValue(next.CalendarID.ToString());

                    if (li != null)
                        li.Selected = true;
                }
                catch (Exception)
                {

                    throw;
                }
                //ddlDelDates.Items.Insert(0, new ListItem("Select a Delivery Date...", "-1"));
            }
        }

        void BindddlPrograms()
        {
            var activePrograms = (from program in hccProgram.GetAll() where program.IsActive select program).ToList();
            if (activePrograms.Any())
            {
                ddlPrograms.DataSource = activePrograms;
                ddlPrograms.DataTextField = "Name";
                ddlPrograms.DataValueField = "ProgramId";
                ddlPrograms.DataBind();
            }
            else
            {
                ddlPrograms.Items.Insert(0, new ListItem("No active programs", "-1"));
            } 
        }

        void btnGetPrograms_Click(object sender, EventArgs e)
        {
            DefaultMenuManager1.CurrentCalendarId = int.Parse(ddlDelDates.SelectedValue);
            DefaultMenuManager1.CurrentProgramId = int.Parse(ddlPrograms.SelectedValue);
            DefaultMenuManager1.Bind();
            DefaultMenuManager1.Visible = true;
        }
    }
}