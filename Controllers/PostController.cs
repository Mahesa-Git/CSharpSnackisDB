using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using CSharpSnackisDB.Models;
using CSharpSnackisDB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
            var allThreads = await _context.Threads.Where(x => x.Topic == topic).Include(x => x.User).OrderBy(x => x.CreateDate).ToListAsync();
            return Ok(allThreads);
        }


        [AllowAnonymous]
        [HttpGet("ReadPostsInThread/{ThreadId}")]
        public async Task<ActionResult> ReadPostsInThread(string threadID)
        {
            var thread = await _context.Threads.Where(x => x.ThreadID == threadID).FirstAsync();
            var allPosts = await _context.Posts.Where(x => x.Thread == thread).Include(x => x.PostReaction).OrderBy(x => x.CreateDate).Include(x => x.User).ToListAsync();

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
        [AllowAnonymous]
        [HttpGet("ReadPostReaction/{postID}")]
        public async Task<ActionResult> ReadPostReaction(string postID)
        {
            var post = await _context.Posts.Where(x => x.PostID == postID).Include(x => x.PostReaction).ThenInclude(x => x.Users).FirstAsync();
            return Ok(post.PostReaction);
        }
        [AllowAnonymous]
        [HttpGet("ReadReplyReaction/{replyID}")]
        public async Task<ActionResult> ReadReplyReaction(string replyID)
        {
            var reply = await _context.Replies.Where(x => x.ReplyID == replyID).Include(x => x.PostReaction).ThenInclude(x => x.Users).FirstAsync();
            return Ok(reply.PostReaction);
        }
        #endregion

        #region THREADS CRUD REGION
        [HttpPost("CreateThread")]
        public async Task<ActionResult> CreateThread([FromBody] ThreadResponseModel thread)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (user is not null)
            {
                var listedWords = await _context.FilteredWords.Select(x => x.Words).ToArrayAsync();
                string outputBodyText = FilteredWordsCheck.Filter(thread.BodyText, listedWords);
                string outputTitle = FilteredWordsCheck.Filter(thread.Title, listedWords);

                var topic = await _context.Topics.Where(x => x.TopicID == thread.TopicId).FirstAsync();
                var newThread = new Thread
                {
                    Title = outputTitle,
                    BodyText = outputBodyText,
                    Topic = topic,
                    User = user,
                    Image = thread.Image
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

            var result = await _context.Threads.Where(x => x.ThreadID == id).Include(x => x.User).Include(x => x.Posts).ThenInclude(x => x.PostReaction).FirstAsync();
            foreach (var post in result.Posts)
            {
                post.Replies = await _context.Replies.Where(x => x.Post.PostID == post.PostID).Include(x => x.PostReaction).ToListAsync();
            }

            if (roles.Contains("root") || roles.Contains("admin") || result.User.Id == user.Id)
            {
                foreach (var post in result.Posts)
                {
                    foreach (var reply in post.Replies)
                    {
                        _context.PostReactions.RemoveRange(reply.PostReaction);
                        _context.Replies.RemoveRange(reply);
                    }
                    _context.PostReactions.RemoveRange(post.PostReaction);
                    _context.Posts.RemoveRange(post);
                }
                _context.Remove(result);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdateThread/{id}")]
        public async Task<ActionResult> UpdateThread([FromBody] Thread thread, [FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            var listedWords = await _context.FilteredWords.Select(x => x.Words).ToArrayAsync();
            string outputBodyText = FilteredWordsCheck.Filter(thread.BodyText, listedWords);
            string outputTitle = FilteredWordsCheck.Filter(thread.Title, listedWords);

            var updateThread = await _context.Threads.Where(x => x.ThreadID == id).FirstAsync();

            if (roles.Contains("root") || roles.Contains("admin") || updateThread.User.Id == user.Id)
            {
                updateThread.Title = outputTitle;
                updateThread.BodyText = outputBodyText;
                updateThread.EditDate = thread.EditDate;

                _context.Update(updateThread);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        #endregion REGION

        #region POST CRUD REGION
        [HttpPost("CreatePost")]
        public async Task<ActionResult> CreatePost([FromBody] PostResponseModel post)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (user is not null)
            {
                var listedWords = await _context.FilteredWords.Select(x => x.Words).ToArrayAsync();
                string outputBodyText = FilteredWordsCheck.Filter(post.BodyText, listedWords);
                string outputTitle = FilteredWordsCheck.Filter(post.Title, listedWords);

                var thread = await _context.Threads.Where(x => x.ThreadID == post.ThreadId).FirstAsync();
                var newPost = new Post
                {
                    Title = outputTitle,
                    BodyText = outputBodyText,
                    Thread = thread,
                    User = user,
                    IsThreadStart = post.IsThreadStart,
                    PostReaction = new PostReaction(),
                    Image = post.Image
                };

                await _context.Posts.AddAsync(newPost);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeletePost/{id}")]
        public async Task<ActionResult> DeletePosts([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            var deletePost = await _context.Posts.Where(x => x.PostID == id).Include(x => x.PostReaction).Include(x => x.Replies).ThenInclude(x => x.PostReaction).Include(x => x.User).Include(x => x.Thread).FirstAsync();

            if (roles.Contains("root") || roles.Contains("admin") || deletePost.User.Id == user.Id)
            {

                foreach (var reply in deletePost.Replies)
                {
                    _context.PostReactions.RemoveRange(reply.PostReaction);
                }
                _context.Replies.RemoveRange(deletePost.Replies);
                _context.PostReactions.RemoveRange(deletePost.PostReaction);
                _context.RemoveRange(deletePost);
                await _context.SaveChangesAsync();
                
                if (deletePost.IsThreadStart)
                {
                    ActionResult deletethread = await DeleteThread(deletePost.Thread.ThreadID);
                }
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpPut("UpdatePost/{id}")]
        public async Task<ActionResult> UpdatePost([FromBody] PostResponseModel post, [FromRoute] string id)
        {

            var listedWords = await _context.FilteredWords.Select(x => x.Words).ToArrayAsync();
            string outputBodyText = FilteredWordsCheck.Filter(post.BodyText, listedWords);
            string outputTitle = FilteredWordsCheck.Filter(post.Title, listedWords);

            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);
            var updatePost = await _context.Posts.Where(x => x.PostID == id).Include(x => x.User).Include(x => x.Thread).FirstAsync();

            if (roles.Contains("root") || roles.Contains("admin") || updatePost.User.Id == user.Id)
            {
                updatePost.Title = outputTitle;
                updatePost.BodyText = outputBodyText;
                updatePost.EditDate = DateTime.Now;
                if (updatePost.IsThreadStart)
                {
                    var thread = await _context.Threads.Where(x => x.ThreadID == updatePost.Thread.ThreadID).FirstAsync();
                    thread.Title = outputTitle; 
                    thread.BodyText = outputBodyText;
                    thread.EditDate = DateTime.Now;
                    _context.Update(thread);
                }

                _context.Update(updatePost);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpGet("ReportPost/{id}")]
        public async Task<ActionResult> ReportPost([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("User"))
            {
                var post = await _context.Posts.Where(x => x.PostID == id).FirstAsync();
                post.IsReported = true;
                _context.Update(post);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpPut("ReactToPost")]
        public async Task<ActionResult> ReactToPost([FromBody] ReactionModel reactionModel)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("User"))
            {
                var post = await _context.Posts.Where(x => x.PostID == reactionModel.TextID).Include(x => x.PostReaction).ThenInclude(x => x.Users).FirstAsync();

                if(reactionModel.AddOrRemove)
                {
                    if(!post.PostReaction.Users.Contains(user))
                    {
                        post.PostReaction.Users.Add(user);
                        post.PostReaction.LikeCounter++;
                        _context.PostReactions.Update(post.PostReaction);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                    

                }
                if(!reactionModel.AddOrRemove)
                {
                    post.PostReaction.Users.Remove(user);
                    post.PostReaction.LikeCounter--;
                    _context.PostReactions.Update(post.PostReaction);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        #endregion

        #region REPLY CRUD REGION
        [HttpPost("CreateReply")]
        public async Task<ActionResult> CreateReply([FromBody] ReplyResponseModel reply)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            var listedWords = await _context.FilteredWords.Select(x => x.Words).ToArrayAsync();
            string outputBodyText = FilteredWordsCheck.Filter(reply.BodyText, listedWords);

            if (user is not null)
            {
                var post = _context.Posts.Where(x => x.PostID == reply.PostId).FirstOrDefault();

                var newReply = new Reply
                {
                    Post = post,
                    BodyText = reply.BodyText,
                    User = user,
                    PostReaction = new PostReaction(),
                    Image = reply.Image

                };
                _context.Add(newReply);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpPut("UpdateReply/{id}")] 
        public async Task<ActionResult> UpdateReply([FromBody] ReplyResponseModel reply, [FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);
            var updateReply = await _context.Replies.Where(x => x.ReplyID == id).Include(x => x.User).FirstAsync();

            var listedWords = await _context.FilteredWords.Select(x => x.Words).ToArrayAsync();
            string outputBodyText = FilteredWordsCheck.Filter(reply.BodyText, listedWords);

            if (roles.Contains("root") || roles.Contains("admin") || updateReply.User.Id == user.Id)
            {
                updateReply.BodyText = outputBodyText;
                updateReply.EditDate = DateTime.Now;

                _context.Update(updateReply);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }

        [HttpDelete("DeleteReply/{id}")] 
        public async Task<ActionResult> DeleteReply([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            var deleteReply = await _context.Replies.Where(x => x.ReplyID == id).Include(x => x.User).Include(x => x.PostReaction).FirstAsync();

            if (roles.Contains("root") || roles.Contains("admin") || deleteReply.User.Id == user.Id)
            {
                _context.PostReactions.RemoveRange(deleteReply.PostReaction);
                _context.Remove(deleteReply);

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpGet("ReportReply/{id}")]
        public async Task<ActionResult> ReportReply([FromRoute] string id)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("User"))
            {
                var reply = await _context.Replies.Where(x => x.ReplyID == id).FirstAsync();
                reply.IsReported = true;
                _context.Update(reply);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPut("ReactToReply")]
        public async Task<ActionResult> ReactToReply([FromBody] ReactionModel reactionModel)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin") || roles.Contains("User"))
            {
                var reply = await _context.Replies.Where(x => x.ReplyID == reactionModel.TextID).Include(x => x.PostReaction).ThenInclude(x => x.Users).FirstAsync();

                if (reactionModel.AddOrRemove)
                {
                    if (!reply.PostReaction.Users.Contains(user))
                    {
                        reply.PostReaction.Users.Add(user);
                        reply.PostReaction.LikeCounter++;
                        _context.PostReactions.Update(reply.PostReaction);
                        await _context.SaveChangesAsync();
                        return Ok();
                    }
                }
                if (!reactionModel.AddOrRemove)
                {
                    reply.PostReaction.Users.Remove(user);
                    reply.PostReaction.LikeCounter--;
                    _context.PostReactions.Update(reply.PostReaction);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            else
            {
                return Unauthorized();
            }
        }
        #endregion
    }
}
