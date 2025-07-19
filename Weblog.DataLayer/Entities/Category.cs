using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weblog.DataLayer.Entities;

public class Category
{
    [Key]
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public int? ParentId { get; set; }

    #region Relations

    [ForeignKey("ParentId")]
    public Category Parent { get; set; }

    public ICollection<Category> Categories { get; set; }

    public ICollection<Blog> Blogs { get; set; }

    #endregion
}