using LeaveTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveTracker.Data
{
    public class LeaveDbContext:DbContext
    {
        public LeaveDbContext(DbContextOptions<LeaveDbContext> options):base(options)
        {
            
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<LeaveType> LeaveTypes { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        
    }
}
