using System.Text;
using ControllerDeAcesso.Authorization;
using ControllerDeAcesso.Data;
using ControllerDeAcesso.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ControllerDeAcesso
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var getConnectionString = builder.Configuration["ConnectionStrings:UserConnetion"];

            builder.Services.AddDbContext<UserDbContext>(opts =>
            {
                opts.UseMySql(getConnectionString,
                ServerVersion.AutoDetect(getConnectionString));
            });

            builder.Services.AddIdentity<UserModel, IdentityRole>()
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<TokenService>();


            builder.Services.AddAuthentication(opts =>
            {
                 opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             }).AddJwtBearer(opts =>
             {
                 opts.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SymmetricSecurityKey"])),
                     ValidateAudience = false,
                     ValidateIssuer = false,
                     ClockSkew = TimeSpan.Zero
                 };
             });

            builder.Services.AddAuthorization(opts =>
            {
                opts.AddPolicy("IdadeMinima", policy => policy.AddRequirements(new IdadeMinima(18)));
            });

            builder.Services.AddSingleton<IAuthorizationHandler, IdadeAuthorization>();





            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}