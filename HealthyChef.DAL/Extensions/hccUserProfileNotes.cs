using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccUserProfileNote
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    EntityKey key = cont.CreateEntityKey("hccUserProfileNotes", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        cont.ApplyCurrentValues(key.EntitySetName, this);
                    }
                    else
                    {
                        cont.hccUserProfileNotes.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    EntityKey key = cont.CreateEntityKey("hccUserProfileNotes", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        cont.hccUserProfileNotes.DeleteObject((hccUserProfileNote)originalItem);
                        cont.SaveChanges();

                        //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, cont.hccUserProfileNotes);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccUserProfileNote> GetBy(int userProfileId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var t = cont.hccUserProfileNotes
                        .Where(a => a.UserProfileID == userProfileId)
                        .OrderByDescending(b => b.DateCreated)
                        .ToList();

                    return t;
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccUserProfileNote> GetBy(int userProfileId, Enums.UserProfileNoteTypes noteType)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var t = cont.hccUserProfileNotes
                        .Where(a => a.UserProfileID == userProfileId && a.NoteTypeID == (int)noteType)
                        .OrderBy(a => a.DateCreated)
                        .ToList();
                    return t;
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccUserProfileNote> GetBy(int userProfileId, Enums.UserProfileNoteTypes noteType, bool? areViewableByUser)
        {
            try
            {
                if (areViewableByUser == null)
                    return GetBy(userProfileId, noteType);
                else
                    return GetBy(userProfileId, noteType).Where(a => a.DisplayToUser == areViewableByUser).ToList();
            }
            catch
            {
                throw;
            }
        }

        public static hccUserProfileNote GetById(int noteId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfileNotes.SingleOrDefault(a => a.NoteID == noteId);
                }
            }
            catch
            {
                throw;
            }
        }

       
    }
}
