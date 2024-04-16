using Blogger.EFCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Helpers.Logging;
using Microsoft.AspNetCore.Identity;
using Helpers.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ValidationService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        //options.Cookie.Name = "MyUserSessionCookie";
        options.LoginPath = new PathString("/Auth/Account/SignIn"); //When the user is not authenticated, they will be redirected to this page.
        options.AccessDeniedPath = "/AccessDenied";    //If user is denied access to method and they try to accees it, they will be redirected to this page.
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(Convert.ToInt64(builder.Configuration.GetSection("Session Time").Value));
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

ILoggerFactory? loggerFactory = app.Services.GetService<ILoggerFactory>();
IConfiguration? configuration = app.Services.GetService<IConfiguration>();
IWebHostEnvironment? env = app.Services.GetService<IWebHostEnvironment>();
loggerFactory.AddFileLogger(app.Services.GetService<IHttpContextAccessor>(), configuration, env, null);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}"
// pattern: "{area=Auth}/{controller=Auth}/{action=Index}/{id?}"
);

app.Run();
