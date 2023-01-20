using AutoMapper;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
}).AddRazorRuntimeCompilation();


builder.Services.AddMvc();



builder.Services.AddHttpContextAccessor();
builder.Services.AddKendo();

builder.Services.Configure<JwtSettings>(builder.Configuration);
var jwtSettings = builder.Configuration.GetSection(typeof(JwtSettings).Name)
                                             .Get<JwtSettings>();

builder.Services.AddSingleton(jwtSettings);

builder.Services.AddHttpClient("", opt =>
{
    opt.BaseAddress = new Uri(builder.Configuration["BaseUrl"]);
});

var profiles = ProfileHelper.GetProfiles();
var configuration = new MapperConfiguration(opt => { opt.AddProfiles(profiles); });
builder.Services.AddSingleton(configuration.CreateMapper());

builder.Services.AddScoped<ICurrentUserService, BaseProject.BO.AuthorizationServices.CurrentUserService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

//builder.Services.Configure<CookieAuthenticationOptions>(options =>
//{
//    options.LoginPath = new PathString("/Account/Login");
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//app.UseExceptionHandler("/Home/Error");
app.UseDeveloperExceptionPage();
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();
//}

app.UseHttpsRedirection();
app.UseStaticFiles();
//var cookiePolicyOptions = new CookiePolicyOptions
//{
//    MinimumSameSitePolicy = SameSiteMode.Strict,
//};
//app.UseCookiePolicy(cookiePolicyOptions);

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
