using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatiCO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        private int GetUserId()
        {
            return int.Parse(
                User.FindFirst("userId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
            );
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromForm] GroupCreateDto dto)
        {
            try
            {
                var userId = GetUserId();
                if (userId == 0) return Unauthorized("Invalid user token");

                var group = await _groupService.CreateGroupAsync(dto, userId);
                return Ok(new { success = true, data = group });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("my-created-groups")]
        public async Task<IActionResult> GetMyCreatedGroups()
        {
            try
            {
                var userId = GetUserId();
                var groups = await _groupService.GetGroupsCreatedByUserAsync(userId);

                return Ok(new { success = true, data = groups });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("single/{groupId}")]
        public async Task<IActionResult> GetSingleGroup(int groupId)
        {
            try
            {
                var data = await _groupService.GetSingleGroupAsync(groupId);
                return Ok(new { success = true, data });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateGroup([FromForm] GroupUpdateDto dto)
        {
            var userId = GetUserId();
            var result = await _groupService.UpdateGroupAsync(dto, userId);
            return Ok(new { success = true, data = result });
        }


        [HttpPost("add-members")]
        public async Task<IActionResult> AddMembers(GroupMemberAddDto dto)
        {
            try
            {
                var userId = GetUserId();
                await _groupService.AddMembersAsync(dto,userId);
               
                var updated = await _groupService.GetSingleGroupAsync(dto.GroupId);
                return Ok(new { success = true, data = updated, message = "Members added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }


        [HttpGet("{groupId}/members")]
        public async Task<IActionResult> GetMembers(int groupId)
        {
            try
            {
                var members = await _groupService.GetMembersAsync(groupId);
                return Ok(new { success = true, data = members });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("{groupId}/messages")]
        public async Task<IActionResult> GetHistory(int groupId)
        {
            try
            {
                var history = await _groupService.GetChatHistoryAsync(groupId);
                return Ok(new { success = true, data = history });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage([FromForm] GroupMessageSendDto dto)
        {
            try
            {
                var result = await _groupService.SendMessageAsync(dto);
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("{groupId}")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            try
            {
                var userId = GetUserId();
                await _groupService.DeleteGroupAsync(groupId, userId);

                return Ok(new { success = true, message = "Group deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("my-groups")]
        public async Task<IActionResult> GetMyGroups()
        {
            try
            {
                var userId = GetUserId();
                var groups = await _groupService.GetGroupsByUserAsync(userId);
                return Ok(new { success = true, data = groups });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

    }
}
