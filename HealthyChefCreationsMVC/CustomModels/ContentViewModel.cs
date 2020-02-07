using BayshoreSolutions.WebModules.ContentModule;
using BayshoreSolutions.WebModules.QuickContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class ContentViewModel
    {
        public static readonly string routeProvider = @"";
        string _relVirtualPath = string.Empty;

        public int Id { get; set; }
        public string Title { get; set; }
        public string PageContentTitle { get; set; }
        public string HtmlString { get; set; }

        public string Body { get; set; }
        public string FrontPageSlideShow { get; set; }
        public string HeaderImage { get; set; }
        public string HeaderText { get; set; }
        public string MasterHeader { get; set; }
        public string RightTopBox { get; set; }

        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        
        public ContentViewModel()
        {

        }

        //public ContentViewModel(int _moduleId)
        //{
        //    this.Id =_moduleId;
        //    System.Text.StringBuilder _htmlStr = new System.Text.StringBuilder();
        //    try
        //    {
        //        var InfoWebpage = BayshoreSolutions.WebModules.Webpage.GetWebpage(_moduleId);
        //        if (InfoWebpage != null)
        //        {
        //            this.Id = InfoWebpage.Id;
        //            this.Title = InfoWebpage.Title;

        //            var _allModules = InfoWebpage.Modules.OrderBy(p => p.SortOrder).ToList();

        //            foreach (var m in _allModules)
        //            {
        //                BayshoreSolutions.WebModules.ContentModule.Content content = null;
        //                content = BayshoreSolutions.WebModules.ContentModule.Content.GetActiveContentForDisplay(m.Id, BayshoreSolutions.WebModules.CultureCode.Current);
        //                if (null != content)
        //                {
        //                    _htmlStr.Append(content.Text);
        //                }

        //            }
        //        }

        //        this.HtmlString = _htmlStr.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}


        //// new implementation for page sections and place holders

        public ContentViewModel(int _moduleId)
        {
            this.Id =_moduleId;
            //default to string.empty
            this.Body = string.Empty;
            this.HeaderImage = string.Empty;
            this.RightTopBox = string.Empty;
            this.HeaderText = string.Empty;
            this.MasterHeader = string.Empty;
            this.FrontPageSlideShow = string.Empty;

            System.Text.StringBuilder _htmlStr = new System.Text.StringBuilder();
            try
            {
                var InfoWebpage = BayshoreSolutions.WebModules.Webpage.GetWebpage(_moduleId);
                if (InfoWebpage != null)
                {
                    this.Id = InfoWebpage.Id;
                    this.Title = InfoWebpage.Title;
                    this.PageContentTitle = InfoWebpage.Text;

                    foreach (var m in InfoWebpage.Modules)
                    {
                        BayshoreSolutions.WebModules.ContentModule.Content content = null;
                        content = BayshoreSolutions.WebModules.ContentModule.Content.GetActiveContentForDisplay(m.Id, BayshoreSolutions.WebModules.CultureCode.Current);
                        if (null != content)
                        {
                            switch(m.Placeholder)
                            {
                                case "Body":
                                    this.Body += content.Text;
                                    break;
                                case "HeaderImage":
                                    this.HeaderImage += content.Text;
                                    break;
                                case "RightTopBox":
                                    this.RightTopBox += content.Text;
                                    break;
                                case "HeaderText":
                                    this.HeaderText += content.Text;
                                    break;
                                case "masterHeader":
                                    this.MasterHeader += content.Text;
                                    break;
                                case "FrontPageSlideShow":
                                    this.FrontPageSlideShow = content.Text;
                                    break;
                                default:
                                    this.Body += content.Text;
                                    break;

                            }

                        }
                    }


                }

                this.HtmlString = _htmlStr.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public ContentViewModel(string virtualPath)
        {
            this._relVirtualPath = string.Empty;            
            this._relVirtualPath += @"/" + virtualPath;

            //default to string.empty
            this.Body = string.Empty;
            this.HeaderImage = string.Empty;
            this.RightTopBox = string.Empty;
            this.HeaderText = string.Empty;
            this.MasterHeader = string.Empty;
            this.FrontPageSlideShow = string.Empty;

            System.Text.StringBuilder _htmlStr = new System.Text.StringBuilder();
            try
            {
                var InfoWebpage = BayshoreSolutions.WebModules.Webpage.GetWebpage(_relVirtualPath);
                if (InfoWebpage != null)
                {
                    this.Id = InfoWebpage.Id;
                    this.Title = InfoWebpage.Text;

                    foreach (var m in InfoWebpage.Modules)
                    {
                        BayshoreSolutions.WebModules.ContentModule.Content content = null;
                        content = BayshoreSolutions.WebModules.ContentModule.Content.GetActiveContentForDisplay(m.Id, BayshoreSolutions.WebModules.CultureCode.Current);
                        if (null != content)
                        {
                            switch (m.Placeholder)
                            {
                                case "Body":
                                    this.Body += content.Text;
                                    break;
                                case "HeaderImage":
                                    this.HeaderImage += content.Text;
                                    break;
                                case "RightTopBox":
                                    this.RightTopBox += content.Text;
                                    break;
                                case "HeaderText":
                                    this.HeaderText += content.Text;
                                    break;
                                case "masterHeader":
                                    this.MasterHeader += content.Text;
                                    break;
                                case "FrontPageSlideShow":
                                    this.FrontPageSlideShow = content.Text;
                                    break;
                                default:
                                    this.Body += content.Text;
                                    break;

                            }

                        }
                    }


                }

                this.HtmlString = _htmlStr.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ContentViewModel(bool isParent)
        {

            var _absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri;
            if (_absoluteUri.Contains(routeProvider))
            {
                _absoluteUri = HttpContext.Current.Request.Url.AbsoluteUri;
            }

            Uri UriObj = new Uri(_absoluteUri);
            
            //default to string.empty
            this.Body = string.Empty;
            this.HeaderImage = string.Empty;
            this.RightTopBox = string.Empty;
            this.HeaderText = string.Empty;
            this.MasterHeader = string.Empty;
            this.FrontPageSlideShow = string.Empty;

            System.Text.StringBuilder _htmlStr = new System.Text.StringBuilder();
            try
            {
                var InfoWebpage = BayshoreSolutions.WebModules.Webpage.GetWebpage(UriObj);
                if (InfoWebpage != null)
                {
                    this.Id = InfoWebpage.Id;
                    this.Title = InfoWebpage.Title;
                    this.MetaKeywords = InfoWebpage.MetaKeywords;
                    this.MetaDescription = InfoWebpage.MetaDescription;
                    this.PageContentTitle = InfoWebpage.Text;

                    foreach (var m in InfoWebpage.Modules)
                    {
                        BayshoreSolutions.WebModules.ContentModule.Content content = null;
                        content = BayshoreSolutions.WebModules.ContentModule.Content.GetActiveContentForDisplay(m.Id, BayshoreSolutions.WebModules.CultureCode.Current);
                        if (null != content)
                        {
                            switch (m.Placeholder)
                            {
                                case "Body":
                                    this.Body += content.Text;
                                    break;
                                case "HeaderImage":
                                    this.HeaderImage += content.Text;
                                    break;
                                case "RightTopBox":
                                    this.RightTopBox += content.Text;
                                    break;
                                case "HeaderText":
                                    this.HeaderText += content.Text;
                                    break;
                                case "masterHeader":
                                    this.MasterHeader += content.Text;
                                    break;
                                case "FrontPageSlideShow":
                                    this.FrontPageSlideShow = content.Text;
                                    break;
                                default:
                                    this.Body += content.Text;
                                    break;

                            }

                        }
                    }

                }

                this.HtmlString = _htmlStr.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static QuickContentContent GetActiveContent(string ContentName)
        {
            if (!string.IsNullOrEmpty(ContentName))
            {
                QuickContentContentCollection coll = new QuickContentContentCollection()
                    .Where(BayshoreSolutions.WebModules.QuickContent.QuickContentContent.Columns.ContentName, ContentName)
                    .Where(QuickContentContent.Columns.Culture, Thread.CurrentThread.CurrentUICulture.Name.ToLower())
                    .Where(QuickContentContent.Columns.StatusId, 2)
                    .Load();

                if (coll.Count > 0)
                {
                    return coll[0];
                }
            }
            return new QuickContentContent();
        }
    }

}