using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;

namespace BayshoreSolutions.WebModules.Security.Login
{
    public partial class Edit : System.Web.UI.Page
    {
        int _parentInstanceId = 0;
        int _pageId = 0;
        int _instanceId = 0;
        int _moduleId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["ParentInstanceId"], out _parentInstanceId);
            int.TryParse(Request.QueryString["PageId"], out _pageId);
            int.TryParse(Request.QueryString["InstanceId"], out _instanceId);
            int.TryParse(Request.QueryString["ModuleId"], out _moduleId);

            if (!IsPostBack)
            {
                using (SqlConnection cxn = new SqlConnection(Settings.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("Security_LoginModule_get", cxn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ModuleId", _moduleId);
                    cmd.Connection.Open();
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        int loginPageId = 0;
                        int passwordRecoverPageId = 0;
                        while (r.Read())
                        {
                            int.TryParse(r["LoginPageId"].ToString(), out loginPageId);
                            int.TryParse(r["PasswordRecoveryPageId"].ToString(), out passwordRecoverPageId);
                        }
                        if (loginPageId > 0)
                            loginPage.SelectedNavigationId = loginPageId;
                        if (passwordRecoverPageId > 0)
                            PasswordRecoveryPage.SelectedNavigationId = passwordRecoverPageId;
                    }
                }
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            leavePage();
        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection cxn = new SqlConnection(Settings.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("Security_LoginModule_AddUpdate", cxn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ModuleId", _moduleId);
                cmd.Parameters.AddWithValue("@LoginPageId", this.loginPage.SelectedNavigationId);
                cmd.Parameters.AddWithValue("@PasswordRecoveryPageId", this.PasswordRecoveryPage.SelectedNavigationId);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }

            leavePage();
        }
        private void leavePage()
        {
            Response.Redirect("~/WebModules/Admin/MyWebsite/Default.aspx?instanceId=" + _instanceId);
        }
    }
}
