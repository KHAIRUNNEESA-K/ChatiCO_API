using ChatiCO.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Validators
{
    public class AddContactRequestDtoValidators : AbstractValidator<AddContactRequestDto>
    {
        public AddContactRequestDtoValidators()
        {
            
            RuleFor(x => x.ContactUserId).GreaterThan(0).WithMessage("ContactUserId must be greater than 0.");
        }

    }
}
