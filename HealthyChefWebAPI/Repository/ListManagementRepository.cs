using HealthyChef.DAL;
using HealthyChef.DAL.Extensions;
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
    public class ListManagementRepository
    {
        public static string GetAllPrograms()
        {
            try
            {
                List<Programs> retVals = new List<Programs>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETACTIVEPROGRAMS", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Programs()
                            {
                                ProgramID = DBUtil.GetIntField(t, "ProgramID"),
                                Name = DBUtil.GetCharField(t, "Name"),
                                Description = DBUtil.GetCharField(t, "Description"),
                                IsActive = DBUtil.GetBoolField(t, "IsActive")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(d => d.ProgramID).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        public static string GetMealPrefs()
        {
            try
            {
                List<MealPreferences> retVals = new List<MealPreferences>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETMEALPREFS", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new MealPreferences()
                            {
                                PreferenceID = DBUtil.GetIntField(t, "PreferenceID"),
                                Name = DBUtil.GetCharField(t, "Name"),
                                Description = DBUtil.GetCharField(t, "Description"),
                                IsRetired = DBUtil.GetBoolField(t, "IsRetired")
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
                return Ex.Message;
            }

        }

        public static string GetCustomerPrefs()
        {
            try
            {
                List<CustomerPreferences> retVals = new List<CustomerPreferences>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETCUSTMERPREFS", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new CustomerPreferences()
                            {
                                PreferenceID = DBUtil.GetIntField(t, "PreferenceID"),
                                Name = DBUtil.GetCharField(t, "Name"),
                                Description = DBUtil.GetCharField(t, "Description"),
                                IsRetired = DBUtil.GetBoolField(t, "IsRetired")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(d => d.PreferenceID).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        public static string GetPlans()
        {
            try
            {
                List<Plan> retVals = new List<Plan>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETPLANS", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Plan()
                            {
                                PlanID = DBUtil.GetIntField(t, "PlanID"),
                                ProgramID = DBUtil.GetIntField(t, "ProgramID"),
                                Name = DBUtil.GetCharField(t, "Name"),
                                Description = DBUtil.GetCharField(t, "Description"),
                                IsActive = DBUtil.GetBoolField(t, "IsActive"),
                                NumDaysPerWeek = DBUtil.GetIntField(t, "NumDaysPerWeek"),
                                NumWeeks = DBUtil.GetIntField(t, "NumWeeks"),
                                PricePerDay = DBUtil.GetDecimalField(t, "PricePerDay"),
                                IsTaxEligible = DBUtil.GetBoolField(t, "IsTaxEligible"),
                                IsDefault = DBUtil.GetBoolField(t, "IsDefault"),
                                ProgramName = DBUtil.GetCharField(t, "ProgramName")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.ProgramName)
                                .ThenBy(a => a.NumWeeks)
                                .ThenBy(a => a.NumDaysPerWeek).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return Ex.Message;
            }

        }

        public static string GetAllergens()
        {
            try
            {
                List<Allergens> retVals = new List<Allergens>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllAllergens", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Allergens()
                            {
                                AllergenID = DBUtil.GetIntField(t, "AllergenID"),
                                Name = DBUtil.GetCharField(t, "Name"),
                                Description = DBUtil.GetCharField(t, "Description"),
                                IsActive = DBUtil.GetBoolField(t, "IsActive"),
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.Name).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string GetAllCoupons()
        {
            try
            {
                List<Coupon> retVals = new List<Coupon>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllCoupons", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Coupon()
                            {
                                CouponID = Convert.ToInt16(t["CouponID"]),
                                DiscountTypeID = DBUtil.GetIntField(t, "DiscountTypeID"),
                                UsageTypeID = DBUtil.GetIntField(t, "UsageTypeID"),
                                RedeemCode = DBUtil.GetCharField(t, "RedeemCode"),
                                Title = DBUtil.GetCharField(t, "Title"),
                                Description = DBUtil.GetCharField(t, "Description"),
                                Amount = DBUtil.GetDecimalField(t, "Amount"),
                                StartDate = DBUtil.GetNullableDateTimeField(t, "StartDate"),
                                EndDate = DBUtil.GetNullableDateTimeField(t, "EndDate"),
                                IsActive = DBUtil.GetBoolField(t, "IsActive"),
                                CreatedBy = DBUtil.GetGuidField(t, "CreatedBy"),
                                CreatedDate = DBUtil.GetNullableDateTimeField(t, "CreatedDate")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.RedeemCode).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string GetAllIngredient()
        {
            try
            {
                List<Ingredients> retVals = new List<Ingredients>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllIngredient", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Ingredients()
                            {
                                IngredientID = DBUtil.GetIntField(t, "IngredientID"),
                                Name = DBUtil.GetCharField(t, "Name"),
                                Description = DBUtil.GetCharField(t, "Description"),
                                IsRetired = DBUtil.GetBoolField(t, "IsRetired")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.Name).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string GetMessageboxSizes()
        {
            try
            {
                List<MessageBox> retVals = new List<MessageBox>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("hcc_GetBoxSizeList", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new MessageBox()
                            {
                                BoxID = DBUtil.GetIntField(t, "BoxID"),
                                BoxName = DBUtil.GetCharField(t, "BoxName"),
                                DIM_H = DBUtil.GetDecimalField(t, "DIM_H"),
                                DIM_W = DBUtil.GetDecimalField(t, "DIM_W"),
                                DIM_L = DBUtil.GetDecimalField(t, "DIM_L"),
                                MaxNoMeals = DBUtil.GetIntField(t, "MaxNoMeals")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(x => x.BoxName).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string GetShippingZone()
        {
            try
            {
                List<ShippingZone> retVals = new List<ShippingZone>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GetAllShippingZone", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new ShippingZone()
                            {
                                ZoneID = DBUtil.GetIntField(t, "ZoneID"),
                                ZoneName = DBUtil.GetCharField(t, "ZoneName"),
                                Multiplier = DBUtil.GetCharField(t, "Multiplier"),
                                MinFee = DBUtil.GetDecimalField(t, "MinFee"),
                                MaxFee = DBUtil.GetDecimalField(t, "MinFee"),
                                IsDefaultShippingZone = DBUtil.GetBoolField(t, "IsDefaultShippingZone"),
                                IsPickupShippingZone = DBUtil.GetBoolField(t, "IsPickupShippingZone"),
                                TypeName = DBUtil.GetCharField(t, "TypeName")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.ZoneName).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string GetItems()
        {
            try
            {
                List<Items> retVals = new List<Items>();

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("GETITEMS", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Items()
                            {
                                MENUITEMID = DBUtil.GetIntField(t, "MENUITEMID"),
                                ITEMNAME = DBUtil.GetCharField(t, "ITEMNAME"),
                                MEALTYPE = Enums.GetEnumDescription(((Enums.MealTypes)DBUtil.GetIntField(t, "MEALTYPE"))),
                                COSTCHILD = DBUtil.GetDecimalField(t, "COSTCHILD"),
                                COSTREGULAR = DBUtil.GetDecimalField(t, "COSTREGULAR"),
                                COSTLARGE = DBUtil.GetDecimalField(t, "COSTLARGE"),
                                ISTAXELIGIBLE = DBUtil.GetBoolField(t, "ISTAXELIGIBLE"),
                                ISRETIRED = DBUtil.GetBoolField(t, "ISRETIRED"),
                                ALLERGENS = DBUtil.GetCharField(t, "ALLERGENS"),
                                COSTSMALL = DBUtil.GetDecimalField(t, "COSTSMALL")
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.ITEMNAME).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        public static AllergensItem AddOrUpdateAllergen(Allergens _allergen)
        {
            AllergensItem _allergenAdded = new AllergensItem();
            bool isupdate = false;
            bool _isAllergenExists = false;
            
            try
            {
                //validation
                _allergenAdded = new AllergensItem(_allergen);
                if(!_allergenAdded.isValid)
                {
                    return _allergenAdded;
                }

                hccAllergen allergen = hccAllergen.GetById(_allergen.AllergenID);
                if (allergen != null)
                {
                    //allergen.IsActive = _allergen.IsActive;
                    isupdate = true;
                    if (allergen.Name != _allergen.Name)
                    {
                        _isAllergenExists = IsAllergenExistsWithName(_allergen.Name);
                    }
                }
                else
                {
                    allergen = new hccAllergen { IsActive = true };
                    _isAllergenExists = IsAllergenExistsWithName(_allergen.Name);
                }

                if (_isAllergenExists)
                {
                    _allergenAdded.Message = "There is already a allergen that uses the name entered.";
                }
                else
                {
                    allergen.Name = _allergen.Name.Trim();
                    allergen.Description = _allergen.Description.Trim();
                    allergen.Save();

                    //prepare result
                    _allergenAdded.AllergenID = allergen.AllergenID;
                    _allergenAdded.Name = allergen.Name;
                    _allergenAdded.Description = allergen.Description;
                    _allergenAdded.IsActive = allergen.IsActive;

                    //prepare responce
                    _allergenAdded.StatusCode = System.Net.HttpStatusCode.OK;
                    _allergenAdded.IsSuccess = true;
                    _allergenAdded.Message = isupdate ? "Allergen Updated Successfully" : "Allergen Added Successfully";

                }
                return _allergenAdded;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating allergen :" : "Error while adding allergen : ";
                _allergenAdded.Message = _error + Environment.NewLine + ex.Message;
                return _allergenAdded;
            }

        }

        public static bool IsAllergenExistsWithName(string _allergenName)
        {
            bool Exists = false;
            var _allAllergensList = hccAllergen.GetAll();
            if (_allAllergensList.Count != 0)
            {
                if (_allAllergensList.Where(x => x.Name == _allergenName).ToList().Count != 0)
                {
                    Exists = true;
                }
            }
            else
            {
                return Exists;
            }

            return Exists;
        }


        public static CouponItem AddOrUpdateCoupon(Coupon _coupon)
        {
            CouponItem _couponAdded = new CouponItem();
            bool isupdate = false;
            bool _isCouponExists = false;
            try
            {
                //validation
                _couponAdded = new CouponItem(_coupon);
                if (!_couponAdded.isValid)
                {
                    return _couponAdded;
                }


                hccCoupon coupon = hccCoupon.GetById(_coupon.CouponID);

                if (coupon == null)
                {
                    coupon = new hccCoupon
                    {
                        CreatedBy = _coupon.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                }
                else
                    isupdate = true;

                coupon.RedeemCode = _coupon.RedeemCode.Trim().ToUpper();
                coupon.Title = _coupon.Title.Trim();
                coupon.Description = _coupon.Description.Trim();
                coupon.Amount = _coupon.Amount;
                coupon.DiscountTypeID = _coupon.DiscountTypeID;
                coupon.UsageTypeID = _coupon.UsageTypeID;

                if (string.IsNullOrWhiteSpace(_coupon.StartDateString))
                {
                    coupon.StartDate = null;
                }
                else
                {
                    coupon.StartDate = DateTime.Parse(_coupon.StartDateString);
                }

                if (string.IsNullOrWhiteSpace(_coupon.EndDateString))
                {
                    coupon.EndDate = null;
                }
                else
                {
                    coupon.EndDate = DateTime.Parse(_coupon.EndDateString);
                }

                coupon.Save();

                //prepare result
                _couponAdded.CouponID = coupon.CouponID;
                _couponAdded.DiscountTypeID = coupon.DiscountTypeID;
                _couponAdded.UsageTypeID = coupon.UsageTypeID;
                _couponAdded.RedeemCode = coupon.RedeemCode;
                _couponAdded.Title = coupon.Title;
                _couponAdded.Description = coupon.Description;
                _couponAdded.Amount = coupon.Amount;
                _couponAdded.StartDate = coupon.StartDate.ToString();
                _couponAdded.EndDate = coupon.EndDate.ToString();
                _couponAdded.IsActive = coupon.IsActive;

                //prepare responce
                _couponAdded.StatusCode = System.Net.HttpStatusCode.OK;
                _couponAdded.IsSuccess = true;
                _couponAdded.Message = isupdate ? "Coupon Updated Successfully" : "Coupon Added Successfully";

                return _couponAdded;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating coupon :" : "Error while adding coupon : ";
                _couponAdded.Message = _error + Environment.NewLine + ex.Message;
                return _couponAdded;
            }

        }

        public static ShippingZoneItem AddOrUpdateShippingZone(ShippingZone _shippingZone)
        {
            ShippingZoneItem _shippingZoneAdded = new ShippingZoneItem();
            bool isupdate = false;
            bool _isshippingZoneExists = false;
            try
            {
                var zipcode = 0;
                int pn = hccShippingZone.AddUpdateShippingZone(zipcode, _shippingZone.ZoneName, _shippingZone.Description, _shippingZone.Multiplier, _shippingZone.MinFee.ToString(), _shippingZone.MaxFee.ToString(), _shippingZone.IsDefaultShippingZone, _shippingZone.IsPickupShippingZone,_shippingZone.OrderMinimum);
                if (pn > 0)
                {
                    _shippingZoneAdded.IsSuccess = true;
                    _shippingZoneAdded.StatusCode = System.Net.HttpStatusCode.OK;
                    if(pn == 1)
                    {
                        _shippingZoneAdded.Message = "ShippingZone Added successfully";
                    }
                    else
                    {
                        isupdate = true;
                        _shippingZoneAdded.Message = "ShippingZone updated successfully";
                    }
                }
                else if (pn == -1)
                {
                    _shippingZoneAdded.Message = "Saving failed for ShippingZone";
                }
                else if (pn == -2)
                {
                    _shippingZoneAdded.Message = "Update Failed for ShippingZone";
                }
                else if (pn == -3)
                {
                    _shippingZoneAdded.Message = "ShippingZone Name already exists";
                }

                return _shippingZoneAdded;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating ShippingZone :" : "Error while adding ShippingZone : ";
                _shippingZoneAdded.Message = _error + Environment.NewLine + ex.Message;
                return _shippingZoneAdded;
            }

        }

        public static PostHttpResponse DeleteShippingZone(int shippingZoneId)
        {
            var res = new PostHttpResponse();
            try
            {
                var _deleteShippingZone = hccShippingZone.Delete(shippingZoneId);
                if (_deleteShippingZone > 0)
                {
                    res.IsSuccess = true;
                    res.StatusCode = System.Net.HttpStatusCode.OK;
                    res.Message = "Successfully deleted the shippingzone";
                }

            }
            catch (Exception ex)
            {
                res.Message = "Error in deleting the shippingzone : " + Environment.NewLine + ex.Message;
            }
            return res;

        }

        public static IngredientItem AddOrUpdateIngredient(Ingredients _ingredient)
        {
            IngredientItem _ingredientAdded = new IngredientItem();
            bool isupdate = false;
            bool _isIngredientExists = false;
            try
            {
                //validation
                _ingredientAdded = new IngredientItem(_ingredient);
                if (!_ingredientAdded.isValid)
                {
                    return _ingredientAdded;
                }

                hccIngredient Ingredient = hccIngredient.GetById(_ingredient.IngredientID);

                if (Ingredient == null)
                {
                    Ingredient = new hccIngredient { IsRetired = false };
                    _isIngredientExists = IsIngredientExistsWithName(_ingredient.Name);
                }
                else
                {
                    isupdate = true;
                    if (Ingredient.Name != _ingredient.Name)
                    {
                        _isIngredientExists = IsIngredientExistsWithName(_ingredient.Name);
                    }
                }

                if (_isIngredientExists)
                {
                    _ingredientAdded.Message = "There is already a Ingredient that uses the name entered.";
                }
                else
                {
                    Ingredient.Name = _ingredient.Name.Trim();
                    Ingredient.Description = _ingredient.Description.Trim();
                    Ingredient.Save();

                    hccIngredientAllergen.RemoveAllergensBy(Ingredient.IngredientID);
                    _ingredient.AllergensIds
                        .ForEach(delegate (int allgId)
                        {
                            hccIngredientAllergen tre = new hccIngredientAllergen()
                            {
                                AllergenID = allgId,
                                IngredientID = Ingredient.IngredientID
                            };
                            tre.Save();
                        });

                    Ingredient.Save();

                    //prepare result
                    _ingredientAdded.IngredientID = Ingredient.IngredientID;
                    _ingredientAdded.Name = Ingredient.Name;
                    _ingredientAdded.Description = Ingredient.Description;
                    _ingredientAdded.IsRetired = Ingredient.IsRetired;

                    //prepare responce
                    _ingredientAdded.StatusCode = System.Net.HttpStatusCode.OK;
                    _ingredientAdded.IsSuccess = true;
                    _ingredientAdded.Message = isupdate ? "Ingredient Updated Successfully" : "Ingredient Added Successfully";

                }
                return _ingredientAdded;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating Ingredient :" : "Error while adding Ingredient : ";
                _ingredientAdded.Message = _error + Environment.NewLine + ex.Message;
                return _ingredientAdded;
            }

        }

        public static bool IsIngredientExistsWithName(string _ingredientName)
        {
            bool Exists = false;
            var _allIngredientsList = hccIngredient.GetAll();
            if (_allIngredientsList.Count != 0)
            {
                if (_allIngredientsList.Where(x => x.Name == _ingredientName).ToList().Count != 0)
                {
                    Exists = true;
                }
            }
            else
            {
                return Exists;
            }

            return Exists;
        }

        public static PostHttpResponse AddOrUpdateItem(ItemPost _item)
        {
            PostHttpResponse _res = new PostHttpResponse();
            bool isupdate = false;
            bool _isItemExists = false;
            try
            {
                //validation
                _res = ValidateItem(_item);
                if(!_res.isValid)
                {
                    return _res;
                }

                hccMenuItem CurrentMenuItem = hccMenuItem.GetById(_item.MenuItemId);

                hccMenuItemNutritionData nutData = null;

                if (CurrentMenuItem == null)
                {
                    CurrentMenuItem = new hccMenuItem();
                    _isItemExists = IsItemExistsWithName(_item.ItemName);
                }
                else
                {
                    isupdate = true; 
                    nutData = hccMenuItemNutritionData.GetBy(CurrentMenuItem.MenuItemID);
                    if (CurrentMenuItem.Name != _item.ItemName)
                    {
                        _isItemExists = IsItemExistsWithName(_item.ItemName);
                    }
                }

                if (_isItemExists)
                {
                    _res.Message = "There is already a MenuItem that uses the name entered.";
                }
                else
                {
                    if (nutData == null)
                        nutData = new hccMenuItemNutritionData();

                    // Save Menu Item
                    CurrentMenuItem.Name = _item.ItemName;
                    CurrentMenuItem.Description = _item.Description;
                    CurrentMenuItem.MealTypeID = _item.MealTypeId;
                    CurrentMenuItem.CostChild = Convert.ToDecimal(_item.CostChild);
                    CurrentMenuItem.CostRegular = Convert.ToDecimal(_item.CostRegular);
                    CurrentMenuItem.CostSmall = Convert.ToDecimal(_item.CostSmall);
                    CurrentMenuItem.CostLarge = Convert.ToDecimal(_item.CostLarge);
                    CurrentMenuItem.IsTaxEligible = _item.IsTaxEligible;
                    //CurrentMenuItem.IsRetired = _item.IsRetired;

                    CurrentMenuItem.UseCostChild = _item.UseCostChild;
                    CurrentMenuItem.UseCostSmall = _item.UseCostSmall;
                    CurrentMenuItem.UseCostRegular = _item.UseCostRegular;
                    CurrentMenuItem.UseCostLarge = _item.UseCostLarge;

                    CurrentMenuItem.CanyonRanchRecipe = _item.CanyonRanchRecipe;
                    CurrentMenuItem.CanyonRanchApproved = _item.CanyonRanchApproved;
                    CurrentMenuItem.VegetarianOptionAvailable = _item.VegetarianOptionAvailable;
                    CurrentMenuItem.VeganOptionAvailable = _item.VeganOptionAvailable;
                    CurrentMenuItem.GlutenFreeOptionAvailable = _item.GlutenFreeOptionAvailable;

                    CurrentMenuItem.Save();

                    //Save the nutrition data
                    nutData.Calories =Convert.ToInt16 (_item.Caleries);
                    nutData.DietaryFiber = Convert.ToInt16(_item.DietaryFiber);
                    nutData.Protein = Convert.ToInt16(_item.Protein);
                    nutData.TotalCarbohydrates = Convert.ToInt16(_item.TotalCarbohydrates);
                    nutData.TotalFat = Convert.ToInt16(_item.TotalFat);
                    nutData.MenuItemID = CurrentMenuItem.MenuItemID;

                    nutData.Save();

                    //save ingredient mappings
                    _item.selectedIngredients.ForEach(delegate (int ingredientId)
                    {
                        hccMenuItemIngredient sel = hccMenuItemIngredient.GetBy(ingredientId, CurrentMenuItem.MenuItemID);

                        if (sel == null)
                            sel = new hccMenuItemIngredient { IngredientID = ingredientId, MenuItemID = CurrentMenuItem.MenuItemID, };

                        sel.Save();
                    });

                    //save prefrences mappings
                    _item.selectedPrefs.ForEach(delegate (int prefId)
                    {
                        hccMenuItemPreference sel = hccMenuItemPreference.GetBy(prefId, CurrentMenuItem.MenuItemID);

                        if (sel == null)
                            sel = new hccMenuItemPreference { PreferenceID = prefId, MenuItemID = CurrentMenuItem.MenuItemID };

                        sel.Save();
                    });

                    //prepare responce 
                    _res.StatusCode = System.Net.HttpStatusCode.OK;
                    _res.IsSuccess = true;
                    _res.Message = isupdate ? "Menu Item Updated Successfully" : "Menu Item Added Successfully";
                }

                return _res;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating Menu item :" : "Error while adding Menu item : ";
                _res.Message = _error + Environment.NewLine + ex.Message;
                return _res;
            }

        }

        public static bool IsItemExistsWithName(string _itemName)
        {
            bool Exists = false;
            var _allItemsList = hccMenuItem.GetAll();
            if (_allItemsList.Count != 0)
            {
                if (_allItemsList.Where(x => x.Name == _itemName).ToList().Count != 0)
                {
                    Exists = true;
                }
            }
            else
            {
                return Exists;
            }

            return Exists;
        }

        public static PostHttpResponse ValidateItem(ItemPost _item)
        {
            PostHttpResponse _res = new PostHttpResponse();
            if(_item.MealTypeId == -1)
            {
                _res.AddValidationError("Item MealType is Required");
                _res.isValid = false;
            }
            if (string.IsNullOrEmpty(_item.ItemName))
            {
                _res.AddValidationError("Item Name is Required");
                _res.isValid = false;
            }
            if (_item.CostChild == "")
            {
                _res.AddValidationError("CostChild is Required");
                _res.isValid = false;
            }
            if (_item.CostSmall == "")
            {
                _res.AddValidationError("CostSmall is Required");
                _res.isValid = false;
            }
            if (_item.CostRegular == "")
            {
                _res.AddValidationError("CostRegular is Required");
                _res.isValid = false;
            }
            if (_item.CostLarge == "")
            {
                _res.AddValidationError("CostLarge is Required");
                _res.isValid = false;
            }
            if (string.IsNullOrEmpty(_item.Description))
            {
                _res.AddValidationError("Item Description is Required");
                _res.isValid = false;
            }

            //nutrition data
            if (_item.Caleries == "")
            {
                _res.AddValidationError("Item Caleries is Required in Nutrition Info");
                _res.isValid = false;
            }
            if (_item.TotalCarbohydrates == "")
            {
                _res.AddValidationError("Item Total Carbohydrates is Required in Nutrition Info");
                _res.isValid = false;
            }
            if (_item.Protein == "")
            {
                _res.AddValidationError("Item Protein is Required in Nutrition Info");
                _res.isValid = false;
            }
            if (_item.TotalFat == "")
            {
                _res.AddValidationError("Item Total Fat is Required in Nutrition Info");
                _res.isValid = false;
            }
            if (_item.DietaryFiber == "")
            {
                _res.AddValidationError("Item Dietary Fiber is Required in Nutrition Info");
                _res.isValid = false;
            }

            return _res;
        }

        public static PlanItem AddOrUpdatePlan(Plan _plan)
        {
            PlanItem _planAdded = new PlanItem();
            bool isupdate = false;
            bool _isPlanExists = false;
            try
            {
                //validate
                _planAdded = new PlanItem(_plan);
                if(!_planAdded.isValid)
                {
                    return _planAdded;
                }

                hccProgramPlan plan = hccProgramPlan.GetById(_plan.PlanID);
                if (plan != null)
                {
                    //allergen.IsActive = _allergen.IsActive;
                    isupdate = true;
                    if (plan.Name != _plan.Name)
                    {
                        _isPlanExists = IsPlanExistsWithName(_plan.Name);
                    }
                }
                else
                {
                    plan = new hccProgramPlan { IsActive = true };
                    _isPlanExists = IsPlanExistsWithName(_plan.Name);
                }

                if (_isPlanExists)
                {
                    _planAdded.Message = "There is already a Plan that uses the name entered.";
                }
                else
                {
                    plan.Name = _plan.Name;
                    plan.Description = _plan.Description;
                    plan.ProgramID = _plan.ProgramID;
                    plan.IsTaxEligible = _plan.IsTaxEligible;
                    plan.IsDefault = _plan.IsDefault;
                    plan.PricePerDay = _plan.PricePerDay;
                    plan.NumWeeks = _plan.NumWeeks;
                    plan.NumDaysPerWeek = _plan.NumDaysPerWeek;
                    plan.Save();

                    //prepare result
                    _planAdded.PlanID = plan.PlanID;
                    _planAdded.ProgramID = plan.ProgramID;
                    _planAdded.Name = plan.Name;
                    _planAdded.Description = plan.Description;
                    _planAdded.IsActive = plan.IsActive;
                    _planAdded.NumDaysPerWeek = plan.NumDaysPerWeek;
                    _planAdded.NumWeeks = plan.NumWeeks;
                    _planAdded.PricePerDay = plan.PricePerDay;
                    _planAdded.IsTaxEligible = plan.IsTaxEligible;
                    _planAdded.IsDefault = plan.IsDefault;

                    //prepare responce
                    _planAdded.StatusCode = System.Net.HttpStatusCode.OK;
                    _planAdded.IsSuccess = true;
                    _planAdded.Message = isupdate ? "Plan Updated Successfully" : "Plan Added Successfully";

                }
                return _planAdded;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating allergen :" : "Error while adding allergen : ";
                _planAdded.Message = _error + Environment.NewLine + ex.Message;
                return _planAdded;
            }

        }

        public static bool IsPlanExistsWithName(string _planName)
        {
            bool Exists = false;
            var _allPlansList = hccProgramPlan.GetAll();
            if (_allPlansList.Count != 0)
            {
                if (_allPlansList.Where(x => x.Name == _planName).ToList().Count != 0)
                {
                    Exists = true;
                }
            }
            else
            {
                return Exists;
            }

            return Exists;
        }

        public static MealPreferencesItem AddOrUpdatePreference(MealPreferences _pref)
        {
            MealPreferencesItem _prefAdded = new MealPreferencesItem();
            bool isupdate = false;
            bool _isPrefExists = false;
            try
            {
                _prefAdded = new MealPreferencesItem(_pref);
                if (!_prefAdded.isValid)
                {
                    return _prefAdded;
                }

                hccPreference pref = hccPreference.GetById(_pref.PreferenceID);
                if (pref != null)
                {
                    //allergen.IsActive = _allergen.IsActive;
                    isupdate = true;
                    if (pref.Name != _pref.Name)
                    {
                        _isPrefExists = IsPrefExistsWithName(_pref.Name);
                    }
                }
                else
                {
                    pref = new hccPreference { IsRetired = false };
                    _isPrefExists = IsPrefExistsWithName(_pref.Name);
                }

                if (_isPrefExists)
                {
                    _prefAdded.Message = "There is already a Preference that uses the name entered.";
                }
                else
                {
                    pref.Name = _pref.Name.Trim();
                    pref.Description = _pref.Description.Trim();
                    pref.PreferenceType = _pref.PrefType;
                    pref.Save();

                    //prepare result
                    _prefAdded.PreferenceID = pref.PreferenceID;
                    _prefAdded.Name = pref.Name;
                    _prefAdded.Description = pref.Description;
                    _prefAdded.IsRetired = pref.IsRetired;
                    _prefAdded.PrefType = pref.PreferenceType;

                    //prepare responce
                    _prefAdded.StatusCode = System.Net.HttpStatusCode.OK;
                    _prefAdded.IsSuccess = true;
                    _prefAdded.Message = isupdate ? "Preference Updated Successfully" : "Preference Added Successfully";

                }
                return _prefAdded;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating Preference :" : "Error while adding Preference : ";
                _prefAdded.Message = _error + Environment.NewLine + ex.Message;
                return _prefAdded;
            }

        }

        public static bool IsPrefExistsWithName(string _prefName)
        {
            bool Exists = false;
            var _allPreferencesList = hccPreference.GetAll();
            if (_allPreferencesList.Count != 0)
            {
                if (_allPreferencesList.Where(x => x.Name == _prefName).ToList().Count != 0)
                {
                    Exists = true;
                }
            }
            else
            {
                return Exists;
            }

            return Exists;
        }

        public static ProgramItem AddOrUpdateProgram(Programs _program)
        {
            ProgramItem _programAdded = new ProgramItem();
            bool isupdate = false;
            bool _isProgramExists = false;
            try
            {
                hccProgram program = hccProgram.GetById(_program.ProgramID);
                if (program != null)
                {
                    //allergen.IsActive = _allergen.IsActive;
                    isupdate = true;
                    if (program.Name != _program.Name)
                    {
                        program.IsActive = _program.IsActive;
                        _isProgramExists = IsProgramExistsWithName(_program.Name);
                    }
                }
                else
                {
                    program = new hccProgram { IsActive = true };
                    _isProgramExists = IsProgramExistsWithName(_program.Name);
                }

                if (_isProgramExists)
                {
                    _programAdded.Message = "There is already a program that uses the name entered.";
                }
                else
                {
                    program.Name = _program.Name;
                    program.Description = _program.Description;
                    program.ImagePath = _program.ImagePath;
                    program.MoreInfoNavID = _program.MoreInfoNavID;

                    program.Save();

                    //prepare result
                    _programAdded.Name = program.Name;
                    _programAdded.Description = program.Description;
                    _programAdded.ImagePath = program.ImagePath;
                    _programAdded.MoreInfoNavID = program.MoreInfoNavID ?? 0;
                    _programAdded.IsActive = program.IsActive;

                    //prepare responce
                    _programAdded.StatusCode = System.Net.HttpStatusCode.OK;
                    _programAdded.IsSuccess = true;
                    _programAdded.Message = isupdate ? "Program Updated Successfully" : "Program Added Successfully";

                }
                return _programAdded;
            }
            catch (Exception ex)
            {
                string _error = isupdate ? "Error while updating Program :" : "Error while adding Program : ";
                _programAdded.Message = _error + Environment.NewLine + ex.Message;
                return _programAdded;
            }

        }

        public static bool IsProgramExistsWithName(string _programName)
        {
            bool Exists = false;
            var _allprogramsList = hccProgram.GetAll();
            if (_allprogramsList.Count != 0)
            {
                if (_allprogramsList.Where(x => x.Name == _programName).ToList().Count != 0)
                {
                    Exists = true;
                }
            }
            else
            {
                return Exists;
            }

            return Exists;
        }
    }
}