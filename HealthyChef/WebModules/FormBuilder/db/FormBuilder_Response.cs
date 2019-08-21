using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Text;
using System.Data;

namespace BayshoreSolutions.WebModules.FormBuilder
{
    public partial class FormBuilder_Response
    {
        public static DataTable CreateDataTable(List<FormBuilder_FieldInput> fieldInputs, bool truncateLongStrings)
        {
            if (null == fieldInputs || fieldInputs.Count == 0) return null;
            var dt = new DataTable();

            int nModuleId = fieldInputs[0].FormBuilder_Field.ModuleId;

            FormBuilderDataContext dc = new FormBuilderDataContext();
            var fields = from f in dc.FormBuilder_Fields
                         where f.ModuleId == nModuleId
                         select f;

            //standard fields
            dt.Columns.Add(new DataColumn("CreatedOn", typeof(DateTime)));

            var pageCount = 0;
            //form-specific fields
            foreach (FormBuilder_Field field in fields)
            {
                // Must guarantee unique column names in DataTable
                dt.Columns.Add(field.Name + "_" + field.Id);
                //check if this is a multipage form
                if (field.Type == (int)FormBuilder_Field.FieldType.PageHeader) pageCount++;
            }

            var isMultiPage = pageCount > 0;

            if (isMultiPage) dt.Columns.Add("IsComplete");

            //standard fields
            dt.Columns.Add("CreatedBy");
            dt.Columns.Add("IPAddress");
            dt.Columns.Add("ResponseId");

            //keep track of responses that have been added already
            var responseIds = new List<int>();

            foreach (FormBuilder_FieldInput fieldInput in fieldInputs)
            {
                if (!responseIds.Contains(fieldInput.ResponseId))
                {
                    responseIds.Add(fieldInput.ResponseId);
                    //get all the inputs for this response.
                    var assocInputs = new List<FormBuilder_FieldInput>();
                    foreach (var fi in fieldInputs)
                    {
                        if (fi.ResponseId == fieldInput.ResponseId)
                            assocInputs.Add(fi);
                    }

                    //add the respective row.
                    DataRow dr = dt.NewRow();
                    FormBuilder_Response response = fieldInput.FormBuilder_Response;
                    if (isMultiPage) dr["IsComplete"] = response.IsComplete ? "Yes" : "No";
                    dr["CreatedBy"] = response.CreatedBy;
                    dr["CreatedOn"] = response.CreatedOn;
                    dr["IPAddress"] = response.IPAddress;
                    foreach (FormBuilder_FieldInput i in assocInputs)
                    {
                        dr[i.FormBuilder_Field.Name + "_" + i.FormBuilder_Field.Id] =
                            (truncateLongStrings ? Truncate(i.InputValue) : i.InputValue);
                    }
                    dr["ResponseId"] = fieldInput.ResponseId;
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
        private static string Truncate(string inputValue)
        {
            if (inputValue != null)
            {
                string retString = inputValue;
                const int MAX_CHARS = 200;
                if (inputValue.Length > MAX_CHARS)
                {
                    retString = inputValue.Substring(0, MAX_CHARS);
                    retString += "..";
                }
                return retString;
            }
            return String.Empty;
        }

        /* added - SM */

        public static Dictionary<string, string> GetResponseByResponseId(int responseId)
        {
            FormBuilderDataContext dc = new FormBuilderDataContext();
            var values = from f in dc.FormBuilder_Fields
                         from i in dc.FormBuilder_FieldInputs
                         where f.Id == i.FieldId
                         where i.ResponseId == responseId
                         select new { f.Id, f.Name, f.Type, i.InputValue };

            var response = (from r in dc.FormBuilder_Responses
                            where r.Id == responseId
                            select r).Single();


            var dict = new Dictionary<string, string>();
            foreach (var value in values)
            {
                string strKey = string.Format("{0}_{1}", value.Name, value.Id);
                string strValue = value.InputValue;

                if (value.Type == (int)FormBuilder_Field.FieldType.FileUpload)
                {
                    if (!string.IsNullOrEmpty(strValue))
                    {
                        string strUrl = new WebModuleBase().ResolveUrl(value.InputValue.Replace("~", "").Replace("//", "/").Replace(@"\", ""));
                        strValue = String.Format("<a href='{0}' target='_blank'>{0}</a>", strUrl);
                    }
                }
                dict.Add(strKey, strValue);

            }
            //add the ip addr etc.
            dict.Add("Is Complete", response.IsComplete ? "Yes" : "No");
            dict.Add("IP Address", response.IPAddress);
            dict.Add("Submitted On", response.CreatedOn.ToString("MM/dd/yyyy"));
            dict.Add("Submitted By", response.CreatedBy);

            // add referrer and keywords
            var referrer = (from r in dc.FormBuilder_ResponseReferrers
                            where r.ResponseId == responseId
                            select r).FirstOrDefault();
            if (referrer != null)
            {
                dict.Add("Referring Url", referrer.referringUrl);
                dict.Add("Landing Url", referrer.landingUrl);
                dict.Add("Referring Domain", referrer.domain);
                dict.Add("Keywords", referrer.query);
            }


            return dict;

        }


        internal class InputValues
        {
            internal string FieldName { get; set; }
            internal string InputValue { get; set; }
        }

        public void EmailNotifyAdmin()
        {
            try
            {
                var body = new StringBuilder();

                string strNotifyEmails = this.FormBuilder_Module.NotifyEmail;
                string[] astrNotifyEmails = strNotifyEmails.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (astrNotifyEmails.Length > 0)
                {
                    WebModuleInfo module = WebModule.GetModule(this.FormBuilder_Module.ModuleId);

                    var dbContext = new FormBuilderDataContext();
                    List<FormBuilder_FieldInput> lInputsSorted = (from i in dbContext.FormBuilder_FieldInputs
                                                                  where i.ResponseId == this.Id
                                                                  orderby i.FormBuilder_Field.SortOrder
                                                                  select i).ToList();

                    foreach (var input in lInputsSorted)
                    {
                        if (input.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.FileUpload
                            && !String.IsNullOrEmpty(input.InputValue))
                            body.Append(input.FormBuilder_Field.Name
                                + String.Format(": <b>{0}",
                                input.InputValue.Replace("~", "").Replace("//", "").Replace(@"\", "")) + "</b><br>");
                        else if (input.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.SectionHeader)
                            body.Append(String.Format("<br/><b>{0}</b><br/>", input.FormBuilder_Field.Name));
                        else
                            body.Append(input.FormBuilder_Field.Name + ": <b>" + input.InputValue + "</b><br>");
                    }

                    MailMessage email = new MailMessage();
                    foreach (string strNotifyEmail in astrNotifyEmails)
                    {
                        MailAddress address = new MailAddress(strNotifyEmail.Trim());
                        email.To.Add(address);
                    }

                    email.Body = "<font face='Arial'>New form response submission has been received.<br>" +
                                 "<hr>" + body + "<br>" +
                                 "To view all form responses, " +
                                 "<a href='" + Common.Web.Url.ToAbsoluteUrl(module.GetEditUrl()) + "'>click here</a>." +
                                 "</font>";

                    email.Subject = string.Format("{0}: {1}",
                        Website.Current.Resource.Name,
                        module.Webpage.Title);

                    email.IsBodyHtml = true;

                    new SmtpClient().Send(email);
                }
            }
            catch (Exception ex)
            {
                string strMessage = "Failed sending contact us notification message";
                BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(strMessage, this, ex);
            }
        }

        public void EmailAcknowledgement()
        {
            try
            {
                var dc = new FormBuilderDataContext();
                FormBuilder_Module form = FormBuilder_Module;

                if (form.Ack_Enabled)
                {
                    var message = new MailMessage();

                    string strNotifyEmails = this.FormBuilder_Module.NotifyEmail;
                    string[] astrNotifyEmails = strNotifyEmails.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (astrNotifyEmails.Length > 0)
                    {
                        foreach (string strNotifyEmail in astrNotifyEmails)
                        {
                            MailAddress address = new MailAddress(strNotifyEmail.Trim());
                            message.Bcc.Add(address);
                        }
                    }

                    if (!string.IsNullOrEmpty(form.Ack_FromEmailAddress))
                    {
                        message.From = new MailAddress(form.Ack_FromEmailAddress);
                    }

                    List<FormBuilder_FieldInput> inputs = (from fi in dc.FormBuilder_FieldInputs
                                                           where fi.ResponseId == Id
                                                           select fi).ToList();

                    //sort by SortOrder.
                    inputs.Sort(delegate(FormBuilder_FieldInput a, FormBuilder_FieldInput b)
                    {
                        return a.FormBuilder_Field.SortOrder.CompareTo(b.FormBuilder_Field.SortOrder);
                    });

                    var sbSubject = new StringBuilder(form.Ack_Subject);
                    var sbBody = new StringBuilder(form.Ack_Body);

                    foreach (FormBuilder_FieldInput input in inputs)
                    {
                        if (string.Compare(input.FormBuilder_Field.Name, form.Ack_EmailAddressFieldLabel, true) == 0)
                        {
                            message.To.Add(new MailAddress(input.InputValue));
                        }

                        string strToken = string.Format("##{0}##", input.FormBuilder_Field.Name);
                        sbSubject = sbSubject.Replace(strToken, input.InputValue);
                        sbSubject = sbSubject.Replace(strToken.ToLower(), input.InputValue);
                        sbSubject = sbSubject.Replace(strToken.ToUpper(), input.InputValue);
                        sbBody = sbBody.Replace(strToken, input.InputValue);
                        sbBody = sbBody.Replace(strToken.ToLower(), input.InputValue);
                        sbBody = sbBody.Replace(strToken.ToUpper(), input.InputValue);
                    }

                    if (message.To.Count > 0)
                    {
                        message.Subject = sbSubject.ToString();
                        message.Body = sbBody.ToString();
                        message.IsBodyHtml = true;

                        var smtpClient = new SmtpClient();
                        smtpClient.Send(message);
                    }
                }
            }
            catch (Exception ex)
            {
                WebModulesAuditEvent.Raise("Failed sending acknowledgement email", this, ex);
            }
        }
    }
}
