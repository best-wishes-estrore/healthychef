﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using HealthyChef.Common;
using Microsoft.Reporting.WebForms;
using System.IO;
using System.Reflection;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class ChefProductionWorksheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hccProductionCalendar nextProdCal = hccProductionCalendar.GetNext4Calendars().FirstOrDefault();

                txtStartDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.AddDays(-1).ToShortDateString();
                txtEndDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
                ButtonRefresh_Click(this, new EventArgs());
            }
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            Page.Validate("MealOrderGroup");

            if (Page.IsValid)
            {
                DateTime startDate;
                bool tryStart = DateTime.TryParse(txtStartDate.Text.Trim(), out startDate);
                DateTime endDate;
                bool tryEnd = DateTime.TryParse(txtEndDate.Text.Trim(), out endDate);

                DateTime? searchStart = null;
                DateTime? searchEnd = null;

                if (tryStart)
                    searchStart = startDate;

                if (tryEnd)
                    searchEnd = endDate.AddDays(1);

                List<ChefProdItem> list = hccMenuItem.GetChefProdItems(searchStart.Value, searchEnd.Value);
             
                try
                {
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/ChefProd2.rdlc");
                                                      
                    ReportDataSource rds1 = new ReportDataSource("ChefProd2", list);
                    ReportViewer1.LocalReport.DataSources.Add(rds1);

                    this.ReportViewer1.LocalReport.Refresh();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }

    //public class ChefProdGroupItemHolder
    //{
    //    public int Count { get; set; }
    //    public Enums.MealTypes MealType { get; set; }
    //    public List<Tuple<string, int, int>> NameCountList = new List<Tuple<string, int, int>>(); // ItemName, ItemInstances, GroupTotalCount

    //    public ChefProdGroupItemHolder()
    //    { }
    //}
}