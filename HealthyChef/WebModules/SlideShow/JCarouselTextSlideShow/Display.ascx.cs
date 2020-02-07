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
	public partial class JCarouselTextSlideShow_Display : BayshoreSolutions.WebModules.WebModuleBase
	{		
		protected void Page_Init(object sender, EventArgs e)
		{
			lvSlideShow.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvSlideShow_ItemDataBound);

			lvSlideShowNav.ItemDataBound += new EventHandler<ListViewItemEventArgs>(lvSlideShowNav_ItemDataBound);
			
			//prevent loading multiple css style sheet
			HtmlControl css = null;
			css = Page.Header.FindControl("PopupEditorCSS") as HtmlControl;

			if (css == null)
			{
				//load the style sheet
				HtmlLink cssLink = new HtmlLink();
				cssLink.ID = "PopupEditorCSS";
				cssLink.Href = ResolveUrl("~/WebModules/SlideShow/public/css/jCarouselSlider.css");
				cssLink.Attributes["rel"] = "stylesheet";
				cssLink.Attributes["type"] = "text/css";

				// Add the HtmlLink to the Head section of the page.
				Page.Header.Controls.Add(cssLink);
			}
		}

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

				switch ((NavType)module.NavType)
				{
					case NavType.per_slide:
						{
							phPerSlideNav.Visible = true;
							phPrevNextNav.Visible = false;
						}
						break;
					case NavType.prev_next:
						{
							phPerSlideNav.Visible = false;
							phPrevNextNav.Visible = true;
						}
						break;
					case NavType.per_slide_with_prev_next:
						{
							phPerSlideNav.Visible = true;
							phPrevNextNav.Visible = true;
						}
						break;
					case NavType.none:
					default:
						{
							phPerSlideNav.Visible = false;
							phPrevNextNav.Visible = false;
						}
						break;
				}

				// if random
				if (module.ImageDisplayOrder == 1)
				{
					Random random = new Random();
					System.Collections.Generic.List<SlideShowImage> lImages = new System.Collections.Generic.List<SlideShowImage>();
					foreach (SlideShowImage image in images)
					{
						double d = random.NextDouble();
						if (d > 0.5)
						{
							lImages.Add(image);
						}
						else
						{
							lImages.Insert(0, image);
						}
					}
					lvSlideShow.DataSource = lImages;
					lvSlideShowNav.DataSource = lImages;
				}
				else
				{
					// sequential
					lvSlideShow.DataSource = images;
					lvSlideShowNav.DataSource = images;
				}
				lvSlideShow.DataBind();
				lvSlideShowNav.DataBind();
			}

			if (divSlideShow != null)
			{
				if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(JsSlideShow_Display), "wm_slider_css" + divSlideShow.ClientID))
					Page.ClientScript.RegisterClientScriptBlock(typeof(JsSlideShow_Display),
						"wm_xfade_css" + divSlideShow.ClientID,
						module.GetJCarouselTextOnlySliderCss(divSlideShow.ClientID),
						false);

				if (!Page.ClientScript.IsClientScriptIncludeRegistered(typeof(JsSlideShow_Display), "wm_slider"))
				{
					string strUrl = ResolveUrl("~/WebModules/SlideShow/public/js/jquery.jcarousel.js");
					Page.ClientScript.RegisterClientScriptInclude(typeof(JsSlideShow_Display),
						"wm_slider",
						strUrl);
				}

				if (!Page.ClientScript.IsStartupScriptRegistered(typeof(JsSlideShow_Display), "wm_slider_" + divSlideShow.ClientID))
					Page.ClientScript.RegisterStartupScript(typeof(JsSlideShow_Display),
						"wm_slider_" + divSlideShow.ClientID,
						module.GetJCarouselTextSliderInitScript(divSlideShow.ClientID),
						true);
			}
		}

		protected void lvSlideShow_ItemDataBound(object sender, ListViewItemEventArgs e)
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				SlideShowImage slideShowImage = e.Item.DataItem as SlideShowImage;
				if (slideShowImage != null)
				{
					//HtmlAnchor anchorLinkUrl = e.Item.FindControl("anchorLinkUrl") as HtmlAnchor;

					//if (anchorLinkUrl != null)
					//{
					//    if (!string.IsNullOrEmpty(slideShowImage.LinkUrl))
					//    {
					//        anchorLinkUrl.HRef = slideShowImage.LinkUrl;
					//    }
					//}

					HtmlContainerControl liSlide = e.Item.FindControl("liSlide") as HtmlContainerControl;

					if (liSlide != null)
					{
						if (!string.IsNullOrEmpty(slideShowImage.LinkUrl))
						{
							liSlide.Attributes["onclick"] = "window.location = '" + slideShowImage.LinkUrl + "';";
						}
					}
				}
			}
		}

		protected void lvSlideShowNav_ItemDataBound(object sender, ListViewItemEventArgs e)
		{
			if (e.Item.ItemType == ListViewItemType.DataItem)
			{
				HtmlAnchor aSlideNav = (HtmlAnchor)e.Item.FindControl("aSlideNav");

				if (aSlideNav != null)
					aSlideNav.InnerText = (e.Item.DataItemIndex + 1).ToString();
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