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
        private const string ApiKey = "localhost:44302";
        private const string FeKey = "localhost:44335";

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
                //Lägg in kontroll för chats med 0 eller 1 user (om usern deletad) och ta bort chatsen.
                return Ok(chatList);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("NewChat")]
        public async Task<ActionResult> ReadCategories([FromBody] NewChatModel newChat)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            if (user is not null)
            {
                var recipantUser = await _context.Users.Where(x => x.Id == newChat.RecipantID).FirstAsync();
                try
                {
                    var chatExist = await _context.GroupChats
                        .Include(x => x.Users)
                        .Where(x => x.Users
                        .Contains(user) && x.Users.Contains(recipantUser))
                        .FirstAsync();

                    return BadRequest();
                }
                catch (System.Exception)
                {
                    var users = new List<User>() { user, recipantUser };
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
