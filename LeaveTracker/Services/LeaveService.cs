using LeaveTracker.Data;

namespace LeaveTracker.Services
{
    public class LeaveService
    {
        private readonly LeaveDbContext _context;

        public LeaveService(LeaveDbContext context)
        {
            _context = context;
        }
        public async Task<bool> IsLeaveAvailable(int empId, DateTime from, DateTime to)
        {
            var days = (to - from).TotalDays + 1;

            // You can implement balance logic here.

            return days <= 12; // Example rule
        }
    }
}
