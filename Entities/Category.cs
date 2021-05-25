using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class Category
    {
        public string CategoryID { get; set; }
        public List<Topic> Topics { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }

        public Category()
        {
            CategoryID = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
    }
    public class CategoryResponseModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

}
