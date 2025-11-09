using CarShareBLL.DTOs.AdminDTOs;
using CarShareBLL.Interfaces;
using CarShareBLL.DTOs.AdminDTOs;
using CarShareBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarSharePresentationLayer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("pending-owners")]
        public async Task<IActionResult> GetPendingOwners()
        {
            var owners = await _adminService.GetPendingCarOwnersAsync();
            return Ok(owners);
        }

        [HttpPost("approve-owner")]
        public async Task<IActionResult> ApproveOwner([FromBody] CarOwnerApprovalRequestDto request)
        {
            var result = await _adminService.ApproveOrRejectCarOwnerAsync(request);
            if (result == null)
                return NotFound(new { message = "Owner not found or invalid" });

            return Ok(result);
        }

        [HttpGet("all-owners")]
        public async Task<IActionResult> GetAllOwners()
        {
            var owners = await _adminService.GetAllCarOwnersAsync();
            return Ok(owners);
        }
    }
}
