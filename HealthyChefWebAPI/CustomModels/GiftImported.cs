using HealthyChefWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class GiftImported
    {
       public int ImportId { get; set; }
       public long Code { get; set; }
       public string Amount { get; set; }
       public string DateAdded { get; set; }
       public string DateExpires { get; set; }
    }
    public class ImportedGiftCertificate
    {
        public int ImportedGiftcertId { get; set; }
        public int Code { get; set; }
        public decimal Amount { get; set; }
        public bool Redeemed { get; set; }
        public string DateUsed { get; set; }
    }

    public class ImportedGiftCertificateResult : PostHttpResponse
    {
        public int ImportedGiftcertId { get; set; }
        public int Code { get; set; }
        public decimal Amount { get; set; }
        public bool Redeemed { get; set; }
        public string DateUsed { get; set; }
    }
}