﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharpSnackisDB.Entities
{
    public class Reply
    {
        public string ReplyID { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsReported { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public Category Category { get; set; }
        public GroupChat GroupChat { get; set; }

        public Reply()
        {
            ReplyID = Guid.NewGuid().ToString();
        }
    }
}
