namespace MonopolyEntity.Windows.UserControls.GameControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BoxItem
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? BusLevel { get; set; }

        public int? BoxId { get; set; }

        public int? RearityId { get; set; }

        public int? StationId { get; set; }

        public int? PicId { get; set; }

        public int? StationTypeId { get; set; }

        public int? ItemId { get; set; }

        public virtual LotBox LotBox { get; set; }

        public virtual Item Item { get; set; }

        public virtual PictureFile PictureFile { get; set; }

        public virtual Rearity Rearity { get; set; }

        public virtual Station Station { get; set; }

        public virtual StationType StationType { get; set; }
    }
}
