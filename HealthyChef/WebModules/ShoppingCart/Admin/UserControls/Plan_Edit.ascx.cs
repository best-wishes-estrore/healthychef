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
    public partial class Plan_Edit : FormControlBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
            btnDeactivate.Click += new EventHandler(btnRetire_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            planid.Value = this.PrimaryKeyIndex.ToString();
            if (!IsPostBack)
            {
                BindddlPrograms();
                BindddlNumDays();
                BindddlNumWeeks();

                SetButtons();
            }
        }

        protected override void LoadForm()
        {
            try
            {
                hccProgramPlan plan = hccProgramPlan.GetById(this.PrimaryKeyIndex);

                if (plan != null)
                {
                    txtPlanName.Text = plan.Name;
                    txtPlanDesc.Text = plan.Description;

                    chkIsTaxEligible.Checked = plan.IsTaxEligible;
                    txtPricePerDay.Text = plan.PricePerDay.ToString("f2");

                    BindddlPrograms();
                    BindddlNumDays();
                    BindddlNumWeeks();

                    ddlPrograms.SelectedIndex = ddlPrograms.Items.IndexOf(ddlPrograms.Items.FindByValue(plan.ProgramID.ToString()));
                    chkIsDefault.Checked = plan.IsDefault;

                    ddlNumDays.SelectedIndex = ddlNumDays.Items.IndexOf(ddlNumDays.Items.FindByValue(plan.NumDaysPerWeek.ToString()));
                    ddlNumWeeks.SelectedIndex = ddlNumWeeks.Items.IndexOf(ddlNumWeeks.Items.FindByValue(plan.NumWeeks.ToString()));
                }

                SetButtons();
            }
            catch { throw; }
        }

        protected override void SaveForm()
        {
            try
            {
                hccProgramPlan plan = hccProgramPlan.GetById(this.PrimaryKeyIndex);

                if (plan == null)
                    plan = new hccProgramPlan { IsActive = true };

                plan.Name = txtPlanName.Text.Trim();
                plan.ProgramID = int.Parse(ddlPrograms.SelectedValue);
                plan.IsDefault = chkIsDefault.Checked;
                plan.Description = txtPlanDesc.Text.Trim();
                plan.PricePerDay = decimal.Parse(txtPricePerDay.Text.Trim());
                plan.NumDaysPerWeek = int.Parse(ddlNumDays.SelectedValue);
                plan.NumWeeks = int.Parse(ddlNumWeeks.SelectedValue);
                plan.IsTaxEligible = chkIsTaxEligible.Checked;
                plan.Save();

                this.OnSaved(new ControlSavedEventArgs(plan.PlanID));
            }
            catch
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtPlanName.Text = string.Empty;
            txtPlanDesc.Text = string.Empty;
            ddlPrograms.ClearSelection();
            chkIsDefault.Checked = false;
            chkIsTaxEligible.Checked = false;
            txtPricePerDay.Text = string.Empty;
            ddlNumWeeks.ClearSelection();
            ddlNumDays.ClearSelection();
            SetButtons();
        }

        void BindddlPrograms()
        {
            if (ddlPrograms.Items.Count == 0)
            {
                ddlPrograms.DataSource = hccProgram.GetBy(true);
                ddlPrograms.DataTextField = "Name";
                ddlPrograms.DataValueField = "ProgramID";
                ddlPrograms.DataBind();

                ddlPrograms.Items.Insert(0, new ListItem("Select a Program...", "-1"));
            }
        }

        void BindddlNumDays()
        {
            if (ddlNumDays.Items.Count == 0)
            {
                ddlNumDays.DataSource = new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }.ToList();
                ddlNumDays.DataBind();
            }
        }

        void BindddlNumWeeks()
        {
            if (ddlNumWeeks.Items.Count == 0)
            {
                ddlNumWeeks.DataSource = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 
                11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 
                21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 
                31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 
                41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52 }.ToList();
                ddlNumWeeks.DataBind();
            }
        }

        protected void btnRetire_Click(object sender, EventArgs e)
        {
            hccProgramPlan pref = hccProgramPlan.GetById(this.PrimaryKeyIndex);

            if (pref != null)
            {
                pref.Retire(!UseReactivateBehavior);
                SaveForm();
            }
        }

        protected override void SetButtons()
        {
            ShowDeactivate = false;

            if (this.PrimaryKeyIndex > 0)
            {
                ShowDeactivate = true;

                hccProgramPlan plan = hccProgramPlan.GetById(this.PrimaryKeyIndex);

                if (plan != null)
                {
                    if (plan.IsActive)
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
            hccProgramPlan plan = hccProgramPlan.GetBy(txtPlanName.Text.Trim());

            if (plan != null && plan.PlanID != this.PrimaryKeyIndex)
                args.IsValid = false;
        }
    }
}