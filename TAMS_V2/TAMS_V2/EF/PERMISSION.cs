namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PERMISSION")]
    public partial class PERMISSION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PERMISSION()
        {
            USER_PERMISSION = new HashSet<USER_PERMISSION>();
        }

        [Key]
        public long Permission_ID { get; set; }

        [Column("Permission")]
        [StringLength(100)]
        public string Permission1 { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<USER_PERMISSION> USER_PERMISSION { get; set; }
    }
}
