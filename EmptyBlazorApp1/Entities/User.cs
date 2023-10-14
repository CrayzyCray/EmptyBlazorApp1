namespace EmptyBlazorApp1.Entities;

public class User {
    public int Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public byte[] Password { get; set; } = Array.Empty<byte>();
    public string Salt { get; set; } = string.Empty;
    public string? Name { get; set; }
}