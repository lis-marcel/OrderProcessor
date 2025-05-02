using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Service;

namespace OrderProcessor.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<DbStorage>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("AppDbConnection")));

            builder.Services.AddScoped<OrderService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowVueApp",
                builder =>
                {
                    builder.WithOrigins("https://localhost:8081", "https://127.0.0.1:8081",
                           "http://localhost:8081", "http://127.0.0.1:8081") // Replace with your Vue.js app URL
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials();
                });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            // Apply CORS middleware early in the pipeline
            app.UseCors("AllowVueApp");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}