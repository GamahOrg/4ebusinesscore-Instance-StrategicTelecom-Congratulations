using _4EPlatForm.Infrastructure.Context;
using _4EPlatform_StrategicTelecom_Medals_Api.GrpClientsConfig;
using Congrats.API.Extension;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Rega.Qimah.PermessionGroup;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddUnitOfWorkRepository();
builder.Services.AddDiServices();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 7235, o => o.Protocols = HttpProtocols.Http2);

    // ADDED THIS LINE to fix the problem
    options.Listen(IPAddress.Any, 5245, o => o.Protocols = HttpProtocols.Http1);

});

builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseLazyLoadingProxies().UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(true);
});
builder.Services.AddLocalizationOptions();
Log.Logger = new LoggerConfiguration()
      .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
      {
          AutoRegisterTemplate = true,
          AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
      }).CreateLogger();
builder.Host.UseSerilog();
builder.Services.AddControllers();

var logger = new LoggerConfiguration()
          .ReadFrom.Configuration(builder.Configuration)
          .Enrich.FromLogContext()
          .CreateLogger();
builder.Host.UseSerilog(logger);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "4EPlatform-StrategicTelecom-Congratulations APIs Reference",
    });

    c.AddSecurityDefinition(
    "token",
    new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Name = HeaderNames.Authorization
    }
        );
    c.AddSecurityRequirement(
    new OpenApiSecurityRequirement
    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "token"
                            },
                        },
                        Array.Empty<string>()
                    }
    }
        );
});
#endregion
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddUserManagementGrpcClients(builder.Configuration["GrpcServiceSettings:UserManagementUrl"]);
builder.Services.AddHostedService<AddPermissionsMiddleware>();
builder.Services.AddScoped<PermissionsGroupingCheckerMiddleware, PermissionsGroupingCheckerMiddleware>();
//builder.Services.AddRateLimitings(builder.Configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowCors",
        builder =>
        {
            builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:3000")
            .SetIsOriginAllowed((host) => true)
            .AllowCredentials();
        });
});

builder.Services.AddVersioning();

builder.Services.AddAuthentication("Bearer")
      .AddJwtBearer("Bearer", options =>
      {
          // the name of your api resources   
          options.Audience = "UserManagementServer";
          /// identity server url                    
          options.Authority = builder.Configuration.GetValue<string>("IdentityUrl");
          options.RequireHttpsMetadata = false;
      });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ResourceOwnerPolicy", policy =>
    {
        policy.AddAuthenticationSchemes("Bearer");
        policy.RequireClaim("scope", "4EMicroServices"); // Require scope associated with Resource Owner Password token
    });

    options.AddPolicy("ClientCredentialsPolicy", policy =>
    {
        policy.AddAuthenticationSchemes("Bearer");
        policy.RequireClaim("scope", "CPAggregatorAPI"); // Require scope associated with Client Credentials token
    });
});

builder.Services.AddGrpc();

var app = builder.Build();
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Files")),
//    RequestPath = "/Upload"
//});
//var uploadPath = Path.Combine(app.Environment.WebRootPath, "Uploads");
//
//if (!Directory.Exists(uploadPath))
//{
//    Directory.CreateDirectory(uploadPath);
//}
// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDBContext>();
    var serviceProvider = serviceScope.ServiceProvider;
    if (!serviceScope.ServiceProvider.GetService<AppDBContext>().AllMigrationsApplied())
    {
        serviceScope.ServiceProvider.GetService<AppDBContext>().Migrate();
    }
    ContextSeed.Seed(dbContext, serviceProvider,builder.Configuration);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
    RequestPath = "/wwwroot"
});
//app.MapGrpcReflectionService();
#region Localization Middleware
var options = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(options.Value);
#endregion
app.UseCors("AllowCors");
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandler>();
app.MapControllers();
app.UseSerilogRequestLogging();
app.AddMapGRPCServicesApp();
//app.UseRateLimiter();
//app.MapGrpcService<UnavalibleCourseService>();
app.UseMiddleware<PermissionsGroupingCheckerMiddleware>();
app.Run();
