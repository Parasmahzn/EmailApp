using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EmailApp.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailServerModel _emailServer;
        public EmailService(IConfiguration configuration)
        {
            _emailServer = configuration.GetSection("EmailConfiguration").Get<EmailServerModel>();
        }

        public bool SendEmail(EmailDto request)
        {
            return SendEmailTo(request);
        }

        public bool SendBulkEmail(EmailDto request)
        {
            bool send = false;
            List<string> recipient = new();
            if (!string.IsNullOrEmpty(request.To))
            {
                recipient = request.To.Split(",").ToList();
            }
            foreach (var item in recipient)
            {
                request.To = item;
                send = SendEmailTo(request);
            }
            return send;
        }

        private bool SendEmailTo(EmailDto request)
        {
            bool isEmailSent = false;
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_emailServer.Username));
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = request.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_emailServer.Host, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailServer.Username, _emailServer.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
                isEmailSent = true;
            }
            catch (Exception ex)
            {

                string ExceptionLogPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\");
                string FileName = ExceptionLogPath + "ExLog" + DateTime.Now.ToString("yyyyMMdd") + ".txt";

                if (!File.Exists(FileName))
                {
                    File.Create(FileName).Dispose();
                }
                using StreamWriter sw = File.AppendText(FileName);
                sw.WriteLine("=============Error Logging ===========");
                sw.WriteLine("===========Start============= " + DateTime.Now);
                sw.WriteLine("Error Message: " + ex.Message);
                sw.WriteLine("Stack Trace: " + ex.StackTrace);
                sw.WriteLine("===========End============= " + DateTime.Now);
            }
            return isEmailSent;
        }
    }
}
