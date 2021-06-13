using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;

namespace RSS.CompleteApp.Extensions
{
    public static class LocalizationExtension
    {
        public static IApplicationBuilder UseLocalizationConfiguration(this IApplicationBuilder application)
        {
            var defaultCulture = new CultureInfo("pt-BR");

            var locationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };

            application.UseRequestLocalization(locationOptions);

            return application;
        }
    }
}
