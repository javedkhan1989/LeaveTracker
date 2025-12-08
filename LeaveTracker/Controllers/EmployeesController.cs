using LeaveTracker.Data;
using LeaveTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly LeaveDbContext _context;

        public EmployeesController(LeaveDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll()
        {
            return Ok(_context.Employees.ToList());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Employee e)
        {
            _context.Employees.Add(e);
            _context.SaveChanges();
            return Ok(e);
        }
    }
}
