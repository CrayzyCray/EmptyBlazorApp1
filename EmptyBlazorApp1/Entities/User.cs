namespace EmptyBlazorApp1.Entities;

public class User {
    public int Id { get; set; }
    public string LoginCode { get; set; } = string.Empty;
    public string? Name { get; set; }
}