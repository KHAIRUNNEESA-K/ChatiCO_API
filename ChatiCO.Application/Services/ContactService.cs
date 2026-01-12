using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ContactService : IContactService
{
    private readonly IContactRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidator<AddContactRequestDto> _validator;
    private readonly IPresenceService _presenceService;

    public ContactService(
        IContactRepository repo,
        IMapper mapper,
        IValidator<AddContactRequestDto> validator,
        IPresenceService presenceService)
    {
        _repo = repo;
        _mapper = mapper;
        _validator = validator;
        _presenceService=presenceService;
    }
    public async Task<object> GetAvailableUsersAsync()
    {
        try
        {
            var users = await _repo.GetAvailableUsersAsync();
            var userDtos = _mapper.Map<List<ContactDto>>(users);

            return new
            {
                success = true,
                users = userDtos
            };
        }
        catch (UnauthorizedAccessException ex)
        {
            return new
            {
                success = false,
                message = "Unauthorized: " + ex.Message
            };
        }
        catch (Exception ex)
        {
            return new
            {
                success = false,
                message = "Something went wrong: " + ex.Message
            };
        }
    }
    public async Task<object> AddContactAsync(int currentUserId, AddContactRequestDto dto)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new
                {
                    success = false,
                    message = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage))
                };
            }

            if (currentUserId == dto.ContactUserId)
                return new { success = false, message = "You cannot add yourself" };

          
            var existingContacts = await _repo.GetUserContactsAsync(currentUserId);
            if (existingContacts.Any(c => c.ContactUserId == dto.ContactUserId))
                return new { success = false, message = "User already added" };

            var contact = new Contacts
            {
                UserId = currentUserId,
                ContactUserId = dto.ContactUserId,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = currentUserId.ToString()
            };

            await _repo.AddContactAsync(contact);

            return new { success = true, message = "Contact added successfully" };
        }
        catch (Exception ex)
        {
            return new
            {
                success = false,
                message = "Something went wrong: " + ex.Message
            };
        }
    }

   public async Task<object> GetUserContactsAsync(int currentUserId)
{
    try
    {
        var contacts = await _repo.GetUserContactsAsync(currentUserId);
        foreach (var c in contacts)
        {
            c.IsOnline = await _presenceService.IsUserOnlineAsync(c.ContactUserId.ToString());
        }

        return new
        {
            success = true,
            contacts = contacts
        };
    }
    catch (Exception ex)
    {
        return new { success = false, message = ex.Message };
    }
}

    public async Task<object> DeleteContactAsync(int contactId, int currentUserId)
    {
        try
        {
            var result = await _repo.DeleteContactAsync(contactId, currentUserId);

            bool isSuccess = (bool)result.GetType().GetProperty("success").GetValue(result);
            string message = (string)result.GetType().GetProperty("message").GetValue(result);

            if (!isSuccess)
                return new { success = false, message };

            return new { success = true, message };
        }
        catch (Exception ex)
        {
            return new { success = false, message = ex.Message };
        }
    }

    public async Task<object> BlockContactAsync(int currentUserId, int contactUserId)
    {
        var result = await _repo.BlockContactAsync(currentUserId,contactUserId);

        if (!result)
        {
            var existing = await _repo.GetUserContactsAsync(currentUserId);
            if (!existing.Any(c => c.ContactUserId == contactUserId))
                return new { success = false, message = "Contact not found" };

            return new { success = false, message = "Contact already blocked" };
        }

        return new { success = true, message = "Contact blocked successfully" };
    }

    public async Task<object> UnblockContactAsync(int currentUserId, int contactUserId)
    {
        var result = await _repo.UnblockContactAsync(currentUserId,contactUserId);

        if (!result)
        {
            var existing = await _repo.GetUserContactsAsync(currentUserId);
            if (!existing.Any(c => c.ContactUserId == contactUserId))
                return new { success = false, message = "Contact not found" };

            return new { success = false, message = "Contact already unblocked" };
        }

        return new { success = true, message = "Contact unblocked successfully" };
    }

    public async Task<object> GetBlockedContactsAsync(int currentUserId)
    {

        var blockedContacts = await _repo.GetBlockedContactsAsync(currentUserId);

        return new
        {
            success = true,
            contacts = blockedContacts
        };
    }
    public async Task<object> GetAllOtherUsersAsync(int currentUserId)
    {
        try
        {
            var users = await _repo.GetAllOtherUsersAsync(currentUserId);
            var userDtos = _mapper.Map<List<ContactDto>>(users);

            return new
            {
                success = true,
                users = userDtos
            };
        }
        catch (UnauthorizedAccessException ex)
        {
            return new
            {
                success = false,
                message = "Unauthorized: " + ex.Message
            };
        }
        catch (Exception ex)
        {
            return new
            {
                success = false,
                message = "Something went wrong: " + ex.Message
            };
        }
    }

}
