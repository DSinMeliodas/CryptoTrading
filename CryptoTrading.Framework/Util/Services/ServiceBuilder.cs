using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CryptoTrading.Framework.Util.Services;

internal static class ServiceBuilder
{
    public static IHost BuildConfiguredService<T>(string[] args) where T : class, IHostedService
    {
        var hostbuilder = Host.CreateDefaultBuilder(args);
        hostbuilder = hostbuilder.ConfigureServices(ConfigureService<T>);
        return hostbuilder.Build();
    }

    private static void ConfigureService<T>(HostBuilderContext hostContext, IServiceCollection services)
        where T : class, IHostedService
    {
        _ = services.AddHostedService<T>();
    }
}