using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Models.Entities
{
    public class User : IdentityUser
    {
        public string Country { get; set; }
        public string MailToken { get; set; }
        public DateTime CreateDate { get; set; }
        public string ProfileText { get; set; }
        public bool IsBanned { get; set; }
        public List<Post> Posts { get; set; }
        public List<Reply> Replies { get; set; }
        public List<Thread> Threads { get; set; }
        public List<GroupChat> GroupChats { get; set; }

        public User()
        {
            CreateDate = DateTime.Now;
        }

    }
}
