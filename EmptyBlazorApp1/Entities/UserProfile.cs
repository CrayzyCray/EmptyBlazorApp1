namespace EmptyBlazorApp1.Entities;

public class UserProfile {
    public int       Id       { get; set; }
    public string?   Name     { get; set; }
    public DateOnly? Birthday { get; set; }
    public int       UserId   { get; set; }
    public User?     User     { get; set; }

    public UserProfile() {
    }

    public UserProfile(User user) {
        User = user;
    }
}