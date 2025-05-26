using Avalonia;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.IO;
using FileTidyDatabase;
using Microsoft.EntityFrameworkCore;
using FileTidyBase.Controller;
using FileTidyUI.ViewModels;

namespace FileTidyUI.Desktop;

class Program
{
    public static IServiceProvider ServiceProvider;

    [STAThread]
    public static void Main(string[] args)
    {
        var outputTemplate = "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message}{NewLine}in method {MemberName} at {FilePath}:{LineNumber}{NewLine}{Exception}{NewLine}";
        // Configure Serilog  
        Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Warning()
                    .Enrich.FromLogContext()
                    .WriteTo.File("log/{Date}.log", Serilog.Events.LogEventLevel.Warning, outputTemplate) 
                    .WriteTo.Console(Serilog.Events.LogEventLevel.Warning, outputTemplate, theme: AnsiConsoleTheme.Literate)
                    .CreateLogger();

        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            Log.Fatal(e.ExceptionObject as Exception, "Unhandled exception occurred");
        };

        try
        {
            Log.Information("Starting application");

            // Configure Dependency Injection  
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            AddTransients(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
            App.ServiceProvider = ServiceProvider;

            // After building the service provider
            using (var scope = ServiceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<FileTidyDbContext>();
                dbContext.Database.Migrate(); // Applies migrations and creates the database if needed
            }

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void AddTransients(ServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<SortFolderController>();
        serviceCollection.AddTransient<MainViewModel>();
        serviceCollection.AddTransient<SettingsController>();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        var dataDirectory = Path.Combine(AppContext.BaseDirectory, "data");
        Directory.CreateDirectory(dataDirectory);

        var dbPath = Path.Combine(dataDirectory, "data.db");
        var connectionString = $"Data Source={dbPath}";

        services.AddDbContext<FileTidyDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddSingleton<FileBaseController>();
        services.AddSingleton<SettingsController>();
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();



}
