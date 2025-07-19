using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Weblog.DataLayer.Entities;

public class User
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }

    public string? Email { get; set; }
    public int? Age { get; set; }
    public bool? IsMale { get; set; } = true;
    public string? Biography { get; set; }

    public string? AvatarName { get; set; } = "default.png";

    #region Relations

    public List<ChatMessage> ChatMessages { get; set; }

    public ICollection<UserGroup> UserGroups { get; set; }

    #endregion
}
