using System;
using System.Data.Entity;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HealthyChef.DAL;
using HealthyChef.Common;

using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Security;
using Microsoft.VisualBasic;

namespace HealthyChef.WebModules.ShoppingCart
{
    public partial class ProgramsDisplay : System.Web.UI.UserControl
    {
        public Guid CartUserASPNetId
        {
            get
            {
                if (ViewState["CartUserASPNetId"] == null)
                    ViewState["CartUserASPNetId"] = Guid.Empty;

                return Guid.Parse(ViewState["CartUserASPNetId"].ToString());
            }
            set { ViewState["CartUserASPNetId"] = value; }
        }

        protected string _ProgramName;
        protected string _ProgramId;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            rptMenuPrograms.ItemDataBound += rptMenuPrograms_ItemDataBound;
            rpet_plan_options.ItemDataBound += rpet_plan_options_ItemDataBound;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)//ViewState["CartUserASPNetId"] == null
            {
                hlAutoRenewLogin.Visible = true;
                hlAutoRenewLogin.NavigateUrl = "~/login.aspx?rp=" + Request.Url.ToString().Split('/').Last();

                divRecurring.Visible = false;
                //cbxRecurring.AutoPostBack = true;
                //cbxRecurring.CheckedChanged += cbxRecurring_CheckedChanged;
            }
        }

        //void cbxRecurring_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (ViewState["CartUserASPNetId"] == null)
        //    {
        //        Response.Redirect("~/login.aspx?rp=" + Request.Url.ToString().Split('/').Last(), true);
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Page.RouteData.Values["programname"] != null)
                {
                    if (string.IsNullOrEmpty(Page.RouteData.Values["programname"].ToString()))
                    {
                        BindrptMenuPrograms();
                    }
                    else
                    {
                        hccProgram program = hccProgram.GetBy(true).Where(x => Regex.Replace(x.Name, @"[^\w\d/]", "-") == Page.RouteData.Values["programname"].ToString()).SingleOrDefault();
                        if (program != null)
                            FillProgram(program);
                        else
                            BindrptMenuPrograms();
                    }
                }
                else
                {
                    BindrptMenuPrograms();
                }
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //if (Request.QueryString["rc"] != null && Request.QueryString["rc"] == "true")
            //    cbxRecurring.Checked = true;
        }

        public void lnkSelectClick(Object sender, EventArgs e)
        {
            try
            {
                int programId = (Information.IsNumeric(((LinkButton)sender).CommandArgument)) ? Convert.ToInt32(((LinkButton)sender).CommandArgument) : 0;
                hccProgram program = hccProgram.GetById(programId);
                FillProgram(program);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void BindrptMenuPrograms()
        {
            List<hccProgram> programs = hccProgram.GetBy(true).Where(p => p.DisplayOnWebsite).ToList();

            rptMenuPrograms.DataSource = programs;
            rptMenuPrograms.DataBind();
        }

        void rptMenuPrograms_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                hccProgram program = (hccProgram)e.Item.DataItem;

                Label lblPricePerDay = (Label)e.Item.FindControl("lblPricePerDay");

                if (lblPricePerDay != null)
                    lblPricePerDay.Text = program.GetCheapestPlanPrice().ToString("c");

                if (program.MoreInfoNavID.HasValue)
                {
                    HyperLink lnkMoreInfo = (HyperLink)e.Item.FindControl("lnkMoreInfo");

                    if (lnkMoreInfo != null && program.MoreInfoNavID.HasValue && program.MoreInfoNavID.Value > 0)
                        lnkMoreInfo.NavigateUrl = BayshoreSolutions.WebModules.Webpage.GetWebpage(program.MoreInfoNavID.Value).Path;
                }
            }
        }

        void rpet_plan_options_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            hccProgramOption progOpt = (hccProgramOption)e.Item.DataItem;
            Label lblOptionPrice = (Label)e.Item.FindControl("lblOptionPrice");

            if (lblOptionPrice != null)
            {
                if (progOpt.OptionValue == 0)
                    lblOptionPrice.Text = string.Format("{0:f2}", progOpt.OptionValue);
                else if (progOpt.OptionValue < 0)
                    lblOptionPrice.Text = "-$" + string.Format("{0:f2}", Math.Abs(progOpt.OptionValue));
                else if (progOpt.OptionValue > 0)
                    lblOptionPrice.Text = "+$" + string.Format("{0:f2}", progOpt.OptionValue);
            }
        }

        //public string GetProgramImage(string programName)
        //{
        //    var fileName = "/userfiles/images/programs/" + programName.Replace(" ", "-") + ".jpg";

        //    if (!File.Exists(Server.MapPath(fileName)))
        //        return "";

        //    return "<img width='150' height='126' alt='' src='/userfiles/images/programs/" + programName.Replace(" ", "-") + ".jpg' class='left' />";
        //}

        protected void btn_add_to_cart_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("AddToCartGroup");

                if (Page.IsValid)
                {
                    hccCart userCart = hccCart.GetCurrentCart();

                    //Define form variables
                    int itemId = Convert.ToInt32(Request.Form["plan_type"]);
                    int optionId = ((Request.Form["plan_option"] == null) ? 0 : Convert.ToInt32(Request.Form["plan_option"]));

                    //Select chosen Program Plan
                    hccProgramPlan plan = hccProgramPlan.GetById(itemId);
                    if (plan == null)
                        throw new Exception("ProgramPlan not found: " + itemId.ToString());

                    hccProgram prog = hccProgram.GetById(plan.ProgramID);
                    hccProgramOption option = hccProgramOption.GetBy(plan.ProgramID).Where(a => a.ProgramOptionID == optionId).SingleOrDefault();

                    int numDays = plan.NumDaysPerWeek * plan.NumWeeks;
                    int numMeals = numDays * plan.MealsPerDay;
                    decimal dailyPrice = plan.PricePerDay + option.OptionValue;
                    decimal itemPrice = numDays * dailyPrice;
                    DateTime deliveryDate = DateTime.Parse(ddl_delivery_date.SelectedValue);

                    MembershipUser user = Helpers.LoggedUser;

                    hccCartItem newItem = new hccCartItem
                    {
                        CartID = userCart.CartID,
                        CreatedBy = (user == null ? Guid.Empty : (Guid)user.ProviderUserKey),
                        CreatedDate = DateTime.Now,
                        IsTaxable = plan.IsTaxEligible,
                        ItemDesc = plan.Description,
                        NumberOfMeals = numMeals,
                        //ItemName = string.Format("{0} - {1} - {2} - {3} & {4}", (prog == null ? string.Empty : prog.Name), plan.Name, option.OptionText, deliveryDate.ToShortDateString(), numMeals),
                        ItemName = string.Format("{0} - {1} - {2} - {3}", (prog == null ? string.Empty : prog.Name), plan.Name, option.OptionText, deliveryDate.ToShortDateString()),
                        ItemPrice = itemPrice,
                        ItemTypeID = (int)Enums.CartItemType.DefinedPlan,
                        Plan_IsAutoRenew = false, //chx_renew.Checked,
                        Plan_PlanID = itemId,
                        Plan_ProgramOptionID = optionId,
                        DeliveryDate = deliveryDate,
                        Quantity = int.Parse(txt_quantity.Text),
                        UserProfileID = ((ddlProfiles.Items.Count == 0) ? 0 : Convert.ToInt32(ddlProfiles.SelectedValue)),
                        IsCompleted = false
                    };
                    Meals obj = new Meals();
                    obj.CartID = newItem.CartID;
                    obj.MealCount = newItem.NumberOfMeals;
                    obj.NoOfWeeks = plan.NumWeeks;

                    var ID = obj.CartID;
                    var Meal = obj.MealCount;
                    var Weeks = obj.NoOfWeeks;

                    HealthyChef.Templates.HealthyChef.Controls.TopHeader header =
                            (HealthyChef.Templates.HealthyChef.Controls.TopHeader)this.Page.Master.FindControl("TopHeader1");
                    if (header != null)
                    {
                        header.MealsCountVal(ID, Meal);
                    }

                    newItem.GetOrderNumber(userCart);
                    int profileId = 0;
                    if (divProfiles.Visible)
                    {
                        profileId = int.Parse(ddlProfiles.SelectedValue);
                    }
                    else
                    {
                        if (CartUserASPNetId != Guid.Empty)
                            profileId = hccUserProfile.GetParentProfileBy(CartUserASPNetId).UserProfileID;
                    }

                    if (profileId > 0)
                        newItem.UserProfileID = profileId;

                    hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, newItem.ItemName, profileId);

                    if (existItem == null)
                    {
                        newItem.Save();

                        hccProductionCalendar cal;

                        for (int i = 0; i < plan.NumWeeks; i++)
                        {
                            cal = hccProductionCalendar.GetBy(newItem.DeliveryDate.AddDays(7 * i));

                            if (cal != null)
                            {
                                hccCartItemCalendar cartCal = new hccCartItemCalendar { CalendarID = cal.CalendarID, CartItemID = newItem.CartItemID };
                                cartCal.Save();
                            }
                            else
                                BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(
                                    "No production calendar found for Delivery Date: " + newItem.DeliveryDate.AddDays(7 * i).ToShortDateString(), this);
                        }
                    }
                    else
                    {
                        existItem.AdjustQuantity(existItem.Quantity + newItem.Quantity);
                    }

                    //Recurring Order Record
                    if (cbxRecurring.Checked)
                    {
                        List<hccRecurringOrder> lstRo = null;
                        if (Session["autorenew"] != null)
                            lstRo = ((List<hccRecurringOrder>)Session["autorenew"]);
                        else
                            lstRo = new List<hccRecurringOrder>();

                        //var filter = cartItemsRecurring.Where(ci => ci.ItemType == Enums.CartItemType.DefinedPlan);

                        //for(var i = 0; i < int.Parse(txt_quantity.Text); i++)
                        //{
                        lstRo.Add(new hccRecurringOrder
                        {
                            CartID = userCart.CartID,
                            CartItemID = newItem.CartItemID,
                            UserProfileID = newItem.UserProfileID,
                            AspNetUserID = userCart.AspNetUserID,
                            PurchaseNumber = userCart.PurchaseNumber,
                            TotalAmount = newItem.ItemPrice
                        });
                        Session["autorenew"] = lstRo;
                        //}
                    }

                    HealthyChef.Templates.HealthyChef.Controls.TopHeader header1 =
                             (HealthyChef.Templates.HealthyChef.Controls.TopHeader)this.Page.Master.FindControl("TopHeader1");

                    if (header1 != null)
                        header.SetCartCount();

                    //Redirect user to Program Selection screen
                    Response.Redirect("~/meal-programs.aspx");
                    //multi_programs.ActiveViewIndex = 0;
                    //litMessage.Text = "Your Meal Program has been added to your cart.";

                }
                else { }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void lkbCancel_Click(object sender, EventArgs e)
        {
            //Redirect user to Program Selection screen
            multi_programs.ActiveViewIndex = 0;

            Clear();
        }

        void BindddlProfiles()
        {
            // get all profiles under the CartUserASPNetId
            var activeProfiles = (from profile in hccUserProfile.GetBy(CartUserASPNetId) where profile.IsActive select profile).ToList();

            if (activeProfiles.Any())
            {
                ddlProfiles.DataSource = activeProfiles;
                ddlProfiles.DataTextField = "ProfileName";
                ddlProfiles.DataValueField = "UserProfileID";
                ddlProfiles.DataBind();

                divProfiles.Visible = true;
            }
        }

        void Clear()
        {
            ddl_delivery_date.ClearSelection();
            //chx_renew.Checked = false;
            txt_quantity.Text = "1";

        }

        private void FillProgram(hccProgram program)
        {
            _ProgramName = program.Name;
            _ProgramId = program.ProgramID.ToString();

            rpet_plan_types.DataSource = hccProgramPlan.GetBy(program.ProgramID, true).Where(prog => prog.PricePerDay > 0).ToList();
            rpet_plan_types.DataBind();

            //Bind Plan Option data if there is any
            rpet_plan_options.DataSource = hccProgramOption.GetBy(program.ProgramID)
                .Where(a => !string.IsNullOrWhiteSpace(a.OptionText)).ToList();
            rpet_plan_options.DataBind();

            //Get the valid delivery dates
            List<hccProductionCalendar> calendar = hccProductionCalendar.GetNext4Calendars();

            //List<hccProductionCalendar> calendar = hccProductionCalendar.GetAll().Where(cal => cal.OrderCutOffDate > DateTime.Now).OrderBy(ord => ord.DeliveryDate).ToList();
            ddl_delivery_date.DataSource = calendar;
            ddl_delivery_date.DataTextField = "DeliveryDate";
            ddl_delivery_date.DataValueField = "DeliveryDate";//"CalendarID";
            ddl_delivery_date.DataBind();

            MembershipUser user = Helpers.LoggedUser;

            if (user != null)
            {
                CartUserASPNetId = (Guid)user.ProviderUserKey;
                BindddlProfiles();
            }

            multi_programs.ActiveViewIndex = multi_programs.ActiveViewIndex + 1;
        }
    }

    public class PlanOption
    {
        public int OptionId { get; set; }
        public decimal PricePerDay { get; set; }
        public string Description { get; set; }
    }
    public class Meals
    {
        public int CartID { get; set; }
        public int MealCount { get; set; }
        public int NoOfWeeks { get; set; }
    }
}