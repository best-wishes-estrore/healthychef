using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class ProductionCalendar_Edit : HealthyChef.Common.FormControlBase
    {
        private hccProductionCalendar CurrentProductionCalendar { get; set; }
        

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            txtOrderDeliveryDate.TextChanged += new EventHandler(txtOrderDeliveryDate_TextChanged);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
            //btnRetire.Click += new EventHandler(btnRetire_Click);
        }
                
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDdlMenus();
                //SetButtons();
            }
        }

        protected override void LoadForm()
        {
            //PrimaryKeyIndex = Convert.ToInt32(CurrentCalendarId.Value);
            CurrentProductionCalendar = hccProductionCalendar.GetById(PrimaryKeyIndex);

            if (CurrentProductionCalendar != null)
            {
                BindDdlMenus();
                txtCalendarName.Text = CurrentProductionCalendar.Name;
                ddlMenus.SelectedIndex = ddlMenus.Items.IndexOf(ddlMenus.Items.FindByValue(CurrentProductionCalendar.MenuID.ToString()));
                txtOrderCutOffDate.Text = CurrentProductionCalendar.OrderCutOffDate.ToString("MM/dd/yyyy");
                txtOrderDeliveryDate.Text = CurrentProductionCalendar.DeliveryDate.ToString("MM/dd/yyyy");
                txtDescription.Text = CurrentProductionCalendar.Description;
            }
        }

        protected override void SaveForm()
        {
            CurrentProductionCalendar = hccProductionCalendar.GetById(PrimaryKeyIndex);

            if (CurrentProductionCalendar == null)
                CurrentProductionCalendar = new hccProductionCalendar();

            CurrentProductionCalendar.Name = txtCalendarName.Text.Trim();
            CurrentProductionCalendar.Description = txtDescription.Text.Trim();
            CurrentProductionCalendar.DeliveryDate = DateTime.Parse(txtOrderDeliveryDate.Text.Trim());           
            CurrentProductionCalendar.OrderCutOffDate = DateTime.Parse(txtOrderCutOffDate.Text.Trim());
            CurrentProductionCalendar.MenuID = int.Parse(ddlMenus.SelectedValue);

           int calId = CurrentProductionCalendar.Save();

           OnSaved(new ControlSavedEventArgs(calId));
        }
       
        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtCalendarName.Text = string.Empty;
            ddlMenus.ClearSelection();
            txtOrderDeliveryDate.Text = string.Empty;
            txtOrderCutOffDate.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }

        void BindDdlMenus()
        {
            if (ddlMenus.Items.Count == 0)
            {
                ddlMenus.DataSource = hccMenu.GetAll();
                ddlMenus.DataTextField = "Name";
                ddlMenus.DataValueField = "MenuID";
                ddlMenus.DataBind();

                ddlMenus.Items.Insert(0, new ListItem("Select a menu...", "-1"));
            }
        }
        
        void txtOrderDeliveryDate_TextChanged(object sender, EventArgs e)
        {
            DateTime delDate = DateTime.Parse(txtOrderDeliveryDate.Text);
            DateTime cutOffDate = delDate.AddDays(-8);
            txtOrderCutOffDate.Text = cutOffDate.ToShortDateString();
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        protected void cstName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            hccProductionCalendar existCal = hccProductionCalendar.GetBy(txtCalendarName.Text.Trim());

            if (existCal != null && existCal.CalendarID != this.PrimaryKeyIndex)
                args.IsValid = false;
        }
    }
}