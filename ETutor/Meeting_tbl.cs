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
    
    public partial class Meeting_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Meeting_tbl()
        {
            this.MeetingMinits_tbl = new HashSet<MeetingMinits_tbl>();
            this.User_tbl2 = new HashSet<User_tbl>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public System.DateTime MeetingDate { get; set; }
        public string Venue { get; set; }
        public Nullable<bool> IsCanceled { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
        public string UpdatedBy { get; set; }
    
        public virtual User_tbl User_tbl { get; set; }
        public virtual User_tbl User_tbl1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeetingMinits_tbl> MeetingMinits_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User_tbl> User_tbl2 { get; set; }
    }
}
