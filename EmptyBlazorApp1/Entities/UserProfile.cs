namespace EmptyBlazorApp1.Entities;

public class UserProfile {
    public int       Id                { get; set; }
    public string?   Name              { get; set; }
    public DateOnly? Birthday          { get; set; }
    public string?   University        { get; set; }
    public string?   Course            { get; set; }
    public string?   Group             { get; set; }
    public string?   PhoneNumber       { get; set; }
    public string?   SocialNetworkLink { get; set; }
    public string?   Email             { get; set; }

    public int   UserId { get; set; }
    public User? User   { get; set; }

    public UserProfile() {
    }

    public UserProfile(User user) {
        User = user;
    }
}