namespace MonopolyDLL.DBService
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Cell")]
    public partial class Cell
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cell()
        {
            Casinos = new HashSet<Casino>();
            Chances = new HashSet<Chance>();
            Stations = new HashSet<Station>();
            Taxes = new HashSet<Tax>();
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? BoardId { get; set; }

        public int? CellType { get; set; }

        public int? PicId { get; set; }

        public virtual Board Board { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Casino> Casinos { get; set; }

        public virtual CellType CellType1 { get; set; }

        public virtual PictureFile PictureFile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chance> Chances { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Station> Stations { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tax> Taxes { get; set; }
    }
}
