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
    public partial class UserProfileNote_Edit : FormControlBase
    {   //this.PrimaryKeyIndex as hccUserProfileNote.NoteId 
        public Enums.UserProfileNoteTypes CurrentNoteType
        {
            get
            {
                if (ViewState["CurrentNoteType"] == null)
                    return Enums.UserProfileNoteTypes.Unknown;
                else
                    return (Enums.UserProfileNoteTypes)ViewState["CurrentNoteType"];
            }
            set
            {
                ViewState["CurrentNoteType"] = value;
            }
        }

        public int CurrentUserProfileId
        {
            get
            {
                if (ViewState["CurrentUserProfileId"] == null)
                    ViewState["CurrentUserProfileId"] = 0;

                return int.Parse(ViewState["CurrentUserProfileId"].ToString());
            }
            set
            {
                ViewState["CurrentUserProfileId"] = value;
            }
        }

        public bool AllowDisplayToUser
        {
            get
            {
                if (ViewState["AllowDisplayToUser"] == null)
                    ViewState["AllowDisplayToUser"] = false;

                return bool.Parse(ViewState["AllowDisplayToUser"].ToString());
            }
            set
            {
                ViewState["AllowDisplayToUser"] = value;
            }
        }

        public bool ShowAllNotes
        {
            get
            {
                if (ViewState["ShowAllNotes"] == null)
                    ViewState["ShowAllNotes"] = false;

                return bool.Parse(ViewState["ShowAllNotes"].ToString());
            }
            set
            {
                ViewState["ShowAllNotes"] = value;
            }
        }

        public bool AllowAddEdit
        {
            get
            {
                if (ViewState["AllowAddEdit"] == null)
                    ViewState["AllowAddEdit"] = false;

                return bool.Parse(ViewState["AllowAddEdit"].ToString());
            }
            set
            {
                ViewState["AllowAddEdit"] = value;
            }
        }

        public string UserDisplayNotesTitle
        {
            get
            {
                if (ViewState["UserDisplayNotesTitle"] == null)
                    ViewState["UserDisplayNotesTitle"] = string.Empty;

                return ViewState["UserDisplayNotesTitle"].ToString();
            }
            set
            {
                ViewState["UserDisplayNotesTitle"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.ValidationGroup = "UserNotesEditGroup" + Guid.NewGuid().ToString();

            base.OnInit(e);
            
            gvwNotes.RowCreated += new GridViewRowEventHandler(gvwNotes_RowCreated);
            gvwNotes.RowDeleting += new GridViewDeleteEventHandler(gvwNotes_RowDeleting);
            gvwNotes.SelectedIndexChanged += new EventHandler(gvwNotes_SelectedIndexChanged);

            btnAddNote.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
        }

        void LoadAddEdit()
        {
            if (AllowAddEdit)
            {
                pnlNotesAddEdit.Visible = true;

                BindgvwNotes();

                if (AllowDisplayToUser)
                {
                    chkNoteDisplayToUser.Visible = true;
                    chkNoteDisplayToUser.Attributes.Add("onclick", "javascript: return confirm('Are you sure that you want to change whether or not this note is to be displayed to the user?')");
                }
            }
        }

        void LoadUserDisplay()
        {
            if (AllowDisplayToUser)
            {
                UserDisplayNotesTitle = HealthyChef.Common.Enums.GetEnumDescription(CurrentNoteType) + "(s)";
                List<hccUserProfileNote> notes;

                if(ShowAllNotes)
                    notes = hccUserProfileNote.GetBy(CurrentUserProfileId, CurrentNoteType, null);
                else
                    notes = hccUserProfileNote.GetBy(CurrentUserProfileId, CurrentNoteType, true);

                //if (notes.Count > 0)
                //{
                //    pnlNotesDisplay.Visible = true;
                //    lblNotesTitle.Text = UserDisplayNotesTitle;
                //    lblDisplayNotes.Text = notes.Select(a=>a.Note).DefaultIfEmpty(string.Empty).Aggregate((a,b) => a + "; " + b);                    
                //}
            }
        }
        
        protected override void LoadForm()
        {
            LoadAddEdit();
            LoadUserDisplay();
        }

        protected override void SaveForm()
        {
            try
            {
                if (CurrentUserProfileId > 0)
                {
                    hccUserProfileNote note = null;

                    if (this.PrimaryKeyIndex > 0)
                        note = hccUserProfileNote.GetById(this.PrimaryKeyIndex);

                    if (note == null)
                        note = new hccUserProfileNote
                        {
                            DateCreated = DateTime.Now,                            
                            UserProfileID = CurrentUserProfileId,
                            NoteTypeID = (int)CurrentNoteType
                        };

                    if (AllowDisplayToUser)
                        note.DisplayToUser = chkNoteDisplayToUser.Checked;
                    else
                        note.DisplayToUser = false;

                    note.Note = txtNote.Text.Trim();

                    note.Save();
                    this.PrimaryKeyIndex = note.NoteID;

                    Clear();

                    BindgvwNotes();

                    LoadAddEdit();
                    LoadUserDisplay();

                    OnSaved(new ControlSavedEventArgs(this.PrimaryKeyIndex));
                }
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
                txtNote.Text = string.Empty;
                chkNoteDisplayToUser.Checked = false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void BindgvwNotes()
        {
            try
            {
                List<hccUserProfileNote> notes = hccUserProfileNote.GetBy(CurrentUserProfileId, CurrentNoteType);

                gvwNotes.DataSource = notes;
                gvwNotes.DataBind();
            }
            catch
            {
                throw;
            }
        }

        void gvwNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int noteId = int.Parse(gvwNotes.DataKeys[gvwNotes.SelectedIndex].Value.ToString());

                if (noteId > 0)
                {
                    this.PrimaryKeyIndex = noteId;
                    hccUserProfileNote note = hccUserProfileNote.GetById(this.PrimaryKeyIndex);

                    txtNote.Text = note.Note;

                    if(AllowDisplayToUser)
                        chkNoteDisplayToUser.Checked = note.DisplayToUser;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void gvwNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int noteId = int.Parse(gvwNotes.DataKeys[e.RowIndex].Value.ToString());

                hccUserProfileNote delNote = hccUserProfileNote.GetById(noteId);

                if (delNote != null)
                {
                    delNote.Delete();
                }
                Clear();
                BindgvwNotes();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void gvwNotes_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lkbDelete = e.Row.Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Delete");

                    if (lkbDelete != null)
                        lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to delete this note?');");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}