namespace EmptyBlazorApp1.Entities;

public class Session {
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public User? User { get; set; }
    public DateTime CreatedAt { get; set; }
}