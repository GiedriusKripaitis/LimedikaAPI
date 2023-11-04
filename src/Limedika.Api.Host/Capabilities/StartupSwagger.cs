using Microsoft.OpenApi.Models;

namespace Limedika.Api.Host.Capabilities;

public static class StartupSwagger
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = SwaggerConfig.Default.Title,
                    Description = SwaggerConfig.Default.Description,
                });
            });
    }

    public static void UseSwagger(this IApplicationBuilder app)
    {
        if (SwaggerConfig.Default.Operational)
        {
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
                c.RouteTemplate = SwaggerConfig.Default.RouteTemplate;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfig.Default.SwaggerEndpoint, SwaggerConfig.Default.Title);
                c.RoutePrefix = SwaggerConfig.Default.RoutePrefix;
            });
        }
    }
}
