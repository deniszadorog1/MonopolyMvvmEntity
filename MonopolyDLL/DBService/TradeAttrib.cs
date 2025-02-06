namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TradeAttrib
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TradeAttrib()
        {
            BusinessToGives = new HashSet<BusinessToGive>();
        }

        public int Id { get; set; }

        public int? TradeId { get; set; }

        public int? PlayerId { get; set; }

        public int? MoneyToGive { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BusinessToGive> BusinessToGives { get; set; }

        public virtual PlayerGame PlayerGame { get; set; }

        public virtual Trade Trade { get; set; }
    }
}
