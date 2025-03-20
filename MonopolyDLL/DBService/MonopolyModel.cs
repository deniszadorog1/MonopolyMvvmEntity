using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace MonopolyDLL.DBService
{
    public partial class MonopolyModel : DbContext
    {
        public MonopolyModel()
            : base("name=MonopolyModel")
        {
        }

        public virtual DbSet<Board> Board { get; set; }
        public virtual DbSet<BoxItems> BoxItems { get; set; }
        public virtual DbSet<BusinessToGive> BusinessToGive { get; set; }
        public virtual DbSet<Casino> Casino { get; set; }
        public virtual DbSet<CasinoTypes> CasinoTypes { get; set; }
        public virtual DbSet<Cell> Cell { get; set; }
        public virtual DbSet<CellType> CellType { get; set; }
        public virtual DbSet<Chance> Chance { get; set; }
        public virtual DbSet<ChanceMoney> ChanceMoney { get; set; }
        public virtual DbSet<ChanceMove> ChanceMove { get; set; }
        public virtual DbSet<ChanceType> ChanceType { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<InventoryStaff> InventoryStaff { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<LotBox> LotBox { get; set; }
        public virtual DbSet<PictureFile> PictureFile { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<PlayerGame> PlayerGame { get; set; }
        public virtual DbSet<PriceForLevel> PriceForLevel { get; set; }
        public virtual DbSet<PriceMultiplier> PriceMultiplier { get; set; }
        public virtual DbSet<PriceType> PriceType { get; set; }
        public virtual DbSet<Rearity> Rarity { get; set; }
        public virtual DbSet<Station> Station { get; set; }
        public virtual DbSet<StationType> StationType { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<SystemColors> SystemColors { get; set; }
        public virtual DbSet<Tax> Tax { get; set; }
        public virtual DbSet<TaxType> TaxType { get; set; }
        public virtual DbSet<Trade> Trade { get; set; }
        public virtual DbSet<TradeAttribs> TradeAttribs { get; set; }
        public virtual DbSet<TradeStatus> TradeStatus { get; set; }
        public virtual DbSet<WonLevels> WonLevels { get; set; }
        public virtual DbSet<WonPlayer> WonPlayer { get; set; }
        public virtual DbSet<WonTablesInCasino> WonTablesInCasino { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxItems>()
                .HasMany(e => e.Items)
                .WithOptional(e => e.BoxItems)
                .HasForeignKey(e => e.ItemId);

            modelBuilder.Entity<CasinoTypes>()
                .HasMany(e => e.WonTablesInCasino)
                .WithOptional(e => e.CasinoTypes)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<CellType>()
                .HasMany(e => e.Cell)
                .WithOptional(e => e.CellType1)
                .HasForeignKey(e => e.CellType);

            modelBuilder.Entity<Items>()
                .HasMany(e => e.InventoryStaff)
                .WithOptional(e => e.Items)
                .HasForeignKey(e => e.StaffId);

            modelBuilder.Entity<LotBox>()
                .HasMany(e => e.BoxItems)
                .WithOptional(e => e.LotBox)
                .HasForeignKey(e => e.BoxId);

            modelBuilder.Entity<LotBox>()
                .HasMany(e => e.Items)
                .WithOptional(e => e.LotBox)
                .HasForeignKey(e => e.BoxId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.BoxItems)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PicId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.Cell)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PicId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.LotBox)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PicId);

            modelBuilder.Entity<PictureFile>()
                .HasMany(e => e.Player)
                .WithOptional(e => e.PictureFile)
                .HasForeignKey(e => e.PictureId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.Trade)
                .WithOptional(e => e.PlayerGame)
                .HasForeignKey(e => e.ReciverId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.Trade1)
                .WithOptional(e => e.PlayerGame1)
                .HasForeignKey(e => e.SenderId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.TradeAttribs)
                .WithOptional(e => e.PlayerGame)
                .HasForeignKey(e => e.PlayerId);

            modelBuilder.Entity<PlayerGame>()
                .HasMany(e => e.WonPlayer)
                .WithOptional(e => e.PlayerGame)
                .HasForeignKey(e => e.PlayerId);

            modelBuilder.Entity<PriceMultiplier>()
                .HasMany(e => e.BoxItems)
                .WithOptional(e => e.PriceMultiplier)
                .HasForeignKey(e => e.MultiplierId);

            modelBuilder.Entity<StationType>()
                .HasMany(e => e.Station)
                .WithOptional(e => e.StationType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<SystemColors>()
                .HasMany(e => e.Rearity)
                .WithOptional(e => e.SystemColors)
                .HasForeignKey(e => e.ColorId);

            modelBuilder.Entity<TaxType>()
                .HasMany(e => e.Tax)
                .WithOptional(e => e.TaxType)
                .HasForeignKey(e => e.TypeId);

            modelBuilder.Entity<TradeAttribs>()
                .HasMany(e => e.BusinessToGive)
                .WithOptional(e => e.TradeAttribs)
                .HasForeignKey(e => e.TradeAttribId);

            modelBuilder.Entity<TradeStatus>()
                .HasMany(e => e.Trade)
                .WithOptional(e => e.TradeStatus)
                .HasForeignKey(e => e.StatusId);

            modelBuilder.Entity<WonLevels>()
                .HasMany(e => e.WonTablesInCasino)
                .WithOptional(e => e.WonLevels)
                .HasForeignKey(e => e.WonLevelId);
        }
    }
}
