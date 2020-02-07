using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class ProgramPlanAddToCart : FormControlBase
    {// primaryKey as hccCartId 
        protected override void OnInit(EventArgs e)
        {
            this.Page.MaintainScrollPositionOnPostBack = true;

            base.OnInit(e);

            ddlPrograms.SelectedIndexChanged += ddlPrograms_SelectedIndexChanged;
            ddlPlans.SelectedIndexChanged += DisplayDailyPrice;
            ddlOptions.SelectedIndexChanged += DisplayDailyPrice;
            btnSave.Click += base.SubmitButtonClick;
            btnCancel.Click += base.CancelButtonClick;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected override void LoadForm()
        {
            BindddlPrograms();
            BindddlStartDates();
            BindddlProfiles();
        }

        protected override void SaveForm()
        {
            try
            {
                hccCart userCart = hccCart.GetById(this.PrimaryKeyIndex);
                MembershipUser user = Membership.GetUser(userCart.AspNetUserID);

                if (user != null)
                {
                    hccProgramPlan selPlan = hccProgramPlan.GetById(int.Parse(ddlPlans.SelectedValue));

                    if (selPlan != null)
                    {
                        List<hccProgramOption> progOptions = hccProgramOption.GetBy(selPlan.ProgramID);
                        hccProgramOption option = progOptions.Where(a => a.ProgramOptionID == (int.Parse(ddlOptions.SelectedValue))).SingleOrDefault();
                        hccProgram prog = hccProgram.GetById(selPlan.ProgramID);

                        int numDays = selPlan.NumDaysPerWeek * selPlan.NumWeeks;
                        decimal dailyPrice = selPlan.PricePerDay + option.OptionValue;
                        decimal itemPrice = numDays * dailyPrice;
                        int profileId = 0;
                        //bool autoRenew = chkAutoRenew.Checked;
                        string itemFullName = string.Empty;
                        DateTime startDate = DateTime.Parse(ddlStartDates.SelectedItem.Text);

                        itemFullName = string.Format("{0} - {1} - {2} - {3}",
                            prog == null ? string.Empty : prog.Name, selPlan.Name, option.OptionText, startDate.ToShortDateString());

                        if (divProfiles.Visible)
                        {
                            profileId = int.Parse(ddlProfiles.SelectedValue);
                        }
                        else
                            profileId = hccUserProfile.GetParentProfileBy(userCart.AspNetUserID.Value).UserProfileID;

                        if (userCart != null && selPlan != null)
                        {
                            int currentQty = int.Parse(txtQuantity.Text.Trim());
                            hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, itemFullName, profileId);

                            if (existItem == null)
                            {
                                hccCartItem planItem = new hccCartItem
                                {
                                    CartID = userCart.CartID,
                                    CreatedBy = (Guid)Membership.GetUser().ProviderUserKey,
                                    CreatedDate = DateTime.Now,
                                    IsTaxable = selPlan.IsTaxEligible,
                                    ItemDesc = selPlan.Description,
                                    ItemName = itemFullName,
                                    ItemPrice = itemPrice,
                                    ItemTypeID = (int)Enums.CartItemType.DefinedPlan,
                                    Plan_IsAutoRenew = false, //autoRenew,
                                    Plan_PlanID = selPlan.PlanID,
                                    Plan_ProgramOptionID = option.ProgramOptionID,
                                    DeliveryDate = startDate,
                                    UserProfileID = profileId,
                                    Quantity = currentQty,
                                    IsCompleted = false
                                };      

                                planItem.GetOrderNumber(userCart);   
                                planItem.Save();

                                hccProductionCalendar cal;

                                for (int i = 0; i < selPlan.NumWeeks; i++)
                                {
                                    cal = hccProductionCalendar.GetBy(planItem.DeliveryDate.AddDays(7 * i));

                                    if (cal != null)
                                    {
                                        hccCartItemCalendar cartCal = new hccCartItemCalendar { CalendarID = cal.CalendarID, CartItemID = planItem.CartItemID, IsFulfilled = false };
                                        cartCal.Save();
                                    }
                                    else
                                        BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(
                                            "No production calendar found for Delivery Date: " + planItem.DeliveryDate.AddDays(7 * i).ToShortDateString(), this);
                                }
                                

                                if (cbxRecurring.Checked)
                                {
                                    var cartItemsRecurring = hccCartItem.GetBy(userCart.CartID);
                                    var filter = cartItemsRecurring.Where(ci => ci.ItemType == Enums.CartItemType.DefinedPlan);
                                    var roItem = new hccRecurringOrder();
                                    roItem.CartID = planItem.CartID;
                                    roItem.CartItemID = planItem.CartItemID;
                                    roItem.UserProfileID = planItem.UserProfileID;
                                    roItem.AspNetUserID = userCart.AspNetUserID;
                                    roItem.PurchaseNumber = userCart.PurchaseNumber;
                                    roItem.TotalAmount = Math.Round(Convert.ToDecimal(Convert.ToDouble(planItem.ItemPrice) - Convert.ToDouble(planItem.ItemPrice) * 0.05), 2);
                                    roItem.Save();
                                    if(planItem!=null)

                                    {
                                        planItem.Plan_IsAutoRenew = true;
                                        planItem.Save();
                                    }

                                        //foreach (var recurringOrder in filter.Select(item => new hccRecurringOrder
                                        //{
                                        //    CartID = item.CartID,
                                        //    CartItemID = item.CartItemID,
                                        //    UserProfileID = item.UserProfileID,
                                        //    AspNetUserID = userCart.AspNetUserID,
                                        //    PurchaseNumber = userCart.PurchaseNumber,
                                        //    TotalAmount = userCart.TotalAmount
                                        //}))
                                        //{
                                        //    recurringOrder.Save();
                                        //}
                                }
                                OnSaved(new ControlSavedEventArgs(planItem.CartItemID));
                                cbxRecurring.Checked = false;
                            }
                            else
                            {
                                if (existItem.AdjustQuantity(existItem.Quantity + currentQty))
                                    OnSaved(new ControlSavedEventArgs(existItem.CartItemID));
                                cbxRecurring.Checked = false;
                            }
                        }
                    }
                }
                //Page.Response.Redirect(Page.Request.Url.ToString()+ "#tabs9", true);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            ddlPrograms.ClearSelection();
            ddlPlans.ClearSelection();
            ddlPlans.Enabled = false;
            ddlOptions.ClearSelection();
            ddlOptions.Enabled = false;
            ddlStartDates.ClearSelection();
            //chkAutoRenew.Checked = false;
            ddlProfiles.ClearSelection();
            divProfiles.Visible = false;
            txtQuantity.Text = "1";
        }

        void BindddlPrograms()
        {
            if (ddlPrograms.Items.Count == 0)
            {
                ddlPrograms.DataSource = hccProgram.GetBy(true);
                ddlPrograms.DataTextField = "Name";
                ddlPrograms.DataValueField = "ProgramID";
                ddlPrograms.DataBind();

                ddlPrograms.Items.Insert(0, new ListItem("Select a Program...", "-1"));
                ddlPlans.Items.Insert(0, new ListItem("Select a Plan...", "-1"));
                ddlOptions.Items.Insert(0, new ListItem("Select Options...", "-1"));
            }
        }

        void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            int programId = int.Parse(ddlPrograms.SelectedValue);

            if (programId > 0)
            {
                BindddlPlans(programId);
                BindddlOptions(programId);
            }
            else
            {
                ddlPlans.Enabled = false;
                ddlOptions.Enabled = false;
            }
        }

        void BindddlPlans(int programId)
        {
            ddlPlans.ClearSelection();
            ddlPlans.Items.Clear();

            ddlPlans.DataSource = hccProgramPlan.GetBy(programId, true);
            ddlPlans.DataTextField = "NameAndPrice";
            ddlPlans.DataValueField = "PlanID";
            ddlPlans.DataBind();

            ddlPlans.Items.Insert(0, new ListItem("Select a Plan...", "-1"));
            ddlPlans.Enabled = true;
        }

        void DisplayDailyPrice(object sender, EventArgs e)
        {
            if (ddlPlans.SelectedIndex > 0 && ddlOptions.SelectedIndex > 0)
            {
                hccProgramPlan plan = hccProgramPlan.GetById(int.Parse(ddlPlans.SelectedValue));
                hccProgram program = hccProgram.GetById(int.Parse(ddlPrograms.SelectedValue));
                hccProgramOption option = hccProgramOption.GetBy(program.ProgramID).SingleOrDefault(a => a.ProgramOptionID == int.Parse(ddlOptions.SelectedValue));

                if (plan != null && option != null)
                {
                    int numDays = plan.NumDaysPerWeek * plan.NumWeeks;
                    decimal dailyPrice = plan.PricePerDay + option.OptionValue;
                    decimal itemPrice = numDays * dailyPrice;

                    lblPlanPrice.Text = string.Format("Daily Price: {0}; Total Price: {1}", dailyPrice.ToString("c"), itemPrice.ToString("c"));
                }
                else
                    lblPlanPrice.Text = string.Empty;
            }
            else
                lblPlanPrice.Text = string.Empty;
        }

        void BindddlOptions(int programId)
        {
            ddlOptions.ClearSelection();
            ddlOptions.Items.Clear();

            hccProgram prog = hccProgram.GetById(programId);

            ddlOptions.DataSource = hccProgramOption.GetBy(prog.ProgramID).Where(a => !string.IsNullOrWhiteSpace(a.OptionText)).OrderBy(a => a.OptionIndex).ToList();
            ddlOptions.DataTextField = "FullText";
            ddlOptions.DataValueField = "ProgramOptionID";
            ddlOptions.DataBind();

            ddlOptions.Items.Insert(0, new ListItem("Select Options...", "-1"));
            ddlOptions.Enabled = true;
        }

        void BindddlStartDates()
        {
            if (ddlStartDates.Items.Count == 0)
            {
                ddlStartDates.DataSource = hccProductionCalendar.GetNext4Last2Calendars();
                ddlStartDates.DataTextField = "DeliveryDate";
                ddlStartDates.DataTextFormatString = "{0:MM/dd/yyyy}";
                ddlStartDates.DataValueField = "CalendarID";
                ddlStartDates.DataBind();

                ddlStartDates.Items.Insert(0, new ListItem("Select Start Date...", "-1"));
            }
        }

        void BindddlProfiles()
        {
            // get all profiles under the CartUserASPNetId
            hccCart userCart = hccCart.GetById(this.PrimaryKeyIndex);
            List<hccUserProfile> memberProfiles = hccUserProfile.GetBy(userCart.AspNetUserID.Value, true);

            if (memberProfiles.Count > 1)
            {
                ddlProfiles.DataSource = memberProfiles;
                ddlProfiles.DataTextField = "ProfileName";
                ddlProfiles.DataValueField = "UserProfileID";
                ddlProfiles.DataBind();

                ddlProfiles.Items.Insert(0, new ListItem("Select a profile...", "-1"));
                divProfiles.Visible = true;

                rfvProfiles.ValidationGroup = this.ValidationGroup;
            }
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }
    }
}