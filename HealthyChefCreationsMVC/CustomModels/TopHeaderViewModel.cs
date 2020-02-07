using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BayshoreSolutions.WebModules.QuickContent;
using System.Threading;
using BayshoreSolutions.WebModules.SlideShow;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class TopHeaderViewModel : BayshoreSolutions.WebModules.WebModuleBase
    {
        private const int sliderModuleId = 3;

        public int CartCount { get; set; }
        public bool ShowAdminLink { get; set; }

        public List<SlideImage> listImages { set; get; }

        public TopHeaderViewModel()
        {
            CartCount = 0;
            ShowAdminLink = false;

            SlideShowImageCollection images = SlideShowImage.GetByModuleId(sliderModuleId, "sortOrder", "desc");

            this.listImages = new List<SlideImage>();
            for (int i = 0; i < images.Count; i++)
            {
                SlideShowImage slideShowImage = images[i];
                string url = slideShowImage.GetFullPath();
                this.listImages.Add(new SlideImage { imageName = images[i].ImageFileName, imageURL = url, linkUrl = images[i].LinkUrl });
            }

        }
    }

    public class SlideImage
    {
        public string imageName { set; get; }
        public string imageURL { set; get; }
        public string linkUrl { set; get; }

    }

    public class FooterViewModel
    {
        private const string _footerContent = "FooterContent";
        public string HtmlString { get; set; }
        public HeaderViewModel headerViewModel = new HeaderViewModel();

        public FooterViewModel()
        {
            this.headerViewModel = new HeaderViewModel();
            this.HtmlString = string.Empty;
            var _content = ContentViewModel.GetActiveContent(_footerContent);

            if (_content != null)
            {
                if (_content.IsLoaded)
                {
                    this.HtmlString = _content.Body;
                }
            }
        }

    }

    public class WhiteBoxViewModel
    {
        private string[] _sidePanelHeader = new string[] {
                                            "Sidebar Panel 01",
                                            "Sidebar Panel 02",
                                            "Sidebar Panel 03",
                                            "Sidebar Panel 04",
                                            "Sidebar Panel 05"
                                            };

        public List<string> SidePanelsHtml { get; set; }

        public WhiteBoxViewModel()
        {
            this.SidePanelsHtml = new List<string>();

            foreach (var s in this._sidePanelHeader)
            {
                var _sideBarContent = ContentViewModel.GetActiveContent(s);

                if (_sideBarContent != null)
                {
                    if (_sideBarContent.IsLoaded)
                    {
                        this.SidePanelsHtml.Add(_sideBarContent.Body);
                    }
                } 
            }
        }
    }

}