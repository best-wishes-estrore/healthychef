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
    public partial class Program_Edit : FormControlBase
    {
        protected hccProgram CurrentProgram { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            txtCalRange1Text.TextChanged += new EventHandler(txtCalRange1Text_TextChanged);
            txtCalRange2Text.TextChanged += new EventHandler(txtCalRange2Text_TextChanged);
            txtCalRange3Text.TextChanged += new EventHandler(txtCalRange3Text_TextChanged);
            txtCalRange4Text.TextChanged += new EventHandler(txtCalRange4Text_TextChanged);
            txtCalRange5Text.TextChanged += new EventHandler(txtCalRange5Text_TextChanged);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            //btnCancel.Click += new EventHandler(base.CancelButtonClick);
            btnDeactivate.Click += new EventHandler(btnDeactivate_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
         {
            if (!IsPostBack)
            {
                BindgvwMealTypes();
                SetButtons();
                LoadForm(); //to load the saved values before refresh or page-load
            }
        }

        protected override void LoadForm()
        {
            try
            {
                CurrentProgram = hccProgram.GetById(this.PrimaryKeyIndex);
                if (!IsPostBack)
                {
                    if (CurrentProgram == null)
                        CurrentProgram = new hccProgram();

                    txtProgramName.Text = CurrentProgram.Name;
                    txtProgramDesc.Text = CurrentProgram.Description;

                    if (CurrentProgram.MoreInfoNavID.HasValue)
                        PagePicker1.SelectedNavigationId = CurrentProgram.MoreInfoNavID.Value;

                    if (!string.IsNullOrWhiteSpace(CurrentProgram.ImagePath))
                        ImagePicker1.ImagePath = CurrentProgram.ImagePath;

                    chkDisplayOnWebsite.Checked = CurrentProgram.DisplayOnWebsite;

                    //set req meal type vals
                    List<hccProgramMealType> mt = hccProgramMealType.GetBy(CurrentProgram.ProgramID);

                    foreach (GridViewRow row in gvwMealTypes.Rows)
                    {
                        if (row.RowType == DataControlRowType.DataRow)
                        {
                            int rowMealTypeId = int.Parse(gvwMealTypes.DataKeys[row.RowIndex].Value.ToString());

                            hccProgramMealType progMealType = mt.Where(a => a.ProgramID == this.PrimaryKeyIndex
                                && a.MealTypeID == rowMealTypeId).SingleOrDefault();

                            TextBox txtReqQuantity = (TextBox)row.FindControl("txtReqQuantity");
                            if (txtReqQuantity != null && progMealType != null)
                                txtReqQuantity.Text = progMealType.RequiredQuantity.ToString();
                        }
                    }

                    try
                    {
                        List<hccProgramOption> cr = hccProgramOption.GetBy(CurrentProgram.ProgramID);

                        if (cr != null)
                        {
                            txtCalRange1Text.Text = cr.Single(a => a.OptionIndex == 1).OptionText;
                            txtCalRange1Value.Text = cr.Single(a => a.OptionIndex == 1).OptionValue.ToString("f2");
                            chkIsDefault1.Checked = cr.Single(a => a.OptionIndex == 1).IsDefault;
                            txtCalRange2Text.Text = cr.Single(a => a.OptionIndex == 2).OptionText;
                            txtCalRange2Value.Text = cr.Single(a => a.OptionIndex == 2).OptionValue.ToString("f2");
                            chkIsDefault2.Checked = cr.Single(a => a.OptionIndex == 2).IsDefault;
                            txtCalRange3Text.Text = cr.Single(a => a.OptionIndex == 3).OptionText;
                            txtCalRange3Value.Text = cr.Single(a => a.OptionIndex == 3).OptionValue.ToString("f2");
                            chkIsDefault3.Checked = cr.Single(a => a.OptionIndex == 3).IsDefault;
                            txtCalRange4Text.Text = cr.Single(a => a.OptionIndex == 4).OptionText;
                            txtCalRange4Value.Text = cr.Single(a => a.OptionIndex == 4).OptionValue.ToString("f2");
                            chkIsDefault4.Checked = cr.Single(a => a.OptionIndex == 4).IsDefault;
                            txtCalRange5Text.Text = cr.Single(a => a.OptionIndex == 5).OptionText;
                            txtCalRange5Value.Text = cr.Single(a => a.OptionIndex == 5).OptionValue.ToString("f2");
                            chkIsDefault5.Checked = cr.Single(a => a.OptionIndex == 5).IsDefault;
                        }
                    }
                    catch { }

                    try
                    {
                        gvwPlans.DataSource = hccProgramPlan.GetBy(CurrentProgram.ProgramID, true);
                        gvwPlans.DataBind();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    SetButtons();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SaveForm()
        {
            try
            {
                CurrentProgram = hccProgram.GetById(this.PrimaryKeyIndex);

                if (CurrentProgram == null)
                    CurrentProgram = new hccProgram { IsActive = true };

                CurrentProgram.Name = txtProgramName.Text.Trim();
                CurrentProgram.Description = txtProgramDesc.Text.Trim();

                if (PagePicker1.SelectedNavigationId > 0)
                    CurrentProgram.MoreInfoNavID = PagePicker1.SelectedNavigationId;
                else
                    CurrentProgram.MoreInfoNavID = 0;

                if (!String.IsNullOrWhiteSpace(ImagePicker1.ImagePath))
                    CurrentProgram.ImagePath = ImagePicker1.ImagePath;

                CurrentProgram.DisplayOnWebsite = chkDisplayOnWebsite.Checked;

                if (this.PrimaryKeyIndex == 0)
                {
                    CurrentProgram.Save();
                    this.PrimaryKeyIndex = CurrentProgram.ProgramID;
                }

                //set req meal type vals
                List<hccProgramMealType> mt = hccProgramMealType.GetBy(CurrentProgram.ProgramID);

                foreach (GridViewRow row in gvwMealTypes.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        int rowMealTypeId = int.Parse(gvwMealTypes.DataKeys[row.RowIndex].Value.ToString());

                        hccProgramMealType progMealType = mt.Where(a => a.ProgramID == this.PrimaryKeyIndex
                            && a.MealTypeID == rowMealTypeId).SingleOrDefault();

                        if (progMealType == null)
                            progMealType = new hccProgramMealType { ProgramID = this.PrimaryKeyIndex, MealTypeID = rowMealTypeId, RequiredQuantity = 0 };

                        TextBox txtReqQuantity = (TextBox)row.FindControl("txtReqQuantity");

                        if (txtReqQuantity != null)
                            progMealType.RequiredQuantity =
                                string.IsNullOrWhiteSpace(txtReqQuantity.Text.Trim()) ? 0 : int.Parse(txtReqQuantity.Text.Trim());

                        progMealType.Save();
                    }
                }


                List<hccProgramOption> cr = hccProgramOption.GetBy(CurrentProgram.ProgramID);
                var optionValue = 0m;
                hccProgramOption opt;
                try
                {
                    opt = cr.FirstOrDefault(x => x.OptionIndex == 1);
                    //hccProgramOption opt1 = cr.SingleOrDefault(a => a.OptionIndex == 1);
                    optionValue = txtCalRange1Value.Text.Trim() != "" ? decimal.Parse(txtCalRange1Value.Text.Trim()) : 0m;

                    if (opt == null)
                    {
                        
                        opt = new hccProgramOption
                            {
                                OptionIndex = 1,
                                ProgramID = CurrentProgram.ProgramID,
                                OptionText = txtCalRange1Text.Text.Trim(),
                                OptionValue = optionValue,
                                IsDefault = chkIsDefault1.Checked
                            };
                        //opt.Save();
                    }
                    else
                    {
                        opt.OptionIndex = 1;
                        opt.ProgramID = CurrentProgram.ProgramID;
                        opt.OptionText = txtCalRange1Text.Text.Trim();
                        opt.OptionValue = optionValue;
                        opt.IsDefault = chkIsDefault1.Checked;                        
                    };
                    opt.Save();                        
                }
                catch { }

                try
                {
                    opt = cr.FirstOrDefault(x => x.OptionIndex == 2);
                    //hccProgramOption opt2 = cr.SingleOrDefault(a => a.OptionIndex == 2);
                    optionValue = txtCalRange2Value.Text.Trim() != "" ? decimal.Parse(txtCalRange2Value.Text.Trim()) : 0m;

                    if (opt == null)
                    {
                        opt = new hccProgramOption
                        {
                            OptionIndex = 2,
                            ProgramID = CurrentProgram.ProgramID,
                            OptionText = txtCalRange2Text.Text.Trim(),
                            OptionValue = optionValue,
                            IsDefault = chkIsDefault2.Checked
                        };
                    }
                    else
                    {
                        opt.OptionIndex = 2;
                        opt.ProgramID = CurrentProgram.ProgramID;
                        opt.OptionText = txtCalRange2Text.Text.Trim();
                        opt.OptionValue = optionValue;
                        opt.IsDefault = chkIsDefault2.Checked;
                    };
                    opt.Save(); 
                }
                catch { }

                try
                {
                    //hccProgramOption opt3 = cr.SingleOrDefault(a => a.OptionIndex == 3);
                    opt = cr.FirstOrDefault(x => x.OptionIndex == 3);
                    optionValue = txtCalRange3Value.Text.Trim() != "" ? decimal.Parse(txtCalRange3Value.Text.Trim()) : 0m;
                    if (opt == null)
                    {
                        opt = new hccProgramOption
                        {
                            OptionIndex = 3,
                            ProgramID = CurrentProgram.ProgramID,
                            OptionText = txtCalRange3Text.Text.Trim(),
                            OptionValue = optionValue,
                            IsDefault = chkIsDefault3.Checked
                        };
                        //opt3.Save();
                    }
                    else
                    {
                        opt.OptionIndex = 3;
                        opt.ProgramID = CurrentProgram.ProgramID;
                        opt.OptionText = txtCalRange3Text.Text.Trim();
                        opt.OptionValue = optionValue;
                        opt.IsDefault = chkIsDefault3.Checked;
                    };
                    opt.Save();
                }
                catch { }

                try
                {
                    opt = cr.FirstOrDefault(a => a.OptionIndex == 4);
                    optionValue = txtCalRange4Value.Text.Trim() != "" ? decimal.Parse(txtCalRange4Value.Text.Trim()) : 0m;
                    if (opt == null)
                    {
                        opt = new hccProgramOption
                        {
                            OptionIndex = 4,
                            ProgramID = CurrentProgram.ProgramID,
                            OptionText = txtCalRange4Text.Text.Trim(),
                            OptionValue = optionValue,
                            IsDefault = chkIsDefault4.Checked
                        };
                        //opt4.Save();
                    }
                    else
                    {
                        opt.OptionIndex = 4;
                        opt.ProgramID = CurrentProgram.ProgramID;
                        opt.OptionText = txtCalRange4Text.Text.Trim();
                        opt.OptionValue = optionValue;
                        opt.IsDefault = chkIsDefault4.Checked;

                    };
                    opt.Save();
                }
                catch { }

                try
                {
                    opt = cr.FirstOrDefault(a => a.OptionIndex == 5);
                    optionValue = txtCalRange5Value.Text.Trim() != "" ? decimal.Parse(txtCalRange5Value.Text.Trim()) : 0m;
                    if (opt == null)
                    {
                        opt = new hccProgramOption
                        {
                            OptionIndex = 5,
                            ProgramID = CurrentProgram.ProgramID,
                            OptionText = txtCalRange5Text.Text.Trim(),
                            OptionValue = optionValue,
                            IsDefault = chkIsDefault5.Checked
                        };
                        //opt5.Save();
                    }
                    else
                    {
                        opt.OptionIndex = 5;
                        opt.ProgramID = CurrentProgram.ProgramID;
                        opt.OptionText = txtCalRange5Text.Text.Trim();
                        opt.OptionValue = optionValue;
                        opt.IsDefault = chkIsDefault5.Checked;
                    };
                    opt.Save();
                }
                catch { }

                CurrentProgram.Save();
                this.OnSaved(new ControlSavedEventArgs(CurrentProgram.ProgramID));
            }
            catch
            {
                throw;
            }
            Response.Redirect("~/WebModules/ShoppingCart/Admin/ProgramManager.aspx", false);
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            ShowDeactivate = false;
            txtProgramName.Text = string.Empty;
            txtProgramDesc.Text = string.Empty;
            PagePicker1.ClearSelection();
            CleargvwMealTypes();
            txtCalRange1Text.Text = string.Empty;
            txtCalRange1Value.Text = string.Empty;
            chkIsDefault1.Checked = false;
            txtCalRange2Text.Text = string.Empty;
            txtCalRange2Value.Text = string.Empty;
            chkIsDefault2.Checked = false;
            txtCalRange3Text.Text = string.Empty;
            txtCalRange3Value.Text = string.Empty;
            chkIsDefault3.Checked = false;
            txtCalRange4Text.Text = string.Empty;
            txtCalRange4Value.Text = string.Empty;
            chkIsDefault4.Checked = false;
            txtCalRange5Text.Text = string.Empty;
            txtCalRange5Value.Text = string.Empty;
            chkIsDefault5.Checked = false;
            chkDisplayOnWebsite.Checked = false;
            gvwPlans.DataSource = hccProgramPlan.GetBy(0, true);
            gvwPlans.DataBind();
            ImagePicker1.ImagePath = "";
            SetButtons();
        }

        protected override void SetButtons()
        {
            ShowDeactivate = false;

            if (this.PrimaryKeyIndex > 0)
            {
                ShowDeactivate = true;

                if (CurrentProgram == null)
                    CurrentProgram = hccProgram.GetById(this.PrimaryKeyIndex);

                if (CurrentProgram != null)
                {
                    if (CurrentProgram.IsActive)
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

        void txtCalRange5Text_TextChanged(object sender, EventArgs e)
        {
            rfvCalRange5Value.Enabled = !string.IsNullOrWhiteSpace(txtCalRange5Text.Text);
        }

        void txtCalRange4Text_TextChanged(object sender, EventArgs e)
        {
            rfvCalRange4Value.Enabled = !string.IsNullOrWhiteSpace(txtCalRange4Text.Text);
        }

        void txtCalRange3Text_TextChanged(object sender, EventArgs e)
        {
            rfvCalRange3Value.Enabled = !string.IsNullOrWhiteSpace(txtCalRange3Text.Text);
        }

        void txtCalRange2Text_TextChanged(object sender, EventArgs e)
        {
            rfvCalRange2Value.Enabled = !string.IsNullOrWhiteSpace(txtCalRange2Text.Text);
        }

        void txtCalRange1Text_TextChanged(object sender, EventArgs e)
        {
            rfvCalRange1Value.Enabled = !string.IsNullOrWhiteSpace(txtCalRange1Text.Text);
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            if (CurrentProgram == null)
                CurrentProgram = hccProgram.GetById(this.PrimaryKeyIndex);

            if (CurrentProgram != null)
            {
                CurrentProgram.Retire(!UseReactivateBehavior);
                this.OnSaved(new ControlSavedEventArgs(CurrentProgram.ProgramID));
            }
        }

        void BindgvwMealTypes()
        {
            List<RequiredMealType> reqTypes = new List<RequiredMealType>();
            List<Tuple<string, int>> mealTypes = Enums.GetEnumAsTupleList(typeof(Enums.MealTypes));
            Tuple<string, int> unkType = mealTypes.Single(a => a.Item1 == "Unknown");
            mealTypes.Remove(unkType);

            foreach (Tuple<string, int> mealType in mealTypes)
            {
                reqTypes.Add(new RequiredMealType
                {
                    MealTypeID = mealType.Item2,
                    MealTypeName = mealType.Item1,
                    RequiredQuantity = 0
                });
            }

            gvwMealTypes.DataSource = reqTypes;
            gvwMealTypes.DataBind();
        }

        void CleargvwMealTypes()
        {
            foreach (GridViewRow row in gvwMealTypes.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtReqQuantity = (TextBox)row.FindControl("txtReqQuantity");

                    if (txtReqQuantity != null)
                        txtReqQuantity.Text = 0.ToString();
                }
            }
        }

        protected void cstName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            hccProgram existProg = hccProgram.GetBy(txtProgramName.Text.Trim());

            if (existProg != null && existProg.ProgramID != this.PrimaryKeyIndex)
                args.IsValid = false;
        }

        protected void cstValPlanPriceDefault_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            int checkedCount = 0;

            if (chkIsDefault1.Checked) { checkedCount++; }
            if (chkIsDefault2.Checked) { checkedCount++; }
            if (chkIsDefault3.Checked) { checkedCount++; }
            if (chkIsDefault4.Checked) { checkedCount++; }
            if (chkIsDefault5.Checked) { checkedCount++; }

            if (checkedCount == 0 || checkedCount > 1)
                e.IsValid = false;
        }
    }

    public class RequiredMealType
    {
        public int MealTypeID { get; set; }
        public int ProgramID { get; set; }
        public int RequiredQuantity { get; set; }
        public string MealTypeName { get; set; }
    }
}