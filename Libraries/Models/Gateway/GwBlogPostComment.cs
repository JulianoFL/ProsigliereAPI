using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Models.Gateway
{
    public class GwBlogPostComment
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Content for the post comment
        /// *Required*
        /// </summary>
        public string Content { get; set; }
    }
}
