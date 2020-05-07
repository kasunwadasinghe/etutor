using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class MeetingViewModel
    {
        public int meetingId { get; set; }
        public string description { get; set; }
        public string place { get; set; }
        public List<DropdownViewModel> members { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string createdBy { get; set; }
        public string createdById { get; set; }
        public bool isOwner { get; set; }
    }
}