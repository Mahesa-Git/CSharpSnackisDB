using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class Post
    {
        public string PostID { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public string UserId { get; set; }
        public bool IsReported { get; set; }
        public bool IsThreadStart { get; set; }
        public User User { get; set; }
        public Thread Thread { get; set; }
        public List<PostReaction> PostReactions { get; set; }
        public List<Reply> Replies { get; set; }

        public Post()
        {
            PostID = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
    public class PostResponseModel
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public string ThreadId { get; set; }
        public bool IsThreadStart { get; set; }
    }
}
