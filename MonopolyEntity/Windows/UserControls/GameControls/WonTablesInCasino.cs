namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WonTablesInCasino")]
    public partial class WonTablesInCasino
    {
        public int Id { get; set; }

        public int? CasinoId { get; set; }

        public int? TypeId { get; set; }

        public int? WonLevelId { get; set; }

        public virtual Casino Casino { get; set; }

        public virtual CasinoType CasinoType { get; set; }

        public virtual WonLevel WonLevel { get; set; }
    }
}
