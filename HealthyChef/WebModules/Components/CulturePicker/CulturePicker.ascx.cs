using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Collections.Generic;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Components.CulturePicker
{
    public partial class CulturePicker : System.Web.UI.UserControl
    {
        public void Init_()
        {
            BadLanguage.Visible = false;
            string currentCultureCode = CultureCode.Current;
            List<CultureCode> activeCultures = CultureCode.Find(null, null, true, null);
            //remove aliased cultures
            activeCultures = activeCultures.FindAll(
                delegate(CultureCode c) { return string.IsNullOrEmpty(c.AliasToCultureCode); });

            if (null != activeCultures && activeCultures.Count > 1)
            { //the system is configured for multiple cultures.
                this.Visible = true;

                //CultureLinks.DataSource = activeCultures;
                //CultureLinks.DataBind();

                CultureSelect.Items.Clear();
                foreach (CultureCode c in activeCultures)
                {
                    CultureInfo ci = CultureInfo.CreateSpecificCulture(c.Name);
                    ListItem li = new ListItem(ci.NativeName, c.Name);
                    li.Selected = string.Equals(CultureCode.Current, c.Name, StringComparison.OrdinalIgnoreCase);
                    CultureSelect.Items.Add(li);
                }

                BadLanguage.Visible = !activeCultures.Exists(delegate(CultureCode c)
                    { return c.Name.ToLower() == currentCultureCode.ToLower(); });
            }
            else //the system is not configured for multiple cultures.
            {
                this.Visible = false;
            }
        }

        /*
        protected void CultureLinks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                Response.Cookies.Set(new HttpCookie("Culture", e.CommandArgument.ToString()));
                //force the page to re-load using the newly-selected culture.
                Response.Redirect(Request.RawUrl);
            }
        }
        */

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Init_();
            }
        }

        protected void CultureSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            CultureCode.SetCookieCulture(CultureSelect.Text);

            //force the page to re-load using the newly-selected culture.
            Response.Redirect(Request.RawUrl);
        }
    }
}