using Limedika.Contracts.Settings;
using Limedika.Infrastructure.Repositories;
using Limedika.Integrations;
using Limedika.Services;

namespace Limedika.Api.Host.Capabilities;

public static class StartupInjection
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetSection("Swagger").Bind(SwaggerConfig.Default);
        services.Configure<SwaggerConfig>(configuration.GetSection("Swagger"));
        configuration.GetSection("PostIt").Bind(PostItSettings.Default);
        services.Configure<PostItSettings>(configuration.GetSection("PostIt"));

        services.AddScoped<IClientService, ClientService>();

        services.AddScoped<IPostItClient, PostItClient>();
        
        services.AddScoped<IClientsRepository, ClientsRepository>();
    }
}
