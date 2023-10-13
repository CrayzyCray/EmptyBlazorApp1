﻿using EmptyBlazorApp1.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmptyBlazorApp1.Services {
    public class DbService {
        public AppDbContext DbContext => _context;

        AppDbContext _context;

        public DbService() {
            _context = new();
        }
    }
}
