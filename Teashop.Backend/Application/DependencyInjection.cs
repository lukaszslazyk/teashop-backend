using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Teashop.Backend.Application.Commons.Behaviours;
using Teashop.Backend.Application.Product.Queries.GetProductsBySpecification;

namespace Teashop.Backend.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient<ISortOptionNameParser, SortOptionNameParser>();

            return services;
        }
    }
}
