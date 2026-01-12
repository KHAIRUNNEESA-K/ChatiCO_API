using ChatiCO.API.Hubs;
using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatiCO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _service;
        private readonly IHubContext<ChatHub> _chatHub;

        public MessageController(IMessageService service, IHubContext<ChatHub> chatHub)
        {
            _service = service;
            _chatHub = chatHub;
        }
        private int GetUserId()
        {
            return int.Parse(
                User.FindFirst("userId")?.Value ??
                User.FindFirst(ClaimTypes.NameIdentifier)?.Value!
            );
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromForm] MessageCreateRequestDto request)
        {
            int senderId = GetUserId();

            var result = await _service.SendMessageAsync(senderId, request);

            var success = (bool)result.GetType().GetProperty("success")!.GetValue(result)!;
            if (success)
            {
                var messageDto = (MessageDto)result.GetType().GetProperty("message")!.GetValue(result)!;

                // Send to receiver
                await _chatHub.Clients.User(messageDto.ReceiverId.ToString())
                    .SendAsync("ReceiveMessage", messageDto);

                // Send to sender
                await _chatHub.Clients.User(senderId.ToString())
                 .SendAsync("ReceiveMessage", messageDto);
            }

            return Ok(result);
        }



        [HttpGet("get-messages/{receiverId}")]
        public async Task<IActionResult> GetMessages(int receiverId)
        {
            int senderId = GetUserId();   
            var result = await _service.GetMessagesBetweenUsersAsync(senderId, receiverId);
            return Ok(result);
        }

        [HttpGet("last/{receiverId}")]
        public async Task<IActionResult> GetLastMessage(int receiverId)
        {
            int senderId = GetUserId();   
            var result = await _service.GetLastMessageAsync(senderId, receiverId);
            return Ok(result);
        }

        [HttpGet("unread/{contactId}")]
        public async Task<IActionResult> GetUnreadCount(int contactId)
        {
            int userId = GetUserId();  
            var result = await _service.GetUnreadMessagesCountAsync(userId, contactId);
            return Ok(result);
        }

        [HttpPut("read/{messageId}")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var result = await _service.MarkMessageAsReadAsync(messageId);
            return Ok(result);
        }

        [HttpPut("delivered/{messageId}")]
        public async Task<IActionResult> MarkAsDelivered(int messageId)
        {
            var result = await _service.MarkMessageAsDeliveredAsync(messageId);
            return Ok(result);
        }
    }
}
