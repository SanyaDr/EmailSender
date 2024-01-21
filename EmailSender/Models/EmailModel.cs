using System.ComponentModel.DataAnnotations;

namespace EmailSender.Models;

public class EmailModel
{
    [Required(ErrorMessage = "Введите email получателя!")]
    [EmailAddress]
    public string Email { get; set; }
    public string Subject { get; set; }
    [Required(ErrorMessage = "Введите текст сообщения")]    
    public string Text { get; set; }
}