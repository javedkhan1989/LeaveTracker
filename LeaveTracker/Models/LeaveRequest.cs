using System.Reflection.Metadata.Ecma335;

namespace LeaveTracker.Models
{
    public class LeaveRequest
    {
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public int LeaveTypeId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        public string Reason { get; set; } = "";
        public string Status { get; set; } = "Pending";

        public Employee? Employee { get; set; }
        public LeaveType? LeaveType { get; set; }
    }
}
