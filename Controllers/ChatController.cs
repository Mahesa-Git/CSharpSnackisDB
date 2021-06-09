using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public class ChatController : ControllerBase
    {
        private Context _context;
        private readonly UserManager<User> _userManager;

        public ChatController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetUsers()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if (user is not null)
            {
                var users = await _context.Users.ToListAsync();
                users.Remove(user);

                return Ok(users);
            }
            return Unauthorized();
        }

        [HttpGet("GetChats")]
        public async Task<ActionResult> GetChats()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            if (user is not null)
            {
                var chatList = await _context.GroupChats.Include(x => x.Users).Include(x => x.Replies.OrderBy(x => x.CreateDate)).Where(x => x.Users.Contains(user)).ToListAsync();
                foreach (var chat in chatList)
                {
                    if(chat.Users.Count < 2)
                    {
                        _context.GroupChats.Remove(chat);
                        await _context.SaveChangesAsync();
                    }
                }
                return Ok(chatList);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("NewChat")]
        public async Task<ActionResult> ReadCategories([FromBody] List<string> newChat)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if (user is not null)
            {
                var users = new List<User>();
                users.Add(user);
                foreach (var recipantID in newChat)
                {
                    var recipantUsers = await _context.Users.Where(x => x.Id == recipantID).FirstAsync();
                    users.Add(recipantUsers);
                }
                try
                {
                    var allChatsWithUser = await _context.GroupChats.Include(x => x.Users).Where(x => x.Users.Contains(user)).ToListAsync();

                    if(allChatsWithUser.Count > 0)
                    {
                        foreach (var groupChat in allChatsWithUser)
                        {
                            if (users.All(groupChat.Users.Contains))
                            {
                                return BadRequest();
                            }
                        }
                    }
                    var chat = new GroupChat()
                    {
                        Users = users
                    };
                    await _context.GroupChats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (System.Exception)
                {
                    var chat = new GroupChat()
                    {
                        Users = users
                    };
                    await _context.GroupChats.AddAsync(chat);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
            else
                return BadRequest();
        }


        [HttpPost("NewReply")]
        public async Task<ActionResult> NewReply([FromBody] ReplyToChatModel newReply)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if (user is not null)
            {
                var chat = await _context.GroupChats.Where(x => x.GroupChatID == newReply.GroupChatID).Include(x => x.Replies).FirstAsync();
                var reply = new Reply()
                {
                    BodyText = newReply.BodyText,
                    User = user,
                    GroupChat = chat
                };
                chat.Replies.Add(reply);
                _context.GroupChats.Update(chat);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
