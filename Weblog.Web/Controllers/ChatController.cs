using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Weblog.DataLayer.Context;
using Weblog.DataLayer.Entities;
using Weblog.Web.DTOs;

namespace Weblog.Web.Controllers;

[Authorize]
public class ChatController : Controller
{
    private readonly WeblogContext _context;

    public ChatController(WeblogContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index()
    {
        // get userId
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        // get groups based on user
        List<ChatGroup> groups;
        if (userId == 1)
        {
            groups = await _context.ChatGroups.Where(g => !g.IsDelete).ToListAsync();
        }
        else
        {
            groups = await _context.ChatGroups.Include(g => g.UserGroups)
                .Where(g => !g.IsDelete && g.UserGroups.Any(ug => ug.UserId == userId)).ToListAsync();
        }
        
        ViewBag.User = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        
        return View(groups);
    }
    
    [HttpGet("/GetGroupMessages/{groupId}")]
    [ProducesResponseType(typeof(List<MessageVm>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGroupMessages(int groupId, int pageId = 1, int pageSize = 10)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if (groupId < 1)
        {
            return BadRequest();
        }
        var group = await _context.ChatGroups.Include(g => g.UserGroups).FirstOrDefaultAsync(g => g.GroupId == groupId);
        if (group == null)
        {
            return NotFound();
        }
        if (group.UserGroups.All(ug => ug.UserId != userId) && userId != 1)
        {
            return BadRequest("Your not a member of this group.");
        }
        
        var query = _context.ChatMessages
            .Include(m => m.User)
            .Where(m => m.GroupId == groupId).AsQueryable();
        
        #region Pagination

        int take = pageSize;
        int pageCount = (int)(Math.Ceiling((decimal)query.Count() / take));
        if (pageId < 1)
        {
            pageId = 1;
        }

        if (pageId > pageCount)
        {
            pageId = pageCount;
        }

        int skip = (pageId - 1) * take;
        if (skip < 0)
        {
            skip = 0;
        }

        #endregion
        
        var messages = await query.OrderByDescending(m => m.SendDateTime).Skip(skip).Take(take)
            .Select(m => new MessageVm()
            {
                MessageId = m.MessageId,
                UserId = m.UserId,
                Text = m.Text,
                Avatar = m.User.AvatarName,
                Time = m.SendDateTime,
            })
            .ToListAsync();

        return Ok(messages);
    }
}