using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETutor.ViewModel
{
    public class MessageHistoryViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string student { get; set; }
        public string studentId { get; set; }
        public string tutor { get; set; }
        public string tutorId { get; set; }
        public decimal overallNoMessage { get; set; }
        public decimal last7NoMessages { get; set; }
        public decimal avgNoMessages { get; set; }
        public decimal tutorMessages { get; set; }
    }
}