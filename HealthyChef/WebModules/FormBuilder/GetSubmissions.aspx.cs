using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Bss = BayshoreSolutions.Common;
using cms = BayshoreSolutions.WebModules.Cms;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
namespace BayshoreSolutions.WebModules.FormBuilder
{
    public partial class GetSubmissions : System.Web.UI.Page
    {
        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }
        protected string SortBy
        {
            get
            {
                if (ViewState["ContactUs::SortField"] == null)
                    ViewState["ContactUs::SortField"] = "CreatedOn";
                return ViewState["ContactUs::SortField"].ToString();
            }
            set
            {
                ViewState["ContactUs::SortField"] = value;
            }

        }
        protected string SortDirection
        {
            get
            {
                if (ViewState["ContactUs::SortDirection"] == null)
                    return "DESC";
                return ViewState["ContactUs::SortDirection"].ToString();
            }
            set
            {
                ViewState["ContactUs::SortDirection"] = value;

            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            InitModule();
            AddStyleSheetToHeader();
            if (!IsPostBack)
            {
                ShowList();
            }

        }
        private void InitModule()
        {
            int moduleId = 0;
            int.TryParse(Request["ModuleId"], out moduleId);
            this.ModuleId = moduleId;

            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            WebpageInfo page = null;
            if (this.ModuleId <= 0
                || null == module
                || null == (page = module.Webpage))
            {
                cms.Admin.RedirectToMainMenu();
            }
            Page.Title = module.WebModuleType.Name + " Module";

            if (string.IsNullOrEmpty(txtStartDate.Text))
            {
                txtStartDate.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            }
            if (string.IsNullOrEmpty(txtEndDate.Text))
            {
                txtEndDate.Text = DateTime.Now.ToShortDateString();
            }
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowList();

        }
        protected void btnReturnToList_Click(object sender, EventArgs e)
        {
            mvwSubmission.SetActiveView(vwList);

        }

        private void ShowList()
        {
            DataTable table = GetDataTable();
            if (table != null)
            {
                BuildGridView(table);
                grdSubmissions.DataKeyNames = new string[] { "ResponseId" };
                grdSubmissions.DataSource = table;
            }
            grdSubmissions.DataBind();
        }

        private DataTable GetDataTable()
        {
            DataTable dtResults = null;

            DateTime dtStartDate = Convert.ToDateTime(txtStartDate.Text);
            DateTime dtEndDate = Convert.ToDateTime(txtEndDate.Text).AddDays(1);
            FormBuilderDataContext dc = new FormBuilderDataContext();

            List<FormBuilder_FieldInput> lFieldInputs = (from fi in dc.FormBuilder_FieldInputs
                                                         from r in dc.FormBuilder_Responses
                                                         where r.ModuleId == this.ModuleId
                                                         where r.CreatedOn >= dtStartDate
                                                         where r.CreatedOn <= dtEndDate
                                                         where fi.ResponseId == r.Id
                                                         orderby fi.FormBuilder_Field.SortOrder
                                                         select fi).ToList();
            if (lFieldInputs.Count > 0)
            {
                DataTable dtFieldInputs = FormBuilder_Response.CreateDataTable(lFieldInputs, true);
                if (dtFieldInputs != null)
                {
                    dtResults = new DataTable();
                    foreach (DataColumn c in dtFieldInputs.Columns)
                    {
                        dtResults.Columns.Add(new DataColumn(c.ColumnName, c.DataType));
                    }

                    var referringUrl = dtResults.Columns.Add();
                    referringUrl.AllowDBNull = true;
                    referringUrl.ColumnName = "ReferringUrl";
                    referringUrl.DataType = typeof(string);

                    var landingUrl = dtResults.Columns.Add();
                    landingUrl.AllowDBNull = true;
                    landingUrl.ColumnName = "LandingUrl";
                    landingUrl.DataType = typeof(string);

                    var referrer = dtResults.Columns.Add();
                    referrer.AllowDBNull = true;
                    referrer.ColumnName = "ReferringDomain";
                    referrer.DataType = typeof(string);

                    var keywords = dtResults.Columns.Add();
                    keywords.AllowDBNull = true;
                    keywords.ColumnName = "Keywords";
                    keywords.DataType = typeof(string);

                    foreach (DataRow row in dtFieldInputs.Rows)
                    {
                        var newRow = dtResults.Rows.Add(row.ItemArray);

                        int responseId = Convert.ToInt32(row["ResponseId"]);
                        var responseReferrer = dc.FormBuilder_ResponseReferrers.Where(r => r.ResponseId == responseId).FirstOrDefault();
                        if (responseReferrer != null)
                        {
                            newRow["ReferringUrl"] = responseReferrer.referringUrl;
                            newRow["LandingUrl"] = responseReferrer.landingUrl;
                            newRow["ReferringDomain"] = responseReferrer.domain;
                            newRow["Keywords"] = responseReferrer.query;
                        }
                    }
                }
            }

            return dtResults;
        }

        private void BuildGridView(DataTable dt)
        {
            int? nMaxResponseDisplayColumns = null;
            int n;
            string strMaxResponseDisplayColumns = System.Configuration.ConfigurationManager.AppSettings["FormBuilder_MaxResponseDisplayColumns"];
            if (!string.IsNullOrEmpty(strMaxResponseDisplayColumns))
            {
                if (int.TryParse(strMaxResponseDisplayColumns, out n))
                {
                    nMaxResponseDisplayColumns = n;
                }
            }

            grdSubmissions.Columns.Clear();

            CommandField command = new CommandField();
            command.ShowSelectButton = true;
            command.SelectText = "Details";
            grdSubmissions.Columns.Add(command);

            int nColumns = 0;
            foreach (DataColumn column in dt.Columns)
            {
                BoundField col = new BoundField();
                col.DataField = column.ColumnName;
                col.HeaderText = GetHeaderText(column.ColumnName);

                col.SortExpression = column.ColumnName;
                grdSubmissions.Columns.Add(col);
                nColumns++;
                if (nMaxResponseDisplayColumns.HasValue)
                {
                    if (nColumns >= nMaxResponseDisplayColumns)
                    {
                        break;
                    }
                }
            }

            if ((!nMaxResponseDisplayColumns.HasValue) || (nColumns < nMaxResponseDisplayColumns.Value))
            {
                // add referrer domain and query
                BoundField refColumn = new BoundField();
                refColumn.HeaderText = "Referring Domain";
                refColumn.DataField = "ReferringDomain";
                grdSubmissions.Columns.Add(refColumn);
                nColumns++;
            }

            if ((!nMaxResponseDisplayColumns.HasValue) || (nColumns < nMaxResponseDisplayColumns.Value))
            {
                BoundField keywordsColumn = new BoundField();
                keywordsColumn.HeaderText = "Keywords";
                keywordsColumn.DataField = "Keywords";
                grdSubmissions.Columns.Add(keywordsColumn);
                nColumns++;
            }

            CommandField delcommand = new CommandField();
            delcommand.ShowDeleteButton = true;
            grdSubmissions.Columns.Add(delcommand);
        }

        private string GetHeaderText(string colName)
        {
            // Strip off the "_id" on the end of the DataTable column name to get the actual header
            if (colName.Contains('_'))
            {
                return colName.Substring(0, colName.LastIndexOf('_'));
            }
            return colName;
        }

        protected void bssExport_Click(object sender, EventArgs e)
        {
            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            bssExport.OutputFileName = string.Format("{0}-responses-{1:yyyy-MMM-dd}",
                Bss.BssString.Sanitize(module.Name, "_"),
                DateTime.Now);


            bssExport.DataSource = GetDataTable();
        }

        protected void grdSubmissions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort")
            {
                this.SortBy = e.CommandArgument.ToString();
                this.SortDirection = this.SortDirection == "ASC" ? "DESC" : "ASC";
                ShowList();
                return;
            }

            if (e.CommandName == "Select")
            {
                int rowNum = int.Parse(e.CommandArgument.ToString());
                int responseId = int.Parse(grdSubmissions.DataKeys[rowNum].Value.ToString());
                StringBuilder sb = new StringBuilder();
                Dictionary<string, string> values = FormBuilder_Response.GetResponseByResponseId(responseId);

                //DataRow row = dt.Rows[rowNum];
                sb.Append("<table class='submission-detail' cellpadding='5'>");
                foreach (string key in values.Keys)
                {
                    string strHeaderText = GetHeaderText(key);
                    string strValue = (string)values[key];

                    sb.AppendFormat("<tr><td>{0}:</td><td><b>{1}</b></td>", strHeaderText, strValue);
                }


                sb.Append("</table>");
                divDetail.InnerHtml = sb.ToString();
                mvwSubmission.SetActiveView(vwDetail);
            }

            if (e.CommandName == "Delete")
            {
                int rowNum = int.Parse(e.CommandArgument.ToString());
                int responseId = int.Parse(grdSubmissions.DataKeys[rowNum].Value.ToString());
                FormBuilderDataContext dc = new FormBuilderDataContext();
                FormBuilder_Response response = (from r in dc.FormBuilder_Responses
                                                 where r.Id == responseId
                                                 select r).FirstOrDefault();

                if (response != null)
                {
                    response.Id = responseId;
                    dc.FormBuilder_Responses.DeleteOnSubmit(response);
                    dc.SubmitChanges();
                }
                ShowList();
            }
        }

        protected void grdSubmissions_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void grdSubmissions_OnSelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void grdSubmissions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
        private void AddStyleSheetToHeader()
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/FormBuilder/public/css/FormBuilder.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            Header.Controls.AddAt(1, cssLink);
        }


    }
}
