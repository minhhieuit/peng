using FluentValidation;

namespace Peng.Modules.Members.Application.Commands.RegisterMember;

public class RegisterMemberCommandValidator : AbstractValidator<RegisterMemberCommand>
{
    public RegisterMemberCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
    }
}
