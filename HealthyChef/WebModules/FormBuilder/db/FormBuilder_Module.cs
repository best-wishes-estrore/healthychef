using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BayshoreSolutions.WebModules.FormBuilder
{
    public partial class FormBuilder_Module
    {
        public static string GetValidationGroup(int moduleId)
        {
            return "FormBuilderResponse_" + moduleId;
        }
    }
}
