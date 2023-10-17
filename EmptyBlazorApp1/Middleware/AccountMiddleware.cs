using System.Security.Claims;
using EmptyBlazorApp1.Entities;
using EmptyBlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Middleware;

public class AccountMiddleware {
    private RequestDelegate _next;
    private DbService       _dbService;
    private AppDbContext    _dbContext => _dbService.DbContext;
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    public const string SessionIdCode = "SessionId";

    static readonly Func<AppDbContext, string, Session?> GetSessionIncludeUser
        = EF.CompileQuery((AppDbContext context, string sessionId) =>
                              context.Sessions
                                     .Include(s => s.User)
                                     .FirstOrDefault(s => s.SessionId == sessionId));

    public AccountMiddleware(RequestDelegate next, DbService dbService, ProtectedLocalStorage protectedLocalStorage) {
        _next      = next;
        _dbService = dbService;
        _protectedLocalStorage = protectedLocalStorage;
    }

    private          int                   num = 0;

    public async Task InvokeAsync(HttpContext context, DbService dbService) {
        if (context.User.Identity.IsAuthenticated || context.Items.ContainsKey(SessionIdCode)) {
            Console.WriteLine("TrySetSessionId" + num++);
            TrySetSessionId(context);
        }
        else {
            Console.WriteLine("TryCookieAuthenticate" + num++);
            await TryCookieAuthenticate(context);
        }

        await _next.Invoke(context);
    }

    async Task TrySetSessionId(HttpContext context) {
        var data    = await _protectedLocalStorage.GetAsync<string>(SessionIdCode);
        if (data.Value is null)
            return;
        if (context.Items[SessionIdCode] is string sessionId)
            context.Response.Cookies.Append(SessionIdCode, sessionId);
    }

    async Task<bool> TryCookieAuthenticate(HttpContext context) {
        var requestCookies  = context.Request.Cookies;
        var responseCookies = context.Response.Cookies;

        if (!requestCookies.ContainsKey(SessionIdCode)) {
            return false;
        }

        var sessionId = requestCookies[SessionIdCode];

        if (sessionId is null) return false;

        var session = GetSessionIncludeUser(_dbContext, sessionId);

        if (session is null) {
            responseCookies.Delete(SessionIdCode);
            return false;
        }

        var user = session.User;
        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme,
                                                user.Username,
                                                null);
        context.User = new ClaimsPrincipal(claimsIdentity);
        return true;
    }
}