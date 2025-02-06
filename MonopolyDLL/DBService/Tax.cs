namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Tax")]
    public partial class Tax
    {
        public int Id { get; set; }

        public int? CellId { get; set; }

        public int? TypeId { get; set; }

        public virtual Cell Cell { get; set; }

        public virtual TaxType TaxType { get; set; }
    }
}
