using FluentValidation;

namespace Peng.Modules.Members.Application.Commands.ChangeMemberPassword;

public class ChangeMemberPasswordCommandValidator : AbstractValidator<ChangeMemberPasswordCommand>
{
    public ChangeMemberPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8);
    }
}
