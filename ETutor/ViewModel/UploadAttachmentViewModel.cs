using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class UploadAttachmentViewModel
    {
        public int attachmentId { get; set; }
        public string uploadBy { get; set; }
        public string uploadById { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public bool isOwner { get; set; }
        public bool isEdit { get; set; }
        public List<CommentViewModel> comments { get; set; }
        public CommentViewModel comment { get; set; }
    }
}