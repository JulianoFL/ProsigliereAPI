using AutoMapper;
using Libraries.Models.Gateway;
using Libraries.Models.Parameters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProsigliereAPI.Models.DBModels;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ProsigliereAPI.Controllers
{
    /// <summary>
    /// Basic post functions route
    /// </summary>
    [ApiController]
    [Route("/api/posts", Name = "Posts functions route")]
    public class PostsController :BaseController
    {
        public PostsController(IBlogPostRepository DBRepo, ILogger<PostsController> Logger, IMapper Mapper) : base(DBRepo, Logger, Mapper)
        {
        }


        /// <summary>
        /// Returns a list of all posts
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(typeof(List<GwBlogPost>), 200)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(GwBlogPost))]        
        public ActionResult GetAll()
        {
            return Ok(Mapper.Map<List<GwBlogPost>>(DBRepo.GetAllPosts()));
        }

        /// <summary>
        /// Return a post by Id
        /// </summary>
        /// <param name="id" example="100">Post id</param>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(List<GwBlogPost>), 200)]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(GwBlogPost))]        
        public ActionResult GetById([Required]int id)
        {
            return Ok(Mapper.Map<GwBlogPost>(DBRepo.GetById(id)));
        }


        /// <summary>
        /// Creates a new post
        /// </summary>        
        [HttpPost()]
        [ProducesResponseType(typeof(List<GwBlogPost>), 200)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ModelStateDictionary))]
        public ActionResult NewPost([FromBody] JBlogPost NewPost)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DBBlogPost DBPost = Mapper.Map<DBBlogPost>(NewPost);

            return Ok(DBRepo.NewPost(Mapper.Map<DBBlogPost>(DBPost)));
        }

        /// <summary>
        /// Adds a commentary to a post
        /// </summary>
        /// <param name="id" example="100">id of the post</param>
        /// <param name="NewComment">object with comentary to add to a post</param>        
        [HttpPost("{id}/comments")]
        [ProducesResponseType(typeof(List<GwBlogPost>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(NotFoundResult))]
        //[ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ModelStateDictionary))]
        public ActionResult AddCommentPost([Required]int? id, [FromBody] JBlogPostComment NewComment)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            DBBlogPostComment DBComment = Mapper.Map<DBBlogPostComment>(NewComment);

            try
            {
                return Ok(DBRepo.AddCommentPost((int)id, DBComment));
            }
            catch(Exception E) 
            {
                return BadRequest(E.Message);
            }
        }
    }
}
