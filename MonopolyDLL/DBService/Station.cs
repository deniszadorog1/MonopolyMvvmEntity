namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Station")]
    public partial class Station
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Station()
        {
            PriceForLevel = new HashSet<PriceForLevel>();
        }

        public int Id { get; set; }

        public int? CellId { get; set; }

        public int? Price { get; set; }

        public int? UpgradePrice { get; set; }

        public int? TypeId { get; set; }

        public int? DepositPrice { get; set; }

        public int? RebuyPrice { get; set; }

        public virtual Cell Cell { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriceForLevel> PriceForLevel { get; set; }

        public virtual StationType StationType { get; set; }
    }
}
