using Sales.IntegrationEventLogEF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;

var configuration = GetConfiguration();

Log.Logger = CreateSerilogLogger(configuration);

try
{
    Log.Information("Configuring web host ({ApplicationContext})...", Program.AppName);
    var host = CreateHostBuilder(configuration, args);

    //host.MigrateDbContext<SalesContext>( (context, services) =>
    //{
    //     context.Database.EnsureCreated();
    //}).MigrateDbContext<IntegrationEventLogContext>( (context, services) =>
    //{
    //     context.Database.EnsureCreated();
    //});
    using (var context = new SalesContext(new DbContextOptions<SalesContext> {} ))
    {
        // Creates the database if not exists
        context.Database.EnsureCreated();
    }
    using (var context = new IntegrationEventLogContext(new DbContextOptions<IntegrationEventLogContext> { }))
    {
        // Creates the database if not exists
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }

    Log.Information("Starting web host ({ApplicationContext})...", Program.AppName);
    host.Run();

    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", Program.AppName);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

IWebHost CreateHostBuilder(IConfiguration configuration, string[] args)
{
    return WebHost.CreateDefaultBuilder(args)
      .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
      .CaptureStartupErrors(false)
      .ConfigureKestrel(options =>
      {
          options.Listen(IPAddress.Any, 80, listenOptions =>
          {
              listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
          });
      })
      .UseStartup<Startup>()
      .UseContentRoot(Directory.GetCurrentDirectory())
      .UseSerilog()
      .Build();
}

Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
{
    return new LoggerConfiguration()
        .MinimumLevel.Verbose()
        .Enrich.WithProperty("ApplicationContext", Program.AppName)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
}

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

public partial class Program
{
    public static string Namespace = typeof(Startup).Namespace;
    public static string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
}