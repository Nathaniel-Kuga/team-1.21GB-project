using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.GPT3.Extensions;
using Team121GBCapstoneProject.Areas.Identity.Data;
using Team121GBCapstoneProject.Data;
using Team121GBCapstoneProject.Services.Abstract;
using Team121GBCapstoneProject.Services.Concrete;
using Team121GBCapstoneProject.DAL.Abstract;
using Team121GBCapstoneProject.DAL.Concrete;
using Team121GBCapstoneProject.Models;
using Team121GBCapstoneProject.Areas.Identity.Data;
using OpenAI.GPT3.Interfaces;
using System;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Http;
using Team121GBCapstoneProject.Services;


var builder = WebApplication.CreateBuilder(args);

var reCAPTCHASecretKey = builder.Configuration["GamingPlatform:reCAPTCHASecretKey"];
var DalleSecretKey = builder.Configuration["OpenAIServiceOptions:ApiKey"];
var SendGridKey = builder.Configuration["SendGridKey"];
var igdbApiClientIdKey = builder.Configuration["GamingPlatform:igdbClientId"];
var igdbApiBearerTokenKey = builder.Configuration["GamingPlatform:igdbBearerToken"];

builder.Services.AddHttpClient();
// Add services to the container.
builder.Services.AddScoped<IReCaptchaService, ReCaptchaService>(recaptcha => new ReCaptchaService(reCAPTCHASecretKey,
                                                                             new HttpClient()
                                                                             {
                                                                                 BaseAddress = new Uri("https://www.google.com/recaptcha/api/siteverify")
                                                                             }));

// Add Swagger middleware
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IIgdbService, IgdbService>();

var connectionString = builder.Configuration.GetConnectionString("AuthConnection") ?? throw new InvalidOperationException("Connection string 'AuthConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Allows for Razor page editing without needing to rebuild
//builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var GPconnectionString = builder.Configuration.GetConnectionString("GPConnection");
builder.Services.AddDbContext<GPDbContext>(options => options
                            .UseLazyLoadingProxies()    // Will use lazy loading, but not in LINQPad as it doesn't run Program.cs
                            .UseSqlServer(GPconnectionString));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); //Register all generic repositories
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonGameListRepository, PersonGameListRepository>();

builder.Services.AddSwaggerGen();

builder.Services.AddOpenAIService(settings =>
{
    settings.ApiKey = DalleSecretKey;
});
builder.Services.AddScoped<IOpenAIService, OpenAIService>();
builder.Services.AddScoped<IDalleService, DalleService>();

//var openAiService = builder.Services.BuildServiceProvider().GetRequiredService<IOpenAIService>();
//openAiService.SetDefaultModelId(Models.Davinci);


var app = builder.Build();


// Enable middleware to serve generated Swagger as a JSON endpoint.
app.UseSwagger();

// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GP API V1");
});


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

app.MapControllerRoute(
    name: "game",
    pattern: "api/Game/{query}",
    defaults: new { controller = "Game", action = "Index"}
);

app.MapRazorPages();

app.Run();