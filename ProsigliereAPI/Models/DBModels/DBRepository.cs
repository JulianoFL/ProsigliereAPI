using LiteDB;
using Microsoft.Extensions.Hosting;
using ProsigliereAPI.Models.DBModels;
using System.Xml.Linq;

namespace ProsigliereAPI.Models.DBModels
{
    public interface IBlogPostRepository
    {
        /// <summary>
        /// Returns all posts
        /// </summary>
        /// <returns>List of posts</returns>
        List<DBBlogPost> GetAllPosts();

        /// <summary>
        /// Get a post by it's Id
        /// Returns a **BadRequest** to a invalid Id
        /// </summary>
        /// <param name="Id">Id of the post</param>
        /// <returns>Post found, or *NULL* to post not found</returns>
        DBBlogPost? GetById(int Id);

        /// <summary>
        /// Create a new post on the blog
        /// </summary>
        /// <param name="Post">New post to create</param>
        DBBlogPost NewPost(DBBlogPost Post);

        /// <summary>
        /// Add a new comments to a post
        /// </summary>
        /// <remarks>
        /// Can thrown an exception on post not found
        /// </remarks>
        /// <param name="NewComment">Comment to add</param>
        DBBlogPost AddCommentPost(int PostId, DBBlogPostComment NewComment);
    }


    //https://localhost:7274/swagger/index.html
    public class BlogPostRepository:IBlogPostRepository
    {
        private readonly LiteDatabase Database;

        //For naming the collections in DB
        public static readonly string PostsCollectionName = "blog_posts";
        public static readonly string PostsCommentsCollectionName = "blog_post_comments";

        public BlogPostRepository(LiteDatabase Database)
        {
            var Mapper = BsonMapper.Global;

            //map id and field names of the document
            Mapper.Entity<DBBlogPost>()
                .Id(x => x.Id, true)
                .Field(x => x.Title, nameof(DBBlogPost.Title).ToLower())
                .Field(x => x.Content, nameof(DBBlogPost.Content).ToLower())
                .DbRef(x => x.Comments, PostsCommentsCollectionName);

            Mapper.Entity<DBBlogPostComment>()
               .Id(x => x.Id, true)
               .Field(x => x.Content, nameof(DBBlogPostComment.Content).ToLower());


            this.Database = Database;


            #region Tests

            if(GetAllPosts().Count == 0)
            {
                DBBlogPost Post = new DBBlogPost();
                Post.Content = "Content added at init of applicaction";
                Post.Title = "Post of testing";

                DBBlogPostComment Comm1 = new DBBlogPostComment();
                Comm1.Content = "Comm 1";

                DBBlogPostComment Comm2 = new DBBlogPostComment();
                Comm2.Content = "Commm 2";

                Post.Comments.Add(Comm1);
                Post.Comments.Add(Comm2);

                NewPost(Post);
            }
            #endregion

        }

        #region IBlogPostRepository
        /// <inheritdoc />
        public List<DBBlogPost> GetAllPosts()
        {
            var PostsCollection = Database.GetCollection<DBBlogPost>(PostsCollectionName).Include(x => x.Comments);

            return PostsCollection.FindAll().ToList();
        }

        /// <inheritdoc />
        public DBBlogPost? GetById(int Id)
        {
            var Collection = Database.GetCollection<DBBlogPost>(PostsCollectionName).Include(x => x.Comments);
            Collection.EnsureIndex(x => x.Id, unique: true);

            var Post = Collection.FindById(Id);


            return Post;
        }

        /// <inheritdoc />
        public DBBlogPost NewPost(DBBlogPost Post)
        {
            var PostCollection = Database.GetCollection<DBBlogPost>(PostsCollectionName).Include(x => x.Comments);
            var CommentsCollection = Database.GetCollection<DBBlogPostComment>(PostsCommentsCollectionName);

            if(Post.Comments != null)
                CommentsCollection.InsertBulk(Post.Comments);

            PostCollection.Insert(Post);

            return Post;
        }

        /// <inheritdoc />
        public DBBlogPost AddCommentPost(int PostId, DBBlogPostComment NewComment)
        {
            var PostCollection = Database.GetCollection<DBBlogPost>(PostsCollectionName).Include(x => x.Comments);
            PostCollection.EnsureIndex(x => x.Id, unique: true);


            var Post = PostCollection.FindById(PostId);

            if(Post == null)
                throw new Exception("Post not found");

            Post.Comments.Add(NewComment);

            var CommentsCollection = Database.GetCollection<DBBlogPostComment>(PostsCommentsCollectionName);
            CommentsCollection.Insert(NewComment);

            PostCollection.Update(Post);

            return Post;
        }

        #endregion


    }
}
