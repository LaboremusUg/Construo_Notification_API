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
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "";
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .Enrich.FromLogContext()
    .CreateLogger();

Log.Information("Starting up Env:{Environment}", environment);
try
{

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;

    var connectionString = configuration.GetConnectionString("DefaultConnection");
    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
    builder.Services.AddHangfire(options => options.UsePostgreSqlStorage(connectionString));

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.CustomAddSwaggerGen(configuration);

    //builder.Services.AddSwaggerGen();

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

    var authenticationSettings = builder.Configuration.GetAuthServiceSettings();

    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = authenticationSettings.AuthorityUrl;
            options.Audience = authenticationSettings.ApiName;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };
        });

    builder.Services.AddHangfireServer();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddScoped<ISmsLogRepository, SmsLogRepository>();
    builder.Services.AddScoped<IServiceProviderRepository, ServiceProviderRepository>();
    builder.Services.AddScoped<IClientRepository, ClientRepository>();
    builder.Services.AddScoped<ISmsService, SmsService>();
    builder.Services.AddScoped<IClientRepository, ClientRepository>();
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

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    { }


    app.Use(async (context, next) =>
    {
        context.Response.Headers.Add("Pragma", "no-cache");
        context.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("X-Frame-Options", "DENY");
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        //context.Response.Headers.Add("Content-Security-Policy", "default - src 'self';");
        await next();
    });

    app.CustomUseSwagger(configuration);

    app.UseHttpsRedirection();
    app.UseCors("default");
    app.UseAuthorization();
    app.UseHangfireServer();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseHangfireDashboard("/queue", new DashboardOptions
    {
        Authorization = new IDashboardAuthorizationFilter[] { new HangfireAuthorizationFilter() }
    });
    app.MapControllers();

    var provider = app.Services.GetService<IServiceProvider>().CreateScope().ServiceProvider;

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