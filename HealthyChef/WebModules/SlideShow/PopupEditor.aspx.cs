using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.SlideShow
{
	public partial class PopupEditor : Page
	{
		protected string ContentName = "";
		protected string ContentText = "";
		protected string LinkUrl = "";
		protected int ContentId = 0;

		//This is the secret sauce to get the uploader to work properly
		protected void Page_Init(object sender, EventArgs e)
		{
			//txtContent.SkinPath = "skins/office2003/";
			//Session["FCKeditor:UserFilesPath"] = Page.ResolveUrl("~/CMSFiles");

		}
		protected void Page_Load(object sender, EventArgs e)
		{
			ContentId = Request.QueryString["id"] == null ? 0 : Convert.ToInt32(Request.QueryString["id"].ToString());

			if (!Page.IsPostBack)
			{

				if (ContentId > 0)
				{
					SetEditableContent();
				}

			}

		}

		private void SetEditableContent()
		{
			lblMessage.Text = string.Empty;
			txtContent.Value = String.Empty;
			SlideShowImage slideImage;

			slideImage = SlideShowImage.GetById(this.ContentId);

			if (slideImage != null)
			{
				this.ContentName = slideImage.SlideTextContentName;
				this.ContentText = slideImage.SlideTextContent;
				this.LinkUrl = slideImage.LinkUrl;
			}

			txtContent.Value = this.ContentText;
			txtContentName.Text = this.ContentName;
			txtLinkUrl.Text = this.LinkUrl;

			//EditorModalPopup.Show();
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			lblMessage.Text = string.Empty;

			SlideShowImage slideImg = SlideShowImage.GetById(this.ContentId);
			
			// Update the content
			slideImg.SlideTextContentName = txtContentName.Text.Trim();
			slideImg.SlideTextContent = txtContent.Value;
			slideImg.LinkUrl = txtLinkUrl.Text.Trim();

			slideImg.Save(Page.User.Identity.Name);

			// Reload content
			SetEditableContent();

			lblMessage.Text = "<br>Your changes have been saved.";
		}

	}
}
