using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult> CreateCategories([FromBody] Category newCategory)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                await _context.Categories.AddAsync(newCategory);
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
                var result = _context.Categories.Where(x => x.CategoryID == id).FirstOrDefault();

                _context.Remove(result);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult> UpdateCategory([FromBody] Category category, string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var updateCategory = _context.Categories.Where(x => x.CategoryID == id).FirstOrDefault();

                updateCategory.Title = category.Title;
                updateCategory.Description = category.Description;
                updateCategory.CreateDate = category.CreateDate;

                _context.Update(updateCategory);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [AllowAnonymous]
        [HttpGet("ReadCategory")]
        public async Task<ActionResult> ReadCategory()
        {
            var allCategories = await _context.Categories.ToListAsync();
            return Ok(allCategories);
        }

        [HttpPost("CreateTopic")]
        public async Task<ActionResult> CreateTopics([FromBody] Topic newTopic)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                await _context.Topics.AddAsync(newTopic);
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
                var result = _context.Topics.Where(x => x.TopicID == id).FirstOrDefault();

                _context.Remove(result);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdateTopic/{id}")]
        public async Task<ActionResult> UpdateTopic([FromBody] Topic topics, string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var updateTopic = _context.Topics.Where(x => x.TopicID == id).FirstOrDefault();

                updateTopic.Title = topics.Title;
                updateTopic.Category = topics.Category;
                updateTopic.CreateDate = topics.CreateDate;

                _context.Update(updateTopic);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpGet("ReadTopic")]
        public async Task<ActionResult> ReadTopics()
        {
            var allTopics = await _context.Topics.ToListAsync();
            return Ok(allTopics);
        }


    }
}
