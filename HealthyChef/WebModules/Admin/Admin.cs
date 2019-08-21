using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using BayshoreSolutions.WebModules;
using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.Cms
{
    public class Admin
    {
        public static readonly string MainMenuUrl = "~/WebModules/Admin/MyWebsite/Default.aspx";

        public static string GetMainMenuUrl(int navId)
        {
            return MainMenuUrl + "?instanceId=" + navId;
        }
        public static void RedirectToMainMenu()
        {
            RedirectToMainMenu(Webpage.RootNavigationId);
        }
        public static void RedirectToMainMenu(int navId)
        {
            HttpContext.Current.Response.Redirect(GetMainMenuUrl(navId));
        }
        public static void BindPlaceholdersToList(WebpageInfo page, DropDownList list, string defaultSelectedValue)
        {
            List<ContentPlaceHolder> placeholders =
                Bss.Web.UI.UITool.GetDescendentControls<ContentPlaceHolder>(
                    new Page().LoadControl(page.TemplatePath));

            //sort alphabetically.
            placeholders.Sort(delegate(ContentPlaceHolder p1, ContentPlaceHolder p2)
            {
                return p1.ID.CompareTo(p2.ID);
            });

            list.DataSource = placeholders;
            list.DataTextField = "ID";
            list.DataValueField = "ID";
            list.DataBind();

            if (null != list.Items.FindByValue(defaultSelectedValue))
                list.SelectedValue = defaultSelectedValue;
        }
    }
}
