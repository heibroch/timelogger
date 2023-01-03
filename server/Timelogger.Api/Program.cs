using Timelogger.Core.Interfaces;
using Timelogger.Infrastructure.MessageBus;
using Timelogger.Infrastructure.HealthMonitor;
using Timelogger.Infrastructure.LoginManager;
using Timelogger.Infrastructure.Logging;
using Timelogger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace Timelogger.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddInternalLogging();
        builder.Services.AddInternalMessageBus();
        builder.Services.AddHealthMonitoring();
        builder.Services.AddLoginService();
        builder.Services.AddSingleton<MySuperFakeApiContext>(); //Immitate a in-mem db (would use the other, but time is of the essence)
        builder.Services.AddTransient<ILogWorkRepository, LogWorkRepository>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "Project API", Version = "v1" });
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                  }
                }
            );
        });
        builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
        builder.Services.AddCors();

        var app = builder.Build();

        //Tiny hack to ensure services are running. We could also use hosted services
        app.Services.StartHealthMonitoring();
        app.Services.StartLoginService();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors(builder => builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials());
        }

        //app.UseHttpsRedirection();
        //app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
