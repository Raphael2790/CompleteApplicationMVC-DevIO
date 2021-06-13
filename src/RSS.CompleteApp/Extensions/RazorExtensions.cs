using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSS.CompleteApp.Extensions
{
    public static class RazorExtensions
    {
        public static string DocumentFormat(this RazorPage page, int personType, string document)
        {
            return personType == 1 ? Convert.ToUInt64(document).ToString(@"000\.000\.000\-00") :
                Convert.ToUInt64(document).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}
