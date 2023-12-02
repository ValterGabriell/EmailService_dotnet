using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;

namespace EmailService.Controllers;

[ApiController]
[Route("/api/v1/")]
public class EmailController : ControllerBase
{
    [HttpPost]
    public IActionResult SendEmail(string body)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("immothy.reichert51@ethereal.email"));
        email.To.Add(MailboxAddress.Parse("immothy.reichert51@ethereal.email"));
        email.Subject = "Test email";
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtpClient = new SmtpClient();
        smtpClient.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
        smtpClient.Authenticate("timmothy.reichert51@ethereal.email", "sanz7NBw8J7djckN92");
        smtpClient.Send(email);
        smtpClient.Disconnect(true);

        return Ok();
    }
}