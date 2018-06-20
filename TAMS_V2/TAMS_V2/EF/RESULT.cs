namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RESULT")]
    public partial class RESULT
    {
        [Key]
        public long Result_ID { get; set; }

        public long? Checking_Document_ID { get; set; }

        public long? Checking_Sentence_Position { get; set; }

        [StringLength(500)]
        public string Document_IDs { get; set; }

        public long? Sentence_ID { get; set; }

        public virtual CHECKING_DOCUMENT CHECKING_DOCUMENT { get; set; }

        public virtual SENTENCE SENTENCE { get; set; }
    }
}
