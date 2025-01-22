using LiteDB;

namespace ProsigliereAPI.Models.DBModels
{
    /// <summary>
    /// DBModel for storing a blog post
    /// </summary>
    public class DBBlogPost
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
        public string Content {  get; set; }  

        /// <summary>
        /// List of comments per post
        /// </summary>
        public List<DBBlogPostComment> Comments { get; set; } = new List<DBBlogPostComment>();
    }
}
