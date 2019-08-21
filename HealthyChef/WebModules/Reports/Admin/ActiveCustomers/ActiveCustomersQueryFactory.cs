using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthyChef.WebModules.Reports.Admin
{
    public static class ActiveCustomersQueryFactory
    {
        public static List<ActiveCustomerDto> CreateQuery(DateTime startDate, DateTime endDate, int programId)
        {
           using(healthychefEntities context = new healthychefEntities())
           {
               context.CommandTimeout = 600;

               QueryDataObject dataObject = new QueryDataObject();
               dataObject.StartTime = startDate;
               dataObject.EndTime = endDate;
               dataObject.ProgramId = programId;

               var query = context.hccCarts
                   .Where(c => c.StatusID == (int)Enums.CartStatus.Paid);

               if (programId == -3)
               {
                   return new QueryGC(context, dataObject).Process(query);
               }
               else if (programId == -2)
               {
                   return new QueryAlc(context, dataObject).Process(query);
               }
               else if (programId == -1)
               {
                   return new QueryAll(context, dataObject).Process(query);
               }
               else if (programId == -4)
               {
                    return new QueryAlcFamily(context, dataObject).Process(query);
                }
                else
               {
                   return new QueryType(context, dataObject).Process(query);
               }
           }
        }
    }
}