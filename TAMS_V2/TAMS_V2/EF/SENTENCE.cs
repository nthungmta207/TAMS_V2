namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SENTENCE")]
    public partial class SENTENCE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SENTENCE()
        {
            RESULTs = new HashSet<RESULT>();
            SENTENCE_DOCUMENT = new HashSet<SENTENCE_DOCUMENT>();
        }

        [Key]
        public long Sentence_ID { get; set; }

        [Column(TypeName = "ntext")]
        public string Sentence_Content { get; set; }

        public long? Hash_Value { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RESULT> RESULTs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SENTENCE_DOCUMENT> SENTENCE_DOCUMENT { get; set; }
    }
}
