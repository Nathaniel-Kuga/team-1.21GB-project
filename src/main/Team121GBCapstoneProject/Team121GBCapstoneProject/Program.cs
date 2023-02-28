using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.Data;
using Team121GBCapstoneProject.Services;

using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;

using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

var reCAPTCHASecretKey = builder.Configuration["GamingPlatform:reCAPTCHASecretKey"];

// Add services to the container.
builder.Services.AddScoped<IReCaptchaService, ReCaptchaService>(recaptcha => new ReCaptchaService(reCAPTCHASecretKey,
                                                                             new HttpClient()
                                                                             { BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify")
                                                                             }));
var connectionString = builder.Configuration.GetConnectionString("AuthConnection") ?? throw new InvalidOperationException("Connection string 'AuthConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Allows for Razor page editing without needing to rebuild
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var GPconnectionString = builder.Configuration.GetConnectionString("GPConnection");
builder.Services.AddDbContext<GPDbContext>(options => options
                            .UseLazyLoadingProxies()    // Will use lazy loading, but not in LINQPad as it doesn't run Program.cs
                            .UseSqlServer(GPconnectionString));
builder.Services.AddScoped<IGameRepository, GameRepository>();


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

app.UseAuthorization();

// If program says "Index Not Found" run: dotnet watch run (only on VS 2022)

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();