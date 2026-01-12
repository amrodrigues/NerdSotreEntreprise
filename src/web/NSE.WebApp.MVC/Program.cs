using Microsoft.AspNetCore.Authentication.Cookies;
using NSE.WebApp.MVC.Configuration;
using NSE.WebApp.MVC.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.RegisterServices();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login"; // Caminho da sua View de Login
        options.AccessDeniedPath = "/acesso-negado";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
 
//}
app.UseExceptionHandler("/erro/500");
app.UseStatusCodePagesWithRedirects("/erro/{0}");
app.UseHsts();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseIdentityConfiguration();



app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
