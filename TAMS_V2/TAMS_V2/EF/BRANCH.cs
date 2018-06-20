namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BRANCH")]
    public partial class BRANCH
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BRANCH()
        {
            CHECKING_DOCUMENT = new HashSet<CHECKING_DOCUMENT>();
            DOCUMENTs = new HashSet<DOCUMENT>();
        }

        [Key]
        public long Branch_ID { get; set; }

        [StringLength(100)]
        public string Branch_Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CHECKING_DOCUMENT> CHECKING_DOCUMENT { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCUMENT> DOCUMENTs { get; set; }
    }
}
