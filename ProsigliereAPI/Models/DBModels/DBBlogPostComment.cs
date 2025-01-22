using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace ProsigliereAPI.Models.DBModels
{
    /// <summary>
    /// DBModel for the commentaries for a blog post
    /// </summary>
    public class DBBlogPostComment
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
