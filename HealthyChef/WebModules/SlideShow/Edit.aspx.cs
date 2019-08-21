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
using System.Text.RegularExpressions;

using BayshoreSolutions.WebModules;
using cms = BayshoreSolutions.WebModules.Cms;
using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.SlideShow
{
    public partial class Edit : System.Web.UI.Page
    {
        public readonly string AllowedImageTypes_Flash_Text = ".jpg (or .jpeg)";
        public readonly string AllowedImageTypes_Normal_Text = ".jpg, .jpeg, .gif, or .png";
        public readonly string[] AllowedImageTypes_Flash = new string[] { ".jpg", ".jpeg", };
        public readonly string[] AllowedImageTypes_Normal = new string[] { ".jpg", ".jpeg", ".gif", ".png" };

        /// <summary>
        /// The module instance id assigned by the CMS.
        /// </summary>
        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }

        /// <summary>
        /// The instance id (navigation id) of the page that contains the module.
        /// </summary>
        public int PageNavigationId
        {
            get { return (int)(ViewState["PageNavigationId"] ?? -1); }
            set { ViewState["PageNavigationId"] = value; }
        }

        public bool IsFlashSlideShow
        {
            get { return (bool)(ViewState["IsFlashSlideShow"] ?? false); }
            set { ViewState["IsFlashSlideShow"] = value; }
        }

		public bool IsJQuerySlidingContentSlideShow
		{
			get { return (bool)(ViewState["IsJQuerySlidingContentSlideShow"] ?? false); }
			set { ViewState["IsJQuerySlidingContentSlideShow"] = value; }
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

        public bool IsFlexSlideShow
        {
            get { return (bool)(ViewState["IsFlexSlideShow"] ?? false); }
            set { ViewState["IsFlexSlideShow"] = value; }
        }

        private void InitModule()
        {
            int moduleId = 0;
            int.TryParse(Request["ModuleId"], out moduleId);
            this.ModuleId = moduleId;

            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            WebpageInfo page = null;
            if (this.ModuleId <= 0
                || null == module
                || null == (page = module.Webpage))
            {
                cms.Admin.RedirectToMainMenu();
                //uncomment line below (and remove line above) for legacy WebModules.
                //Response.Redirect("~/WebModules/Admin/MyWebsite/Default.aspx?instanceId=" + Webpage.RootNavigationId);
            }
            this.PageNavigationId = page.InstanceId;

            //check user permissions.
            if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(this.PageNavigationId, Page.User))
                throw new System.Security.SecurityException("The current user does not have permission to access this resource.");

            ModuleName.Text = module.Name;
            ModulTypeName.Text = module.WebModuleType.Name;
            MainMenuLink.HRef = cms.Admin.GetMainMenuUrl(this.PageNavigationId);
            //uncomment line below (and remove line above) for legacy WebModules.
            //MainMenuLink.HRef = "~/WebModules/Admin/MyWebsite/Default.aspx?instanceId=" + this.PageNavigationId;

            EnsureModule();
        }

        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        private void EnsureModule()
        {
            //model.SlideShow_Module module = model.SlideShow_Module.Get(this.ModuleId);
            SlideShowModule module = SlideShowModule.GetByModuleId(this.ModuleId);
            if (null == module)
            {
                module = new SlideShowModule();
                module.ModuleId = this.ModuleId;
                module.Save();
            }
        }

        private void LoadModule()
        {
            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            SlideShowModule slideshow = SlideShowModule.GetByModuleId(this.ModuleId);
			//model.SlideShow_Module slideshow = model.SlideShow_Module.Get(this.ModuleId);

            ClassName_div.Visible = false;
            ClassNameTextBox.Text = slideshow.SlideShowClassName ?? string.Empty;

			this.IsFlashSlideShow = (module.WebModuleType.Name == "Slide Show (Flash)");

			this.IsJQuerySlidingContentSlideShow = (module.WebModuleType.Name == "Slide Show (Sliding jQuery)");

			this.IsJQuerySlidingTextContentSlideShow = (module.WebModuleType.Name == "Slide Show (Sliding jQuery WYSIWYG)");

            this.IsGalleryViewSlideShow = (module.WebModuleType.Name == "Slide Show (Image Gallery)");

            this.IsFlexSlideShow = (module.WebModuleType.Name == "Slide Show (Flex SlideShow)");

            FlashFileCtl.ImagePath = slideshow.FlashFileName;
            HeightCtl.Text = slideshow.Height.ToString();
            WidthCtl.Text = slideshow.Width.ToString();
            ImageDisplayTimeCtl.Text = slideshow.ImageDisplayTime.ToString();
            ImageDisplayOrderCtl.Text = slideshow.ImageDisplayOrder.ToString();
            ImageLoopingCtl.Checked = slideshow.ImageLooping;
            ImageFadeTimeCtl.Text = slideshow.ImageFadeTime.ToString();
            //ImageXPositionCtl.Text = slideshow.ImageXPosition.ToString();
			//ImageYPositionCtl.Text = slideshow.ImageYPosition.ToString();

			if (this.IsJQuerySlidingContentSlideShow || this.IsJQuerySlidingTextContentSlideShow)
			{
				ImageNavTypeCtl.Text = slideshow.NavType.ToString();
				ImageWrapTypeCtl.Text = slideshow.WrapType.ToString();
			}

			if (this.IsJQuerySlidingTextContentSlideShow)
				SlideShowImagesCtl.IsJQuerySlidingTextContentSlideShow = true;

            if (this.IsGalleryViewSlideShow)
                SlideShowImagesCtl.IsGalleryViewSlideShow = true;

			SlideShowImagesCtl.Load_(this.ModuleId);

			if (this.IsFlashSlideShow)
				InitFlashSlideshowAdmin();
			else if (this.IsJQuerySlidingContentSlideShow)
				InitJQuerySlidingContentSlideshowAdmin();
			else if (this.IsJQuerySlidingTextContentSlideShow)
				InitJQuerySlidingTextContentSlideshowAdmin();
            else if (this.IsGalleryViewSlideShow)
                InitGalleryViewSlideshowAdmin();
            else if (this.IsFlexSlideShow)
            {
                InitFlexSlideShow();
            }
            else
                InitNormalSlideshowAdmin();
        }

        private void InitFlexSlideShow()
        {
            NavType_div.Visible = false;
            DisplayOrder_div.Visible = false;
            FadeTime_div.Visible = false;
            FadeTime_div.Visible = false;
            WrapType_div.Visible = false;
            FlashFile_div.Visible = false;
            WidthHelp.Visible = false;
            HeightHelp.Visible = false;
            
            ImageLoopingCtl.Visible = false; //currently, we can't disable looping on the javascript slideshow.
            AllowedImageTypes.Text = AllowedImageTypes_Normal_Text;

            phImageManagement.Visible = true;
            phContentManagement.Visible = false;
            ClassName_div.Visible = true;
        }

		//javascript using jcarousel with sliding instead of fading.
		private void InitJQuerySlidingContentSlideshowAdmin()
		{
			NavType_div.Visible = true;
			FadeTime_div.Visible = false;
			FlashFile_div.Visible = false;
			WidthHelp.Visible = false;
			HeightHelp.Visible = false;
			ImageLoopingCtl.Visible = false; //currently, we can't disable looping on the javascript slideshow.
			AllowedImageTypes.Text = AllowedImageTypes_Normal_Text;

			phImageManagement.Visible = true;
			phContentManagement.Visible = false;
		}

		//javascript using jcarousel with WYSIWYG content instead of images.
		private void InitJQuerySlidingTextContentSlideshowAdmin()
		{
			NavType_div.Visible = true;
			FadeTime_div.Visible = false;
			FlashFile_div.Visible = false;
			WidthHelp.Visible = false;
			HeightHelp.Visible = false;
			ImageLoopingCtl.Visible = false; //currently, we can't disable looping on the javascript slideshow.
			AllowedImageTypes.Text = AllowedImageTypes_Normal_Text;

			phImageManagement.Visible = false;
			phContentManagement.Visible = true;
			Image_SaveButton.Text = "Add Content";
			SlideShowImagesCtl.IsJQuerySlidingTextContentSlideShow = true;
		}

        //javascript, no flash.
        private void InitNormalSlideshowAdmin()
		{
			NavType_div.Visible = false;
            FlashFile_div.Visible = false;
            WidthHelp.Visible = false;
            HeightHelp.Visible = false;
            ImageLoopingCtl.Visible = false; //currently, we can't disable looping on the javascript slideshow.
			AllowedImageTypes.Text = AllowedImageTypes_Normal_Text;

			phImageManagement.Visible = true;
			phContentManagement.Visible = false;
        }

        //requires the user to upload a special flash file.
        private void InitFlashSlideshowAdmin()
		{
			NavType_div.Visible = false;
            FlashFile_div.Visible = true;
            WidthHelp.Visible = true;
            HeightHelp.Visible = true;
            ImageLoopingCtl.Visible = true;
			AllowedImageTypes.Text = AllowedImageTypes_Flash_Text;

			phImageManagement.Visible = true;
			phContentManagement.Visible = false;
        }

        private void InitGalleryViewSlideshowAdmin()
        {
            NavType_div.Visible = false;
            FlashFile_div.Visible = false;
            DisplayOrder_div.Visible = false;
            WrapType_div.Visible = false;
            WidthHelp.Visible = false;
            HeightHelp.Visible = false;
            ImageLoopingCtl.Visible = false; 
            AllowedImageTypes.Text = AllowedImageTypes_Normal_Text;

            phImageManagement.Visible = true;
            phContentManagement.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = SlideShow_Module_SaveButton.UniqueID;

            if (!IsPostBack)
            {
                InitModule();
                LoadModule();
            }
        }

        /// <summary>
        /// Gets the current entity and fills its with the form input values.
        /// If input is not valid, returns null.
        /// </summary>
        public SlideShowModule GetInput_SlideShow_Module()
        {
            //model.SlideShow_Module slideShow_Module = model.SlideShow_Module.Get(this.ModuleId);
            SlideShowModule slideShow_Module = SlideShowModule.GetByModuleId(this.ModuleId);
            slideShow_Module.FlashFileName = FlashFileCtl.ImagePath;
            slideShow_Module.Height = short.Parse(HeightCtl.Text.Trim());
            slideShow_Module.Width = short.Parse(WidthCtl.Text.Trim());
            slideShow_Module.ImageDisplayTime = short.Parse(ImageDisplayTimeCtl.Text.Trim());
            slideShow_Module.ImageDisplayOrder = short.Parse(ImageDisplayOrderCtl.Text.Trim());
            slideShow_Module.ImageLooping = ImageLoopingCtl.Checked;
            slideShow_Module.ImageFadeTime = short.Parse(ImageFadeTimeCtl.Text.Trim());
            slideShow_Module.ImageXPosition = 0; //int.Parse(ImageXPositionCtl.Text.Trim());
            slideShow_Module.ImageYPosition = 0; //int.Parse(ImageYPositionCtl.Text.Trim());

			if (this.IsJQuerySlidingContentSlideShow || this.IsJQuerySlidingTextContentSlideShow)
			{
				slideShow_Module.NavType = short.Parse(ImageNavTypeCtl.Text.Trim());
				slideShow_Module.WrapType = short.Parse(ImageWrapTypeCtl.Text.Trim());
			}

            return slideShow_Module;
        }

        public bool ValidateInput_SlideShow_Module()
        {
            if (this.IsFlashSlideShow
                && !".swf".Equals(System.IO.Path.GetExtension(FlashFileCtl.ImagePath), StringComparison.OrdinalIgnoreCase))
            {
                Msg.ShowError("Invalid Flash file. File must be .swf format.");
                return false;
            }

            return true;
        }

        protected void SlideShow_Module_SaveButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid || !ValidateInput_SlideShow_Module()) 
                return;

            SlideShowModule slideShow_Module = GetInput_SlideShow_Module();
            slideShow_Module.SlideShowClassName = ClassNameTextBox.Text;
            slideShow_Module.Save();

            cms.Admin.RedirectToMainMenu(this.PageNavigationId);
        }

        protected void SlideShow_Module_CancelButton_Click(object sender, EventArgs e)
        {
            cms.Admin.RedirectToMainMenu(this.PageNavigationId);
        }

        bool IsFileExtensionValid(string fileExtension)
        {
            if (null != fileExtension) fileExtension = fileExtension.ToLower();

            string[] allowedFileTypes = this.IsFlashSlideShow
                ? AllowedImageTypes_Flash
                : AllowedImageTypes_Normal;

            foreach (string imgType in allowedFileTypes)
                if (imgType == fileExtension)
                    return true;

            return false;
        }

        protected void Image_SaveButton_Click(object sender, EventArgs e)
		{
			HttpPostedFile postedFile = Image_FileUploadCtl.PostedFile;
			string filename = String.Empty;
			string fileExtension = String.Empty;

			if (!this.IsJQuerySlidingTextContentSlideShow)
			{
				if (null == postedFile) throw new InvalidOperationException("Image_FileUploadCtl.PostedFile is null.");

				filename = Path.GetFileName(Image_FileUploadCtl.PostedFile.FileName);
				Regex reg = new Regex(@"[^A-Za-z0-9.()_]");
				filename = reg.Replace(filename, String.Empty);
				fileExtension = Path.GetExtension(filename);


				if (!IsFileExtensionValid(fileExtension))
				{
					Msg.ShowError(string.Format("Invalid file format. Image format must be {0} format.",
						(this.IsFlashSlideShow ? AllowedImageTypes_Flash_Text : AllowedImageTypes_Normal_Text)));
					return;
				}
			}

           //SlideShow_Image image = new SlideShow_Image();
            SlideShowImage image = new SlideShowImage();
            image.ModuleId = this.ModuleId;
            image.ImageFileName = filename;

			if (this.IsJQuerySlidingTextContentSlideShow)
			{
				image.SlideTextContentName = Content_Name.Text.Trim();
				image.SlideTextContent = SlideContent.Text;
			}
			else
			{
                if (IsGalleryViewSlideShow)
                {
                    image.SlideTextContentName = Content_Name.Text.Trim();
                    image.SlideTextContent = SlideContent.Text;
                }

				string savePath = Server.MapPath(image.GetFullPath());
				string saveDir = Path.GetDirectoryName(savePath);

				if (!Directory.Exists(saveDir))
					Directory.CreateDirectory(saveDir);

				if (File.Exists(savePath))
					image.ImageFileName = RenameFile(filename, fileExtension, savePath);

				Image_FileUploadCtl.SaveAs(Server.MapPath(image.GetFullPath()));
			}

            image.Save();

			if (this.IsJQuerySlidingTextContentSlideShow)
				SlideShowImagesCtl.IsJQuerySlidingTextContentSlideShow = true;

            SlideShowImagesCtl.Load_(this.ModuleId);

			if (this.IsJQuerySlidingTextContentSlideShow)
			{
				Msg.ShowSuccess("Added new content to slide show.");

				SlideContent.Text = String.Empty;
				Content_Name.Text = String.Empty;
			}
			else
				Msg.ShowSuccess("Added new image to slide show.");

            Content_Name.Text = "";
            SlideContent.Text = "";
        }

        private string RenameFile(string filename, string fileExtension, string path)
        {
            path = path.Replace(filename, string.Empty);
            filename = filename.Replace(fileExtension, string.Empty) + "(1)" + fileExtension;
            if (File.Exists(Path.Combine(path, filename)))
                filename = RenameFile(filename, fileExtension, path);

            return filename;
        }

    }
}
