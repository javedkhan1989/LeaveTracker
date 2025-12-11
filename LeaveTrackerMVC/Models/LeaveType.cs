using System.ComponentModel.DataAnnotations;

namespace LeaveTrackerMVC.Models
{
    public class LeaveType
    {

        public int LeaveTypeId { get; set; }
        [Required]
        public string TypeName { get; set; } = "";
        [Required]
        public int DefaultDays { get; set; }
    }
}
