using Limedika.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Limedika.Api.Host.Capabilities;

public static class StartupDatabase
{
    public static void ConfigureDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LimedikaDataContext>(builder => builder.UseSqlServer(configuration.GetConnectionString("LimedikaDb"),
            x =>
            {
                x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Limedika");
                x.MigrationsAssembly("Limedika.Infrastructure");
                x.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            }));
    }

    public static void RunMigration<T>(this IServiceProvider services)
    {
        using (IServiceScope scope = services.CreateScope())
        {
            DbContext db = (DbContext)scope.ServiceProvider.GetRequiredService(typeof(T));
            db.Database.Migrate();
        }
    }
}
