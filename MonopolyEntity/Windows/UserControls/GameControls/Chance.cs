namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Chance")]
    public partial class Chance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chance()
        {
            ChanceMoneys = new HashSet<ChanceMoney>();
            ChanceMoves = new HashSet<ChanceMove>();
        }

        public int Id { get; set; }

        public int? CellId { get; set; }

        public int? TypeActionId { get; set; }

        public int? TypeId { get; set; }

        public virtual Cell Cell { get; set; }

        public virtual ChanceType ChanceType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChanceMoney> ChanceMoneys { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChanceMove> ChanceMoves { get; set; }
    }
}
