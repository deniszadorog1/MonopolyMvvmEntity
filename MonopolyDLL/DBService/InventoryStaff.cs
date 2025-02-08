namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InventoryStaff")]
    public partial class InventoryStaff
    {
        public int Id { get; set; }

        public int? PlayerId { get; set; }

        public int? StaffId { get; set; }

        public int? StationId { get; set; }

        public bool? IfEnabled { get; set; }

        public virtual Player Player { get; set; }

        public virtual Item Item { get; set; }
    }
}
