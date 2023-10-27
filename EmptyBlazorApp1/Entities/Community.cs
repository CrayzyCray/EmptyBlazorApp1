namespace EmptyBlazorApp1.Entities;

public class Community {
    public int                     Id          { get; set; }
    public string                  Title       { get; set; } = string.Empty;
    public string                  Description { get; set; } = string.Empty;
    public List<User>              Members     { get; set; }
    public List<CommunityHashTags> HashTags    { get; set; }
    public User                    Creator     { get; set; }
    public int                     CreatorId   { get; set; }
}