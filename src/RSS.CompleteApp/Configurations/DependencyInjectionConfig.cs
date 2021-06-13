using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using RSS.Business.Interfaces;
using RSS.CompleteApp.Extensions;
using RSS.Data.Context;
using RSS.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSS.CompleteApp.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<CompleteAppDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, CurrencyValidationAttributeProvider>();
            return services;
        }
    }
}
