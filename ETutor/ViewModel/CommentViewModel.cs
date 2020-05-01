using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class CommentViewModel
    {
        public int commentId { get; set; }
        public DateTime? commentedOn { get; set; }
        public string commentBy { get; set; }
        public string commentById { get; set; }
        public string text { get; set; }
        public bool isOwner { get; set; }
    }
}