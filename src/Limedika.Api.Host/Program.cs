using Limedika.Api.Host.Capabilities;
using Limedika.Infrastructure;
using Newtonsoft.Json.Converters;
using Serilog;
using Serilog.Formatting.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureSwagger();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter());
    });

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.Console(new JsonFormatter())
    .CreateLogger();

builder.Host.UseSerilog();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseHttpsRedirection();

app.MapControllers();

app.Services.RunMigration<LimedikaDataContext>();

app.Run();
