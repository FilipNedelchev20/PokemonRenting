using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokemonRenting.Repositories;
using PokemonRenting.Repositories.Implementation;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Web.CustomMiddleWare;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PokemonContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PokemonContext>();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddAutoMapper(typeof(PokemonRepository));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pokemon}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
