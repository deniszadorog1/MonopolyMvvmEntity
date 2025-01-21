namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rearity")]
    public partial class Rearity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Rearity()
        {
            BoxItems = new HashSet<BoxItem>();
        }

        public int Id { get; set; }

        [Column("Rearity")]
        [StringLength(255)]
        public string Rearity1 { get; set; }

        public int? ColorId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoxItem> BoxItems { get; set; }

        public virtual SystemColor SystemColor { get; set; }
    }
}
