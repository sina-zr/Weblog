namespace Weblog.Web.DTOs;

public class MessageVm
{
    public ulong MessageId { get; set; }
    public DateTime Time { get; set; }
    public int UserId { get; set; }
    public string Text { get; set; }
    public string Avatar { get; set; }
}