namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Items
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Items()
        {
            InventoryStaff = new HashSet<InventoryStaff>();
        }

        public int Id { get; set; }

        public bool? IsBox { get; set; }

        public int? ItemId { get; set; }

        public int? BoxId { get; set; }

        public virtual BoxItems BoxItems { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventoryStaff> InventoryStaff { get; set; }

        public virtual LotBox LotBox { get; set; }
    }
}
