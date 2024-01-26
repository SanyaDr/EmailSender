using System.ComponentModel.DataAnnotations;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace EmailSender.Models;

public class EmailModel
{
    // Дата отправки сообщения
    public DateOnly Date { get; set; }
    // public DateTime Date { get; set; }
    
    [Required(ErrorMessage = "Введите email получателя!")]
    [EmailAddress]
    public string Email { get; set; }
    [Required(ErrorMessage = "Введите тему сообщения!")]
    public string Subject { get; set; }
    [Required(ErrorMessage = "Введите текст сообщения")]    
    public string Text { get; set; }
}