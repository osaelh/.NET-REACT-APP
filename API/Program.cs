using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
           var Host = CreateHostBuilder(args).Build();
           using var Scope = Host.Services.CreateScope();

           var services= Scope.ServiceProvider;

           try
           {
               var context = services.GetRequiredService<DataContext>();
               context.Database.Migrate();
               await Seed.SeedData(context);
           }
           catch (Exception ex)
           {
               
               var logger = services.GetRequiredService<ILogger<Program>>();
               logger.LogError(ex,"An error occured during thne migration");
           }
           Host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
