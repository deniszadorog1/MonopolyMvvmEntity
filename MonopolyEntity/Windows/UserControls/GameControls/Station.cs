namespace MonopolyEntity.Windows.UserControls.GameControls
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
            BoxItems = new HashSet<BoxItem>();
            PriceForLevels = new HashSet<PriceForLevel>();
        }

        public int Id { get; set; }

        public int? CellId { get; set; }

        public int? Price { get; set; }

        public int? UpgradePrice { get; set; }

        public int? OwnerId { get; set; }

        public int? TypeId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoxItem> BoxItems { get; set; }

        public virtual Cell Cell { get; set; }

        public virtual PlayerGame PlayerGame { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriceForLevel> PriceForLevels { get; set; }

        public virtual StationType StationType { get; set; }
    }
}
