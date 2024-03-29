using System.Diagnostics;
using EmptyBlazorApp1.Middleware;
using EmptyBlazorApp1.Services;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MudBlazor.Services;
using Westwind.AspNetCore.LiveReload;

Console.InputEncoding = System.Text.Encoding.UTF8;
Console.OutputEncoding = System.Text.Encoding.UTF8;

var builder  = WebApplication.CreateBuilder(args);
var services = builder.Services;

//services.AddLiveReload();

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddMudServices();

services.AddHttpClient();

services.AddSingleton<DbService>();
services.AddHttpContextAccessor();
services.AddSingleton<AuthenticationService>();
services.AddScoped<AccountService>();
services.AddScoped<NavManager>();


var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseLiveReload();
//app.UseAuthentication()
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<AuthenticationMiddleware>();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();