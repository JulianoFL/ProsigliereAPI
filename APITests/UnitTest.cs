using Moq;
using Xunit;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using ProsigliereAPI.Models.DBModels;
using Microsoft.Extensions.Hosting;


namespace ProsigliereAPI.Tests.DBModels
{
    public class DBRepositoryTestCases:IClassFixture<DBRepositoryFixture>, IDisposable
    {
        private readonly BlogPostRepository DBRepo;

        //A GUID where can find the tests posts and comments of the tests
        private string SimpleTestCase { get; init; }

        //A test post id
        private int TestPost { get; init; }

        public DBRepositoryTestCases(DBRepositoryFixture Fixture)
        {
            DBRepo = Fixture.DBRepo;
            SimpleTestCase = Fixture.SimpleTestCase;
            TestPost = Fixture.TestPost;
        }


        [Fact]
        public void NewPost_ShouldAddNewPost()
        {
            // Arrange
            var NewPost = new DBBlogPost { Title = "New Post", Content = SimpleTestCase };

            // Act
            var Result = DBRepo.NewPost(NewPost);


            var Check = DBRepo.GetById(Result.Id);

            // Assert
            Assert.NotNull(Check);
            Assert.Equal(SimpleTestCase, Result.Content);
        }


        [Fact]
        public void GetAllPosts_ShouldReturnTestPost()
        {
            var Check = DBRepo.GetAllPosts();

            Assert.NotNull(Check);
            Assert.Contains(Check, x => x.Id == TestPost);
        }

        [Fact]
        public void GetById_ShouldReturnPost_WhenPostExists()
        {
            var Check = DBRepo.GetById(TestPost);

            Assert.NotNull(Check);
            Assert.Equal(TestPost, Check.Id);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenPostNotExist()
        {
            // Act
            var Check = DBRepo.GetById(9999);

            // Assert
            Assert.Null(Check);
        }


        [Fact]
        public void AddComment_ShouldAddComment()
        {
            // Arrange
            var NewComment = new DBBlogPostComment { Content = SimpleTestCase };

            // Act
            var Check = DBRepo.AddCommentPost(TestPost, NewComment);

            // Assert
            Assert.NotNull(Check);
            Assert.Equal(1, Check.Comments.Count);
            Assert.Equal(SimpleTestCase, Check.Comments[0].Content);
        }

        [Fact]
        public void AddCommentPost_ShouldThrowException_WhenPostNotFound()
        {
            // Arrange
            var NewComment = new DBBlogPostComment { Content = "New Comment" };

            // Act & Assert
            var Exception = Assert.Throws<Exception>(() => DBRepo.AddCommentPost(9999, NewComment));
            Assert.Contains("Post not found", Exception.Message);
        }

        public void Dispose()
        {
            new LiteDatabase(":memory:").GetCollection<DBBlogPost>("blog_posts").Delete(TestPost);
        }
    }



    public class DBRepositoryFixture
    {
        public BlogPostRepository DBRepo { get; }
        public int TestPost;
        public string SimpleTestCase;

        public DBRepositoryFixture()
        {
            SimpleTestCase = Guid.NewGuid().ToString();

            DBBlogPost NewPost = new DBBlogPost();
            NewPost.Content = SimpleTestCase;
            NewPost.Title = "Test case";

            var Database = new LiteDatabase(":memory:");
            var PostCollection = Database.GetCollection<DBBlogPost>(BlogPostRepository.PostsCollectionName);

            DBRepo = new BlogPostRepository(Database);

            PostCollection.Insert(NewPost);

            TestPost = NewPost.Id;
        }
    }
}