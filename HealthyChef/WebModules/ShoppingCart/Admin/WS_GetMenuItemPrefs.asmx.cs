using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    /// <summary>
    /// Summary description for WS_GetMenuItemPrefs
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WS_GetMenuItemPrefs : System.Web.Services.WebService
    {
        [WebMethod]
        public XmlDocument GetPreferences()
        {
            try
            {
                if (this.Context.Request.QueryString["mid"] != null
                    && !string.IsNullOrWhiteSpace(this.Context.Request.QueryString["mid"]))
                {
                    string prefString = string.Empty, nutInfo = string.Empty, allrgsList = string.Empty;
                    int menuItemId = int.Parse(this.Context.Request.QueryString["mid"]);

                    if (menuItemId > 0)
                    {
                        hccMenuItem menuItem = hccMenuItem.GetById(menuItemId);

                        if (menuItem != null)
                        {
                            // nutrition
                            hccMenuItemNutritionData menuNut = hccMenuItemNutritionData.GetBy(menuItem.MenuItemID);

                            if (menuNut != null)
                            {
                                nutInfo = string.Format("Calories: {0}, Fat: {1}, Protein: {2},  Carbohydrates: {3}, Fiber: {4}, Sodium: {5}",
                                    menuNut.Calories.ToString("f2"), menuNut.TotalFat.ToString("f2"), menuNut.Protein.ToString("f2"),
                                    menuNut.TotalCarbohydrates.ToString("f2"), menuNut.DietaryFiber.ToString("f2"), menuNut.Sodium.ToString("f2"));
                            }
                            else
                            {
                                nutInfo = string.Format("Calories: {0}, Fat: {1}, Protein: {2},  Carbohydrates: {3}, Fiber: {4}, Sodium: {5}", 0, 0, 0, 0, 0,0);
                            }

                            // allergens
                            allrgsList = "Allergens: " + menuItem.AllergensList;

                            // prefs
                            List<hccPreference> prefs = menuItem.GetPreferences();

                            prefs.ForEach(delegate(hccPreference pref)
                            {
                                prefString += pref.PreferenceID.ToString() + ":" + pref.Name + "|";
                            });

                            prefString = prefString.TrimEnd('|');
                        }

                        if (prefString.Trim() == "")
                            prefString = "None";

                        string retVal = string.Format("<menuData><nutrition>{0}</nutrition><allergens>{1}</allergens><prefs>{2}</prefs></menuData>", nutInfo, allrgsList, prefString);
                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(retVal);

                        return xml;
                    }
                    else
                        return null;
                }
                else
                    return null;

            }
            catch { throw; }
        }
    }
}
