using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using CSharpSnackisDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AdminAuthController : ControllerBase
    {

        private const string ApiKey = "localhost:44302";
        private const string FeKey = "localhost:44335";

        private Context _context;
        private readonly UserManager<User> _userManager;

        public AdminAuthController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        #region ADMIN REGISTER/DELETE REGION
        [HttpPost("adminregister")]
        public async Task<ActionResult> AdminRegister([FromBody] RegisterAdminModel model)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                User newUser = new User()
                {
                    Email = model.Email,
                    UserName = model.Username,
                    EmailConfirmed = true,
                };
                var UserCheck = await _userManager.FindByNameAsync(model.Username);
                var UserMailCheck = await _userManager.FindByEmailAsync(model.Email);

                if (UserCheck != null)
                    return BadRequest("Username in use");

                if (UserMailCheck != null)
                    return BadRequest("E-mail in use");

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    User user1 = await _userManager.FindByNameAsync(newUser.UserName);

                    if (user1 is not null)
                    {
                        await _userManager.AddToRoleAsync(newUser, "root");
                        _context.SaveChanges();
                        return Ok();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    StringBuilder errorString = new StringBuilder();

                    foreach (var error in result.Errors)
                    {
                        errorString.Append(error.Description);
                    }
                    return NotFound();
                }
            }
            else
                return Unauthorized();

        }
        [HttpDelete("admindelete")]
        public async Task<ActionResult> AdminDelete()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    _context.Remove(user);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return BadRequest("Something went wrong");
                }
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }
        #endregion

        #region BAN/UNBAN/EDIT/GET USER REGION
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {

            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var users = await _context.Users.ToListAsync();
                users.Remove(user);
                return Ok(users);
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpGet("BanUser/{userID}")]
        public async Task<ActionResult> BanUser([FromRoute] string userID)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var userToBan = _context.Users.Where(x => x.Id == userID).FirstOrDefault();
                userToBan.IsBanned = true;
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpGet("UnbanUser/{userID}")]
        public async Task<ActionResult> UnBanUser([FromRoute] string userID)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                var userToBan = _context.Users.Where(x => x.Id == userID).FirstOrDefault();
                userToBan.IsBanned = false;
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return Unauthorized();
        }
        [HttpPut("ChangeUserInfo")]
        public async Task<ActionResult> ChangeUserInfo([FromBody] RegisterModel registerModel, string userID)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("root") || roles.Contains("admin"))
            {
                try
                {
                    var userToChange = _context.Users.Where(x => x.Id == userID).FirstOrDefault();
                    userToChange.UserName = registerModel.Username;
                    userToChange.NormalizedUserName = registerModel.Username.ToUpper();
                    userToChange.Email = registerModel.Email;
                    userToChange.NormalizedEmail = registerModel.Email.ToUpper();
                    userToChange.Country = registerModel.Country;
                    userToChange.ProfileText = registerModel.ProfileText;

                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception)
                {

                    return BadRequest();
                }
                
            }
            else
                return Unauthorized();
        }
        #endregion
    }
}
