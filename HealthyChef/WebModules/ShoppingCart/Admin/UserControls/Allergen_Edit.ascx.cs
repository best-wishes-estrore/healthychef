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
    public partial class Allergen_Edit : FormControlBase
    {
        // this.PrimaryKeyIndex as hccAllergen.AllergenId

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
            btnDeactivate.Click += new EventHandler(btnRetire_Click);

            gvwIngUsage.PageIndexChanging += new GridViewPageEventHandler(gvwIngUsage_PageIndexChanging);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            allergenID.Value = this.PrimaryKeyIndex.ToString();
            if (!IsPostBack)
            {
                SetButtons();
            }
        }

        protected void btnRetire_Click(object sender, EventArgs e)
        {
            hccAllergen allergen = hccAllergen.GetById(this.PrimaryKeyIndex);

            if (allergen != null)
            {
                allergen.Retire(!UseReactivateBehavior);
                SaveForm();
            }
        }

        protected override void LoadForm()
        {
            allergenID.Value = this.PrimaryKeyIndex.ToString();
            hccAllergen allergen = hccAllergen.GetById(this.PrimaryKeyIndex);

            if (allergen != null)
            {
                txtAllergenName.Text = allergen.Name;
                txtAllergenDesc.Text = allergen.Description;

                BindgvwIngUsage();
            }

            SetButtons();
        }

        protected override void SaveForm()
        {
            try
            {
                hccAllergen allergen = hccAllergen.GetById(this.PrimaryKeyIndex);

                if (allergen == null)
                    allergen = new hccAllergen { IsActive = true };

                allergen.Name = txtAllergenName.Text.Trim();
                allergen.Description = txtAllergenDesc.Text.Trim();
                allergen.Save();

                this.OnSaved(new ControlSavedEventArgs(allergen.AllergenID));
            }
            catch
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtAllergenName.Text = string.Empty;
            txtAllergenDesc.Text = string.Empty;
            SetButtons();
        }

        protected override void SetButtons()
        {
            ShowDeactivate = false;

            if (this.PrimaryKeyIndex > 0)
            {
                ShowDeactivate = true;

                hccAllergen CurrentAllergen = hccAllergen.GetById(this.PrimaryKeyIndex);

                if (CurrentAllergen != null)
                {
                    if (CurrentAllergen.IsActive)
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
            hccAllergen existAllergen = hccAllergen.GetBy(txtAllergenName.Text.Trim());

            if (existAllergen != null && existAllergen.AllergenID != this.PrimaryKeyIndex)
                args.IsValid = false;
        }

        void BindgvwIngUsage()
        {
            List<hccIngredient> items = hccIngredient.GetBy(this.PrimaryKeyIndex);
            gvwIngUsage.DataSource = items;
            gvwIngUsage.DataBind();
        }

        void gvwIngUsage_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwIngUsage.PageIndex = e.NewPageIndex;
            BindgvwIngUsage();
        }
    }
}