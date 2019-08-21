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
using System.Text;
using System.Collections.Generic;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class AddModule : System.Web.UI.Page
    {
        int _pageId;
        int _instanceId;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = SaveButton.UniqueID;

            _pageId = int.Parse(Request.QueryString["PageId"]);
            _instanceId = int.Parse(Request.QueryString["InstanceId"]);

            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                WebpageInfo page = Webpage.GetWebpage(_instanceId);

                Cms.Admin.BindPlaceholdersToList(page, PlaceholderDropDownList, "Body");

                //bind modules list.
                List<WebModuleType> modules = WebModuleType.GetAddableModuleTypes();
                if (modules.Count == 0)
                {
                    this.SaveButton.Enabled = false;
                    this.NoModules.Visible = true;
                }
                foreach (WebModuleType moduleType in modules)
                {
                    this.ModuleList.Items.Add(
                        new ListItem(
                        (!string.IsNullOrEmpty(moduleType.Description)) ? string.Format("{0} - <span style=\"color: #5A5A5A;\">{1}</span>", moduleType.Name, moduleType.Description) : moduleType.Name,
                            string.Format("{0}:{1}", moduleType.WebApplicationType.Name, moduleType.Name)));
                }
                //select Content module by default.
                if (null != this.ModuleList.Items.FindByValue("Content Module:Content"))
                    this.ModuleList.SelectedValue = "Content Module:Content";
            }
        }

        protected WebModuleType GetSelectedModuleType()
        {
            string[] selectedValue = ModuleList.SelectedValue.Split(':');
            string applicationName = selectedValue[0];
            string moduleTypeName = selectedValue[1];
            return WebApplicationType.Items[applicationName].Modules[moduleTypeName];
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (PlaceholderDropDownList.Items.Count == 0) throw new InvalidOperationException("The template (master page) must have a content placeholder.");

            WebModuleType selectedModuleType = GetSelectedModuleType();
            WebModuleInfo module = null;
            int aliasModuleId = -1;
            string placeholder = PlaceholderDropDownList.SelectedItem.ToString();
            string instanceName = ModuleNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(instanceName))
            { //auto-generate a unique name for the module instance
                int i = 1;
                List<WebModuleInfo> pageModules = WebModule.GetModules(_instanceId);
                do
                {
                    instanceName = selectedModuleType.Name + " " + i++;
                } while (pageModules.Exists(
                    delegate(WebModuleInfo m) { return m.Name == instanceName; }));
            }

            if (UseExistingModule.Checked)
            {
                aliasModuleId = int.Parse(ExistingModulesPagesSelect.Text);
            }

            module = WebModule.CreateModule(
                instanceName,
                selectedModuleType.WebApplicationType.Name,
                selectedModuleType.Name,
                _pageId,
                placeholder,
                aliasModuleId);

            Response.Redirect(module.GetAddUrl());
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Cms.Admin.RedirectToMainMenu(_instanceId);
        }

        protected void UseExistingModule_CheckedChanged(object sender, EventArgs e)
        {
            LoadExistingModules();
        }

        private void LoadExistingModules()
        {
            ExistingModulesPagesSelect.Items.Clear();
            UseExistingModule.Enabled = true;

            ExistingModulePanel.Visible = UseExistingModule.Checked;
            if (UseExistingModule.Checked)
            {
                WebpageInfo currentPage = Webpage.GetWebpage(_instanceId);
                WebModuleType selectedModuleType = GetSelectedModuleType();
                List<WebpageInfo> allPages = Webpage.GetDescendants(
                    Webpage.GetWebpage(currentPage.Website.RootNavigationId));
                allPages.Add(currentPage.Website.RootWebpage);

                List<WebpageInfo> pagesWithSelectedModuleType =
                    PagesModulesList.GetPagesWithModuleType(selectedModuleType, allPages);

                //sort by path
                pagesWithSelectedModuleType.Sort(
                    delegate(WebpageInfo p1, WebpageInfo p2)
                    {
                        return p1.Path.CompareTo(p2.Path);
                    });

                bool existingModules = (pagesWithSelectedModuleType.Count > 0);
                ExistingModulesPagesSelect.Visible = existingModules;
                UseExistingModule.Enabled = existingModules;
                NoExistingModulesFoundCtl.Visible = !existingModules;
                if (!existingModules)
                {
                    NoExistingModulesFoundCtl.Text = string.Format("There are no existing '{0}' modules.",
                        selectedModuleType.Name);
                    UseExistingModule.Checked = false;
                }
                else
                {
                    foreach (WebpageInfo p in pagesWithSelectedModuleType)
                    {
                        //find all modules of the specified type
                        //(a page may have multiple modules of the same type).
                        List<WebModuleInfo> modules =
                            p.Modules.FindAll(delegate(WebModuleInfo m)
                            {
                                return !m.IsAlias //currently, we don't support "alias to alias".
                                    && m.WebModuleType.Name == selectedModuleType.Name;
                            });
                        foreach (WebModuleInfo m in modules)
                            //page path + module name.
                            ExistingModulesPagesSelect.Items.Add(new ListItem(
                                ResolveUrl(p.Path) + " - " + m.Name,
                                m.Id.ToString()));
                    }
                }
            }
        }

        protected void ModuleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadExistingModules();
        }
    }
}