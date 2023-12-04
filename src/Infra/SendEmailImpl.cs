using EmailService.Dto;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace EmailService.Infra;

public class SendEmailImpl : IEmailInfraPort
{
    private readonly IConfiguration _configuration;

    public SendEmailImpl(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void sendEmail(EmailDto emailDto)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailUsername").Value));
        email.To.Add(MailboxAddress.Parse(emailDto.To));
        email.Subject = emailDto.Subject;
        email.Body = new TextPart(TextFormat.Html) { Text = emailDto.Body };

        using var smtpClient = new SmtpClient();
        smtpClient.Connect(_configuration.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
        smtpClient.Authenticate(_configuration.GetSection("EmailUsername").Value,
            _configuration.GetSection("EmailPassword").Value);
        smtpClient.Send(email);
        smtpClient.Disconnect(true);
    }
}