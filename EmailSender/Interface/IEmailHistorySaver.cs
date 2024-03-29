using EmailSender.Models;

namespace EmailSender.Interface;

public interface IEmailHistorySaver
{
    public Task AddToHistory(EmailModel newMessage);
    public IReadOnlyCollection<EmailModel> GetHistory();
}