namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BusinessToGive")]
    public partial class BusinessToGive
    {
        public int Id { get; set; }

        public int? UnitId { get; set; }

        public int? TradeAttribId { get; set; }

        public virtual TradeAttrib TradeAttrib { get; set; }
    }
}
