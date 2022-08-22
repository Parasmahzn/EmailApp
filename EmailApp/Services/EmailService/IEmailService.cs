namespace EmailApp.Services.EmailService
{
    public interface IEmailService
    {
        bool SendEmail(EmailDto request, out string message);
        bool SendBulkEmail(EmailDto request);
    }
}
