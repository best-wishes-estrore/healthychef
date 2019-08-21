using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public class ShippingLabelDetailsComparer : IEqualityComparer<ShippingLabelDetails>
    {
        public bool Equals(ShippingLabelDetails x, ShippingLabelDetails y)
        {
            if (x.PurchaseNum == y.PurchaseNum && x.OrderNumber == y.OrderNumber &&
                x.Address1 == y.Address1 && x.Address2 == y.Address2 &&
                x.City == y.City && x.Company == y.Company && x.Contact == y.Contact &&
                x.Country == y.Country  &&
                x.Email == y.Email && x.Phone == y.Phone && x.StateProvince == y.StateProvince && x.Zip == y.Zip)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(ShippingLabelDetails obj)
        {
            return 1;
        }
    }
}
