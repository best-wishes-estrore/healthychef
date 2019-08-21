using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.SlideShow.FlexSlideShow
{
    public partial class Display : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SlideShowModule module = SlideShowModule.GetByModuleId(this.ModuleId);
                //slider_container.CssClass = module.SlideShowClassName;

                SlideShowImageCollection images = SlideShowImage.GetByModuleId(this.ModuleId, "sortOrder", "desc");
                ImagesList.DataSource = images;
                ImagesList.DataBind();
            }
        }

        protected void Item_DataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (e.Item.DataItem != null && e.Item.DataItem is SlideShowImage)
                {
                    SlideShowImage slideShowImage = (SlideShowImage)e.Item.DataItem;
                    Image image = e.Item.FindControl("image") as Image;
                    image.ImageUrl = Page.ResolveUrl(slideShowImage.GetFullPath());

                    HyperLink hyperlink = e.Item.FindControl("hyperlink") as HyperLink;
                    hyperlink.NavigateUrl = slideShowImage.LinkUrl;
                }
            }
        }
    }
}