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
    
    public partial class MailRecipit_tbl
    {
        public int MailId { get; set; }
        public string ToId { get; set; }
        public Nullable<bool> IsFail { get; set; }
    
        public virtual Mail_tbl Mail_tbl { get; set; }
        public virtual User_tbl User_tbl { get; set; }
    }
}
