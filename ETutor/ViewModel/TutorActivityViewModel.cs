using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class TutorActivityViewModel
    {
        public string tutorId { get; set; }
        public string tutorName { get; set; }
        public int noMessages { get; set; }
        public int noMeetings { get; set; }
        public int noBlogs { get; set; }
    }
}