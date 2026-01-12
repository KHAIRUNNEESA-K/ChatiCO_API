using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using FluentValidation;
using System.Text;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repo;
    private readonly IContactRepository _contactRepository;
    private readonly IMapper _mapper;
    private readonly ICloudinaryFileStorage _fileStorage;
    private readonly IValidator<MessageCreateRequestDto> _validator;


    public MessageService(
        IMessageRepository repo,
        IContactRepository contactRepository,
        IMapper mapper,
        ICloudinaryFileStorage fileStorage,
        IValidator<MessageCreateRequestDto> validator)
    {
        _repo = repo;
        _contactRepository = contactRepository;
        _mapper = mapper;
        _fileStorage = fileStorage;
        _validator = validator;
    }

    public async Task<object> SendMessageAsync(int senderId, MessageCreateRequestDto request)
    {
        // 1️⃣ Validation & block checks
        var validation = await _validator.ValidateAsync(request);
        if (!validation.IsValid)
            return new
            {
                success = false,
                message = string.Join(", ", validation.Errors.Select(e => e.ErrorMessage))
            };

        if (await _contactRepository.IsBlockedAsync(senderId, request.ReceiverId))
            return new { success = false, message = "You have blocked this user." };

        if (await _contactRepository.IsBlockedAsync(request.ReceiverId, senderId))
            return new { success = false, message = "This user has blocked you." };

        // 2️⃣ Handle file or text
        string? fileUrl = null;
        string? fileType = null;
        string? fileName = null;
        byte[]? textBytes = null;

        if (request.File != null)
        {
            fileUrl = await _fileStorage.UploadFileAsync(request.File);
            fileType = request.File.ContentType;
            fileName = request.File.FileName;
        }
        else if (!string.IsNullOrEmpty(request.Content))
        {
            textBytes = Encoding.UTF8.GetBytes(request.Content);
        }

       
        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = request.ReceiverId,
            MessageType = request.File != null ? "Image" : "Text",
            Content = textBytes,
            FileUrl = fileUrl,
            FileName = fileName,
            FileType = fileType,
            TempId = request.TempId,
            SentTime = DateTimeOffset.UtcNow,
            CreatedBy = senderId.ToString()
        };

        await _repo.AddMessageAsync(message);

        
        var dto = new MessageDto
        {
            SenderId = senderId,
            ReceiverId = request.ReceiverId,
            MessageType = message.MessageType,
            Content = message.MessageType == "Image" ? fileUrl : request.Content, 
            FileUrl = fileUrl,
            FileName = fileName,
            FileType = fileType,
            TempId = request.TempId,
            SentTime = message.SentTime
        };

        return new
        {
            success = true,
            message = dto
        };
    }

    public async Task<object> GetMessagesBetweenUsersAsync(int senderId, int receiverId)
    {
        var messages = await _repo.GetMessagesBetweenUsersAsync(senderId, receiverId);
        var dtos = _mapper.Map<List<MessageDto>>(messages);

        return new { success = true, messages = dtos };
    }
    public async Task<object> GetLastMessageAsync(int senderId, int receiverId)
    {
        var message = await _repo.GetLastMessageAsync(senderId, receiverId);
        var dto = _mapper.Map<MessageDto>(message);

        return new { success = true, lastMessage = dto };
    }

    public async Task<object> GetUnreadMessagesCountAsync(int userId, int contactId)
    {
        var count = await _repo.GetUnreadMessagesCountAsync(userId, contactId);
        return new { success = true, unread = count };
    }

    public async Task<object> MarkMessageAsReadAsync(int messageId)
    {
        await _repo.MarkMessageAsReadAsync(messageId);
        return new { success = true };
    }
    public async Task<object> MarkMessageAsDeliveredAsync(int messageId)
    {
        await _repo.MarkMessageAsDeliveredAsync(messageId);
        return new { success = true };
    }
}
