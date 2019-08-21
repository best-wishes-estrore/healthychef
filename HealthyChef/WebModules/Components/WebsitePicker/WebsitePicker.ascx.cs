using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Components.WebsitePicker
{
    public partial class WebsitePicker : System.Web.UI.UserControl
    {
        public event System.EventHandler WebsiteSelected;

        private void EnsureDataBound()
        {
            if (WebsiteSelect.Items.Count == 0)
                LoadWebsitesSelect();
        }

        public int SelectedWebsiteId
        {
            get
            {
                EnsureDataBound();

                return int.Parse(WebsiteSelect.Text);
            }
            set
            {
                EnsureDataBound();

                WebsiteSelect.Text = value.ToString();
            }
        }

        private void OnWebsiteSelected(object sender, EventArgs e)
        {
            if (null != WebsiteSelected)
            {
                WebsiteSelected(sender, e);
            }
        }

        private void LoadWebsitesSelect()
        {
            WebsiteSelect.Items.Clear();
            foreach (Website website in Website.Get())
            {
                WebpageInfo rootPage = Webpage.GetWebpage(website.RootNavigationId);

                WebsiteSelect.Items.Add(
                    new ListItem(string.Format("{0} (root page: {1})",
                            website.Resource.Name,
                            rootPage.Text),
                        website.SiteId.ToString()));
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                EnsureDataBound();
            }
        }

        protected void WebsiteSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnWebsiteSelected(WebsiteSelect, e);
        }
    }
}
