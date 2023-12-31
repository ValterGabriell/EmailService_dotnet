﻿using EmailService.Dto;
using Microsoft.AspNetCore.Mvc;

namespace EmailService.ports;

public interface IEmailServicePort
{
    void sendEmail(EmailDto emailDto);
}