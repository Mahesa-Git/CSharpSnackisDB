﻿using System;

namespace CSharpSnackisDB.Entities
{
    public class Reply
    {
        public string ReplyID { get; set; }
        public string BodyText { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
        public bool IsReported { get; set; }
        public User User { get; set; }
        public Post Post { get; set; }
        public GroupChat GroupChat { get; set; }
        public PostReaction PostReaction { get; set; }

        public Reply()
        {
            ReplyID = Guid.NewGuid().ToString();
            CreateDate = DateTime.Now;
        }
        public class ReplyResponseModel
        {
            public string PostId { get; set; }
            public string BodyText { get; set; }

        }
        public class ReplyToChatModel
        {
            public string GroupChatID { get; set; }
            public string BodyText { get; set; }
        }
    }
    public class APIReply
    {
        public string BodyText { get; set; }
        public string UserName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime EditDate { get; set; }
    }
}
