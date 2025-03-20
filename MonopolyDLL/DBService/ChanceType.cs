namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChanceType")]
    public partial class ChanceType
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }
    }
}
