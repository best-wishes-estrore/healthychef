using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using HealthyChef.Common;
using Microsoft.Reporting.WebForms;


namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class WeekAtGlanceReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hccProductionCalendar nextProdCal = hccProductionCalendar.GetNext4Calendars().FirstOrDefault();

                txtStartDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
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

                List<ChefProdItem> list = hccMenuItem.GetWaaGItems(searchStart.Value, searchEnd.Value);

                try
                {   
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/WaaG1.rdlc");

                    ReportDataSource rds1 = new ReportDataSource("WaaG1", list);
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

    public class GroupItemHolder
    {
        public int Count { get; set; }
        public Enums.MealTypes MealType { get; set; }
        public List<Tuple<string, int, int>> NameCountList = new List<Tuple<string, int, int>>(); // ItemName, ItemInstances, GroupTotalCount

        public GroupItemHolder()
        { }
    }
}

//protected string GetMealHeader()
//{
//    var iItemName = (string)Eval("ItemName");

//    if (iItemName == "GroupBreak")
//        return string.Format(
//                "<tr><td colspan='2' style='text-align:left;font-size:14px;font-weight:bold;margin-top:15px;border:none;'>{0}</td>" +
//                    "<td style='text-align:right;font-size:14px;font-weight:bold;margin-top:15px;border:none;'>{1}</td></tr>",
//                Eval("MealTypeDesc").ToString(), Eval("Quantity").ToString());
//    else
//        return string.Empty;
//}

//protected string GetWeekAtGlanceDate()
//{
//    var iItemTypeId = (int)Eval("MealTypeID");

//    if (iItemTypeId == -200)
//    {
//        var dDeliveryDate = (DateTime)Eval("DeliveryDate");
//        return string.Format(
//                "<tr><td colspan='3' style='text-align:left;font-size:14px;font-weight:bold;margin-top:15px;border:none;background-color:silver;'>{0}</td></tr>",
//                dDeliveryDate.ToString("dddd, MMMM dd, yyyy"));
//    }
//    else
//        return string.Empty;
//}

//protected string GetWeekAtGlanceItem()
//{
//    var iItemTypeId = (int)Eval("MealTypeID");
//    var iItemName = (string)Eval("ItemName");


//    if (iItemTypeId != -200 && iItemName != "GroupBreak")
//    {
//        _i++;

//        if (_i % 2 == 0)
//        {
//            return string.Format(
//                "<tr><td style='border:none;'></td><td style='text-align:left;font-size:12px;border:none;'>{0}</td><td style='text-align:right;font-size:12px;border:none;'>{1}</td></tr>",
//                Eval("ItemName").ToString(), Eval("Quantity").ToString());
//        }
//        else
//        {
//            return string.Format(
//                "<tr style='background-color: #DDDDDD;'><td style='border:none;'></td><td style='text-align:left;font-size:12px;border:none;'>{0}</td><td style='text-align:right;font-size:12px;border:none;'>{1}</td></tr>",
//                Eval("ItemName").ToString(), Eval("Quantity").ToString());
//        }
//    }
//    else
//        return string.Empty;


//}