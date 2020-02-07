using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Data.Sql;

namespace BayshoreSolutions.WebModules.SitemapXml
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadSettings();
                EnableDisableControls();
            }
        }

        protected bool IsSet(string strValue)
        {
            bool bSet = false;
            if (string.Compare(strValue, "1") == 0)
            {
                bSet = true;
            }
            else
            {
                if (string.Compare(strValue, "true", true) == 0)
                {
                    bSet = true;
                }
            }

            return bSet;
        }

        protected void LoadSettings()
        {
            BayshoreSolutions.WebModules.WebpageInfo webpageInfo = new BayshoreSolutions.WebModules.WebpageInfo();
            BayshoreSolutions.WebModules.SitemapXmlSettings settings = new SitemapXmlSettings();
            SitemapXmlHelper.LoadSettings(out settings, true);

            checkGenerateSitemap.Checked = settings.Generate;
            txtFileName.Text = settings.FileName;
            webpageInfo.SitemapXml_IncludePage = settings.DefaultIncludePage;
            webpageInfo.SitemapXml_IncludeLastmod = settings.DefaultIncludeLastMod;
            webpageInfo.SitemapXml_LastModified = settings.DefaultLastMod;
            webpageInfo.SitemapXml_IncludeChangefreq = settings.DefaultIncludeChangefreq;
            webpageInfo.SitemapXml_Changefreq = settings.DefaultChangefreq;
            webpageInfo.SitemapXml_IncludePriority = settings.DefaultIncludePriority;
            webpageInfo.SitemapXml_Priority = settings.DefaultPriority;

            sitemapXmlEdit.WebpageInfo = webpageInfo;
        }

        protected void checkGenerateSitemap_CheckedChanged(object sender, EventArgs args)
        {
            EnableDisableControls();
        }

        protected void EnableDisableControls()
        {
            bool bEnabled = checkGenerateSitemap.Checked;

            labelFileName.Enabled = bEnabled;
            txtFileName.Enabled = bEnabled;
            labelDefaultSettings.Enabled = bEnabled;
            sitemapXmlEdit.Enabled = bEnabled;
        }

        protected void btnSave_Click(object sender, EventArgs args)
        {
            if (IsValid)
            {
                try
                {
                    bool bGenerate = checkGenerateSitemap.Checked;
                    string strFileName = txtFileName.Text;
                    if (string.IsNullOrEmpty(strFileName))
                    {
                        strFileName = Page.ResolveUrl("~/sitemap.xml");
                    }
                    if (strFileName[0] != '/')
                    {
                        strFileName = string.Format("/{0}", strFileName);
                    }

                    bool bSitemapXmlIncludePage;
                    bool bSitemapXmlIncludeLastMod;
                    DateTime? dtSitemapXmlLastmod;
                    bool bSitemapXmlIncludeChangeFreg;
                    string strSitemapXmlChangeFreq;
                    bool bSitemapXmlIncludePriority;
                    decimal dSitemapXmlPriority;
                    sitemapXmlEdit.GetSettings(
                        out bSitemapXmlIncludePage,
                        out bSitemapXmlIncludeLastMod,
                        out dtSitemapXmlLastmod,
                        out bSitemapXmlIncludeChangeFreg,
                        out strSitemapXmlChangeFreq,
                        out bSitemapXmlIncludePriority,
                        out dSitemapXmlPriority);

                    SitemapXmlSettings settings = new SitemapXmlSettings();
                    settings.Generate = bGenerate;
                    settings.FileName = strFileName;
                    settings.DefaultIncludePage = bSitemapXmlIncludePage;
                    settings.DefaultIncludeLastMod = bSitemapXmlIncludeLastMod;
                    settings.DefaultLastMod = dtSitemapXmlLastmod;
                    settings.DefaultIncludeChangefreq = bSitemapXmlIncludeChangeFreg;
                    settings.DefaultChangefreq = strSitemapXmlChangeFreq;
                    settings.DefaultIncludePriority = bSitemapXmlIncludePriority;
                    settings.DefaultPriority = dSitemapXmlPriority;
                    SitemapXmlHelper.SaveSettings(settings);

                    messageBox.ShowSuccess("Sitemap settings updated.");
                }
                catch (Exception ex)
                {
                    string strMessage = "Failed saving sitemap settings";
                    BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(strMessage, this, ex);
                    messageBox.ShowError(strMessage);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs args)
        {
            Response.Redirect("~/WebModules/Admin/WebsiteSettings/Default.aspx");
        }

        protected void btnAddAll_Click(object sender, EventArgs args)
        {
            if (IsValid)
            {
                bool bSitemapXmlIncludePage;
                bool bSitemapXmlIncludeLastMod;
                DateTime? dtSitemapXmlLastmod;
                bool bSitemapXmlIncludeChangeFreg;
                string strSitemapXmlChangeFreq;
                bool bSitemapXmlIncludePriority;
                decimal dSitemapXmlPriority;
                sitemapXmlEdit.GetSettings(
                    out bSitemapXmlIncludePage,
                    out bSitemapXmlIncludeLastMod,
                    out dtSitemapXmlLastmod,
                    out bSitemapXmlIncludeChangeFreg,
                    out strSitemapXmlChangeFreq,
                    out bSitemapXmlIncludePriority,
                    out dSitemapXmlPriority);

                if (BayshoreSolutions.WebModules.SitemapXmlHelper.BulkUpdateXmlSitemap(
                        bSitemapXmlIncludePage,
                        bSitemapXmlIncludeLastMod,
                        dtSitemapXmlLastmod,
                        bSitemapXmlIncludeChangeFreg,
                        strSitemapXmlChangeFreq,
                        bSitemapXmlIncludePriority,
                        dSitemapXmlPriority,
                        true))
                {
                    messageBox.ShowSuccess("Sitemap updated successfully.");

                }
                else
                {
                    messageBox.ShowError("An error occurred while updating xml sitmap.");
                }
            }
        }

        protected void btnRemoveAll_Click(object sender, EventArgs args)
        {
            if (BayshoreSolutions.WebModules.SitemapXmlHelper.BulkUpdateXmlSitemap(
                          false,
                          false,
                          null,
                          false,
                          null,
                          false,
                          0.5M,
                          false))
            {
                messageBox.ShowSuccess("Sitemap updated successfully.");
            }
            else
            {
                messageBox.ShowError("An error occurred while updating xml sitmap.");
            }
        }
    }
}
