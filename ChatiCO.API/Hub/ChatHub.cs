using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using ChatiCO.Application.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;
        private readonly IContactRepository _contactRepository;
        private readonly IUserRegistrationRepository _userRegistrationRepository;
        private readonly IPresenceService _presenceService;
        private readonly IGroupService _groupService;
        private readonly IGroupMemberRepository _groupMemberRepo;



        public ChatHub(
            IMessageService messageService,
            IContactRepository contactRepository,
            IUserRegistrationRepository userRegistrationRepository,
            IPresenceService presenceService,
            IGroupService groupService,
            IGroupMemberRepository groupMemberRepo)
        {
            _messageService = messageService;
            _contactRepository = contactRepository;
            _userRegistrationRepository = userRegistrationRepository;
            _presenceService = presenceService;
            _groupService = groupService;
            _groupMemberRepo = groupMemberRepo;
        }
        public async Task SendMessage(MessageCreateRequestDto request)
        {
            var senderIdStr = Context.UserIdentifier ?? Context.User?.FindFirst("UserId")?.Value;
            if (!int.TryParse(senderIdStr, out var senderId)) return;

            var result = await _messageService.SendMessageAsync(senderId, request);

            if ((bool)result.GetType().GetProperty("success")!.GetValue(result)!)
            {
                var dto = (MessageDto)result.GetType().GetProperty("message")!.GetValue(result)!;

                await Clients.User(dto.ReceiverId.ToString()).SendAsync("ReceiveMessage", dto);
                await Clients.Caller.SendAsync("ReceiveMessage", dto);
            }
            else
            {
                var error = result.GetType().GetProperty("message")!.GetValue(result);
                await Clients.Caller.SendAsync("Error", error);
            }
        }



        public override async Task OnConnectedAsync()
        {
            var userIdStr = Context.UserIdentifier ?? Context.User?.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out int userId))
            {
                await base.OnConnectedAsync();
                return;
            }

            await _presenceService.UserConnectedAsync(userIdStr, Context.ConnectionId);

            if (_userRegistrationRepository != null)
            {
                var user = await _userRegistrationRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    user.isOnline = true;
                    user.LastSeen = DateTime.UtcNow;
                    await _userRegistrationRepository.UpdateUserAsync(user);
                }
            }

            var connections = await _presenceService.GetConnectionCountAsync(userIdStr);
            if (connections == 1)
            {
                await NotifyOnlineStatus(userId, true);
            }

            await SendOnlineListToCaller(userIdStr);

          
            var groups = await _groupMemberRepo.GetGroupsByUserIdAsync(userId);
            foreach (var group in groups)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"group_{group.GroupId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userIdStr = Context.UserIdentifier ?? Context.User?.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out int userId))
            {
                await base.OnDisconnectedAsync(exception);
                return;
            }

            await _presenceService.UserDisconnectedAsync(userIdStr, Context.ConnectionId);

            if (_userRegistrationRepository != null)
            {
                var user = await _userRegistrationRepository.GetUserByIdAsync(userId);
                if (user != null)
                {
                    var remaining = await _presenceService.GetConnectionCountAsync(userIdStr);
                    user.isOnline = remaining > 0;
                    user.LastSeen = DateTime.UtcNow;
                    await _userRegistrationRepository.UpdateUserAsync(user);
                }
            }

            var connCount = await _presenceService.GetConnectionCountAsync(userIdStr);
            if (connCount == 0)
            {
                await NotifyOnlineStatus(userId, false);
            }

            await base.OnDisconnectedAsync(exception);
        }

        
        public async Task BlockUser(int contactUserId)
        {
            var userIdStr = Context.UserIdentifier ?? Context.User?.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var currentUserId))
            {
                await Clients.Caller.SendAsync("Error", "Invalid user.");
                return;
            }

            var result = await _contactRepository.BlockContactAsync(currentUserId, contactUserId);

            if (result)
            {
                await Clients.Caller.SendAsync("BlockSuccess", contactUserId);
                await Clients.User(contactUserId.ToString()).SendAsync("BlockedByUser", currentUserId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Failed to block user.");
            }
        }

        public async Task UnblockUser(int contactUserId)
        {
            var userIdStr = Context.UserIdentifier ?? Context.User?.FindFirst("UserId")?.Value;
            if (!int.TryParse(userIdStr, out var currentUserId))
            {
                await Clients.Caller.SendAsync("Error", "Invalid user.");
                return;
            }

            var result = await _contactRepository.UnblockContactAsync(currentUserId, contactUserId);

            if (result)
            {
                await Clients.Caller.SendAsync("UnblockSuccess", contactUserId);
                await Clients.User(contactUserId.ToString()).SendAsync("UnblockedByUser", currentUserId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Failed to unblock user.");
            }
        }

        
        private async Task SendOnlineListToCaller(string callerUserIdStr)
        {
            if (!int.TryParse(callerUserIdStr, out var callerId)) return;

            var blockedByMeIds = await GetBlockedByMeIds(callerId);
            var blockedMeIds = await GetUsersWhoBlockedMeIds(callerId);
            var onlineUsers = await _presenceService.GetAllOnlineUsersAsync();

            var visibleUsers = onlineUsers
                .Select(s => { bool ok = int.TryParse(s, out var v); return (ok, v); })
                .Where(t => t.ok)
                .Select(t => t.v)
                .Where(uid => !blockedByMeIds.Contains(uid) && !blockedMeIds.Contains(uid))
                .Select(i => i.ToString())
                .ToList();

            await Clients.Caller.SendAsync("OnlineUsers", visibleUsers);
        }

        private async Task<List<int>> GetBlockedByMeIds(int currentUserId)
        {
            var blocked = await _contactRepository.GetBlockedContactsAsync(currentUserId);
            return blocked?.Select(c => c.ContactUserId).Where(id => id > 0).Distinct().ToList() ?? new List<int>();
        }

        private async Task<List<int>> GetUsersWhoBlockedMeIds(int currentUserId)
        {
            var blockers = await _contactRepository.GetUsersWhoBlockedMeAsync(currentUserId) ?? new List<ContactDto>();
            var result = new List<int>();
            foreach (var c in blockers)
            {
                if (c == null) continue;

                if (c.GetType().GetProperty("BlockingUserId")?.GetValue(c) is int bi && bi > 0)
                {
                    result.Add(bi);
                }
                else if (c.ContactUserId > 0 && c.ContactUserId != currentUserId)
                {
                    result.Add(c.ContactUserId);
                }
            }

            return result.Distinct().ToList();
        }

        private async Task NotifyOnlineStatus(int userId, bool isOnline)
        {
            var userIdStr = userId.ToString();
            var onlineUsers = await _presenceService.GetAllOnlineUsersAsync();

            foreach (var onlineUserIdStr in onlineUsers)
            {
                if (!int.TryParse(onlineUserIdStr, out var onlineUserId)) continue;

                var recipientBlockedByMe = await GetBlockedByMeIds(onlineUserId);
                var recipientBlockedMe = await GetUsersWhoBlockedMeIds(onlineUserId); 

                if (recipientBlockedByMe.Contains(userId) || recipientBlockedMe.Contains(userId)) continue;

                if (isOnline)
                    await Clients.User(onlineUserIdStr).SendAsync("UserOnline", userIdStr);
                else
                    await Clients.User(onlineUserIdStr).SendAsync("UserOffline", userIdStr);
            }
        }
        public async Task SendGroupMessage(GroupMessageSendDto request)
        {
            var senderIdStr = Context.UserIdentifier ?? Context.User?.FindFirst("UserId")?.Value;
            if (!int.TryParse(senderIdStr, out var senderId)) return;

            var messageDto = await _groupService.SendMessageAsync(request);

            
            await Clients.Group($"group_{request.GroupId}")
                .SendAsync("ReceiveGroupMessage", messageDto);

            await Clients.Caller.SendAsync("ReceiveGroupMessage", messageDto);
        }

        public async Task JoinGroup(int groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"group_{groupId}");
        }
        public async Task LeaveGroup(int groupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"group_{groupId}");
        }


    }
}
