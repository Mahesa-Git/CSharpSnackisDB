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
        private const string FeKey = "localhost:44335";

        private Context _context;
        private readonly UserManager<User> _userManager;

        public AdminPostController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private async Task<bool> IsAdmin()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("root") || roles.Contains("admin"))
                return true;
            else
                return false;
        }

        #region CATEGORIES CRUD REGION
        [HttpGet("GetStatistics")]
        public async Task<ActionResult> GetStatistics()
        {
            if (IsAdmin().Result == true)
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
            if (IsAdmin().Result == true)
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
            if (IsAdmin().Result == true)
            {
                var category = await _context.Categories.Where(x => x.CategoryID == id).Include(x => x.Topics).FirstAsync();
                foreach (var topic in category.Topics)
                {
                    topic.Threads = await _context.Threads.Where(x => x.Topic.TopicID == topic.TopicID).Include(x => x.Posts).ToListAsync();

                    foreach (var thread in topic.Threads)
                    {
                        thread.Posts = await _context.Posts.Where(x => x.Thread.ThreadID == thread.ThreadID).Include(x => x.Replies).ToListAsync();
                    }
                }

                _context.RemoveRange(category);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult> UpdateCategory([FromBody] CategoryResponseModel updateCategory, string id)
        {
            if (IsAdmin().Result == true)
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

        #region TOPICS CRUD REGION
        [HttpPost("CreateTopic")]
        public async Task<ActionResult> CreateTopics([FromBody] TopicResponseModel newTopic)
        {
            if (IsAdmin().Result == true)
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
            if (IsAdmin().Result == true)
            {
                var topic = await _context.Topics.Where(x => x.TopicID == id).Include(x => x.Threads).FirstAsync();

                foreach (var thread in topic.Threads)
                {
                    thread.Posts = await _context.Posts.Where(x => x.Thread.ThreadID == thread.ThreadID).Include(x => x.Replies).ToListAsync();

                }
                _context.RemoveRange(topic);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdateTopic/{newCategoryId}")]
        public async Task<ActionResult> UpdateTopic([FromBody] Topic inputTopic, string newCategoryId)
        {
            if (IsAdmin().Result == true)
            {
                var newCategory = _context.Categories.Where(x => x.CategoryID == newCategoryId).FirstOrDefault();
                var topic = inputTopic;
                topic.Category = newCategory;

                _context.Update(topic);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        #endregion

        #region GET REPORTED POSTS AND REPLIES REGION
        [HttpGet("GetReportedPosts")]
        public async Task<ActionResult> GetReportedPost()
        {
            if (IsAdmin().Result == true)
            {
                var reportedPosts = await _context.Posts.Where(x => x.IsReported == true).ToListAsync();
                return Ok(reportedPosts);
            }
            else
                return Unauthorized();
        }

        [HttpGet("GetReportedReplies")]
        public async Task<ActionResult> GetReportedReplies()
        {
            if (IsAdmin().Result == true)
            {
                var reportedReplies = await _context.Replies.Where(x => x.IsReported == true).ToListAsync();
                return Ok(reportedReplies);
            }
            else
                return Unauthorized();
        }

        [HttpGet("GetReportedThreads")]
        public async Task<ActionResult> GetReportedThreads()
        {
            if (IsAdmin().Result == true)
            {
                var reportedThreads = await _context.Threads.Where(x => x.IsReported == true).ToListAsync();
                return Ok(reportedThreads);
            }
            else
                return Unauthorized();
        }
        #endregion
    }
}
