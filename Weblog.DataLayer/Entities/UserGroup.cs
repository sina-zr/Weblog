using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Weblog.DataLayer.Entities;

public class UserGroup
{
    [Key]
    public ulong UserGroupId { get; set; }

    public int UserId { get; set; }

    public int GroupId { get; set; }

    #region Relations

    [ForeignKey("UserId")]
    public User User { get; set; }

    [ForeignKey("GroupId")]
    public ChatGroup Group { get; set; }

    #endregion
}