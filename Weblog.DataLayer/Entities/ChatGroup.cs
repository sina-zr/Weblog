using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.DataLayer.Entities;

public class ChatGroup
{
    [Key]
    public int GroupId { get; set; }

    [Required]
    public int OwnerId { get; set; }

    [Required]
    public string GroupName { get; set; } = string.Empty;
    public string GroupImageName { get; set; }

    public bool IsDelete { get; set; } = false;

    
    #region Relations
    
    [ForeignKey("OwnerId")]
    public User Owner { get; set; }

    public List<ChatMessage> Messages { get; set; }
    public ICollection<UserGroup> UserGroups { get; set; }

    #endregion
}
