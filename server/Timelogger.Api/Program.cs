
using Microsoft.EntityFrameworkCore;
using Timelogger.Interfaces;

namespace Timelogger.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddSingleton<MySuperFakeApiContext>(); //Immitate a in-mem db (would use the other, but time is of the essence)
        builder.Services.AddTransient<ILogWorkRepository, LogWorkRepository>();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApiContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
        builder.Services.AddCors();

        var app = builder.Build();
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
