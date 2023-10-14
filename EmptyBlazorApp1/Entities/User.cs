namespace EmptyBlazorApp1.Entities;

public class User {
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public string Salt { get; set; } = string.Empty;
    public string? Name { get; set; }

    public User(string username, byte[] passwordHash, string salt) {
        Username = username;
        PasswordHash = passwordHash;
        Salt = salt;
    }
}