using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weblog.DataLayer.Entities
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        
        public int? SubCategoryId { get; set; }

        #region Relation

        public ICollection<BlogImage>? BlogImages { get; set; }
        
        [ForeignKey("SubCategoryId")]
        public Category? SubCategory { get; set; }

        #endregion
    }
}
