﻿using Microsoft.AspNetCore.Mvc;

namespace EmailApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public IActionResult SendEmail(EmailDto request)
        {
            var result = _emailService.SendEmail(request, out string message);
            if (result)
                return Ok(message);
            else
                return BadRequest(message);

        }
        [HttpPost]
        public IActionResult SendBulkEmail(EmailDto request)
        {
            var result = _emailService.SendBulkEmail(request);
            if (result)
                return Ok();
            else
                return BadRequest();

        }

    }
}
