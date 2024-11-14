using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers;
using Models.Catalog;
using Models.DTOs;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Models;

namespace AutoPartsShop.Tests
{
    public class CommentControllerTests
    {
        private readonly CommentController _controller;
        private readonly DataContext _context;

        public CommentControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            SeedDatabase();

            _controller = new CommentController(_context);
        }

        private void SeedDatabase()
        {
            var users = new List<User>
            {
                new User { Id = Guid.NewGuid(), DisplayName = "John Doe" },
                new User { Id = Guid.NewGuid(), DisplayName = "Jane Smith" }
            };
            _context.Users.AddRange(users);

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Laptop", Type = "Electronics", Brand = "Lenovo" },
                new Product { Id = Guid.NewGuid(), Name = "Harry Potter", Type = "Books", Brand = "knygos.lt" }
            };
            _context.Products.AddRange(products);

            var comments = new List<Comment>
            {
                new Comment { Id = Guid.NewGuid(), ProductId = products[0].Id, Content = "Great product!", UserId = users[0].Id, CreatedAt = DateTime.Now },
                new Comment { Id = Guid.NewGuid(), ProductId = products[1].Id, Content = "Loved it!", UserId = users[1].Id, CreatedAt = DateTime.Now }
            };
            _context.Comments.AddRange(comments);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCommentsTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var result = await _controller.GetComments(productId);

            var actionResult = Assert.IsType<ActionResult<IEnumerable<GetComentsByCatalogItemIdDto>>>(result);
            var returnValue = Assert.IsAssignableFrom<IEnumerable<GetComentsByCatalogItemIdDto>>(actionResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task GetCommentTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var commentId = await _context.Comments.Where(c => c.ProductId == productId).Select(c => c.Id).FirstAsync();
            var result = await _controller.GetComment(productId, commentId);

            var actionResult = Assert.IsType<ActionResult<Comment>>(result);
            var returnValue = Assert.IsType<Comment>(actionResult.Value);
            Assert.Equal(commentId, returnValue.Id);
        }

        [Fact]
        public async Task CreateCommentTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var newComment = new CreateCommentDto
            {
                Content = "Amazing!"
            };

            var result = await _controller.CreateComment(productId, newComment);

            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Comment>(actionResult.Value);
            Assert.Equal(newComment.Content, returnValue.Content);
        }

        [Fact]
        public async Task UpdateCommentTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var commentId = await _context.Comments.Where(c => c.ProductId == productId).Select(c => c.Id).FirstAsync();
            var updateComment = new UpdateCommentDto
            {
                Id = commentId,
                ProductId = productId,
                Content = "Updated content"
            };

            var result = await _controller.UpdateComment(productId, commentId, updateComment);

            Assert.IsType<NoContentResult>(result);
            var updatedComment = await _context.Comments.FindAsync(commentId);
            Assert.Equal("Updated content", updatedComment.Content);
        }

        [Fact]
        public async Task DeleteCommentTest()
        {
            var commentId = await _context.Comments.Select(c => c.Id).FirstAsync();
            var result = await _controller.DeleteComment(commentId);

            Assert.IsType<NoContentResult>(result);
            var deletedComment = await _context.Comments.FindAsync(commentId);
            Assert.Null(deletedComment);
        }
    }
}