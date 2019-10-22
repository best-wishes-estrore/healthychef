using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class HeaderViewModel
    {
        public List<ProgramViewModel> programs { get; set; }

        public List<SiteMapNode> mySiteMap
        {
            get
            {
                return _getSiteMapNodes();
            }
        }

        public int CartCount { get; internal set; }

        public HeaderViewModel()
        {
            //var _programs = hccProgram.GetBy(true).Where(p => p.DisplayOnWebsite).ToList();

            //this.programs = new List<ProgramViewModel>();

            //foreach (var _p in _programs)
            //{
            //    this.programs.Add(new ProgramViewModel()
            //    {
            //        ProgramID = _p.ProgramID,
            //        Name = _p.Name,
            //        ImagePath = _p.ImagePath,
            //        Description = _p.Description,
            //        MoreInfoNavID = _p.MoreInfoNavID ?? 0
            //    });
            //}
        }

        List<SiteMapNode> _getSiteMapNodes()
        {
            var _SiteMapNodes = new List<SiteMapNode>();
            int myCounter = 0;

            var _siteMapObj = new BayshoreSolutions.WebModules.SiteMapProvider();

            // to set displayMode = visible
            NameValueCollection collection = new NameValueCollection();
            collection.Add("displayMode", "Visible");
            _siteMapObj.Initialize("WebModulesSiteMapProvider", collection);

            _siteMapObj.BuildSiteMap();
            var _topNavDataSource = _siteMapObj.RootNode;

            if (_topNavDataSource.HasChildNodes)
            {
                foreach (SiteMapNode myNode in _topNavDataSource.ChildNodes)
                {
                    //if(myNode.Title == "Everyday Meal Plans")
                    //{
                    //    myNode.ChildNodes[0].Url = "~/meal-programs/healthy-living.aspx";
                    //}
                    if (myCounter < 5)
                    {
                        _SiteMapNodes.Add(myNode);
                        myCounter++;
                    }
                }

               // _SiteMapNodes.Reverse();
                
            }
            else
            {
                _SiteMapNodes = new List<SiteMapNode>();
            }

            return _SiteMapNodes;
        }
    }
}