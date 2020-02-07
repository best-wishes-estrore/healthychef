using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Classes;
using System.Collections.Generic;
using System.Linq;

namespace HealthyChef.WebModules.Reports.Admin
{
    public class QueryAlcFamily : QueryAll
    {
        public QueryAlcFamily(healthychefEntities context, QueryDataObject data) : base(context, data)
        {

        }

        public override List<ActiveCustomerDto> Process(IQueryable<DAL.hccCart> query)
        {
            var processed = query
                  .SelectMany(c => c.hccCartItems,
                      (cart, item) => new { cart, item })
                  .Where(c => c.item.ItemTypeID == (int)Enums.CartItemType.AlaCarte && c.item.Plan_IsAutoRenew==true )
                  .Select(c => c.cart)
                  .Distinct();

            return base.Process(processed);
        }
    }
}