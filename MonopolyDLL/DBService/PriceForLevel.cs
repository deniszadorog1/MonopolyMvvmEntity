namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PriceForLevel")]
    public partial class PriceForLevel
    {
        public int Id { get; set; }

        public int? StationId { get; set; }

        public int? Price { get; set; }

        public int? TypeId { get; set; }

        public virtual Station Station { get; set; }

        public virtual PriceType PriceType { get; set; }
    }
}
