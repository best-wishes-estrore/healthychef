using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using WM = BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.MasterDetail.Controls
{
    public partial class DetailEditControl : System.Web.UI.UserControl
    {
        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            calPostDate.Date = DateTime.Today;
            DateTime dtNow = DateTime.Now;
            tbPostTime.Text = dtNow.ToShortTimeString();

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // no cacheing, this prevents fckeditor html issues on hitting browser back button
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }


        public void LoadContentItem(int moduleId)
        {
            this.ModuleId = moduleId;
            MasterDetailItem itemRes = GetCurrentMasterDetailItem();

            if (itemRes.IsNew)
            {
                calPostDate.Date = DateTime.Today;
                tbPostTime.Text = DateTime.Now.ToShortTimeString();
            }
            else
                Load_MasterDetail_ItemResource_AndAssociatedObjects(itemRes);
        }

        public void Load_MasterDetail_ItemResource_AndAssociatedObjects(MasterDetailItem itemRes)
        {
            WebModuleInfo module = WebModule.GetModule(itemRes.ModuleId);
            WebpageInfo page = Webpage.GetWebpage(module.Webpage.InstanceId);

            tbTitle.Text = page.Title; //WebpageInfo respects current culture.
            tbMetaKeywords.Text = page.MetaKeywords;
            tbMetaDescription.Text = page.MetaDescription;
            tbPageNavigationText.Text = page.Text;

            tbShortDesc.Text = itemRes.ShortDescription;
            tbLongDesc.Text = itemRes.LongDescription;
            tbTags.Text = itemRes.Tags;

            ImagePathCtl.ImagePath = itemRes.ImagePath;
            cbIsVisible.Checked = page.Visible;
            cbIsFeatured.Checked = itemRes.IsFeatured;

            if (page.PostDate.HasValue)
            {
                calPostDate.Date = page.PostDate.Value.Date;

                if (page.PostDate.Value.TimeOfDay.TotalSeconds > 0.001)
                {
                    tbPostTime.Text = page.PostDate.Value.ToShortTimeString();
                }
                else
                {
                    tbPostTime.Text = string.Empty;
                }
            }
            if (page.RemoveDate.HasValue) calRemoveDate.Date = page.RemoveDate.Value;
        }

        public MasterDetailItem GetCurrentMasterDetailItem()
        {
            //get associated resource for the current culture.
            MasterDetailItem itemRes = MasterDetailItem.GetResource(this.ModuleId);

            if (null == itemRes)
            { //create new
                itemRes = new MasterDetailItem();
                itemRes.ModuleId = this.ModuleId;
                itemRes.Culture = WM.CultureCode.Current;
            }

            //if (null == itemRes)
            //    throw new ArgumentException(string.Format("Invalid MasterDetail item resource (it may have been deleted). editModuleId: {0}.", this.EditModuleId));

            //check permissions...
            //if(itemRes.UserId != (Guid)_user.ProviderUserKey) throw new System.Security.SecurityException("The current user is not authorized to view that item.");

            return itemRes;
        }

        public void SaveMasterDetailItem()
        {
            MasterDetailItem itemRes = GetCurrentMasterDetailItem();
            itemRes.Culture = WM.CultureCode.Current;
            itemRes.StatusId = (int)MasterDetailStatus.Pending;
            itemRes.ShortDescription = tbShortDesc.Text.Trim();
            itemRes.LongDescription = tbLongDesc.Text.Trim();

            if (string.IsNullOrEmpty(tbTags.Text.Trim()))
            {
                itemRes.Tags = "General";
            }
            else
            {
                // Get tags and strip out any extra spaces
                itemRes.Tags = tbTags.Text.Trim().Replace(" ,", ",").Replace(", ", ",");
            }

            itemRes.ImagePath = ImagePathCtl.ImagePath;
            itemRes.IsFeatured = cbIsFeatured.Checked;
            itemRes.Save();
        }

        public WebpageInfo GetInput_WebpageInfo(WebpageInfo page)
        {
            // If no title is specified, use the nav text
            page.Title = string.IsNullOrEmpty(tbTitle.Text.Trim()) ? tbPageNavigationText.Text.Trim() : tbTitle.Text.Trim();
            page.Text = tbPageNavigationText.Text;
            page.MetaKeywords = tbMetaKeywords.Text.Trim();
            page.MetaDescription = tbMetaDescription.Text.Trim();
            page.Visible = cbIsVisible.Checked;
            if (calPostDate.Date > DateTime.MinValue)
            {
                DateTime dtPostDate = calPostDate.Date;

                if (!string.IsNullOrEmpty(tbPostTime.Text))
                {
                    DateTime dtTimeOfDay = DateTime.Parse(tbPostTime.Text);
                    dtPostDate = dtPostDate.Add(dtTimeOfDay.TimeOfDay);
                }

                page.PostDate = dtPostDate;
            }
            else
            {
                page.PostDate = null;
            }

            if (calRemoveDate.Date > DateTime.MinValue)
            {
                page.RemoveDate = calRemoveDate.Date;
            }
            else
            {
                page.RemoveDate = null;
            }
            return page;
        }

        public WebModuleInfo CreateModule(WebpageInfo page)
        {
            WebModuleType moduleType = MasterDetailItem.ModuleType;

            return WebModule.CreateModule(
                MasterDetailItem.Chop(page.Title, 30, false),
                moduleType.WebApplicationType.Name,
                moduleType.Name,
                page.Id,
                "Body",
                -1);
        }

        public string ModuleTemplate;

        public WebpageInfo CreatePage(int parentPageNavigationId)
        {
            WebpageInfo currentPage = Webpage.GetWebpage(parentPageNavigationId);
            WebpageInfo newPage = GetInput_WebpageInfo(currentPage);
            Webpage.WebpageCreateStatus status;
            newPage.ParentInstanceId = parentPageNavigationId;

            //this will be sanitized by CreateWebpage()
            newPage.PathName = string.IsNullOrEmpty(tbTitle.Text.Trim()) ? tbPageNavigationText.Text.Trim() : tbTitle.Text.Trim();

            // Set the template
            if (!String.IsNullOrEmpty(ModuleTemplate))
            {
                var tplGroup = ModuleTemplate.Replace(" - ", "-").Split('-');
                newPage.TemplateGroup = tplGroup[0];
                newPage.Template = tplGroup[1];
            }

            newPage = Webpage.CreateWebpage(newPage, out status);
            if (status != Webpage.WebpageCreateStatus.Success)
                throw new InvalidOperationException("page creation failed: " + status);

            return newPage;
        }

        public void CreateNewContentPageModule(int parentPageNavigationId, string moduleTemplate)
        {
            ModuleTemplate = moduleTemplate;
            CreateNewContentPageModule(parentPageNavigationId);
        }

        //create new content item, content resource, cms page, and cms module.
        public void CreateNewContentPageModule(int parentPageNavigationId)
        {

            //create a new page, using the parent page as a base.
            //properties such as title/keywords/description are set from user inputs.
            WebpageInfo page = CreatePage(parentPageNavigationId);
            //create a MasterDetail module on the new page.
            WebModuleInfo module = CreateModule(page);

            //set the module id for the below methods to use.
            this.ModuleId = module.Id;

            SaveMasterDetailItem();
            Webpage.SortWebpageUp(page.InstanceId);
        }

        //creates/updates the current content item and associated objects. 
        //does not create new CMS objects.
        public void Save()
        {
            if (this.ModuleId <= 0) throw new InvalidOperationException("Invalid module id: " + this.ModuleId);

            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            WebpageInfo page = GetInput_WebpageInfo(module.Webpage);

            //save webpage properties.
            //note: do not update WebpageInfo.PathName, that may hurt SEO.
            Webpage.UpdateWebpage(page);

            //note: we don't need to update the module.

            //set the module id for the below methods to use.
            this.ModuleId = module.Id;

            SaveMasterDetailItem();
        }

        protected void cvShortDesc_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tbShortDesc.Text.Trim().Length <= 256;
        }

        protected void cvLongDesc_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = tbLongDesc.Text.Trim().Length > 0;
        }

    }
}