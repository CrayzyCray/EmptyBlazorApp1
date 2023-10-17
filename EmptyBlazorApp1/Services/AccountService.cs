using System.Security.Claims;
using EmptyBlazorApp1.Entities;
using System.Security.Cryptography;
using System.Text;
using EmptyBlazorApp1.Middleware;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

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

    public const int SaltLength        = 8;
    public const int SessionIdLength   = 64;
    public const int MinPasswordLength = 8;
    public const int MinUsernameLength = 6;

    public bool IsAuthorized() {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    public (bool, string?) TryAuthorize(string username, string password) {
        if (username.Length < MinUsernameLength)
            return (false, UsernameTooShortMessage);
        if (password.Length < MinPasswordLength)
            return (false, PasswordTooShortMessage);

        User? user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user is null)
            return (false, UsernameNotFoundMessage);
        if (!ValidatePassword(user, password))
            return (false, WrongPasswordMessage);
        
        Session session = CreateSession(user);
        _dbContext.SaveChanges();
        
        _httpContextAccessor.HttpContext.Items.Add(AccountMiddleware.SessionIdCode, session.SessionId);

        return (true, null);
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

        User    user    = new(username, passwordHash, salt);
        Session session = CreateSession(user);


        _dbContext.Users.Add(user);
        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();

        _httpContextAccessor.HttpContext.Items.Add(AccountMiddleware.SessionIdCode, session.SessionId);

        return (true, null);
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

        var session = new Session(user, sessionId);
        _dbContext.Sessions.Add(session);
        _dbContext.SaveChanges();
        return session;
    }

    private User? GetUser(string username)
        => _dbContext.Users.FirstOrDefault(u => u.Username == username);

    public AccountService(DbService            dbService,
                          IHttpContextAccessor httpContextAccessor
    ) {
        _dbContext           = dbService.DbContext;
        _httpContextAccessor = httpContextAccessor;
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
}