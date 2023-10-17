using System.Security.Claims;
using EmptyBlazorApp1.Entities;
using EmptyBlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Middleware;

public class AccountMiddleware {
    private RequestDelegate _next;
    private AccountService  _accountService;

    public const string SessionIdCode = "SessionId";

    public AccountMiddleware(RequestDelegate next, AccountService accountService) {
        _next           = next;
        _accountService = accountService;
    }

    public async Task InvokeAsync(HttpContext context, DbService dbService) {
        if (context.User.Identity.IsAuthenticated) {
            await _next(context);
            return;
        }

        var sessionId = context.Request.Cookies[SessionIdCode];

        if (sessionId is null) {
            await _next(context);
            return;
        }

        var success = _accountService.TryAuthorizeBySession(sessionId);

        if (!success) {
            context.Response.Cookies.Delete(SessionIdCode);
        }

        await _next.Invoke(context);
    }
}