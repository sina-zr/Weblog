using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Weblog.DataLayer.Context;
using Weblog.DataLayer.Entities;
using Weblog.Web.DTOs;
using Weblog.Web.Helper;

namespace Weblog.Web;

public sealed class ChatHub : Hub
{
    private readonly WeblogContext _context;

    public ChatHub(WeblogContext context)
    {
        _context = context;
    }
    
    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("ReceiveMessage", $"{Context.ConnectionId} has joined");
    }

    public async Task SendMessage(int groupId, string text)
    {
        var userId = int.Parse(Context.UserIdentifier);
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return;
        }
        
        var newMessage = new ChatMessage()
        {
            UserId = userId,
            GroupId = groupId,
            Text = text
        };
        await _context.AddAsync(newMessage);
        await _context.SaveChangesAsync();

        var group = await _context.ChatGroups.Include(g => g.UserGroups)
            .FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (group == null)
        {
            return;
        }

        var messageVm = new MessageVm()
        {
            MessageId = newMessage.MessageId,
            UserId = newMessage.UserId,
            Text = newMessage.Text,
            Avatar = user.AvatarName ?? "",
            Time = newMessage.SendDateTime,
        };
        var userIDs = group.UserGroups.Select(ug => ug.UserId.ToString()).ToList();
        if (!userIDs.Contains("1")) // making sure admin receives message too
        {
            userIDs.Add("1");
        }
        await Clients.Users(userIDs)
            .SendAsync("ReceiveMessage", messageVm);
    }
}