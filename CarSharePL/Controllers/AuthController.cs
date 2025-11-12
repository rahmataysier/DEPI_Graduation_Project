using Microsoft.AspNetCore.Mvc;
using CarShareBLL.Services;
using CarShareBLL.DTOs;

namespace CarSharePL.Controllers.API
{
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }

            [HttpPost("register")]
            public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterDto registerDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid data"
                    });
                }

                var result = await _authService.RegisterAsync(registerDto);
                return result.Success ? Ok(result) : BadRequest(result);
            }

            [HttpPost("login")]
            public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid data"
                    });
                }

                var result = await _authService.LoginAsync(loginDto);

                if (!result.Success)
                {
                    return Unauthorized(result);
                }

                HttpContext.Session.SetInt32("UserId", result.User.Id);
                HttpContext.Session.SetString("UserRole", result.User.Role);

                return Ok(result);
            }

            [HttpPost("logout")]
            public async Task<ActionResult> Logout()
            {
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId.HasValue)
                {
                    await _authService.LogoutAsync(userId.Value);
                }

                HttpContext.Session.Clear();
                return Ok(new { success = true, message = "Logged out" });
            }
        }
    }