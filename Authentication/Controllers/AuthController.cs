using Authentication.Data;
using Authentication.DTOs;
using Authentication.Models;
using Authentication.Services;
using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthDbContext _db;
        private readonly IAuthService _auth;

        public AuthController(AuthDbContext db,IAuthService auth)
        {
            _db = db;
            _auth = auth;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if(await _db.Users.AnyAsync(u=>u.Username==dto.Username)) return BadRequest("User exists");
            _auth.CreatePasswordHash(dto.Password, out var hash, out var salt);
            var user = new User { Username=dto.Username,PasswordHash=hash,PasswordSalt=salt,Role="Admin"};
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token=_auth.CreateToken(user);
            return Ok(new UserDto { Username=user.Username,Token=token,Role=user.Role!});
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == dto.Username);
            if (user == null) return Unauthorized("Invalid credentials");

            if (!_auth.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt)) return Unauthorized("Invalid credentials");

            var token = _auth.CreateToken(user);
            return Ok(new UserDto { Username = user.Username, Token = token, Role = user.Role! });
        }
    }
}
