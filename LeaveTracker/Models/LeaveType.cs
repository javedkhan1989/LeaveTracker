using System.Reflection.Metadata.Ecma335;

namespace LeaveTracker.Models
{
    public class LeaveType
    {
        public int LeaveTypeId { get; set; }
        public string TypeName { get; set; } = "";
        public int DefaultDays { get; set; }
    }
}
