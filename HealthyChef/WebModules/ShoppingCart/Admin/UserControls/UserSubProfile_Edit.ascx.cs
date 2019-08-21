using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using System.Web.Security;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class UserSubProfile_Edit : FormControlBase
    {   //this.PrimaryKeyIndex as hccUserProfile.UserProfileId(SubProfile)
        public int CurrentParentProfileId
        {
            get
            {
                if (ViewState["ParentProfileId"] == null)
                    ViewState["ParentProfileId"] = 0;

                return int.Parse(ViewState["ParentProfileId"].ToString());
            }
            set { ViewState["ParentProfileId"] = value; }
        }

        public Guid? CurrentParentAspNetId
        {
            get
            {
                if (ViewState["CurrentAspNetId"] == null)
                    return null;
                else
                    return Guid.Parse(ViewState["CurrentAspNetId"].ToString());
            }
            set
            {
                ViewState["CurrentAspNetId"] = value;
            }
        }

        protected hccUserProfile CurrentUserProfile { get; set; }

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

        //protected List<hccUserProfilePhone> PhoneNumberHolder
        //{
        //    get
        //    {
        //        List<hccUserProfilePhone> currentPhones = new List<hccUserProfilePhone>();

        //        if (ViewState["PhoneNumberHolder"] == null)
        //            ViewState["PhoneNumberHolder"] = new List<hccUserProfilePhone>();
        //        else
        //            currentPhones = (List<hccUserProfilePhone>)ViewState["PhoneNumberHolder"];

        //        CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

        //        if (CurrentUserProfile != null)
        //            this.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;

        //        if (this.PrimaryKeyIndex > 0)
        //        {
        //            List<hccUserProfilePhone> userPhones = hccUserProfilePhone.GetBy(this.PrimaryKeyIndex, true);

        //            foreach (hccUserProfilePhone userPhone in userPhones)
        //            {
        //                if (currentPhones.Where(a => a.PhoneID == userPhone.PhoneID).Count() == 0)
        //                    currentPhones.Add(userPhone);
        //            }
        //        }

        //        ViewState["PhoneNumberHolder"] = currentPhones;

        //        return currentPhones;
        //    }
        //    set
        //    {
        //        ViewState["PhoneNumberHolder"] = value;
        //    }
        //}

        protected override void OnInit(EventArgs e)
        {
            this.Page.MaintainScrollPositionOnPostBack = true;
            AddressEdit_SubShipping.SetFormFieldValidationGroup(this.ValidationGroup);

            base.OnInit(e);

            btnAddSubProfile.Click += new EventHandler(btnAddSubProfile_Click);

            chkUseParentShippingAddress.CheckedChanged += chkUseParentShippingAddress_CheckedChanged;
            btnSave0.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel0.Click += new EventHandler(base.CancelButtonClick);

            SubProfileNotesEdit_Shipping.ControlCancelled += ProfileNotesEdit_ControlCancelled;
        }

        protected override void LoadForm()
        {
            try
            {
                //form fields
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);
                AddressEdit_SubShipping.Clear();
                chkUseParentShippingAddress.Checked = false;
                if (CurrentUserProfile != null)
                {
                    chkSubIsActive.Checked = CurrentUserProfile.IsActive;
                    txtSubProfileName.Text = CurrentUserProfile.ProfileName;
                    txtSubFirstName.Text = CurrentUserProfile.FirstName;
                    txtSubLastName.Text = CurrentUserProfile.LastName;

                    //shipping address
                    if (CurrentUserProfile.UseParentShipping)
                    {
                        AddressEdit_SubShipping.Clear();
                        AddressEdit_SubShipping.Visible = false;
                        chkUseParentShippingAddress.Checked = true;
                    }
                    else
                    {
                        AddressEdit_SubShipping.Clear();
                        AddressEdit_SubShipping.Visible = true;
                        if (CurrentUserProfile.ShippingAddressID.HasValue)
                        {
                            AddressEdit_SubShipping.PrimaryKeyIndex = CurrentUserProfile.ShippingAddressID.Value;
                            AddressEdit_SubShipping.Bind();
                        }
                    }

                    //preferences
                    SubProfilePrefsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;

                    //allergens
                    SubProfileAllgsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;

                    //shipping notes
                    SubProfileNotesEdit_Shipping.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                    SubProfileNotesEdit_Shipping.AllowAddEdit = NotesAllowAddEdit;
                    SubProfileNotesEdit_Shipping.AllowDisplayToUser = NotesAllowDisplayToUser;

                    DisplayProfileTabs(true);

                    SubProfileNotesEdit_Shipping.Bind();
                }

                SubProfilePrefsEdit1.NotesAllowAddEdit = NotesAllowAddEdit;
                SubProfilePrefsEdit1.NotesAllowDisplayToUser = NotesAllowDisplayToUser;

                SubProfileAllgsEdit1.NotesAllowAddEdit = NotesAllowAddEdit;
                SubProfileAllgsEdit1.NotesAllowDisplayToUser = NotesAllowDisplayToUser;

                SubProfilePrefsEdit1.Bind();
                SubProfileAllgsEdit1.Bind();
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
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);
                hccUserProfile parentProfile = hccUserProfile.GetById(CurrentParentProfileId);

                if (CurrentUserProfile == null && parentProfile != null)
                {
                    hccUserProfile newProfile = new hccUserProfile
                    {
                        ParentProfileID = CurrentParentProfileId,
                        CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        MembershipID = parentProfile.MembershipID
                    };

                    newProfile.Save();
                    this.PrimaryKeyIndex = newProfile.UserProfileID;
                    CurrentUserProfile = newProfile;
                }

                if (CurrentUserProfile != null)
                {
                    CurrentUserProfile.IsActive = chkSubIsActive.Checked;

                    if (txtSubProfileName.Text.Trim() != CurrentUserProfile.ProfileName)
                        CurrentUserProfile.ProfileName = txtSubProfileName.Text.Trim();

                    if (txtSubFirstName.Text.Trim() != CurrentUserProfile.FirstName)
                        CurrentUserProfile.FirstName = txtSubFirstName.Text.Trim();

                    if (txtSubLastName.Text.Trim() != CurrentUserProfile.LastName)
                        CurrentUserProfile.LastName = txtSubLastName.Text.Trim();

                    //save addresses
                    if (chkUseParentShippingAddress.Checked)
                    {
                        if (CurrentUserProfile.ShippingAddressID != 0)
                        {
                            CurrentUserProfile.UseParentShipping = true;
                            //CurrentUserProfile.ShippingAddressID = parentProfile.ShippingAddressID;
                        }
                        else
                        {
                            CurrentUserProfile.UseParentShipping = true;
                            CurrentUserProfile.ShippingAddressID = AddressEdit_SubShipping.PrimaryKeyIndex;
                        }
                    }
                    else
                    {
                        AddressEdit_SubShipping.Save();
                        CurrentUserProfile.UseParentShipping = false;
                        CurrentUserProfile.ShippingAddressID = AddressEdit_SubShipping.PrimaryKeyIndex;
                    }

                    //preferences
                    SubProfilePrefsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                    SubProfilePrefsEdit1.Save();

                    //allergens
                    SubProfileAllgsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                    SubProfileAllgsEdit1.Save();

                    CurrentUserProfile.Save();
                }

                //DisplayProfileTabs(false);
                lblSubProfileFeedback.Text = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                lblSubProfileFeedback.ForeColor = System.Drawing.Color.Green;

                OnSaved(new ControlSavedEventArgs(this.PrimaryKeyIndex));
                ClearForm();
            }
            catch
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            DisplayProfileTabs(false);

            txtSubProfileName.Text = string.Empty;
            txtSubFirstName.Text = string.Empty;
            txtSubLastName.Text = string.Empty;
            chkUseParentShippingAddress.Checked = false;

            AddressEdit_SubShipping.Clear();
            AddressEdit_SubShipping.Visible = true;
            SubProfileNotesEdit_Shipping.Clear();

            //preferences
            SubProfilePrefsEdit1.Clear();

            //allergens
            SubProfileAllgsEdit1.Clear();
        }

        void btnAddSubProfile_Click(object sender, EventArgs e)
        {
            this.Clear();

            btnAddSubProfile.Visible = false;
            DisplayProfileTabs(true);
            hdnSubLastTab.Value = "Basic";

            this.Bind();
        }

        void chkUseParentShippingAddress_CheckedChanged(object sender, EventArgs e)
        {
            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            if (chkUseParentShippingAddress.Checked)
            {
                AddressEdit_SubShipping.Clear();
                AddressEdit_SubShipping.Visible = false;

                if (CurrentUserProfile != null)
                {
                    //CurrentUserProfile.ShippingAddressID = null;
                    CurrentUserProfile.UseParentShipping = true;
                    CurrentUserProfile.Save();
                    Page.Response.Redirect(Page.Request.Url.ToString(), true);
                }
                else
                {
                    if (txtSubProfileName.Text == "" || txtSubFirstName.Text == "" || txtSubLastName.Text == "")
                    {
                        lblSubProfileFeedback.Text = "Please fill Basic Info before filling Shipping Info";
                        lblSubProfileFeedback.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        try
                        {
                            CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);
                            hccUserProfile parentProfile = hccUserProfile.GetById(CurrentParentProfileId);

                            if (CurrentUserProfile == null && parentProfile != null)
                            {

                                hccUserProfile newProfile = new hccUserProfile
                                {
                                    ParentProfileID = CurrentParentProfileId,
                                    CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                    CreatedDate = DateTime.Now,
                                    IsActive = true,
                                    MembershipID = parentProfile.MembershipID
                                };

                                newProfile.Save();
                                this.PrimaryKeyIndex = newProfile.UserProfileID;
                                CurrentUserProfile = newProfile;

                            }

                            if (CurrentUserProfile != null)
                            {
                                CurrentUserProfile.IsActive = chkSubIsActive.Checked;

                                if (txtSubProfileName.Text.Trim() != CurrentUserProfile.ProfileName)
                                    CurrentUserProfile.ProfileName = txtSubProfileName.Text.Trim();

                                if (txtSubFirstName.Text.Trim() != CurrentUserProfile.FirstName)
                                    CurrentUserProfile.FirstName = txtSubFirstName.Text.Trim();

                                if (txtSubLastName.Text.Trim() != CurrentUserProfile.LastName)
                                    CurrentUserProfile.LastName = txtSubLastName.Text.Trim();

                                //save addresses
                                if (chkUseParentShippingAddress.Checked)
                                {
                                    if (CurrentUserProfile.ShippingAddressID != 0)
                                    {
                                        CurrentUserProfile.UseParentShipping = true;
                                        //CurrentUserProfile.ShippingAddressID = parentProfile.ShippingAddressID;
                                    }
                                    else
                                    {
                                        CurrentUserProfile.UseParentShipping = true;
                                        CurrentUserProfile.ShippingAddressID = AddressEdit_SubShipping.PrimaryKeyIndex;
                                    }
                                }
                                else
                                {
                                    AddressEdit_SubShipping.Save();
                                    CurrentUserProfile.UseParentShipping = false;
                                    CurrentUserProfile.ShippingAddressID = AddressEdit_SubShipping.PrimaryKeyIndex;
                                }

                                //preferences
                                SubProfilePrefsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                                SubProfilePrefsEdit1.Save();

                                //allergens
                                SubProfileAllgsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                                SubProfileAllgsEdit1.Save();

                                CurrentUserProfile.Save();


                                //DisplayProfileTabs(false);
                                lblSubProfileFeedback.Text = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                                lblSubProfileFeedback.ForeColor = System.Drawing.Color.Green;

                                OnSaved(new ControlSavedEventArgs(this.PrimaryKeyIndex));
                            }
                        }
                        catch
                        {
                            throw;
                        }
                    }
                }
            }
            else
            {
                AddressEdit_SubShipping.Visible = true;

                if (CurrentUserProfile != null)
                {
                    CurrentUserProfile.UseParentShipping = false;
                    CurrentUserProfile.Save();
                }
            }

        }

        void DisplayProfileTabs(bool isVisible)
        {
            hdnShowSubTabs.Value = isVisible.ToString().ToLower();
            liAddresses.Visible = isVisible;
            liPrefs.Visible = isVisible;
            liAllergens.Visible = isVisible;

            btnAddSubProfile.Visible = !isVisible;
            btnSave0.Visible = isVisible;
            btnCancel0.Visible = isVisible;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        protected void cstProfileName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            // sub-profiles slightly different than cstProfileName_ServerValidate for main profile
            if (CurrentParentAspNetId.HasValue)
            {
                hccUserProfile prof = hccUserProfile.GetBy(CurrentParentAspNetId.Value, txtSubProfileName.Text.Trim());

                if (prof != null && prof.UserProfileID != this.PrimaryKeyIndex)
                {
                    args.IsValid = false;
                }
            }
        }

        void ProfileNotesEdit_ControlCancelled(object sender)
        {
            ((UserProfileNote_Edit)sender).Clear();
        }
    }
}