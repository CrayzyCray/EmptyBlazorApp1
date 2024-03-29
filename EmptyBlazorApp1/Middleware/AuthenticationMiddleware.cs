using System.Security.Claims;
using EmptyBlazorApp1.Entities;
using EmptyBlazorApp1.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Middleware;

public class AuthenticationMiddleware {
    private RequestDelegate _next;
    private AuthenticationService  _authenticationService;

    public const string SessionIdCode = "SessionId";

    public AuthenticationMiddleware(RequestDelegate next, AuthenticationService authenticationService) {
        _next           = next;
        _authenticationService = authenticationService;
    }

    public async Task InvokeAsync(HttpContext context, DbService dbService) {
        if (context.User.Identity is not null && 
            context.User.Identity.IsAuthenticated) {
            await _next(context);
            return;
        }

        var sessionId = context.Request.Cookies[SessionIdCode];

        if (sessionId is null) {
            await _next(context);
            return;
        }

        var success = _authenticationService.TryAuthorizeBySession(sessionId);

        if (!success) {
            context.Response.Cookies.Delete(SessionIdCode);
        }

        await _next.Invoke(context);
    }
}