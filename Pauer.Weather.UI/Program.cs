using Pauer.Weather.Application;
using Pauer.Weather.Infrastructure;
using Pauer.Weather.UI.Components;

using Serilog;

try
{
    // Stage 1: Bootstrap logger to catch initialization errors
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.Console()
        .CreateBootstrapLogger();

    var builder = WebApplication.CreateBuilder(args);

    // Stage 2: Configure full logger from appsettings.json
    builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();
    builder.Services.AddApplicationServices(builder.Configuration);
    builder.Services.AddInfrastructureServices(builder.Configuration);

    var app = builder.Build();

    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    
    if (!app.Environment.IsDevelopment())
    {
        app.UseHsts();
    }

    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}