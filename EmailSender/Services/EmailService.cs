using EmailSender.Interface;
using EmailSender.Models;
using MimeKit;
using MailKit.Net.Smtp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EmailSender.Services;

//TODO temp solution, удалить после применения IOPTIONS
internal class emailConfig
{
    protected internal string smtp { get; set; }
    protected internal string login { get; set; }
    protected internal string password { get; set; }
    protected internal int port { get; set; }
}

public class EmailService
{
    public async Task SendMessageAsync(string email, string subject, string text)
    {
        using var emailMessage = new MimeMessage();
        string jsonData = File.ReadAllText("config.json");
        emailConfig cfg = new();
        JObject JCfg = JObject.Parse(jsonData);
        cfg.smtp = JCfg["smpt"].ToString();
        cfg.login = JCfg["login"].ToString();
        cfg.password = JCfg["password"].ToString();
        cfg.port = JCfg["port"].Value<int>();
        
        emailMessage.From.Add(new MailboxAddress("Информирование от сайта BlazorSender", cfg.login));
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
            await client.ConnectAsync(cfg.smtp, cfg.port, false);
            await client.AuthenticateAsync(cfg.login, cfg.password);
            await client.SendAsync(emailMessage);

            await client.DisconnectAsync(true);
        }
    }
}