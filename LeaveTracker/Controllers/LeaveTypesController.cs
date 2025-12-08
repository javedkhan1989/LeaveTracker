using LeaveTracker.Data;
using LeaveTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveTypesController : ControllerBase
    {
         private readonly LeaveDbContext _context;

        public LeaveTypesController(LeaveDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAll() => Ok(_context.LeaveTypes.ToList());

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(LeaveType type)
        {
            _context.LeaveTypes.Add(type);
            
            _context.SaveChanges();
            return Ok(type);
        }
    }
}
