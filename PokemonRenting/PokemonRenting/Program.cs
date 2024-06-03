﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using PokemonRenting.Models;
using PokemonRenting.Repositories;
using PokemonRenting.Repositories.DataSeeding;
using PokemonRenting.Repositories.Implementation;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Web.CustomMiddleWare;
using PokemonRenting.Web.Mapper;

namespace PokemonRenting.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<PokemonContext>(options =>
                    options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddDefaultTokenProviders()
                //.AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<PokemonContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
            builder.Services.AddScoped<IDbInitializer, DbInitializer>();




            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new PokemonProfile(builder.Environment));
            });
            var mapper = config.CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "PokemonProjectCookie";
                options.IdleTimeout = TimeSpan.FromMinutes(1);
                options.Cookie.HttpOnly = true;
            });

            builder.Services.AddRazorPages();
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
            });

        
            var app = builder.Build();




            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseMiddleware<ExceptionHandlerMiddleware>();
         
            app.UseStaticFiles();
            DataSeeding();
            app.UseSession();
            app.UseRouting();
          

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name:"areas",
                     pattern: "{area:exists}/{controller=Pokemon}/{action=Index}/{id?}"
                 );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapRazorPages();

            app.Run();
            void DataSeeding()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var DbInitial = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
                    DbInitial.Initialize();
                }
            }
        }

       
    }
}
