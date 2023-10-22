namespace EmptyBlazorApp1.Entities;

public class UserProfile {
    public int       Id                { get; set; }
    public string    Name              { get; set; } = string.Empty;
    public DateTime? Birthday          { get; set; }
    public string    University        { get; set; } = string.Empty;
    public string    Course            { get; set; } = string.Empty;
    public string    Group             { get; set; } = string.Empty;
    public string    PhoneNumber       { get; set; } = string.Empty;
    public string    SocialNetworkLink { get; set; } = string.Empty;
    public string    Email             { get; set; } = string.Empty;

    public int   UserId { get; set; }
    public User? User   { get; set; }

    public UserProfile() {
    }

    public UserProfile(User user) {
        User = user;
    }
}