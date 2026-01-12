using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Application.Interfaces;
using ChatiCO.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ArchiveService : IArchiveService
{
    private readonly IArchiveRepository _repo;
    private readonly IMapper _mapper;
    private readonly IValidator<ArchiveRequestDto> _validator;

    public ArchiveService(
        IArchiveRepository repo,
        IMapper mapper,
        IValidator<ArchiveRequestDto> validator)
    {
        _repo = repo;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<object> AddArchiveAsync(int currentUserId, ArchiveRequestDto dto, string currentUsername)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return new { success = false, message = errors };
            }

            if (currentUserId == dto.ContactId)
                return new { success = false, message = "You cannot archive yourself" };
            var archiveDto = await _repo.AddToArchiveAsync(currentUserId, dto.ContactId);

            if (archiveDto == null)
                return new { success = false, message = "Chat already archived or something went wrong" };

            return new { success = true, archive = archiveDto };
        }
        catch (Exception ex)
        {
            return new { success = false, message = "Something went wrong: " + ex.Message };
        }
    }

    public async Task<object> RemoveArchiveAsync(int currentUserId, int contactId)
    {
        try
        {
            string deletedBy = currentUserId.ToString();  

            var removed = await _repo.RemoveFromArchiveAsync(currentUserId,contactId,deletedBy);

            if (!removed)
                return new { success = false, message = "Archive not found or already unarchived" };

            return new { success = true, message = "Chat unarchived successfully" };
        }
        catch (Exception ex)
        {
            return new { success = false, message = "Something went wrong: " + ex.Message };
        }
    }




    public async Task<object> GetArchivedChatsAsync(int currentUserId)
    {
        try
        {
            var archives = await _repo.GetArchivedContactsAsync(currentUserId);

            return new { success = true, archives };
        }
        catch (Exception ex)
        {
            return new { success = false, message = "Something went wrong: " + ex.Message };
        }
    }

}
