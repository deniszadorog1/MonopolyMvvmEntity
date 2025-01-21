namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChanceMoney")]
    public partial class ChanceMoney
    {
        public int Id { get; set; }

        public int? AmountOfMoney { get; set; }

        public bool? IfGet { get; set; }

        public int? ChanceId { get; set; }

        public virtual Chance Chance { get; set; }
    }
}
