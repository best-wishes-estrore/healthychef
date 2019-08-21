using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccUserProfilePaymentProfile
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccUserProfilePaymentProfiles", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccUserProfilePaymentProfiles", this);
                    }
                    else
                    {
                        cont.hccUserProfilePaymentProfiles.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
        
        public static hccUserProfilePaymentProfile GetById(int paymentProfileId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccUserProfilePaymentProfiles
                        .Where(a => a.PaymentProfileID == paymentProfileId)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccUserProfilePaymentProfile GetBy(int userProfileId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccUserProfilePaymentProfiles
                        .Where(a => a.UserProfileID == userProfileId
                            && a.IsActive == true)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public CardInfo ToCardInfo()
        {
            CardInfo retVal = new CardInfo
            {
                NameOnCard = this.NameOnCard,
                CardNumber = this.CCLast4.ToString(),
                CardType = (Enums.CreditCardType)this.CardTypeID,
                ExpMonth = this.ExpMon,
                ExpYear = this.ExpYear
            };

            return retVal;
        }

        public override string ToString()
        {
            return Enums.GetEnumDescription((Enums.CreditCardType)this.CardTypeID) + " " + this.CCLast4;
        }
    }
}
