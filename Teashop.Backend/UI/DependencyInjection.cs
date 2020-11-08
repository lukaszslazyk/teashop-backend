﻿using Microsoft.Extensions.DependencyInjection;
using Teashop.Backend.UI.Api.Cart.Mappings;
using Teashop.Backend.UI.Api.Cart.Utils;
using Teashop.Backend.UI.Api.Commons.Filters;
using Teashop.Backend.UI.Api.Product.Mappings;

namespace Teashop.Backend.UI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUI(this IServiceCollection services)
        {
            services.AddTransient<ProductMapper>();
            services.AddTransient<CartMapper>();
            services.AddTransient<SessionCartHandler>();
            services.AddControllers(options =>
                options.Filters.Add(new ApiExceptionFilterAttribute()));

            return services;
        }
    }
}
