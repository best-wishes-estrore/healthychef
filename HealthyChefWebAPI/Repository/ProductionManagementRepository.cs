using HealthyChef.DAL;
using HealthyChefWebAPI.Constants;
using HealthyChefWebAPI.CustomModels;
using HealthyChefWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace HealthyChefWebAPI.Repository
{
    public class ProductionManagementRepository
    {

        public static string GetIssuedCerts(string _startDate = null, string _endDate = null)
        {
            try
            {
                List<GiftIssued> retVals = new List<GiftIssued>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETISSUEDCERTIFICATES", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new GiftIssued()
                            {
                                CartItemId = DBUtil.GetIntField(t, "CartItemId"),
                                RedeemCode = DBUtil.GetCharField(t, "RedeemCode"),
                                Amount = DBUtil.GetDecimalField(t, "Amount").ToString("c"),
                                FirstName = DBUtil.GetCharField(t, "FirstName"),
                                Lastname = DBUtil.GetCharField(t, "Lastname"),
                                IssuedDate = DBUtil.GetCharField(t, "IssuedDate"),
                                SendToRecipient = DBUtil.GetBoolField(t, "SendToRecipient"),
                                IssuedDateObj = DBUtil.GetDateTimeField(t, "IssuedDateObj")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.CartItemId).ToList();

                //Filter Records By StartDate and EndDate.
                DateTime startDateObj = new DateTime();
                DateTime endDateObj = new DateTime();
                bool _validStartDate = false;
                bool _validEndDate = false;
                if (!string.IsNullOrEmpty(_startDate))
                {
                    _validStartDate = DateTime.TryParse(_startDate, out startDateObj);
                }
                if (!string.IsNullOrEmpty(_endDate))
                {
                    _validEndDate = DateTime.TryParse(_endDate, out endDateObj);
                }

                if (_validStartDate)
                    retVals = retVals.Where(a => a.IssuedDateObj >= startDateObj.Date).ToList();

                if (_validEndDate)
                    retVals = retVals.Where(a => a.IssuedDateObj <= endDateObj.AddDays(1)).ToList();
                if (retVals.Count > 0)
                {
                    var giftCerts = hccCartItem.GetGiftsByRedeemed(false);
                    string IssuedTota = giftCerts.Sum(a => a.ItemPrice).ToString();
                    var giftissue = new GiftIssued()
                    {
                        //IssuedTotal = String.Format("Total:${0:0.####}", IssuedTota)
                        IssuedTotal= "Total:"+Math.Round(Convert.ToDouble(IssuedTota), 2).ToString("c")
                };

                    retVals.Add(giftissue);
                }
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        public static string GetRedeemdedCerts(string _startDate = null, string _endDate = null)
        {
            try
            {
                List<GiftRedeemed> retVals = new List<GiftRedeemed>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETREDEMEEDCERTIFICATES", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new GiftRedeemed()
                            {
                                CartItemId = DBUtil.GetIntField(t, "CartItemId"),
                                RedeemCode = DBUtil.GetCharField(t, "RedeemCode"),
                                Amount = DBUtil.GetDecimalField(t, "Amount").ToString("c"),
                                IssuedTo = DBUtil.GetCharField(t, "IssuedTo"),
                                IssuedDate = DBUtil.GetCharField(t, "IssuedDate"),
                                RedeemedBy = DBUtil.GetCharField(t, "RedeemedBy"),
                                RedeemedDate = DBUtil.GetCharField(t, "RedeemedDate"),
                                IssuedDateObj = DBUtil.GetDateTimeField(t, "IssuedDateObj")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.CartItemId).ToList();

                //Filter Records By StartDate and EndDate.
                DateTime startDateObj = new DateTime();
                DateTime endDateObj = new DateTime();
                bool _validStartDate = false;
                bool _validEndDate = false;
                if (!string.IsNullOrEmpty(_startDate))
                {
                    _validStartDate = DateTime.TryParse(_startDate, out startDateObj);
                }
                if (!string.IsNullOrEmpty(_endDate))
                {
                    _validEndDate = DateTime.TryParse(_endDate, out endDateObj);
                }

                if (_validStartDate)
                    retVals = retVals.Where(a => a.IssuedDateObj >= startDateObj.Date).ToList();

                if (_validEndDate)
                    retVals = retVals.Where(a => a.IssuedDateObj <= endDateObj.AddDays(1)).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        public static string GetImported()
        {
            try
            {
                List<GiftImported> retVals = new List<GiftImported>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETIMPORTEDCERTIFICATES", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new GiftImported()
                            {
                                ImportId = DBUtil.GetIntField(t, "ImportId"),
                                Code =DBUtil.GetLongField(t,"Code"),
                                Amount = DBUtil.GetDecimalField(t, "Amount").ToString("c"),
                                DateAdded =DBUtil.GetCharField(t,"DateAdded"),
                                DateExpires=DBUtil.GetCharField(t,"DateExpires")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderByDescending(d => d.Code).ToList();
                
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        public static string GetMenus()
        {
            try
            {
                List<Menus> retVals = new List<Menus>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETMENUS", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Menus()
                            {
                                Name=DBUtil.GetCharField(t, "Name"),
                                MenuID=DBUtil.GetIntField(t, "MenuID")

                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(d => d.Name).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                //return string.Empty;
                return Ex.Message;
            }

        }

        public static string GetFutureCalender(string _startDate = null, string _endDate = null)
        {
            try
            {
                List<FutureCalender> retVals = new List<FutureCalender>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETFUTURECALENDER", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new FutureCalender()
                            {
                                CalendarId = DBUtil.GetIntField(t, "CalendarID"),
                                Name =DBUtil.GetCharField(t, "Name"),
                                DeliveryDate=DBUtil.GetCharField(t, "DeliveryDate"),
                                OrderCutOffDate=DBUtil.GetCharField(t, "OrderCutOffDate"),
                                Menu=DBUtil.GetCharField(t, "Menu"),
                                DeliveryDateObj = DBUtil.GetDateTimeField(t, "DeliveryDateObj")
                            });
                        }

                        conn.Close();
                    }
                }

                //retVals = retVals.OrderBy(d => d.DeliveryDate).ToList();

                //Filter Records By StartDate and EndDate.
                DateTime startDateObj = new DateTime();
                DateTime endDateObj = new DateTime();
                bool _validStartDate = false;
                bool _validEndDate = false;
                if (!string.IsNullOrEmpty(_startDate))
                {
                    _validStartDate = DateTime.TryParse(_startDate, out startDateObj);
                }
                if (!string.IsNullOrEmpty(_endDate))
                {
                    _validEndDate = DateTime.TryParse(_endDate, out endDateObj);
                }

                if (_validStartDate)
                    retVals = retVals.Where(a => a.DeliveryDateObj >= startDateObj).ToList();

                if (_validEndDate)
                    retVals = retVals.Where(a => a.DeliveryDateObj <= endDateObj).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        public static string GetPastCalender(string _startDate = null, string _endDate = null)
        {
            try
            {
                List<PastCalender> retVals = new List<PastCalender>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETPASTCALENDER", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new PastCalender()
                            {
                                CalendarId = DBUtil.GetIntField(t, "CalendarID"),
                                Name = DBUtil.GetCharField(t, "Name"),
                                DeliveryDate = DBUtil.GetCharField(t, "DeliveryDate"),
                                OrderCutOffDate = DBUtil.GetCharField(t, "OrderCutOffDate"),
                                Menu = DBUtil.GetCharField(t, "Menu"),
                                DeliveryDateObj = DBUtil.GetDateTimeField(t, "DeliveryDateObj")
                            });
                        }

                        conn.Close();
                    }
                }

                //retVals = retVals.OrderByDescending(d => d.DeliveryDate).ToList();

                //Filter Records By StartDate and EndDate.
                DateTime startDateObj = new DateTime();
                DateTime endDateObj = new DateTime();
                bool _validStartDate = false;
                bool _validEndDate = false;
                if (!string.IsNullOrEmpty(_startDate))
                {
                    _validStartDate = DateTime.TryParse(_startDate, out startDateObj);
                }
                if (!string.IsNullOrEmpty(_endDate))
                {
                    _validEndDate = DateTime.TryParse(_endDate, out endDateObj);
                }

                if (_validStartDate)
                    retVals = retVals.Where(a => a.DeliveryDateObj >= startDateObj).ToList();

                if (_validEndDate)
                    retVals = retVals.Where(a => a.DeliveryDateObj <= endDateObj).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        //Post
        public static ProductionCalenderResult AddorUpdateProductionCalendar(ProductionCalendar _calendar)
        {
            ProductionCalenderResult calendarResult = new ProductionCalenderResult();
            bool IsUpdate = false;
            bool IsCalendarNameExistcheck = false;
            bool IsCalendarExistcheck = false;
            try
            {
                //validating
                calendarResult = new ProductionCalenderResult(_calendar);
                if (!calendarResult.isValid)
                {
                    return calendarResult;
                }

                hccProductionCalendar calendar = hccProductionCalendar.GetById(_calendar.CalendarId);
                if (calendar != null)
                {
                    IsUpdate = true;
                    if (calendar.Name != _calendar.Name)
                    {
                        IsCalendarNameExistcheck = IsCalendarRecordExist(_calendar.Name);
                    }
                    if (calendar.DeliveryDate != Convert.ToDateTime(_calendar.DeliveryDatestring) && calendar.OrderCutOffDate != Convert.ToDateTime(_calendar.OrderCutOffDatestring))
                    {
                        IsCalendarExistcheck = IsCalendarExist(_calendar.DeliveryDate, _calendar.OrderCutOffDate);
                    }
                }
                else
                {
                    IsUpdate = false;
                    calendar = new hccProductionCalendar();
                    IsCalendarNameExistcheck = IsCalendarRecordExist(_calendar.Name);
                    IsCalendarExistcheck = IsCalendarExist(Convert.ToDateTime(_calendar.DeliveryDatestring), Convert.ToDateTime(_calendar.OrderCutOffDatestring));

                }
                if (Convert.ToDateTime(_calendar.DeliveryDatestring) < DateTime.Now)
                {
                    calendarResult.Message = "Delivery Date must be greater than today's date.";
                }
                else if (Convert.ToDateTime(_calendar.DeliveryDatestring) < Convert.ToDateTime(_calendar.OrderCutOffDatestring))
                {
                    calendarResult.Message = "Order cut-off date must be less than delivery date.";
                }
                else
                {
                    if (IsCalendarNameExistcheck || IsCalendarExistcheck)
                    {
                        calendarResult.Message = "Delivery Date is already exist.";
                    }
                    else
                    {
                        calendar.Name = _calendar.Name;
                        calendar.MenuID = _calendar.MenuId;
                        calendar.DeliveryDate = Convert.ToDateTime(_calendar.DeliveryDatestring);
                        calendar.OrderCutOffDate = Convert.ToDateTime(_calendar.OrderCutOffDatestring);
                        calendar.Description = _calendar.Description;
                        calendar.Save();
                        //prepare result
                        calendarResult.CalendarId = calendar.CalendarID;
                        calendarResult.Name = calendar.Name;
                        calendarResult.Description = calendar.Description;
                        calendarResult.MenuId = calendar.MenuID;
                        calendarResult.DeliveryDate = calendar.DeliveryDate;
                        calendarResult.OrderCutOffDate = calendar.OrderCutOffDate;

                        //prepare response
                        calendarResult.StatusCode = System.Net.HttpStatusCode.OK;
                        calendarResult.IsSuccess = true;
                        calendarResult.Message = IsUpdate ? "Calendar Updated Successfully " + DateTime.Now.ToString() + "" : "Calendar Added Successfully " + DateTime.Now.ToString() + "";

                    }
                }
                    return calendarResult;
                
            }
            catch (Exception Ex)
            {
                string _error = IsUpdate ? "Error while updating Calendar :" : "Error while adding Calendar : ";
                calendarResult.Message = _error + Environment.NewLine + Ex.Message;
                return calendarResult;
            }

        }

        public static GiftCertificatesResult UpdateGiftCertificates(GiftCertificates _giftCertificate)
        {
            GiftCertificatesResult giftcertificateResult = new GiftCertificatesResult();

            try
            {

                hccCart cart = hccCart.GetById(_giftCertificate.CartId);
                hccCartItem _cartitem = hccCartItem.GetById(_giftCertificate.CartItemId);
                if (_cartitem != null && cart != null)
                {
                    hccAddress _address = hccAddress.GetById(Convert.ToInt32(_cartitem.Gift_RecipientAddressId));
                    _address = _address == null ? new hccAddress() : _address;
                    // _cartitem.CartID = cart.CartID;
                    if (_cartitem.CartID != 0)
                    {
                        _cartitem.ItemPrice =Convert.ToDecimal(_giftCertificate.Amount);
                        //recipient info
                        _cartitem.Gift_RecipientEmail = _giftCertificate.ReceipientEmail;
                        _cartitem.Gift_RecipientMessage = _giftCertificate.ReceipientMessage;
                        _cartitem.DeliveryDate = hccProductionCalendar.GetNext4Calendars().First().DeliveryDate;

                        _cartitem.GetOrderNumber(cart);

                        _address.FirstName = _giftCertificate.FirstName;
                        _address.LastName = _giftCertificate.LastName;
                        _address.City = _giftCertificate.City;
                        _address.State = _giftCertificate.State;
                        _address.PostalCode = _giftCertificate.Zipcode;
                        _address.Phone = _giftCertificate.Phone;
                        _address.Address1 = _giftCertificate.Address1;
                        _address.Address2 = _giftCertificate.Address2;
                        _address.AddressTypeID = _giftCertificate.AddressTypeID;
                        _address.Save();
                        _cartitem.Gift_RecipientAddressId = _address.AddressID;

                        string itemFullName = string.Format("{0} - {1} - {2} - For: {3} {4}",
                            "Gift Certificate", _cartitem.Gift_RedeemCode, _cartitem.ItemPrice.ToString("c"),
                            _address.FirstName, _address.LastName);

                        _cartitem.ItemName = itemFullName;
                        _cartitem.IsCompleted = _giftCertificate.SendtoReceipient;
                        _cartitem.Save();
                    }


                    //prepare result
                    giftcertificateResult.CartId = cart.CartID;
                    giftcertificateResult.Amount = _cartitem.ItemPrice;
                    giftcertificateResult.ReceipientEmail = _cartitem.Gift_RecipientEmail;
                    giftcertificateResult.ReceipientMessage = _cartitem.Gift_RecipientMessage;
                    giftcertificateResult.FirstName = _address.FirstName;
                    giftcertificateResult.LastName = _address.LastName;
                    giftcertificateResult.State = _address.State;
                    giftcertificateResult.Zipcode = _address.PostalCode;
                    giftcertificateResult.Phone = _address.Phone;
                    giftcertificateResult.Address1 = _address.Address1;
                    giftcertificateResult.Address2 = _address.Address2;
                    giftcertificateResult.City = _address.City;
                    //prepare response
                    giftcertificateResult.StatusCode = System.Net.HttpStatusCode.OK;
                    giftcertificateResult.IsSuccess = true;
                    giftcertificateResult.Message = "Gift Certificate Saved Successfully " + DateTime.Now.ToString() + "";
                }
                else
                {
                    giftcertificateResult.Message = "Gift Certificate Not Found";
                }

                return giftcertificateResult;
            }
            catch (Exception Ex)
            {
                string _error = "Error while Saving Gift Certifate";
                giftcertificateResult.Message = _error + Environment.NewLine + Ex.Message;
                return giftcertificateResult;
            }

        }

        public static ImportedGiftCertificateResult UpdateImportedGiftCertificate(ImportedGiftCertificate _certificate)
        {
            ImportedGiftCertificateResult importGiftcertResult = new ImportedGiftCertificateResult();

            try
            {
                ImportedGiftCert importGiftCert = ImportedGiftCert.GetById(_certificate.ImportedGiftcertId);

                if (importGiftCert != null)
                {
                    string Redeemed = _certificate.Redeemed == true ? "Y" : "N";
                    importGiftCert.code = _certificate.Code;
                    importGiftCert.amount = _certificate.Amount;
                    importGiftCert.is_used = Redeemed;
                    importGiftCert.date_used = _certificate.DateUsed;
                    importGiftCert.Save();

                    //prepare result
                    bool Redeemedresult = importGiftCert.is_used == "y" ? true : false;
                    importGiftcertResult.ImportedGiftcertId = importGiftCert.ImportsGCID;
                    importGiftcertResult.Code = Convert.ToInt32(importGiftCert.code);
                    importGiftcertResult.Redeemed = Redeemedresult;
                    importGiftcertResult.Amount = Convert.ToDecimal(importGiftCert.amount);
                    importGiftcertResult.DateUsed = importGiftCert.date_used;
                    //prepare response
                    importGiftcertResult.StatusCode = System.Net.HttpStatusCode.OK;
                    importGiftcertResult.IsSuccess = true;
                    importGiftcertResult.Message = "Gift Certificate Saved Successfully " + DateTime.Now.ToString() + "";
                }
                else
                {
                    importGiftcertResult.Message = "Gift Certificate Not Found";
                }
                return importGiftcertResult;
            }
            catch (Exception Ex)
            {
                string _error = "Error while Saving  GiftCertificate ";
                importGiftcertResult.Message = _error + Environment.NewLine + Ex.Message;
                return importGiftcertResult;
            }

        }

        public static MenuResult AddorUpdateMenu(Menu _menu)
        {
            MenuResult menuResult = new MenuResult();
            bool IsUpdate = false;
            try
            {
                hccMenu menu = hccMenu.GetById(_menu.MenuID);
                if (menu != null)
                {
                    IsUpdate = true;
                }
                else
                {
                    IsUpdate = false;
                    menu = new hccMenu();
                }
                return menuResult;
            }

            catch (Exception Ex)
            {
                string _error = "Error while Saving  GiftCertificate ";
                menuResult.Message = _error + Environment.NewLine + Ex.Message;
                return menuResult;
            }

        }

        public static bool IsCalendarRecordExist(string Name)
        {
            bool exist = false;
            var calendarlst = hccProductionCalendar.GetAll();
            if (calendarlst.Count != 0)
            {
                if (calendarlst.Where(x => x.Name == Name).ToList().Count() != 0)
                {
                    exist = true;
                }
            }
            else
            {
                exist = false;
            }
            return exist;
        }
        public static bool IsCalendarExist(DateTime deliverydate,DateTime ordercutoffdate)
        {
            bool exist = false;
            var calendarlst = hccProductionCalendar.GetAll();
            if (calendarlst.Count != 0)
            {
                if (calendarlst.Where(x => x.DeliveryDate == deliverydate&&x.OrderCutOffDate==ordercutoffdate).ToList().Count() != 0)
                {
                    exist = true;
                }
            }
            else
            {
                exist = false;
            }
            return exist;
        }


    }
}