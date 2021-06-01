using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class SeedController : ControllerBase
    {
        private Context _context;
        private readonly UserManager<User> _userManager;

        public SeedController(Context context, UserManager<User> userManager)
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
        [HttpPost("SeedCategories")]
        public async Task<ActionResult> SeedCategories()
        {
            if (IsAdmin().Result == true)
            {
                var category = new Category
                {
                    Title = "Bilar",
                    Description = "Here you can discuss cars"
                };
                var category1 = new Category
                {
                    Title = "MC",
                    Description = "Here you can discuss motorcycles"
                };
                var category2 = new Category
                {
                    Title = "Boats",
                    Description = "Here you can discuss variuos boats"
                };
                _context.Add(category);
                _context.Add(category1);
                _context.Add(category2);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest();
        }
        [HttpPost("SeedTopics")]
        public async Task<ActionResult> SeedTopics()
        {
            if (IsAdmin().Result == true)
            {
                var category = _context.Categories.Where(x => x.CategoryID == "0a662320-6508-499d-bd60-768f0bae30f3").FirstOrDefault();
                var topic = new Topic
                {
                    Title = "Ford Mustang Shelby",
                    Category = category
                };
                var topic1 = new Topic
                {
                    Title = "Ferrari 458",
                    Category = category
                };
                var topic2 = new Topic
                {
                    Title = "Lamborghini Aventador",
                    Category = category
                };

                var category1 = _context.Categories.Where(x => x.CategoryID == "88a542c3-f658-497e-9a1f-a8e9b1e24aba").FirstOrDefault();
                var topic3 = new Topic
                {
                    Title = "Triumph Street Triple 675",
                    Category = category1
                };
                var topic4 = new Topic
                {
                    Title = "MV Agusta Brutale",
                    Category = category1
                };
                var topic5 = new Topic
                {
                    Title = "Yamaha MT-07",
                    Category = category1
                };

                var category2 = _context.Categories.Where(x => x.CategoryID == "2cc1a336-2e03-4f05-8b04-575f11f1e671").FirstOrDefault();
                var topic6 = new Topic
                {
                    Title = "Silja Line",
                    Category = category2
                };
                var topic7 = new Topic
                {
                    Title = "Viking Line",
                    Category = category2
                };
                var topic8 = new Topic
                {
                    Title = "Speed boats",
                    Category = category2
                };
                _context.Add(topic);
                _context.Add(topic1);
                _context.Add(topic2);
                _context.Add(topic3);
                _context.Add(topic4);
                _context.Add(topic5);
                _context.Add(topic6);
                _context.Add(topic7);
                _context.Add(topic8);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest();
        }

        [HttpPost("SeedThreads")]
        public async Task<ActionResult> SeedThreads()
        {
            if (IsAdmin().Result == true)
            {
                User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

                var topic = _context.Topics.Where(x => x.TopicID == "03e01867-c726-4b90-a2cf-eca57977ccc1").FirstOrDefault();
                var thread = new Thread
                {
                    Title = "Är Yamaha MT-07 världens bästa hoj?",
                    BodyText = "Jag tycker det",
                    Topic = topic,
                    User = user
                };
                var thread1 = new Thread
                {
                    Title = "Yamahahaha",
                    BodyText = "Fet den e asså!",
                    Topic = topic,
                    User = user
                };

                var topic1 = _context.Topics.Where(x => x.TopicID == "2a4f9684-d2ed-4185-bae0-ab7859992e1d").FirstOrDefault();
                var thread2 = new Thread
                {
                    Title = "Street Triplen är en riktigt tung street fighter!",
                    BodyText = "Alla borde ge den en chans!",
                    Topic = topic1,
                    User = user
                };
                var thread3 = new Thread
                {
                    Title = "Trajja är något för alla fartälskare azzuh",
                    BodyText = "Fet den eeeee",
                    Topic = topic1,
                    User = user
                };
                await _context.AddAsync(thread);
                await _context.AddAsync(thread1);
                await _context.AddAsync(thread2);
                await _context.AddAsync(thread3);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpPost("SeedPosts")]
        public async Task<ActionResult> SeedPosts()
        {
            if (IsAdmin().Result == true)
            {
                User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

                var thread = _context.Threads.Where(x => x.ThreadID == "74cc846d-ae83-4cd8-be61-f7a330736721").FirstOrDefault();
                var post = new Post
                {
                    Title = "Jag håller med",
                    BodyText = "Yamahahahan är helt galen!",
                    User = user,
                    Thread = thread
                };

                var thread1 = _context.Threads.Where(x => x.ThreadID == "ce5296ed-677a-4e76-a30e-9e2c7cc855ae").FirstOrDefault();
                var post1 = new Post
                {
                    Title = "Triumph 4 life!",
                    BodyText = "Inget annat är värt att ha...",
                    User = user,
                    Thread = thread1
                };
                await _context.AddAsync(post);
                await _context.AddAsync(post1);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpPost("SeedReplies")]
        public async Task<ActionResult> SeedReplies()
        {
            if (IsAdmin().Result == true)
            {
                User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

                var post = _context.Posts.Where(x => x.PostID == "21df90f4-35d5-4705-9eea-6f15f23843a7").FirstOrDefault();
                var reply = new Reply
                {
                    BodyText = "hahaha!! Yamahahaha skön du är",
                    User = user,
                    Post = post
                };
                await _context.AddAsync(reply);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
    }
}
