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
    
    public partial class StudentTutor_tbl
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string TutorId { get; set; }
        public Nullable<bool> IsCurrent { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual User_tbl User_tbl { get; set; }
        public virtual User_tbl User_tbl1 { get; set; }
        public virtual User_tbl User_tbl2 { get; set; }
        public virtual User_tbl User_tbl3 { get; set; }
    }
}
