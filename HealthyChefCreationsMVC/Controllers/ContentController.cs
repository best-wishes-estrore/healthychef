using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Extensions;
using HealthyChefCreationsMVC.CustomModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthyChefCreationsMVC.Controllers
{
    public class ContentController : Controller
    {
        hccCartItem CurrentGiftCert=new hccCartItem();
        // GET: Content
        public ActionResult Details(int Id)
        {
            var contentModel = new ContentViewModel(Id);

            return View(contentModel);
        }
        [HttpPost]
        public JsonResult BuyGiftCertificate(GiftCertificateShippinInfo _customerShippingInfo)
        {
            string _message = string.Empty;
            bool _success = false;
            int cartcount= 0;
            try
            {
                hccCart userCart = hccCart.GetCurrentCart();
                CurrentGiftCert = hccCartItem.Gift_GenerateNew(userCart.CartID);

                if (CurrentGiftCert != null)
                {
                    CurrentGiftCert.ItemPrice = decimal.Parse(_customerShippingInfo.Amount);

                    //recipient info
                    CurrentGiftCert.Gift_RecipientEmail = _customerShippingInfo.ReceiptEmail;
                    CurrentGiftCert.Gift_RecipientMessage = _customerShippingInfo.ReceiptMessage;
                    CurrentGiftCert.DeliveryDate = hccProductionCalendar.GetNext4Calendars().First().DeliveryDate;
                    CurrentGiftCert.GetOrderNumber(userCart);
                    MembershipUser user = Helpers.LoggedUser;
                    hccUserProfile _userToUpdate;
                    var CurrentAddress=new hccAddress();
                    if (user != null)
                    {
                        _userToUpdate = GetUserProfileByID(user.ProviderUserKey.ToString());
                    }
                    else
                    {
                        _userToUpdate = null;
                    }
                    if (_userToUpdate != null)
                    {
                        CurrentAddress = hccAddress.GetById(Convert.ToInt32(_userToUpdate.ShippingAddressID));
                    }
                    else
                    {
                        CurrentAddress = null;
                    }
                    hccAddress address;

                    if (CurrentAddress == null)
                    {
                        address = new hccAddress { Country = "US", AddressTypeID = 4 };
                    }
                    else
                    {
                        address = CurrentAddress;
                    }
                    int addrId = address.AddressID;
                    address.FirstName = _customerShippingInfo.FirstName;
                    address.LastName = _customerShippingInfo.LastName;
                    address.Address1 = _customerShippingInfo.Address1;
                    address.Address2 = _customerShippingInfo.Address2;
                    address.City = _customerShippingInfo.City;
                    address.State = _customerShippingInfo.State;
                    address.PostalCode = _customerShippingInfo.PostalCode;
                    address.Phone = _customerShippingInfo.Phone;
                    address.IsBusiness = false;
                    address.Save();

                    CurrentGiftCert.Gift_RecipientAddressId = address.AddressID;

                    string itemFullName = string.Format("{0} - {1} - {2} - For: {3} {4}",
                        "Gift Certificate", CurrentGiftCert.Gift_RedeemCode, CurrentGiftCert.ItemPrice.ToString("c"),
                        address.FirstName, address.LastName);

                    CurrentGiftCert.ItemName = itemFullName;

                    CurrentGiftCert.Save();
                    List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(userCart.CartID);
                    hccCartItem obj = new hccCartItem();

                    cartcount = cartItems.Count;
                    _success = true;
                    _message = "Gift certificate has been added to your cart.";
                }
            }
            catch (Exception ex)
            {
                _message = "Gift certificate not added to your cart.";
                return Json(new { Success = _success, Message = _message, CartCount = cartcount }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = _success, Message = _message, CartCount = cartcount }, JsonRequestBehavior.AllowGet);
        }
        hccUserProfile GetUserProfileByID(string _userID)
        {
            hccUserProfile profile = new hccUserProfile();

            try
            {
                if (!string.IsNullOrEmpty(_userID))
                {
                    Guid aspNetId = Guid.Parse(_userID);
                    profile = hccUserProfile.GetParentProfileBy(aspNetId);
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }

            return profile;
        }

        public ActionResult DetailsByVirtualPath(string virtualpath = null, string virtualpath2 = null, string virtualpath3 = null, string virtualpath4 = null)
        {
            //check the route already exists or not
            bool routeExists = false;
            string _absoluteUri = Request.Url.LocalPath;
            string _currentRoute = @"~/";

            foreach (var route in System.Web.Routing.RouteTable.Routes.OfType<System.Web.Routing.Route>())
            {
                var _abRoute = @"/" + route.Url;
                if (_absoluteUri == _abRoute)
                {
                    routeExists = true;
                    _currentRoute = _abRoute;
                    break;
                }
            }
            if(routeExists)
            {
                return Redirect("~/" + _currentRoute);
            }
            else
            {
                var contentModel = new ContentViewModel(false);
                if (contentModel.Id > 0)
                {
                    var hccProgramdetail = hccProgram.GetAll().Where(x => x.MoreInfoNavID == contentModel.Id).FirstOrDefault();
                    //hccProgram hccProgramdetail = hccProgram.GetBy(contentModel.PageContentTitle);
                    if (hccProgramdetail != null)
                    {
                        ProgramDetailsViewModel programDetailsViewModel = new ProgramDetailsViewModel(hccProgramdetail.ProgramID, 0);
                        return View("~/Views/Programs/Details.cshtml", programDetailsViewModel);
                    }
                    else
                    {
                        return View("Details", contentModel);
                    }
                }
                else
                {
                    if (contentModel.Id != 0)
                        return View("Details", contentModel);
                    else
                        return null;
                }
            }            
        }
    }
}