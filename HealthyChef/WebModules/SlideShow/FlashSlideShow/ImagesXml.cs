using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;

namespace BayshoreSolutions.WebModules.SlideShow
{
    public class ImagesXml: IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int moduleId;
            string moduleId_s = context.Request.QueryString["moduleId"];
            if (null == moduleId_s) throw new ArgumentNullException("moduleId");
            if (!int.TryParse(moduleId_s, out moduleId))
                return;

            SlideShowModule module = SlideShowModule.GetByModuleId(moduleId);
            if (null == module) throw new ArgumentException("Invalid SlideShow module id.");

            SlideShowImageCollection slideshowImages = SlideShowImage.GetByModuleId(moduleId);

            //construct the images XML.

            //timer : number of seconds between each image transition
            //order : how you want your images displayed. choose either 'sequential' or 'random'
            //fadeTime : velocity of image crossfade. Increment for faster fades, decrement for slower. Approximately equal to seconds.
            //looping : if the slide show is in sequential mode, this stops the show at the last image (use 'yes' for looping, 'no' for not)
            //xpos : x position of all loaded clips (0 is default)
            //ypos : y position of all loaded clips (0 is default)

            string imageDisplayOrder_s = ((ImageDisplayOrder)module.ImageDisplayOrder).ToString();
            string imageLooping_s = module.ImageLooping ? "yes" : "no";

            StringBuilder output = new StringBuilder();
            output.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            output.AppendLine(string.Format("<gallery timer=\"{0}\" fadetime=\"{1}\" order=\"{2}\" looping=\"{3}\" xpos=\"{4}\" ypos=\"{5}\">",
                  module.ImageDisplayTime,
                  module.ImageFadeTime,
                  imageDisplayOrder_s,
                  imageLooping_s,
                  module.ImageXPosition,
                  module.ImageYPosition));

            //paths must be relative to the site root (i.e., absolute).
            foreach (SlideShowImage i in slideshowImages)
                output.AppendLine(string.Format("\t<image path=\"{0}\" />", 
                    VirtualPathUtility.ToAbsolute(i.GetFullPath())));

            output.AppendLine("</gallery>");

            //
            //output
            //

            context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.AddDays(1));

            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.Clear();
            context.Response.ContentType = "text/xml";

            context.Response.Write(output.ToString());
        }
    }
}
