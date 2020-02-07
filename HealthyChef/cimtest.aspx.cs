using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AuthorizeNet;
using HealthyChef.AuthNet;

namespace HealthyChef
{
    public partial class cimtest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    CustomerInformationManager cim = new CustomerInformationManager();

                    Customer cust = cim.GetCustomer("9925034");

                    if (cust == null)
                        cust = cim.CreateCustomer("rcreecy@bayshoresolutions.com", "test user");
                    
                    Address addrShipping = new Address
                    {
                        City = "tampa",
                        Company = "some comp",
                        Country = "US",
                        First = "Dev",
                        Last = "Ryan",
                        Phone = "123-234-3456",
                        State = "FL",
                        Street = "600 Westshore Blvd",
                        Zip = "33609",
                        Fax = ""
                    };

                    string addShipResponse = cim.AddShippingAddress(cust, addrShipping);

                    if (cust != null && string.IsNullOrWhiteSpace(cust.ID))
                    {
                        cust.ID = "123456";
                        
                        cim.UpdateCustomer(cust);
                    }

                    string addCCResponse = cim.AddCreditCard(cust, "4111111111111111", 6, 2013, "123", addrShipping);               
                }
                catch (Exception)
                {
                    
                    throw;
                }
               

            }
        }
    }
}