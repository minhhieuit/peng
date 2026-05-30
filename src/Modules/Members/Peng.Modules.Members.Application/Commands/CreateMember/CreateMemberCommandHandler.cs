using Peng.Modules.Members.Application.DTOs;
using Peng.Modules.Members.Domain.Entities;
using Peng.Modules.Members.Domain.Repositories;
using Peng.SharedKernel.Application;

namespace Peng.Modules.Members.Application.Commands.CreateMember;

internal sealed class CreateMemberCommandHandler(
    IMemberRepository memberRepository,
    IPasswordHasher passwordHasher,
    IEmailSender emailSender,
    IMembersUnitOfWork unitOfWork) : ICommandHandler<CreateMemberCommand, CreateMemberResponse>
{
    public async Task<Result<CreateMemberResponse>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        if (await memberRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            return Result.Failure<CreateMemberResponse>(Error.Conflict("Member"));

        var temporaryPassword = TemporaryPasswordGenerator.Generate();
        var passwordHash = passwordHasher.Hash(temporaryPassword);
        var member = Member.CreateByAdmin(request.Email, request.FirstName, request.LastName, passwordHash, request.CreatedByUserId);

        await memberRepository.AddAsync(member, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Seam for an invitation email; the no-op sender just logs today.
        await emailSender.SendAsync(member.Email, "Your account has been created",
            $"An account was created for you. Temporary password: {temporaryPassword}", cancellationToken);

        return Result.Success(new CreateMemberResponse(member.Id, member.Email, member.FullName, temporaryPassword));
    }
}
