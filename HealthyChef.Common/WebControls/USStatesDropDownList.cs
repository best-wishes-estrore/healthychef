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
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:USStatesDropDownList runat=server />")]
    public class USStatesDropDownList : DropDownList
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack)
            {
                base.DataValueField = "Abbr";
                base.DataTextField = "Name";
                base.DataSource = new USStatesDataSource();
                base.DataBind();
            }
        }

        new protected string DataValueField
        {
            get { return base.DataValueField; }
            set { base.DataValueField = value; }
        }

        new protected string DataTextField
        {
            get { return base.DataTextField; }
            set { base.DataTextField = value; }
        }

        new protected object DataSource
        {
            get { return base.DataSource; }
            set { base.DataSource = value;}
        }

        new protected string DataSourceID
        {
            get { return base.DataSourceID; }
            set { base.DataSourceID = value; }
        }
    }
}
