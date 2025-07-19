using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weblog.DataLayer.Entities
{
    public class BlogImage
    {
        [Key]
        public int ImageId { get; set; }
        public int BlogId { get; set; }
        public string ImageName { get; set; }


        #region Relations

        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }

        #endregion
    }
}
