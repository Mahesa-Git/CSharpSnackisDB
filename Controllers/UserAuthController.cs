using CSharpSnackisDB.Data;
using CSharpSnackisDB.Entities;
using CSharpSnackisDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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
        private const string FeKey = "";

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
                Email = model.Email,
                Country = model.Country,
                MailToken = null,
                EmailConfirmed = false,
                ProfileText = model.ProfileText
                
            };
            if (newUser.UserName.Contains(' '))
            {
                return BadRequest("wrong username");
            }
            var UserCheck = await _userManager.FindByNameAsync(model.Username);
            var UserMailCheck = await _userManager.FindByEmailAsync(model.Email);

            if (UserCheck != null)
                return BadRequest("Username in use");

            if (UserMailCheck != null)
                return BadRequest("E-mail in use");

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                User user = await _userManager.FindByNameAsync(newUser.UserName);

                await _userManager.AddToRoleAsync(newUser, "User");

                var userId = await _userManager.GetUserIdAsync(newUser);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                user.MailToken = token;
                await _context.SaveChangesAsync();

                var urlContent = Url.Content($"https://{ApiKey}/userauth/Mailauthentication/{userId}");
                var link = Url.Content($"https://{FeKey}/index"); // ÄNDRAS SEN FE

                await _sender.SendEmailAsync(newUser.Email, "Bekräfta din e-post genom att klicka på länken", "<p>Klicka här för att bekräfta din e-post</p>" + urlContent +
                                                            "</br></br><p>Fungerar inte länken? Få ett nytt utskick genom att försöka logga in på hemsidan:</br></br>" +
                                                            $" {link}");

                return Ok(newUser.Id);
            }
            else
            {
                return BadRequest("Registration failed");
            }
        }

        [HttpDelete("userdelete")]
        public async Task<ActionResult> UserDelete()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);

            try
            {
                _context.Remove(user);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);

            }
            return Ok();

        }

        [HttpPut("userupdate")]
        public async Task<ActionResult> UserUpdate([FromBody] RegisterModel model)
        {
            var UserCheck = await _userManager.FindByNameAsync(model.Username);
            var UserMailCheck = await _userManager.FindByEmailAsync(model.Email);

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
                user.Email = model.Email;
                user.Country = model.Country;
                user.MailToken = null;
                user.EmailConfirmed = false;
                user.NormalizedUserName = model.Username.ToUpper();
                user.NormalizedEmail = model.Email.ToUpper();

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

                return Ok(new { Token = tokenString, Expires = exp });
            }
            else
            {
                return BadRequest("Error, user not found");
            }
        }

        [HttpGet("profile/{id}")]
        public async Task<ActionResult> GetProfile(string Id)
        {
            User user = await _userManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return StatusCode(404, new { message = "User does not exist" });
            }
        }

        [AllowAnonymous]
        [HttpGet("ResendMail/{id}")]
        public async Task<ActionResult> ResendAuthMail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is not null)
            {
                var urlContent = Url.Content($"https://{ApiKey}/userauth/Mailauthentication/{user.Id}");
                var link = Url.Content($"https://{FeKey}/index"); // ändra sen FE

                await _sender.SendEmailAsync(user.Email, "Bekräfta din e-post genom att klicka på länken", "<p>Klicka här för att bekräfta din e-post</p>" + urlContent +
                                                            "</br></br><p>Fungerar inte länken? Få ett nytt utskick genom att försöka logga in på hemsidan:</br></br>" +
                                                            $" {link}");
                return Redirect($"https://{FeKey}/RegisterConfirmation"); // ÄNDRA SEN FE
            }
            else
                return BadRequest("User not found. Try to create another user or contact us at ggwebbshop@gmail.com");
        }

        [AllowAnonymous]
        [HttpGet("MailAuthentication/{id}")]
        public async Task<ActionResult> AuthenticateEmail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return BadRequest("The user was not found. It might be deleted. If it's your account and you've not yet validated your e-mail by clicking the sent link for validation of the account: check your e-mail for messages that might be generated after your first attempt.");

            if (user.EmailConfirmed == true)
                return BadRequest("The account have already been activated for this user");

            var result = await _userManager.ConfirmEmailAsync(user, user.MailToken);

            if (result.Succeeded)
            {
                user.MailToken = null;
                await _context.SaveChangesAsync();
                return Redirect($"https://{FeKey}/SuccessfulEmailConfirm"); // ÄNDRA SEN FE
            }

            else
                return BadRequest("Contact administrator for setting the mailauthentication manually at ggwebshop@gmail.com");
        }

        [AllowAnonymous]
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
    }
}
