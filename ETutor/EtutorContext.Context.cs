﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ETutorEntities : DbContext
    {
        public ETutorEntities()
            : base("name=ETutorEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Document_tbl> Document_tbl { get; set; }
        public virtual DbSet<DocumentComment_tbl> DocumentComment_tbl { get; set; }
        public virtual DbSet<Mail_tbl> Mail_tbl { get; set; }
        public virtual DbSet<MailRecipit_tbl> MailRecipit_tbl { get; set; }
        public virtual DbSet<Meeting_tbl> Meeting_tbl { get; set; }
        public virtual DbSet<MeetingMinits_tbl> MeetingMinits_tbl { get; set; }
        public virtual DbSet<Message_tbl> Message_tbl { get; set; }
        public virtual DbSet<Post_tbl> Post_tbl { get; set; }
        public virtual DbSet<PostComment_tbl> PostComment_tbl { get; set; }
        public virtual DbSet<Role_tbl> Role_tbl { get; set; }
        public virtual DbSet<StudentTutor_tbl> StudentTutor_tbl { get; set; }
        public virtual DbSet<User_tbl> User_tbl { get; set; }
        public virtual DbSet<UserClaims_tbl> UserClaims_tbl { get; set; }
        public virtual DbSet<UserLogins_tbl> UserLogins_tbl { get; set; }
        public virtual DbSet<UserMeeting_tbl> UserMeeting_tbl { get; set; }
        public virtual DbSet<UserMessage_tbl> UserMessage_tbl { get; set; }
        public virtual DbSet<UserRole_tbl> UserRole_tbl { get; set; }
    }
}
