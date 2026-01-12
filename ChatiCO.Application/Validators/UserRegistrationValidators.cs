using FluentValidation;
using ChatiCO.Application.DTOs;

namespace ChatiCO.Application.Validators
{
    public class UserRegistrationValidators : AbstractValidator<RegisterRequestDto>
    {
        public UserRegistrationValidators()
        {
            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 20).WithMessage("Username must be between 3 and 20 characters.")
                .Matches("^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores.");

            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+91[0-9]{10}$").WithMessage("Phone number must be in format +91XXXXXXXXXX");
        }
    }
}
