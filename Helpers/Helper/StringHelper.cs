using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Helper
{
    public static class StringHelper
    {
        public static string Truncate(string value, int maxLength)
        {
            string returnValue = string.Empty;
            
            if (!string.IsNullOrEmpty(value))
                 returnValue = value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
            
            return returnValue;
        }
    }
}
