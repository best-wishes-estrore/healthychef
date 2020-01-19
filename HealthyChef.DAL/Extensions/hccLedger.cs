using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Permissions;
using System.Xml.Linq;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccLedger
    {
        public Enums.LedgerTransactionType TransactionType
        {
            get { return (Enums.LedgerTransactionType)this.TransactionTypeID; }
        }

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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccLedgers", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccLedgers", this);
                    }
                    else
                    {
                        cont.hccLedgers.AddObject(this);
                    }

                    cont.SaveChanges();
                    //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, this);
                }
            }
            catch { throw; }
        }

        public static List<hccLedger> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccLedgers.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static List<hccLedger> GetByMembershipID(Guid aspNetUserId, int? daysBack)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    if (daysBack.HasValue)
                    {
                        DateTime startDate = DateTime.Now.Subtract(new TimeSpan(daysBack.Value, 0, 0, 0));

                        return cont.hccLedgers
                            .Where(a => a.AspNetUserID == aspNetUserId
                                && a.CreatedDate > startDate)
                            .OrderByDescending(a => a.CreatedDate)
                            .ToList();
                    }
                    else
                    {
                        return cont.hccLedgers
                            .Where(a => a.AspNetUserID == aspNetUserId)
                            .OrderByDescending(a => a.CreatedDate)
                            .ToList();
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static hccLedger GetById(int ledgerId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccLedgers.SingleOrDefault(a => a.LedgerID == ledgerId);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static hccLedger GetBy(int cartId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccLedgers.OrderByDescending(f => f.CreatedDate).Where(a => a.AsscCartID == cartId
                          && (a.TransactionTypeID == (int)Enums.LedgerTransactionType.Purchase))
                        .FirstOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<BalanceReportItem> GetBalanceReportItems(DateTime reportDate)
        {
            Guid x = new Guid();
            try
            {
                List<BalanceReportItem> items = new List<BalanceReportItem>();
                List<hccLedger> ledgers = new List<hccLedger>();

                var rt = GetAll().Where(a => a.CreatedDate <= reportDate.AddDays(1))
                    //&& (a.PostBalance > 0.00m || a.PostBalance < 0.00m))
                    .OrderByDescending(a => a.CreatedDate)
                    .GroupBy(a => a.AspNetUserID);

                rt.ToList().ForEach(delegate (IGrouping<Guid, hccLedger> group)
                {
                    BalanceReportItem bri = new BalanceReportItem
                    {
                        MembershipID = group.Key,
                        BalanceAsOfDate = group.First().PostBalance
                    };

                    x = group.Key;
                    hccUserProfile user = hccUserProfile.GetParentProfileBy(group.Key);
                    if (user != null)
                    {
                        bri.CustomerName = user.FullName;
                        bri.CustomerEmail = user.ASPUser.Email;
                    }

                    hccLedger last = hccLedger.GetByMembershipID(group.Key, null)
                        .FirstOrDefault(); //a => a.TransactionType == Enums.LedgerTransactionType.Purchase);

                    if (last != null)
                    {
                        bri.LastPurchaseDate = last.CreatedDate;
                        bri.LastPurchaseAmount = last.PaymentDue;
                    }

                    items.Add(bri);
                });

                return items.OrderBy(a => a.CustomerName).ToList();
            }
            catch (Exception ex)
            {
                var y = x;
                throw;
            }
        }
    }

    public class BalanceReportItem
    {
        public Guid MembershipID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public decimal BalanceAsOfDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal? LastPurchaseAmount { get; set; }
    }
}
