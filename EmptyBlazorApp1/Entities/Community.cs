namespace EmptyBlazorApp1.Entities; 

public class Community {
    public int        Id          { get; set; }
    public string     Title       { get; set; } = string.Empty;
    public string     Description { get; set; } = string.Empty;
    public int        Rating      { get; set; } = 0;
    public int        Subscribers { get; set; } = 0;
    public List<User> Members     { get; set; }
}