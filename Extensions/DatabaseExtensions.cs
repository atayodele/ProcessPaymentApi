using CheckoutApp.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckoutApp.Extensions
{
    public static class DatabaseExtensions
    {

        public static void AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }

        public static IWebHost MigrateDb(this IWebHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                try
                {
                    //Get Contexts
                    var appContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

                    //Migrate Databases
                    appContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = host.Services.GetRequiredService<ILogger<DataContext>>();
                    logger.LogError(ex, "An error occurred migrating the the DB.");
                }
            }
            return host;
        }
    }
}
