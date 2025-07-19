namespace Weblog.Web.DTOs
{
    public class CreateBlogDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int? SubCategoryId { get; set; }
        public List<IFormFile> Images { get; set; }
    }

    public class CreateBlogDtoOutbound
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
