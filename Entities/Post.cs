using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CSharpSnackisDB.Entities.Reply;

namespace CSharpSnackisDB.Entities
{
    public class Post
    {
        public string PostID { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsReported { get; set; }
        public bool IsThreadStart { get; set; }
        public User User { get; set; }
        public Thread Thread { get; set; }
        public PostReaction PostReaction { get; set; }
        public List<Reply> Replies { get; set; }
        public string Image { get; set; }

        public Post()
        {
            PostID = Guid.NewGuid().ToString();
            DateTime utc = new DateTime();
            utc = DateTime.UtcNow;
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

            CreateDate = TimeZoneInfo.ConvertTimeFromUtc(utc, cet);
        }
    }
    public class PostResponseModel
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public string ThreadId { get; set; }
        public bool IsThreadStart { get; set; }
        public string Image { get; set; }
    }
    public class APIPost
    {
        public string UserName { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public string User { get; set; }
        public List<APIReply> Replies { get; set; }
    }
}
