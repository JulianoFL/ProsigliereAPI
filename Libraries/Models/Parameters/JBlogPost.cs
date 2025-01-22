using Libraries.Models.Gateway;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Models.Parameters
{
    public class JBlogPost
    {
        /// <summary>
        /// Post title
        /// *Required*
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "is required")]
        [MaxLength(10, ErrorMessage = "máx 10 caracters")]
        public string Title { get; set; }


        /// <summary>
        /// Post content
        /// *Required*
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "is required")]
        [MaxLength(100, ErrorMessage = "máx 100 caracters")]
        public string Content { get; set; }
    }
}
