namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Trade")]
    public partial class Trade
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trade()
        {
            TradeAttribs = new HashSet<TradeAttribs>();
        }

        public int Id { get; set; }

        public int? GameId { get; set; }

        public int? SenderId { get; set; }

        public int? ReciverId { get; set; }

        public int? StatusId { get; set; }

        public virtual Game Game { get; set; }

        public virtual PlayerGame PlayerGame { get; set; }

        public virtual PlayerGame PlayerGame1 { get; set; }

        public virtual TradeStatus TradeStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TradeAttribs> TradeAttribs { get; set; }
    }
}
