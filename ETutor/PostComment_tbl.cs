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
    
    public partial class PostComment_tbl
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Comment { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
    
        public virtual Post_tbl Post_tbl { get; set; }
        public virtual User_tbl User_tbl { get; set; }
    }
}
