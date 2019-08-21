using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using System.Web.Security;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class UserProfileAllg_Edit : FormControlBase
    {   //this.PrimaryKeyIndex as hccUserProfile.UserProfileId
        hccUserProfile CurrentUserProfile { get; set; }

        /// <summary>
        /// Determines whether the Save button should be displayed. Default = false.
        /// </summary>
        public bool ShowSave
        {
            get
            {
                if (ViewState["ShowSave"] == null)
                    ViewState["ShowSave"] = false;

                return bool.Parse(ViewState["ShowSave"].ToString());
            }
            set
            {
                ViewState["ShowSave"] = value;
            }
        }

        /// <summary>
        /// Sets the text value of the Save button. Default = "Submit".
        /// </summary>
        public string SaveText
        {
            get
            {
                if (ViewState["SaveText"] == null)
                    ViewState["SaveText"] = "Submit";

                return ViewState["SaveText"].ToString();
            }
            set
            {
                ViewState["SaveText"] = value;
            }
        }

        public bool NotesAllowDisplayToUser
        {
            get
            {
                if (ViewState["NotesAllowDisplayToUser"] == null)
                    ViewState["NotesAllowDisplayToUser"] = false;

                return bool.Parse(ViewState["NotesAllowDisplayToUser"].ToString());
            }
            set
            {
                ViewState["NotesAllowDisplayToUser"] = value;
            }
        }

        public bool NotesAllowAddEdit
        {
            get
            {
                if (ViewState["NotesAllowAddEdit"] == null)
                    ViewState["NotesAllowAddEdit"] = false;

                return bool.Parse(ViewState["NotesAllowAddEdit"].ToString());
            }
            set
            {
                ViewState["NotesAllowAddEdit"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
        }

        protected override void LoadForm()
        {
            SetButtons();
            BindcblAllergens();

            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            if (CurrentUserProfile != null)
            {
                List<hccUserProfileAllergen> userAllgs = hccUserProfileAllergen.GetBy(CurrentUserProfile.UserProfileID, true);

                foreach (ListItem item in cblAllergens.Items)
                {
                    int allgId = int.Parse(item.Value);

                    if (userAllgs.Where(a => a.AllergenID == allgId).Count() > 0)
                        item.Selected = true;
                }

                ProfileNotesEdit_Allgs.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                ProfileNotesEdit_Allgs.AllowAddEdit = NotesAllowAddEdit;
                ProfileNotesEdit_Allgs.AllowDisplayToUser = NotesAllowDisplayToUser;
                ProfileNotesEdit_Allgs.Bind();
            }


        }

        protected override void SaveForm()
        {
            try
            {
                if (CurrentUserProfile == null)
                    CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

                if (CurrentUserProfile == null) return;

                List<hccUserProfileAllergen> existingAllergen = hccUserProfileAllergen.GetBy(CurrentUserProfile.UserProfileID, true);
                existingAllergen.ForEach(a=>a.Delete());

                foreach (ListItem item in cblAllergens.Items)
                {
                    if (item.Selected)
                    {
                        hccUserProfileAllergen all = new hccUserProfileAllergen
                        {
                            AllergenID = (int.Parse(item.Value)),
                            UserProfileID = CurrentUserProfile.UserProfileID,
                            IsActive = true
                        };
                        all.Save();
                    }
                }

                CurrentUserProfile.Save();

                lblFeedback.Text = "Allergens saved.";
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected override void ClearForm()
        {
            try
            {
                this.PrimaryKeyIndex = 0;
                cblAllergens.ClearSelection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void BindcblAllergens()
        {
            cblAllergens.ClearSelection();

            if (cblAllergens.Items.Count == 0)
            {
                cblAllergens.DataSource = hccAllergen.GetActive();
                cblAllergens.DataTextField = "Name";
                cblAllergens.DataValueField = "AllergenID";
                cblAllergens.DataBind();
            }
        }

        protected override void SetButtons()
        {
            btnSave.Visible = ShowSave;

            if (ShowSave)
            {
                btnSave.Text = SaveText;
            }
        }
    }
}