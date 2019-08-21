using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.MasterDetail.Controls
{
    public partial class EmailPopup : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //email article link
        protected void EmailArticleLink(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // Check the spam-bot fields for data
            // The anti-spam form fields and session var are already declared in the 
            // DetailDisplay.ascx parent control.  We will reuse them here.
            string s1 = Request.Form.Get("url");
            string s2 = Request.Form.Get("email");
            string s3 = Request.Form.Get("comment");
            object o4 = Session["AntiSpamVar"];
            if (!string.IsNullOrEmpty(Request.Form.Get("url"))
                || !string.IsNullOrEmpty(Request.Form.Get("email"))
                || !string.IsNullOrEmpty(Request.Form.Get("comment"))
                || (Session["AntiSpamVar"] == null))
            {
                Response.End();
                return;
            }

            string subject = string.Format("{0} has sent you a link from the {1} website!",
                tbSenderName.Text, Request.Url.Host);

            string message = string.Format("{0}\n\n{1}",
                tbMessage.Text, Request.Url.AbsoluteUri);

            SmtpClient mail = new SmtpClient();
            mail.Send(tbSenderEmail.Text.Trim(), tbRecipientEmail.Text.Trim(), subject, message);

            // Clear out input data.
            tbSenderName.Text = string.Empty;
            tbSenderEmail.Text = string.Empty;
            tbRecipientEmail.Text = string.Empty;
            tbMessage.Text = string.Empty;
        }
    }

}