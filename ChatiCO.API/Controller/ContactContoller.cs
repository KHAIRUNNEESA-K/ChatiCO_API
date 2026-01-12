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
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet("available-users")]
        public async Task<IActionResult> GetAvailableUsers()
        {
            var result = await _contactService.GetAvailableUsersAsync();
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddContact([FromBody] AddContactRequestDto dto)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _contactService.AddContactAsync(currentUserId, dto);

            return Ok(result);
        }

        [HttpGet("my-contacts")]
        public async Task<IActionResult> GetMyContacts()
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _contactService.GetUserContactsAsync(currentUserId);

            return Ok(result);
        }

        [HttpDelete("delete/{contactId}")]
        public async Task<IActionResult> DeleteContact(int contactId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _contactService.DeleteContactAsync(contactId, currentUserId);

            return Ok(result);
        }

        [HttpPost("block/{contactId}")]
        public async Task<IActionResult> BlockContact(int contactId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _contactService.BlockContactAsync(currentUserId, contactId);
            return Ok(result);
        }

        [HttpPost("unblock/{contactId}")]
        public async Task<IActionResult> UnblockContact(int contactId)
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _contactService.UnblockContactAsync(currentUserId, contactId);
            return Ok(result);
        }

        [HttpGet("blocked-contacts")]
        public async Task<IActionResult> GetBlockedContacts()
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var result = await _contactService.GetBlockedContactsAsync(currentUserId);
            return Ok(result);
        }

        [HttpGet("all-other-users")]
        public async Task<IActionResult> GetAllOtherUsers()
        {
            int currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = await _contactService.GetAllOtherUsersAsync(currentUserId);
            return Ok(result);
        }
    }
}
