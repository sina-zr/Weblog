namespace Weblog.Web.DTOs;

public class EditBlogDto
{
    public int BlogId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int? SubCategoryId { get; set; }
    public List<IFormFile> Images { get; set; }
}