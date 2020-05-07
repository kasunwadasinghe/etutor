using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class MeetingMinutesViewModel
    {
        public int meetingId { get; set; }
        public int meetingMinuteId { get; set; }
        public string description { get; set; }
        public DropdownViewModel presenter { get; set; }
        public bool isEdit { get; set; }
    }
}