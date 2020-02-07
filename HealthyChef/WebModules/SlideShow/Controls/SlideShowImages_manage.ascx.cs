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
using System.IO;
using BayshoreSolutions.WebModules.SlideShow;

namespace BayshoreSolutions.WebModules.Cms.SlideShow.Controls
{
    public partial class SlideShowImages_manage : System.Web.UI.UserControl
    {
        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }

		public bool IsJQuerySlidingTextContentSlideShow
		{
			get { return (bool)(ViewState["IsJQuerySlidingTextContentSlideShow"] ?? false); }
			set { ViewState["IsJQuerySlidingTextContentSlideShow"] = value; }
		}

        public bool IsGalleryViewSlideShow
        {
            get { return (bool)(ViewState["IsGalleryViewSlideShow"] ?? false); }
            set { ViewState["IsGalleryViewSlideShow"] = value; }
        }

		protected void Page_Init(object sender, EventArgs e)
		{
			//prevent loading multiple css style sheet
			HtmlControl css = null;
			css = Page.Header.FindControl("PopupEditorCSS") as HtmlControl;

			if (css == null)
			{
				//load the style sheet
				HtmlLink cssLink = new HtmlLink();
				cssLink.ID = "PopupEditorCSS";
				cssLink.Href = ResolveUrl("~/WebModules/SlideShow/public/css/subModal.css");
				cssLink.Attributes["rel"] = "stylesheet";
				cssLink.Attributes["type"] = "text/css";

				// Add the HtmlLink to the Head section of the page.
				Page.Header.Controls.Add(cssLink);

			}

			// Load javascript
			this.Page.ClientScript.RegisterClientScriptInclude("submodal", ResolveUrl("~/WebModules/SlideShow/public/js/subModal.js"));
		}

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void Load_(int moduleId)
        {
            this.ModuleId = moduleId;
            //ImagesList.DataSource =  SlideShow_Image.Find(null, moduleId, null, null, null, null, null, SlideShow_Image.Columns.SortOrder, true);
            ImagesList.DataSource = SlideShowImage.GetByModuleId(moduleId, SlideShowImage.Columns.SortOrder, null);       
                ImagesList.DataBind();
        }

        protected void ImagesList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = int.Parse(e.CommandArgument.ToString());
            
            
            switch (e.CommandName)
            {
                case "Delete":
                    //SlideShow_Image image = SlideShow_Image.Get(id);
                    SlideShowImage image = SlideShowImage.GetById(id);
										if (null != image)
										{
											try
											{
												File.Delete(Server.MapPath(image.GetFullPath()));
											}
											catch (Exception)
											{
											}
										}
                   
                    SlideShowImage.Delete(id);
                    Msg.ShowSuccess(string.Format("Deleted {0}.", image.ImageFileName));
                    break;
                case "MoveUp":
                    SlideShowImage.MoveUp(id);
                    break;
                case "MoveDown":
                    SlideShowImage.MoveDown(id);
                    break;
                case "Save":
                    SlideShowImage saveImage = SlideShowImage.GetById(id);
                    TextBox txt = e.Item.FindControl("txtLink") as TextBox;

                    if (null != saveImage && txt != null)
                    {
                        saveImage.LinkUrl = txt.Text.Trim();

						if (this.IsJQuerySlidingTextContentSlideShow)
						{
							TextBox txtContentName = e.Item.FindControl("txtContentName") as TextBox;

							if (txtContentName != null)
							{
								saveImage.SlideTextContentName = txtContentName.Text;
							}
						}

                        saveImage.Save();
                    }
                    break;
                default: 
                    break;
            }

            this.Load_(this.ModuleId);
        }

		public string GetBaseUrl()
		{
			string Port = Request.ServerVariables["SERVER_PORT"];
			if (Port == null || Port == "80" || Port == "443")
				Port = "";
			else
				Port = ":" + Port;

			string Protocol = Request.ServerVariables["SERVER_PORT_SECURE"];
			if (Protocol == null || Protocol == "0")
				Protocol = "http://";
			else
				Protocol = "https://";

			// *** Figure out the base Url which points at the application's root
			string strBaseUrl = Protocol + Request.ServerVariables["SERVER_NAME"] + Port + Request.ApplicationPath;
			if (!strBaseUrl.EndsWith("/"))
			{
				strBaseUrl += "/";
			}

			return strBaseUrl;
		}
    }
}