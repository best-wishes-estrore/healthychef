using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthyChef.WebModules.Components.HealthyChef
{
    public partial class SliderControl : System.Web.UI.UserControl
    {
        [PersistenceMode(PersistenceMode.Attribute)]
        public int MinValue { get; set; }

        [PersistenceMode(PersistenceMode.Attribute)]
        public int MaxValue { get; set; }

        [PersistenceMode(PersistenceMode.Attribute)]
        public string SliderLabel { get; set; }

        [PersistenceMode(PersistenceMode.Attribute)]
        public string CurrentValue
        {
            get { return currentValue.Value; }
            set { currentValue.Value = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                sliderValueLabel.Text = SliderLabel;
            }
        }
    }
}