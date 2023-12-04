using EmailService.Dto;

namespace EmailService.ports;

public interface IEmailInfraPort
{
    void sendEmail(EmailDto emailDto);
}