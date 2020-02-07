using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public enum MasterDetailStatus
    {
        Pending = 0,
        Active = 1,
        Past = 2
    }

    public class MasterDetailList
    {
        public static WebApplicationType AppType
        {
            get { return WebApplicationType.Items["MasterDetail"]; }
        }
        public static WebModuleType ModuleType
        {
            get { return AppType.Modules["Master Detail List"]; }
        }

        private static int CompareWebModulesByPostDate(WebModuleInfo left, WebModuleInfo right)
        {
            WebpageInfo pLeft = left.Webpage;
            WebpageInfo pRight = right.Webpage;

            int nComparisonResult = 0;

            if (pLeft.PostDate == pRight.PostDate)
            {
                nComparisonResult = 0;
            }
            else
            {
                if (pLeft.PostDate.HasValue && pRight.PostDate.HasValue)
                {
                    if (pLeft.PostDate > pRight.PostDate)
                    {
                        nComparisonResult = -1;
                    }
                    else
                    {
                        nComparisonResult = 1;
                    }
                }
                else
                {
                    if (pLeft.PostDate.HasValue)
                    {
                        nComparisonResult = -1;
                    }
                    else
                    {
                        nComparisonResult = 1;
                    }
                }
            }

            if (nComparisonResult == 0)
            {
                nComparisonResult = pRight.SortOrder - pLeft.SortOrder;
            }

            return nComparisonResult;
        }

        public static List<WebModuleInfo> GetMasterDetailChildren(int moduleId,
            bool filteryByPostDateAndRemoveDate,
            bool orderByMostRecentPosting)
        {
            return GetMasterDetailChildrenFiltered(moduleId,
                filteryByPostDateAndRemoveDate, orderByMostRecentPosting, null);
        }

        public static List<WebModuleInfo> GetMasterDetailChildrenFiltered(int moduleId,
            bool filteryByPostDateAndRemoveDate,
            bool orderByMostRecentPosting,
            string filterTag)
        {
            //get the module object.
            WebModuleInfo module = WebModule.GetModule(moduleId);
            //get the container page of the module.
            WebpageInfo page = module.Webpage;
            //get all the MasterDetail modules on the child pages.
            List<WebpageInfo> childPages = page.Children;
            List<WebModuleInfo> MasterDetailModules = new List<WebModuleInfo>();
            foreach (WebpageInfo p in childPages)
            {
                MasterDetailModules.AddRange(p.Modules.FindAll(
                    delegate(WebModuleInfo m)
                    {
                        return m.WebModuleType == MasterDetailItem.ModuleType && !m.IsAlias;
                    }));
            }

            if (filteryByPostDateAndRemoveDate)
            { //remove premature/expired/non-visible items.
                MasterDetailModules.RemoveAll(
                    delegate(WebModuleInfo m)
                    {
                        WebpageInfo p = m.Webpage;
                        return (p.PostDate.HasValue && DateTime.Now < p.PostDate)
                            || (p.RemoveDate.HasValue && DateTime.Today >= p.RemoveDate)
                            || (!p.Visible);
                    });
            }

            if (!string.IsNullOrEmpty(filterTag))
            {
                //remove non-matching tags.
                MasterDetailModules.RemoveAll(
                    delegate(WebModuleInfo m)
                    {
                        MasterDetailItem item = MasterDetailItem.GetSafeResource(m.Id);
                        string[] tagArray = item.Tags.Split(',');
                        foreach (var s in tagArray)
                        {
                            if (s == filterTag) return false;
                        }
                        return true;
                    });
            }

            if (orderByMostRecentPosting)
                MasterDetailModules.Sort(CompareWebModulesByPostDate);

            return MasterDetailModules;
        }

        public static List<WebModuleInfo> GetMasterDetailChildrenFiltered(int moduleId,
            bool filteryByPostDateAndRemoveDate,
            bool orderByMostRecentPosting,
            string filterTag, bool displayAllItems)
        {
            //get the module object.
            WebModuleInfo module = WebModule.GetModule(moduleId);
            //get the container page of the module.
            WebpageInfo page = module.Webpage;
            //get all the MasterDetail modules on the child pages.
            List<WebpageInfo> childPages = page.Children;
            List<WebModuleInfo> MasterDetailModules = new List<WebModuleInfo>();
            foreach (WebpageInfo p in childPages)
            {
                MasterDetailModules.AddRange(p.Modules.FindAll(
                    delegate(WebModuleInfo m)
                    {
                        return m.WebModuleType == MasterDetailItem.ModuleType && !m.IsAlias;
                    }));
            }

            if (filteryByPostDateAndRemoveDate)
            { //remove premature/expired/non-visible items.
                MasterDetailModules.RemoveAll(
                    delegate(WebModuleInfo m)
                    {
                        WebpageInfo p = m.Webpage;
                        if (displayAllItems)
                            return (p.PostDate.HasValue && DateTime.Now < p.PostDate)
                                || (p.RemoveDate.HasValue && DateTime.Today >= p.RemoveDate);
                        else
                            return (p.PostDate.HasValue && DateTime.Now < p.PostDate)
                                || (p.RemoveDate.HasValue && DateTime.Today >= p.RemoveDate)
                                || (!p.Visible);
                    });
            }

            if (!string.IsNullOrEmpty(filterTag))
            {
                //remove non-matching tags.
                MasterDetailModules.RemoveAll(
                    delegate(WebModuleInfo m)
                    {
                        MasterDetailItem item = MasterDetailItem.GetSafeResource(m.Id);
                        string[] tagArray = item.Tags.Split(',');
                        foreach (var s in tagArray)
                        {
                            if (s == filterTag) return false;
                        }
                        return true;
                    });
            }

            if (orderByMostRecentPosting)
                MasterDetailModules.Sort(CompareWebModulesByPostDate);

            return MasterDetailModules;
        }

        //also deletes webpage if it is empty.
        public static void DestroyMasterDetailItem(int moduleId)
        {
            //get the module object.
            WebModuleInfo module = WebModule.GetModule(moduleId);

            //get the container page of the module.
            WebpageInfo page = module.Webpage;

            //this cascades and destroys the MasterDetail associated items/resources.
            WebModule.DeleteModule(moduleId);

            //destory the item (this cascades down the resources).
            //MasterDetail_Item.Destroy(moduleId);

            //destory all associated item resources.
            //MasterDetail_ItemResource.Destroy(MasterDetail_ItemResource.Columns.ModuleId, moduleId);

            //if the page has no modules left on it, remove it also.
            if (null == page.Modules || page.Modules.Count == 0)
                Webpage.DeleteWebpage(page.InstanceId);
        }
    }
}
