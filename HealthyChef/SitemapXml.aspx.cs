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
using System.Xml;
using System.Text;

using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.Cms
{
    public partial class SiteMapXml : System.Web.UI.Page
    {
        private enum SiteMapFormat { SitemapsOrg, MicrosoftAspNet }

        private readonly SiteMapFormat _siteMapFormat = SiteMapFormat.SitemapsOrg;

        protected void Page_Load(object sender, EventArgs e)
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
                        string url = ResolveUrl(n.Url); //resolve the absolute path.
                        //this escapes the path separators also...
                        //url = Server.UrlEncode(url);
                        url = Bss.Web.Url.ToAbsoluteUrl(Request.Url, url); //construct an absolute URL.
                        url = System.Security.SecurityElement.Escape(url); //escape XML characters.

                        output.AppendLine("<url>");
                        output.AppendLine(string.Format("<loc>{0}</loc>", url));
                        //output.AppendLine("<lastmod>2005-01-01</lastmod>");
                        //output.AppendLine("<priority>0.5</priority>");
                        //output.AppendLine("<changefreq>weekly</changefreq>");
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

            Response.Write(output.ToString());
        }

        //recursive method for Microsoft sitemap format
        private void BuildMSXmlRecursive(SiteMapNode node, StringBuilder output, int indent)
        {
            indent++;

            for (int i = 1; i <= indent; i++)
                output.Append("\t");

            string url = Bss.Web.Url.ToAbsoluteUrl(Request.Url, ResolveUrl(node.Url));

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
}
