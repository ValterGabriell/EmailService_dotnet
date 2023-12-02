using EmailService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.ports;

public interface EmailServicePort
{
    void sendEmail(EmailDto emailDto);
}