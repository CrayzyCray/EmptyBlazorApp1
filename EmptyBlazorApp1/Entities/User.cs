namespace EmptyBlazorApp1.Entities;

public class User {
    public int     Id           { get; set; }
    public string  Username     { get; set; }
    public byte[]  PasswordHash { get; set; }
    public string  Salt         { get; set; }
    
    public User() { }

    public User(string username, byte[] passwordHash, string salt) {
        Username     = username;
        PasswordHash = passwordHash;
        Salt         = salt;
    }
}