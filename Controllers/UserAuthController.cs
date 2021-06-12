using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using CSharpSnackisDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UserAuthController : ControllerBase
    {
        private const string ApiKey = "localhost:44302";
        private const string FeKey = "localhost:44335";

        private Context _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _sender;

        public UserAuthController(Context context, UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender sender)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _sender = sender;
        }

        #region LOGIN/REGISTER/MAILAUTHENTICATION REGION

        private bool MailChecker(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {
            User user = model.User.Contains("@") ? await _userManager.FindByEmailAsync(model.User) : await _userManager.FindByNameAsync(model.User);

            if (user != null)
            {
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (signInResult.IsNotAllowed)
                {
                    if (user.EmailConfirmed == false)
                    {
                        return BadRequest($"{user.Id}");
                    }
                }
                if (user.IsBanned == true)
                    return Ok("banned");
                if (signInResult.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("default-key-xxxx-aaaa-qqqq-default-key-xxxx-aaaa-qqqq");

                    var exp = DateTime.UtcNow.AddDays(1);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(ClaimTypes.Email, user.Email)
                        }),
                        Expires = exp,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);
                    var userID = user.Id;
                    var roles = await _userManager.GetRolesAsync(user);

                    return Ok(new { Token = tokenString, Expires = exp, userID = userID, Role = roles[0] });
                }
                else
                {
                    return Ok("No user or password matched, try again.");
                }
            }
            else
            {
                return Ok("No such user exists");
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterModel model)
        {
            User newUser = new User()
            {
                UserName = model.Username,
                NormalizedUserName = model.Username.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                Country = model.Country,
                MailToken = null,
                EmailConfirmed = true,
                ProfileText = model.ProfileText,
                Image = model.Image

            };
            if (newUser.UserName.Contains(' '))
            {
                return BadRequest("wrong username");
            }
            var UserCheck = await _userManager.FindByNameAsync(model.Username);
            var UserMailCheck = await _userManager.FindByEmailAsync(model.Email);

            if (UserCheck != null)
                return BadRequest("Username in use");

            if (!MailChecker(model.Email))
                return BadRequest("wrong email format");

            if (UserMailCheck != null)
                return BadRequest("E-mail in use");

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "User");
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return BadRequest("Registration failed");
            }
        }   
        #endregion

        #region USER CRUD REGION
        [HttpPut("userProfileTextUpdate")]
        public async Task<ActionResult> UserProfileTextUpdate([FromBody] RegisterModel model)
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

            var UserCheck = await _userManager.FindByNameAsync(user.UserName);

            if (user is not null)
            {
                
                user.ProfileText = model.ProfileText;

                await _context.SaveChangesAsync();

                return Ok();
            }
            else
            {
                return BadRequest("Error, user not found");
            }
        }
        [HttpPut("userProfileUpdate")]
        public async Task<ActionResult> UserProfileUpdate([FromBody] RegisterModel model)
        {
            var UserMailCheck = await _userManager.FindByEmailAsync(model.Email);
            var UserCheck = await _userManager.FindByNameAsync(model.Username);

            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            User userEmail = await _userManager.FindByEmailAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Email)).Value);

            bool sameEmail = user.Email == model.Email ? true : false;
            bool sameUsername = user.UserName == model.Username ? true : false;

            if (!sameEmail && UserMailCheck != null)
            {
                return BadRequest("E-mail in use");
            }
            if (!sameUsername && UserCheck != null)
            {
                return BadRequest("Username in use");
            }

            if (user is not null)
            {
                user.UserName = model.Username;
                user.NormalizedUserName = model.Username.ToUpper();
                user.Email = model.Email;
                user.NormalizedEmail = model.Email.ToUpper();
                user.EmailConfirmed = true;
                user.Country = model.Country;

                await _context.SaveChangesAsync();

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("default-key-xxxx-aaaa-qqqq-default-key-xxxx-aaaa-qqqq");

                var exp = DateTime.UtcNow.AddDays(1);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                            new Claim(ClaimTypes.Name, model.Username),
                            new Claim(ClaimTypes.Email, model.Email)

                    }),
                    Expires = exp,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                var userID = user.Id;
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(new { Token = tokenString, Expires = exp, userID = userID, Role = roles[0] });
            }
            else
            {
                return BadRequest("Error, user not found");
            }
        }
        [HttpGet("userImageUpdate/{id}")]
        public async Task<ActionResult> UserProfileUpdate([FromRoute] string id)
        {

            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            if (user is not null)
            {
                user.Image = id;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            else
            {
                return BadRequest("Error, user not found");
            }
        }
        [HttpGet("userImageDelete")]
        public async Task<ActionResult> UserProfileDelete()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            if (user is not null)
            {
                user.Image = null;
                await _userManager.UpdateAsync(user);
                return Ok();
            }
            else
            {
                return BadRequest("Error, user not found");
            }
        }
        [HttpPost("changepassword")]
        public async Task<ActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {

            if (model.NewPassword == model.ConfirmNewPassword)
            {
                User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);

                if (user is not null)
                {
                    if (await _userManager.CheckPasswordAsync(user, model.CurrentPassword))
                    {
                        await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                        return Ok(new { message = "Password has been updated." });
                    }
                    else
                    {
                        return NotFound(new { message = $"Your password is incorrect. ({user.AccessFailedCount}) failed attempts." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "No such user found." });
                }
            }
            else
            {
                return BadRequest(new { message = "Your password does not match." });
            }
        }
        [HttpGet("profile/{id}")]
        public async Task<ActionResult> GetProfile(string Id)
        {
            User user = await _userManager.FindByIdAsync(Id);
            
            
            if (user is not null)
            {
                return Ok(user);
            }
            return Unauthorized();
        }
        #endregion
    }
}
