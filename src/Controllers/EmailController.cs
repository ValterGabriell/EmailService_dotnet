using EmailService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.Controllers;

[ApiController]
[Route("/api/v1/")]
public class EmailController : ControllerBase
{
    private readonly EmailServicePort _emailServicePort;

    public EmailController(EmailServicePort emailServicePort)
    {
        _emailServicePort = emailServicePort;
    }

    [HttpPost]
    public IActionResult SendEmail(EmailDto req)
    {
        _emailServicePort.sendEmail(req);
        return Ok();
    }
}