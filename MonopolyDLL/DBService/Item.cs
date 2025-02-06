namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            InventoryStaffs = new HashSet<InventoryStaff>();
        }

        public int Id { get; set; }

        public bool? IsBox { get; set; }

        public int? ItemId { get; set; }

        public int? BoxId { get; set; }

        public virtual BoxItem BoxItem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InventoryStaff> InventoryStaffs { get; set; }

        public virtual LotBox LotBox { get; set; }
    }
}
