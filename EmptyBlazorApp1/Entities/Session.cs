namespace EmptyBlazorApp1.Entities;

public class Session {
    public int      Id        { get; set; }
    public string   SessionId { get; set; }
    public User     User      { get; set; }
    public DateTime CreatedAt { get; set; }

    public Session() { }

    public Session(User user, string sessionId) {
        CreatedAt = DateTime.Now;
        User      = user;
        SessionId = sessionId;
    }
}