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

        public int? FirstLevel { get; set; }

        public int? SecondLevel { get; set; }

        public int? ThirdLevel { get; set; }

        public int? FourthLevel { get; set; }

        public int? FifthLevel { get; set; }

        public int? SixthLevel { get; set; }

        public virtual Station Station { get; set; }
    }
}
