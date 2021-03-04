using CheckoutApp.Context;
using CheckoutApp.Services.Interfaces;
using CheckoutApp.Services.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;

namespace CheckoutApp.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Repository
            services.AddScoped<IPaymentStateService, PaymentStateService>();
            services.AddScoped<IPaymentModelStateService, PaymentModelStateService>();
            services.AddScoped<IGatewayService, GatewayService>();
            services.AddScoped<ICardPaymentService, CardPaymentService>();
            services.AddScoped<IDbContext, DataContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

            //Setup Cors
            services.AddCors();
        }

        public static void AddDocumentation(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Checkout App", Version = "v1" });
            });
        }
    }
}
