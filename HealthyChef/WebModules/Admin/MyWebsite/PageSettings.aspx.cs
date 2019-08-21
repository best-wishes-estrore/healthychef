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

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class PageSettings : System.Web.UI.Page
    {
        int _instanceId;
        protected WebpageInfo _page;

        protected override void OnInit(EventArgs e)
        {
            TemplatePropertiesDisplay.Load += new EventHandler(TemplatePropertiesDisplay_Load);
            base.OnInit(e);
        }

        void TemplatePropertiesDisplay_Load(object sender, EventArgs e)
        {
            TemplateProperties_div.Visible = TemplatePropertiesDisplay.HasControls();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _instanceId = int.Parse(Request.QueryString["InstanceId"]);
            _page = Webpage.GetWebpage(_instanceId);
            sitemapXmlEdit.WebpageInfo = _page;

            ParentPage.HideNavigationId = _instanceId;

            //load controls for dynamic page properties, if any.
            TemplatePropertiesDisplay.ControlPath = _page.TemplatePath;
            TemplatePropertiesDisplay.DefaultValues = _page.TemplateProperties;

            // set fields to defaults
            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                if (_page.ParentInstanceId == null || _page.ParentInstanceId <= Webpage.RootNavigationId)
                {
                    //hide the parent page picker (the portal root must always have a null parent;
                    //website root pages must always be children of the portal root).
                    ParentPage_div.Visible = false;
                    //hide the path name textbox (path name is irrelevant for a website's root page).
                    PathNameEditCtl.Visible = false;
                    ParentPage.SelectedNavigationId = 0;
                }
                else
                {
                    ParentPage.SelectedNavigationId = _page.ParentInstanceId.Value;
                }

                TitleTextBox.Text = _page.Title;
                NavigationTextTextBox.Text = _page.Text;
                PathNameEditCtl.PathName = _page.PathName;
                //DescriptionText.Text = _page.Description;
                MetaKeywordsTextbox.Text = _page.MetaKeywords;
                MetaDescriptionTextbox.Text = _page.MetaDescription;
                VisibleCheckBox.Checked = _page.Visible;

                /*
                // SitemapXml settings
                checkSitemapXmlIncludePage.Checked = _page.SitemapXml_IncludePage;
                if (_page.SitemapXml_IncludePage)
                {
                    checkSitemapXmlIncludeLastMod.Checked = _page.SitemapXml_IncludeLastmod;

                    string strLastMod = null;
                    if( checkSitemapXmlIncludeLastMod.Checked )
                    {
                        if (_page.SitemapXml_LastModified.HasValue)
                        {
                            strLastMod = _page.SitemapXml_LastModified.Value.ToShortDateString();
                            if( _page.SitemapXml_LastModified.Value.TimeOfDay.TotalSeconds > 0)
                            {
                                strLastMod += " " +_page.SitemapXml_LastModified.Value.ToShortTimeString();
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
                    if( checkSitemapXmlIncludePriority.Checked )
                    {
                        decimal dPriority = _page.SitemapXml_Priority;
                        if( dPriority >= 1 )
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
                */

                //templates
                TemplatesDropDownList.DataSource = BayshoreSolutions.WebModules.WebDirectories.GetTemplates();
                TemplatesDropDownList.DataBind();
                TemplatesDropDownList.SelectedValue = _page.TemplateGroup + " - " + _page.Template;

                //themes
                ThemeDropDownList.DataSource = BayshoreSolutions.WebModules.WebDirectories.GetThemes();
                ThemeDropDownList.DataBind();
                ThemeDropDownList.SelectedValue = _page.Theme;
            }

        }

        protected void UpdatePageSettings_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;
            if (_instanceId == Webpage.RootNavigationId) throw new InvalidOperationException("The portal root may not be edited through this form.");

            int? newParentId = null;

            if (ParentPage_div.Visible)
            {
                newParentId = ParentPage.SelectedNavigationId;
            }
            /*else if (_instanceId == Webpage.RootNavigationId)
            { //the portal root must always have a null parent.
                    newParentId = null;
            }
            */
            else
            {   //if the parent page picker is hidden, then we determined during
                //Page_Load that the current page is a website root page;
                //website root pages must always be children of the portal root.
                newParentId = Webpage.RootNavigationId;
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

            /*

            bool bSitemapXmlIncludePage = checkSitemapXmlIncludePage.Checked;
            bool bSitemapXmlIncludeLastMod = false;
            DateTime? dtSitemapXmlLastmod = null;
            bool bSitemapXmlIncludeChangeFreg = true;
            string strSitemapXmlChangeFreq = null;
            bool bSitemapXmlIncludePriority = true;
            decimal dSitemapXmlPriority = 0.5M;

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
            */



            Webpage.WebpageCreateStatus status = Webpage.UpdateWebpageSettings(_instanceId,
                    newParentId,
                    TitleTextBox.Text,
                    NavigationTextTextBox.Text,
                    PathNameEditCtl.PathName,
                    MetaDescriptionTextbox.Text,
                    MetaKeywordsTextbox.Text,
                    MetaDescriptionTextbox.Text,
                    VisibleCheckBox.Checked,
                    bSitemapXmlIncludePage,
                    bSitemapXmlIncludeLastMod,
                    dtSitemapXmlLastmod,
                    bSitemapXmlIncludeChangeFreg,
                    strSitemapXmlChangeFreq,
                    bSitemapXmlIncludePriority,
                    dSitemapXmlPriority,
                    this.TemplatePropertiesDisplay.result
                    ); //xx

            switch (status)
            {
                case Webpage.WebpageCreateStatus.DuplicateName:
                    Msg.ShowError(string.Format("Failed to save page settings. Path name '{0}' already exists.", PathNameEditCtl.PathName));
                    break;
                case Webpage.WebpageCreateStatus.IllegalName:
                    Msg.ShowError(string.Format("Failed to save page settings. Path name '{0}' is not allowed.", PathNameEditCtl.PathName));
                    break;
                case Webpage.WebpageCreateStatus.Success:
                    SaveTemplateAndTheme();
                    Response.Redirect("Default.aspx?InstanceId=" + _instanceId);
                    break;
                case Webpage.WebpageCreateStatus.None:
                default:
                    Msg.ShowError("Error.");
                    break;
            }
        }

        private void SaveTemplateAndTheme()
        {
            string templateGroup = null;
            string template = null;
            string theme = null;

            if (TemplatesDropDownList.SelectedValue != string.Empty)
            {
                string[] selectedValue = TemplatesDropDownList.SelectedValue.Replace(" - ", "-").Split('-');
                templateGroup = selectedValue[0];
                template = selectedValue[1];
            }

            if (ThemeDropDownList.SelectedValue != string.Empty)
            {
                theme = ThemeDropDownList.SelectedValue;
            } //else theme=null so that the Default theme is used.

            Webpage.UpdatePageTemplate(_instanceId, templateGroup, template, theme);
        }

        protected void CancelPageSettings_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?InstanceId=" + _instanceId);
        }
        /*
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
            bool bSitemapSettingsEnabled = checkSitemapXmlIncludePage.Checked;

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
        */


    }
}