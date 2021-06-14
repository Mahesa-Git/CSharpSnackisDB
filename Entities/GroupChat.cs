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
            DateTime utc = new DateTime();
            utc = DateTime.UtcNow;
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

            CreateDate = TimeZoneInfo.ConvertTimeFromUtc(utc, cet);
        }
    }
    public class NewChatModel
    {
        public List<string> RecipantIDs { get; set; }
    }
}
