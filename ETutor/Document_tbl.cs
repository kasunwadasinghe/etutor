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
    
    public partial class Document_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Document_tbl()
        {
            this.DocumentComment_tbl = new HashSet<DocumentComment_tbl>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public Nullable<System.DateTime> UpdateOn { get; set; }
    
        public virtual User_tbl User_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DocumentComment_tbl> DocumentComment_tbl { get; set; }
    }
}
