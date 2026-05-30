using FluentValidation;

namespace Peng.Modules.Members.Application.Commands.CreateMember;

public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}
