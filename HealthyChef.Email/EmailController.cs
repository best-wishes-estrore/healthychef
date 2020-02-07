using HealthyChef.Common;
using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace HealthyChef.Email
{
    public class EmailController
    {
        private System.Configuration.Configuration config;
        private MailSettingsSectionGroup mailSettings;

        public EmailController()
        {
            this.config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            this.mailSettings = (MailSettingsSectionGroup)this.config.GetSectionGroup("system.net/mailSettings");
        }

        protected bool SendMail(string fromAddr, string toAddr, string subject, string message, bool isHtml, List<Attachment> attachments)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toAddr))
                    toAddr = fromAddr;
                MailMessage message1 = new MailMessage();
                if (!string.IsNullOrEmpty(fromAddr))
                {
                    message1.From = new MailAddress(fromAddr);
                }
                else
                {
                    message1.From = new MailAddress("info@healthychefcreations.com");
                }
                if (!string.IsNullOrEmpty(toAddr))
                {
                    string str = toAddr.Trim();
                    char[] chArray = new char[2] { ';', ',' };
                    foreach (string address in str.Split(chArray))
                    {
                        if (!string.IsNullOrWhiteSpace(address))
                            message1.To.Add(new MailAddress(address));
                    }
                }
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (Attachment attachment in attachments)
                        message1.Attachments.Add(attachment);
                }
                message1.Subject = subject;
                message1.Body = message;
                message1.IsBodyHtml = isHtml;
                try
                {
                    new SmtpClient().Send(message1);
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_ToAdmin(string subject, string body)
        {
            try
            {
                return this.SendMail((string)null, WebConfigurationManager.AppSettings["AdministratorEmails"].ToString(), subject, body, false, (List<Attachment>)null);
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_ExceptionToAdmin(string subject, string body, Exception ex)
        {
            try
            {
                string body1 = body + "<br /><br />Message:<br />" + ex.Message + "<br /><br />Stack Trace:<br />" + ex.StackTrace;
                return this.SendMail_ToAdmin(subject, body1);
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_NewUserConfirmation(string toAddress, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toAddress))
                    throw new ArgumentNullException(nameof(toAddress), "MessagingController.SendNewUserEmail: toAddress is blank.");
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentNullException(nameof(password), "MessagingController.SendNewUserEmail: password is blank.");
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Your new Healthy Chef Creations account has been created. Your user name is your email address. Your password is: " + password + "<br /><br />");
                stringBuilder.AppendLine("You may log in at any time to place orders, buy gift certificates, manage your profile and food preferences, or change your password via the My Profile page.  Simply click the link below or go to www.HealthyChefCreations.com, click Member Login and enter your email address and the password you created during your account set up process.<br /><br />");
                stringBuilder.AppendLine("<a href='" + Helpers.GetSiteRoot() + "/login.aspx'>Click Here</a> to log in to your Healthy Chef Creations account.<br /><br />");
                stringBuilder.AppendLine("If you have any questions, believe you received this message in error, or did not create a Healthy Chef Creations account, please contact our Customer Service representatives at 866-575-2433.<br /><br />");
                stringBuilder.AppendLine("We look forward to feeding you!<br /><br />");
                stringBuilder.AppendLine("The Healthy Chef");
                return this.SendMail((string)null, toAddress, "Healthy Chef Creations - New User Created", stringBuilder.ToString(), true, (List<Attachment>)null);
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_PasswordChanged(string toAddress)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toAddress))
                    throw new ArgumentNullException(nameof(toAddress), "MessagingController.SendMail_PasswordChanged: toAddress is blank.");
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Your Healthy Chef Creations password has been changed.<br><br>");
                stringBuilder.AppendLine("You may log in at any time to place orders, buy gift certificates, manage your profile and food preferences, or change your password via the My Profile page. Simply click the link below or go to www.HealthyChefCreations.com, click Member Login and enter your email address and the password you created during your account set up process.<br><br>");
                stringBuilder.AppendLine("<a href='" + Helpers.GetSiteRoot() + "/login.aspx'>Click Here</a> to log in to your Healthy Chef Creations account.<br><br>");
                stringBuilder.AppendLine("If you have any questions, believe you received this message in error, or did not change your Healthy Chef Creations password, please contact our Customer Service representatives at 866-575-2433.<br><br>");
                stringBuilder.AppendLine("We look forward to feeding you!<br /><br />");
                stringBuilder.AppendLine("The Healthy Chef");
                return this.SendMail((string)null, toAddress, "Healthy Chef Creations - Password Changed", stringBuilder.ToString(), true, (List<Attachment>)null);
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_PasswordReset(string toAddress, string password)
        {
            try
            {
                var siteUrl = Helpers.GetSiteRoot();
                var subUrl = "/login.aspx";
                if(siteUrl.Contains("admin"))
                {
                    subUrl = "/AdminLogin.aspx";
                }
                if (string.IsNullOrWhiteSpace(toAddress))
                    throw new ArgumentNullException(nameof(toAddress), "MessagingController.SendMail_PasswordReset: toAddress is blank.");
                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentNullException(nameof(password), "MessagingController.SendMail_PasswordReset: password is blank.");
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("The password for your Healthy Chef Creations account has been reset.<br><br>");
                stringBuilder.AppendLine("Your new Temporary Password is: " + password + "<br/><br/>");
                stringBuilder.AppendLine("Please click the link below or go to www.HealthyChefCreations.com, click Member Login and enter your email address and your Temporary Password.  Then select the Password tab under My Profile to create a new password for your account.<br><br>");
                stringBuilder.AppendLine("<a href='" + Helpers.GetSiteRoot() + subUrl +"'>Click Here</a> to log in to your Healthy Chef Creations account.<br><br>");
                stringBuilder.AppendLine("If you have any questions, believe you received this message in error, or did not change your Healthy Chef Creations password, please contact our Customer Service representatives at 866-575-2433.<br><br>");
                stringBuilder.AppendLine("Thank you,<br /><br />");
                stringBuilder.AppendLine("The Healthy Chef");
                return this.SendMail((string)null, toAddress, "Healthy Chef Creations - Password Reset", stringBuilder.ToString(), true, (List<Attachment>)null);
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_OrderConfirmationCustomer(string toAddress, string customerName, string orderHtml)
        {
            string message = string.Empty;
            try
            {
                string fileText = Helpers.GetFileText("~/HtmlTemplates/OrderConfirmation_Customer.htm");
                if (fileText.Length > 0)
                    message = fileText.Replace("#NAME#", customerName).Replace("#ORDERINFO#", orderHtml).Replace("#STOREEMAIL#", "<a href='mailto:" + this.mailSettings.Smtp.From + "'>" + this.mailSettings.Smtp.From + "</a>").Replace("<img src=\"/App_Themes/HealthyChef/Images/pullmanholt_logo2.gif\" />", "<img src=\"" + Helpers.GetSiteRoot() + "/App_Themes/HealthyChef/Images/pullmanholt_logo2.gif\" />");
                return this.SendMail((string)null, toAddress, "Healthy Chef Creations - Order Confirmation", message, true, (List<Attachment>)null);
            }
            catch
            {
                throw;
            }
        }

        public void SendMail_OrderConfirmationMerchant(string customerName, string orderHtml, int orderId)
        {
            try
            {
                string empty = string.Empty;
                string toAddr = WebConfigurationManager.AppSettings["EmailNewOrderMerchantToAddress"].ToString();
                string fileText = Helpers.GetFileText("~/HtmlTemplates/OrderConfirmation_Merchant.htm");
                if (fileText.Length <= 0)
                    return;
                string str1 = fileText.Replace("#NAME#", customerName).Replace("#ORDER#", orderHtml);
                string str2 = Helpers.GetSiteRoot() + "/WebModules/ShoppingCart/Admin/Purchases.aspx?id=" + orderId.ToString();
                string message = str1.Replace("#LINK#", "<a href='" + str2 + "'>" + str2 + "</a>").Replace("<img src=\"/App_Themes/HealthyChef/Images/logo.png\" />", "<img src=\"" + Helpers.GetSiteRoot() + "/App_Themes/HealthyChef/Images/logo.png\" />");
                new EmailController().SendMail((string)null, toAddr, "Healthy Chef Creations - Order Notice", message, true, (List<Attachment>)null);
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_OrderRefundCustomer(string toAddress, string orderHtml)
        {
            try
            {
                bool flag = false;
                string fileText = Helpers.GetFileText("~/HtmlTemplates/OrderRefund_Customer.htm");
                if (fileText.Length > 0 && !string.IsNullOrEmpty(toAddress))
                {
                    string message = fileText.Replace("#NAME#", toAddress).Replace("#ORDERINFO#", orderHtml).Replace("#STOREEMAIL#", "<a href='mailto:" + this.mailSettings.Smtp.From + "'>" + this.mailSettings.Smtp.From + "</a>").Replace("#LINK#", "<a href='" + Helpers.GetSiteRoot() + "'>Our Store</a>");
                    flag = new EmailController().SendMail((string)null, toAddress, "Healthy Chef Creations - Order Refund Notification", message, true, (List<Attachment>)null);
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool SendMail_OrderShipCustomer(string toAddress, string customerName, string orderHtml)
        {
            try
            {
                bool flag = false;
                string fileText = Helpers.GetFileText("~/HtmlTemplates/OrderShip_Customer.htm");
                if (fileText.Length > 0 && !string.IsNullOrEmpty(toAddress))
                {
                    string message = fileText.Replace("#NAME#", customerName).Replace("#ORDERINFO#", orderHtml).Replace("#STOREEMAIL#", "<a href='mailto:" + this.mailSettings.Smtp.From + "'>" + this.mailSettings.Smtp.From + "</a>").Replace("#LINK#", "<a href='" + Helpers.GetSiteRoot() + "'>Our Store</a>");
                    flag = new EmailController().SendMail((string)null, toAddress, "Healthy Chef Creations - Shipment Notification", message, true, (List<Attachment>)null);
                }
                return flag;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public bool SendMail_ContactUs(string firstName, string lastName, string address, string city, string state, string zipcode, string phone, string email, string question)
        {
            string sMessage = string.Empty;
            string toAddress = "info@healthychefcreations.com"; 
            //string toAddress = "vineeth.m@bestwishesestore.com";
            try
            {
                string template = Helpers.GetFileText("~/HtmlTemplates/ContactUs.htm");
                if (template.Length > 0)
                {
                    sMessage = template;
                    if (firstName != null) { sMessage = sMessage.Replace("#firstName#", firstName); } else { sMessage = sMessage.Replace("#firstName#", ""); }
                    if (lastName != null) { sMessage = sMessage.Replace("#lastName#", lastName); } else { sMessage = sMessage.Replace("#lastName#", ""); }
                    if (address != null) { sMessage = sMessage.Replace("#address#", address); } else { sMessage = sMessage.Replace("#address#", ""); }
                    if (city != null) { sMessage = sMessage.Replace("#city#", city); } else { sMessage = sMessage.Replace("#city#", ""); }
                    if (state != null) { sMessage = sMessage.Replace("#state#", state); } else { sMessage = sMessage.Replace("#state#", ""); }
                    if (zipcode != null) { sMessage = sMessage.Replace("#postalCode#", zipcode); } else { sMessage = sMessage.Replace("#postalCode#", ""); }
                    if (phone != null) { sMessage = sMessage.Replace("#phoneNumber#", phone); } else { sMessage = sMessage.Replace("#phoneNumber#", ""); }
                    if (email != null) { sMessage = sMessage.Replace("#email#", email); } else { sMessage = sMessage.Replace("#email#", ""); }
                    if (question != null) { sMessage = sMessage.Replace("#QustionComments#", question); } else { sMessage = sMessage.Replace("#QustionComments#", ""); }
                    sMessage = sMessage.Replace("#submitedOn#", DateTime.Now.ToString());
                    return SendMail((string)null, toAddress, "Healthy Chef Creations - Contact Us", sMessage, true, null);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
