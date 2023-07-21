using GimenaCreations;
using GimenaCreations.Constants;
using GimenaCreations.Consumers;
using GimenaCreations.Data;
using GimenaCreations.Entities;
using GimenaCreations.Helpers;
using GimenaCreations.Permission;
using GimenaCreations.Services;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using StackExchange.Redis;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages(configure =>
{
    configure.Conventions.AuthorizeFolder("/Admin");
});

builder.Services.AddControllers();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});

//Configure other services up here
var multiplexer = ConnectionMultiplexer.Connect("localhost");
builder.Services.AddSingleton<IConnectionMultiplexer>(multiplexer);
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IFileHelper, FileHelper>();
builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddSignalR(options => options.EnableDetailedErrors = true).AddMessagePackProtocol();

builder.Services.AddHttpClient<WebhookNotificationConsumer>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("MercadoPago:BaseUrl"));
    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", builder.Configuration.GetValue<string>("MercadoPago:AccessToken"));
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    // By default, sagas are in-memory, but should be changed to a durable
    // saga repository.
    x.SetInMemorySagaRepositoryProvider();

    var entryAssembly = Assembly.GetEntryAssembly();

    x.AddConsumers(entryAssembly);
    x.AddSagaStateMachines(entryAssembly);
    x.AddSagas(entryAssembly);
    x.AddActivities(entryAssembly);

    x.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
});


builder.Services.AddHealthChecks()
    .AddCheck("Self", () => HealthCheckResult.Healthy("This web is healthy"), new string[] { "self" })
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), healthQuery: "SELECT 1", name: "sql", failureStatus: HealthStatus.Degraded,
        tags: new string[] { "db", "sql", "sqlserver" })
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddHealthChecksUI(setupSettings =>
{
    setupSettings.SetHeaderText("Health Checks Status");
    setupSettings.AddHealthCheckEndpoint("Web", "/healthz");
}).AddInMemoryStorage();

builder.Services.AddApplicationInsightsTelemetry(new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions
{
    // Disables adaptive sampling.
    EnableAdaptiveSampling = false,

    // Disables QuickPulse (Live Metrics stream).
    EnableQuickPulseMetricStream = false
});

builder.Services.AddHttpContextAccessor();
var app = builder.Build();
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await db.Database.MigrateAsync();

await new ApplicationDbContextSeed()
    .SeedAsync(db, app.Environment, scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContextSeed>>(), scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(),
    scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler(exceptionHandlerApp =>
    {
        exceptionHandlerApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            context.Response.ContentType = Text.Plain;

            await context.Response.WriteAsync("An exception was thrown.");

            var exceptionHandlerPathFeature =
                context.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
            {
                await context.Response.WriteAsync(" The file was not found.");
            }

            if (exceptionHandlerPathFeature?.Path == "/")
            {
                await context.Response.WriteAsync(" Page: Home.");
            }
        });
    });

    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultControllerRoute();
app.MapHealthChecks("/healthz", new HealthCheckOptions { Predicate = _ => true, ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = _ => true });

app.MapHealthChecksUI(setupOptions =>
{
    setupOptions.UIPath = "/hc-ui";
    setupOptions.AddCustomStylesheet("wwwroot/css/site.css");
}).RequireAuthorization(Permissions.HealthCheck.View);

app.MapRazorPages();
app.MapControllers();

app.MapHub<NotificationHub>("/orderstatus", options =>
{
    options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
});

app.Run();