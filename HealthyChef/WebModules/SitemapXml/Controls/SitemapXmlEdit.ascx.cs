using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.SitemapXml.Controls
{
    public partial class SitemapXmlEdit : System.Web.UI.UserControl
    {
        protected BayshoreSolutions.WebModules.WebpageInfo _page = null;
        public BayshoreSolutions.WebModules.WebpageInfo WebpageInfo
        {
            get { return _page; }
            set { _page = value; }
        }

        public bool Enabled
        {
            get
            {
                bool? bEnabled = (bool?)ViewState["Enabled"];
                return (bEnabled.HasValue && bEnabled.Value) || (!bEnabled.HasValue);
            }

            set
            {
                ViewState["Enabled"] = value;
                EnableDisableSitemapControls();
            }
        }



        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // SitemapXml settings
                checkSitemapXmlIncludePage.Checked = _page.SitemapXml_IncludePage;
                if (_page.SitemapXml_IncludePage)
                {
                    checkSitemapXmlIncludeLastMod.Checked = _page.SitemapXml_IncludeLastmod;

                    string strLastMod = null;
                    if (checkSitemapXmlIncludeLastMod.Checked)
                    {
                        if (_page.SitemapXml_LastModified.HasValue)
                        {
                            strLastMod = _page.SitemapXml_LastModified.Value.ToShortDateString();
                            if (_page.SitemapXml_LastModified.Value.TimeOfDay.TotalSeconds > 0)
                            {
                                strLastMod += " " + _page.SitemapXml_LastModified.Value.ToShortTimeString();
                            }
                        }
                    }
                    txtSitemapXmlLastMod.Text = strLastMod;

                    checkSitemapXmlIncludeChangeFreq.Checked = _page.SitemapXml_IncludeChangefreq;
                    if (checkSitemapXmlIncludeChangeFreq.Checked)
                    {
                        string strChangeFreq = _page.SitemapXml_Changefreq;
                        if (!string.IsNullOrEmpty(strChangeFreq))
                        {
                            ListItem liChangeFreq = ddlChangeFreq.Items.FindByValue(strChangeFreq);
                            if (liChangeFreq != null)
                            {
                                ddlChangeFreq.ClearSelection();
                                liChangeFreq.Selected = true;
                            }
                        }
                    }

                    checkSitemapXmlIncludePriority.Checked = _page.SitemapXml_IncludePriority;
                    if (checkSitemapXmlIncludePriority.Checked)
                    {
                        decimal dPriority = _page.SitemapXml_Priority;
                        if (dPriority >= 1)
                        {
                            ddlSitemapXmlPriorityOnes.SelectedValue = "1";
                            ddlSitemapXmlPriorityTenths.SelectedValue = "0";
                        }
                        else
                        {
                            ddlSitemapXmlPriorityOnes.SelectedValue = "0";
                            ddlSitemapXmlPriorityTenths.SelectedValue = ((int)(dPriority * 10M)).ToString();
                        }
                    }
                }
                EnableDisableSitemapControls();
            }
        }

        public void GetSettings(
            out bool bSitemapXmlIncludePage,
            out bool bSitemapXmlIncludeLastMod,
            out DateTime? dtSitemapXmlLastmod,
            out bool bSitemapXmlIncludeChangeFreg,
            out string strSitemapXmlChangeFreq,
            out bool bSitemapXmlIncludePriority,
            out decimal dSitemapXmlPriority)
        {
            bSitemapXmlIncludePage = checkSitemapXmlIncludePage.Checked;
            bSitemapXmlIncludeLastMod = false;
            dtSitemapXmlLastmod = null;
            bSitemapXmlIncludeChangeFreg = true;
            strSitemapXmlChangeFreq = null;
            bSitemapXmlIncludePriority = true;
            dSitemapXmlPriority = 0.5M;

            if (bSitemapXmlIncludePage)
            {
                bSitemapXmlIncludeLastMod = checkSitemapXmlIncludeLastMod.Checked;
                if (bSitemapXmlIncludeLastMod)
                {
                    string strXmlSitemapLastmod = txtSitemapXmlLastMod.Text.Trim();
                    if (!string.IsNullOrEmpty(strXmlSitemapLastmod))
                    {
                        DateTime dt;
                        if (DateTime.TryParse(strXmlSitemapLastmod, out dt))
                        {
                            dtSitemapXmlLastmod = dt;
                        }
                    }
                }

                bSitemapXmlIncludeChangeFreg = checkSitemapXmlIncludeChangeFreq.Checked;
                if (bSitemapXmlIncludeChangeFreg)
                {
                    strSitemapXmlChangeFreq = ddlChangeFreq.SelectedValue;
                }

                bSitemapXmlIncludePriority = checkSitemapXmlIncludePriority.Checked;
                if (bSitemapXmlIncludePriority)
                {
                    decimal dOnes = decimal.Parse(ddlSitemapXmlPriorityOnes.SelectedValue);
                    decimal dTenths = decimal.Parse(ddlSitemapXmlPriorityTenths.SelectedValue) / 10M;
                    dSitemapXmlPriority = dOnes + dTenths;
                }
            }
        }

        protected void checkSitemapXmlIncludePage_CheckedChanged(object sender, EventArgs args)
        {
            EnableDisableSitemapControls();
        }

        protected void checkSitemapXmlIncludeChangeFreq_CheckedChanged(object sender, EventArgs args)
        {
            EnableDisableSitemapControls();
        }

        protected void checkSitemapXmlIncludePriority_CheckedChanged(object sender, EventArgs args)
        {
            EnableDisableSitemapControls();
        }

        protected void checkSitemapXmlIncludeLastmod_CheckedChanged(object sender, EventArgs args)
        {
            EnableDisableSitemapControls();
        }

        protected void ddlSitemapXmlPriorityOnes_SelectedIndexChanged(object sender, EventArgs args)
        {
            EnableDisableSitemapControls();
        }

        protected void EnableDisableSitemapControls()
        {
            bool bControlEnabled = Enabled;

            checkSitemapXmlIncludePage.Enabled = bControlEnabled;

            bool bSitemapSettingsEnabled = bControlEnabled && checkSitemapXmlIncludePage.Checked;

            checkSitemapXmlIncludeChangeFreq.Enabled = bSitemapSettingsEnabled;
            ddlChangeFreq.Enabled = bSitemapSettingsEnabled && checkSitemapXmlIncludeChangeFreq.Checked;

            checkSitemapXmlIncludeLastMod.Enabled = bSitemapSettingsEnabled;
            labelOverride.Enabled = bSitemapSettingsEnabled && checkSitemapXmlIncludeLastMod.Checked;
            txtSitemapXmlLastMod.Enabled = bSitemapSettingsEnabled && checkSitemapXmlIncludeLastMod.Checked;

            checkSitemapXmlIncludePriority.Enabled = bSitemapSettingsEnabled;
            ddlSitemapXmlPriorityOnes.Enabled = bSitemapSettingsEnabled && checkSitemapXmlIncludePriority.Checked;
            bool bEnablePriorityTenths = false;
            if (ddlSitemapXmlPriorityOnes.Enabled)
            {
                if (ddlSitemapXmlPriorityOnes.SelectedValue == "0")
                {
                    bEnablePriorityTenths = true;
                }
                else
                {
                    ddlSitemapXmlPriorityTenths.SelectedValue = "0";
                }
            }

            ddlSitemapXmlPriorityTenths.Enabled = bEnablePriorityTenths;

        }

        protected void cvSitemapXmlLastMod_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            bool bValid = false;

            string strLastModDate = txtSitemapXmlLastMod.Text.Trim();
            if (string.IsNullOrEmpty(strLastModDate))
            {
                bValid = true;
            }
            else
            {
                DateTime dt;
                if (DateTime.TryParse(strLastModDate, out dt))
                {
                    bValid = true;
                }
            }

            args.IsValid = bValid;
        }

    }
}