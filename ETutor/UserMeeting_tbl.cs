//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ETutor
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserMeeting_tbl
    {
        public int UserId { get; set; }
        public int MeetingId { get; set; }
    
        public virtual Meeting_tbl Meeting_tbl { get; set; }
    }
}
