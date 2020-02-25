using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Reflection;
using AuthorizeNet;
using System.Web.Configuration;

namespace HealthyChef.DAL
{
    public partial class hccAddress
    {
        //static healthychefEntities cont
        //{
        //    get
        //    {
        //        if (System.Web.HttpContext.Current != null)
        //            return healthychefEntities.Default;
        //        else
        //            return new healthychefEntities(WebConfigurationManager.ConnectionStrings["healthychefEntities"].ConnectionString);
        //    }
        //}

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Address1)
                    && !string.IsNullOrWhiteSpace(this.City)
                    && !string.IsNullOrWhiteSpace(this.State)
                    && !string.IsNullOrWhiteSpace(this.PostalCode);
            }
        }

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccAddresses", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccAddresses", this);
                    }
                    else
                    {
                        cont.hccAddresses.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccAddresses", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.hccAddresses.DeleteObject((hccAddress)oldObj);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public static hccAddress GetById(int addressId)
        {
            try
            {

                using (var cont = new healthychefEntities())
                {
                    return cont.hccAddresses.SingleOrDefault(a => a.AddressID == addressId);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public HealthyChef.Common.Enums.AddressType GetAddressType()
        {
            try
            {


                return (HealthyChef.Common.Enums.AddressType)this.AddressTypeID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public AuthorizeNet.Address ToAuthNetAddress()
        {
            AuthorizeNet.Address retAddr = new Address
            {
                City = this.City,
                Country = this.Country,
                First = this.FirstName,
                Last = this.LastName,
                Phone = this.Phone,
                State = this.State,
                Street = this.Address1 + (string.IsNullOrWhiteSpace(this.Address2) ? string.Empty : " " + this.Address2),
                Zip = this.PostalCode,
                Fax = string.Empty
               
            };

            return retAddr;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(string.Format("{0} {1}<br>", this.FirstName, this.LastName));
            sb.Append(string.Format("{0}<br>", this.Address1));

            if (!string.IsNullOrEmpty(this.Address2))
                sb.Append(string.Format("{0}<br>", this.Address2));

            sb.Append(string.Format("{0}, {1} {2}<br>", this.City, this.State, this.PostalCode));

            if (!string.IsNullOrEmpty(this.Phone))
                sb.Append(string.Format("{0}<br>", this.Phone));

            return sb.ToString();
        }

        public string ToHtml()
        {
            if (this != null)
            {
                try
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append("<table class='address'>");
                    sb.AppendFormat("<tr><td>{0}</td></tr>", this.FirstName + " " + this.LastName);

                    sb.AppendFormat("<tr><td>{0}</td></tr>", this.Address1);

                    if (!String.IsNullOrEmpty(this.Address2))
                        sb.AppendFormat("<tr><td>{0}</td></tr>", this.Address2);

                    sb.AppendFormat("<tr><td>{0}, {1}  {2} </td></tr>", this.City, this.State, this.PostalCode);

                    if (!String.IsNullOrEmpty(this.Phone))
                        sb.AppendFormat("<tr><td>Phone: {0}</td></tr>", this.Phone);

                    sb.Append("</table>");

                    return sb.ToString();
                }
                catch { return "Address could not be formatted."; }
            }
            else
                return string.Empty;
        }

        public static bool operator ==(hccAddress a, hccAddress b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.Address1 == b.Address1
                && a.AddressTypeID == b.AddressTypeID
                && a.City == b.City
                && a.FirstName == b.FirstName
                && a.LastName == b.LastName
                && a.State == b.State
                && a.PostalCode == b.PostalCode
                && a.Country == b.Country;
        }

        public static bool operator !=(hccAddress a, hccAddress b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public int OrderMinimum { get; set; }
    }
}
