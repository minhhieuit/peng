using Microsoft.Extensions.Logging;
using Peng.SharedKernel.Application;

namespace Peng.SharedKernel.Infrastructure;

/// <summary>
/// Default no-op email sender. Logs the message instead of delivering it so the
/// rest of the system can depend on <see cref="IEmailSender"/> today and a real
/// provider can be swapped in later without touching call sites.
/// </summary>
public class NoOpEmailSender(ILogger<NoOpEmailSender> logger) : IEmailSender
{
    public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        logger.LogInformation("[Email skipped] To: {To} | Subject: {Subject}", to, subject);
        return Task.CompletedTask;
    }
}
