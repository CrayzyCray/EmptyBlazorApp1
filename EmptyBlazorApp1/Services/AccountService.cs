using System.Security.Claims;
using EmptyBlazorApp1.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Services;

public class AccountService {
    readonly AppDbContext         _dbContext;
    readonly SHA256               sha256 = SHA256.Create();
    readonly IHttpContextAccessor _httpContextAccessor;

    const string UsernameNotFoundMessage = "Username not found";
    const string WrongPasswordMessage    = "Wrong password";
    const string UserAlreadyUsedMessage  = "User with this username already exists";
    const string PasswordTooShortMessage = "Password is too short";
    const string UsernameTooShortMessage = "Username is too short";

    public const int    SaltLength        = 8;
    public const int    SessionIdLength   = 64;
    public const int    MinPasswordLength = 8;
    public const int    MinUsernameLength = 6;
    public const string SessionIdCode     = "SessionId";
    
    public string? CurrentUserName => _httpContextAccessor.HttpContext.User.Identity.Name;

    public AccountService(DbService            dbService,
                          IHttpContextAccessor httpContextAccessor
    ) {
        _dbContext           = dbService.DbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthorized() {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public User? GetUserWithProfile() {
        var username = CurrentUserName;
        if (username is null) {
            return null;
        }
        return GetUserIncludeUserProfile(_dbContext, username);
    }

    public string GetCurrentUsername() {
        return _httpContextAccessor.HttpContext.User.Identity.Name;
    }

    public bool TryAuthorizeBySession(string sessionId) {
        var user = GetUser(sessionId);
        if (user is null) {
            return false;
        }

        Authorize(user);
        return true;
    }

    public (bool, string?) TryAuthorize(string username, string password) {
        if (username.Length < MinUsernameLength)
            return (false, UsernameTooShortMessage);
        if (password.Length < MinPasswordLength)
            return (false, PasswordTooShortMessage);

        var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user is null)
            return (false, UsernameNotFoundMessage);
        if (!ValidatePassword(user, password))
            return (false, WrongPasswordMessage);

        var session = CreateSession(user);
        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();

        Authorize(user);

        return (true, session.SessionId);
    }

    public (bool, string?) TryRegister(string username, string password) {
        if (username.Length < MinUsernameLength)
            return (false, UsernameTooShortMessage);
        if (password.Length < MinPasswordLength)
            return (false, PasswordTooShortMessage);

        if (GetUser(username) is User)
            return (false, UserAlreadyUsedMessage);

        var salt         = GenerateSalt();
        var passwordHash = GenerateSaltedHash(password, salt);

        User        user        = new(username, passwordHash, salt);
        Session     session     = CreateSession(user);
        UserProfile userProfile = new(user);

        _dbContext.Users.Add(user);
        _dbContext.Sessions.Add(session);
        _dbContext.UserProfile.Add(userProfile);
        _dbContext.SaveChanges();

        Authorize(user);

        return (true, session.SessionId);
    }

    User? GetUser(string sessionId) {
        var session = GetSessionIncludeUser(_dbContext, sessionId);
        return session?.User;
    }

    void Authorize(User user) {
        List<Claim> claims = new List<Claim> {
                                                 new(ClaimTypes.Name, user.Username)
                                             };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        _httpContextAccessor.HttpContext.User = new ClaimsPrincipal(claimsIdentity);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="user"></param>
    /// <returns>New SessionId</returns>
    Session CreateSession(User user) {
        var sessionId = GenerateRandomString(SessionIdLength);

        while (_dbContext.Sessions.FirstOrDefault(s => s.SessionId == sessionId) is not null) {
            sessionId = GenerateRandomString(SessionIdLength);
        }

        return new Session(user, sessionId);
    }

    string GenerateSalt() {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(SaltLength);
        return Convert.ToBase64String(randomBytes);
    }

    string GenerateRandomString(int bytes) {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(bytes);
        return Convert.ToBase64String(randomBytes);
    }

    byte[] GenerateSaltedHash(string password, string salt) {
        return sha256.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
    }

    bool ValidatePassword(User user, string password) {
        var hash = GenerateSaltedHash(password, user.Salt);
        return hash.SequenceEqual(user.PasswordHash);
    }


    static readonly Func<AppDbContext, string, Session?> GetSessionIncludeUser
        = EF.CompileQuery((AppDbContext context, string sessionId) =>
                              context.Sessions
                                     .Include(s => s.User)
                                     .FirstOrDefault(s => s.SessionId == sessionId));
    
    static readonly Func<AppDbContext, string, User?> GetUserIncludeUserProfile
        = EF.CompileQuery((AppDbContext context, string username) =>
                              context.Users
                                     .Include(s => s.UserProfile)
                                     .FirstOrDefault(s => s.Username == username));
}