using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using HealthyChef.DAL;
using System.Data.Objects;
using System.Collections.Generic;

namespace BayshoreSolutions.WebModules.Cms.EventLog
{
    public partial class _default : System.Web.UI.Page
    {
        public string EventId
        {
            get { return (string)(ViewState["EventId"] ?? String.Empty); }
            set { ViewState["EventId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (null == Request.QueryString["EventId"]) LoadList();
                else
                {
                    this.EventId = Request.QueryString["EventId"];
                    LoadDetails(this.EventId);
                }
            }
        }

        private void LoadList()
        {
            uxEventLogList.DataSource = GetEvent(String.Empty);
            uxEventLogList.DataBind();

            uxListHelp.Visible = true;
            uxDetailsMenu.Visible = false;
        }

        private void LoadDetails(string eventId)
        {
            uxDetailsView.DataSource = GetEvent(eventId);
            uxDetailsView.DataBind();

            uxListHelp.Visible = false;
            uxDetailsMenu.Visible = true;
        }

        private List<aspnet_WebEvent_Get_Result> GetEvent(string eventId)
        {
            List<aspnet_WebEvent_Get_Result> retVals = new List<aspnet_WebEvent_Get_Result>();

            using (healthychefEntities hccEnt = new healthychefEntities())
            {
                var t = hccEnt.aspnet_WebEvent_Get(eventId);

                foreach (aspnet_WebEvent_Get_Result ret in t)
                {
                    retVals.Add(ret);
                }
            }

            return retVals.OrderByDescending(a=>a.EventTime).ToList();
        }

        private void DeleteEvent(string eventId)
        {
            using (SqlConnection cxn = new SqlConnection(global::BayshoreSolutions.WebModules.Settings.ConnectionString))
            using (SqlCommand cmd = cxn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "aspnet_WebEvent_Delete";
                cmd.Parameters.AddWithValue("eventId", eventId);
                cxn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        protected void uxEventLogList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string eventId = (string)uxEventLogList.DataKeys[e.RowIndex].Value;
            DeleteEvent(eventId);
            LoadList();
        }

        protected void uxEventLogList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            uxEventLogList.PageIndex = e.NewPageIndex;
            LoadList();
        }
    }
}
