using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class Preference_Edit : FormControlBase
    {   // Note: this.PrimaryKeyIndex as PreferenceId
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
            btnDeactivate.Click += new EventHandler(btnRetire_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            preferenceid.Value = this.PrimaryKeyIndex.ToString();
            if (!IsPostBack)
            {
                SetButtons();

                BindddlPrefTypes();
            }
        }

        void BindddlPrefTypes()
        {
            if (ddlPrefTypes.Items.Count == 0)
            {
                List<Tuple<string, int>> prefTypes = Enums.GetEnumAsTupleList(typeof(Enums.PreferenceType));

                ddlPrefTypes.DataSource = prefTypes;
                ddlPrefTypes.DataTextField = "Item1";
                ddlPrefTypes.DataValueField = "Item2";
                ddlPrefTypes.DataBind();

                ddlPrefTypes.Items.Insert(0, new ListItem("Select a type...", "-1"));
            }
        }

        protected void btnRetire_Click(object sender, EventArgs e)
        {
            hccPreference pref = hccPreference.GetById(this.PrimaryKeyIndex);

            if (pref != null)
            {
                pref.Retire(!UseReactivateBehavior);
                SaveForm();
            }
        }

        protected override void LoadForm()
        {
            try
            {
                hccPreference pref = hccPreference.GetById(this.PrimaryKeyIndex);

                if (pref != null)
                {
                    txtPrefName.Text = pref.Name;
                    txtPrefDesc.Text = pref.Description;

                    BindddlPrefTypes();

                    ddlPrefTypes.SelectedIndex
                        = ddlPrefTypes.Items.IndexOf(ddlPrefTypes.Items.FindByValue(pref.PreferenceType.ToString()));
                }

                SetButtons();
            }
            catch { throw; }
        }

        protected override void SaveForm()
        {
            try
            {
                hccPreference pref = hccPreference.GetById(this.PrimaryKeyIndex);

                if (pref == null)
                    pref = new hccPreference { IsRetired = false };

                pref.Name = txtPrefName.Text.Trim();
                pref.Description = txtPrefDesc.Text.Trim();
                pref.PreferenceType = (int)(Enums.PreferenceType)Enum.Parse(typeof(Enums.PreferenceType), ddlPrefTypes.SelectedValue);
                pref.Save();

                this.OnSaved(new ControlSavedEventArgs(pref.PreferenceID));
            }
            catch
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtPrefName.Text = string.Empty;
            txtPrefDesc.Text = string.Empty;
            ddlPrefTypes.ClearSelection();
            SetButtons();
        }

        protected override void SetButtons()
        {
            ShowDeactivate = false;

            if (this.PrimaryKeyIndex > 0)
            {
                ShowDeactivate = true;

                hccPreference CurrentPref = hccPreference.GetById(this.PrimaryKeyIndex);

                if (CurrentPref != null)
                {
                    if (!CurrentPref.IsRetired)
                    {
                        btnDeactivate.Text = "Retire";
                        UseReactivateBehavior = false;
                    }
                    else
                    {
                        btnDeactivate.Text = "Reactivate";
                        UseReactivateBehavior = true;
                    }
                }
            }

            btnDeactivate.Visible = ShowDeactivate;
        }

        protected void cstName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            hccPreference existPref = hccPreference.GetBy(int.Parse(ddlPrefTypes.SelectedValue), txtPrefName.Text.Trim());

            if (existPref != null && existPref.PreferenceID != this.PrimaryKeyIndex)
                args.IsValid = false;
        }
    }
}