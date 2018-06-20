namespace TAMS_V2.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class USER_PERMISSION
    {
        [Key]
        [Column(Order = 0)]
        public long User_Type_ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Permission_ID { get; set; }

        [StringLength(200)]
        public string Comment { get; set; }

        public virtual PERMISSION PERMISSION { get; set; }

        public virtual USER_TYPE USER_TYPE { get; set; }
    }
}
