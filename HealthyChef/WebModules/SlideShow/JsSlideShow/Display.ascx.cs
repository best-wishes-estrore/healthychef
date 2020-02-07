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

namespace BayshoreSolutions.WebModules.SlideShow
{
    public partial class JsSlideShow_Display : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CreateResolveUrlScript();
            SlideShowModule module = SlideShowModule.GetByModuleId(this.ModuleId);
            if (null == module) throw new ArgumentException("Invalid SlideShow module id.");
         
            if (!IsPostBack)
            {
							// 0 - sequential
							// 1 - random
							SlideShowImageCollection images = SlideShowImage.GetByModuleId(this.ModuleId, "sortOrder", "asc");

							// if random
							if ( module.ImageDisplayOrder == 1 )
							{
								Random random = new Random();
								System.Collections.Generic.List<SlideShowImage> lImages = new System.Collections.Generic.List<SlideShowImage>();
								foreach (SlideShowImage image in images)
								{
									double d = random.NextDouble();
									if ( d > 0.5)
									{
										lImages.Add(image);
									}
									else
									{
										lImages.Insert(0, image);
									}
								}
								ImagesLinksList.DataSource = lImages;
							}
							else
							{
								// sequential
								ImagesLinksList.DataSource = images;
							}
              ImagesLinksList.DataBind();
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(JsSlideShow_Display), "wm_xfade_css" + slideshow_div.ClientID))
                Page.ClientScript.RegisterClientScriptBlock(typeof(JsSlideShow_Display),
                    "wm_xfade_css" + slideshow_div.ClientID,
                    module.GetCss(slideshow_div.ClientID),
                    false);

            //slideshow javascript adapted from:
            //http://slayeroffice.com/code/imageCrossFade/xfade2.html
            //http://sonspring.com/journal/slideshow-alternative
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(typeof(JsSlideShow_Display), "wm_xfade"))
            {
                string strUrl = ResolveUrl("~/WebModules/SlideShow/public/js/xfade2.js");
                Page.ClientScript.RegisterClientScriptInclude(typeof(JsSlideShow_Display),
                    "wm_xfade",
                    strUrl);
            }

            if (!Page.ClientScript.IsStartupScriptRegistered(typeof(JsSlideShow_Display), "wm_xfade_" + slideshow_div.ClientID))
                Page.ClientScript.RegisterStartupScript(typeof(JsSlideShow_Display),
                    "wm_xfade_" + slideshow_div.ClientID,
                    module.GetInitScript(slideshow_div.ClientID),
                    true);
        }

        protected void ImagesLinksList_ItemDataBound(object sender, RepeaterItemEventArgs args)
        {
            switch (args.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    {
                        SlideShowImage slideShowImage = args.Item.DataItem as SlideShowImage;
                        if (slideShowImage != null)
                        {
                            HtmlAnchor anchorLinkUrl = args.Item.FindControl("anchorLinkUrl") as HtmlAnchor;
                            if (anchorLinkUrl != null)
                            {
                                if (!string.IsNullOrEmpty(slideShowImage.LinkUrl))
                                {
                                    anchorLinkUrl.HRef = slideShowImage.LinkUrl;
                                }
                            }
                        }

                    }
                    break;
            }
        }

        protected void CreateResolveUrlScript()
        {
            string strDirectory = ResolveUrl("~/");
            string strScript = string.Format(@"
var base_url = '{0}';
function wm_slideshow_resolve_url(url) {{
if( url.indexOf(""~/"") == 0 ) {{
url = base_url + url.substring(2);
}}
return url;
}}
", strDirectory);

            Page.ClientScript.RegisterClientScriptBlock(typeof(string), "wm_slideshow_resolve_url", strScript, true);

        }
    }
}