using HealthyChef.DAL;
using HealthyChef.DAL.Classes;
using System.Collections.Generic;
using System.Linq;

namespace HealthyChef.WebModules.Reports.Admin
{
    public class QueryType: QueryAll
    {
        public QueryType(healthychefEntities context, QueryDataObject data)
            : base(context, data)
        {

        }

        public override List<ActiveCustomerDto> Process(IQueryable<DAL.hccCart> query)
        {
            var processed = query.SelectMany(c => c.hccCartItems, (cart, item) => new { cart, item }).Join(Context.hccProgramPlans, cart => cart.item.Plan_PlanID, plan => plan.PlanID, (cart, plan) => new { cart = cart.cart, item = cart.item, plan })
                .Where(m => m.plan.ProgramID == Data.ProgramId).Select(c => c.cart).Distinct();

            return base.Process(processed);
        }
    }
}