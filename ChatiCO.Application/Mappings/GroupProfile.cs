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
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, GroupDetailDto>()
                .ForMember(dest => dest.Members, opt => opt.Ignore()) 
                .ForMember(dest => dest.Messages, opt => opt.Ignore());

            CreateMap<GroupMember, GroupMemberDto>();

            CreateMap<GroupMessage, GroupMessageDto>()
                .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.GroupMessageId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType))
                .ForMember(dest => dest.SentTime, opt => opt.MapFrom(src => src.SentTime))
                .ForMember(dest => dest.TempId, opt => opt.Ignore()); 

            CreateMap<Group, GroupListDto>()
                .ForMember(dest => dest.LastMessage, opt => opt.Ignore())
                .ForMember(dest => dest.LastMessageSenderName, opt => opt.Ignore())
                .ForMember(dest => dest.LastMessageTime, opt => opt.Ignore())
                .ForMember(dest => dest.UnreadMessagesCount, opt => opt.Ignore());
        }
    }
}
