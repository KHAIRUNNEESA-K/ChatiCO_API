using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ChatiCO.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUserRegistrationRepository _userRegistrationRepo;
        private readonly IGroupRepository _groupRepo;
        private readonly IGroupMemberRepository _memberRepo;
        private readonly IGroupMessageRepository _messageRepo;
        private readonly ICloudinaryFileStorage _fileStorage;
        private readonly IMapper _mapper;
        private readonly IGroupNotificationService _groupNotificationService;


        public GroupService(
            IUserRegistrationRepository userRegistrationRepo,
            IGroupRepository groupRepo,
            IGroupMemberRepository memberRepo,
            IGroupMessageRepository messageRepo,
            ICloudinaryFileStorage fileStorage,
            IMapper mapper,
            IGroupNotificationService groupNotificationService)
        {
            
            _groupRepo = groupRepo;
            _memberRepo = memberRepo;
            _messageRepo = messageRepo;
            _fileStorage = fileStorage;
            _mapper = mapper;
            _userRegistrationRepo = userRegistrationRepo;
            _groupNotificationService = groupNotificationService;
        }

        public async Task<GroupDetailDto> CreateGroupAsync(GroupCreateDto dto, int creatorId)
        {
            string? profilePicUrl = null;

            if (dto.GroupProfilePic != null)
                profilePicUrl = await _fileStorage.UploadFileAsync(dto.GroupProfilePic);

            var group = new Group
            {
                Name = dto.GroupName,
                ProfileImageUrl = profilePicUrl,
                CreatedByUserId = creatorId,
                CreatedBy = creatorId.ToString(),
                CreatedOn = DateTime.UtcNow
            };

            await _groupRepo.CreateGroupAsync(group);

            var allMemberIds = dto.MemberIds.Distinct().ToList();
            if (!allMemberIds.Contains(creatorId))
                allMemberIds.Add(creatorId);

            var memberEntities = allMemberIds.Select(uid => new GroupMember
            {
                GroupId = group.GroupId,
                UserId = uid,
                JoinedAt = DateTimeOffset.UtcNow
            }).ToList();

            await _memberRepo.AddMembersAsync(memberEntities);

            var creator = await _userRegistrationRepo.GetUserByIdAsync(creatorId);

            var createdMsg = new GroupMessage
            {
                GroupId = group.GroupId,
                SenderId = creatorId,
                Content = $"{creator.Username} created this group",
                IsSystemMessage = true,
                MessageType = "System",
                TargetUserId = null, 
                SentTime = DateTimeOffset.UtcNow
            };
            await _messageRepo.AddMessageAsync(createdMsg);

            foreach (var memberId in allMemberIds.Where(id => id != creatorId))
            {
                var addedUser = await _userRegistrationRepo.GetUserByIdAsync(memberId);

                var msgForCreator = new GroupMessage
                {
                    GroupId = group.GroupId,
                    SenderId = creatorId,
                    Content = $"You added {addedUser.Username}",
                    IsSystemMessage = true,
                    MessageType = "System",
                    TargetUserId = creatorId,   
                    SentTime = DateTimeOffset.UtcNow
                };

                await _messageRepo.AddMessageAsync(msgForCreator);
            }

            foreach (var memberId in allMemberIds.Where(id => id != creatorId))
            {
                var msgForUser = new GroupMessage
                {
                    GroupId = group.GroupId,
                    SenderId = creatorId,
                    Content = $"{creator.Username} added you",
                    IsSystemMessage = true,
                    MessageType = "System",
                    TargetUserId = memberId,   
                    SentTime = DateTimeOffset.UtcNow
                };

                await _messageRepo.AddMessageAsync(msgForUser);
            }
            var membersWithDetails = new List<GroupMemberDto>();
            foreach (var member in memberEntities)
            {
                var user = await _userRegistrationRepo.GetUserByIdAsync(member.UserId);
                membersWithDetails.Add(new GroupMemberDto
                {
                    UserId = member.UserId,
                    FullName = user?.Username ?? "Unknown",
                    JoinedAt = member.JoinedAt.UtcDateTime
                });
            }

            var groupDto = new GroupDetailDto
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                GroupProfilePicUrl = group.ProfileImageUrl,
                Members = membersWithDetails
            };

            await _groupNotificationService.NotifyNewGroupAdded(groupDto, allMemberIds.ToArray(), creator.Username);

            return groupDto;
        }

        public async Task<List<GroupDetailDto>> GetGroupsCreatedByUserAsync(int userId)
        {
            var groups = await _groupRepo.GetGroupsCreatedByUserAsync(userId);

            return groups.Select(g => new GroupDetailDto
            {
                GroupId = g.GroupId,
                GroupName = g.Name,
                GroupProfilePicUrl = g.ProfileImageUrl
            }).ToList();
        }

        public async Task<GroupDetailDto> GetSingleGroupAsync(int groupId)
        {
            var group = await _groupRepo.GetGroupByIdAsync(groupId);
            if (group == null)
                throw new Exception("Group not found");

            var members = await _memberRepo.GetMembersByGroupIdAsync(groupId);

            var memberDtos = new List<GroupMemberDto>();

            foreach (var member in members)
            {
                var user = await _userRegistrationRepo.GetUserByIdAsync(member.UserId);

                memberDtos.Add(new GroupMemberDto
                {
                    UserId = user.UserId,
                    FullName = user.Username,
                    JoinedAt = member.JoinedAt.UtcDateTime
                });
            }

            return new GroupDetailDto
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                GroupProfilePicUrl = group.ProfileImageUrl,
                Members = memberDtos
            };
        }

        public async Task<GroupDetailDto> UpdateGroupAsync(GroupUpdateDto dto, int userId)
        {
            var group = await _groupRepo.GetGroupByIdAsync(dto.GroupId);
            if (group == null) throw new Exception("Group not found");

            if (!string.IsNullOrWhiteSpace(dto.GroupName))
                group.Name = dto.GroupName;

            if (dto.ProfilePic != null)
                group.ProfileImageUrl = await _fileStorage.UploadFileAsync(dto.ProfilePic);

            group.ModifiedBy = userId.ToString();
            group.ModifiedOn = DateTime.UtcNow;
            await _groupRepo.UpdateGroupAsync(group);

            var members = await _memberRepo.GetMembersByGroupIdAsync(dto.GroupId);
            var groupDto = _mapper.Map<GroupDetailDto>(group);
            groupDto.Members = members.Select(x => _mapper.Map<GroupMemberDto>(x)).ToList();

            return groupDto;
        }

        public async Task AddMembersAsync(GroupMemberAddDto dto, int userId) 
        {
            var adder = await _userRegistrationRepo.GetUserByIdAsync(userId); 
            var group = await _groupRepo.GetGroupByIdAsync(dto.GroupId);

            var existingMembers = (await _memberRepo.GetMembersByGroupIdAsync(dto.GroupId))
                .Select(m => m.UserId);

            var newMemberIds = dto.MemberIds
                .Distinct()
                .Where(id => !existingMembers.Contains(id))
                .ToList();

            if (!newMemberIds.Any())
                return;

            var validMemberIds = new List<int>();

            foreach (var id in newMemberIds)
            {
                var user = await _userRegistrationRepo.GetUserByIdAsync(id);
                if (user != null)
                    validMemberIds.Add(id);
            }

            if (!validMemberIds.Any())
                return;

            foreach (var newMemberId in validMemberIds)
            {
                var systemMessage = new GroupMessage
                {
                    GroupId = group.GroupId,
                    SenderId = userId,
                    Content = $"{adder.Username} added you to '{group.Name}'",
                    IsSystemMessage = true,
                    MessageType = "System",
                    SentTime = DateTimeOffset.UtcNow
                };
                await _messageRepo.AddMessageAsync(systemMessage);
            }

            var newMembers = validMemberIds
                .Select(id => new GroupMember
                {
                    GroupId = dto.GroupId,
                    UserId = id,
                    JoinedAt = DateTimeOffset.UtcNow
                })
                .ToList();

            await _memberRepo.AddMembersAsync(newMembers);

            var allMembers = await _memberRepo.GetMembersByGroupIdAsync(dto.GroupId);
            var memberDtos = new List<GroupMemberDto>();
            foreach (var m in allMembers)
            {
                var u = await _userRegistrationRepo.GetUserByIdAsync(m.UserId);
                memberDtos.Add(new GroupMemberDto
                {
                    UserId = m.UserId,
                    FullName = u?.Username ?? "Unknown",
                    JoinedAt = m.JoinedAt.UtcDateTime
                });
            }

            var groupDto = new GroupDetailDto
            {
                GroupId = group.GroupId,
                GroupName = group.Name,
                GroupProfilePicUrl = group.ProfileImageUrl,
                Members = memberDtos
            };

            await _groupNotificationService.NotifyNewGroupAdded(groupDto, validMemberIds.ToArray(), adder.Username);

        }


        public async Task<List<GroupMemberDto>> GetMembersAsync(int groupId)
        {
            var members = await _memberRepo.GetMembersByGroupIdAsync(groupId);
            return members.Select(m => _mapper.Map<GroupMemberDto>(m)).ToList();
        }

        public async Task<GroupMessageDto> SendMessageAsync(GroupMessageSendDto dto)
        {
            string? fileUrl = null;

            if (dto.File != null)
                fileUrl = await _fileStorage.UploadFileAsync(dto.File);

            var message = new GroupMessage
            {
                GroupId = dto.GroupId,
                SenderId = dto.SenderId,
                MessageType = dto.MessageType,
                Content = dto.MessageType == "Text" ? dto.TextContent : null,
                FileUrl = fileUrl,
                SentTime = DateTimeOffset.UtcNow
            };

            await _messageRepo.AddMessageAsync(message);

            return new GroupMessageDto
            {
                MessageId = message.GroupMessageId,
                GroupId = message.GroupId,
                SenderId = message.SenderId,
                MessageType = message.MessageType,
                Content = message.Content,
                FileUrl = message.FileUrl,
                TempId = dto.TempId,
                SentTime = message.SentTime
            };
        }

        public async Task<List<GroupMessageDto>> GetChatHistoryAsync(int groupId)
        {
            var messages = await _messageRepo.GetGroupMessagesAsync(groupId);
            return messages.Select(m => _mapper.Map<GroupMessageDto>(m)).ToList();
        }
        public async Task DeleteGroupAsync(int groupId, int userId)
        {
            var group = await _groupRepo.GetGroupByIdAsync(groupId);
            if (group == null)
                throw new Exception("Group not found");

            group.IsDeleted = true;
            group.DeletedBy = userId.ToString();
            group.ModifiedOn = DateTime.UtcNow;

            await _groupRepo.UpdateGroupAsync(group);
        }
        public async Task<List<GroupDetailDto>> GetGroupsByUserAsync(int userId)
        {
            
            var groups = await _groupRepo.GetUserGroupsAsync(userId);

            var result = new List<GroupDetailDto>();

            foreach (var group in groups)
            {
                var members = await _memberRepo.GetMembersByGroupIdAsync(group.GroupId);
                var memberDtos = new List<GroupMemberDto>();
                foreach (var member in members)
                {
                    var user = await _userRegistrationRepo.GetUserByIdAsync(member.UserId);
                    memberDtos.Add(new GroupMemberDto
                    {
                        UserId = member.UserId,
                        FullName = user?.Username ?? "Unknown",
                        JoinedAt = member.JoinedAt.UtcDateTime
                    });
                }

                result.Add(new GroupDetailDto
                {
                    GroupId = group.GroupId,
                    GroupName = group.Name,
                    GroupProfilePicUrl = group.ProfileImageUrl,
                    Members = memberDtos
                });
            }

            return result;
        }


    }
}
