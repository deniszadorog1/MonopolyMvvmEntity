namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChanceMove")]
    public partial class ChanceMove
    {
        public int Id { get; set; }

        public int? Steps { get; set; }

        public bool? IfForward { get; set; }

        public int? ChanceId { get; set; }

        public virtual Chance Chance { get; set; }
    }
}
