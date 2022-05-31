namespace EmailApp.Services.EmailService
{
    public interface IEmailService
    {
        bool SendEmail(EmailDto request);
        bool SendBulkEmail(EmailDto request);
    }
}
