using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChefCreationsMVC.CustomModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using Bss = BayshoreSolutions.Common;

namespace HealthyChefCreationsMVC.Controllers
{
    public class CommonController : Controller
    {

        [ChildActionOnly]
        public PartialViewResult TopHeaderSection()
        {
            TopHeaderViewModel topHeaderViewModel = new TopHeaderViewModel();

            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    hccCart cart = null;
                    if (user == null)
                        cart = hccCart.GetCurrentCart();
                    else
                    {
                        cart = hccCart.GetCurrentCart(user);
                        //topHeaderViewModel.ShowAdminLink = Roles.IsUserInRole(user.UserName, "Administrators")
                        //                               || Roles.IsUserInRole(user.UserName, "EmployeeProduction")
                        //                               || Roles.IsUserInRole(user.UserName, "EmployeeService")
                        //                               || Roles.IsUserInRole(user.UserName, "EmployeeManager");
                    }

                    if (cart != null)
                    {
                        List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                        hccCartItem obj = new hccCartItem();

                        topHeaderViewModel.CartCount = cartItems.Count;
                    }
                    else
                        topHeaderViewModel.CartCount = 0;


                }
            }
            catch (Exception)
            {
                throw;
            }

            return PartialView("~/Views/Shared/_TopHeader.cshtml", topHeaderViewModel);
        }


        [ChildActionOnly]
        [OutputCache(Duration = 120)]
        public PartialViewResult FooterSection()
        {
            FooterViewModel footerViewModel = new FooterViewModel();
            return PartialView("~/Views/Shared/_Footer.cshtml", footerViewModel);
        }

        [ChildActionOnly]
        public PartialViewResult MobileHeaderSection()
        {
            MobileHeaderModel mobileHeaderModel = new MobileHeaderModel();

            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    hccCart cart = null;
                    if (user == null)
                        cart = hccCart.GetCurrentCart();
                    else
                    {
                        cart = hccCart.GetCurrentCart(user);
                        //topHeaderViewModel.ShowAdminLink = Roles.IsUserInRole(user.UserName, "Administrators")
                        //                               || Roles.IsUserInRole(user.UserName, "EmployeeProduction")
                        //                               || Roles.IsUserInRole(user.UserName, "EmployeeService")
                        //                               || Roles.IsUserInRole(user.UserName, "EmployeeManager");
                    }

                    if (cart != null)
                    {
                        List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                        hccCartItem obj = new hccCartItem();

                        mobileHeaderModel.topHeaderViewModel.CartCount = cartItems.Count;
                    }
                    else
                        mobileHeaderModel.topHeaderViewModel.CartCount = 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return PartialView("~/Views/Shared/_MobileHeader.cshtml", mobileHeaderModel);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 60)]
        public PartialViewResult HeaderSection()
        {
            HeaderViewModel headerViewModel = new HeaderViewModel();
            MembershipUser user = Helpers.LoggedUser;
            if (user == null || Roles.IsUserInRole(user.UserName, "Customer"))
            {
                hccCart cart = (user == null) ? hccCart.GetCurrentCart() : hccCart.GetCurrentCart(user);
                if (cart != null)
                {
                    List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                    headerViewModel.CartCount = cartItems.Count;
                }
            }
            return PartialView("~/Views/Shared/_Header.cshtml", headerViewModel);
        }

        [ChildActionOnly]
        [OutputCache(Duration = 120)]
        public PartialViewResult WhiteBoxSection()
        {
            WhiteBoxViewModel whiteBoxViewModel = new WhiteBoxViewModel();
            return PartialView("~/Views/Shared/_WhiteBox.cshtml", whiteBoxViewModel);
        }

        //sitemap showing in ui start
        [XmlRoot("urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public class Sitemap
        {
            private ArrayList _map;

            public Sitemap()
            {
                _map = new ArrayList();
            }

            [XmlElement("url")]
            public Location[] Locations
            {
                get
                {
                    Location[] items = new Location[_map.Count];
                    _map.CopyTo(items);
                    return items;
                }
                set
                {
                    if (value == null)
                        return;
                    var items = (Location[])value;
                    _map.Clear();
                    foreach (Location item in items)
                        _map.Add(item);
                }
            }

            public int Add(Location item)
            {
                return _map.Add(item);
            }
        }

        public class Location
        {
            public enum EChangeFrequency
            {
                Always,
                Hourly,
                Daily,
                Weekly,
                Monthly,
                Yearly,
                Never
            }

            [XmlElement("loc")]
            public string Url { get; set; }

            [XmlElement("changefreq")]
            public EChangeFrequency? ChangeFrequency { get; set; }
            public bool ShouldSerializeChangeFrequency() { return ChangeFrequency.HasValue; }

            [XmlElement("lastmod")]
            public DateTime? LastModified { get; set; }
            public bool ShouldSerializeLastModified() { return LastModified.HasValue; }

            [XmlElement("priority")]
            public double? Priority { get; set; }
            public bool ShouldSerializePriority() { return Priority.HasValue; }
        }

        public class XmlResult : ActionResult
        {
            private readonly object _objectToSerialize;

            public XmlResult(object objectToSerialize)
            {
                _objectToSerialize = objectToSerialize;
            }

            public object ObjectToSerialize
            {
                get { return _objectToSerialize; }
            }

            public override void ExecuteResult(ControllerContext context)
            {
                if (_objectToSerialize != null)
                {
                    context.HttpContext.Response.Clear();
                    var xs = new XmlSerializer(_objectToSerialize.GetType());
                    context.HttpContext.Response.ContentType = "text/xml";
                    xs.Serialize(context.HttpContext.Response.Output, _objectToSerialize);
                }
            }
        }
        public ActionResult SitemapNode()
        {
            var sm = new Sitemap();
            foreach (SiteMapNode n in SiteMap.RootNode.GetAllNodes())
            {
                sm.Add(new Location()
                {
                    Url = Request.Url.Scheme + "://" + Request.Url.Authority + n.Url.Remove(0, 1),
                    LastModified = DateTime.UtcNow,
                    Priority = 0.5D
                });
            }
            return new XmlResult(sm);
        }
        public ActionResult SitemapProvider()
        {
            var _SiteMapNodes = new List<SiteMapProviderXml>();
            int myCounter = 0;

            var _siteMapObj = new BayshoreSolutions.WebModules.SiteMapProvider();

            // to set displayMode = visible
            NameValueCollection collection = new NameValueCollection();
            collection.Add("displayMode", "Visible");
            _siteMapObj.Initialize("WebModulesSiteMapProvider", collection);

            _siteMapObj.BuildSiteMap();
            var _ListofSitemaps = _siteMapObj.RootNode;

            if (_ListofSitemaps.HasChildNodes)
            {

                foreach (SiteMapNode myNode in _ListofSitemaps.ChildNodes)
                {
                    SiteMapProviderXml siteMapProviderXml = new SiteMapProviderXml();
                    List<SiteMapProviderXml> listchildsiteMapXml = new List<SiteMapProviderXml>();
                    siteMapProviderXml.Name = myNode.Title;
                    siteMapProviderXml.Url = myNode.Url.TrimStart('~');
                    foreach (SiteMapNode sitemapnode in myNode.ChildNodes)
                    {
                        SiteMapProviderXml childsiteMapProviderXml = new SiteMapProviderXml();
                        childsiteMapProviderXml.Name = sitemapnode.Title;
                        childsiteMapProviderXml.Url = sitemapnode.Url.TrimStart('~');
                        listchildsiteMapXml.Add(childsiteMapProviderXml);
                    }
                    siteMapProviderXml.listofSitemap = listchildsiteMapXml;
                    _SiteMapNodes.Add(siteMapProviderXml);
                }
            }
            else
            {
                _SiteMapNodes = new List<SiteMapProviderXml>();
            }
            return View(_SiteMapNodes);
        }
        //end

        //sitemp creating in xml formate
        private enum SiteMapFormat { SitemapsOrg, MicrosoftAspNet }

        private readonly SiteMapFormat _siteMapFormat = SiteMapFormat.SitemapsOrg;

        public ActionResult SiteMapNodeInXml()
        {
            StringBuilder output = new StringBuilder();

            Response.ClearHeaders();
            Response.ClearContent();
            Response.Clear();
            Response.ContentType = "text/xml";

            output.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");

            switch (_siteMapFormat)
            {
                //sitemaps.org sitemap format (used by all major search engines: Google, Yahoo, MSN).
                //http://www.sitemaps.org/schemas/sitemap/0.9
                case SiteMapFormat.SitemapsOrg:
                    output.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/sitemap.xsd\">");

                    foreach (SiteMapNode n in SiteMap.RootNode.GetAllNodes())
                    {
                        string url = n.Url; //resolve the absolute path.
                        //this escapes the path separators also...
                        //url = Server.UrlEncode(url);
                        url = Bss.Web.Url.ToAbsoluteUrl(Request.Url, url); //construct an absolute URL.
                        url = System.Security.SecurityElement.Escape(url); //escape XML characters.

                        output.AppendLine("<url>");
                        output.AppendLine(string.Format("<loc>{0}</loc>", url));
                        //output.AppendLine("<lastmod>"+DateTime.Now+"</lastmod>");
                        //output.AppendLine("<priority>0.5</priority>");
                        output.AppendLine("</url>");
                    }

                    output.AppendLine("</urlset>");
                    break;

                //Microsoft sitemap format. less common. if in doubt, use the sitemaps.org format instead.
                //http://schemas.microsoft.com/AspNet/SiteMap-File-1.0
                case SiteMapFormat.MicrosoftAspNet:
                    int indent = 0;

                    output.AppendLine("<siteMap xmlns=\"http://schemas.microsoft.com/AspNet/SiteMap-File-1.0\" >");
                    BuildMSXmlRecursive(SiteMap.RootNode, output, indent);
                    output.AppendLine("</siteMap>");
                    break;
            }

            return Content(output.ToString());
        }

        //recursive method for Microsoft sitemap format
        private void BuildMSXmlRecursive(SiteMapNode node, StringBuilder output, int indent)
        {
            indent++;

            for (int i = 1; i <= indent; i++)
                output.Append("\t");

            string url = Bss.Web.Url.ToAbsoluteUrl(Request.Url, node.Url);

            output.AppendLine(string.Format("<siteMapNode url=\"{0}\" title=\"{1}\" {2}>",
                url,
                node.Title,
                node.ChildNodes.Count == 0 ? "/" : ""));

            foreach (SiteMapNode n in node.ChildNodes)
            {
                BuildMSXmlRecursive(n, output, indent);
            }

            if (node.ChildNodes.Count != 0) output.AppendLine("</siteMapNode>");

            indent--;
        }
    }
    //end
}