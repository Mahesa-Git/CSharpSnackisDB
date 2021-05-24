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
        private const string FeKey = "";

        private Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

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
    }
}
