namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PriceType")]
    public partial class PriceType
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Type { get; set; }
    }
}
