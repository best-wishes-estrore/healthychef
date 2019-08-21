using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;

using security = BayshoreSolutions.WebModules.Security;

namespace BayshoreSolutions.WebModules.Cms.Security.Model
{
    public class SecurityEmail
    {
        public static void Send(string to, string subject, string body)
        {
            SmtpClient smtp = new SmtpClient();
            MailMessage email = new MailMessage();

            email.To.Add(to);
            email.IsBodyHtml = false;
            email.Subject = string.Format("[{0}] {1}", Website.Current.Resource.Name, subject);
            email.Body = string.Format("{0}\n{1}\n{2}\n",
                SecuritySettings.EmailBodyHeader,
                body,
                SecuritySettings.EmailBodyFooter);

            smtp.Send(email);
        }
        public static void NotifyUserOfApproval(MembershipUser user)
        {
            string body = SecuritySettings.UserRegistrationEmailBody;

            body += string.Format("\n\nUsername: {0}\nPassword: {1}\n",
                user.UserName,
                //generate a new password to send to the newly-approved user
                user.ResetPassword());

            Send(user.Email,
                SecuritySettings.UserRegistrationEmailSubject,
                body);
        }
    }
}
