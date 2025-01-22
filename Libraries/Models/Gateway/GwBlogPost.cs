using Libraries.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Models.Gateway
{
    public class GwBlogPost
    {
        /// <summary>
        /// Key
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Post title
        /// *Required*
        /// </summary>
        public string Title { get; set; }


        /// <summary>
        /// Post content
        /// *Required*
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// List of comments per post
        /// </summary>
        public List<GwBlogPostComment> Comments { get; set; } = new List<GwBlogPostComment>();
    }
}
