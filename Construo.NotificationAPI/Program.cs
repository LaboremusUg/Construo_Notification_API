using Construo.NotificationAPI.Configs;
using Construo.NotificationAPI.Core.Extensions;
using Construo.NotificationAPI.Core.Filters;
using Construo.NotificationAPI.Data;
using Construo.NotificationAPI.Models.Sms;
using Construo.NotificationAPI.Repository;
using Construo.NotificationAPI.Services;
using Construo.NotificationAPI.Validation;
using Construo.NotificationAPI.ViewModels;
using FluentValidation;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Information("Starting up Env:{Environment}", environment);

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    var configuration = builder.Configuration;

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    builder.Services.AddHangfire(options => options.UsePostgreSqlStorage(connectionString));

    builder.Services.AddLogging();
    builder.Logging.AddConsole();

    var authenticationSettings = configuration.GetAuthServiceSettings();

    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = authenticationSettings.AuthorityUrl;
            options.Audience = authenticationSettings.ApiName;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidTypes = new[] { "at+jwt", "JWT" },
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("Authentication failed: " + context.Exception.Message);
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine("Token validated successfully.");
                    return Task.CompletedTask;
                }
            };
        });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.CustomAddSwaggerGen(configuration);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("default", policy =>
        {
            policy.WithOrigins()
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });

    builder.Services.AddHangfireServer();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddScoped<ISmsLogRepository, SmsLogRepository>();
    builder.Services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
    builder.Services.AddScoped<IClientRepository, ClientRepository>();
    builder.Services.AddScoped<ISmsService, SmsService>();
    builder.Services.AddScoped<IEmailService, EmailService>();
    builder.Services.AddScoped<IMessageQueueRepository, MessageQueueRepository>();
    builder.Services.AddScoped<IMessageLoggingService, MessageLoggingService>();
    builder.Services.AddScoped<IExchangeMailService, ExchangeMailService>();
    builder.Services.AddScoped<IValidator<Sms>, SmsContext>();
    builder.Services.AddAutoMapper(config =>
    {
        config.CreateMap<Client, ClientViewModel>();
        config.CreateMap<ClientViewModel, Client>();
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.CustomUseSwagger(configuration);
    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.UseSerilogRequestLogging();
    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Pragma", "no-cache");
        context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        await next();
    });

    app.UseCors("default");
    app.UseHangfireServer();
    app.UseHangfireDashboard("/queue", new DashboardOptions
    {
        Authorization = new IDashboardAuthorizationFilter[] { new HangfireAuthorizationFilter() }
    });

    app.MapControllers();
    Seed.InitDatabase(app);
    app.UseFileServer();
    app.Run();
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}
