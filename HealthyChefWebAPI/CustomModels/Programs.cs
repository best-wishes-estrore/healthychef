using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Programs
    {
        public int ProgramID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public int MoreInfoNavID { get; set; }
        public string ImagePath { get; set; }

    }


    public class ProgramItem : Helpers.PostHttpResponse
    {
        public int ProgramID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public int MoreInfoNavID { get; set; }
        public string ImagePath { get; set; }
    }

}