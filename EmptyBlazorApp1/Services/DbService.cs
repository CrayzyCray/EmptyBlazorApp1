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

    public void AddCommunity(Community community) {
        lock (_lock) {
            _context.Communities.Add(community);
            _context.SaveChanges();
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

    public List<Community> GetCommunities() {
        lock (_lock)
            return _context.Communities.ToList();
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

    public void LoadCreatedCommunities(User user) {
        lock (_lock) {
            _context.Entry(user).Collection(u => u.CreatedCommunities!).Load();
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
}