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
    
    public partial class Mail_tbl
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Mail_tbl()
        {
            this.MailRecipit_tbl = new HashSet<MailRecipit_tbl>();
        }
    
        public int Id { get; set; }
        public string FromId { get; set; }
        public string Subject { get; set; }
        public string MailBody { get; set; }
        public string TriggerBy { get; set; }
        public Nullable<bool> IsFail { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
    
        public virtual User_tbl User_tbl { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MailRecipit_tbl> MailRecipit_tbl { get; set; }
    }
}
