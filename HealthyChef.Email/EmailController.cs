using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Configuration;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.IO;
using System.ComponentModel;
using HealthyChef.Common;
using System.Net;

namespace HealthyChef.Email
{
    /// <summary>
    /// Summary description for MessagingController
    /// </summary>
    public class EmailController
    {
        Configuration config;
        MailSettingsSectionGroup mailSettings;

        public EmailController()
        {
            config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            mailSettings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");
        }

        /// <summary>
        /// Send null value for fromAddress to use web.config from system.net/mailSettings/smtp:from value.
        /// Send null value for attachments, if no attachments.
        /// </summary>
        /// <param name="fromAddr"></param>
        /// <param name="toAddr"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="isHtml"></param>
        /// <returns></returns>
        protected bool SendMail(string fromAddr, string toAddr, string subject, string message,
            bool isHtml, List<Attachment> attachments)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toAddr)) // send it back to yourself? sure why not
                    toAddr = fromAddr;

                MailMessage msg = new MailMessage();

                // if fromAddr is null the default email address in the <system.net><mailSettings> 
                // section in the web.config will be used.
                if (!string.IsNullOrEmpty(fromAddr))
                {
                    msg.From = new MailAddress(fromAddr);
                }

                // toAddr can be a string of semi-colon or comma delimited email addresses.
                if (!String.IsNullOrEmpty(toAddr))
                {
                    string[] toList = toAddr.Trim().Split(';', ',');
                    foreach (string to in toList)
                    {
                        if (!string.IsNullOrWhiteSpace(to))
                            msg.To.Add(new MailAddress(to));
                    }
                }

                // iterate attachments
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (Attachment attch in attachments)
                    {
                        msg.Attachments.Add(attch);
                    }
                }

                //if (HttpContext.Current.Request.IsLocal)
                //{
                //    msg.To.Clear();
                //    msg.To.Add("rcreecy@bayshoresolutions.com");
                //}
                                
                msg.Subject = subject;
                msg.Body = message;
                msg.IsBodyHtml = isHtml;

                try
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.UseDefaultCredentials = false;
                    smtp.Host = "smtpout.secureserver.net";
                    smtp.Port = 465;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("noreply@healthychefcreations.com", "hccn0r3p7y");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    //smtp.UseDefaultCredentials = false;
                    //smtp.Host = "smtp.gmail.com";
                    //smtp.Port = 587;
                    //smtp.EnableSsl = true;
                    //smtp.Timeout = 20000;
                    //smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtp.Credentials = new NetworkCredential("noreply@healthychefcreations.com", "hccn0r3p7y");

                    smtp.Send(msg);
                    return true;
                }
                catch(Exception ex)
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
                string toAddr = WebConfigurationManager.AppSettings["AdministratorEmails"].ToString();

                return SendMail(null, toAddr, subject, body, false, null); 
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
                string emailBody = body + "<br /><br />Message:<br />" + ex.Message + "<br /><br />Stack Trace:<br />" + ex.StackTrace;

                return SendMail_ToAdmin(subject, emailBody); ;
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
                if (string.IsNullOrWhiteSpace(toAddress)) { throw new ArgumentNullException("toAddress", "MessagingController.SendNewUserEmail: toAddress is blank."); }
                if (string.IsNullOrWhiteSpace(password)) { throw new ArgumentNullException("password", "MessagingController.SendNewUserEmail: password is blank."); }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your new Healthy Chef Creations account has been created. Your user name is your email address. Your password is: " + password + "<br /><br />");
                sb.AppendLine("You may log in at any time to place orders, buy gift certificates, manage your profile and food preferences, or change your password via the My Profile page.  Simply click the link below or go to www.HealthyChefCreations.com, click Member Login and enter your email address and the password you created during your account set up process.<br /><br />");
                sb.AppendLine("<a href='" + Helpers.GetSiteRoot() + "/login.aspx'>Click Here</a> to log in to your Healthy Chef Creations account.<br /><br />");
                sb.AppendLine("If you have any questions, believe you received this message in error, or did not create a Healthy Chef Creations account, please contact our Customer Service representatives at 866-575-2433.<br /><br />");
                sb.AppendLine("We look forward to feeding you!<br /><br />");
                sb.AppendLine("The Healthy Chef");

                return SendMail(null, toAddress, "Healthy Chef Creations - New User Created", sb.ToString(), true, null);
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
                if (string.IsNullOrWhiteSpace(toAddress)) { throw new ArgumentNullException("toAddress", "MessagingController.SendMail_PasswordChanged: toAddress is blank."); }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your Healthy Chef Creations password has been changed.<br><br>");
                sb.AppendLine("You may log in at any time to place orders, buy gift certificates, manage your profile and food preferences, or change your password via the My Profile page. Simply click the link below or go to www.HealthyChefCreations.com, click Member Login and enter your email address and the password you created during your account set up process.<br><br>");
                sb.AppendLine("<a href='" + Helpers.GetSiteRoot() + "/login.aspx'>Click Here</a> to log in to your Healthy Chef Creations account.<br><br>");
                sb.AppendLine("If you have any questions, believe you received this message in error, or did not change your Healthy Chef Creations password, please contact our Customer Service representatives at 866-575-2433.<br><br>");
                sb.AppendLine("We look forward to feeding you!<br /><br />");
                sb.AppendLine("The Healthy Chef");

                return SendMail(null, toAddress, "Healthy Chef Creations - Password Changed", sb.ToString(), true, null);
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
                if (string.IsNullOrWhiteSpace(toAddress)) { throw new ArgumentNullException("toAddress", "MessagingController.SendMail_PasswordReset: toAddress is blank."); }
                if (string.IsNullOrWhiteSpace(password)) { throw new ArgumentNullException("password", "MessagingController.SendMail_PasswordReset: password is blank."); }

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("The password for your Healthy Chef Creations account has been reset.<br><br>");
                sb.AppendLine("Your new Temporary Password is: " + password + "<br/><br/>");
                sb.AppendLine("Please click the link below or go to www.HealthyChefCreations.com, click Member Login and enter your email address and your Temporary Password.  Then select the Password tab under My Profile to create a new password for your account.<br><br>");
                sb.AppendLine("<a href='" + Helpers.GetSiteRoot() + "/login.aspx'>Click Here</a> to log in to your Healthy Chef Creations account.<br><br>");
                sb.AppendLine("If you have any questions, believe you received this message in error, or did not change your Healthy Chef Creations password, please contact our Customer Service representatives at 866-575-2433.<br><br>");
                sb.AppendLine("Thank you,<br /><br />");
                sb.AppendLine("The Healthy Chef");

                return SendMail(null, toAddress, "Healthy Chef Creations - Password Reset", sb.ToString(), true, null);
            }
            catch
            {
                throw;
            }
        }

        public bool SendMail_OrderConfirmationCustomer(string toAddress, string customerName, string orderHtml)
        {
            string sMessage = string.Empty;
            //to user saying thank you                            
            try
            {
                //load up the template
                string template = Helpers.GetFileText("~/HtmlTemplates/OrderConfirmation_Customer.htm");

                if (template.Length > 0)
                {
                    //run some tag replacements. First with the name
                    sMessage = template;
                    sMessage = sMessage.Replace("#NAME#", customerName);

                    //ordernumber
                    sMessage = sMessage.Replace("#ORDERINFO#", orderHtml);

                    //admin email
                    sMessage = sMessage.Replace("#STOREEMAIL#", "<a href='mailto:" + mailSettings.Smtp.From + "'>" + mailSettings.Smtp.From + "</a>");

                    //link
                    //sMessage = sMessage.Replace("#LINK#", "<a href='" + Helpers.GetSiteRoot() + "'>Our Store</a>");

                    string imageTag = "<img src=\"" + Helpers.GetSiteRoot() + "/App_Themes/HealthyChef/Images/pullmanholt_logo2.gif\" />";
                    sMessage = sMessage.Replace("<img src=\"/App_Themes/HealthyChef/Images/pullmanholt_logo2.gif\" />", imageTag);
                }

                return SendMail(null, toAddress, "Healthy Chef Creations - Order Confirmation", sMessage, true, null);


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
                string sMessage = string.Empty;
                string toAddr = WebConfigurationManager.AppSettings["EmailNewOrderMerchantToAddress"].ToString();

                //load up the template
                string template = Helpers.GetFileText("~/HtmlTemplates/OrderConfirmation_Merchant.htm");

                if (template.Length > 0)
                {
                    //run some tag replacements. First with the name
                    sMessage = template;

                    sMessage = sMessage.Replace("#NAME#", customerName);

                    //ordernumber
                    sMessage = sMessage.Replace("#ORDER#", orderHtml);

                    //items
                    string linkStr = Helpers.GetSiteRoot() + "/WebModules/ShoppingCart/Admin/Purchases.aspx?id=" + orderId.ToString();
                    sMessage = sMessage.Replace("#LINK#", "<a href='" + linkStr + "'>" + linkStr + "</a>");

                    string imageTag = "<img src=\"" + Helpers.GetSiteRoot() + "/App_Themes/HealthyChef/Images/logo.png\" />";
                    sMessage = sMessage.Replace("<img src=\"/App_Themes/HealthyChef/Images/logo.png\" />", imageTag);

                    //send it off!
                    EmailController mc = new EmailController();
                    mc.SendMail(null, toAddr, "Healthy Chef Creations - Order Notice", sMessage, true, null);
                }
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


                bool success = false;

                //load up the template
                //there is a text template too...
                string template = Helpers.GetFileText("~/HtmlTemplates/OrderRefund_Customer.htm");

                if ((template.Length > 0) && (!String.IsNullOrEmpty(toAddress)))
                {
                    //run some tag replacements. First with the name
                    string sMessage = template;
                    sMessage = sMessage.Replace("#NAME#", toAddress);
                    //ordernumber
                    sMessage = sMessage.Replace("#ORDERINFO#", orderHtml);
                    //admin email
                    sMessage = sMessage.Replace("#STOREEMAIL#", "<a href='mailto:" + mailSettings.Smtp.From + "'>" + mailSettings.Smtp.From + "</a>");
                    //link
                    sMessage = sMessage.Replace("#LINK#", "<a href='" + Helpers.GetSiteRoot() + "'>Our Store</a>");
                    //setup the mailer

                    //send it off!
                    EmailController mc = new EmailController();
                    success = mc.SendMail(null, toAddress, "Healthy Chef Creations - Order Refund Notification",
                        sMessage, true, null);

                }
                return success;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SendMail_OrderShipCustomer(string toAddress, string customerName, string orderHtml)
        {
            try
            {


                bool success = false;

                //load up the template
                //there is a text template too...
                string template = Helpers.GetFileText("~/HtmlTemplates/OrderShip_Customer.htm");

                if ((template.Length > 0) && (!String.IsNullOrEmpty(toAddress)))
                {
                    //run some tag replacements. First with the name
                    string sMessage = template;
                    sMessage = sMessage.Replace("#NAME#", customerName);
                    //ordernumber
                    sMessage = sMessage.Replace("#ORDERINFO#", orderHtml);
                    //admin email
                    sMessage = sMessage.Replace("#STOREEMAIL#", "<a href='mailto:" + mailSettings.Smtp.From + "'>" + mailSettings.Smtp.From + "</a>");
                    //link
                    sMessage = sMessage.Replace("#LINK#", "<a href='" + Helpers.GetSiteRoot() + "'>Our Store</a>");
                    //tracking info

                    //if (order.OrderItemCount > 0)
                    //{
                    //    string trackingMsg = "";

                    //    List<OrderTrackingNumber> trackNums = order.TrackingNumbers;

                    //    if (trackNums.Count > 0)
                    //    {
                    //        if (trackNums.Count == 1)
                    //            trackingMsg = "<br />Your shipment tracking number is below:";
                    //        else
                    //            trackingMsg = "<br />Your shipment tracking numbers are below:";

                    //        foreach (OrderTrackingNumber trackNum in order.TrackingNumbers)
                    //        {
                    //            string linkURL = "http://wwwapps.ups.com/etracking/tracking.cgi?tracknum=" + trackNum.TrackingNumber;
                    //            trackingMsg += ".&nbsp;&nbsp;<a href='" + linkURL + "'>View your UPS tracking info by clicking here.</a>";

                    //            trackingMsg += "<br />";
                    //        }
                    //    }

                    //    sMessage = sMessage.Replace("#TRACKINGNUMBER#", trackingMsg);
                    //}

                    //send it off!
                    EmailController mc = new EmailController();
                    success = mc.SendMail(null, toAddress, "Healthy Chef Creations - Shipment Notification", sMessage, true, null);
                }
                return success;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
