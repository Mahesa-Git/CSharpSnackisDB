using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class Topic
    {
        public string TopicID { get; set; }
        public Category Category { get; set; }
        public List<Thread> Threads { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }

        public Topic()
        {
            TopicID = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
    public class TopicResponseModel
    {
        public string Title { get; set; }
        public string CategoryId { get; set; }
    }
}
