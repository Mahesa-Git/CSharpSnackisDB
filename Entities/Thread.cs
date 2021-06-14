using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class Thread
    {
        public string ThreadID { get; set; }
        public User User { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsReported { get; set; }
        public Topic Topic { get; set; }
        public List<Post> Posts { get; set; }
        public Thread()
        {
            ThreadID = Guid.NewGuid().ToString();
            DateTime utc = new DateTime();
            utc = DateTime.UtcNow;
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");

            CreateDate = TimeZoneInfo.ConvertTimeFromUtc(utc, cet);
        }
    }
    public class ThreadResponseModel
    {
        public string Title { get; set; }
        public string Image { get; set; }
        public string BodyText { get; set; }
        public string TopicId { get; set; }
    }
    public class APIThread
    {
        public string ThreadID { get; set; }
        public string UserName { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public List<APIPost> Posts { get; set; }
    }
}
