using EmptyBlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Services;

public class DbService {
    public AppDbContext DbContext => _context;

    AppDbContext _context;
    object _lock = new();

    public DbService() {
        _context = new();
    }

    public int GetSubscribersCount(Community community) {
        lock (_lock) {
            LoadSubscribers(community);
        }
        return community.Members!.Count;
    }

    public void AddCommunity(Community community) {
        lock (_lock) {
            _context.Communities.Add(community);
            _context.SaveChanges();
        }
    }
    
    public List<CommunityHashTag> GetAvailableCommunityHashTags() {
        lock (_lock) {
            //_context.HashTags.Load();
            return _context.HashTags.ToList();
        }
    }

    public List<Community> GetSubscriptionCommunities(User user) {
        lock (_lock)
            LoadCommunities(user);
        return user.Communities!;
    }

    public List<Community> GetCreatedCommunities(User user) {
        lock (_lock)
            LoadCreatedCommunities(user);
        return user.CreatedCommunities!;
    }

    public bool IsUserSubscribed(User user, Community community) {
        lock (_lock) {
            LoadCommunities(user);
            return user.Communities!.Any(c => c.Id == community.Id);
        }
    }

    public void Unsubscribe(User user, Community community) {
        lock (_lock) {
            if (user.Communities is null) {
                LoadCommunities(user);
            }
            user.Communities!.Remove(community);
            _context.SaveChanges();
        }
    }

    public void Subscribe(User user, Community community) {
        lock (_lock) {
            if (user.Communities is null) {
                LoadCommunities(user);
            }
            user.Communities!.Add(community);
            _context.SaveChanges();
        }
    }

    public List<Community> GetCommunities() {
        lock (_lock)
            return _context.Communities.ToList();
    }
    
public List<Community> GetCommunities(List<CommunityHashTag> tags) {
    lock (_lock) {
        //var posts = context.Posts.Where(p => tags.All(t => p.Tags.Contains(t)
        return _context.Communities.Where(c => c.HashTags.Any(t => tags.Contains(t))).ToList();
    }
        
}

public List<Community> GetCommunities(User user, List<CommunityHashTag> tags) {
    lock (_lock) {
        if (user.Communities is null) {
            LoadCommunities(user);
        }
        var t = user.Communities!.Where(c => c.HashTags.Any(t => tags.Contains(t)));
        return t.ToList();
    }
}

    public Session? GetSessionIncludeUser(string sessionId) {
        lock (_lock)
            return QuerrySessionIncludeUserFromSessionId(_context, sessionId);
    }

    public Session? GetSession(string sessionId) {
        lock (_lock)
            return QuerrySessionFromSessionId(_context, sessionId);
    }

    public User? GetUserIncludeUserProfile(string username) {
        lock (_lock)
            return QuerryUserIncludeUserProfileFromName(_context, username);
    }

    public User? GetUser(string username) {
        lock (_lock)
            return QuerryUserFromName(_context, username);
    }

    public void AddSessionUserUserProfile(Session session, User user, UserProfile userProfile) {
        lock (_lock) {
            _context.Users.Add(user);
            _context.Sessions.Add(session);
            _context.UserProfile.Add(userProfile);
            _context.SaveChanges();
        }
    }

    public void AddSession(Session session) {
        lock (_lock) {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }
    }
    
    public void LoadCommunities(User user) {
        lock ( _lock) {
            _context.Entry(user).Collection(u => u.Communities!).Load();
        }
    }

    public void LoadSubscribers(Community community) {
        lock (_lock) {
            _context.Entry(community).Collection(c => c.Members!).Load();
        }
    }

    public void LoadCreatedCommunities(User user) {
        lock (_lock) {
            _context.Entry(user).Collection(u => u.CreatedCommunities!).Load();
        }
    }

    public void LoadCreator(Community community) {
        lock (_lock) {
            _context.Entry(community).Reference(u => u.Creator).Load();
        }
    }

    static readonly Func<AppDbContext, string, Session?> QuerrySessionIncludeUserFromSessionId
        = EF.CompileQuery((AppDbContext context, string sessionId) =>
                              context.Sessions
                                     .Include(s => s.User)
                                     .FirstOrDefault(s => s.SessionId == sessionId));

    static readonly Func<AppDbContext, string, User?> QuerryUserIncludeUserProfileFromName
        = EF.CompileQuery((AppDbContext context, string username) =>
                              context.Users
                                     .Include(s => s.UserProfile)
                                     .FirstOrDefault(s => s.Username == username));

    static readonly Func<AppDbContext, string, User?> QuerryUserFromName
        = EF.CompileQuery((AppDbContext context, string username) =>
                              context.Users
                                     .FirstOrDefault(s => s.Username == username));

    static readonly Func<AppDbContext, string, Session?> QuerrySessionFromSessionId
        = EF.CompileQuery((AppDbContext context, string sessionId) =>
                              context.Sessions
                                     .FirstOrDefault(s => s.SessionId == sessionId));

    public List<CommunityHashTag> GetTags(Community community) {
        lock (_lock) {
            LoadTags(community);
            return community.HashTags!;
        }
    }

    public void LoadTags(Community community) {
        lock (_lock) {
            _context.Entry(community).Collection(c => c.HashTags!).Load();
        }
    }

    public void UpdateCommunity(Community community) {
        lock (_lock) {
            _context.SaveChanges();
        }
    }
}