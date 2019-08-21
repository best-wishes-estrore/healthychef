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
    public partial class UserProfilePrefs_Edit : FormControlBase
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
            BindcblPreferences();

            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            if (CurrentUserProfile != null)
            {
                List<hccUserProfilePreference> userPrefs = hccUserProfilePreference.GetBy(CurrentUserProfile.UserProfileID, true);

                foreach (ListItem item in cblPreferences.Items)
                {
                    int prefId = int.Parse(item.Value);

                    if (userPrefs.Where(a => a.PreferenceID == prefId).Count() > 0)
                        item.Selected = true;
                }

                ProfileNotesEdit_Prefs.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                ProfileNotesEdit_Prefs.AllowAddEdit = NotesAllowAddEdit;
                ProfileNotesEdit_Prefs.AllowDisplayToUser = NotesAllowDisplayToUser;
                ProfileNotesEdit_Prefs.Bind();
            }
        }

        protected override void SaveForm()
        {
            try
            {
                if (CurrentUserProfile == null)
                    CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

                //If PrimaryKeyIndex was not found, then there are no preferences to be saved.
                if (CurrentUserProfile == null) return;

                List<hccUserProfilePreference> existingUserPrefs = hccUserProfilePreference.GetBy(CurrentUserProfile.UserProfileID, true);
                existingUserPrefs.ForEach(delegate(hccUserProfilePreference userPref) { userPref.Delete(); });

                foreach (ListItem item in cblPreferences.Items)
                {
                    if (item.Selected)
                    {
                        hccUserProfilePreference newPref = new hccUserProfilePreference
                        {
                            PreferenceID = int.Parse(item.Value),
                            UserProfileID = CurrentUserProfile.UserProfileID,
                            IsActive = true
                        };
                        newPref.Save();
                    }
                }

                CurrentUserProfile.Save();

                lblFeedback.Text = "Preferences saved.";
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
                cblPreferences.ClearSelection();
            }
            catch (Exception)
            {
                throw;
            }
        }

        void BindcblPreferences()
        {
            cblPreferences.ClearSelection();

            if (cblPreferences.Items.Count == 0)
            {
                cblPreferences.DataSource = hccPreference.GetBy(Enums.PreferenceType.Customer, false);
                cblPreferences.DataTextField = "Name";
                cblPreferences.DataValueField = "PreferenceID";
                cblPreferences.DataBind();
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