namespace Peng.SharedKernel.Application;

/// <summary>
/// Seam for outbound email. The default implementation is a no-op that logs;
/// wire up a real SMTP/provider implementation when email delivery is needed.
/// </summary>
public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
}
