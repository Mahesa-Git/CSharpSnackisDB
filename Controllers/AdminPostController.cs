using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AdminPostController : ControllerBase
    {
        private const string ApiKey = "localhost:44302";
        private const string FeKey = "";

        private Context _context;
        private readonly UserManager<User> _userManager;

        public AdminPostController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region CATEGORIES CRUD
        [HttpGet("GetStatistics")]
        public async Task<ActionResult> GetStatistics()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var posts = await _context.Posts.CountAsync();
                var threads = await _context.Threads.CountAsync();
                var categories = await _context.Categories.CountAsync();
                var users = await _context.Users.CountAsync();

                int[] returnArr = new int[] { posts, threads, categories, users };

                return Ok(returnArr);
            }
            else
                return Unauthorized();
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult> CreateCategories([FromBody] CategoryResponseModel newCategory)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var category = new Category
                {
                    Title = newCategory.Title,
                    Description = newCategory.Description
                };
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<ActionResult> DeleteCategories([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var category = _context.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
                var topics = category.Topics;

                foreach (var topic in topics)
                {
                    foreach (var thread in topic.Threads)
                    {
                        foreach (var post in thread.Posts)
                        {
                            foreach (var reply in post.Replies)
                            {
                                _context.Remove(reply);
                            }
                            _context.Remove(post);
                        }
                        _context.Remove(thread);
                    }
                    _context.Remove(topic);
                }

                _context.Remove(category);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult> UpdateCategory([FromBody] CategoryResponseModel updateCategory, string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var category = _context.Categories.Where(x => x.CategoryID == id).FirstOrDefault();

                category.Title = updateCategory.Title;
                category.Description = updateCategory.Description;
                category.CreateDate = DateTime.Now;

                _context.Update(updateCategory);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        #endregion

        #region TOPICS CRUD
        [HttpPost("CreateTopic")]
        public async Task<ActionResult> CreateTopics([FromBody] TopicResponseModel newTopic)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var category = _context.Categories.Where(x => x.CategoryID == newTopic.CategoryId).FirstOrDefault();
                var topic = new Topic
                {
                    Title = newTopic.Title,
                    Category = category
                };

                await _context.Topics.AddAsync(topic);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeleteTopic/{id}")]
        public async Task<ActionResult> DeleteTopics([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var topic = _context.Topics.Where(x => x.TopicID == id).FirstOrDefault();
                var threads = topic.Threads;
                foreach (var thread in threads)
                {
                    foreach (var post in thread.Posts)
                    {
                        foreach (var reply in post.Replies)
                        {
                            _context.Remove(reply);
                        }
                        _context.Remove(post);
                    }
                    _context.Remove(thread);
                }
                _context.Remove(topic);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdateTopic/{TopicId}")]
        public async Task<ActionResult> UpdateTopic([FromBody] Topic inputTopic, Category categoryToChange)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var topic = inputTopic;
                topic.Category = categoryToChange;

                _context.Update(topic);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        #endregion
    }
}
