using System.Numerics;
using EmailSender.Interface;
using EmailSender.Models;
using Newtonsoft.Json;

namespace EmailSender.Services;

public class EmailHistoryJsonService: IEmailHistorySaver
{
    public async Task AddToHistory(EmailModel newMessage)
    {
        string json = await File.ReadAllTextAsync("./History.json");
        var hist = JsonConvert.DeserializeObject<Queue<EmailModel>>(json) ?? new Queue<EmailModel>();
        hist.Enqueue(newMessage);
        if (hist.Count() > 200)
        {
            // hist.Append(newMessage);
            hist.Dequeue();
        }
        json = JsonConvert.SerializeObject(hist, Formatting.Indented);
        await File.WriteAllTextAsync("./History.json", json);
    }

    public async Task<IReadOnlyCollection<EmailModel>> GetHistory()
    {
        string json = File.ReadAllText("./History.json");
        var hist = JsonConvert.DeserializeObject<IReadOnlyCollection<EmailModel>>(json) ??
                   new List<EmailModel>().AsReadOnly();
        return hist;
    }
}