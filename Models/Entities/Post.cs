using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Models.Entities
{
    public class Post
    {
        public Guid PostID { get; set; }
        public string Title { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsReported { get; set; }
        public User User { get; set; }
        public List<PostReaction> PostReactions { get; set; }
        public Category Category { get; set; }
        public List<Reply> Replies { get; set; }

        public Post()
        {
            PostID = Guid.NewGuid();
        }
    }
}
