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
    public partial class FlashSlideShow_Display : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SlideShowModule module = SlideShowModule.GetByModuleId(this.ModuleId);
                if (null == module) throw new ArgumentException("Invalid SlideShow module id.");

                FlashSlideShowCtl.FlashUrl = module.FlashFileName;
                FlashSlideShowCtl.Height = module.Height;
                FlashSlideShowCtl.Width = module.Width;

                //the flash must be programmed to send moduleId to ~/images.xml.aspx?moduleId=n.
                FlashSlideShowCtl.FlashVars = "moduleId=" + this.ModuleId;
            }
        }
    }
}