namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BoxItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BoxItem()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? BoxId { get; set; }

        public int? RearityId { get; set; }

        public int? PicId { get; set; }

        public int? StationTypeId { get; set; }

        public int? StationId { get; set; }

        public int? MultiplierId { get; set; }

        public virtual LotBox LotBox { get; set; }

        public virtual PriceMultiplier PriceMultiplier { get; set; }

        public virtual PictureFile PictureFile { get; set; }

        public virtual Rearity Rearity { get; set; }

        public virtual StationType StationType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }
    }
}
