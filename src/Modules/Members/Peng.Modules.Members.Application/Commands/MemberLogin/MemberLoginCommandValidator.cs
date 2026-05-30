using FluentValidation;

namespace Peng.Modules.Members.Application.Commands.MemberLogin;

public class MemberLoginCommandValidator : AbstractValidator<MemberLoginCommand>
{
    public MemberLoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
