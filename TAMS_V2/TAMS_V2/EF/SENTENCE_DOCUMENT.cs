namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SENTENCE_DOCUMENT
    {
        public long ID { get; set; }

        public long? Document_ID { get; set; }

        public long? Sentence_ID { get; set; }

        [StringLength(500)]
        public string Position { get; set; }

        public virtual DOCUMENT DOCUMENT { get; set; }

        public virtual SENTENCE SENTENCE { get; set; }
    }
}
