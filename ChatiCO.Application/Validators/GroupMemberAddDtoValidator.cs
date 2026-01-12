using ChatiCO.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Application.Validators
{
    public class GroupMemberAddDtoValidator : AbstractValidator<GroupMemberAddDto>
    {
        public GroupMemberAddDtoValidator()
        {
            RuleFor(x => x.GroupId)
                .GreaterThan(0)
                .WithMessage("Invalid Group ID");

            RuleFor(x => x.MemberIds)
                .NotNull()
                .WithMessage("Member list cannot be null")
                .Must(list => list.Count > 0)
                .WithMessage("At least one member must be added");
        }
    }
}
