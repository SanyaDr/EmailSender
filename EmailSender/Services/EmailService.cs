using EmailSender.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;


namespace EmailSender.Services;

public class EmailService
{
    private readonly SmtpConfig _config;

    public EmailService(IOptions<SmtpConfig> options)
    {
        _config = options.Value;
    }
    
    public async Task SendMessageAsync(string email, string subject, string text)
    {
        using var emailMessage = new MimeMessage();
        
        emailMessage.From.Add(new MailboxAddress("Информирование от сайта BlazorSender", _config.UserName ));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart("Plain")
        {
            Text = text
        };

        {
            EmailHistoryJsonService saver = new();
            await saver.AddToHistory(new EmailModel()
            {
                Date = DateOnly.FromDateTime(DateTime.Now),
                Email = email,
                Subject = subject,
                Text = text
            });
        }

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_config.Host, _config.Port, false);
            await client.AuthenticateAsync(_config.UserName, _config.Password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}