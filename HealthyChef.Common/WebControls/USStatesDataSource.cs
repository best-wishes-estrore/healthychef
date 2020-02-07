using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthyChef.Common
{
    [ToolboxData("<{0}:USStatesDataSource runat=server></{0}:USStatesDataSource>")]
    public class USStatesDataSource : DataSourceControl
    {
        protected override DataSourceView GetView(string viewName)
        {
            return new USStatesDataSourceView(this, viewName);
        }
    }

    public class USStatesDataSourceView : DataSourceView
    {
        public USStatesDataSourceView(IDataSource parent, string viewname)
            : base(parent, viewname)
        {

        }


        protected override System.Collections.IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            return Helpers.US_States;
        }
    }
}
