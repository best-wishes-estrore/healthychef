using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;

using RssToolkit;
using BayshoreSolutions.WebModules;
using System.ComponentModel;
using System.Collections.Generic;
using RssToolkit.Rss;

namespace BayshoreSolutions.WebModules.MasterDetail.Rss
{
    public class RssFeed : RssHttpHandlerBase<MasterDetailRss>
    {
        protected override void PopulateRss(string channelName, string userName)
        {
            // Get the data
            string qModuleId = HttpContext.Current.Request["moduleId"];
            int moduleId;
            if (!int.TryParse(qModuleId, out moduleId) || moduleId <= 0)
                throw new ArgumentException("Invalid or missing module id: " + moduleId);

            WebModuleInfo MasterDetailListModule = WebModule.GetModule(moduleId);
            if (MasterDetailListModule.WebModuleType != MasterDetailList.ModuleType)
                throw new ArgumentException(string.Format("Invalid module type '{0}'. Module type must be '{1}'", MasterDetailListModule.ModuleTypeName, MasterDetailList.ModuleType.Name));
            WebpageInfo MasterDetailListPage = MasterDetailListModule.Webpage;
            if (null == MasterDetailListModule)
                throw new ArgumentException("Invalid module id: " + moduleId);
            List<WebModuleInfo> MasterDetailItemModules = MasterDetailList.GetMasterDetailChildren(moduleId, true, true);


            // Build the feed
            Rss.Channel = new MasterDetailChannel();
            Rss.Version = "2.0";
            Rss.Channel.Title = MasterDetailListPage.Title;
            Rss.Channel.Description = MasterDetailListPage.MetaDescription;
            Rss.Channel.Link = MasterDetailListPage.Path;

            Rss.Channel.Items = new List<MasterDetailRssItem>();
            if (!string.IsNullOrEmpty(channelName))
            {
                Rss.Channel.Title += " '" + channelName + "'";
            }

            if (!string.IsNullOrEmpty(userName))
            {
                Rss.Channel.Title += " (generated for " + userName + ")";
            }

            int maxItemsPerPage = 25;
            int i = 0;

            foreach (WebModuleInfo m in MasterDetailItemModules)
            {
                WebpageInfo p = m.Webpage;
                MasterDetailItem itemRes = MasterDetailItem.GetSafeResource(m.Id);
                //string postDateString = "";
                //if (p.PostDate.HasValue)
                //{
                //this is a hack
                //	postDateString = p.PostDate.Value.GetDateTimeFormats(
                //		System.Globalization.CultureInfo.GetCultureInfo("en-US"))[103];
                //}

                MasterDetailRssItem item = new MasterDetailRssItem();
                item.Title = p.Title;
                item.Description = itemRes.GetSummary(p);
                if (p.PostDate.HasValue)
                {
                    item.PubDate = RssXmlHelper.ToRfc822(p.PostDate.Value);
                }
                item.Link = VirtualPathUtility.ToAbsolute(p.Path);

                Rss.Channel.Items.Add(item);

                i++;
                if (i == maxItemsPerPage) break;
            }

        }
    }
}
