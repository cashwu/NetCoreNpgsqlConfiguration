using Cashwu.AspNetCore.Configuration.PostgreSQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace testDatabaseConfiguration
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddPostgreSqlEntityFrameworkValues(options =>
                    {
                        options.ConnectionStringName = "DefaultConnection";
                        options.PollingInterval = 5000;
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}