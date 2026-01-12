using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Domain.Entities;

public class ArchiveProfile : Profile
{
    public ArchiveProfile()
    {

        CreateMap<Archive, ArchiveResponseDto>()
            .ForMember(dest => dest.ArchiveId, opt => opt.MapFrom(src => src.ArchiveId))
            .ForMember(dest => dest.ContactUsername,
             opt => opt.MapFrom(src => src.Contact.Username))
            .ForMember(dest => dest.ContactProfilePicture,
             opt => opt.MapFrom(src => src.Contact.ProfilePicturePath));

    }
}
