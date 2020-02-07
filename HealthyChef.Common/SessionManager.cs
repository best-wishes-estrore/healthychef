using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HealthyChef.Common
{
    public static class SessionManager
    {
        static object GetSessionItem(string name)
        {
            if (HttpContext.Current.Session[name] == null)
                return null;

            return HttpContext.Current.Session[name];
        }

        static void SetSessionItem(string name, object value)
        {
            HttpContext.Current.Session[name] = value;
        }

        public static void Clear()
        {
            SessionManager.CurrentUserProfileInfoChanged = false;
        }

        public static bool CurrentUserProfileInfoChanged
        {
            get
            {
                if (GetSessionItem("CurrentUserProfileInfoChanged") == null)
                    SetSessionItem("CurrentUserProfileInfoChanged", false);

                return bool.Parse(GetSessionItem("CurrentUserProfileInfoChanged").ToString());
            }
            set
            {
                SetSessionItem("CurrentUserProfileInfoChanged", value);
            }
        }
    }
}
