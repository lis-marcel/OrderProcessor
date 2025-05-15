using Microsoft.EntityFrameworkCore;
using OrderProcessor.BO;
using OrderProcessor.Service;
using OrderProcessor.Service.Auth;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

            // Configure JWT authentication
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddSingleton<TokenService>();
            builder.Services.AddScoped<AuthService>();

            // Add JWT authentication
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings!.SecretKey);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
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