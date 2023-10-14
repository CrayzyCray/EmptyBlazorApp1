using EmptyBlazorApp1.Entities;
using System.Security.Cryptography;
using System.Text;

namespace EmptyBlazorApp1.Services;

public class AccountService {
    AppDbContext _dbContext;
    SHA256 sha256 = SHA256.Create();

    const string UsernameNotFoundMessage = "Username not found";
    const string WrongPasswordMessage = "Wrong password";
    const string UserAlreadyUsedMessage = "User with this username already exists";
    const string PasswordTooShortMessage = "Password is too short";
    const string UsernameTooShortMessage = "Username is too short";

    public const int SaltLength = 8;
    public const int MinPasswordLength = 8;
    public const int MinUsernameLength = 8;

    public (User?, string) TryAuthorize(string username, string password) {
        if (username.Length < MinUsernameLength)
            return (null, UsernameTooShortMessage);
        if (password.Length < MinPasswordLength)
            return (null, PasswordTooShortMessage);

        User? user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
        if (user is null)
            return (null, UsernameNotFoundMessage);
        if (ValidatePassword(user, password))
            return (user, string.Empty);
        return (null, WrongPasswordMessage);
    }

    public (User?, string) TryRegister(string username, string password) {
        if (username.Length < MinUsernameLength)
            return (null, UsernameTooShortMessage);
        if (password.Length < MinPasswordLength)
            return (null, PasswordTooShortMessage);

        if (GetUser(username) is User)
            return (null, UserAlreadyUsedMessage);

        var salt = GenerateSalt();
        var passwordHash = GenerateSaltedHash(password, salt);

        User user = new(username, passwordHash, salt);
        _dbContext.Users.Add(user);
        _dbContext.SaveChanges();
        return (user, string.Empty);
    }

    private User? GetUser(string username) 
        => _dbContext.Users.FirstOrDefault(u => u.Username == username);

    public AccountService(DbService dbService) {
        _dbContext = dbService.DbContext;
    }

    string GenerateSalt() {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(SaltLength);
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