namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Casino")]
    public partial class Casino
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Casino()
        {
            WonTablesInCasinoes = new HashSet<WonTablesInCasino>();
        }

        public int Id { get; set; }

        public int? CellId { get; set; }

        public int? PriceToPlay { get; set; }

        public virtual Cell Cell { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WonTablesInCasino> WonTablesInCasinoes { get; set; }
    }
}
