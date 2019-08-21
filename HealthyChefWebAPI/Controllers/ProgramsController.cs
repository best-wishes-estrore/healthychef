using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HealthyChefWebAPI.Controllers
{
    //[Authorize(Roles = "Administrators")]
    public class ProgramsController : ApiController
    {
        public List<hccProgram> Index()
        {
            List<hccProgram> programs = hccProgram.GetBy(true).Where(p => p.DisplayOnWebsite).ToList();

            return programs;
        }
    }
}
