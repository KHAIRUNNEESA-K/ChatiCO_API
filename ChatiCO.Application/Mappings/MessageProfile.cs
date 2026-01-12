using AutoMapper;
using ChatiCO.Application.DTOs;
using ChatiCO.Domain.Entities;
using System.Text;

namespace ChatiCO.Application.Mapping
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            
            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src =>
                    src.MessageType == "Text" && src.Content != null
                        ? Encoding.UTF8.GetBytes(src.Content)
                        : null
                ))
                .ForMember(dest => dest.MessageType, opt => opt.MapFrom(src => src.MessageType))
                .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType))
                .ForMember(dest => dest.SentTime, opt => opt.MapFrom(src => src.SentTime));

           
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src =>
                    src.MessageType == "Text" && src.Content != null
                        ? Encoding.UTF8.GetString(src.Content)
                        : null
                ))
                .ForMember(dest => dest.MessageType, opt => opt.MapFrom(src => src.MessageType))
                .ForMember(dest => dest.FileUrl, opt => opt.MapFrom(src => src.FileUrl))
                .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest => dest.FileType, opt => opt.MapFrom(src => src.FileType))
                .ForMember(dest => dest.SentTime, opt => opt.MapFrom(src => src.SentTime));
        }
    }
}
