using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Models.Entities
{
    public class GroupChat
    {
        public Guid GroupChatID { get; set; }
        public List<User> Users { get; set; }
        public List<Reply> Replies { get; set; }
        public DateTime CreateDate { get; set; }

        public GroupChat()
        {
            GroupChatID = Guid.NewGuid();
        }
    }
}
