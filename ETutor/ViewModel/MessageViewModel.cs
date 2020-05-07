using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class MessageViewModel
    {
        public int messageId { get; internal set; }
        public string description { get; set; }
        public string sendBy { get; set; }
        public string date { get; set; }
        public List<DropdownViewModel> members { get; set; }
        public string sendById { get; internal set; }
        public bool isOwner { get; internal set; }
        public string to { get; internal set; }
    }
}