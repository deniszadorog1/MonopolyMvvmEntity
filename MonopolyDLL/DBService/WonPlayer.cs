namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WonPlayer")]
    public partial class WonPlayer
    {
        public int Id { get; set; }

        public int? PlayerId { get; set; }

        public virtual PlayerGame PlayerGame { get; set; }
    }
}
