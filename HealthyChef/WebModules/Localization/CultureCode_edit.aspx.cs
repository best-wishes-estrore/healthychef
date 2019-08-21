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

using Bss = BayshoreSolutions.Common;
using WM = BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Cms.Localization
{
    public partial class CultureCode_edit : System.Web.UI.Page
    {
        protected static readonly string _entityName_cultureCode = "Culture Code";

        public bool New_CultureCode
        {
            get { return (bool)(ViewState["New_CultureCode"] ?? false); }
            set { ViewState["New_CultureCode"] = value; }
        }

        public string CultureCode
        {
            get { return (string)(ViewState["CultureCode"] ?? null); }
            set { ViewState["CultureCode"] = value; }
        }

        private void Init_()
        {
            //
            //load AliasToCultureCodeCtl list
            //
            WM.CultureCodeCollection activeCultureCodes = WM.CultureCode.Find(null, null, true, null);
            AliasToCultureCodeCtl.DataSource = activeCultureCodes.FindAll(
                //only allow aliases to non-aliased culture codes (keep it simple for now...).
                delegate(WM.CultureCode c) { return !c.IsAlias; });
            AliasToCultureCodeCtl.DataTextField = "Name";
            AliasToCultureCodeCtl.DataValueField = "Name";
            AliasToCultureCodeCtl.DataBind();
            AliasToCultureCodeCtl.Items.Insert(0, string.Empty);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init_();

                this.CultureCode = Request.QueryString["CultureCode"];
                this.New_CultureCode = Bss.Web.Request.GetQueryStringValue<bool>("New_CultureCode", false);

                if (!this.New_CultureCode && null == this.CultureCode)
                    List_CultureCode(null, null);
                else
                    Edit_CultureCode();
            }
        }

        private void List_CultureCode(string sortExpr, bool? sortDir)
        {
            MultiViewCtl.SetActiveView(View_List_CultureCode);

            CultureCode_List.DataSource = WM.CultureCode.GetReader(
                null,
                null,
                sortExpr,
                sortDir);
            CultureCode_List.DataBind();
        }

        private void Edit_CultureCode()
        {
            MultiViewCtl.SetActiveView(View_Edit_CultureCode);

            WM.CultureCode cultureCode = GetCurrent_CultureCode();

            if (cultureCode.IsNew)
            { //create new
                Page.Title = "Create New " + _entityName_cultureCode;
                NewCultureCodeNamePanel.Visible = true;
                IsDefaultMsg.Visible = false;
                IsNotDefaultMsg.Visible = false;
                CultureCodeNameHeaderCtl.InnerHtml = "New " + _entityName_cultureCode;
            }
            else
            { //update existing
                Page.Title = "Edit " + _entityName_cultureCode;
                CultureCodeNameHeaderCtl.InnerHtml = cultureCode.Name;
                Load_CultureCode(cultureCode);
            }

            Page.Form.DefaultButton = CultureCode_SaveButton.UniqueID;
        }

        /// <summary>Gets the current entity from the database, or returns a new entity.</summary>
        public WM.CultureCode GetCurrent_CultureCode()
        {
            WM.CultureCode cultureCode = null;

            if (this.New_CultureCode)
                cultureCode = new CultureCode();
            else
                cultureCode = WM.CultureCode.Get(this.CultureCode);

            //if (string.IsNullOrEmpty(this.CultureCode)) throw new ArgumentNullException("CultureCode");

            if (null == cultureCode) throw new InvalidOperationException(string.Format("The specified {0} was not found (it may have been deleted).", _entityName_cultureCode.ToLower()));

            //check permissions...
            //if(cultureCode.UserId != (Guid)_user.ProviderUserKey) throw new System.Security.SecurityException("The current user is not authorized to view that item.");

            return cultureCode;
        }

        /// <summary>Gets the current entity and fills its with the form input values.</summary>
        public WM.CultureCode GetInput_CultureCode()
        {
            WM.CultureCode cultureCode = GetCurrent_CultureCode();
            string cultureCodeInput = CultureCodeCtl.Text.Trim().ToLower();

            //if we are in "create new" mode, don't update an existing culture.
            if (this.New_CultureCode
                && null != WM.CultureCode.Get(cultureCodeInput))
            {
                Msg.ShowError(string.Format("The {0} '{1}' already exists.", _entityName_cultureCode.ToLower(), cultureCodeInput));
                return null;
            }

            try //attempt to create a CultureInfo object using the culture code.
            {
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(cultureCodeInput);
            }
            catch
            {
                Msg.ShowError(string.Format("'{0}' is not a valid {1}.", cultureCodeInput, _entityName_cultureCode.ToLower()));
                return null;
            }

            if (cultureCode.IsNew)
            {
                cultureCode.Name = cultureCodeInput;
            }

            //'IsDefault' is updated via other means.
            //cultureCode.IsDefault = IsDefaultCtl.Checked;
            cultureCode.IsActive = IsActiveCtl.Checked;

            if (AliasToCultureCodeCtl.SelectedIndex > 0)
            {
                //validate: double-check to make sure that we are not aliasing to an alias
                //(we _could_ allow aliases-to-aliases, but let's keep it simple for now).
                WM.CultureCode aliasToCultureCode = WM.CultureCode.Get(AliasToCultureCodeCtl.Text);
                if (aliasToCultureCode.IsAlias)
                {
                    Msg.ShowError("Cannot alias to another alias.");
                    return null;
                }
            }

            //validation passed; set the alias key.
            cultureCode.AliasToCultureCode = AliasToCultureCodeCtl.Text;

            return cultureCode;
        }

        public void Load_CultureCode(WM.CultureCode cultureCode)
        {
            //string.Format("Set the system default culture code to <strong>{0}</strong>.",
            SetSystemDefaultButton.ToolTip =
                string.Format("Set the system default culture to '{0}'.",
                    this.CultureCode);

            bool isDefault = cultureCode.IsDefault;
            IsDefaultMsg.Visible = isDefault;
            IsNotDefaultMsg.Visible = !isDefault;
            IsActiveCtl.Enabled = !isDefault;
            if (isDefault)
                IsActiveCtl.ToolTip = "The system default culture must be active.";

            NewCultureCodeNamePanel.Visible = false;
            //CultureCodeCtl.Text = cultureCode.Name;
            IsActiveCtl.Checked = cultureCode.IsActive;
            AliasToCultureCodeCtl.Text = cultureCode.AliasToCultureCode;
        }

        protected void CultureCode_SaveButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            WM.CultureCode cultureCode = GetInput_CultureCode();
            if (null == cultureCode) return; //validation failed.
            cultureCode.Save();

            List_CultureCode(null, null);
            Msg.ShowSuccess(string.Format("Saved {0} '{1}'.",
                _entityName_cultureCode.ToLower(),
                cultureCode.Name));
        }

        protected void CultureCode_CancelButton_Click(object sender, EventArgs e)
        {
            List_CultureCode(null, null);
            Msg.Show("Changes to the item were discarded.");
        }

        protected void CultureCode_List_OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string cultureCode = (string)CultureCode_List.DataKeys[e.RowIndex].Value;
            WM.CultureCode.Destroy(cultureCode);
            List_CultureCode(null, null);
            Msg.Show("The cultureCode was deleted.");
        }

        protected void CultureCode_List_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName == "Select")
            //{
            //    this.CultureCode = string.Parse(e.CommandArgument.ToString());
            //}
        }

        protected void CultureCode_List_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //}
        }

        protected void SetSystemDefaultButton_Click(object sender, EventArgs e)
        {
            WM.CultureCode oldDefaultCulture = WM.CultureCode.Find(null, true, null, null).First;
            WM.CultureCode.SetDefaultCulture(this.CultureCode);
            Msg.ShowSuccess(string.Format(
                "Changed the system default culture from <strong>{0}</strong> to <strong>{1}</strong>.",
                oldDefaultCulture.Name,
                this.CultureCode));
        }
    }
}
