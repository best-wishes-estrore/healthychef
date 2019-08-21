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
    public partial class Ingredient_Edit : FormControlBase
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
            ingredientid.Value = this.PrimaryKeyIndex.ToString();
            if (!IsPostBack)
            {
                SetButtons();

                if (PrimaryKeyIndex <= 0)
                {
                    epAllergens.Bind<hccAllergen>(hccAllergen.GetActive(), new List<hccAllergen>());
                }
            }
        }

        protected void btnRetire_Click(object sender, EventArgs e)
        {
            hccIngredient Ingredient = hccIngredient.GetById(this.PrimaryKeyIndex);

            if (Ingredient != null)
            {
                Ingredient.Retire(!UseReactivateBehavior);  
                SaveForm();                              
            }
        }

        protected override void LoadForm()
        {
            hccIngredient ingredient = hccIngredient.GetById(this.PrimaryKeyIndex);
            
            if (ingredient != null)
            {
                epAllergens.Bind<hccAllergen>(hccAllergen.GetActive(), hccIngredientAllergen.GetAllergensBy(ingredient.IngredientID));
                txtIngredientName.Text = ingredient.Name;
                txtIngredientDesc.Text = ingredient.Description;

                List<hccMenuItem> items = hccMenuItem.GetBy(ingredient.IngredientID);
                gvwIngUsage.DataSource = items;
                gvwIngUsage.DataBind();
            }

            SetButtons();
        }

        protected override void SaveForm()
        {
            try
            {
                hccIngredient Ingredient = hccIngredient.GetById(this.PrimaryKeyIndex);

                if (Ingredient == null)
                    Ingredient = new hccIngredient { IsRetired = false };

                Ingredient.Name = txtIngredientName.Text.Trim();
                Ingredient.Description = txtIngredientDesc.Text.Trim();
                Ingredient.Save();

                hccIngredientAllergen.RemoveAllergensBy(Ingredient.IngredientID);
                epAllergens.GetSelectedKeys()
                    .ForEach(delegate(int allgId)
                {
                    hccIngredientAllergen tre = new hccIngredientAllergen()
                    {
                        AllergenID = allgId,
                        IngredientID = Ingredient.IngredientID
                    };
                    tre.Save();
                });

                Ingredient.Save();

                this.OnSaved(new ControlSavedEventArgs(Ingredient.IngredientID));
            }
            catch
            {
                throw;
            }

        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtIngredientName.Text = string.Empty;
            txtIngredientDesc.Text = string.Empty;
            epAllergens.Reset();
            gvwIngUsage.DataSource = new List<hccMenuItem>();
            gvwIngUsage.DataBind();

            SetButtons();
        }

        protected override  void SetButtons()
        {
            ShowDeactivate = false;

            if (this.PrimaryKeyIndex > 0)
            {
                ShowDeactivate = true;

                hccIngredient CurrentIngredient = hccIngredient.GetById(this.PrimaryKeyIndex);

                if (CurrentIngredient != null)
                {
                    if (!CurrentIngredient.IsRetired)
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
            hccIngredient existIngred = hccIngredient.GetBy(txtIngredientName.Text.Trim());

            if (existIngred != null && existIngred.IngredientID != this.PrimaryKeyIndex)
                args.IsValid = false;
        }
    }
}