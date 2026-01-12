using ChatiCO.Application.DTOs;
using FluentValidation;

namespace ChatiCO.Application.Validators
{
    public class MessageValidator : AbstractValidator<MessageDto>
    {
        public MessageValidator()
        {

            RuleFor(m => m.ReceiverId)
                .GreaterThan(0)
                .WithMessage("ReceiverId is required and must be greater than 0.");

            RuleFor(m => m.MessageType)
                .NotEmpty()
                .WithMessage("MessageType is required.")
                .Must(mt => mt == "Text" || mt == "Image" || mt == "File")
                .WithMessage("MessageType must be Text, Image, or File.");

            RuleFor(m => m.Content)
                .NotNull()
                .When(m => m.MessageType == "Text")
                .WithMessage("Text message must have content.");

            RuleFor(m => m.FileName)
                .NotEmpty()
                .When(m => m.MessageType == "Image" || m.MessageType == "File")
                .WithMessage("FileName is required for Image or File messages.");

            RuleFor(m => m.FileType)
                .NotEmpty()
                .When(m => m.MessageType == "Image" || m.MessageType == "File")
                .WithMessage("FileType is required for Image or File messages.");
        }
    }
}
