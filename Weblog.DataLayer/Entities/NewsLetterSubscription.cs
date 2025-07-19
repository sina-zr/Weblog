using System.ComponentModel.DataAnnotations;

namespace Weblog.DataLayer.Entities;

public class NewsLetterSubscription
{
    [Key]
    public ulong Id { get; set; }

    [Required]
    public string Email { get; set; }
}