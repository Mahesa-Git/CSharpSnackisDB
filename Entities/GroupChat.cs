using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class GroupChat
    {
        public string GroupChatID { get; set; }
        public List<User> Users { get; set; }
        public List<Reply> Replies { get; set; }
        public DateTime CreateDate { get; set; }

        public GroupChat()
        {
            GroupChatID = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
    public class NewChatModel
    {
        public List<string> RecipantIDs { get; set; }
    }
}
