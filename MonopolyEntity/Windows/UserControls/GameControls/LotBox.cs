namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LotBox")]
    public partial class LotBox
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LotBox()
        {
            BoxItems = new HashSet<BoxItem>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? PicId { get; set; }

        public int? ItemId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoxItem> BoxItems { get; set; }

        public virtual Item Item { get; set; }

        public virtual PictureFile PictureFile { get; set; }
    }
}
