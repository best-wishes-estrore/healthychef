using HealthyChef.DAL;
using HealthyChef.DAL.Classes;
using System.Collections.Generic;
using System.Linq;

namespace HealthyChef.WebModules.Reports.Admin
{
    public class QueryAll
    {
        public QueryAll(healthychefEntities context, QueryDataObject data)
        {
            Context = context;
            Data = data;
        }

        public QueryDataObject Data
        {
            get;
            set;
        }

        public healthychefEntities Context
        {

            get;
            set;
        }

        public virtual List<ActiveCustomerDto> Process(IQueryable<DAL.hccCart> query)
        {
            return query.GroupJoin(Context.hccUserProfiles,
                      cart => cart.AspNetUserID,
                      profile => profile.MembershipID,
                      (c, p) => new { cart = c, profile = p.DefaultIfEmpty() })
                  .SelectMany(g => g.profile.Select(profile => new { cart = g.cart, profile }).Where(x => x.profile.IsActive))
                  .Join(Context.aspnet_Membership,
                      cart => cart.cart.AspNetUserID,
                      member => member.UserId,
                      (c, m) => new { cart = c.cart, profile = c.profile, member = m })
                  .GroupBy(u => new { u.member.UserId })
                  .Select(u => new
                  {
                      FirstPurchaseDate = Context.hccCarts.Where(c => u.Select(y => y.cart.AspNetUserID).Contains(c.AspNetUserID)).Min(c => c.PurchaseDate),
                      LastPurchase = u.FirstOrDefault(x => x.cart.PurchaseDate == u.Max(c => c.cart.PurchaseDate)), 
                      TotalOrders = u.OrderByDescending(x => x.cart.CartID).Count(),
                      //TotalAmount = u.OrderByDescending(x => x.cart.)
                  })
                  .Where(u =>
                      u.LastPurchase.cart.PurchaseDate >= Data.StartTime
                      && u.LastPurchase.cart.PurchaseDate <= Data.EndTime
                      && u.LastPurchase.member.IsApproved)
                  .Select(u => new ActiveCustomerDto
                  {
                      Email = u.LastPurchase.member.Email,
                      CustomerName = u.LastPurchase.profile.LastName + ", " + u.LastPurchase.profile.FirstName,
                      Address = u.LastPurchase.profile.hccAddressShipping.Address1 + (!string.IsNullOrEmpty(u.LastPurchase.profile.hccAddressShipping.Address2) ? ", " + u.LastPurchase.profile.hccAddressShipping.Address2 : ""),
                      ZipCode = u.LastPurchase.profile.hccAddressShipping.PostalCode,
                      PhoneNumber = u.LastPurchase.profile.hccAddressShipping.Phone,
                      TotalOrders = u.TotalOrders,
                      //TotalAmount = u.TotalAmount,
                      FirstPurchaseDate = u.FirstPurchaseDate,
                      LastPurchaseDate = u.LastPurchase.cart.PurchaseDate,
                      LastPurchaseAmount = u.LastPurchase.cart.TotalAmount,
                      LastPurchaseId = u.LastPurchase.cart.PurchaseNumber,
                      MktgOptIn = (u.LastPurchase.profile.CanyonRanchCustomer ?? false) ? "Y" : "N",                      

                  })                  
                  .OrderBy(c => c.CustomerName)
                  .ToList();
        }


        //public virtual List<ActiveCustomerDto> Process(IQueryable<DAL.hccCart> query)
        //{
        //    // var catQuery = Context.hccUserProfiles;
        //    //// return query.Select(c => new { c.CartID, c.AspNetUserID, c.PurchaseDate, c.PurchaseNumber, c.TotalAmount }).Distinct();
        //    // return catQuery.GroupJoin(Context.hccCarts,
        //    //         profile => profile.MembershipID,
        //    //         cart => cart.AspNetUserID,
        //    //         (p, c) => new { p, c })
        //    //         .SelectMany(x => x.c.DefaultIfEmpty(), (x, c) => new { profile = x.p, cart = c }).Where(x => x.profile.IsActive)
        //    //.SelectMany(g => g.profile.Select(profile => new { cart = g.cart, profile }).Where(x => x.profile.IsActive))
        //    //.SelectMany(g => g.cart.DefaultIfEmpty()cart => new { cart = g.cart, profile }).Where(x => x.profile.IsActive)

        //    //parents.GroupJoin(children, p => p.Id, c => c.Id, (p, c) => new { p, c })
        //    //.SelectMany(x => x.c.DefaultIfEmpty(), (x, c) => new { x.p.Value, c?.ChildValue })
        //    query = Context.hccCarts;
        //    return query.Select(c => new { c.CartID, c.AspNetUserID, c.PurchaseDate, c.PurchaseNumber, c.TotalAmount }).Distinct().GroupJoin(Context.hccUserProfiles,
        //              cart => cart.AspNetUserID,
        //              profile => profile.MembershipID,
        //              (c, p) => new { cart = c, profile = p })
        //          .SelectMany(g => g.profile.Select(profile => new { cart = g.cart, profile }).Where(x => x.profile.IsActive)                  
        //    .Join(Context.aspnet_Membership,
        //        cart => cart.cart.AspNetUserID,
        //        member => member.UserId,
        //        (c, m) => new { cart = c.cart, profile = c.profile, member = m })
        //    .Where(m => m.member.IsApproved)
        //    .GroupBy(u => new { u.member.UserId })    
        //  .Select(u => new
        //  {
        //      Email = u.Select(p => p.profile.aspnet_Membership.Email).FirstOrDefault(),
        //      CustomerName = u.Select(p => p.profile.LastName.Trim() + "," + p.profile.FirstName.Trim()).FirstOrDefault(),
        //      Address = u.Select(p => p.profile.hccAddressShipping.Address1.Trim()).FirstOrDefault(),
        //      ZipCode = u.Select(p => p.profile.hccAddressShipping.PostalCode.Trim()).FirstOrDefault(),
        //      PhoneNumber = u.Select(p => p.profile.hccAddressShipping.Phone.Trim()).FirstOrDefault(),

        //      LastPurchaseDate = u.OrderByDescending(c => c.cart.CartID).Select(c => c.cart.PurchaseDate).FirstOrDefault(),
        //      FirstPurchaseDate = u.OrderBy(c => c.cart.CartID).Select(c => c.cart.PurchaseDate).FirstOrDefault(),
        //      LastPurchaseAmount = u.OrderByDescending(c => c.cart.CartID).Select(c => c.cart.TotalAmount).FirstOrDefault(),
        //      LastPurchaseId = u.OrderByDescending(c => c.cart.CartID).Select(c => c.cart.PurchaseNumber).FirstOrDefault(),
        //      TotalAmount = u.OrderByDescending(c => c.cart.CartID).Select(c => c.cart.TotalAmount).Sum(),
        //      TotalOrders = u.OrderByDescending(c => c.cart.CartID).Count(),

        //      AccountPreferences = "Test Data",
        //      AccountAllergens = "Test Data",
        //      AccountCustomPreferences = "Test Data",
        //      ProductType = "Test Data",
        //  })

        //          .Select(u => new ActiveCustomerDto
        //          {
        //              //Context.hccCarts.Where(c => u.Select(y => y.cart.AspNetUserID).Contains(c.AspNetUserID)).Min(c => c.PurchaseDate),
                      
        //              Email = u.Email,
        //              CustomerName = u.CustomerName,
        //              Address = u.Address,
        //              ZipCode = u.ZipCode,
        //              PhoneNumber = u.PhoneNumber,
        //              FirstPurchaseDate = u.FirstPurchaseDate,
        //              LastPurchaseDate = u.LastPurchaseDate,
        //              LastPurchaseId = u.LastPurchaseId,
        //              TotalOrders = u.TotalOrders,
        //              TotalAmount = u.TotalAmount,
        //              LastPurchaseAmount = u.LastPurchaseAmount,
        //              AccountPreferences = u.AccountPreferences,
        //              AccountAllergens = u.AccountAllergens,
        //              AccountCustomPreferences = u.AccountCustomPreferences,
        //              ProductType = u.ProductType,
                      
        //              //Address = u.LastPurchase.profile.hccAddressShipping.Address1 + (!string.IsNullOrEmpty(u.LastPurchase.profile.hccAddressShipping.Address2) ? ", " + u.LastPurchase.profile.hccAddressShipping.Address2 : ""),
        //              //ZipCode = u.LastPurchase.profile.hccAddressShipping.PostalCode,
        //              //PhoneNumber = u.LastPurchase.profile.hccAddressShipping.Phone,
        //              //FirstPurchaseDate = u.FirstPurchaseDate.cart.PurchaseDate,
        //              //LastPurchaseDate = u.LastPurchase.cart.PurchaseDate,
        //              //LastPurchaseAmount = u.LastPurchase.cart.TotalAmount,
        //              //LastPurchaseId = u.LastPurchase.cart.PurchaseNumber,
        //              //TotalOrders =  u.TotalOrders,
        //              //MktgOptIn = (u.LastPurchase.profile.CanyonRanchCustomer ?? false) ? "Y" : "N",
        //              //TotalAmount = u.TotalAmount,
        //              //AccountPreferences = u.AccountPreferences.FirstOrDefault(),
        //              //AccountAllergens = u.AccountAllergens.FirstOrDefault()
        //          })
                  
        //          .Where(u=>u.FirstPurchaseDate>=Data.StartTime && u.LastPurchaseDate<=Data.EndTime)

        //        )
        //          .OrderBy(c => c.CustomerName)
        //          .ToList();
        //}

        //public List<ActiveCustomerDto> CartTotalAmount(IQueryable<DAL.hccCart> query)
        //{
        //    return query.GroupJoin(Context.hccUserProfiles,
        //                cart => cart.AspNetUserID,
        //                profile => profile.MembershipID,                        
        //                (c, p) => new { cart = c, profile = p.DefaultIfEmpty() })
        //            .SelectMany(g => g.profile.Select(profile => new { cart = g.cart, profile }).Where(x => x.profile.IsActive))
        //            .Join(Context.aspnet_Membership,
        //                cart => cart.cart.AspNetUserID,
        //                member => member.UserId,
        //                (c, m) => new { cart = c.cart, profile = c.profile, member = m })
        //            .GroupBy(u => new { u.member.UserId })
        //            .Select(u => new
        //            {
        //                FirstPurchaseDate = Context.hccCarts.Where(c => u.Select(y => y.cart.AspNetUserID).Contains(c.AspNetUserID)).Min(c => c.PurchaseDate),
        //                LastPurchase = u.FirstOrDefault(x => x.cart.PurchaseDate == u.Max(c => c.cart.PurchaseDate)),
        //                TotalAmount = Context.hccCarts.Where(c => u.Select(y => y.cart.AspNetUserID).Contains(c.AspNetUserID) && c.StatusID == 20).Sum(c => c.TotalAmount),

        //            })
        //            .Where(u => u.LastPurchase.member.IsApproved)
        //            .Select(u => new ActiveCustomerDto
        //            {
        //                //Email = u.LastPurchase.member.Email,
        //                //CustomerName = u.LastPurchase.profile.LastName + ", " + u.LastPurchase.profile.FirstName,
        //                //Address = u.LastPurchase.profile.hccAddressShipping.Address1 + (!string.IsNullOrEmpty(u.LastPurchase.profile.hccAddressShipping.Address2) ? ", " + u.LastPurchase.profile.hccAddressShipping.Address2 : ""),
        //                //ZipCode = u.LastPurchase.profile.hccAddressShipping.PostalCode,
        //                //PhoneNumber = u.LastPurchase.profile.hccAddressShipping.Phone,
        //                //FirstPurchaseDate = u.FirstPurchaseDate,
        //                //LastPurchaseDate = u.LastPurchase.cart.PurchaseDate,
        //                //LastPurchaseAmount = u.LastPurchase.cart.TotalAmount,
        //                LastPurchaseId = u.LastPurchase.cart.PurchaseNumber
        //                //MktgOptIn = (u.LastPurchase.profile.CanyonRanchCustomer ?? false) ? "Y" : "N",

        //                //TotalAmount = 0,
        //              //  ProductType = 

        //             //   AccountAllergens ="",
        //              //  AccountCustomPreferences="",
        //             //   AccountPreferences="",
        //            })
        //            .OrderBy(c => c.CustomerName)
        //            .ToList();
        //}
    }
}