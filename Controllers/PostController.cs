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
    public class UserPostController : ControllerBase
    {
        private const string ApiKey = "localhost:44302";
        private const string FeKey = "";

        private Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserPostController(Context context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("CreateThread")]
        public async Task<ActionResult> CreateThread([FromBody] Thread newThread)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("user"))
            {
                await _context.Threads.AddAsync(newThread);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeleteThread/{id}")]
        public async Task<ActionResult> DeleteThread([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("user"))
            {
                var result = _context.Threads.Where(x => x.ThreadID == id).FirstOrDefault();

                _context.Remove(result);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdateThread/{id}")]
        public async Task<ActionResult> UpdateThread([FromBody] Thread thread, string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("user"))
            {
                var updateThread = _context.Threads.Where(x => x.ThreadID == id).FirstOrDefault();

                updateThread.Title = thread.Title;
                updateThread.BodyText = thread.BodyText;
                updateThread.CreateDate = thread.CreateDate;

                _context.Update(updateThread);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [AllowAnonymous]
        [HttpGet("ReadThread")]
        public async Task<ActionResult> ReadThread()
        {
            var allThreads = await _context.Threads.ToListAsync();
            return Ok(allThreads);
        }

        /// <summary>
        /// /////////////
        /// </summary>
        /// <returns></returns>

        [HttpPost("CreatePost")]
        public async Task<ActionResult> CreatePost([FromBody] Post newPost)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("user"))
            {
                await _context.Posts.AddAsync(newPost);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeletePost/{id}")]
        public async Task<ActionResult> DeletePost([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("user"))
            {
                var result = _context.Posts.Where(x => x.PostID == id).FirstOrDefault();

                _context.Remove(result);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdatePost/{id}")]
        public async Task<ActionResult> UpdatePost([FromBody] Post post, string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("user"))
            {
                var updatePost = _context.Posts.Where(x => x.PostID == id).FirstOrDefault();

                updatePost.Title = post.Title;
                updatePost.BodyText = post.BodyText;
                updatePost.CreateDate = post.CreateDate;

                _context.Update(updatePost);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("ReplyPost/{id}")]
        public async Task<ActionResult> ReplyPost([FromBody] Reply reply, string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("user"))
            {
                var replyPost = _context.Replies.Where(x => x.ReplyID == id).FirstOrDefault();

                replyPost.BodyText = reply.BodyText;
                replyPost.CreateDate = reply.CreateDate;

                _context.Update(replyPost);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }


        [AllowAnonymous]
        [HttpGet("ReadPost")]
        public async Task<ActionResult> ReadPost(Thread thread)
        {
            var selectThread = await _context.Threads.Where(x => x.Topic == thread.Topic).ToListAsync();

            //var allPosts = _context.Posts.Where(x => x)

            return Ok(selectThread);
        }
    }
}
