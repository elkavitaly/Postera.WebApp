using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Postera.WebApp
{
    public class Program
    {
        public static Task Main(string[] args) =>
            Host
                .CreateDefaultBuilder()
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.ConfigureKestrel((context, options) =>
                    {
                        options.Configure(context.Configuration.GetSection("Kestrel"));
                    });
                    builder.UseStartup<Startup>();
                    builder.ConfigureLogging(loggingBuilder => loggingBuilder.AddConsole());
                })
                .RunConsoleAsync();
    }
}
