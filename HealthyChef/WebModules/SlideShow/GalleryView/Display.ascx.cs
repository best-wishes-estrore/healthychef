using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using BayshoreSolutions.WebModules.SlideShow;

namespace BayshoreSolutions.WebModules.SlideShow.GalleryView
{
    public partial class Display : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Add CSS link
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/SlideShow/public/css/jquery.galleryview-3.0-dev.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            this.Page.Header.Controls.AddAt(1, cssLink);

            // Include script links
            Page.ClientScript.RegisterClientScriptInclude("jquery171", "https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js");
            Page.ClientScript.RegisterClientScriptInclude("timers", "/WebModules/SlideShow/public/js/jquery.timers-1.2.js");
            Page.ClientScript.RegisterClientScriptInclude("easing", "/WebModules/SlideShow/public/js/jquery.easing.1.3.js");
            Page.ClientScript.RegisterClientScriptInclude("galleryview", "/WebModules/SlideShow/public/js/jquery.galleryview-3.0-dev.js");

            // Script to instantiate gallery
            string script = @"
            <script>
                $(function () {
                    $('#" + GalleryId + @"').galleryView({
                        panel_width: " + Module.Width + @",
                        panel_height: " + Module.Height + @",
                        transition_speed: " + (Module.ImageFadeTime * 1000) + @",
                        transition_interval: " + (Module.ImageDisplayTime * 1000) + @" ,
                        show_overlays: true
                    });
                });
            </script>";
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "galleryview-instance", script);

            if (!Page.IsPostBack)
            {
                rptImages.DataSource = Images;
                rptImages.DataBind();
            }
        }

        public string GalleryId
        {
            get { return "galleryview" + ModuleId; }
        }

        public SlideShowModule Module
        {
            get 
            { 
                return SlideShowModule.GetByModuleId(this.ModuleId);
            }
        }

        public SlideShowImageCollection Images
        {
            get
            {
                return SlideShowImage.GetByModuleId(this.ModuleId, "sortOrder", "asc"); ;
            }
        }
    }
}