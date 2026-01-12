using ChatiCO.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Validators
{
    public class GroupMessageSendDtoValidator : AbstractValidator<GroupMessageSendDto>
    {
        public GroupMessageSendDtoValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage("Invalid Group ID");

            RuleFor(x => x.SenderId)
                .GreaterThan(0)
                .WithMessage("Invalid Sender ID");

            RuleFor(x => x.MessageType)
                .NotEmpty()
                .WithMessage("Message type is required")
                .Must(type => type == "Text" || type == "Image")
                .WithMessage("MessageType must be 'Text' or 'Image'");

            RuleFor(x => x.TextContent)
                .MaximumLength(5000)
                .When(x => x.MessageType == "Text")
                .WithMessage("Text content cannot exceed 5000 characters");

            RuleFor(x => x.File)
                .Must(file => file == null || file.Length <= 10 * 1024 * 1024) 
                .When(x => x.MessageType == "Image")
                .WithMessage("File size cannot exceed 10MB");
        }
    }
}
