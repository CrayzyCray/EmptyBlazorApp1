using EmptyBlazorApp1.Middleware;
using EmptyBlazorApp1.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;

var builder  = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddMudServices();

services.AddHttpClient();

services.AddSingleton<DbService>();
services.AddHttpContextAccessor();
services.AddSingleton<AccountService>();
services.AddScoped<ProtectedLocalStorage>();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseAuthentication()
//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<AccountMiddleware>();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();