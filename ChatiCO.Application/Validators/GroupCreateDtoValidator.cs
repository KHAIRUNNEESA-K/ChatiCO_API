using ChatiCO.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Validators
{
    public class GroupCreateDtoValidator : AbstractValidator<GroupCreateDto>
    {
        public GroupCreateDtoValidator()
        {
            RuleFor(x => x.GroupName)
                .NotEmpty()
                .WithMessage("Group name is required")
                .MaximumLength(100)
                .WithMessage("Group name cannot exceed 100 characters");

            RuleFor(x => x.MemberIds)
                .NotNull()
                .WithMessage("Member list cannot be null")
                .Must(list => list.Count > 0)
                .WithMessage("At least one member must be added");

            RuleFor(x => x.GroupProfilePic)
                .Must(file => file == null || file.Length <= 5 * 1024 * 1024) 
                .WithMessage("Profile picture size cannot exceed 5MB");
        }
    }
}
