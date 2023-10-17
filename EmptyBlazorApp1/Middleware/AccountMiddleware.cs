using System.Security.Claims;
using EmptyBlazorApp1.Entities;
using EmptyBlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Middleware;

public class AccountMiddleware {
    private RequestDelegate _next;
    private DbService       _dbService;
    private AppDbContext    _dbContext => _dbService.DbContext;

    public const string SessionIdCode = "SessionId";

    static readonly Func<AppDbContext, string, Session?> GetSessionIncludeUser
        = EF.CompileQuery((AppDbContext context, string sessionId) =>
                              context.Sessions
                                     .Include(s => s.User)
                                     .FirstOrDefault(s => s.SessionId == sessionId));

    public AccountMiddleware(RequestDelegate next, DbService dbService) {
        _next      = next;
        _dbService = dbService;
    }

    public async Task InvokeAsync(HttpContext context, DbService dbService) {
        if (context.User.Identity.IsAuthenticated)
            TrySetSessionId(context);
        else
            TryCookieAuthenticate(context);

        await _next.Invoke(context);
    }

    void TrySetSessionId(HttpContext context) {
        var cookies = context.Request.Cookies;
        if (cookies.ContainsKey(SessionIdCode))
            return;
        if (context.Items[SessionIdCode] is string sessionId)
            context.Response.Cookies.Append(SessionIdCode, sessionId);
    }

    bool TryCookieAuthenticate(HttpContext context) {
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