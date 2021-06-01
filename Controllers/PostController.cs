using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static CSharpSnackisDB.Entities.Reply;

namespace CSharpSnackisDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private const string ApiKey = "localhost:44302";
        private const string FeKey = "";

        private Context _context;
        private readonly UserManager<User> _userManager;

        public PostController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region READ DATA REGION
        [AllowAnonymous]
        [HttpGet("ReadCategory")]
        public async Task<ActionResult> ReadCategories()
        {
            var allCategories = await _context.Categories.OrderBy(x => x.CreateDate).ToListAsync();
            return Ok(allCategories);
        }

        [AllowAnonymous]
        [HttpGet("ReadTopicsInCategory/{categoryId}")]
        public async Task<ActionResult> ReadTopicsInCategory(string categoryID)
        {
            var category = await _context.Categories.Where(x => x.CategoryID == categoryID).FirstAsync();
            var allTopics = await _context.Topics.Where(x => x.Category == category).OrderBy(x => x.CreateDate).ToListAsync();
            return Ok(allTopics);
        }

        [AllowAnonymous]
        [HttpGet("ReadThreadsInTopic/{TopicId}")]
        public async Task<ActionResult> ReadThreadsInTopics(string topicID)
        {
            var topic = await _context.Topics.Where(x => x.TopicID == topicID).FirstAsync();
            var allThreads = await _context.Threads.Where(x => x.Topic == topic).OrderBy(x => x.CreateDate).ToListAsync();
            return Ok(allThreads);
        }

        [AllowAnonymous]
        [HttpGet("ReadPostsInThread/{ThreadId}")]
        public async Task<ActionResult> ReadPostsInThread(string threadID)
        {
            var thread = await _context.Threads.Where(x => x.ThreadID == threadID).FirstAsync();
            var allPosts = await _context.Posts.Where(x => x.Thread == thread).OrderBy(x => x.CreateDate).Include(x => x.User).ToListAsync();

            return Ok(allPosts);
        }

        [AllowAnonymous]
        [HttpGet("ReadRepliesToPost/{PostId}")]
        public async Task<ActionResult> ReadRepliesToPost(string postID)
        {
            var post = await _context.Posts.Where(x => x.PostID == postID).FirstAsync();
            var allReplies = await _context.Replies.Where(x => x.Post == post).OrderBy(x => x.CreateDate).Include(x => x.User).ToListAsync();

            return Ok(allReplies);
        }
        #endregion

        #region THREADS CRUD REGION
        [HttpPost("CreateThread")] //godkänd
        public async Task<ActionResult> CreateThread([FromBody] ThreadResponseModel thread)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (user is not null)
            {
                var topic = await _context.Topics.Where(x => x.TopicID == thread.TopicId).FirstAsync();
                var newThread = new Thread
                {
                    Title = thread.Title,
                    BodyText = thread.BodyText,
                    Topic = topic,
                    User = user,
                    UserId = user.Id
                };
                await _context.Threads.AddAsync(newThread);
                await _context.SaveChangesAsync();

                return Ok(newThread.ThreadID);
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeleteThread/{id}")]
        public async Task<ActionResult> DeleteThread([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            var result = await _context.Threads.Where(x => x.ThreadID == id).Include(x => x.User).Include(x => x.Posts).FirstAsync();
            foreach (var post in result.Posts)
            {
                post.Replies = await _context.Replies.Where(x => x.Post.PostID == post.PostID).ToListAsync();
            }

            if (roles.Contains("root") || roles.Contains("admin") || result.User.Id == user.Id)
            {
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

            var updateThread = await _context.Threads.Where(x => x.ThreadID == id).FirstAsync();

            if (roles.Contains("root") || roles.Contains("admin") || updateThread.User.Id == user.Id)
            {
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


        #endregion REGION

        #region POST CRUD REGION
        [HttpPost("CreatePost")] //godkänd
        public async Task<ActionResult> CreatePost([FromBody] PostResponseModel post)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (user is not null)
            {
                var thread = await _context.Threads.Where(x => x.ThreadID == post.ThreadId).FirstAsync();
                var newPost = new Post
                {
                    Title = post.Title,
                    BodyText = post.BodyText,
                    Thread = thread,
                    User = user,
                    UserId = user.Id,
                    IsThreadStart = post.IsThreadStart
                };

                await _context.Posts.AddAsync(newPost);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeletePost/{id}")] //GODKÄND
        public async Task<ActionResult> DeletePosts([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            var post = await _context.Posts.Where(x => x.PostID == id).Include(x => x.Replies).Include(x => x.User).Include(x => x.Thread).FirstAsync();

            if (roles.Contains("root") || roles.Contains("admin") || post.User.Id == user.Id)
            {
                if (post.IsThreadStart)
                {
                    ActionResult deletethread = await DeleteThread(post.Thread.ThreadID);
                }

                else if (post.Replies.Count > 0)
                     _context.RemoveRange(post.Replies);

                _context.Remove(post);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdatePost/{id}")]//godkänd
        public async Task<ActionResult> UpdatePost([FromBody] Post post, string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || post.User.Id == user.Id)
            {
                var updatePost = await _context.Posts.Where(x => x.PostID == id).FirstAsync();

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
        #endregion

        #region REPLY CRUD REGION
        [HttpPost("CreateReply")]//godkänd
        public async Task<ActionResult> CreateReply([FromBody] ReplyResponseModel reply)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (user is not null)
            {
                var post = _context.Posts.Where(x => x.PostID == reply.PostId).FirstOrDefault();

                var newReply = new Reply
                {
                    Post = post,
                    BodyText = reply.BodyText,
                    User = user,
                    UserId = user.Id
                };
                _context.Add(newReply);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpPut("UpdateReply")] //Godkänd
        public async Task<ActionResult> UpdateReply([FromBody] Reply reply)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || reply.User.Id == user.Id)
            {
                var replyResult = await _context.Replies.Where(x => x.ReplyID == reply.ReplyID).FirstAsync();

                replyResult.BodyText = reply.BodyText;
                replyResult.CreateDate = reply.CreateDate;

                _context.Update(replyResult);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeleteReply/{id}")] //GODKÄND
        public async Task<ActionResult> DeleteReply([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            var result = await _context.Replies.Where(x => x.ReplyID == id).Include(x => x.User).FirstAsync();

            if (roles.Contains("root") || roles.Contains("admin") || result.User.Id == user.Id)
            {
                _context.Remove(result);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        #endregion
    }
}
