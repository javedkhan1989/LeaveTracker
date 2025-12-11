using System.ComponentModel.DataAnnotations;

namespace LeaveTrackerMVC.Models
{
    public class LeaveRequest
    {
        public int? LeaveRequestId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        public int LeaveTypeId { get; set; }
        [Required]
        public DateTime FromDate { get; set; }
        [Required]
        public DateTime ToDate { get; set; }
        [Required]
        public string Reason { get; set; } = "";
        [Required]
        public string Status { get; set; } = "Pending";

        public Employee? Employee { get; set; }
        public LeaveType? LeaveType { get; set; }
    }
}
