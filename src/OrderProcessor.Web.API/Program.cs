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

            builder.Services.AddScoped<OrderCreationService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", // Allow specific origin
                builder =>
                {
                    builder.WithOrigins("http://localhost:8080")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
