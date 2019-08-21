using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChefCreationsMVC.CustomModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthyChefCreationsMVC.Controllers
{
    public class ProgramsController : Controller
    {
        [HttpGet]
        public ActionResult EverydayMealPlan()
        {
            HeaderViewModel headerViewModel = new HeaderViewModel();
            List<string> NavigationIdsList = new List<string>();
            var everydaymealplanschilds = headerViewModel.mySiteMap[1];
            foreach(var childnode in everydaymealplanschilds.ChildNodes)
            {
                NavigationIdsList.Add(((System.Web.SiteMapNode)childnode).Key.ToString());
            }
            
            List<ProgramViewModel> everydaymealplansViewModel = new List<ProgramViewModel>();
            for (var i = 0; i < NavigationIdsList.Count(); i++)
            {
                ProgramViewModel programViewModel = new ProgramViewModel();
                hccProgram hccprogram = hccProgram.GetByMoreInfoId(Convert.ToInt16(NavigationIdsList[i]));
                var healthyLiving = hccProgram.GetAll().Where(x=>x.ProgramID == 49).FirstOrDefault();
                if (hccprogram != null && hccprogram.ProgramID == 51 && healthyLiving !=null)
                {
                    programViewModel.ProgramID = hccprogram.ProgramID;
                    programViewModel.Name = healthyLiving.Name;
                    programViewModel.ImagePath = healthyLiving.ImagePath;
                    programViewModel.Description = healthyLiving.Description;
                    programViewModel.Price = hccprogram.GetCheapestPlanPrice().ToString("c");
                    programViewModel.MoreInfoNavID = hccprogram.MoreInfoNavID ?? 0;
                }
                else if (hccprogram != null)
                {
                    programViewModel.ProgramID = hccprogram.ProgramID;
                    programViewModel.Name = hccprogram.Name;
                    programViewModel.ImagePath = hccprogram.ImagePath;
                    programViewModel.Description = hccprogram.Description;
                    programViewModel.Price = hccprogram.GetCheapestPlanPrice().ToString("c");
                    programViewModel.MoreInfoNavID = hccprogram.MoreInfoNavID ?? 0;
                }
                everydaymealplansViewModel.Add(programViewModel);
            }
            return View(everydaymealplansViewModel);
        }
        [HttpGet]
        public ActionResult WeightLossProgram()
        {
            HeaderViewModel headerViewModel = new HeaderViewModel();
            List<string> NavigationIdsList = new List<string>();
            var weightlossprogramschild = headerViewModel.mySiteMap[2];
            foreach (var childnode in weightlossprogramschild.ChildNodes)
            {
                NavigationIdsList.Add(((System.Web.SiteMapNode)childnode).Key.ToString());
            }
            List<ProgramViewModel> weightlossViewModel = new List<ProgramViewModel>();
            for (var i=0;i<NavigationIdsList.Count();i++)
            {
                ProgramViewModel programViewModel = new ProgramViewModel();
                hccProgram hccprogram = hccProgram.GetByMoreInfoId(Convert.ToInt16(NavigationIdsList[i]));
                if (hccprogram != null)
                {
                    programViewModel.ProgramID = hccprogram.ProgramID;
                    programViewModel.Name = hccprogram.Name;
                    programViewModel.ImagePath = hccprogram.ImagePath;
                    programViewModel.Description = hccprogram.Description;
                    programViewModel.Price = hccprogram.GetCheapestPlanPrice().ToString("c");
                    programViewModel.MoreInfoNavID =hccprogram.MoreInfoNavID ?? 0;
                }
                weightlossViewModel.Add(programViewModel);
            }
            return View(weightlossViewModel);
        }
        // GET: Programs
        [HttpGet]
        public ActionResult Index()
        {
            ProgramsViewModel programsViewModel = new ProgramsViewModel();
            return View(programsViewModel);
        }

        [HttpGet]
        public ActionResult Details(int Id)//, string programName = null)
        {
            ProgramDetailsViewModel programDetailsViewModel = new ProgramDetailsViewModel(Id, 0);
            return View(programDetailsViewModel);
        }

        [HttpGet]
        public ActionResult DetailsbyCheckbox(int Id=0, string programName=null)
        {
            ProgramDetailsViewModel programDetailsViewModel = new ProgramDetailsViewModel(Id, 0);
            return View("~/Views/Programs/Details.cshtml", programDetailsViewModel);
        }

        [HttpPost]
        public ActionResult DetailsbyCalendarId(int Id, int calendarid)
        {
            var programDetailsViewModel = new ProgramDetailsViewModel(Id, calendarid);
            return Json(new { Message = RenderRazorViewToString("~/Views/Programs/_ExampleDisplayMenu.cshtml",programDetailsViewModel)}, JsonRequestBehavior.AllowGet);
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        [HttpGet]
        public ActionResult MoreInfo(int Id)
        {
            int navigationId = 0;
            var _program = hccProgram.GetBy(true).
                            Where(p => p.DisplayOnWebsite && p.ProgramID == Id)
                            .FirstOrDefault();
            if (_program != null)
            {
                navigationId = _program.MoreInfoNavID ?? 0;
            }

            return RedirectToAction("Details", "Content", new { Id = navigationId });
        }

        [HttpPost]
        public JsonResult AddProgramToCart(ProgramAddToCartModel programAddToCartModel)
        {
            ResponseModel objResponseModel = new ResponseModel();
            try
            {
                if (!programAddToCartModel.isRecurring)
                {
                    objResponseModel = AddToCartPurchaseonetimeorder(programAddToCartModel);
                }
                else
                {
                    objResponseModel = AddToCartSubscribeorder(programAddToCartModel);
                }
                objResponseModel.result = "";
                objResponseModel.statusCode = 200;
                objResponseModel.errorMessage = "";
                objResponseModel.successMessage = "Product Added To Cart";

            }
            catch (Exception ex)
            {
                objResponseModel.result = "";
                objResponseModel.statusCode = 400;
                objResponseModel.errorMessage = ex.Message;
                objResponseModel.successMessage = "";
            }

            return Json(objResponseModel, JsonRequestBehavior.AllowGet);
        }
        private ResponseModel AddToCartPurchaseonetimeorder(ProgramAddToCartModel programAddToCartModel)
        {
            ResponseModel objResponseModel = new ResponseModel();
            try
            {
                if (programAddToCartModel != null)
                {
                    if (programAddToCartModel.Quantity <= 0)
                    {
                        // if Quantity <=0 return to Viewpage, without Perofrorming "AddtoCart"

                        objResponseModel.result = "";
                        objResponseModel.statusCode = 400;
                        objResponseModel.errorMessage = "Quantity should be greater than zero";
                        objResponseModel.successMessage = "";
                    }
                    else
                    {
                        // if Quantity>0

                        hccCart userCart = hccCart.GetCurrentCart();

                        //Define form variables
                        int itemId = programAddToCartModel.itemId;
                        int optionId = programAddToCartModel.optionId;

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
                        if (programAddToCartModel.isRecurring)
                        {
                            double itempricewithdiscount = Convert.ToDouble(itemPrice) - Convert.ToDouble(itemPrice) * 0.05;
                            itemPrice = Convert.ToDecimal(itempricewithdiscount);
                        }
                        DateTime deliveryDate = DateTime.Parse(programAddToCartModel.deliveryDateString);

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
                            Plan_IsAutoRenew = programAddToCartModel.isRecurring,//false, //chx_renew.Checked,
                            Plan_PlanID = itemId,
                            Plan_ProgramOptionID = optionId,
                            DeliveryDate = deliveryDate,
                            Quantity = programAddToCartModel.Quantity,
                            UserProfileID = programAddToCartModel.profileId,
                            IsCompleted = false
                        };

                        Meals obj = new Meals();
                        obj.CartID = newItem.CartID;
                        obj.MealCount = newItem.NumberOfMeals;
                        obj.NoOfWeeks = plan.NumWeeks;

                        var ID = obj.CartID;
                        var Meal = obj.MealCount;
                        var Weeks = obj.NoOfWeeks;

                        newItem.GetOrderNumber(userCart);
                        int profileId = 0;
                        if (programAddToCartModel.profileId != 0)
                        {
                            profileId = programAddToCartModel.profileId;
                        }
                        else
                        {
                            if (user != null)
                            {
                                var CartUserASPNetId = Guid.Empty;
                                bool isSuccess = Guid.TryParse(Convert.ToString(user.ProviderUserKey), out CartUserASPNetId);

                                if (CartUserASPNetId != Guid.Empty)
                                    profileId = hccUserProfile.GetParentProfileBy(CartUserASPNetId).UserProfileID;
                            }
                        }

                        if (profileId > 0)
                            newItem.UserProfileID = profileId;

                        //hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, newItem.ItemName, profileId);
                        var existItem = hccCartItem.GetBy(userCart.CartID).Where(x => x.UserProfileID == profileId && x.ItemName == newItem.ItemName && x.Plan_IsAutoRenew == false);
                        //var existitemssubscriptionornot = hccCartItem.GetBy(userCart.CartID).Where(x => x.UserProfileID == profileId && x.ItemName == newItem.ItemName && x.Plan_IsAutoRenew == true);

                        if (existItem.Count() == 0)
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
                            //Recurring Order Record
                            if (programAddToCartModel.isRecurring)
                            {
                                List<hccRecurringOrder> lstRo = null;
                                if (Session["autorenew"] != null)
                                    lstRo = ((List<hccRecurringOrder>)Session["autorenew"]);
                                else
                                    lstRo = new List<hccRecurringOrder>();

                                hccRecurringOrder hccrecurringOrder = new hccRecurringOrder
                                {
                                    CartID = userCart.CartID,
                                    CartItemID = newItem.CartItemID,
                                    UserProfileID = newItem.UserProfileID,
                                    AspNetUserID = userCart.AspNetUserID,
                                    PurchaseNumber = userCart.PurchaseNumber,
                                    TotalAmount = newItem.ItemPrice
                                };
                                //hccrecurringOrder.Save();
                                lstRo.Add(hccrecurringOrder);

                                Session["autorenew"] = lstRo;
                            }
                        }
                        else
                        {
                            var increasequanityitem = existItem.Where(x => x.OrderNumber == newItem.OrderNumber).FirstOrDefault();
                            increasequanityitem.AdjustQuantity(increasequanityitem.Quantity + newItem.Quantity);

                        }
                        objResponseModel.result = "";
                        objResponseModel.statusCode = 200;
                        objResponseModel.errorMessage = "";
                        objResponseModel.successMessage = "Product Added To Cart";

                    }
                }
                else
                {

                    objResponseModel.result = "";
                    objResponseModel.statusCode = 400;
                    objResponseModel.errorMessage = "Please select valid data";
                    objResponseModel.successMessage = "";
                }
            }
            catch (Exception ex)
            {
                objResponseModel.result = "";
                objResponseModel.statusCode = 400;
                objResponseModel.errorMessage = ex.Message;
                objResponseModel.successMessage = "";
            }
            return objResponseModel;
        }
        private ResponseModel AddToCartSubscribeorder(ProgramAddToCartModel programAddToCartModel)
        {
            ResponseModel objResponseModel = new ResponseModel();
            try
            {
                if (programAddToCartModel != null)
                {
                    if (programAddToCartModel.Quantity <= 0)
                    {
                        // if Quantity <=0 return to Viewpage, without Perofrorming "AddtoCart"

                        objResponseModel.result = "";
                        objResponseModel.statusCode = 400;
                        objResponseModel.errorMessage = "Quantity should be greater than zero";
                        objResponseModel.successMessage = "";
                    }
                    else
                    {
                        // if Quantity>0

                        hccCart userCart = hccCart.GetCurrentCart();

                        //Define form variables
                        int itemId = programAddToCartModel.itemId;
                        int optionId = programAddToCartModel.optionId;

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
                        //if (programAddToCartModel.isRecurring)
                        //{
                        //    double itempricewithdiscount = Convert.ToDouble(itemPrice) - Convert.ToDouble(itemPrice) * 0.05;
                        //    itemPrice = Convert.ToDecimal(itempricewithdiscount);
                        //}
                        DateTime deliveryDate = DateTime.Parse(programAddToCartModel.deliveryDateString);
                        var adjustmentprice = Math.Round(Convert.ToDecimal(Convert.ToDouble(itemPrice) - Convert.ToDouble(itemPrice) * 0.05), 2);
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
                            Plan_IsAutoRenew = programAddToCartModel.isRecurring,//false, //chx_renew.Checked,
                            Plan_PlanID = itemId,
                            Plan_ProgramOptionID = optionId,
                            DeliveryDate = deliveryDate,
                            Quantity = programAddToCartModel.Quantity,
                            UserProfileID = programAddToCartModel.profileId,
                            IsCompleted = false,
                            DiscountAdjPrice = Math.Round(Convert.ToDecimal(Convert.ToDouble(itemPrice) - Convert.ToDouble(itemPrice) * 0.05), 2),
                            DiscountPerEach= Math.Round(Convert.ToDecimal(Convert.ToDouble(itemPrice) - Convert.ToDouble(adjustmentprice)), 2)
                        };

                        Meals obj = new Meals();
                        obj.CartID = newItem.CartID;
                        obj.MealCount = newItem.NumberOfMeals;
                        obj.NoOfWeeks = plan.NumWeeks;

                        var ID = obj.CartID;
                        var Meal = obj.MealCount;
                        var Weeks = obj.NoOfWeeks;

                        newItem.GetOrderNumber(userCart);
                        int profileId = 0;
                        if (programAddToCartModel.profileId != 0)
                        {
                            profileId = programAddToCartModel.profileId;
                        }
                        else
                        {
                            if (user != null)
                            {
                                var CartUserASPNetId = Guid.Empty;
                                bool isSuccess = Guid.TryParse(Convert.ToString(user.ProviderUserKey), out CartUserASPNetId);

                                if (CartUserASPNetId != Guid.Empty)
                                    profileId = hccUserProfile.GetParentProfileBy(CartUserASPNetId).UserProfileID;
                            }
                        }

                        if (profileId > 0)
                            newItem.UserProfileID = profileId;

                        //hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, newItem.ItemName, profileId);
                        //var existItem = hccCartItem.GetBy(userCart.CartID).Where(x => x.UserProfileID == profileId && x.ItemName == newItem.ItemName && x.Plan_IsAutoRenew == false);
                        var existitemssubscriptionornot = hccCartItem.GetBy(userCart.CartID).Where(x => x.UserProfileID == profileId && x.ItemName == newItem.ItemName && x.Plan_IsAutoRenew == true);

                        if (existitemssubscriptionornot.Count() == 0)
                        {
                            newItem.Save();
                            //var subtotaldiscountamount = Math.Round(userCart.SubTotalDiscount + newItem.DiscountPerEach, 2);
                            //userCart.SubTotalDiscount = Math.Round(subtotaldiscountamount, 2);
                            //userCart.Save();
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
                            //Recurring Order Record
                            if (programAddToCartModel.isRecurring)
                            {
                                List<hccRecurringOrder> lstRo = null;
                                if (Session["autorenew"] != null)
                                    lstRo = ((List<hccRecurringOrder>)Session["autorenew"]);
                                else
                                    lstRo = new List<hccRecurringOrder>();

                                hccRecurringOrder hccrecurringOrder = new hccRecurringOrder
                                {
                                    CartID = userCart.CartID,
                                    CartItemID = newItem.CartItemID,
                                    UserProfileID = newItem.UserProfileID,
                                    AspNetUserID = userCart.AspNetUserID,
                                    PurchaseNumber = userCart.PurchaseNumber,
                                    TotalAmount = Math.Round(Convert.ToDecimal(Convert.ToDouble(newItem.ItemPrice) -Convert.ToDouble(newItem.ItemPrice) * 0.05), 2)
                            };
                                //hccrecurringOrder.Save();
                                lstRo.Add(hccrecurringOrder);

                                Session["autorenew"] = lstRo;
                            }
                        }
                        else
                        {
                            var increasequanityitem = existitemssubscriptionornot.Where(x => x.OrderNumber == newItem.OrderNumber).FirstOrDefault();
                            increasequanityitem.AdjustQuantity(increasequanityitem.Quantity + newItem.Quantity);
                            increasequanityitem.DiscountAdjPrice = Math.Round(Convert.ToDecimal(Convert.ToDouble(itemPrice) - Convert.ToDouble(itemPrice) * 0.05), 2);
                            increasequanityitem.DiscountPerEach = Math.Round(Convert.ToDecimal(Convert.ToDouble(itemPrice) - Convert.ToDouble(adjustmentprice)), 2);
                            increasequanityitem.Quantity = increasequanityitem.Quantity + newItem.Quantity;
                            increasequanityitem.Save();
                            var subtotaldiscountamount = Math.Round(userCart.SubTotalDiscount + newItem.DiscountPerEach, 2);
                            userCart.SubTotalDiscount = Math.Round(subtotaldiscountamount, 2);
                            userCart.Save();
                        }
                        objResponseModel.result = "";
                        objResponseModel.statusCode = 200;
                        objResponseModel.errorMessage = "";
                        objResponseModel.successMessage = "Product Added To Cart";
                    }
                }
                else
                {

                    objResponseModel.result = "";
                    objResponseModel.statusCode = 400;
                    objResponseModel.errorMessage = "Please select valid data";
                    objResponseModel.successMessage = "";
                }
            }
            catch (Exception ex)
            {
                objResponseModel.result = "";
                objResponseModel.statusCode = 400;
                objResponseModel.errorMessage = ex.Message;
                objResponseModel.successMessage = "";
            }
            return objResponseModel;
        }
        [HttpPost]
        public JsonResult GetPlanPriceByNumofweeks(ProgramPlanDetails PlanPriceByNumofweeks)
        {
            try
            {
                hccProgramOption hccProgramOption = hccProgramOption.GetById(PlanPriceByNumofweeks.ProgramOptionId);
                hccProgramPlan hccProgramPlan = hccProgramPlan.GetBy(PlanPriceByNumofweeks.ProgramId, true).Where(x => x.PricePerDay > 0 && x.NumWeeks == PlanPriceByNumofweeks.NumWeeks && x.NumDaysPerWeek == PlanPriceByNumofweeks.NumDaysPerWeek).FirstOrDefault();
                if (hccProgramOption != null)
                {
                    PlanPriceByNumofweeks.PlanOptionprice = hccProgramOption.OptionValue;
                }
                if (hccProgramPlan != null)
                {
                    PlanPriceByNumofweeks.Planprice = hccProgramPlan.PricePerDay;
                    PlanPriceByNumofweeks.PlanId = hccProgramPlan.PlanID;
                }
                return Json(PlanPriceByNumofweeks, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}