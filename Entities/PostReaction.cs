using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class PostReaction
    {
        public string PostReactionID { get; set; }
        public List<User> Users { get; set; }
        public bool AddOrRemove { get; set; }
        public int LikeCounter { get; set; }

        public PostReaction()
        {
            PostReactionID = Guid.NewGuid().ToString();
        }
    }
}
