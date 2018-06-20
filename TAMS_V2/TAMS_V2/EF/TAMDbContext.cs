namespace TAMS_V2.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class TAMDbContext : DbContext
    {
        public TAMDbContext()
            : base("name=TAMDbContext")
        {
        }

        public virtual DbSet<BRANCH> BRANCHes { get; set; }
        public virtual DbSet<CHECKING_DOCUMENT> CHECKING_DOCUMENT { get; set; }
        public virtual DbSet<DOCUMENT> DOCUMENTs { get; set; }
        public virtual DbSet<LANGUAGE> LANGUAGEs { get; set; }
        public virtual DbSet<PERMISSION> PERMISSIONs { get; set; }
        public virtual DbSet<RESULT> RESULTs { get; set; }
        public virtual DbSet<SENTENCE> SENTENCEs { get; set; }
        public virtual DbSet<SENTENCE_DOCUMENT> SENTENCE_DOCUMENT { get; set; }
        public virtual DbSet<SUPERVISOR> SUPERVISORs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<USER> USERs { get; set; }
        public virtual DbSet<USER_PERMISSION> USER_PERMISSION { get; set; }
        public virtual DbSet<USER_TYPE> USER_TYPE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CHECKING_DOCUMENT>()
                .Property(e => e.File_Name)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKING_DOCUMENT>()
                .Property(e => e.Document_Type)
                .IsUnicode(false);

            modelBuilder.Entity<CHECKING_DOCUMENT>()
                .HasMany(e => e.RESULTs)
                .WithOptional(e => e.CHECKING_DOCUMENT)
                .HasForeignKey(e => e.Checking_Document_ID);

            modelBuilder.Entity<DOCUMENT>()
                .Property(e => e.File_Name)
                .IsUnicode(false);

            modelBuilder.Entity<DOCUMENT>()
                .Property(e => e.Document_Type)
                .IsUnicode(false);

            modelBuilder.Entity<PERMISSION>()
                .HasMany(e => e.USER_PERMISSION)
                .WithRequired(e => e.PERMISSION)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RESULT>()
                .Property(e => e.Document_IDs)
                .IsUnicode(false);

            modelBuilder.Entity<SENTENCE_DOCUMENT>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<SUPERVISOR>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<USER_TYPE>()
                .HasMany(e => e.USER_PERMISSION)
                .WithRequired(e => e.USER_TYPE)
                .WillCascadeOnDelete(false);
        }
    }
}
