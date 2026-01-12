using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatiCO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArchiveController : ControllerBase
    {
        private readonly IArchiveService _service;

        public ArchiveController(IArchiveService service)
        {
            _service = service;
        }

        private int GetCurrentUserId()
        {
            return int.Parse(
                User.FindFirst("userId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
            );
        }

        private string GetCurrentUsername()
        {
            return User.FindFirst("username")?.Value
                ?? User.FindFirst(ClaimTypes.Name)?.Value
                ?? "UnknownUser";
        }
        [HttpPost("add-to-archive")]
        public async Task<IActionResult> AddArchive([FromBody] ArchiveRequestDto dto)
        {
            int currentUserId = GetCurrentUserId();
            string currentUsername = GetCurrentUsername();

            var result = await _service.AddArchiveAsync(currentUserId, dto, currentUsername);
            return Ok(result);
        }
       
        [HttpDelete("remove-from-archive/{contactId}")]
        public async Task<IActionResult> RemoveArchive(int contactId)
        {
            int currentUserId = GetCurrentUserId();
            string currentUsername = GetCurrentUsername();

            var result = await _service.RemoveArchiveAsync(currentUserId, contactId);
            return Ok(result);
        }
        [HttpGet("get-archive")]
        public async Task<IActionResult> GetArchivedChats()
        {
            int currentUserId = GetCurrentUserId();
            var result = await _service.GetArchivedChatsAsync(currentUserId);

            return Ok(result);
        }
    }
}
