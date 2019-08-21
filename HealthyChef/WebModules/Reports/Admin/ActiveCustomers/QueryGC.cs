using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Classes;
using System.Collections.Generic;
using System.Linq;

namespace HealthyChef.WebModules.Reports.Admin
{
    public class QueryGC : QueryAll
    {
        public QueryGC(healthychefEntities context, QueryDataObject data)
            : base(context, data)
        {
        }

        public override List<ActiveCustomerDto> Process(IQueryable<DAL.hccCart> query)
        {
            var processed = query
                  .SelectMany(c => c.hccCartItems,
                      (cart, item) => new { cart, item })
                  .Where(c => c.item.ItemTypeID == (int)Enums.CartItemType.GiftCard)
                  .Select(c => c.cart)
                  .Distinct();
           return base.Process(processed);
        }
    }
}