using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                var replies = await _context.Replies.CountAsync();
                var threads = await _context.Threads.CountAsync();
                var users = await _context.Users.CountAsync();

                var reportedUsers = await _context.Users.Where(x => x.IsReported == true).CountAsync();
                var reportedPosts = await _context.Posts.Where(x => x.IsReported == true).CountAsync();
                var reportedReplies = await _context.Replies.Where(x => x.IsReported == true).CountAsync();
                var reportedThreads = await _context.Threads.Where(x => x.IsReported == true).CountAsync();
                var totalReports = reportedUsers + reportedPosts + reportedReplies + reportedThreads;

                int[] returnArr = new int[] { users, posts + replies, threads, totalReports };

                return Ok(returnArr);
            }
            else
                return Unauthorized();
        }

        [HttpGet("GetReportedObjects")]
        public async Task<ActionResult> GetReportedObjects()
        {
            if (IsAdmin().Result == true)
            {
                var reportedUsers = await _context.Users.Where(x => x.IsReported == true).ToListAsync();
                var reportedPosts = await _context.Posts.Where(x => x.IsReported == true).Include(x => x.Thread).ToListAsync();
                var reportedReplies = await _context.Replies.Where(x => x.IsReported == true).Include(x => x.Post).ThenInclude(x => x.Thread).ToListAsync();
                var reportedThreads = await _context.Threads.Where(x => x.IsReported == true).Include(x => x.Topic).ToListAsync();
                var returnValues = new List<object>
                {
                    reportedUsers,
                    reportedPosts,
                    reportedReplies,
                    reportedThreads
                };
                return Ok(returnValues);
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

        [HttpPut("UpdateTopic/{topicId}")]
        public async Task<ActionResult> UpdateTopic([FromBody] Topic inputTopic, [FromRoute] string topicId)
        {
            if (IsAdmin().Result == true)
            {
                var newTopicName = await _context.Topics.Where(x => x.TopicID == topicId).FirstAsync();

                newTopicName.Title = inputTopic.Title;

                //var topic = inputTopic;
                //topic = newTopicName;

                _context.Update(newTopicName);
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
