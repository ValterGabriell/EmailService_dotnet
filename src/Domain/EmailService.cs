using EmailService.Dto;

namespace EmailService.Domain;

public class EmailService : IEmailServicePort
{
    private readonly IEmailInfraPort _emailInfra;

    public EmailService(IEmailInfraPort emailInfra)
    {
        _emailInfra = emailInfra;
    }

    public void sendEmail(EmailDto emailDto)
    {
        emailDto.ValidatingFields();
        _emailInfra.sendEmail(emailDto);
    }
}