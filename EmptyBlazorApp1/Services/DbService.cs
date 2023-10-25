﻿using EmptyBlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Services;

public class DbService {
    public AppDbContext DbContext => _context;

    AppDbContext _context;

    public DbService() {
        _context = new();
    }

    public Session? GetSessionIncludeUser(string sessionId) {
        return QuerrySessionIncludeUserFromSessionId(_context, sessionId);
    }

    public Session? GetSession(string sessionId) {
        return QuerrySessionFromSessionId(_context, sessionId);
    }

    public User? GetUserIncludeUserProfile(string username) {
        return QuerryUserIncludeUserProfileFromName(_context, username);
    }

    public User? GetUser(string username) {
        return QuerryUserFromName(_context, username);
    }

    public void AddSessionUserUserProfile(Session session, User user, UserProfile userProfile) {
        _context.Users.Add(user);
        _context.Sessions.Add(session);
        _context.UserProfile.Add(userProfile);
        _context.SaveChanges();
    }

    public void AddSession(Session session) {
        _context.Sessions.Add(session);
        _context.SaveChanges();
    }
    
    public void LoadCommunities(User user) {
        _context.Entry(user).Collection(u => u.Communities).Load();
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