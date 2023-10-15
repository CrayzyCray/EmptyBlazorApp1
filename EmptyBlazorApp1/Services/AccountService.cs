using System.Security.Claims;
using EmptyBlazorApp1.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EmptyBlazorApp1.Services;

public class AccountService {
    AppDbContext                 _dbContext;
    SHA256                       sha256 = SHA256.Create();
    private IHttpContextAccessor _httpContextAccessor;

    const string UsernameNotFoundMessage = "Username not found";
    const string WrongPasswordMessage    = "Wrong password";
    const string UserAlreadyUsedMessage  = "User with this username already exists";
    const string PasswordTooShortMessage = "Password is too short";
    const string UsernameTooShortMessage = "Username is too short";

    public const int SaltLength        = 8;
    public const int SessionIdLength   = 64;
    public const int MinPasswordLength = 8;
    public const int MinUsernameLength = 6;

    public bool IsAuthorized(IHttpContextAccessor accessor) {
        return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
    }

    void SetClaimsCookie(User user) {
        var claims = new List<Claim> {
                                         new(ClaimTypes.Name, user.Username),
                                         new(ClaimTypes.NameIdentifier, user.Id.ToString())
                                     };
        _httpContextAccessor.HttpContext.User =
            new ClaimsPrincipal(new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme));
    }

    public (User?, string?) TryAuthorize(string username, string password) {
        if (username.Length < MinUsernameLength)
            return (null, UsernameTooShortMessage);
        if (password.Length < MinPasswordLength)
            return (null, PasswordTooShortMessage);

        User? user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user is null)
            return (null, UsernameNotFoundMessage);
        if (ValidatePassword(user, password)) {
            SetClaimsCookie(user);
            return (user, null);
        }

        return (null, WrongPasswordMessage);
    }

    public (User?, string?) TryRegister(string username, string password) {
        if (username.Length < MinUsernameLength)
            return (null, UsernameTooShortMessage);
        if (password.Length < MinPasswordLength)
            return (null, PasswordTooShortMessage);

        if (GetUser(username) is User)
            return (null, UserAlreadyUsedMessage);

        var salt         = GenerateSalt();
        var passwordHash = GenerateSaltedHash(password, salt);

        User user = new(username, passwordHash, salt);
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        SetClaimsCookie(user);
        return (user, null);
    }

    private User? GetUser(string username)
        => _dbContext.Users.FirstOrDefault(u => u.Username == username);

    public AccountService(DbService dbService, IHttpContextAccessor httpContextAccessor) {
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