using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class SearchParams
    {
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public Int64 purchaseNumber { get; set; }
        public string deliveryDate { get; set; }
        public string roles { get; set; }
        public int pagenumber { get; set; }
        public int pagesize { get; set; }
        public int totalrecords { get; set; }

        public Guid CurrentLoggedUserId { get; set; }
        public SearchParams()
        {
            this.lastName = this.email = this.phone = null;
            this.deliveryDate = null;
            this.roles = "Customer";
        }
    }

    public class SearchParamsForPurchases
    {
        public int StatusId { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string Purchasedate { get; set; }
        public string purchaseNumber { get; set; }
        public string deliveryDate { get; set; }
        public int pagenumber { get; set; }
        public int pagesize { get; set; }
        public int totalrecords { get; set; }
    }

    public class SearchParamsForOrderFulfillment
    {
        public string lastName { get; set; }
        public string email { get; set; }
        public string purchaseNumber { get; set; }
        public string deliveryDate { get; set; }
        public int ddlStatusId { get; set; }
        public int ddlTypesId { get; set; }
        public string ddlStatusValue { get; set; }
        public string ddlTypesValue { get; set; }
    }

    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public SqlParameter[] PaginationParams
        {
            get
            {
                return new SqlParameter[] { new SqlParameter("@PAGENUMBER", this.PageNumber), new SqlParameter("@PAGESIZE", this.PageSize), };
            }
        }

        public int Skip
        {
            get
            {
                return this.PageSize * (this.PageNumber - 1);
            }
        }

        public Pagination()
        {
            PageNumber = 1;
            PageSize = 50;
        }


        public Pagination(int _pageNumber, int _pageSize)
        {
            PageNumber = _pageNumber;
            PageSize = _pageSize;
        }

    }
}