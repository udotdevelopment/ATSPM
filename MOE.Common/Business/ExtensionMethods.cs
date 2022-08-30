using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOE.Common.Business
{
    public static class ExtensionMethods
    {
        public static string GetDescription(this Enum e)
        {
            var fieldInfo = e.GetType().GetField(e.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : e.ToString();
        }
    }
}