using System;
using System.Collections.Generic;
using System.Web;
using BayshoreSolutions.WebModules;
using WM = BayshoreSolutions.WebModules;
using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class MasterDetailItem
    {
        public static WebModuleType ModuleType
        {
            get { return MasterDetailList.AppType.Modules["Master Detail Item"]; }
        }

        public static MasterDetailItem GetResource(int moduleId)
        {
            return GetResource(moduleId, WM.CultureCode.Current);
        }
        public static MasterDetailItem GetResource(int moduleId, string cultureCode)
        {
            MasterDetailItemCollection itemResL = new MasterDetailItemCollection()
                .Where(MasterDetailItem.Columns.ModuleId, moduleId)
                .Where(MasterDetailItem.Columns.Culture, cultureCode)
                .Load();
            if (null == itemResL || itemResL.Count == 0)
            {
                return null;
            }
            else return itemResL[0];
        }

        public static MasterDetailItem GetSafeResource(int moduleId)
        {
            return GetSafeResource(moduleId, WM.CultureCode.Current);
        }
        public static MasterDetailItem GetSafeResource(int moduleId, string cultureCode)
        {
            MasterDetailItem itemRes = MasterDetailItem.GetResource(moduleId, cultureCode);

            if (null == itemRes)
            {
                //try default culture
                WM.CultureCode defaultCultureCode = WM.CultureCode.GetDefaultCulture();
                if (null != defaultCultureCode && WM.CultureCode.Current != defaultCultureCode.Name)
                    itemRes = MasterDetailItem.GetResource(moduleId, defaultCultureCode.Name);
            }

            return itemRes;
        }

        //finds the first content list that "contains" this item, if any.
        public MasterDetailSetting GetContentList()
        {
            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            WebpageInfo parentPage = module.Webpage.Parent;
            MasterDetailSetting itemList = null;
            foreach (WebModuleInfo m in parentPage.Modules)
                if (m.WebModuleType == MasterDetailList.ModuleType)
                {
                    itemList = MasterDetailSetting.FetchByID(m.Id);
                    break;
                }

            return itemList;
        }

        //if WebpageInfo.MetaDescription is empty, generates a summary based on the BodyText.
        public string GetSummary(WebpageInfo p)
        {
            string summary = this.ShortDescription;
            if (null == summary || summary.Trim().Length == 0)
            {
                //auto-generate summary
                string fullBodyTextWithoutHtmlTags = Bss.Web.Html.StripTags(this.LongDescription);
                summary = Chop(fullBodyTextWithoutHtmlTags, 300, true);
            }
            return summary;
        }
        public static string Chop(string s, int nrChar, bool appendEllipsisIfChopped)
        {
            if (null != s)
            {
                s = s.Trim();
                if (s.Length > nrChar)
                {
                    s = s.Remove(nrChar);
                    if (appendEllipsisIfChopped)
                        s = s + "...";
                }
            }
            return s;
        }

    }
}
