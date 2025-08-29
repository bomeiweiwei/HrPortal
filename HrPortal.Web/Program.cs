using HrPortal.EF.Data;
using HrPortal.IoC;
using HrPortal.Shared.SysConfigs;
using HrPortal.Web.SeedData;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 讀取 MasterConnection
//builder.Services.AddDbContext<PersonSystemContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("MasterConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// 啟用 Cookie Authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Identity/Login";
        opt.LogoutPath = "/Identity/Logout";
        opt.AccessDeniedPath = "/Identity/Denied";
        opt.Cookie.Name = "hr.auth";                 // 自訂 Cookie 名稱
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.Always; // https 環境請保留 Always
        opt.SlidingExpiration = true;
        opt.ExpireTimeSpan = TimeSpan.FromHours(8);
    });
builder.Services.AddAuthorization();

// 初始化設定
ConfigurationManager configuration = builder.Configuration;
ConfigManager.Initial(configuration);
// 管理注入
builder.Services.RegisterService(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    //using (var scope = app.Services.CreateScope())
    //{
    //    var context = scope.ServiceProvider.GetRequiredService<PersonSystemContext>();
    //    DbInitializer.Seed(context);
    //}
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

