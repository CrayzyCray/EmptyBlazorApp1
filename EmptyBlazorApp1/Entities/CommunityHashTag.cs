namespace EmptyBlazorApp1.Entities;

public class CommunityHashTag {
    public int             Id          { get; set; }
    public string          Name        { get; set; } = string.Empty;

    public override string ToString() {
        return Name;
    }
}