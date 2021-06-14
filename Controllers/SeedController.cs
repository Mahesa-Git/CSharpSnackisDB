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
using System.IO;
using System.Text;

namespace CSharpSnackisDB.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class SeedController : ControllerBase
    {
        private Context _context;
        private readonly UserManager<User> _userManager;

        public SeedController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private async Task<bool> IsAdmin()
        {
            User user = await _userManager.FindByNameAsync(User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name)).Value);
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("root") || roles.Contains("admin"))
                return true;
            else
                return false;
        }
        [HttpPost("SeedCategories")]
        public async Task<ActionResult> SeedCategoriesAndTopics()
        {
            if (IsAdmin().Result == true)
            {
                var category = new Category
                {
                    Title = "Bilar",
                    Description = "Här kan du diskutera allt om bilar"
                };
                var category1 = new Category
                {
                    Title = "MC",
                    Description = "Här kan du diskutera allt om motorcyklar"
                };
                var category2 = new Category
                {
                    Title = "Båtar",
                    Description = "Här kan du diskutera allt om båtar"
                };
                await _context.AddAsync(category);
                await _context.AddAsync(category1);
                await _context.AddAsync(category2);
                await _context.SaveChangesAsync();

                var topicCars = await _context.Categories.Where(x => x.Title == "Bilar").FirstAsync();
                var topicMc = await _context.Categories.Where(x => x.Title == "MC").FirstAsync();
                var topicBoats = await _context.Categories.Where(x => x.Title == "Båtar").FirstAsync();

                var topics = new string[] { "Allmänt", "Mek-snack", "Marknad" };
                var categories = new Category[] { category, category1, category2 };

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var topicToAdd = new Topic
                        {
                            Title = topics[j],
                            Category = categories[i]
                        };
                        await _context.AddAsync(topicToAdd);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            else
                return BadRequest();
        }
        [HttpPost("SeedFilteredWords")]
        public async Task<ActionResult> SeedFilteredWords()
        {
            if (IsAdmin().Result == true)
            {
                string[] filteredWords = Services.FilteredWordsCheck.SeedWords();
                foreach (var word in filteredWords)
                {
                    var wordIns = new FilteredWords();
                    wordIns.Words = word;
                    await _context.FilteredWords.AddAsync(wordIns);
                }
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
