using ChatiCO.Application.DTOs;
using ChatiCO.Domain.Entities;
using FluentValidation;

public class ArchiveValidator : AbstractValidator<ArchiveRequestDto>
{
    public ArchiveValidator()
    {


        RuleFor(a => a.ContactId)
            .NotEmpty().WithMessage("ContactId is required.")
            .GreaterThan(0).WithMessage("ContactId must be greater than 0.");
    }
}
