using LeaveTracker.Data;
using LeaveTracker.Models;
using LeaveTracker.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRequestsController : ControllerBase
    {

        private readonly LeaveDbContext _context;
        private readonly LeaveService _leaveService;

        public LeaveRequestsController(LeaveDbContext context, LeaveService leaveService)
        {
            _context = context;
            _leaveService = leaveService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var data = await _context.LeaveRequests
                .Include(e => e.Employee)
                .Include(e => e.LeaveType)
                .ToListAsync();

            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApplyLeave(LeaveRequest request)
        {
            bool available = await _leaveService.IsLeaveAvailable(request.EmployeeId, request.FromDate, request.ToDate);

            if (!available)
                return BadRequest("Not enough leave balance");

            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();
            return Ok(request);
        }

        [HttpPut("{id}/approve")]
        [Authorize(Roles = "Admin")]
        public IActionResult Approve(int id)
        {
            var req = _context.LeaveRequests.Find(id);
            if (req == null) return NotFound();

            req.Status = "Approved";
            _context.SaveChanges();

            return Ok(req);
        }

        [HttpPut("{id}/reject")]
        [Authorize(Roles = "Admin")]
        public IActionResult Reject(int id)
        {
            var req = _context.LeaveRequests.Find(id);
            if (req == null) return NotFound();

            req.Status = "Rejected";
            _context.SaveChanges();

            return Ok(req);
        }
    }
}
