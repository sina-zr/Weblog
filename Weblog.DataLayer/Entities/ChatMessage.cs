using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weblog.DataLayer.Entities;

public class ChatMessage
{
    [Key]
    public ulong MessageId { get; set; }

    [Required]
    public int UserId { get; set; }

    public int GroupId { get; set; }

    [Required]
    public string Text { get; set; }

    [Required]
    public DateTime SendDateTime { get; set; } = DateTime.Now;

    #region Relations

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("GroupId")]
    public ChatGroup Group { get; set; }

    #endregion
}