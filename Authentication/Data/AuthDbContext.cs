using Authentication.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Data
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

    }
}
