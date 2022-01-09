using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(AdaTheDev.SampleFunctionApp1.Startup))]

namespace AdaTheDev.SampleFunctionApp1
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<MySettings>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("MySettings").Bind(settings);
                });
        }
    }
}