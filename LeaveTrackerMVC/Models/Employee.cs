using System.ComponentModel.DataAnnotations;

namespace LeaveTrackerMVC.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int ManagerId { get; set; }
    }
}
