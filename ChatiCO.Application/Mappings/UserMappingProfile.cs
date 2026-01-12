using ChatiCO.Application.DTOs;
using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ChatiCO.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<RegisterRequestDto, User>();
            CreateMap<User, RegisterRequestDto>();
            CreateMap<User, ContactDto>()
                .ForMember(dest => dest.ContactUserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ContactUsername, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.MapFrom(src => src.ProfilePicturePath));

        }
    }
}
