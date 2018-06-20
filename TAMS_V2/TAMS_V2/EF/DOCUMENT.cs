namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DOCUMENT")]
    public partial class DOCUMENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCUMENT()
        {
            SENTENCE_DOCUMENT = new HashSet<SENTENCE_DOCUMENT>();
        }

        [Key]
        public long Document_ID { get; set; }

        [StringLength(300)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Author { get; set; }

        public long? Supervisor_ID { get; set; }

        [StringLength(50)]
        public string File_Name { get; set; }

        public long? Language_ID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Published_Date { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Accepted_Date { get; set; }

        [StringLength(50)]
        public string Document_Type { get; set; }

        public long? Branch_ID { get; set; }

        public double? Size { get; set; }

        public bool? Status { get; set; }

        public long? User_ID { get; set; }

        public virtual BRANCH BRANCH { get; set; }

        public virtual LANGUAGE LANGUAGE { get; set; }

        public virtual SUPERVISOR SUPERVISOR { get; set; }

        public virtual USER USER { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENTENCE_DOCUMENT> SENTENCE_DOCUMENT { get; set; }
    }
}
