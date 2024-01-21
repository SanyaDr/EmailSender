using EmailSender.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailSender.Controllers;

public class EmailController : Controller
{
    public async Task<IActionResult> SendMessage(string email, string subject, string text)
    {
        EmailService emailService = new();
        await emailService.SendMessageAsync(email, subject, text);
        return RedirectToAction("");
    }
}