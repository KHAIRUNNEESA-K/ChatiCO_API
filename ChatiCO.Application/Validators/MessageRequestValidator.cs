using ChatiCO.Application.DTOs;
using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Validators
{
    public class MessageRequestValidator : AbstractValidator<MessageCreateRequestDto>
    {
        public MessageRequestValidator()
        {
            RuleFor(x => x.ReceiverId)
                .GreaterThan(0).WithMessage("ReceiverId must be provided.");

            RuleFor(x => x.MessageType)
                .NotEmpty().WithMessage("MessageType is required.")
                .Must(mt => mt == "Text" || mt == "Image")
                .WithMessage("MessageType must be either 'Text' or 'Image'.");

            When(x => x.MessageType == "Text", () =>
            {
                RuleFor(x => x.Content)
                    .NotEmpty().WithMessage("Content is required for text messages.");
            });

            When(x => x.MessageType == "Image", () =>
            {
                RuleFor(x => x.File)
                    .NotNull().WithMessage("File is required for image messages.");
            });
        }
    }
}
