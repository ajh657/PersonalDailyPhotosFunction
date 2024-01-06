using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonalDailyPhotosFunction;
using PersonalDailyPhotosFunction.NTFY;
using PersonalDailyPhotosFunction.WebDav;


[assembly: FunctionsStartup(typeof(Startup))]

namespace PersonalDailyPhotosFunction
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var services = builder.Services;

            services.AddSingleton<IWebDavInterop, WebDavInterop>();
            services.AddSingleton<INTFYInterop, NTFYInterop>();

            services.AddSingleton<PersonalDailyPhotosFunction>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            builder.ConfigurationBuilder
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
