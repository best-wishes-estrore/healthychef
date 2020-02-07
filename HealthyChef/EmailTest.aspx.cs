using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;

namespace HealthyChef
{
    public partial class EmailTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var t = hccLedger.GetBalanceReportItems(DateTime.Parse("3/15/2013"));

            }
        }
    }
}