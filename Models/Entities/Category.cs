using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Models.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public List<Topic> Topics { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Post> Posts { get; set; }
        public List<Reply> Replies { get; set; }
    }
}
