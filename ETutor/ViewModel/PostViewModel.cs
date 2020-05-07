using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class PostViewModel
    {
        public int postId { get; internal set; }
        public string description { get; internal set; }
        public string title { get; internal set; }
        public string autherById { get; internal set; }
        public string autherBy { get; internal set; }
        public bool isOwner { get; internal set; }
        public object comments { get; internal set; }
        public CommentViewModel comment { get; internal set; }
        public string createdOn { get; internal set; }
    }
}