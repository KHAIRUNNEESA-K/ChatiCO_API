using ChatiCO.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Validators
{
    public class UserLoginValidators:AbstractValidator<LoginRequestDto>
    {
        public UserLoginValidators()
        {
            RuleFor(u => u.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\+91[0-9]{10}$").WithMessage("Phone number must be in format +91XXXXXXXXXX");
        }
    }
}
