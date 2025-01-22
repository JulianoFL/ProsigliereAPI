using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Models.Parameters
{
    public class JBlogPostComment
    {
        /// <summary>
        /// Content for the post comment
        /// *Required*
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "is required")]
        [MaxLength(100, ErrorMessage = "máx 100 caracters")]
        public string Content { get; set; }
    }
}
