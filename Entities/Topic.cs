using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class Topic
    {
        public int TopicID { get; set; }
        public Category Category { get; set; }
        public List<Thread> Threads { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
