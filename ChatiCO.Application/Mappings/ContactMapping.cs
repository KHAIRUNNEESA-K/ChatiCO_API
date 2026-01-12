using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Mappings
{
    public class ContactMapping :Profile
    {
        public ContactMapping()
        {
            CreateMap<Contacts, ContactDto>()
                .ForMember(dest => dest.ContactUserId, opt => opt.MapFrom(src => src.ContactUserId))
                .ForMember(dest => dest.ContactUsername, opt => opt.MapFrom(src => src.ContactUser.Username))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.MapFrom(src => src.ContactUser.ProfilePicturePath));
        }

    }
}
