using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

using BayshoreSolutions.WebModules;
using BayshoreSolutions.WebModules.Security;

namespace BayshoreSolutions.WebModules.Cms.Controls
{
    public partial class ModuleList : System.Web.UI.UserControl
    {
        protected int _instanceId;

        protected void LoadModules()
        {
            List<WebModuleInfo> modules = WebModule.GetModules(_instanceId);
            ModulesGridView.DataSource = modules;
            ModulesGridView.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // get the page navigation id
            if (!int.TryParse(Request.QueryString["InstanceId"], out _instanceId))
            {
                _instanceId = WebModulesProfile.Current.StartPageId;

                if (_instanceId <= 0)
                    _instanceId = Webpage.RootNavigationId;
            }

            if (!IsPostBack)
            {
                //check user permissions.
                if (!NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(Permission.MSG_SECURITY_FAILURE);

                bool allowEdit = Permission.AllowEditContent();
                bool allowManagePage = Permission.AllowManagePage();

                //sort buttons
                ModulesGridView.Columns[3].Visible = allowEdit;
                ModulesGridView.Columns[4].Visible = allowEdit;
                //settings
                ModulesGridView.Columns[5].Visible = allowEdit;
                //delete
                ModulesGridView.Columns[6].Visible = allowManagePage;

                LoadModules();
            }
        }

        protected void ModulesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandArg = e.CommandArgument.ToString();
            int moduleId;

            if (commandArg != string.Empty)
            {
                string command = e.CommandName;

                if (command == "SortUp")
                {
                    moduleId = int.Parse(commandArg);
                    WebModule.SortWebModuleUp(_instanceId, moduleId);
                }
                else if (command == "SortDown")
                {
                    moduleId = int.Parse(commandArg);
                    WebModule.SortWebModuleDown(_instanceId, moduleId);
                }
                else if (command == "DeleteModule")
                {
                    moduleId = int.Parse(commandArg);
                    Response.Redirect("ModuleDelete.aspx?moduleID=" + moduleId + "&instanceID=" + _instanceId);
                }
                else if (command == "ViewShares")
                {
                    moduleId = int.Parse(commandArg);
                    Response.Redirect("PagesSharingModule.aspx?moduleID=" + moduleId + "&instanceID=" + _instanceId);
                }

                LoadModules();
            }
        }

        protected void ModulesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HtmlAnchor editModuleSettingsLink = (HtmlAnchor)e.Row.FindControl("EditModuleSettingsLink");
                WebModuleInfo module = (WebModuleInfo)e.Row.DataItem;
                string icon = null;

                if (module.IsAlias)
                {
                    icon = "~/WebModules/Admin/Images/Icons/Small/ModuleAlias.png";
                    editModuleSettingsLink.Title = "This module is an alias (it shares another module).";
                }
                else
                {
                    icon = module.WebModuleType.Icon;

                    if (string.IsNullOrEmpty(icon))
                        icon = "~/WebModules/Admin/Images/Icons/Small/Module.gif";
                }

                icon = Page.ResolveUrl(icon);

                editModuleSettingsLink.Style["background-image"] = icon;

                ImageButton btnDelete = e.Row.FindControl("btnDelete") as ImageButton;
                if (btnDelete != null)
                {
                    if (module.ShareCount > 0)
                    {
                        btnDelete.ImageUrl = "~/WebModules/Admin/Images/Icons/Small/Versions.gif";
                        btnDelete.ToolTip = "View Shares";
                        btnDelete.CommandName = "ViewShares";
                    }
                }
            }
        }


    }
}