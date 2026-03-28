using cis.Models.Rest;
using CISApps.Models.Rest;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IApi, Api>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Index";        // ��鹷ҧ˹�� Login
        options.LogoutPath = "/Home/Logout";      // ��鹷ҧ˹�� Logout
        options.AccessDeniedPath = "/Home/Error"; // ˹�� Access Denied
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // ���� session
        options.SlidingExpiration = true;
    });

// ���� Authorization
builder.Services.AddAuthorization();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://web-app.bora.dopa.go.th/meetrens");
    client.Timeout = TimeSpan.FromSeconds(30);
});
var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
