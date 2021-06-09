using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CSharpSnackisDB.Entities.Reply;

namespace CSharpSnackisDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class APIThreadController : ControllerBase
    {
        private Random _rnd;
        private Context _context;
        public APIThreadController(Context context, Random rnd)
        {
            _rnd = rnd;
            _context = context;
        }
        [HttpGet("GetRandomThread")]
        public async Task<ActionResult> GetRandomThread()
        {
            var threads = await _context.Threads.Include(x => x.Posts).ThenInclude(x => x.Replies).OrderBy(x => x.CreateDate).Include(x => x.User).ToListAsync();
            var randomThread = threads[_rnd.Next(0, threads.Count())];
            var _apiThread = new APIThread();
            _apiThread.BodyText = randomThread.BodyText;
            _apiThread.Title = randomThread.Title;
            _apiThread.CreateDate = randomThread.CreateDate;
            _apiThread.EditDate = randomThread.EditDate;
            _apiThread.ThreadID = randomThread.ThreadID;
            _apiThread.UserName = randomThread.User.UserName;
            _apiThread.Posts = new List<APIPost>();

            foreach (var post in randomThread.Posts)
            {
                var _apiPost = new APIPost();
                _apiPost.Replies = new List<APIReply>();
                foreach (var reply in post.Replies)
                {
                    var _apiReply = new APIReply(); 
                    _apiReply.UserName = reply.User.UserName;
                    _apiReply.BodyText = reply.BodyText;
                    _apiReply.CreateDate = reply.CreateDate;
                    _apiReply.EditDate = reply.EditDate;

                    _apiPost.Replies.Add(_apiReply);
                }
                _apiPost.BodyText = post.BodyText;
                _apiPost.Title = post.Title;
                _apiPost.UserName = post.User.UserName;
                _apiPost.CreateDate = post.CreateDate;
                _apiPost.EditDate = post.EditDate;

                _apiThread.Posts.Add(_apiPost);
            }
            return Ok(_apiThread);
        }
    }
}
