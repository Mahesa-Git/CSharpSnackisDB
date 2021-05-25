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
        public bool IsReported { get; set; }
        public Topic Topic { get; set; }
        public List<Post> Posts { get; set; }
        public Thread()
        {
            ThreadID = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
    public class ThreadResponseModel
    {
        public string Title { get; set; }
        public string BodyText { get; set; }
        public string TopicId { get; set; }
    }
}
