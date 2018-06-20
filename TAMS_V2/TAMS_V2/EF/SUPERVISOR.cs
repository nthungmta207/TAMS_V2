namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SUPERVISOR")]
    public partial class SUPERVISOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SUPERVISOR()
        {
            CHECKING_DOCUMENT = new HashSet<CHECKING_DOCUMENT>();
            DOCUMENTs = new HashSet<DOCUMENT>();
        }

        [Key]
        public long Supervisor_ID { get; set; }

        public long? Branch_ID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public bool? Sex { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Date_Of_Birth { get; set; }

        [StringLength(100)]
        public string Degree { get; set; }

        [StringLength(100)]
        public string Academic_Title { get; set; }

        [StringLength(200)]
        public string Department { get; set; }

        [Column(TypeName = "ntext")]
        public string Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHECKING_DOCUMENT> CHECKING_DOCUMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENT> DOCUMENTs { get; set; }
    }
}
